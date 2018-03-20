using Scada.Agent.Ctrl.ServiceReference1;
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
            double sum = client.Sum(2, 2);
            client.Close();

            MessageBox.Show(sum.ToString());
        }
    }
}
