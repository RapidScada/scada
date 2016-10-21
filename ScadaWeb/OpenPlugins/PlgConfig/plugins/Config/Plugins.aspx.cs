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
 * Summary  : Plugins web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.UI;
using System;
using System.Collections.Generic;

namespace Scada.Web.Plugins.Config
{
    /// <summary>
    /// Plugins web form
    /// <para>Веб-форма плагинов</para>
    /// </summary>
    public partial class WFrmPlugins : System.Web.UI.Page
    {
        /// <summary>
        /// Состояния плагинов
        /// </summary>
        protected enum PlaginStates
        {
            /// <summary>
            /// Не активен
            /// </summary>
            Inactive,
            /// <summary>
            /// Активен
            /// </summary>
            Active,
            /// <summary>
            /// Активен, но не загружен
            /// </summary>
            ActiveNotLoaded
        }

        /// <summary>
        /// Элемент списка плагинов
        /// </summary>
        protected class PluginItem
        {
            /// <summary>
            /// Получить или установить состояние плагина
            /// </summary>
            public PlaginStates State { get; set; }
            /// <summary>
            /// Получить или установить наименование плагина
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить описание плагина
            /// </summary>
            public string Descr { get; set; }
            /// <summary>
            /// Получить или установить короткое имя файла библиотеки плагина
            /// </summary>
            public string FileName { get; set; }
        }


        private AppData appData;   // общие данные веб-приложения
        private UserData userData; // данные пользователя приложения


        /// <summary>
        /// Получить список плагинов
        /// </summary>
        private List<PluginItem> GetPluginItems()
        {
            List<PluginItem> pluginItems = new List<PluginItem>();

            foreach (PluginSpec pluginSpec in userData.PluginSpecs)
            {
                pluginItems.Add(new PluginItem()
                {
                    State = PlaginStates.Active,
                    Name = pluginSpec.Name,
                    Descr = pluginSpec.Descr,
                    FileName = ""
                });
            }

            return pluginItems;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // проверка входа в систему и прав
            userData.CheckLoggedOn(true);

            if (!userData.UserRights.ConfigRight)
                throw new ScadaException(CommonPhrases.NoRights);

            if (!IsPostBack)
            {
                // перевод веб-страницы
                Translator.TranslatePage(Page, "Scada.Web.Plugins.Config.WFrmPlugins");

                // построение списка плагинов
                repPlugins.DataSource = GetPluginItems();
                repPlugins.DataBind();
            }
        }
    }
}