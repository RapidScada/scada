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

                if (client.Login(out string errMsg, sessionID, "admin", "", "Default"))
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
