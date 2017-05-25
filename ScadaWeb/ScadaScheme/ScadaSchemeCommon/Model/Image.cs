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
 * Summary  : Image of a scheme
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.PropertyGrid;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Image of a scheme
    /// <para>Изображение схемы</para>
    /// </summary>
    [TypeConverter(typeof(ImageConverter))]
    [Editor(typeof(ImageEditor), typeof(UITypeEditor))]
    public class Image
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Image()
        {
            Name = "";
            Data = null;
        }


        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить или установить данные
        /// </summary>
        public byte[] Data { get; set; }


        /// <summary>
        /// Загрузить изображение из XML-узла
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            Name = xmlNode.GetChildAsString("Name");
            Data = Convert.FromBase64String(xmlNode.GetChildAsString("Data"));
        }

        /// <summary>
        /// Сохранить изображение в XML-узле
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("Data", 
                Data != null && Data.Length > 0 ? 
                Convert.ToBase64String(Data, Base64FormattingOptions.None) : "");
        }

        /// <summary>
        /// Копировать объект
        /// </summary>
        public Image Copy()
        {
            return new Image()
            {
                Name = Name,
                Data = Data
            };
        }
    }
}
