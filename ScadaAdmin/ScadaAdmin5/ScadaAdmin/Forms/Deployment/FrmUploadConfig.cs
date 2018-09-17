using Scada.Admin.Deployment;
using Scada.Admin.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    public partial class FrmUploadConfig : Form
    {
        private FrmUploadConfig()
        {
            InitializeComponent();
        }

        public FrmUploadConfig(DeploymentSettings deploymentSettings, Instance instance)
            : this()
        {

        }
    }
}
