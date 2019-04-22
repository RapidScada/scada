/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : Provides data exchange between instances of BaseTable and DataTable
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Provides data exchange between instances of BaseTable and DataTable.
    /// <para>Обеспечивает обмен данными между экземплярами BaseTable и DataTable.</para>
    /// </summary>
    public static class TableConverter
    {
        /// <summary>
        /// Creates a new item getting values from the row.
        /// </summary>
        public static object CreateItem(Type itemType, DataRow row, PropertyDescriptorCollection props)
        {
            object item = Activator.CreateInstance(itemType);

            foreach (PropertyDescriptor prop in props)
            {
                try
                {
                    object value = row[prop.Name];
                    if (value != DBNull.Value)
                        prop.SetValue(item, value);
                }
                catch (ArgumentException)
                {
                    // column not found
                }
            }

            return item;
        }

        /// <summary>
        /// Converts the BaseTable to a DataTable.
        /// </summary>
        public static DataTable ToDataTable(this IBaseTable baseTable, bool allowNull = false)
        {
            return baseTable.EnumerateItems().ToDataTable(baseTable.ItemType, allowNull);
        }

        /// <summary>
        /// Converts the collection to a DataTable.
        /// </summary>
        public static DataTable ToDataTable(this IEnumerable items, Type itemType, bool allowNull = false)
        {
            // create columns
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(itemType);
            DataTable dataTable = new DataTable();

            foreach (PropertyDescriptor prop in props)
            {
                bool isNullable = prop.PropertyType.IsNullable();
                bool isClass = prop.PropertyType.IsClass;

                DataColumn col = new DataColumn()
                {
                    ColumnName = prop.Name,
                    DataType = isNullable ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType,
                    AllowDBNull = isNullable || isClass || allowNull
                };

                if (col.DataType == typeof(bool) && !col.AllowDBNull)
                    col.DefaultValue = false;

                dataTable.Columns.Add(col);
            }

            // copy data
            int propCnt = props.Count;
            object[] values = new object[propCnt];
            dataTable.BeginLoadData();

            try
            {
                foreach (object item in items)
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
        /// Copies the changes from the DataTable to the BaseTable.
        /// </summary>
        public static void RetrieveChanges(this IBaseTable baseTable, DataTable dataTable)
        {
            // delete rows from the target table
            DataRow[] deletedRows = dataTable.Select("", "", DataViewRowState.Deleted);

            foreach (DataRow row in deletedRows)
            {
                int key = (int)row[baseTable.PrimaryKey, DataRowVersion.Original];
                baseTable.RemoveItem(key);
                row.AcceptChanges();
            }

            // change rows in the target table
            Type itemType = baseTable.ItemType;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(itemType);
            DataRow[] modifiedRows = dataTable.Select("", "", DataViewRowState.ModifiedCurrent);

            foreach (DataRow row in modifiedRows)
            {
                object item = CreateItem(itemType, row, props);
                int origKey = (int)row[baseTable.PrimaryKey, DataRowVersion.Original];
                int curKey = (int)row[baseTable.PrimaryKey, DataRowVersion.Current];

                if (origKey != curKey)
                    baseTable.RemoveItem(origKey);

                baseTable.AddObject(item);
                row.AcceptChanges();
            }

            // add rows to the target table
            DataRow[] addedRows = dataTable.Select("", "", DataViewRowState.Added);

            foreach (DataRow row in addedRows)
            {
                object item = CreateItem(itemType, row, props);
                baseTable.AddObject(item);
                row.AcceptChanges();
            }
        }
    }
}
