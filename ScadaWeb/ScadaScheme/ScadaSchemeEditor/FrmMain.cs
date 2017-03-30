using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;

namespace Scada.Scheme.Editor
{
    public partial class FrmMain : Form
    {
        private ServiceHost schemeEditorSvcHost;


        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Process.Start("chrome");
            Process.Start("file:///D:/Misha/My%20progs/SCADA/Source/scada/ScadaWeb/ScadaScheme/ScadaSchemeEditor/bin/Debug/Web/page.html");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                schemeEditorSvcHost = new ServiceHost(typeof(SchemeEditorSvc));
                ServiceBehaviorAttribute behavior =
                    schemeEditorSvcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.UseSynchronizationContext = false;
                schemeEditorSvcHost.Open();

                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (schemeEditorSvcHost != null)
            {
                try { schemeEditorSvcHost.Close(); }
                catch { schemeEditorSvcHost.Abort(); }
                MessageBox.Show("Closed");
            }
        }
    }
}
