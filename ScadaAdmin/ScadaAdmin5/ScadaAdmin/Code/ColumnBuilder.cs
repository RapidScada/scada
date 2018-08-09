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

        public DataGridViewColumn[] CreateColumns<T>(BaseTable<T> baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException("table");

            if (typeof(T) == typeof(Obj))
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
