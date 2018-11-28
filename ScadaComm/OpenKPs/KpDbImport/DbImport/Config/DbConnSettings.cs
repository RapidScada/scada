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
 * Summary  : DB connection settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

namespace Scada.Comm.Devices.DbImport.Config
{
    /// <summary>
    /// DB connection settings.
    /// <para>Настройки соединения с БД.</para>
    /// </summary>
    internal class DbConnSettings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DbConnSettings()
        {
            Server = "";
            Database = "";
            User = "";
            Password = "";
            ConnectionString = "";
        }


        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the database username.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the database user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
