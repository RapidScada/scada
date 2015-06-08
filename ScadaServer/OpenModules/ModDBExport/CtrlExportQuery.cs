using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Server.Modules.DBExport
{
    public partial class CtrlExportQuery : UserControl
    {
        public CtrlExportQuery()
        {
            InitializeComponent();
        }


        public bool Export
        {
            get
            {
                return chkExport.Checked;
            }
            set
            {
                chkExport.Checked = value;
            }
        }

        public string Query
        {
            get
            {
                return txtQuery.Text;
            }
            set
            {
                txtQuery.Text = value;
            }
        }

        public string Example
        {
            get
            {
                return txtExample.Text;
            }
            set
            {
                txtExample.Text = value;
            }
        }
    }
}
