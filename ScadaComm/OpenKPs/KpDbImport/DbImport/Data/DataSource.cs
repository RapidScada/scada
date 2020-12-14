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

using Scada.Comm.Devices.DbImport.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Scada.Comm.Devices.DbImport.Data
{
    /// <summary>
    /// The base class of the data source.
    /// <para>Базовый класс источника данных.</para>
    /// </summary>
    internal abstract class DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected DataSource()
        {
            Connection = null;
            SelectCommand = null;
            ExportCommands = new SortedList<int, DbCommand>();
        }


        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        protected DbConnection Connection { get; set; }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        public DbCommand SelectCommand { get; protected set; }

        /// <summary>
        /// Gets the export commands.
        /// </summary>
        public SortedList<int, DbCommand> ExportCommands { get; protected set; }


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
        public abstract string BuildConnectionString(DbConnSettings connSettings);

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
        /// Initializes the data source.
        /// </summary>
        public void Init(string connectionString, KpConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            Connection = CreateConnection();
            Connection.ConnectionString = connectionString;

            SelectCommand = CreateCommand();
            SelectCommand.CommandText = config.SelectQuery;
            SelectCommand.Connection = Connection;

            foreach (ExportCmd exportCmd in config.ExportCmds)
            {
                DbCommand exportCommand = CreateCommand();
                exportCommand.CommandText = exportCmd.Query;
                exportCommand.Connection = Connection;
                ExportCommands[exportCmd.CmdNum] = exportCommand;
            }
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
