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
 * Module   : PlgScheme
 * Summary  : Scheme plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System.Collections.Generic;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Scheme plugin specification
    /// <para>Спецификация плагина схем</para>
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
                return "0.0.0.1";
            }
        }

        /// <summary>
        /// Получить спецификации представлений, которые реализуются плагином
        /// </summary>
        public override List<ViewSpec> ViewSpecs
        {
            get
            {
                return new List<ViewSpec>() { new SchemeSpec() };
            }
        }
    }
}