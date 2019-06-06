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
 * Module   : KpDBImport
 * Summary  : Implements a data source for MySQL
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using MySql.Data.MySqlClient;
using Scada.Comm.Devices.DbImport.Configuration;
using System;
using System.Data.Common;

namespace Scada.Comm.Devices.DbImport.Data
{
    /// <summary>
    /// Implements a data source for MySQL.
    /// <para>Реализует источник данных для MySQL.</para>
    /// </summary>
    internal class MySqlDataSource : DataSource
    {
        /// <summary>
        /// The default port of the database server.
        /// </summary>
        private const int DefaultPort = 3306;


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// Creates a command.
        /// </summary>
        protected override DbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        /// <summary>
        /// Adds the command parameter containing the value.
        /// </summary>
        protected override void AddCmdParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");
            if (!(cmd is MySqlCommand))
                throw new ArgumentException("MySqlCommand is required.", "cmd");

            MySqlCommand mySqlCmd = (MySqlCommand)cmd;
            mySqlCmd.Parameters.AddWithValue(paramName, value);
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            if (Connection != null)
                MySqlConnection.ClearPool((MySqlConnection)Connection);
        }


        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public override string BuildConnectionString(DbConnSettings connSettings)
        {
            return BuildMySqlConnectionString(connSettings);
        }

        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public static string BuildMySqlConnectionString(DbConnSettings connSettings)
        {
            if (connSettings == null)
                throw new ArgumentNullException("connSettings");

            ExtractHostAndPort(connSettings.Server, DefaultPort, out string host, out int port);

            return new MySqlConnectionStringBuilder()
            {
                Server = host,
                Port = (uint)port,
                Database = connSettings.Database,
                UserID = connSettings.User,
                Password = connSettings.Password
            }
            .ToString();
        }
    }
}
