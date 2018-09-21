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
        /// Gets the short file name of the table.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets the type of the table items.
        /// </summary>
        Type ItemType { get; }


        /// <summary>
        /// Adds or updates an item in the table.
        /// </summary>
        void AddObject(object obj);

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
