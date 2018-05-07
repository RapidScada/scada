using Scada;
using ScadaAdmin.AgentSvcRef;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ScadaAdmin.Remote
{
    public partial class FrmDownloadConfig : Form
    {
        public FrmDownloadConfig()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Создать вектор инициализации на освнове ид. сессии
        /// </summary>
        private static byte[] CreateIV(long sessionID)
        {
            byte[] iv = new byte[ScadaUtils.IVSize];
            byte[] sessBuf = BitConverter.GetBytes(sessionID);
            int sessBufLen = sessBuf.Length;

            for (int i = 0; i < ScadaUtils.IVSize; i++)
            {
                iv[i] = sessBuf[i % sessBufLen];
            }

            return iv;
        }

        /// <summary>
        /// Зашифровать пароль
        /// </summary>
        private static string EncryptPassword(string password, long sessionID, byte[] secretKey)
        {
            return ScadaUtils.Encrypt(password, secretKey, CreateIV(sessionID));
        }


        private void FrmDownloadConfig_Load(object sender, EventArgs e)
        {

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            AgentSvcClient client = new AgentSvcClient();

            try
            {
                client.CreateSession(out long sessionID);
                MessageBox.Show("Session ID = " + sessionID);

                byte[] secretKey = ScadaUtils.HexToBytes("5ABF5A7FD01752A2F1DFD21370B96EA462B0AE5C66A64F8901C9E1E2A06E40F1");
                string encryptedPassword = EncryptPassword("12345", sessionID, secretKey);

                if (client.Login(out string errMsg, sessionID, "admin", encryptedPassword, "Default"))
                    MessageBox.Show("Login OK");
                else
                    MessageBox.Show(errMsg);

                Stream stream = client.DownloadConfig(sessionID, 
                    new ConfigOptions() { ConfigParts = ConfigParts.All });

                if (stream == null)
                {
                    MessageBox.Show("Stream is null.");
                }
                else
                {
                    DateTime t0 = DateTime.UtcNow;
                    byte[] buf = new byte[1024];
                    Stream saver = File.Create(@"C:\SCADA\config.zip");
                    int cnt;

                    while ((cnt = stream.Read(buf, 0, buf.Length)) > 0)
                    {
                        saver.Write(buf, 0, cnt);
                    }

                    saver.Close();
                    MessageBox.Show("Done in " + (int)(DateTime.UtcNow - t0).TotalMilliseconds + " ms");
                }
            }
            finally
            {
                client.Close();
            }
        }
    }
}
