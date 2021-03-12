/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Summary  : Snapshot of application user data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Data.Models;
using Scada.Web.Plugins;
using Scada.Web.Shell;
using System;
using System.Collections.Generic;

namespace Scada.Web
{
    /// <summary>
    /// Snapshot of application user data.
    /// <para>Снимок данных пользователя приложения.</para>
    /// </summary>
    public class UserShot
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        protected UserShot()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserShot(UserShot source)
        {
            IpAddress = source.IpAddress;
            SessionID = source.SessionID;
            LoggedOn = source.LoggedOn;
            LogonDT = source.LogonDT;
            StartPage = source.StartPage;
            UserProps = source.UserProps;
            UserRights = source.UserRights;
            UserMenu = source.UserMenu;
            UserViews = source.UserViews;
            UserContent = source.UserContent;

            WebSettings = source.WebSettings;
            ViewSettings = source.ViewSettings;
            PluginSpecs = source.PluginSpecs;
            UiObjSpecs = source.UiObjSpecs;
        }


        /// <summary>
        /// Получить IP-адрес пользователя
        /// </summary>
        public string IpAddress { get; protected set; }

        /// <summary>
        /// Получить идентификатор сессии
        /// </summary>
        public string SessionID { get; protected set; }

        /// <summary>
        /// Получить признак, выполнен ли вход пользователя в систему
        /// </summary>
        public bool LoggedOn { get; protected set; }

        /// <summary>
        /// Получить дату и время входа пользователя в систему
        /// </summary>
        public DateTime LogonDT { get; protected set; }

        /// <summary>
        /// Gets or sets the start page for the current user.
        /// </summary>
        public string StartPage { get; set; }

        /// <summary>
        /// Получить свойства пользователя
        /// </summary>
        /// <remarks>Объект пересоздаётся после входа в систему</remarks>
        public UserProps UserProps { get; protected set; }

        /// <summary>
        /// Получить права пользователя
        /// </summary>
        /// <remarks>Объект пересоздаётся после входа в систему.
        /// Объект после инициализации не изменяется экземпляром данного класса и не должен изменяться извне,
        /// таким образом, чтение его данных является потокобезопасным</remarks>
        public UserRights UserRights { get; protected set; }

        /// <summary>
        /// Получить меню пользователя
        /// </summary>
        public UserMenu UserMenu { get; protected set; }

        /// <summary>
        /// Получить представления пользователя
        /// </summary>
        public UserViews UserViews { get; protected set; }

        /// <summary>
        /// Получить контент пользователя
        /// </summary>
        public UserContent UserContent { get; protected set; }


        /// <summary>
        /// Получить ссылку на настройки веб-приложения
        /// </summary>
        public WebSettings WebSettings { get; protected set; }

        /// <summary>
        /// Получить ссылку на настройки представлений
        /// </summary>
        public ViewSettings ViewSettings { get; protected set; }

        /// <summary>
        /// Получить ссылку на список спецификаций плагинов
        /// </summary>
        public List<PluginSpec> PluginSpecs { get; protected set; }

        /// <summary>
        /// Получить ссылку на словарь спецификаций объектов пользовательского интерфейса, ключ - код типа объекта
        /// </summary>
        public Dictionary<string, UiObjSpec> UiObjSpecs { get; protected set; }
    }
}