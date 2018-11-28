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
 * Module   : KpDBImport
 * Summary  : Driver configuration.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices.DbImport.Config
{
    /// <summary>
    /// Driver configuration.
    /// <para>Конфигурация драйвера.</para>
    /// </summary>
    internal class Config
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Config()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the database type.
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// Gets the DB connection settings.
        /// </summary>
        public DbConnSettings DbConnSettings { get; private set; }

        /// <summary>
        /// Gets or sets the SQL-query to retrieve data.
        /// </summary>
        public string SelectQuery { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to calculate tag count automatically by parsing the query.
        /// </summary>
        public bool AutoTagCount { get; set; }

        /// <summary>
        /// Gets or sets the exact number of tags.
        /// </summary>
        public int TagCount { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            DbType = DbType.Undefined;
            DbConnSettings = new DbConnSettings();
            SelectQuery = "";
            AutoTagCount = true;
            TagCount = 0;
        }
    }
}
