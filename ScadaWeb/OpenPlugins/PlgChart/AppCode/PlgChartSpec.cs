/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : PlgChart
 * Summary  : Chart plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2017
 */

using Scada.Web.Plugins.Chart;
using Scada.Web.Shell;
using System.Collections.Generic;
using System.IO;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Chart plugin specification
    /// <para>Спецификация плагина графиков</para>
    /// </summary>
    public class PlgChartSpec : PluginSpec
    {
        private DictUpdater dictUpdater; // объект для обновления словаря плагина


        /// <summary>
        /// Конструктор
        /// </summary>
        public PlgChartSpec()
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
                    "Графики" :
                    "Chart";
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
                    "Плагин обеспечивает отображение графиков." :
                    "The plugin provides displaying charts.";
            }
        }

        /// <summary>
        /// Получить версию плагина
        /// </summary>
        public override string Version
        {
            get
            {
                return ChartUtils.ChartVersion;
            }
        }

        /// <summary>
        /// Получить спецификации отчётов, которые реализуются плагином
        /// </summary>
        public override List<ReportSpec> ReportSpecs
        {
            get
            {
                return new List<ReportSpec>() { new MinDataRepSpec() };
            }
        }

        /// <summary>
        /// Получить пути к дополнительным скриптам, которые реализуются плагином
        /// </summary>
        public override ScriptPaths ScriptPaths
        {
            get
            {
                return new ScriptPaths() { ChartScriptPath = "~/plugins/Chart/js/chartdialog.js" };
            }
        }


        /// <summary>
        /// Инициализировать плагин
        /// </summary>
        public override void Init()
        {
            dictUpdater = new DictUpdater(
                string.Format("{0}Chart{1}lang{1}", AppDirs.PluginsDir, Path.DirectorySeparatorChar), 
                "PlgChart", () => { ChartPhrases.Init(); MinDataRepPhrases.Init(); }, Log);
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