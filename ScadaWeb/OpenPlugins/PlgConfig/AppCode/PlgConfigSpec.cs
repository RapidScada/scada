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
 * Module   : PlgConfig
 * Summary  : Configuration plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Web.Shell;
using System.Collections.Generic;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Configuration plugin specification
    /// <para>Спецификация плагина конфигурации</para>
    /// </summary>
    public class PlgConfigSpec : PluginSpec
    {
        /// <summary>
        /// Получить наименование плагина
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ?
                    "Конфигуратор" :
                    "Configurator";
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
                    "Плагин позволяет конфигурировать Rapid SCADA через веб-интерфейс." :
                    "The plugin allows to configure Rapid SCADA using web interface.";
            }
        }

        /// <summary>
        /// Получить версию плагина
        /// </summary>
        public override string Version
        {
            get
            {
                return "0.0.0.1";
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
                adminMenuItem.Subitems.Add(new MenuItem("Active users", "~/plugins/Config/ActiveUsers.aspx"));
                adminMenuItem.Subitems.Add(new MenuItem("Cache state", "~/plugins/Config/CacheState.aspx"));
                menuItems.Add(adminMenuItem);

                MenuItem configMenuItem = MenuItem.FromStandardMenuItem(StandardMenuItems.Config);
                configMenuItem.Subitems.Add(new MenuItem("Web application", "~/plugins/Config/WebConfig.aspx"));
                menuItems.Add(configMenuItem);

                return menuItems;
            }
            else
            {
                return null;
            }
        }
    }
}