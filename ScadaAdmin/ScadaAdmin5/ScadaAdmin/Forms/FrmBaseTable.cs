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
    public partial class FrmBaseTable : Form, IChildForm
    {
        protected BaseTable table;


        public FrmBaseTable()
        {
            InitializeComponent();
        }

        public FrmBaseTable(BaseTable table)
            : this()
        {
            this.table = table ?? throw new ArgumentNullException("table");
        }


        public ChildFormTag ChildFormTag { get; set; }


        public void Save()
        {
            DataTable testTable = new DataTable("Test");
            testTable.Columns.Add("Col1", typeof(string));
            testTable.Columns.Add("Col2", typeof(string));
            for (int i = 0; i < 10000; i++)
            {
                DataRow row = testTable.NewRow();
                row[0] = "test";
                row[1] = "aaa";
                testTable.Rows.Add(row);
            }

            DateTime t0 = DateTime.UtcNow;
            string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Log", table.Name + ".xml");

            using (XmlWriter writer = XmlWriter.Create(fileName, new XmlWriterSettings() { Indent = true }))
            {
                testTable.WriteXml(writer);
            }
            //table.Save(fileName);
            MessageBox.Show((DateTime.UtcNow - t0).TotalSeconds.ToString() + " sec");
        }


        private void FrmBaseTable_Load(object sender, EventArgs e)
        {
            bindingSource.DataSource = table.Rows;
            ColumnBuilder columnBuilder = new ColumnBuilder();
            dataGridView.Columns.AddRange(columnBuilder.CreateColumns(table));
            ScadaUiUtils.AutoResizeColumns(dataGridView);
        }
    }
}
