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
 * Module   : PlgSchBasicComp
 * Summary  : Basic scheme components plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme;
using Scada.Web.Plugins.SchBasicComp;

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
        internal const string PlgVersion = "5.0.0.0";


        /// <summary>
        /// Получить наименование плагина
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ?
                    "Основные компоненты схем" :
                    "Basic scheme components";
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
    }
}