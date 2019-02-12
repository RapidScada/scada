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
 * Summary  : Represents an index for an instance of BaseTable
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents an index for an instance of BaseTable.
    /// <para>Представляет индекс для экземпляра BaseTable.</para>
    /// </summary>
    public class TableIndex
    {
        /// <summary>
        /// The indexed property.
        /// </summary>
        protected PropertyDescriptor indexProp;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableIndex(string columnName)
        {
            indexProp = null;
            ColumnName = columnName ?? throw new ArgumentNullException("columnName");
            ItemGroups = new SortedDictionary<int, SortedDictionary<int, object>>();
            IsReady = false;
        }


        /// <summary>
        /// Gets the name of the indexed column.
        /// </summary>
        public string ColumnName { get; protected set; }

        /// <summary>
        /// Gets the groups of table items referred by the index.
        /// </summary>
        /// <remarks>Value of ItemGroups is a dictionary of table items sorted by primary key.</remarks>
        public SortedDictionary<int, SortedDictionary<int, object>> ItemGroups { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the index is ready to use.
        /// </summary>
        public bool IsReady { get; set; }



        /// <summary>
        /// Gets the value of the indexed property.
        /// </summary>
        private int GetIndexKey(object item)
        {
            if (indexProp == null)
            {
                PropertyDescriptorCollection itemProps = TypeDescriptor.GetProperties(item);
                PropertyDescriptor prop = itemProps[ColumnName];

                if (prop == null)
                {
                    throw new InvalidOperationException("The item doesn't contain the required property.");
                }
                else if (!(prop.PropertyType == typeof(int) ||
                    prop.PropertyType.IsNullable() && Nullable.GetUnderlyingType(prop.PropertyType) == typeof(int)))
                {
                    throw new InvalidOperationException("The property must be integer.");
                }

                indexProp = prop;
            }

            object indexKey = indexProp.GetValue(item);
            return indexKey == null ? 0 : (int)indexKey;
        }

        /// <summary>
        /// Adds the item to the index.
        /// </summary>
        public void AddToIndex(object item, int itemKey)
        {
            int indexKey = GetIndexKey(item);

            if (!ItemGroups.TryGetValue(indexKey, out SortedDictionary<int, object> group))
            {
                group = new SortedDictionary<int, object>();
                ItemGroups.Add(indexKey, group);
            }

            group[itemKey] = item;
        }

        /// <summary>
        /// Removes the item from the index.
        /// </summary>
        public void RemoveFromIndex(object item, int itemKey)
        {
            int indexKey = GetIndexKey(item);

            if (ItemGroups.TryGetValue(indexKey, out SortedDictionary<int, object> group))
            {
                group.Remove(itemKey);
                if (group.Count == 0)
                    ItemGroups.Remove(indexKey);
            }
        }

        /// <summary>
        /// Resets the index to its initial state.
        /// </summary>
        public void Reset()
        {
            indexProp = null;
            ItemGroups.Clear();
            IsReady = false;
        }
    }
}
