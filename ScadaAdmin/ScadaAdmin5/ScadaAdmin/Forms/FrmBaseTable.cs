using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.App.Forms
{
    public partial class FrmBaseTable : Form, IChildForm
    {
        public FrmBaseTable()
        {
            InitializeComponent();
        }


        public ChildFormTag ChildFormTag { get; set; }


        public void Save()
        {
        }


        private void FrmBaseTable_Load(object sender, EventArgs e)
        {
            Text = label1.Text = DateTime.Now.ToString();
        }
    }
}
