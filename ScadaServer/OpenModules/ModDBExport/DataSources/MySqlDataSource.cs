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
 * Summary  : MySQL interacting traits
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// MySQL interacting traits
    /// <para>Особенности взаимодействия с MySQL</para>
    /// </summary>
    internal class MySqlDataSource : DataSource
    {
        /// <summary>
        /// Порт по умолчанию
        /// </summary>
        private const int DefaultPort = 3306;


        /// <summary>
        /// Конструктор
        /// </summary>
        public MySqlDataSource()
            : base()
        {
            DBType = DBType.MySQL;
        }


        /// <summary>
        /// Проверить существование и тип соединения с БД
        /// </summary>
        private void CheckConnection()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection is not inited.");
            if (!(Connection is MySqlConnection))
                throw new InvalidOperationException("MySqlConnection is required.");
        }


        /// <summary>
        /// Создать соединение с БД
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// Очистить пул приложений
        /// </summary>
        protected override void ClearPool()
        {
            CheckConnection();
            MySqlConnection.ClearPool((MySqlConnection)Connection);
        }

        /// <summary>
        /// Создать команду
        /// </summary>
        protected override DbCommand CreateCommand(string cmdText)
        {
            CheckConnection();
            return new MySqlCommand(cmdText, (MySqlConnection)Connection);
        }

        /// <summary>
        /// Добавить параметр команды со значением
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
        /// Построить строку соединения с БД на основе остальных свойств соединения
        /// </summary>
        public override string BuildConnectionString()
        {
            string host;
            int port;
            ExtractHostAndPort(Server, DefaultPort, out host, out port);

            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = host;
            csb.Port = (uint)port;
            csb.Database = Database;
            csb.UserID = User;
            csb.Password = Password;

            return csb.ToString();
        }
    }
}
