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
 * Summary  : Defines a font
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using Scada.Scheme.Model.PropertyGrid;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;

namespace Scada.Scheme.Model.DataTypes
{
    /// <summary>
    /// Defines a font
    /// <para>Определяет шрифт</para>
    /// </summary>
    [TypeConverter(typeof(FontConverter))]
    [Editor(typeof(FontEditor), typeof(UITypeEditor))]
    [Serializable]
    public class Font
    {
        /// <summary>
        /// Наименование по умолчанию
        /// </summary>
        public const string DefaultName = "Arial";
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public const int DefaultSize = 12;
        /// <summary>
        /// Шрифт по умолчанию
        /// </summary>
        public static readonly Font Default = new Font();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Font()
        {
            Name = DefaultName;
            Size = DefaultSize;
            Bold = false;
            Italic = false;
            Underline = false;
        }


        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить или установить размер
        /// </summary>
        public int Size { get; set; }
        
        /// <summary>
        /// Получить или установить признак, что шрифт жирный
        /// </summary>
        public bool Bold { get; set; }
        
        /// <summary>
        /// Получить или установить признак, что шрифт курсив
        /// </summary>
        public bool Italic { get; set; }
        
        /// <summary>
        /// Получить или установить признак, что шрифт подчёркнутый
        /// </summary>
        public bool Underline { get; set; }


        /// <summary>
        /// Получить значение дочернего XML-узла в виде шрифта
        /// </summary>
        public static Font GetChildAsFont(XmlNode parentXmlNode, string childNodeName)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);

            if (node == null)
            {
                return null;
            }
            else
            {
                Font font = new Font();
                font.Name = node.GetChildAsString("Name", Default.Name);
                font.Size = node.GetChildAsInt("Size", Default.Size);
                font.Bold = node.GetChildAsBool("Bold", Default.Bold);
                font.Italic = node.GetChildAsBool("Italic", Default.Italic);
                font.Underline = node.GetChildAsBool("Underline", Default.Underline);
                return font;
            }
        }

        /// <summary>
        /// Создать и добавить XML-элемент шрифта
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, Font font)
        {
            if (font == null)
            {
                return null;
            }
            else
            {
                XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
                xmlElem.AppendElem("Name", font.Name);
                xmlElem.AppendElem("Size", font.Size);
                xmlElem.AppendElem("Bold", font.Bold);
                xmlElem.AppendElem("Italic", font.Italic);
                xmlElem.AppendElem("Underline", font.Underline);
                return (XmlElement)parentXmlElem.AppendChild(xmlElem);
            }
        }

        /// <summary>
        /// Клонировать объект
        /// </summary>
        public Font Clone()
        {
            return (Font)ScadaUtils.DeepClone(this);
        }
    }
}
