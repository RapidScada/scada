/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : The base class of the data source
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using Scada.Config;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Scada.Db
{
    /// <summary>
    /// The base class of the data source.
    /// <para>Базовый класс источника данных.</para>
    /// </summary>
    public abstract class DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataSource(DbConnectionOptions connectionOptions)
        {
            ConnectionOptions = connectionOptions ?? throw new ArgumentNullException(nameof(connectionOptions));
            Connection = CreateConnection();
            Connection.ConnectionString = BuildConnectionString(connectionOptions);
        }


        /// <summary>
        /// Gets the database connection options.
        /// </summary>
        public DbConnectionOptions ConnectionOptions { get; }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public DbConnection Connection { get; }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected abstract DbConnection CreateConnection();

        /// <summary>
        /// Creates a command.
        /// </summary>
        protected abstract DbCommand CreateCommand();

        /// <summary>
        /// Adds the command parameter containing the value.
        /// </summary>
        protected abstract void AddCmdParamWithValue(DbCommand cmd, string paramName, object value);

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected abstract void ClearPool();

        /// <summary>
        /// Extracts host name and port from the specified server name.
        /// </summary>
        protected static void ExtractHostAndPort(string server, int defaultPort, out string host, out int port)
        {
            int ind = server.IndexOf(':');

            if (ind >= 0)
            {
                host = server.Substring(0, ind);
                if (!int.TryParse(server.Substring(ind + 1), out port))
                    port = defaultPort;
            }
            else
            {
                host = server;
                port = defaultPort;
            }
        }


        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public abstract string BuildConnectionString(DbConnectionOptions connectionOptions);

        /// <summary>
        /// Connects to the database.
        /// </summary>
        public void Connect()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection is not initialized.");

            try
            {
                Connection.Open();
            }
            catch
            {
                Connection.Close();
                ClearPool();
                throw;
            }
        }

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        public void Disconnect()
        {
            Connection?.Close();
        }

        /// <summary>
        /// Sets the command parameter.
        /// </summary>
        public void SetCmdParam(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            if (cmd.Parameters.Contains(paramName))
                cmd.Parameters[paramName].Value = value;
            else
                AddCmdParamWithValue(cmd, paramName, value);
        }
    }
}
