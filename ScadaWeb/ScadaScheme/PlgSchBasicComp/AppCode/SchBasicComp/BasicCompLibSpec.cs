/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : Specification of the basic scheme components library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using Scada.Scheme;
using Scada.Web.Properties;
using System.Collections.Generic;

namespace Scada.Web.Plugins.SchBasicComp
{
    /// <summary>
    /// Specification of the basic scheme components library
    /// <para>Спецификация библиотеки основных компонентов схемы</para>
    /// </summary>
    public class BasicCompLibSpec : CompLibSpec
    {
        /// <summary>
        /// Получить префикс XML-элементов
        /// </summary>
        public override string XmlPrefix
        {
            get
            {
                return "basic";
            }
        }

        /// <summary>
        /// Получить пространство имён XML-элементов
        /// </summary>
        public override string XmlNs
        {
            get
            {
                return "urn:rapidscada:scheme:basic";
            }
        }

        /// <summary>
        /// Получить заголовок группы в редакторе
        /// </summary>
        public override string GroupHeader
        {
            get
            {
                return Localization.UseRussian ? "Основные" : "Basic";
            }
        }

        /// <summary>
        /// Получить ссылки на файлы CSS, которые необходимы для работы компонентов
        /// </summary>
        public override List<string> Styles
        {
            get
            {
                return new List<string>()
                {
                    "SchBasicComp/css/basiccomp.min.css"
                };
            }
        }

        /// <summary>
        /// Получить ссылки на файлы JavaScript, которые необходимы для работы компонентов
        /// </summary>
        public override List<string> Scripts
        {
            get
            {
                return new List<string>()
                {
                    "SchBasicComp/js/basiccomprender.js"
                };
            }
        }


        /// <summary>
        /// Создать элементы списка компонентов
        /// </summary>
        protected override List<CompItem> CreateCompItems()
        {
            return new List<CompItem>()
            {
                new CompItem(Resources.button, typeof(Button)),
                new CompItem(Resources.led, typeof(Led)),
                new CompItem(Resources.link, typeof(Link)),
                new CompItem(Resources.toggle, typeof(Toggle))
            };
        }

        /// <summary>
        /// Создать фабрику компонентов
        /// </summary>
        protected override CompFactory CreateCompFactory()
        {
            return new BasicCompFactory();
        }
    }
}