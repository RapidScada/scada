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
 * Summary  : OLE DB interacting traits
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Data.Common;
using System.Data.OleDb;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// OLE DB interacting traits
    /// <para>Особенности взаимодействия с OLE DB</para>
    /// </summary>
    internal class OleDbDataSource : DataSource
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OleDbDataSource()
            : base()
        {
            DBType = DBType.OLEDB;
        }


        /// <summary>
        /// Проверить существование и тип соединения с БД
        /// </summary>
        private void CheckConnection()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection is not inited.");
            if (!(Connection is OleDbConnection))
                throw new InvalidOperationException("OleDbConnection is required.");
        }


        /// <summary>
        /// Создать соединение с БД
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new OleDbConnection();
        }

        /// <summary>
        /// Очистить пул приложений
        /// </summary>
        protected override void ClearPool()
        {
            // метод очистки пула соединений OLE DB отсутствует
        }

        /// <summary>
        /// Создать команду
        /// </summary>
        protected override DbCommand CreateCommand(string cmdText)
        {
            CheckConnection();
            return new OleDbCommand(cmdText, (OleDbConnection)Connection);
        }

        /// <summary>
        /// Добавить параметр команды со значением
        /// </summary>
        protected override void AddCmdParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");
            if (!(cmd is OleDbCommand))
                throw new ArgumentException("OleDbCommand is required.", "cmd");

            OleDbCommand oleDbCmd = (OleDbCommand)cmd;
            oleDbCmd.Parameters.AddWithValue(paramName, value);
        }


        /// <summary>
        /// Построить строку соединения с БД на основе остальных свойств соединения
        /// </summary>
        public override string BuildConnectionString()
        {
            return "";
        }
    }
}
