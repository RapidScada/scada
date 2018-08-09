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
        protected BaseTable baseTable;
        protected DataTable dataTable;


        public FrmBaseTable()
        {
            InitializeComponent();
        }

        public FrmBaseTable(BaseTable baseTable)
            : this()
        {
            this.baseTable = baseTable ?? throw new ArgumentNullException("baseTable");
        }

        public FrmBaseTable(DataTable dataTable)
            : this()
        {
            this.dataTable = dataTable ?? throw new ArgumentNullException("dataTable");
        }


        public ChildFormTag ChildFormTag { get; set; }


        public void Save()
        {
            DateTime t0 = DateTime.UtcNow;

            if (baseTable != null)
            {
                string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Log",
                    baseTable.Name + ".xml");

                baseTable.Save(fileName);
            }
            else if (dataTable != null)
            {
                string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Log",
                    dataTable.TableName + ".xml");

                using (XmlWriter writer = XmlWriter.Create(fileName, new XmlWriterSettings() { Indent = true }))
                {
                    dataTable.WriteXml(writer);
                }
            }

            MessageBox.Show((DateTime.UtcNow - t0).TotalSeconds.ToString() + " sec");
        }


        private void FrmBaseTable_Load(object sender, EventArgs e)
        {
            /*Table<Obj> myTable = new Table<Obj>();
            for (int i = 1; i <= 10000; i++)
            {
                myTable.Rows.Add(new Obj() { ObjNum = i, Name = "aa", Descr = "bb" });
            }

            BindingList<Obj> bl = new BindingList<Obj>(myTable.Items);
            bindingSource.DataSource = bl;
            ColumnBuilder columnBuilder = new ColumnBuilder();
            dataGridView.Columns.AddRange(columnBuilder.CreateColumns(myTable));*/

            if (AdminUtils.IsRunningOnMono)
            {
                // because of the bug in Mono 5.12.0.301
                dataGridView.AllowUserToAddRows = false;
            }

            if (baseTable != null)
            {
                Text = baseTable.Title;
                bindingSource.DataSource = baseTable.Rows;
                ColumnBuilder columnBuilder = new ColumnBuilder();
                dataGridView.Columns.AddRange(columnBuilder.CreateColumns(baseTable));
            }
            else if (dataTable != null)
            {
                Text = dataTable.TableName;
                bindingSource.DataSource = dataTable;
                ColumnBuilder columnBuilder = new ColumnBuilder();
                dataGridView.Columns.AddRange(columnBuilder.CreateColumns(dataTable));
            }

            ScadaUiUtils.AutoResizeColumns(dataGridView);
        }
    }
}
