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
 * Module   : ScadaSchemeCommon
 * Summary  : Item of the component list in the editor
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using System;
using System.Drawing;

namespace Scada.Scheme
{
    /// <summary>
    /// Item of the component list in the editor
    /// <para>Элемент списка компонентов в редакторе</para>
    /// </summary>
    public class CompItem
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CompItem(Image icon, Type compType)
            : this(icon, GetDisplayName(compType), compType)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CompItem(Image icon, string displayName, Type compType)
        {
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentException("Display name must not be empty.", "displayName");
            if (compType == null)
                throw new ArgumentNullException("compType");

            Icon = icon;
            DisplayName = displayName;
            CompType = compType;
        }


        /// <summary>
        /// Получить иконку, отображаемую в редакторе
        /// </summary>
        public Image Icon { get; }

        /// <summary>
        /// Получить наименование, отображаемое в редакторе
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Получить тип компонента
        /// </summary>
        public Type CompType { get; }


        /// <summary>
        /// Получить отображаемое наименование из словарей
        /// </summary>
        private static string GetDisplayName(Type compType)
        {
            Localization.Dict dict;
            return Localization.Dictionaries.TryGetValue(compType.FullName, out dict) ?
                dict.GetPhrase("DisplayName", compType.Name) :
                compType.Name;
        }
    }
}
