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
        private BaseTable<T> baseTable;


        public FrmBaseTableGeneric(BaseTable<T> baseTable)
            : base()
        {
            this.baseTable = baseTable ?? throw new ArgumentNullException("baseTable");
        }


        protected override void MyLoad()
        {
            base.MyLoad();
            Text = baseTable.Title + " - FrmBaseTableGeneric";
            bindingSource.DataSource = baseTable.Items.ToDataTable();
            ColumnBuilder columnBuilder = new ColumnBuilder();
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
