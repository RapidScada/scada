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
 * Summary  : Size of a popup
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Web.Plugins.SchBasicComp
{
    /// <summary>
    /// Size of a popup
    /// <para>Размер всплывающего окна</para>
    /// </summary>
    [CM.TypeConverter(typeof(PopupSizeConverter))]
    [Serializable]
    public struct PopupSize
    {
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public static readonly PopupSize Default = new PopupSize(PopupWidth.Normal, 300);


        /// <summary>
        /// Конструктор
        /// </summary>
        public PopupSize(PopupWidth width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }


        /// <summary>
        /// Получить или установить ширину
        /// </summary>
        [DisplayName("Width")]
        public PopupWidth Width { get; set; }

        /// <summary>
        /// Получить или установить высоту
        /// </summary>
        [DisplayName("Height")]
        public int Height { get; set; }


        /// <summary>
        /// Получить значение дочернего XML-узла в виде размера
        /// </summary>
        public static PopupSize GetChildAsSize(XmlNode parentXmlNode, string childNodeName)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ?
                Default :
                new PopupSize(node.GetChildAsEnum<PopupWidth>("Width"), node.GetChildAsInt("Height"));
        }

        /// <summary>
        /// Создать и добавить XML-элемент размера
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, PopupSize popupSize)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            xmlElem.AppendElem("Width", popupSize.Width);
            xmlElem.AppendElem("Height", popupSize.Height);
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }
    }
}
