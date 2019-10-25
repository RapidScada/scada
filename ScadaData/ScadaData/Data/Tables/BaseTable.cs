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
 * Summary  : Represents the table of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents the table of the configuration database.
    /// <para>Представляет таблицу базы конфигурации.</para>
    /// </summary>
    public class BaseTable<T> : IBaseTable
    {
        /// <summary>
        /// The primary key of the table.
        /// </summary>
        protected string primaryKey;
        /// <summary>
        /// The property that is a primary key.
        /// </summary>
        protected PropertyDescriptor primaryKeyProp;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseTable(string name, string primaryKey, string title)
        {
            Name = name;
            PrimaryKey = primaryKey;
            Title = title;
            Items = new SortedDictionary<int, T>();
            Indexes = new Dictionary<string, TableIndex>();
            DependsOn = new List<TableRelation>();
            Dependent = new List<TableRelation>();
            Modified = false;
            IndexesEnabled = true;
        }


        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the primary key of the table.
        /// </summary>
        public string PrimaryKey
        {
            get
            {
                return primaryKey;
            }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("The primary key can not be empty.");

                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(T))[value];

                if (prop == null)
                    throw new ArgumentException("The primary key property not found.");

                if (prop.PropertyType != typeof(int))
                    throw new ArgumentException("The primary key must be an integer.");

                primaryKey = value;
                primaryKeyProp = prop;
            }
        }

        /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the short file name of the table.
        /// </summary>
        public string FileName
        {
            get
            {
                return Name + ".xml";
            }
        }

        /// <summary>
        /// Gets the type of the table items.
        /// </summary>
        public Type ItemType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Gets the number of table items.
        /// </summary>
        public int ItemCount
        {
            get
            {
                return Items.Count;
            }
        }

        /// <summary>
        /// Gets the table items sorted by primary key.
        /// </summary>
        /// <remarks>Do not change items directly to avoid data corruption.</remarks>
        public SortedDictionary<int, T> Items { get; protected set; }

        /// <summary>
        /// Gets the table indexes accessed by column name.
        /// </summary>
        public Dictionary<string, TableIndex> Indexes { get; protected set; }

        /// <summary>
        /// Gets the tables that this table depends on (foreign keys).
        /// </summary>
        public List<TableRelation> DependsOn { get; protected set; }

        /// <summary>
        /// Gets the tables that depend on this table.
        /// </summary>
        public List<TableRelation> Dependent { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table was modified.
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all indexes of the table are maintained up to date.
        /// </summary>
        public bool IndexesEnabled { get; set; }


        /// <summary>
        /// Adds references to the item to the table indexes.
        /// </summary>
        private void AddToIndexes(T item, int itemKey)
        {
            if (IndexesEnabled)
            {
                foreach (TableIndex index in Indexes.Values)
                {
                    if (index.IsReady)
                        index.AddToIndex(item, itemKey);
                }
            }
        }

        /// <summary>
        /// Removes references to the item from the table indexes.
        /// </summary>
        private void RemoveFromIndexes(int key)
        {
            if (IndexesEnabled && Items.TryGetValue(key, out T item))
            {
                foreach (TableIndex index in Indexes.Values)
                {
                    if (index.IsReady)
                        index.RemoveFromIndex(item, key);
                }
            }
        }


        /// <summary>
        /// Adds or updates an item in the table.
        /// </summary>
        public void AddItem(T item)
        {
            int itemKey = GetPkValue(item);
            RemoveFromIndexes(itemKey);
            Items[itemKey] = item;
            AddToIndexes(item, itemKey);
        }

        /// <summary>
        /// Adds or updates an item in the table.
        /// </summary>
        public void AddObject(object obj)
        {
            if (obj is T item)
                AddItem(item);
        }

        /// <summary>
        /// Removes an item with the specified primary key.
        /// </summary>
        public void RemoveItem(int key)
        {
            RemoveFromIndexes(key);
            Items.Remove(key);
        }

        /// <summary>
        /// Removes all items.
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();

            foreach (TableIndex index in Indexes.Values)
            {
                index.Reset();
            }
        }

        /// <summary>
        /// Gets the primary key value of the item.
        /// </summary>
        public int GetPkValue(object item)
        {
            return (int)primaryKeyProp.GetValue(item);
        }

        /// <summary>
        /// Sets the primary key value of the item.
        /// </summary>
        public void SetPkValue(object item, int key)
        {
            primaryKeyProp.SetValue(item, key);
        }

        /// <summary>
        /// Checks if there is an item with the specified primary key.
        /// </summary>
        public bool PkExists(int key)
        {
            return Items.ContainsKey(key);
        }

        /// <summary>
        /// Adds a new index.
        /// </summary>
        public TableIndex AddIndex(string columnName)
        {
            TableIndex index = new TableIndex(columnName, ItemType);
            Indexes.Add(columnName, index);
            return index;
        }

        /// <summary>
        /// Gets an index by the column name, populating it if necessary.
        /// </summary>
        public bool TryGetIndex(string columnName, out TableIndex index)
        {
            if (Indexes.TryGetValue(columnName, out index))
            {
                if (!index.IsReady)
                {
                    index.IsReady = true;
                    foreach (KeyValuePair<int, T> pair in Items)
                    {
                        index.AddToIndex(pair.Value, pair.Key);
                    }
                }

                return true;
            }
            else
            {
                index = null;
                return false;
            }
        }

        /// <summary>
        /// Returns an enumerable collection of the table primary keys.
        /// </summary>
        public IEnumerable<int> EnumerateKeys()
        {
            foreach (int key in Items.Keys)
            {
                yield return key;
            }
        }

        /// <summary>
        /// Returns an enumerable collection of the table items.
        /// </summary>
        public IEnumerable EnumerateItems()
        {
            foreach (T item in Items.Values)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Selects the items that match the specified filter.
        /// </summary>
        public IEnumerable SelectItems(TableFilter tableFilter, bool indexRequired = false)
        {
            if (tableFilter == null)
                throw new ArgumentNullException("tableFilter");

            // find the property used by the filter
            PropertyDescriptor filterProp = TypeDescriptor.GetProperties(ItemType)[tableFilter.ColumnName];
            if (filterProp == null)
                throw new ArgumentException("The filter property not found.");

            // get the matched items
            if (TryGetIndex(tableFilter.ColumnName, out TableIndex index))
            {
                int indexKey = tableFilter.Value == null ? 0 : (int)tableFilter.Value;

                foreach (object item in index.SelectItems(indexKey))
                {
                    yield return item;
                }
            }
            else if (indexRequired)
            {
                throw new ScadaException("Index not found.");
            }
            else
            {
                object filterVal = tableFilter.Value;

                foreach (T item in Items.Values)
                {
                    object val = filterProp.GetValue(item);
                    if (Equals(val, filterVal))
                        yield return item;
                }
            }
        }

        /// <summary>
        /// Loads the table from the specified file.
        /// </summary>
        public void Load(string fileName)
        {
            Items.Clear();
            Modified = false;

            List<T> list;
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            using (XmlReader reader = XmlReader.Create(fileName))
            {
                list = (List<T>)serializer.Deserialize(reader);
            }

            foreach (T item in list)
            {
                Items.Add(GetPkValue(item), item);
            }
        }

        /// <summary>
        /// Saves the table to the specified file.
        /// </summary>
        public void Save(string fileName)
        {
            List<T> list = new List<T>(Items.Values);
            XmlSerializer serializer = new XmlSerializer(list.GetType());
            XmlWriterSettings writerSettings = new XmlWriterSettings() { Indent = true };

            using (XmlWriter writer = XmlWriter.Create(fileName, writerSettings))
            {
                serializer.Serialize(writer, list);
            }

            Modified = false;
        }
    }
}
