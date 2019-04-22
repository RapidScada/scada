/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : PlgMonitor
 * Summary  : Monitor plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Web.Shell;
using System.Collections.Generic;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Monitor plugin specification
    /// <para>Спецификация плагина мониторинга</para>
    /// </summary>
    public class PlgMonitorSpec : PluginSpec
    {
        /// <summary>
        /// Версия плагина
        /// </summary>
        internal const string PlgVersion = "5.0.0.0";


        /// <summary>
        /// Получить наименование плагина
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ?
                    "Монитор" :
                    "Monitor";
            }
        }

        /// <summary>
        /// Получить описание плагина
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ?
                    "Плагин показывает состояние работы веб-приложения." :
                    "The plugin displays the web application state.";
            }
        }

        /// <summary>
        /// Получить версию плагина
        /// </summary>
        public override string Version
        {
            get
            {
                return PlgVersion;
            }
        }


        /// <summary>
        /// Получить элементы меню, доступные пользователю
        /// </summary>
        public override List<MenuItem> GetMenuItems(UserData userData)
        {
            if (userData.UserRights.ConfigRight)
            {
                List<MenuItem> menuItems = new List<MenuItem>();

                MenuItem adminMenuItem = MenuItem.FromStandardMenuItem(StandardMenuItems.Admin);
                adminMenuItem.Subitems.Add(new MenuItem("Active Users", "~/plugins/Monitor/ActiveUsers.aspx"));
                adminMenuItem.Subitems.Add(new MenuItem("Cache State", "~/plugins/Monitor/CacheState.aspx"));
                menuItems.Add(adminMenuItem);

                return menuItems;
            }
            else
            {
                return null;
            }
        }
    }
}