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
 * Module   : ScadaData
 * Summary  : Defines functionality to operate with the tables of the configuration database.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Defines functionality to operate with the tables of the configuration database.
    /// <para>Определяет функциональность для работы с таблицами базы данных конфигурации.</para>
    /// </summary>
    public interface IBaseTable
    {
        /// <summary>
        /// Gets the table name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the primary key of the table.
        /// </summary>
        string PrimaryKey { get; }
        
        /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets the short file name of the table.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets the type of the table items.
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// Gets the number of table items.
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        /// Gets the table indexes accessed by column name.
        /// </summary>
        Dictionary<string, TableIndex> Indexes { get; }

        /// <summary>
        /// Gets the tables that this table depends on (foreign keys).
        /// </summary>
        List<TableRelation> DependsOn { get; }

        /// <summary>
        /// Gets the tables that depend on this table.
        /// </summary>
        List<TableRelation> Dependent { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the table was modified.
        /// </summary>
        bool Modified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all indexes of the table are maintained up to date.
        /// </summary>
        bool IndexesEnabled { get; set; }


        /// <summary>
        /// Adds or updates an item in the table.
        /// </summary>
        void AddObject(object obj);

        /// <summary>
        /// Removes an item with the specified primary key.
        /// </summary>
        void RemoveItem(int key);

        /// <summary>
        /// Removes all items.
        /// </summary>
        void ClearItems();

        /// <summary>
        /// Gets the primary key value of the item.
        /// </summary>
        int GetPkValue(object item);

        /// <summary>
        /// Sets the primary key value of the item.
        /// </summary>
        void SetPkValue(object item, int key);

        /// <summary>
        /// Checks if there is an item with the specified primary key.
        /// </summary>
        bool PkExists(int key);

        /// <summary>
        /// Adds a new index.
        /// </summary>
        TableIndex AddIndex(string columnName);

        /// <summary>
        /// Gets an index by the column name, populating it if necessary.
        /// </summary>
        bool TryGetIndex(string columnName, out TableIndex index);

        /// <summary>
        /// Returns an enumerable collection of the table primary keys.
        /// </summary>
        IEnumerable<int> EnumerateKeys();

        /// <summary>
        /// Returns an enumerable collection of the table items.
        /// </summary>
        /// <returns></returns>
        IEnumerable EnumerateItems();

        /// <summary>
        /// Selects the items that match the specified filter.
        /// </summary>
        IEnumerable SelectItems(TableFilter tableFilter, bool indexRequired = false);

        /// <summary>
        /// Loads the table from the specified file.
        /// </summary>
        void Load(string fileName);

        /// <summary>
        /// Saves the table to the specified file.
        /// </summary>
        void Save(string fileName);
    }
}
