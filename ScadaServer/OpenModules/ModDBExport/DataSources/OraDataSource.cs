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
 * Summary  : Oracle interacting traits
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;

#pragma warning disable 618 // отключить предупреждение об устаревших классах Oracle

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// Oracle interacting traits
    /// <para>Особенности взаимодействия с Oracle</para>
    /// </summary>
    internal class OraDataSource : DataSource
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OraDataSource()
            : base()
        {
            DBType = DBType.Oracle;
        }


        /// <summary>
        /// Проверить существование и тип соединения с БД
        /// </summary>
        private void CheckConnection()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection is not inited.");
            if (!(Connection is OracleConnection))
                throw new InvalidOperationException("OracleConnection is required.");
        }


        /// <summary>
        /// Создать соединение с БД
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new OracleConnection();
        }

        /// <summary>
        /// Очистить пул приложений
        /// </summary>
        protected override void ClearPool()
        {
            CheckConnection();
            OracleConnection.ClearPool((OracleConnection)Connection);
        }

        /// <summary>
        /// Создать команду
        /// </summary>
        protected override DbCommand CreateCommand(string cmdText)
        {
            CheckConnection();
            return new OracleCommand(cmdText, (OracleConnection)Connection);
        }

        /// <summary>
        /// Добавить параметр команды со значением
        /// </summary>
        protected override void AddCmdParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");
            if (!(cmd is OracleCommand))
                throw new ArgumentException("OracleCommand is required.", "cmd");

            OracleCommand oraCmd = (OracleCommand)cmd;
            oraCmd.Parameters.AddWithValue(paramName, value);
        }


        /// <summary>
        /// Построить строку соединения с БД на основе остальных свойств соединения
        /// </summary>
        public override string BuildConnectionString()
        {
            return string.Format("Server={0}{1};User ID={2};Password={3}", 
                Server, string.IsNullOrEmpty(Database) ? "" : "/" + Database, User, Password);
        }
    }
}
