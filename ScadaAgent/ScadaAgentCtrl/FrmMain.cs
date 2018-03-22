using Scada.Agent.Ctrl.ServiceReference1;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Agent.Ctrl
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            AgentSvcClient client = new AgentSvcClient();

            try
            {
                double sum = client.Sum(2, 2);
                MessageBox.Show(sum.ToString());

                Stream stream = client.DownloadFile(0, AppPath.Base);
                if (stream == null)
                {
                    MessageBox.Show("Stream is null.");
                }
                else
                {
                    DateTime t0 = DateTime.UtcNow;
                    byte[] buf = new byte[1024];
                    Stream saver = File.Create(@"D:\file1.txt");
                    int cnt;

                    while ((cnt = stream.Read(buf, 0, buf.Length)) > 0)
                    {
                        saver.Write(buf, 0, cnt);
                    }

                    saver.Close();
                    MessageBox.Show("Done in " + (int)(DateTime.UtcNow - t0).TotalMilliseconds + " ms");

                    /*int cnt = stream.Read(buf, 0, buf.Length);
                    string s = System.Text.Encoding.ASCII.GetString(buf, 0, cnt);
                    MessageBox.Show(s);*/
                }
            }
            finally
            {
                client.Close();
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            AgentSvcClient client = new AgentSvcClient();
            Stream stream = null;

            try
            {
                ConfigOptions configOptions = new ConfigOptions();

                /*byte[] buffer = System.Text.Encoding.ASCII.GetBytes("I'm Muzzy");
                stream = new MemoryStream(buffer.Length);
                stream.Write(buffer, 0, buffer.Length);
                stream.Position = 0;*/

                stream = File.Open(@"D:\big.txt", FileMode.Open);
                client.UploadConfig(configOptions, 0, stream);
                MessageBox.Show("Done");
            }
            finally
            {
                stream?.Close();
                client.Close();
            }
        }
    }
}
