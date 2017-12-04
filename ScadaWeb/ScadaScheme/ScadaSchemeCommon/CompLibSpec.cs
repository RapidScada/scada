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
 * Module   : ScadaSchemeCommon
 * Summary  : The base class for scheme component library specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System.Collections.Generic;

namespace Scada.Scheme
{
    /// <summary>
    /// The base class for scheme component library specification
    /// <para>Родительский класс спецификации библиотеки компонентов схем</para>
    /// </summary>
    public abstract class CompLibSpec
    {
        /// <summary>
        /// Элементы списка компонентов в редакторе
        /// </summary>
        protected List<CompItem> compItems;
        /// <summary>
        /// Фабрика для создания компонентов
        /// </summary>
        protected CompFactory compFactory;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CompLibSpec()
        {
            compItems = null;
            compFactory = null;
        }


        /// <summary>
        /// Получить префикс XML-элементов
        /// </summary>
        public abstract string XmlPrefix { get; }

        /// <summary>
        /// Получить пространство имён XML-элементов
        /// </summary>
        public abstract string XmlNs { get; }

        /// <summary>
        /// Получить заголовок группы в редакторе
        /// </summary>
        public abstract string GroupHeader { get; }

        /// <summary>
        /// Получить элементы списка компонентов в редакторе
        /// </summary>
        public List<CompItem> CompItems
        {
            get
            {
                if (compItems == null)
                    compItems = CreateCompItems();
                return compItems;
            }
        }

        /// <summary>
        /// Получить фабрику для создания компонентов
        /// </summary>
        public CompFactory CompFactory
        {
            get
            {
                if (compFactory == null)
                    compFactory = CreateCompFactory();
                return compFactory;
            }
        }

        /// <summary>
        /// Получить ссылки на файлы CSS, которые необходимы для работы компонентов
        /// </summary>
        /// <remarks>Путь указывается относительно директории плагинов</remarks>
        public virtual List<string> Styles
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получить ссылки на файлы JavaScript, которые необходимы для работы компонентов
        /// </summary>
        /// <remarks>Путь указывается относительно директории плагинов</remarks>
        public virtual List<string> Scripts
        {
            get
            {
                return null;
            }
        }


        /// <summary>
        /// Создать элементы списка компонентов
        /// </summary>
        protected abstract List<CompItem> CreateCompItems();

        /// <summary>
        /// Создать фабрику компонентов
        /// </summary>
        protected abstract CompFactory CreateCompFactory();

        /// <summary>
        /// Проверить, что библиотека компонентов пригодна к использованию
        /// </summary>
        public virtual bool Validate(out string errMsg)
        {
            errMsg = "";
            return 
                !string.IsNullOrEmpty(XmlPrefix) && 
                !string.IsNullOrEmpty(XmlNs) &&
                !string.IsNullOrEmpty(GroupHeader) &&
                CompItems != null && CompItems.Count > 0 &&
                CompFactory != null;
        }
    }
}
