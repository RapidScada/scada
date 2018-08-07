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
 * Summary  : Represents the table of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System.Collections.Generic;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the table of the configuration database.
    /// <para>Представляет таблицу базы конфигурации.</para>
    /// </summary>
    public class BaseTable<T>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseTable()
            : this("", "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseTable(string name, string title)
        {
            Name = name ?? "";
            Title = title ?? "";
            Rows = new List<T>();
        }


        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets rows.
        /// </summary>
        public List<T> Rows { get; protected set; }
    }
}
