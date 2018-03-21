using Scada.Agent.Ctrl.ServiceReference1;
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
                    byte[] buf = new byte[100];
                    int cnt = stream.Read(buf, 0, buf.Length);
                    string s = System.Text.Encoding.ASCII.GetString(buf, 0, cnt);
                    MessageBox.Show(s);
                }
            }
            finally
            {
                client.Close();
            }
        }
    }
}
