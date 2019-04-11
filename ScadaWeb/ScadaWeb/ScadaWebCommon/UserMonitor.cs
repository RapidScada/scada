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
 * Module   : ScadaWebCommon
 * Summary  : User activity monitor
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2018
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Web;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// User activity monitor
    /// <para>Монитор активности пользователей</para>
    /// </summary>
    public class UserMonitor
    {
        /// <summary>
        /// Класс, позволяющий сравнивать снимки данных пользователей
        /// </summary>
        protected class UserShotComparer : IComparer<UserShot>
        {
            /// <summary>
            /// Сравнить два объекта
            /// </summary>
            public int Compare(UserShot x, UserShot y)
            {
                int comp1 = x.LoggedOn.CompareTo(y.LoggedOn);
                if (comp1 == 0)
                {
                    string userName1 = x.UserProps == null ? "" : x.UserProps.UserName;
                    string userName2 = y.UserProps == null ? "" : y.UserProps.UserName;
                    int comp2 = string.Compare(userName1, userName2, StringComparison.OrdinalIgnoreCase);
                    return comp2 == 0 ? 
                        string.Compare(x.IpAddress, y.IpAddress, StringComparison.OrdinalIgnoreCase) : 
                        comp2;
                }
                else
                {
                    return comp1;
                }
            }
        }

        /// <summary>
        /// Объект для сравнения снимков данных пользователей для сортировки
        /// </summary>
        protected static readonly UserShotComparer UserShotComp = new UserShotComparer();
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;
        /// <summary>
        /// Данные пользователей с доступом по ид. сессии
        /// </summary>
        protected readonly Dictionary<string, UserData> userDataDict;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected UserMonitor()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserMonitor(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this.log = log;
            userDataDict = new Dictionary<string, UserData>();
        }


        /// <summary>
        /// Проверить контекст веб-операции и его основные свойства на null
        /// </summary>
        private void CheckWebOperationContext(WebOperationContext webOpContext)
        {
            const string msg = "Web operation context or its properties are undefined.";

            if (webOpContext == null)
                throw new ArgumentNullException("webOpContext", msg);
            if (webOpContext.IncomingRequest == null)
                throw new ArgumentNullException("webOpContext.IncomingRequest", msg);
        }

        /// <summary>
        /// Извлечь ид. сессии из HTTP-заголовка
        /// </summary>
        private string ExtractSessionID(string cookieHeader)
        {
            if (cookieHeader == null)
            {
                return null;
            }
            else
            {
                const string SessionCookieName = "ASP.NET_SessionId=";
                int i1 = cookieHeader.IndexOf(SessionCookieName);

                if (i1 < 0)
                {
                    return null;
                }
                else
                {
                    i1 += SessionCookieName.Length;
                    int i2 = cookieHeader.IndexOf(';', i1);
                    return i2 < 0 ? 
                        cookieHeader.Substring(i1) : 
                        cookieHeader.Substring(i1, i2 - i1);
                }
            }
        }

        /// <summary>
        /// Попытаться получить данные пользователя по контексту веб-операции
        /// </summary>
        /// <remarks>Метод получения данных пользователя не должен быть public,
        /// потому что возникнут проблемы с многопоточностью</remarks>
        private bool TryGetUserData(WebOperationContext webOpContext, out UserData userData)
        {
            CheckWebOperationContext(webOpContext);

            try
            {
                string cookieHeader = webOpContext.IncomingRequest.Headers[HttpRequestHeader.Cookie];
                string sessionID = ExtractSessionID(cookieHeader);

                if (sessionID != null)
                {
                    lock (userDataDict)
                    {
                        return userDataDict.TryGetValue(sessionID, out userData);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении данных пользователя по контексту веб-операции" :
                    "Error getting user data by the web operation context");
            }

            userData = null;
            return false;
        }


        /// <summary>
        /// Добавить информацию о пользователе
        /// </summary>
        public void AddUser(UserData userData)
        {
            try
            {
                string sessionID = userData.SessionID;

                if (userData != null && !string.IsNullOrEmpty(sessionID))
                {
                    lock (userDataDict)
                    {
                        if (userDataDict.ContainsKey(sessionID))
                        {
                            userDataDict[sessionID] = userData;
                            log.WriteAction(string.Format(Localization.UseRussian ?
                                "Обновлена информация о пользователе. IP-адрес: {0}. Сессия: {1}" :
                                "User information has been updated. IP address: {0}. Session: {1}",
                                userData.IpAddress, sessionID));
                        }
                        else
                        {
                            userDataDict.Add(sessionID, userData);
                            log.WriteAction(string.Format(Localization.UseRussian ?
                                "Добавлена информация о пользователе. IP-адрес: {0}. Сессия: {1}" :
                                "User information has been added. IP address: {0}. Session: {1}",
                                userData.IpAddress, sessionID));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при добавлении информации о пользователе" :
                    "Error adding the user information");
            }
        }

        /// <summary>
        /// Удалить информацию о пользователе
        /// </summary>
        public void RemoveUser(string sessionID)
        {
            try
            {
                if (sessionID != null)
                {
                    lock (userDataDict)
                    {
                        UserData userData;
                        if (userDataDict.TryGetValue(sessionID, out userData))
                        {
                            if (userData.LoggedOn)
                                userData.Logout();
                            userDataDict.Remove(sessionID);
                            log.WriteAction(string.Format(Localization.UseRussian ?
                                "Удалена информация о пользователе. IP-адрес: {0}. Сессия: {1}" :
                                "User information has been removed. IP address: {0}. Session: {1}",
                                userData.IpAddress, sessionID));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при удалении информации о пользователе" :
                    "Error removing the user information");
            }
        }

        /// <summary>
        /// Проверить, что пользователь вошёл в систему, и получить его права
        /// </summary>
        public bool UserIsLoggedOn(WebOperationContext webOpContext, out UserRights userRights)
        {
            UserData userData;

            if (TryGetUserData(webOpContext, out userData) && userData.LoggedOn && userData.UserRights != null)
            {
                userRights = userData.UserRights;
                return userData.LoggedOn; // свойство могло измениться в другом потоке
            }
            else
            {
                userRights = null;
                return false;
            }
        }

        /// <summary>
        /// Проверить, что пользователь вошёл в систему, и получить его снимок
        /// </summary>
        public bool UserIsLoggedOn(WebOperationContext webOpContext, out UserShot userShot)
        {
            UserData userData;

            if (TryGetUserData(webOpContext, out userData) && userData.LoggedOn)
            {
                userShot = new UserShot(userData);
                return userData.LoggedOn; // свойство могло измениться в другом потоке
            }
            else
            {
                userShot = null;
                return false;
            }
        }

        /// <summary>
        /// Проверить, что пользователь вошёл систему, и получить его права
        /// </summary>
        public bool CheckLoggedOn(out UserRights userRights, bool throwOnFail = true)
        {
            if (UserIsLoggedOn(WebOperationContext.Current, out userRights))
                return true;
            else if (throwOnFail)
                throw new ScadaException(WebPhrases.NotLoggedOn);
            else
                return false;
        }

        /// <summary>
        /// Проверить, что пользователь вошёл систему, и получить снимок его данных
        /// </summary>
        public bool CheckLoggedOn(out UserShot userShot, bool throwOnFail = true)
        {
            if (UserIsLoggedOn(WebOperationContext.Current, out userShot))
                return true;
            else if (throwOnFail)
                throw new ScadaException(WebPhrases.NotLoggedOn);
            else
                return false;
        }

        /// <summary>
        /// Получить снимки данных активных пользователей
        /// </summary>
        public UserShot[] GetActiveUsers()
        {
            try
            {
                lock (userDataDict)
                {
                    UserShot[] userShotArr = new UserShot[userDataDict.Count];
                    int i = 0;

                    foreach (UserData userData in userDataDict.Values)
                    {
                        userShotArr[i] = new UserShot(userData);
                        i++;
                    }


                    Array.Sort(userShotArr, UserShotComp);
                    return userShotArr;
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении снимков данных активных пользователей" :
                    "Error getting snapshots of the active users data");
                return new UserData[0];
            }
        }
    }
}
