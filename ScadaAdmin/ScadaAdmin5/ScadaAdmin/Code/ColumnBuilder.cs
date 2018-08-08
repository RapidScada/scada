using Scada.Admin.Project;
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    internal class ColumnBuilder
    {
        private static DataGridViewTextBoxColumn NewTextBoxColumn(string dataPropertyName)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName
            };
        }

        public DataGridViewColumn[] CreateColumns(BaseTable table)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (table.ItemType == typeof(Obj))
            {
                return new DataGridViewColumn[]
                {
                    NewTextBoxColumn("ObjNum"),
                    NewTextBoxColumn("Name"),
                    NewTextBoxColumn("Descr")
                };
            }
            else
            {
                return new DataGridViewColumn[0];
            }
        }

        public DataGridViewColumn[] CreateColumns(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException("dataTable");

            if (dataTable.TableName == "Obj")
            {
                return new DataGridViewColumn[]
                {
                    NewTextBoxColumn("ObjNum"),
                    NewTextBoxColumn("Name"),
                    NewTextBoxColumn("Descr")
                };
            }
            else
            {
                return new DataGridViewColumn[0];
            }
        }
    }
}
