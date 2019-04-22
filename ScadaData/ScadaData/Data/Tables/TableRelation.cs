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
 * Summary  : Represents a relationship between two tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a relationship between two tables.
    /// <para>Представляет отношение между двумя таблицами.</para>
    /// </summary>
    public class TableRelation
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableRelation(IBaseTable parentTable, IBaseTable childTable, string childColumn)
        {
            ParentTable = parentTable ?? throw new ArgumentNullException("parentTable");
            ChildTable = childTable ?? throw new ArgumentNullException("childTable");
            ChildColumn = childColumn ?? throw new ArgumentNullException("childColumn");
        }


        /// <summary>
        /// Gets the parent table.
        /// </summary>
        public IBaseTable ParentTable { get; protected set; }

        /// <summary>
        /// Gets the child table.
        /// </summary>
        public IBaseTable ChildTable { get; protected set; }

        /// <summary>
        /// Gets the column name of the child table.
        /// </summary>
        /// <remarks>Whereas the column of the parent table is its primary key.</remarks>
        public string ChildColumn { get; protected set; }
    }
}
