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
 * Summary  : Implements a data source for Microsoft SQL Server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Comm.Devices.DbImport.Configuration;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Scada.Comm.Devices.DbImport.Data
{
    /// <summary>
    /// Implements a data source for Microsoft SQL Server.
    /// <para>Реализует источник данных для Microsoft SQL Server.</para>
    /// </summary>
    internal class SqlDataSource : DataSource
    {
        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        /// <summary>
        /// Creates a command.
        /// </summary>
        protected override DbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        /// Adds the command parameter containing the value.
        /// </summary>
        protected override void AddCmdParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");
            if (!(cmd is SqlCommand))
                throw new ArgumentException("SqlCommand is required.", "cmd");

            SqlCommand sqlCmd = (SqlCommand)cmd;
            sqlCmd.Parameters.AddWithValue(paramName, value);
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            if (Connection != null)
                SqlConnection.ClearPool((SqlConnection)Connection);
        }


        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public override string BuildConnectionString(DbConnSettings connSettings)
        {
            return BuildSqlConnectionString(connSettings);
        }

        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public static string BuildSqlConnectionString(DbConnSettings connSettings)
        {
            if (connSettings == null)
                throw new ArgumentNullException("connSettings");

            return string.Format("Server={0};Database={1};User ID={2};Password={3}", 
                connSettings.Server, connSettings.Database, connSettings.User, connSettings.Password);
        }
    }
}
