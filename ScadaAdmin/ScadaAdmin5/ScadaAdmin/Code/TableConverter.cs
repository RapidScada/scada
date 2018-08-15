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
 * Summary  :  Provides data exchange between instances of BaseTable and DataTable
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.Project;
using System;
using System.ComponentModel;
using System.Data;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Provides data exchange between instances of BaseTable and DataTable.
    /// <para>Обеспечивает обмен данными между экземплярами BaseTable и DataTable.</para>
    /// </summary>
    internal static class TableConverter
    {
        /// <summary>
        /// Creates a new item getting values from the row.
        /// </summary>
        private static T CreateItem<T>(DataRow row, PropertyDescriptorCollection props)
        {
            T item = Activator.CreateInstance<T>();

            foreach (PropertyDescriptor prop in props)
            {
                object value = row[prop.Name];
                if (value != DBNull.Value)
                    prop.SetValue(item, value);
            }

            return item;
        }

        /// <summary>
        /// Converts the list to a data table.
        /// </summary>
        public static DataTable ToDataTable<T>(this BaseTable<T> baseTable, bool allowNull = false)
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
            dataTable.BeginLoadData();

            try
            {
                foreach (T item in baseTable.Items.Values)
                {
                    for (int i = 0; i < propCnt; i++)
                    {
                        values[i] = props[i].GetValue(item) ?? DBNull.Value;
                    }

                    dataTable.Rows.Add(values);
                }
            }
            finally
            {
                dataTable.EndLoadData();
                dataTable.AcceptChanges();
            }

            return dataTable;
        }

        /// <summary>
        /// Copies the changes from the table to the list.
        /// </summary>
        public static void RetrieveChanges<T>(this BaseTable<T> baseTable, DataTable dataTable)
        {
            // delete rows from the target table
            DataRow[] deletedRows = dataTable.Select("", "", DataViewRowState.Deleted);

            foreach (DataRow row in deletedRows)
            {
                int key = (int)row[baseTable.PrimaryKey];
                baseTable.Items.Remove(key);
                row.AcceptChanges();
            }

            // change rows in the target table
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataRow[] modifiedRows = dataTable.Select("", "", DataViewRowState.ModifiedCurrent);

            foreach (DataRow row in modifiedRows)
            {
                T item = CreateItem<T>(row, props);
                int origKey = (int)row[baseTable.PrimaryKey, DataRowVersion.Original];
                int curKey = (int)row[baseTable.PrimaryKey, DataRowVersion.Current];

                if (origKey == curKey)
                {
                    baseTable.Set(item);
                }
                else
                {
                    baseTable.Items.Remove(origKey);
                    baseTable.Add(item);
                }

                row.AcceptChanges();
            }

            // add rows to the target table
            DataRow[] addedRows = dataTable.Select("", "", DataViewRowState.Added);

            foreach (DataRow row in addedRows)
            {
                T item = CreateItem<T>(row, props);
                baseTable.Add(item);
                row.AcceptChanges();
            }
        }
    }
}
