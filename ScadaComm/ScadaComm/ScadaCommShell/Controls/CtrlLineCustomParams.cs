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
    public partial class CtrlLineCustomParams : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineCustomParams()
        {
            InitializeComponent();
            SetColumnNames();
        }


        /// <summary>
        /// Sets the column names for the translation.
        /// </summary>
        private void SetColumnNames()
        {
            colParamName.Name = "colParamName";
            colParamValue.Name = "colParamValue";
        }
    }
}
