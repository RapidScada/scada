/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ModDBExport
 * Summary  : PostgreSQL interacting traits
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Npgsql;
using System;
using System.Data.Common;

namespace Scada.Server.Modules.DBExport.DataSources
{
    /// <summary>
    /// PostgreSQL interacting traits
    /// <para>Особенности взаимодействия с PostgreSQL</para>
    /// </summary>
    internal class PgSqlDataSource : DataSource
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PgSqlDataSource()
            : base()
        {
            DBType = DBType.PostgreSQL;
        }


        /// <summary>
        /// Проверить существование и тип соединения с БД
        /// </summary>
        private void CheckConnection()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection is not inited.");
            if (!(Connection is NpgsqlConnection))
                throw new InvalidOperationException("NpgsqlConnection is required.");
        }


        /// <summary>
        /// Создать соединение с БД
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new NpgsqlConnection();
        }


        /// <summary>
        /// Построить строку соединения с БД на основе остальных свойств соединения
        /// </summary>
        protected override string BuildConnectionString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Очистить пул приложений
        /// </summary>
        protected override void ClearPool()
        {
            NpgsqlConnection.ClearAllPools();
        }

        /// <summary>
        /// Создать команду
        /// </summary>
        protected override DbCommand CreateCommand(string cmdText)
        {
            CheckConnection();
            return new NpgsqlCommand(cmdText, (NpgsqlConnection)Connection);
        }

        /// <summary>
        /// Добавить параметр команды со значением
        /// </summary>
        protected override void AddCmdParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");
            if (!(cmd is NpgsqlCommand))
                throw new ArgumentException("NpgsqlCommand is required.", "cmd");

            NpgsqlCommand pgSqlCmd = (NpgsqlCommand)cmd;
            pgSqlCmd.Parameters.AddWithValue(paramName, value);
        }
    }
}
