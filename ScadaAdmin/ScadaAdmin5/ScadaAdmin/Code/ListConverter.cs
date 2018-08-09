using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Converts a list to a data table and back
    /// </summary>
    public static class ListConverter
    {
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            foreach (PropertyDescriptor prop in props)
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }

            int propCnt = props.Count;
            object[] values = new object[propCnt];

            foreach (T item in list)
            {
                for (int i = 0; i < propCnt; i++)
                {
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static void RetrieveChanges<T>(this IList<T> list, DataTable dataTable)
        {
            DataView addedRowsView = new DataView(dataTable, "", "", DataViewRowState.Added);

            foreach (DataRowView rowView in addedRowsView)
            {

            }
        }
    }
}
