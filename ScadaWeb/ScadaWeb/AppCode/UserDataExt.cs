/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : SCADA-Web
 * Summary  : Application user data extension for the web application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2014
 */

using System;
using System.Collections.Specialized;
using System.Web;

namespace Scada.Web
{
    /// <summary>
    /// Application user data extension for the web application
    /// <para>Расширение данных пользователя для веб-приложения</para>
    /// </summary>
    public static class UserDataExt
    {
        /// <summary>
        /// Защищённый ключ системы
        /// </summary>
        private const string SecretKey = "ccppylrICbbhayK78wRO";


        /// <summary>
        /// Проверить HTTP-контекст и его основные свойства на null
        /// </summary>
        private static void CheckContext(HttpContext context, bool checkCookies = false)
        {
            const string msg = "HTTP context or it's properties are undefined.";

            if (context == null)
                throw new ArgumentNullException("context", msg);
            if (context.Session == null)
                throw new ArgumentNullException("context.Session", msg);
            if (context.Request == null)
                throw new ArgumentNullException("context.Request", msg);
            if (context.Response == null)
                throw new ArgumentNullException("context.Response", msg);

            if (checkCookies)
            {
                if (context.Request.Cookies == null)
                    throw new ArgumentNullException("context.Request.Cookies", msg);
                if (context.Response.Cookies == null)
                    throw new ArgumentNullException("context.Response.Cookies", msg);
            }
        }

        /// <summary>
        /// Сохранить в cookie информацию о входе пользователя в систему
        /// </summary>
        private static void RememberUser(HttpContext context, string login, string hash)
        {
            HttpCookie cookie = new HttpCookie("Login");
            cookie.Values.Set("Login", login);
            cookie.Values.Set("Hash", hash);
            cookie.Expires = DateTime.UtcNow.Add(ScadaUtils.CookieExpiration);
            context.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// Извлечь из cookie имя пользователя и признак входа в систему
        /// </summary>
        private static void RestoreUser(HttpContext context, out string login, out bool loggedOn)
        {
            CheckContext(context, true);
            HttpCookie cookie = context.Request.Cookies["Login"];

            if (cookie != null && cookie.HasKeys)
            {
                login = cookie.Values["Login"] ?? "";
                loggedOn = login != "" && cookie.Values["Hash"] == GetCookieHash(login, context.Request.Headers);
            }
            else
            {
                login = "";
                loggedOn = false;
            }
        }
                
        /// <summary>
        /// Получить хеш-функцию для защиты cookie пользователя
        /// </summary>
        private static string GetCookieHash(string login, NameValueCollection headers)
        {
            string headersStr = headers == null ? "" :
                headers["Accept"] + headers["Accept-Encoding"] + headers["Accept-Language"] + headers["User-Agent"];
            return ScadaUtils.ComputeHash(login + headersStr + SecretKey);
        }


        /// <summary>
        /// Проверить вход пользователя в систему, в случае его отсутствия перейти на страницу входа в систему
        /// </summary>
        public static void CheckLoggedOn(this UserData userData, HttpContext context, bool redirectToLogin = true)
        {
            if (!userData.LoggedOn)
            {
                // проверка входа пользователя, сохранённого в cookie
                if (AppData.WebSettings.RemEnabled)
                {
                    string login;
                    bool loggedOn;
                    RestoreUser(context, out login, out loggedOn);

                    if (loggedOn && !userData.Login(login))
                        ForgetUserLoggedOn(context);
                }

                // переход на страницу входа в систему
                if (!userData.LoggedOn && redirectToLogin)
                    context.Response.Redirect("~/Login.aspx");
            }
        }

        /// <summary>
        /// Сохранить в cookie информацию о входе пользователя в систему
        /// </summary>
        public static void RememberUser(this UserData userData, HttpContext context, bool loginOnly)
        {
            if (userData.LoggedOn)
            {
                CheckContext(context, true);
                RememberUser(context, userData.UserLogin, 
                    loginOnly ? "" : GetCookieHash(userData.UserLogin, context.Request.Headers));
            }
        }


        /// <summary>
        /// Удалить из cookie признак входа пользователя в систему
        /// </summary>
        public static void ForgetUserLoggedOn(HttpContext context)
        {
            string email = RestoreUserLogin(context);
            RememberUser(context, email, "");
        }

        /// <summary>
        /// Извлечь из cookie имя пользователя для входа в систему
        /// </summary>
        public static string RestoreUserLogin(HttpContext context)
        {
            CheckContext(context, true);
            HttpCookie cookie = context.Request.Cookies["Login"];
            return cookie != null && cookie.HasKeys ? (cookie.Values["Login"] ?? "") : "";
        }
    }
}