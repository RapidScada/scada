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
 * Summary  : Size in two-dimensional space
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model.DataTypes
{
    /// <summary>
    /// Size in two-dimensional space
    /// <para>Размер в двумерном пространстве</para>
    /// </summary>
    [CM.TypeConverter(typeof(SizeConverter))]
    [Serializable]
    public struct Size
    {
        /// <summary>
        /// Нулевой размер
        /// </summary>
        public static readonly Size Zero = new Size(0, 0);
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public static readonly Size Default = new Size(100, 100);


        /// <summary>
        /// Конструктор
        /// </summary>
        public Size(int width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }


        /// <summary>
        /// Получить или установить ширину
        /// </summary>
        [DisplayName("Width")]
        public int Width { get; set; }

        /// <summary>
        /// Получить или установить высоту
        /// </summary>
        [DisplayName("Height")]
        public int Height { get; set; }


        /// <summary>
        /// Получить значение дочернего XML-узла в виде размера
        /// </summary>
        public static Size GetChildAsSize(XmlNode parentXmlNode, string childNodeName, Size? defaultSize = null)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ?
                (defaultSize ?? Default) :
                new Size(node.GetChildAsInt("Width"), node.GetChildAsInt("Height"));
        }

        /// <summary>
        /// Создать и добавить XML-элемент размера
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, Size size)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            xmlElem.AppendElem("Width", size.Width);
            xmlElem.AppendElem("Height", size.Height);
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }
    }
}
