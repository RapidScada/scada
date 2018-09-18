using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Admin.Deployment;
using Scada.Admin.Project;

namespace Scada.Admin.App.Controls.Deployment
{
    public partial class CtrlTransferSettings : UserControl
    {
        public CtrlTransferSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        public void SettingsToControls(TransferSettings transferSettings)
        {

        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        public void ControlsToSettings(TransferSettings transferSettings)
        {

        }

        /// <summary>
        /// Clears and disables the control.
        /// </summary>
        public void Disable()
        {

        }
    }
}
