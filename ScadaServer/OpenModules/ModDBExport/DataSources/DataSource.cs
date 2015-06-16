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
 * Summary  : The base class for interacting with database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// The base class for interacting with database
    /// <para>Базовый класс для взаимодействия с БД</para>
    /// </summary>
    internal abstract class DataSource : IComparable<DataSource>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DataSource()
        {
            DBType = DBType.Undefined;
            Server = "";
            Database = "";
            User = "";
            Password = "";
            ConnectionString = "";

            Connection = null;
            ExportCurDataCmd = null;
            ExportArcDataCmd = null;
            ExportEventCmd = null;
        }


        /// <summary>
        /// Получить или установить тип БД
        /// </summary>
        public DBType DBType { get; set; }

        /// <summary>
        /// Получить или установить сервер БД
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Получить или установить имя БД
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Получить или установить пользователя БД
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Получить или установить пароль пользователя БД
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Получить или установить строку соединения с БД
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Получить наименование источника данных
        /// </summary>
        public string Name
        {
            get
            {
                return DBType + (string.IsNullOrEmpty(Server) ? "" : " - " + Server);
            }
        }



        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        public DbConnection Connection { get; protected set; }

        /// <summary>
        /// Получить команду экспорта текущих данных
        /// </summary>
        public DbCommand ExportCurDataCmd { get; protected set; }

        /// <summary>
        /// Получить команду экспорта архивных данных
        /// </summary>
        public DbCommand ExportArcDataCmd { get; protected set; }

        /// <summary>
        /// Получить команду экспорта события
        /// </summary>
        public DbCommand ExportEventCmd { get; protected set; }


        /// <summary>
        /// Создать соединение с БД
        /// </summary>
        protected abstract DbConnection CreateConnection();

        /// <summary>
        /// Очистить пул приложений
        /// </summary>
        protected abstract void ClearPool();

        /// <summary>
        /// Создать команду
        /// </summary>
        protected abstract DbCommand CreateCommand(string cmdText);

        /// <summary>
        /// Добавить параметр команды со значением
        /// </summary>
        protected abstract void AddCmdParamWithValue(DbCommand cmd, string paramName, object value);

        /// <summary>
        /// Извлечь имя хоста и порт из имени сервера
        /// </summary>
        protected void ExtractHostAndPort(string server, int defaultPort, out string host, out int port)
        {
            int ind = Server.IndexOf(':');

            if (ind >= 0)
            {
                host = Server.Substring(0, ind);
                try { port = int.Parse(Server.Substring(ind + 1)); }
                catch { port = defaultPort; }
            }
            else
            {
                host = Server;
                port = defaultPort;
            }
        }


        /// <summary>
        /// Построить строку соединения с БД на основе остальных свойств соединения
        /// </summary>
        public abstract string BuildConnectionString();

        /// <summary>
        /// Инициализировать соединение с БД
        /// </summary>
        public void InitConnection()
        {
            Connection = CreateConnection();
            if (string.IsNullOrEmpty(ConnectionString))
                ConnectionString = BuildConnectionString();
            Connection.ConnectionString = ConnectionString;
        }

        /// <summary>
        /// Соединиться с БД
        /// </summary>
        public void Connect()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection is not inited.");

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
        /// Разъединиться с БД
        /// </summary>
        public void Disconnect()
        {
            if (Connection != null)
                Connection.Close();
        }

        /// <summary>
        /// Инициализировать команды экспорта данных
        /// </summary>
        public void InitCommands(string exportCurDataQuery, string exportArcDataQuery, string exportEventQuery)
        {
            ExportCurDataCmd = string.IsNullOrEmpty(exportCurDataQuery) ? null : CreateCommand(exportCurDataQuery);
            ExportArcDataCmd = string.IsNullOrEmpty(exportArcDataQuery) ? null : CreateCommand(exportArcDataQuery);
            ExportEventCmd = string.IsNullOrEmpty(exportArcDataQuery) ? null : CreateCommand(exportEventQuery);
        }

        /// <summary>
        /// Установить значение параметра команды
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

        /// <summary>
        /// Клонировать источник данных
        /// </summary>
        public virtual DataSource Clone()
        {
            DataSource dataSourceCopy = (DataSource)Activator.CreateInstance(this.GetType());
            dataSourceCopy.DBType = DBType;
            dataSourceCopy.Server = Server;
            dataSourceCopy.Database = Database;
            dataSourceCopy.User = User;
            dataSourceCopy.Password = Password;
            dataSourceCopy.ConnectionString = ConnectionString;
            return dataSourceCopy;
        }

        /// <summary>
        /// Сравнить текущий объект с другим объектом такого же типа
        /// </summary>
        public int CompareTo(DataSource other)
        {
            int comp = DBType.CompareTo(other.DBType);
            return comp == 0 ? Name.CompareTo(other.Name) : comp;
        }
    }
}
