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
 * Summary  : Represents the table of the configuration database with typed items
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the table of the configuration database with typed items.
    /// <para>Представляет таблицу базы конфигурации с типизированными элементами.</para>
    /// </summary>
    public class Table<T> : BaseTable
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Table()
            : this("", "")
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Table(string name, string title)
            : base(name, title)
        {
            Items = new List<T>();
        }


        /// <summary>
        /// Get the typed items of the table.
        /// </summary>
        public List<T> Items { get; protected set; }

        /// <summary>
        /// Gets the type of the table items.
        /// </summary>
        public override Type ItemType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Gets the rows.
        /// </summary>
        public override IList Rows
        {
            get
            {
                return Items as IList;
            }
        }
    }
}
