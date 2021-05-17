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
 * Summary  : Implements a data source for OLE DB
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Config;
using System;
using System.Data.Common;
using System.Data.OleDb;

namespace Scada.Db
{
    /// <summary>
    /// Implements a data source for OLE DB.
    /// <para>Реализует источник данных для OLE DB.</para>
    /// </summary>
    public class OleDbDataSource : DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OleDbDataSource(DbConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new OleDbConnection();
        }

        /// <summary>
        /// Adds the command parameter containing the value.
        /// </summary>
        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            return cmd is OleDbCommand oleDbCmd ?
                oleDbCmd.Parameters.AddWithValue(paramName, value) :
                throw new ArgumentException("OleDbCommand is required.", nameof(cmd));
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            // do nothing
        }


        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public override string BuildConnectionString(DbConnectionOptions connectionOptions)
        {
            return "";
        }
    }
}
