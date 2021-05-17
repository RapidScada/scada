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
 * Summary  : Implements a data source for PostgreSQL
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Npgsql;
using Scada.Config;
using System;
using System.Data.Common;

namespace Scada.Db
{
    /// <summary>
    /// Implements a data source for PostgreSQL.
    /// <para>Реализует источник данных для PostgreSQL.</para>
    /// </summary>
    public class PgSqlDataSource : DataSource
    {
        /// <summary>
        /// The default port of the database server.
        /// </summary>
        private const int DefaultPort = 5432;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PgSqlDataSource(DbConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new NpgsqlConnection();
        }

        /// <summary>
        /// Adds the command parameter containing the value.
        /// </summary>
        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            return cmd is NpgsqlCommand pgSqlCmd ?
                pgSqlCmd.Parameters.AddWithValue(paramName, value) :
                throw new ArgumentException("NpgsqlCommand is required.", nameof(cmd));
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            NpgsqlConnection.ClearAllPools();
        }


        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public override string BuildConnectionString(DbConnectionOptions connectionOptions)
        {
            return BuildPgSqlConnectionString(connectionOptions);
        }

        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public static string BuildPgSqlConnectionString(DbConnectionOptions connectionOptions, bool hidePassword = false)
        {
            if (connectionOptions == null)
                throw new ArgumentNullException(nameof(connectionOptions));

            ExtractHostAndPort(connectionOptions.Server, DefaultPort, out string host, out int port);
            return string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4}",
                host, port, connectionOptions.Database, connectionOptions.Username, 
                hidePassword ? HiddenPassword : connectionOptions.Password);
        }
    }
}
