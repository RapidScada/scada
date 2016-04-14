/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Summary  : Allows to remember that a user is logged on
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.IO;
using System.Web;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Allows to remember that a user is logged on
    /// <para>Позволяет запоминать, что пользователь вошёл в систему</para>
    /// </summary>
    public class RememberMe
    {
        /// <summary>
        /// Учётные данные
        /// </summary>
        public class Credentials
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Credentials(string browserID, string oneTimePassword)
            {
                BrowserID = browserID ?? "";
                OneTimePassword = oneTimePassword ?? "";
            }

            /// <summary>
            /// Получить идентификатор, присваиваемый браузеру пользователя
            /// </summary>
            public string BrowserID { get; private set; }
            /// <summary>
            /// Одноразовый пароль
            /// </summary>
            public string OneTimePassword { get; private set; }
        }

        /// <summary>
        /// Длительность хранения информации о входе пользователя в систему
        /// </summary>
        private static readonly TimeSpan ExpireSpan = TimeSpan.FromDays(7);


        /// <summary>
        /// Конструктор
        /// </summary>
        public RememberMe()
        {
        }


        /// <summary>
        /// Проверить, что пользователю разрешён вход в систему
        /// </summary>
        private bool ValidateUser(string username, Credentials credentials, out Credentials newCredentials)
        {
            newCredentials = null;
            return false;
        }

        /// <summary>
        /// Создать учётные данные для пользователя на стороне сервера
        /// </summary>
        private Credentials CreateCredentials(string username)
        {
            return null;
        }

        /// <summary>
        /// Очистить учётные данные пользователя на стороне сервера
        /// </summary>
        private void ClearCredentials(string username)
        {
        }

        /// <summary>
        /// Создать cookie для записи информации о входе в систему
        /// </summary>
        private HttpCookie CreateCookie(string username, Credentials credentials)
        {
            HttpCookie cookie = new HttpCookie("User");
            cookie.Values.Set("Username", username);

            if (credentials != null)
            {
                cookie.Values.Set("BrowserID", credentials.BrowserID);
                cookie.Values.Set("OneTimePassword", credentials.OneTimePassword);
            }

            cookie.Expires = DateTime.Now.Add(ExpireSpan);
            return cookie;
        }


        /// <summary>
        /// Проверить, что пользователю, данные которого записаны в cookies, разрешён вход в систему
        /// </summary>
        public bool ValidateUser(HttpContext httpContext, out string username, out string alert)
        {
            alert = "";
            ScadaWebUtils.CheckHttpContext(httpContext, true);
            HttpCookie reqCookie = httpContext.Request.Cookies["User"];

            if (reqCookie != null && reqCookie.HasKeys)
            {
                username = reqCookie.Values["Username"] ?? "";
                Credentials credentials = new Credentials(
                    reqCookie.Values["BrowserID"], reqCookie.Values["OneTimePassword"]);
                Credentials newCredentials;

                if (ValidateUser(username, credentials, out newCredentials))
                {
                    HttpCookie respCookie = CreateCookie(username, newCredentials);
                    httpContext.Response.Cookies.Set(respCookie);
                    return true;
                }
                else
                {
                    HttpCookie respCookie = CreateCookie(username, null);
                    httpContext.Response.Cookies.Set(respCookie);
                    return false;
                }
            }
            else
            {
                username = "";
                return false;
            }
        }

        /// <summary>
        /// Извлечь имя пользователя из cookies
        /// </summary>
        public string RestoreUsername(HttpContext httpContext)
        {
            ScadaWebUtils.CheckHttpContext(httpContext, true);
            HttpCookie cookie = httpContext.Request.Cookies["User"];
            return cookie != null && cookie.HasKeys ? (cookie.Values["Username"] ?? "") : "";
        }

        /// <summary>
        /// Запомнить имя пользователя в cookies
        /// </summary>
        public void RememberUsername(string username, HttpContext httpContext)
        {
            ScadaWebUtils.CheckHttpContext(httpContext, true);
            HttpCookie cookie = CreateCookie(username, null);
            httpContext.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// Запомнить, что пользователь вошёл в систему
        /// </summary>
        public void RememberUser(string username, HttpContext httpContext)
        {
            ScadaWebUtils.CheckHttpContext(httpContext, true);
            Credentials credentials = CreateCredentials(username);
            HttpCookie cookie = CreateCookie(username, credentials);
            httpContext.Response.Cookies.Set(cookie);
        }
    }
}
