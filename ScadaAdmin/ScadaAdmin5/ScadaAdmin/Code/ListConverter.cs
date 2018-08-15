/*
 * Copyright 2018 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaAdminCommon
 * Summary  : Provides data exchange between a list and a table.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Provides data exchange between a list and a table.
    /// <para>Обеспечивает обмен данными между списком и таблицей.</para>
    /// </summary>
    public static class ListConverter
    {
        /// <summary>
        /// Converts the list to a data table.
        /// </summary>
        public static DataTable ToDataTable<T>(this IList<T> list, bool allowNull = false)
        {
            // create columns
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            foreach (PropertyDescriptor prop in props)
            {
                bool isNullable = prop.PropertyType.IsNullable();
                bool isClass = prop.PropertyType.IsClass;

                dataTable.Columns.Add(new DataColumn()
                {
                    ColumnName = prop.Name,
                    DataType = isNullable ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType,
                    AllowDBNull = isNullable || isClass || allowNull
                });
            }

            // copy data
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

        /// <summary>
        /// Copies the changes from the table to the list.
        /// </summary>
        public static void RetrieveChanges<T>(this IList<T> list, DataTable dataTable)
        {
            DataView addedRowsView = new DataView(dataTable, "", "", DataViewRowState.Added);
            DataView modifiedRowsView = new DataView(dataTable, "", "", DataViewRowState.ModifiedCurrent);
            DataView deletedRowsView = new DataView(dataTable, "", "", DataViewRowState.Deleted);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            foreach (DataRowView rowView in addedRowsView)
            {
                T item = Activator.CreateInstance<T>();

                foreach (PropertyDescriptor prop in props)
                {
                    object value = rowView[prop.Name];
                    prop.SetValue(item, value);
                }

                list.Add(item);
            }
        }
    }
}
