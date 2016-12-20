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
using System.Web.Hosting;
using Scada.Data.Models;

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
        /// Конструктор, ограничивающий создание объекта из других классов
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
        /// Получить признак, выполнен ли вход пользователя в систему
        /// </summary>
        public bool LoggedOn { get; private set; }

        /// <summary>
        /// Получить дату и время входа пользователя в систему
        /// </summary>
        public DateTime LogonDT { get; private set; }

        /// <summary>
        /// Получить свойства пользователя
        /// </summary>
        /// <remarks>Объект пересоздаётся после входа в систему</remarks>
        public UserProps UserProps { get; private set; }

        /// <summary>
        /// Получить права пользователя
        /// </summary>
        /// <remarks>Объект пересоздаётся после входа в систему.
        /// Объект после инициализации не изменяется экземпляром данного класса и не должен изменяться извне,
        /// таким образом, чтение его данных является потокобезопасным</remarks>
        public UserRights UserRights { get; private set; }

        /// <summary>
        /// Получить меню пользователя
        /// </summary>
        public UserMenu UserMenu { get; private set; }

        /// <summary>
        /// Получить представления пользователя
        /// </summary>
        public UserViews UserViews { get; private set; }

        /// <summary>
        /// Получить контент пользователя
        /// </summary>
        public UserContent UserContent { get; private set; }


        /// <summary>
        /// Получить ссылку на настройки веб-приложения
        /// </summary>
        public WebSettings WebSettings { get; private set; }

        /// <summary>
        /// Получить ссылку на настройки представлений
        /// </summary>
        public ViewSettings ViewSettings { get; private set; }

        /// <summary>
        /// Получить ссылку на список спецификаций плагинов
        /// </summary>
        public List<PluginSpec> PluginSpecs { get; private set; }

        /// <summary>
        /// Получить ссылку на словарь спецификаций объектов пользовательского интерфейса, ключ - код типа объекта
        /// </summary>
        public Dictionary<string, UiObjSpec> UiObjSpecs { get; private set; }


        /// <summary>
        /// Очистить данные пользователя
        /// </summary>
        private void ClearUser()
        {
            LoggedOn = false;
            LogonDT = DateTime.MinValue;
            UserProps = null;
            UserRights = null;
            UserMenu = null;
            UserViews = null;
            UserContent = null;
        }

        /// <summary>
        /// Очистить ссылки на объекты общих данных приложения
        /// </summary>
        private void ClearAppDataRefs()
        {
            WebSettings = null;
            ViewSettings = null;
            PluginSpecs = null;
            UiObjSpecs = null;
        }

        /// <summary>
        /// Обновить ссылки на объекты общих данных приложения
        /// </summary>
        private void UpdateAppDataRefs()
        {
            WebSettings = AppData.WebSettings;
            ViewSettings = AppData.ViewSettings;
            PluginSpecs = AppData.PluginSpecs;
            UiObjSpecs = AppData.UiObjSpecs;
        }

        /// <summary>
        /// Вызвать метод OnUserLogin для плагинов
        /// </summary>
        private void RaiseOnUserLogin()
        {
            if (PluginSpecs != null)
            {
                foreach (PluginSpec pluginSpec in PluginSpecs)
                {
                    try
                    {
                        pluginSpec.OnUserLogin(this);
                    }
                    catch (Exception ex)
                    {
                        AppData.Log.WriteException(ex, Localization.UseRussian ?
                            "Ошибка при выполнении действий при входе пользователя в плагине \"{0}\"" :
                            "Error executing actions on user login in the plugin \"{0}\"", pluginSpec.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать метод OnUserLogout для плагинов
        /// </summary>
        private void RaiseOnUserLogout()
        {
            if (PluginSpecs != null)
            {
                foreach (PluginSpec pluginSpec in PluginSpecs)
                {
                    try
                    {
                        pluginSpec.OnUserLogout(this);
                    }
                    catch (Exception ex)
                    {
                        AppData.Log.WriteException(ex, Localization.UseRussian ?
                            "Ошибка при выполнении действий при выходе пользователя в плагине \"{0}\"" :
                            "Error executing actions on user logout in the plugin \"{0}\"", pluginSpec.Name);
                    }
                }
            }
        }


        /// <summary>
        /// Выполнить вход пользователя в систему
        /// </summary>
        /// <remarks>Если пароль равен null, то он не проверяется</remarks>
        public bool Login(string username, string password, out string errMsg)
        {
            username = username == null ? "" : username.Trim();
            int roleID;
            AppData.Refresh();

            if (AppData.CheckUser(username, password, password != null, out roleID, out errMsg))
            {
                LoggedOn = true;
                LogonDT = DateTime.Now;

                // заполнение свойств пользователя
                UserProps = new UserProps();
                UserProps.UserID = AppData.DataAccess.GetUserID(username);
                UserProps.UserName = username;
                UserProps.RoleID = roleID;
                UserProps.RoleName = AppData.DataAccess.GetRoleName(roleID);

                if (password == null)
                {
                    AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                        "Вход в систему без пароля: {0} ({1}). IP-адрес: {2}" :
                        "Login without a password: {0} ({1}). IP address: {2}", 
                        username, UserProps.RoleName, IpAddress));
                }
                else
                {
                    AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                        "Вход в систему: {0} ({1}). IP-адрес: {2}" :
                        "Login: {0} ({1}). IP address: {2}", 
                        username, UserProps.RoleName, IpAddress));
                }

                UserRights userRights = new UserRights();
                userRights.Init(roleID, AppData.DataAccess);
                UserRights = userRights;

                AppData.UserMonitor.AddUser(this);
                UpdateAppDataRefs();
                RaiseOnUserLogin();

                UserMenu = new UserMenu(AppData.Log);
                UserMenu.Init(this);
                UserViews = new UserViews(AppData.Log);
                UserViews.Init(this, AppData.DataAccess);
                UserContent = new UserContent(AppData.Log);
                UserContent.Init(this, AppData.DataAccess);
                return true;
            }
            else
            {
                Logout();
                AppData.Log.WriteError(string.Format(Localization.UseRussian ?
                    "Неудачная попытка входа в систему: {0}{1}. IP-адрес: {2}" :
                    "Unsuccessful login attempt: {0}{1}. IP address: {2}",
                    username == "" ? "" : username + " - ", errMsg.TrimEnd('.'), IpAddress));
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
        /// Выполнить вход пользователя в систему для последующей отладки
        /// </summary>
        public void LoginForDebug()
        {
            AppData.Init(HostingEnvironment.MapPath("~"));
            string errMsg;
            Login("admin", out errMsg);
        }

        /// <summary>
        /// Выполнить выход пользователя из системы
        /// </summary>
        public void Logout()
        {
            if (LoggedOn)
            {
                RaiseOnUserLogout();
                AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                    "Выход из системы: {0}. IP-адрес: {1}" :
                    "Logout: {0}. IP address: {1}", UserProps.UserName, IpAddress));
            }

            ClearUser();
            UpdateAppDataRefs();
        }        

        /// <summary>
        /// Выполнить выход и повторный вход в систему
        /// </summary>
        public bool ReLogin()
        {
            if (LoggedOn)
            {
                string username = UserProps.UserName;
                Logout();
                string errMsg;
                return Login(username, out errMsg);
            }
            else
            {
                return false;
            }
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
                        string returnUrl = HttpUtility.UrlEncode(httpContext.Request.Url.ToString());
                        httpContext.Response.Redirect(alert == "" ?
                            string.Format(UrlTemplates.LoginWithReturn, returnUrl) :
                            string.Format(UrlTemplates.LoginWithAlert, returnUrl, HttpUtility.UrlEncode(alert)));
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