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
using Scada.Client;
using Utils;
using Scada.Web.Plugins;

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
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private UserData()
        {
            Clear();
        }


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
        /// Получить IP-адрес пользователя
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// Получить права пользователя
        /// </summary>
        public UserRights UserRights { get; private set; }


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
        private void Clear()
        {
            UserName = "";
            UserID = 0;
            RoleName = "";
            LoggedOn = false;
            LogonDT = DateTime.MinValue;
            IpAddress = "";
            UserRights = null;

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
        public bool Login(string username)
        {
            string errMsg;
            return Login(username, null, out errMsg);
        }

        /// <summary>
        /// Выполнить выход пользователя из системы
        /// </summary>
        public void Logout()
        {
            Clear();
            UpdateAppDataRefs();
        }


        /// <summary>
        /// Получить данные пользователя приложения
        /// </summary>
        /// <remarks>Для веб-приложения данные пользователя сохраняются в сессии</remarks>
        public static UserData GetUserData()
        {
            ScadaWebUtils.CheckHttpContext();
            HttpSessionState session = HttpContext.Current.Session;
            UserData userData = session["UserData"] as UserData;

            if (userData == null)
            {
                // обновление данных веб-приложения
                AppData.Refresh();

                // создание данных пользователя
                userData = new UserData();
                session.Add("UserData", userData);

                // получение IP-адреса
                userData.IpAddress = HttpContext.Current.Request.UserHostAddress;

                // обновление ссылок на объекты общих данных приложения
                userData.UpdateAppDataRefs();
            }

            return userData;
        }
    }
}