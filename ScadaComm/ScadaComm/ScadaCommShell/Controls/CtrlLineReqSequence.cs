using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Shell.Controls
{
    public partial class CtrlLineReqSequence : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineReqSequence()
        {
            InitializeComponent();
            SetColumnNames();
        }


        /// <summary>
        /// Sets the column names for the translation.
        /// </summary>
        private void SetColumnNames()
        {
            colDeviceOrder.Name = "colDeviceOrder";
            colDeviceActive.Name = "colDeviceActive";
            colDeviceBound.Name = "colDeviceBound";
            colDeviceNumber.Name = "colDeviceNumber";
            colDeviceName.Name = "colDeviceName";
            colDeviceDll.Name = "colDeviceDll";
            colDeviceAddress.Name = "colDeviceAddress";
            colDeviceCallNum.Name = "colDeviceCallNum";
            colDeviceTimeout.Name = "colDeviceTimeout";
            colDeviceDelay.Name = "colDeviceDelay";
            colDeviceTime.Name = "colDeviceTime";
            colDevicePeriod.Name = "colDevicePeriod";
            colDeviceCmdLine.Name = "colDeviceCmdLine";
        }
    }
}
