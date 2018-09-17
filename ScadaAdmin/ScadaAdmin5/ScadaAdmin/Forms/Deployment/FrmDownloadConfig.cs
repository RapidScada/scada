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
    public partial class FrmDownloadConfig : Form
    {
        private FrmDownloadConfig()
        {
            InitializeComponent();
        }

        public FrmDownloadConfig(ScadaProject project, Instance instance)
            : this()
        {

        }


        public bool BaseModified { get; protected set; }

        public bool InterfaceModified { get; protected set; }

        public bool InstanceModified { get; protected set; }
    }
}
