/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ScadaData.Db
 * Summary  : Implements a data source for MySQL
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using MySql.Data.MySqlClient;
using Scada.Config;
using System;
using System.Data.Common;

namespace Scada.Db
{
    /// <summary>
    /// Implements a data source for MySQL.
    /// <para>Реализует источник данных для MySQL.</para>
    /// </summary>
    public class MySqlDataSource : DataSource
    {
        /// <summary>
        /// The default port of the database server.
        /// </summary>
        private const int DefaultPort = 3306;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MySqlDataSource(DbConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// Adds the command parameter containing the value.
        /// </summary>
        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            return cmd is MySqlCommand mySqlCmd ?
                mySqlCmd.Parameters.AddWithValue(paramName, value) :
                throw new ArgumentException("MySqlCommand is required.", nameof(cmd));
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
        public override string BuildConnectionString(DbConnectionOptions connectionOptions)
        {
            return BuildMySqlConnectionString(connectionOptions);
        }

        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public static string BuildMySqlConnectionString(DbConnectionOptions connectionOptions, bool hidePassword = false)
        {
            if (connectionOptions == null)
                throw new ArgumentNullException(nameof(connectionOptions));

            ExtractHostAndPort(connectionOptions.Server, DefaultPort, out string host, out int port);

            return new MySqlConnectionStringBuilder()
            {
                Server = host,
                Port = (uint)port,
                Database = connectionOptions.Database,
                UserID = connectionOptions.Username,
                Password = hidePassword ? HiddenPassword : connectionOptions.Password
            }
            .ToString();
        }
    }
}
