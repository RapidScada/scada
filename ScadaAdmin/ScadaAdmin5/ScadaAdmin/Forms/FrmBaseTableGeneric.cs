using Scada.Admin.App.Code;
using Scada.Admin.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.App.Forms
{
    public class FrmBaseTableGeneric<T> : FrmBaseTable, IChildForm
    {
        private readonly BaseTable<T> baseTable; // the table being edited
        private readonly ScadaProject project;   // the project under development


        public FrmBaseTableGeneric(BaseTable<T> baseTable, ScadaProject project)
            : base()
        {
            this.baseTable = baseTable ?? throw new ArgumentNullException("baseTable");
            this.project = project ?? throw new ArgumentNullException("project");
        }


        protected override void MyLoad()
        {
            base.MyLoad();
            Text = baseTable.Title;
            bindingSource.DataSource = baseTable.Items.ToDataTable();
            ColumnBuilder columnBuilder = new ColumnBuilder(project.ConfigBase);
            dataGridView.Columns.AddRange(columnBuilder.CreateColumns(baseTable.ItemType));
            dataGridView.AutoResizeColumns();
        }

        public void Save()
        {
            DateTime t0 = DateTime.UtcNow;

            string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Log",
                baseTable.Name + ".xml");

            baseTable.Save(fileName);

            MessageBox.Show((DateTime.UtcNow - t0).TotalSeconds.ToString() + " sec");
        }
    }
}
