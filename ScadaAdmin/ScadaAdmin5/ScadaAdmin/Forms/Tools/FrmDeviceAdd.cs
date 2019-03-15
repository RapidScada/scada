using Scada.Admin.App.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    public partial class FrmDeviceAdd : Form
    {
        public FrmDeviceAdd()
        {
            InitializeComponent();

            txtName.MaxLength = ColumnLength.Name;
            txtCallNum.MaxLength = ColumnLength.Default;
            txtDescr.MaxLength = ColumnLength.Description;
        }

        private void FrmDeviceAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
