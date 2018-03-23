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
 * Module   : ScadaAgentCore
 * Summary  : Session manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System.Collections.Generic;
using System.Text;
using Utils;

namespace Scada.Agent
{
    /// <summary>
    /// Session manager
    /// <para>Менеджер сессий</para>
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// Количество попыток получения уникального ид. сессии
        /// </summary>
        private const int GetIDAttemtps = 100;

        private Dictionary<long, Session> sessions; // список сессий, ключ - ид. сессии
        private ILog log; // журнал приложения


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private SessionManager()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SessionManager(ILog log)
        {
            this.log = log;
            sessions = new Dictionary<long, Session>();
        }


        /// <summary>
        /// Создать сессию и добавить в список сессий
        /// </summary>
        public Session CreateSession()
        {
            lock (sessions)
            {
                long sessionID = CryptoUtils.GetRandomLong();
                int attempt = 0;
                bool duplicated;

                while (duplicated = sessions.ContainsKey(sessionID) && ++attempt <= GetIDAttemtps)
                {
                    sessionID = CryptoUtils.GetRandomLong();
                }

                if (duplicated)
                {
                    log.WriteError(Localization.UseRussian ?
                        "Не удалось создать сессию" :
                        "Unable to create session");
                    return null;
                }
                else
                {
                    Session session = new Session(sessionID);
                    sessions.Add(sessionID, session);
                    log.WriteAction(string.Format(Localization.UseRussian ? 
                        "Создана сессия с ид. {0}" : 
                        "Session with ID {0} created", sessionID));
                    return session;
                }
            }
        }

        /// <summary>
        /// Получить сессию по идентификатору
        /// </summary>
        public Session GetSession(long sessionID)
        {
            lock (sessions)
            {
                return sessions.TryGetValue(sessionID, out Session session) ? session: null;
            }
        }

        /// <summary>
        /// Удалить неактивные сессии
        /// </summary>
        public void RemoveInactiveSessions()
        {

        }

        /// <summary>
        /// Получить информацию о сессиях
        /// </summary>
        public string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder();

            lock (sessions)
            {
                foreach (Session session in sessions.Values)
                {
                    sbInfo.AppendLine(session.ToString());
                }
            }

            return sbInfo.ToString();
        }
    }
}
