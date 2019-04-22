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
 * Module   : PlgTable
 * Summary  : Table plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Table;
using Scada.Web.Plugins.Table;
using Scada.Web.Shell;
using System.Collections.Generic;
using System.IO;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Table plugin specification
    /// <para>Спецификация табличного плагина</para>
    /// </summary>
    public class PlgTableSpec : PluginSpec
    {
        private DictUpdater dictUpdater; // объект для обновления словаря плагина


        /// <summary>
        /// Конструктор
        /// </summary>
        public PlgTableSpec()
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
                    "Таблицы" :
                    "Tables";
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
                    "Плагин обеспечивает отображение табличных представлений и событий." :
                    "The plugin provides displaying table views and events.";
            }
        }

        /// <summary>
        /// Получить версию плагина
        /// </summary>
        public override string Version
        {
            get
            {
                return TableUtils.TableVersion;
            }
        }

        /// <summary>
        /// Получить спецификации представлений, которые реализуются плагином
        /// </summary>
        public override List<ViewSpec> ViewSpecs
        {
            get
            {
                return new List<ViewSpec>() { new TableViewSpec() };
            }
        }

        /// <summary>
        /// Получить спецификации окон данных, которые реализуются плагином
        /// </summary>
        public override List<DataWndSpec> DataWndSpecs
        {
            get
            {
                return new List<DataWndSpec>() { new EventsWndSpec()/*, new SampleWndSpec()*/ };
            }
        }

        /// <summary>
        /// Получить пути к дополнительным скриптам, которые реализуются плагином
        /// </summary>
        public override ScriptPaths ScriptPaths
        {
            get
            {
                return new ScriptPaths()
                {
                    CmdScriptPath = "~/plugins/Table/js/cmddialog.js",
                    EventAckScriptPath = "~/plugins/Table/js/eventackdialog.js"
                };
            }
        }


        /// <summary>
        /// Инициализировать плагин
        /// </summary>
        public override void Init()
        {
            dictUpdater = new DictUpdater(
                string.Format("{0}Table{1}lang{1}", AppDirs.PluginsDir, Path.DirectorySeparatorChar), 
                "ScadaTable", TablePhrases.Init, Log);
        }

        /// <summary>
        /// Выполнить действия после успешного входа пользователя в систему
        /// </summary>
        public override void OnUserLogin(UserData userData)
        {
            dictUpdater.Update();
        }
    }
}
