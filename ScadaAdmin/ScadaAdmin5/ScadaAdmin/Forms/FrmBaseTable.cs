using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Data.Models;
using Scada.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using WinControl;

namespace Scada.Admin.App.Forms
{
    public partial class FrmBaseTable : Form
    {
        protected DataTable dataTable;


        public FrmBaseTable()
        {
            InitializeComponent();
        }


        public ChildFormTag ChildFormTag { get; set; }



        protected virtual void MyLoad()
        {
            if (AdminUtils.IsRunningOnMono)
            {
                // because of the bug in Mono 5.12.0.301
                dataGridView.AllowUserToAddRows = false;
            }
        }


        private void FrmBaseTable_Load(object sender, EventArgs e)
        {
            MyLoad();
        }
    }
}
