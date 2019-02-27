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
using System.Collections;
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
        public TableIndex(string columnName, Type itemType)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException("Column name must not be empty.", "columnName");
            if (itemType == null)
                throw new ArgumentNullException("itemType");

            indexProp = GetIndexProp(columnName, itemType);
            ColumnName = columnName;
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
        /// Gets a value indicating whether null values are allowed in the indexed column.
        /// </summary>
        public bool AllowNull { get; protected set; }


        /// <summary>
        /// Gets the indexed property.
        /// </summary>
        private PropertyDescriptor GetIndexProp(string columnName, Type itemType)
        {
            PropertyDescriptorCollection itemProps = TypeDescriptor.GetProperties(itemType);
            PropertyDescriptor prop = itemProps[columnName];

            if (prop == null)
                throw new InvalidOperationException("The item type doesn't contain the required property.");

            AllowNull = prop.PropertyType.IsNullable();
            Type propType = AllowNull ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;

            if (propType != typeof(int))
                throw new InvalidOperationException("The property must be integer.");

            return prop;
        }

        /// <summary>
        /// Gets the value of the indexed property.
        /// </summary>
        private int GetIndexKey(object item)
        {
            object indexKey = indexProp.GetValue(item);
            return indexKey == null ? 0 : (int)indexKey;
        }


        /// <summary>
        /// Adds the item to the index.
        /// </summary>
        public void AddToIndex(object item, int itemKey)
        {
            if (item == null)
                throw new ArgumentNullException("item");

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
            if (item == null)
                throw new ArgumentNullException("item");

            int indexKey = GetIndexKey(item);

            if (ItemGroups.TryGetValue(indexKey, out SortedDictionary<int, object> group))
            {
                group.Remove(itemKey);
                if (group.Count == 0)
                    ItemGroups.Remove(indexKey);
            }
        }

        /// <summary>
        /// Checks whether the specified key is represented in the index.
        /// </summary>
        public bool IndexKeyExists(int indexKey)
        {
            return ItemGroups.ContainsKey(indexKey);
        }

        /// <summary>
        /// Selects items by the specified index key.
        /// </summary>
        public IEnumerable SelectItems(int indexKey)
        {
            if (ItemGroups.TryGetValue(indexKey, out SortedDictionary<int, object> group))
            {
                foreach (object item in group.Values)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Resets the index to its initial state.
        /// </summary>
        public void Reset()
        {
            ItemGroups.Clear();
            IsReady = false;
        }
    }
}
