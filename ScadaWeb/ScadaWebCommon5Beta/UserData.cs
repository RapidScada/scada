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
 * Summary  : Application user data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2016
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Scada.Web.Plugins;
using Scada.Web.Shell;

namespace Scada.Web
{
    /// <summary>
    /// Application user data
    /// <para>Данные пользователя приложения</para>
    /// </summary>
    /// <remarks>Inheritance is impossible because class is shared by different modules
    /// <para>Наследование невозможно, т.к. класс совместно используется различными модулями</para></remarks>
    public sealed class UserData
    {
        /// <summary>
        /// Общие данные веб-приложения
        /// </summary>
        private static readonly AppData AppData = AppData.GetAppData();

        
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private UserData()
        {
            IpAddress = "";
            SessionID = "";

            ClearUser();
            ClearAppDataRefs();
        }


        /// <summary>
        /// Получить IP-адрес пользователя
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// Получить идентификатор сессии
        /// </summary>
        public string SessionID { get; private set; }


        /// <summary>
        /// Получить имя пользователя
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Получить идентификатор пользователя в базе конфигурации
        /// </summary>
        public int UserID { get; private set; }

        /// <summary>
        /// Получить идентификатор роли пользователя
        /// </summary>
        public int RoleID { get; private set; }

        /// <summary>
        /// Получить наименование роли пользователя
        /// </summary>
        public string RoleName { get; private set; }

        /// <summary>
        /// Получить признак, выполнен ли вход пользователя в систему
        /// </summary>
        public bool LoggedOn { get; private set; }

        /// <summary>
        /// Получить дату и время входа пользователя в систему
        /// </summary>
        public DateTime LogonDT { get; private set; }

        /// <summary>
        /// Получить права пользователя
        /// </summary>
        public UserRights UserRights { get; private set; }

        /// <summary>
        /// Получить меню пользователя
        /// </summary>
        public UserMenu UserMenu { get; private set; }


        /// <summary>
        /// Получить ссылку на настройки веб-приложения
        /// </summary>
        public WebSettings WebSettings { get; private set; }

        /// <summary>
        /// Получить ссылку на настройки представлений
        /// </summary>
        public ViewSettings ViewSettings { get; private set; }

        /// <summary>
        /// Получить ссылку на список плагинов
        /// </summary>
        public List<PluginSpec> PluginSpecs { get; private set; }

        /// <summary>
        /// Получить ссылку на словарь спецификаций представлений, ключ - код типа представления
        /// </summary>
        public Dictionary<string, ViewSpec> ViewSpecs { get; private set; }


        /// <summary>
        /// Очистить данные пользователя
        /// </summary>
        private void ClearUser()
        {
            UserName = "";
            UserID = 0;
            RoleName = "";
            LoggedOn = false;
            LogonDT = DateTime.MinValue;
            UserRights = null;
            UserMenu = null;
        }

        /// <summary>
        /// Очистить ссылки на объекты общих данных приложения
        /// </summary>
        private void ClearAppDataRefs()
        {
            WebSettings = null;
            ViewSettings = null;
            PluginSpecs = null;
            ViewSpecs = null;
        }

        /// <summary>
        /// Обновить ссылки на объекты общих данных приложения
        /// </summary>
        private void UpdateAppDataRefs()
        {
            WebSettings = AppData.WebSettings;
            ViewSettings = AppData.ViewSettings;
            PluginSpecs = AppData.PluginSpecs;
            ViewSpecs = AppData.ViewSpecs;
        }


        /// <summary>
        /// Выполнить вход пользователя в систему
        /// </summary>
        /// <remarks>Если пароль равен null, то он не проверяется</remarks>
        public bool Login(string username, string password, out string errMsg)
        {
            username = username == null ? "" : username.Trim();
            int roleID;

            if (AppData.CheckUser(username, password, password != null, out roleID, out errMsg))
            {
                // заполнение свойств пользователя
                UserName = username;
                UserID = AppData.DataAccess.GetUserID(username);
                RoleID = roleID;
                RoleName = AppData.DataAccess.GetRoleName(RoleID);
                LoggedOn = true;
                LogonDT = DateTime.Now;

                UpdateAppDataRefs();
                UserRights = new UserRights();
                UserRights.Init(roleID);
                UserMenu = new UserMenu(AppData.Log);
                UserMenu.Init(this, PluginSpecs);

                if (password == null)
                {
                    AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                        "Вход в систему без пароля: {0} ({1}). IP-адрес: {2}" :
                        "Login without a password: {0} ({1}). IP address: {2}", 
                        username, RoleName, IpAddress));
                }
                else
                {
                    AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                        "Вход в систему: {0} ({1}). IP-адрес: {2}" :
                        "Login: {0} ({1}). IP address: {2}", 
                        username, RoleName, IpAddress));
                }

                return true;
            }
            else
            {
                Logout();
                AppData.Log.WriteError(string.Format(Localization.UseRussian ?
                    "Неудачная попытка входа в систему: {0}{1}. IP-адрес: {2}" :
                    "Unsuccessful login attempt: {0}{1}. IP address: {2}",
                    username == "" ? "" : username + " - ", errMsg, IpAddress));
                return false;
            }
        }

        /// <summary>
        /// Выполнить вход пользователя в систему без проверки пароля
        /// </summary>
        public bool Login(string username, out string errMsg)
        {
            return Login(username, null, out errMsg);
        }

        /// <summary>
        /// Выполнить выход пользователя из системы
        /// </summary>
        public void Logout()
        {
            if (LoggedOn)
            {
                AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                    "Выход из системы: {0}. IP-адрес: {1}" :
                    "Logout: {0}. IP address: {1}", UserName, IpAddress));
            }

            ClearUser();
            UpdateAppDataRefs();
        }

        /// <summary>
        /// Проверить, что пользователь вошёл систему. 
        /// Если вход не выполнен, то перейти на страницу входа или вызвать исключение
        /// </summary>
        public void CheckLoggedOn(bool tryToLogin)
        {
            if (!LoggedOn)
            {
                if (tryToLogin)
                {
                    HttpContext httpContext = HttpContext.Current;
                    WebUtils.CheckHttpContext(httpContext);

                    // попытка входа с использованием cookies
                    string username;
                    string alert = "";

                    if (WebSettings.RemEnabled &&
                        AppData.RememberMe.ValidateUser(httpContext, out username, out alert))
                    {
                        Login(username, out alert);
                    }

                    // переход на страницу входа
                    if (!LoggedOn)
                    {
                        httpContext.Response.Redirect("~/Login.aspx" +
                            "?return=" + HttpUtility.UrlEncode(httpContext.Request.Url.ToString()) +
                            (alert == "" ? "" : "&alert=" + HttpUtility.UrlEncode(alert)));
                    }
                }
                else
                {
                    throw new ScadaException(WebPhrases.NotLoggedOn);
                }
            }
        }


        /// <summary>
        /// Получить данные пользователя приложения
        /// </summary>
        /// <remarks>Для веб-приложения данные пользователя сохраняются в сессии</remarks>
        public static UserData GetUserData()
        {
            HttpContext httpContext = HttpContext.Current;
            WebUtils.CheckHttpContext(httpContext);
            HttpSessionState session = httpContext.Session;
            UserData userData = session["UserData"] as UserData;

            if (userData == null)
            {
                // обновление данных веб-приложения
                AppData.Refresh();

                // создание данных пользователя
                userData = new UserData();
                session.Add("UserData", userData);

                // получение IP-адреса и идентификатора сессии
                userData.IpAddress = httpContext.Request.UserHostAddress;
                userData.SessionID = session.SessionID;

                // обновление ссылок на объекты общих данных приложения
                userData.UpdateAppDataRefs();
            }

            return userData;
        }

        /// <summary>
        /// Проверить допустимость имени пользователя
        /// </summary>
        public static void ValidateUserName(string username)
        {
            if (username == null)
                throw new ArgumentNullException("username", "Username must not be null.");

            if (username == "")
                throw new ArgumentException("Username must not be null or empty.", "username");

            if (username.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException("Username contains invalid character.", "username"); ;
        }
    }
}