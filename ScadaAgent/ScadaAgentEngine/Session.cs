/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : ScadaAgentEngine
 * Summary  : Session manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Text;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Session of communication with the agent
    /// <para>Сессия связи с агентом</para>
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private Session()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Session(long sessionID)
        {
            ID = sessionID;
            IpAddress = "";
            LoggedOn = false;
            Username = "";
            ScadaInstance = null;
            ActivityDT = DateTime.UtcNow;
        }


        /// <summary>
        /// Получить идентификатор сессии
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// Получить IP-адрес подключения
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Получить или установить признак, авторизован ли пользователь агента
        /// </summary>
        public bool LoggedOn { get; set; }

        /// <summary>
        /// Получить или установить имя пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Получить или установить экземпляр системы
        /// </summary>
        public ScadaInstance ScadaInstance { get; set; }

        /// <summary>
        /// Получить или установить дату и время последней активности (UTC)
        /// </summary>
        public DateTime ActivityDT { get; private set; }


        /// <summary>
        /// Зарегистрировать активность
        /// </summary>
        public void RegisterActivity()
        {
            ActivityDT = DateTime.UtcNow;
        }

        /// <summary>
        /// Установить данные авторизованного пользователя
        /// </summary>
        public void SetUser(string username, ScadaInstance scadaInstance)
        {
            LoggedOn = true;
            Username = username;
            ScadaInstance = scadaInstance;
        }

        /// <summary>
        /// Очистить данные, связанные с пользователем
        /// </summary>
        public void ClearUser()
        {
            LoggedOn = false;
            Username = "";
            ScadaInstance = null;
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[").Append(ID).Append("] ").Append(IpAddress);

            if (LoggedOn)
                sb.Append("; ").Append(Username);

            sb.Append("; ").Append(ActivityDT.ToLocalTime().ToString("T", Localization.Culture));

            return sb.ToString();
        }
    }
}
