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
 * Summary  : Creates data sources
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Config;
using System;

namespace Scada.Db
{
    /// <summary>
    /// Creates data sources.
    /// <para>Создает источники данных.</para>
    /// </summary>
    public static class DataSourceFactory
    {
        /// <summary>
        /// Gets a new data source.
        /// </summary>
        public static DataSource GetDataSource(DbConnectionOptions connectionOptions)
        {
            if (connectionOptions == null)
                throw new ArgumentNullException(nameof(connectionOptions));

            switch (connectionOptions.KnownDBMS)
            {
                case KnownDBMS.PostgreSQL:
                    return new PgSqlDataSource(connectionOptions);

                case KnownDBMS.MySQL:
                    return new MySqlDataSource(connectionOptions);

                case KnownDBMS.MSSQL:
                    return new SqlDataSource(connectionOptions);

                case KnownDBMS.Oracle:
                    return new OraDataSource(connectionOptions);

                case KnownDBMS.OLEDB:
                    return new OleDbDataSource(connectionOptions);

                default:
                    throw new ScadaException("Unknown DBMS.");
            }
        }
    }
}
