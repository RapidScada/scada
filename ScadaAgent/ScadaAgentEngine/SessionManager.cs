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
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Session manager
    /// <para>Менеджер сессий</para>
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// Макс. количество сессий
        /// </summary>
        private const int MaxSessionCnt = 100;
        /// <summary>
        /// Макс. количество попыток получения уникального ид. сессии
        /// </summary>
        private const int MaxGetSessionIDAttempts = 100;
        /// <summary>
        /// Время жизни сессии, если нет активности
        /// </summary>
        private readonly TimeSpan SessionLifetime = TimeSpan.FromMinutes(1);

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
            this.log = log ?? throw new ArgumentNullException("log");
            sessions = new Dictionary<long, Session>();
        }


        /// <summary>
        /// Создать сессию и добавить в список сессий
        /// </summary>
        public Session CreateSession()
        {
            lock (sessions)
            {
                long sessionID = 0;
                bool sessionOK = false;

                if (sessions.Count < MaxSessionCnt)
                {
                    sessionID = ScadaUtils.GetRandomLong();
                    int attemptNum = 0;
                    bool duplicated;

                    while (duplicated = sessionID == 0 || sessions.ContainsKey(sessionID) && 
                        ++attemptNum <= MaxGetSessionIDAttempts)
                    {
                        sessionID = ScadaUtils.GetRandomLong();
                    }

                    sessionOK = !duplicated;
                }

                if (sessionOK)
                {
                    Session session = new Session(sessionID);
                    sessions.Add(sessionID, session);
                    log.WriteAction(string.Format(Localization.UseRussian ?
                        "Создана сессия с ид. {0}" :
                        "Session with ID {0} created", sessionID));
                    return session;
                }
                else
                {
                    log.WriteError(Localization.UseRussian ?
                        "Не удалось создать сессию" :
                        "Unable to create session");
                    return null;
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
            DateTime utcNowDT = DateTime.UtcNow;
            List<long> keysToRemove = new List<long>();

            lock (sessions)
            {
                foreach (KeyValuePair<long, Session> pair in sessions)
                {
                    if (utcNowDT - pair.Value.ActivityDT > SessionLifetime)
                        keysToRemove.Add(pair.Key);
                }

                foreach (long key in keysToRemove)
                {
                    sessions.Remove(key);
                }
            }
        }

        /// <summary>
        /// Удалить все сессии
        /// </summary>
        public void RemoveAllSessions()
        {
            lock (sessions)
            {
                sessions.Clear();
            }
        }

        /// <summary>
        /// Получить информацию о сессиях
        /// </summary>
        public string GetInfo()
        {
            lock (sessions)
            {
                if (sessions.Count > 0)
                {
                    StringBuilder sbInfo = new StringBuilder();

                    foreach (Session session in sessions.Values)
                    {
                        sbInfo.AppendLine(session.ToString());
                    }

                    return sbInfo.ToString();
                }
                else
                {
                    return Localization.UseRussian ?
                        "Нет" :
                        "No";
                }
            }
        }
    }
}
