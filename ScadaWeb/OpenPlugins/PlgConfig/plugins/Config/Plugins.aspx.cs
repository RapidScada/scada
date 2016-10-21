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
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;

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
            /// Не удалось загрузить
            /// </summary>
            NotLoaded
        }

        /// <summary>
        /// Элемент списка плагинов
        /// </summary>
        protected class PluginItem : IComparable<PluginItem>
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PluginItem(string name, string descr, string fileName, PlaginStates state)
            {
                State = state;
                Name = name;
                Descr = descr;
                FileName = fileName;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public PluginItem(PluginSpec pluginSpec, PlaginStates state)
            {
                State = state;
                Name = pluginSpec.Name;
                Descr = pluginSpec.Descr;
                FileName = Path.GetFileName(Assembly.GetAssembly(pluginSpec.GetType()).Location);
            }

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
            /// <summary>
            /// Получить или установить состояние плагина
            /// </summary>
            public PlaginStates State { get; set; }

            /// <summary>
            /// Сравнить текущий объект с другим объектом такого же типа
            /// </summary>
            public int CompareTo(PluginItem other)
            {
                return Name.CompareTo(other.Name);
            }
        }


        private AppData appData;   // общие данные веб-приложения
        private UserData userData; // данные пользователя приложения


        /// <summary>
        /// Получить все доступные плагины
        /// </summary>
        private List<PluginItem> GetAllPlugins()
        {
            List<PluginItem> pluginItems = new List<PluginItem>();
            HashSet<string> existingFileNames = GetExistingFileNames();
            HashSet<string> settingsFileNames = GetSettingsFileNames();
            HashSet<string> allFileNames = new HashSet<string>();
            allFileNames.UnionWith(existingFileNames);
            allFileNames.UnionWith(settingsFileNames);
            Dictionary<string, PluginItem> activePlugins = GetActivePlugins();

            foreach (string fileName in allFileNames)
            {
                PluginItem pluginItem;

                if (activePlugins.TryGetValue(fileName, out pluginItem))
                {
                    // добавление активного плагина
                    pluginItems.Add(pluginItem);
                }
                else if (settingsFileNames.Contains(fileName))
                {
                    // добавление активного, но не загруженного плагина
                    pluginItems.Add(new PluginItem(fileName, "", fileName, PlaginStates.NotLoaded));
                }
                else
                {
                    // загрузка спецификации неактивного плагина
                    string errMsg;
                    PluginSpec pluginSpec = PluginSpec.CreateFromDll(appData.AppDirs.BinDir + fileName, out errMsg);

                    if (pluginSpec == null)
                        appData.Log.WriteError(errMsg);
                    else
                        pluginItems.Add(new PluginItem(pluginSpec, PlaginStates.Inactive));
                }
            }

            pluginItems.Sort();
            return pluginItems;
        }

        /// <summary>
        /// Получить имена существующих файлов библиотек плагинов
        /// </summary>
        private HashSet<string> GetExistingFileNames()
        {
            HashSet<string> fileNameSet = new HashSet<string>();
            DirectoryInfo dirInfo = new DirectoryInfo(appData.AppDirs.BinDir);
            FileInfo[] fileInfoArr = dirInfo.GetFiles("plg*.dll", SearchOption.TopDirectoryOnly);

            foreach (FileInfo fileInfo in fileInfoArr)
            {
                string fileName = fileInfo.Name;
                if (!fileName.EndsWith("common.dll", StringComparison.OrdinalIgnoreCase))
                    fileNameSet.Add(fileName);
            }

            return fileNameSet;
        }

        /// <summary>
        /// Получить имена файлов библиотек плагинов из настроек
        /// </summary>
        private HashSet<string> GetSettingsFileNames()
        {
            HashSet<string> fileNameSet = new HashSet<string>();
            fileNameSet.UnionWith(userData.WebSettings.PluginFileNames);
            return fileNameSet;
        }

        /// <summary>
        /// Получить словарь активных плагинов, ключ - имя файла библиотеки
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, PluginItem> GetActivePlugins()
        {
            Dictionary<string, PluginItem> activePlugins = new Dictionary<string, PluginItem>();

            foreach (PluginSpec pluginSpec in userData.PluginSpecs)
            {
                PluginItem pluginItem = new PluginItem(pluginSpec, PlaginStates.Active);
                activePlugins[pluginItem.FileName] = pluginItem;
            }

            return activePlugins;
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
                repPlugins.DataSource = GetAllPlugins();
                repPlugins.DataBind();
            }
        }

        protected void repPlugins_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // настройка кнопок активации и деактивации
            PluginItem pluginItem = e.Item.DataItem as PluginItem;

            if (pluginItem != null)
            {
                if (pluginItem.State == PlaginStates.Inactive)
                {
                    Button btnDeactivate = (Button)e.Item.FindControl("btnDeactivate");
                    btnDeactivate.Visible = false;
                }
                else
                {
                    Button btnActivate = (Button)e.Item.FindControl("btnActivate");
                    btnActivate.Visible = false;
                }
            }
        }

        protected void repPlugins_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}