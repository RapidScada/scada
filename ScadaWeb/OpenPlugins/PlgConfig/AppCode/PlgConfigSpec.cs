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
 * Module   : PlgConfig
 * Summary  : Configuration plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Web.Plugins.Config;
using Scada.Web.Shell;
using System.Collections.Generic;
using System.IO;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Configuration plugin specification
    /// <para>Спецификация плагина конфигурации</para>
    /// </summary>
    public class PlgConfigSpec : PluginSpec
    {
        /// <summary>
        /// Версия плагина
        /// </summary>
        internal const string PlgVersion = "5.0.1.0";

        private DictUpdater dictUpdater; // объект для обновления словаря плагина

        
        /// <summary>
        /// Конструктор
        /// </summary>
        public PlgConfigSpec()
            : base()
        {
            dictUpdater = null;
        }


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
                    "Плагин позволяет конфигурировать веб-приложение через браузер." :
                    "The plugin allows to configure the web application using browser.";
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
        /// Инициализировать плагин
        /// </summary>
        public override void Init()
        {
            dictUpdater = new DictUpdater(
                string.Format("{0}Config{1}lang{1}", AppDirs.PluginsDir, Path.DirectorySeparatorChar),
                "PlgConfig", PlgPhrases.Init, Log);
        }

        /// <summary>
        /// Выполнить действия после успешного входа пользователя в систему
        /// </summary>
        public override void OnUserLogin(UserData userData)
        {
            dictUpdater.Update();
        }

        /// <summary>
        /// Получить элементы меню, доступные пользователю
        /// </summary>
        public override List<MenuItem> GetMenuItems(UserData userData)
        {
            if (userData.LoggedOn)
            {
                List<MenuItem> menuItems = new List<MenuItem>();

                // элемент меню для конфигурации веб-приложения
                if (userData.UserRights.ConfigRight)
                {
                    MenuItem configMenuItem = MenuItem.FromStandardMenuItem(StandardMenuItems.Config);
                    configMenuItem.Subitems.Add(new MenuItem(PlgPhrases.WebConfigMenuItem,
                        "~/plugins/Config/WebConfig.aspx"));
                    menuItems.Add(configMenuItem);
                }

                // элемент меню для просмотра или управления установленными плагинами
                MenuItem pluginsMenuItem = MenuItem.FromStandardMenuItem(StandardMenuItems.Plugins);
                pluginsMenuItem.Subitems.Add(new MenuItem(PlgPhrases.InstalledPluginsMenuItem,
                    "~/plugins/Config/Plugins.aspx"));
                menuItems.Add(pluginsMenuItem);

                return menuItems;
            }
            else
            {
                return null;
            }
        }
    }
}