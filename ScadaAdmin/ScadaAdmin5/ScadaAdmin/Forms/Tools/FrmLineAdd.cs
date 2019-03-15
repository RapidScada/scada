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
    public partial class FrmLineAdd : Form
    {
        public FrmLineAdd()
        {
            InitializeComponent();

            txtName.MaxLength = ColumnLength.Name;
            txtDescr.MaxLength = ColumnLength.Description;
        }

        private void FrmLineAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
