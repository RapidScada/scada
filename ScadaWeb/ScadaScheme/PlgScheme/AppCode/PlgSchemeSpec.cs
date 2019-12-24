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
 * Module   : PlgScheme
 * Summary  : Scheme plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Scheme;
using Scada.Web.Plugins.Scheme;
using System.Collections.Generic;
using System.IO;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Scheme plugin specification
    /// <para>Спецификация плагина схем</para>
    /// </summary>
    public class PlgSchemeSpec : PluginSpec
    {
        private DictUpdater schemeDictUpdater; // объект для обновления словаря схем
        private DictUpdater pluginDictUpdater; // объект для обновления словаря плагина


        /// <summary>
        /// Конструктор
        /// </summary>
        public PlgSchemeSpec()
            : base()
        {
            schemeDictUpdater = null;
            pluginDictUpdater = null;
        }


        /// <summary>
        /// Получить наименование плагина
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ?
                    "Схемы" :
                    "Schemes";
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
                    "Плагин обеспечивает отображение мнемосхем." :
                    "The plugin provides displaying schemes.";
            }
        }

        /// <summary>
        /// Получить версию плагина
        /// </summary>
        public override string Version
        {
            get
            {
                return SchemeUtils.SchemeVersion;
            }
        }

        /// <summary>
        /// Получить спецификации представлений, которые реализуются плагином
        /// </summary>
        public override List<ViewSpec> ViewSpecs
        {
            get
            {
                return new List<ViewSpec>() { new SchemeViewSpec() };
            }
        }


        /// <summary>
        /// Инициализировать плагин
        /// </summary>
        public override void Init()
        {
            // создание объектов для обновления словарей
            string dir = Path.Combine(AppDirs.PluginsDir, "Scheme", "lang");
            schemeDictUpdater = new DictUpdater(dir, "ScadaScheme", null, Log);
            pluginDictUpdater = new DictUpdater(dir, "PlgScheme", SchemePhrases.Init, Log);

            // инициализация контекста схем и менеджера компонентов
            AppDirs appDirs = AppData.GetAppData().AppDirs;
            SchemeContext.GetInstance().Init(appDirs);
            CompManager.GetInstance().Init(appDirs, Log);
        }

        /// <summary>
        /// Выполнить действия после успешного входа пользователя в систему
        /// </summary>
        public override void OnUserLogin(UserData userData)
        {
            // обновление словарей
            schemeDictUpdater.Update();
            pluginDictUpdater.Update();

            // извлечение компонентов из плагинов
            CompManager.GetInstance().RetrieveCompFromPlugins(userData.PluginSpecs);
        }
    }
}