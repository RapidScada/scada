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
 * Summary  : Unknown scheme component
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Web.Script.Serialization;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Unknown scheme component
    /// <para>Неизвестный компонент схемы</para>
    /// </summary>
    /// <remarks>Stores the properties of the component that failed to load
    /// <para>Сохраняет свойства компонента, который не удалось загрузить</para></remarks>
    [Serializable]
    public sealed class UnknownComponent : BaseComponent
    {
        /// <summary>
        /// Получить или установить XML-узел, содержащий свойства компонента
        /// </summary>
        [CM.Browsable(false), ScriptIgnore]
        public XmlNode XmlNode { get; set; }

        /// <summary>
        /// Получить наименование XML-узла
        /// </summary>
        #region Attributes
        [DisplayName("XML node"), Category(Categories.Design)]
        [Description("The XML node that contains component properties in the scheme file.")]
        #endregion
        public string XmlNodeName
        {
            get
            {
                return XmlNode == null ? "" : XmlNode.Name;
            }
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(XmlNodeName);
        }
    }
}
