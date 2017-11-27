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
 * Summary  : The base class for component library specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System.Collections.Generic;

namespace Scada.Scheme
{
    /// <summary>
    /// The base class for component library specification
    /// <para>Родительский класс спецификации библиотеки компонентов</para>
    /// </summary>
    public abstract class CompLibSpec
    {
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
        /// Получить спецификации компонентов, которые содержит библиотека
        /// </summary>
        public abstract List<CompSpec> CompSpecs { get; }

        /// <summary>
        /// Получить фабрику для создания компонентов
        /// </summary>
        public abstract CompFactory CompFactory { get; }
    }
}
