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

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents an index for an instance of BaseTable.
    /// <para>Представляет индекс для экземпляра BaseTable.</para>
    /// </summary>
    public class TableIndex
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableIndex(string columnName)
        {
            ColumnName = columnName ?? throw new ArgumentNullException("columnName");
            ItemGroups = new SortedDictionary<int, SortedDictionary<int, object>>();
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
    }
}
