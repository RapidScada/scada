/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : PlgSchBasicComp
 * Summary  : Basic scheme components plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2020
 */

using Scada.Scheme;
using Scada.Scheme.Model.PropertyGrid;
using Scada.Web.Plugins.SchBasicComp;
using System.IO;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Basic scheme components plugin specification
    /// <para>Спецификация плагина основных компонентов схем</para>
    /// </summary>
    public class PlgSchBasicCompSpec : PluginSpec, ISchemeComp
    {
        /// <summary>
        /// Версия плагина
        /// </summary>
        internal const string PlgVersion = "5.1.1.0";

        
        /// <summary>
        /// Получить наименование плагина
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ?
                    "Основные компоненты схем" :
                    "Basic Scheme Components";
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
                    "Набор основных компонентов для отображения на мнемосхемах." :
                    "A set of basic components for display on schemes.";
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
        /// Получить спецификацию библиотеки компонентов
        /// </summary>
        CompLibSpec ISchemeComp.CompLibSpec
        {
            get
            {
                return new BasicCompLibSpec();
            }
        }

        
        /// <summary>
        /// Инициализировать плагин
        /// </summary>
        public override void Init()
        {
            if (SchemeContext.GetInstance().EditorMode)
            {
                // загрузка словарей
                if (!Localization.LoadDictionaries(Path.Combine(AppDirs.PluginsDir, "SchBasicComp", "lang"),
                    "PlgSchBasicComp", out string errMsg))
                {
                    Log.WriteError(errMsg);
                }

                // перевод атрибутов классов, которые используются при редактировании, но не являются компонентами схем
                AttrTranslator attrTranslator = new AttrTranslator();
                attrTranslator.TranslateAttrs(typeof(ColorCondition));
                attrTranslator.TranslateAttrs(typeof(PopupSize));
            }
        }
    }
}
