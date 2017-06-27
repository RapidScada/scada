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
 * Summary  : Point in a two-dimensional plane
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.PropertyGrid;
using System;
using System.ComponentModel;
using System.Xml;

namespace Scada.Scheme.Model.DataTypes
{
    /// <summary>
    /// Point in a two-dimensional plane
    /// <para>Точка в двумерной плоскости</para>
    /// </summary>
    [TypeConverter(typeof(PointConverter))]
    [Serializable]
    public struct Point
    {
        /// <summary>
        /// Точка по умолчанию
        /// </summary>
        public static readonly Point Default = new Point(0, 0);


        /// <summary>
        /// Конструктор
        /// </summary>
        public Point(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }


        /// <summary>
        /// Получить или установить координату X
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Получить или установить координату Y
        /// </summary>
        public int Y { get; set; }


        /// <summary>
        /// Получить значение дочернего XML-узла в виде точки
        /// </summary>
        public static Point GetChildAsPoint(XmlNode parentXmlNode, string childNodeName)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ?
                Default :
                new Point(node.GetChildAsInt("X"), node.GetChildAsInt("Y"));
        }

        /// <summary>
        /// Создать и добавить XML-элемент точки
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, Point point)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            xmlElem.AppendElem("X", point.X);
            xmlElem.AppendElem("Y", point.Y);
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }
    }
}
