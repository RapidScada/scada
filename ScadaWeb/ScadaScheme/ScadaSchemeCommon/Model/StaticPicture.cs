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
 * Summary  : Scheme component that represents static picture
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using Scada.Scheme.Model.DataTypes;
using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Drawing.Design;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Scheme component that represents static picture
    /// <para>Компонент схемы, представляющий статический рисунок</para>
    /// </summary>
    [Serializable]
    public class StaticPicture : BaseComponent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public StaticPicture()
        {
            BorderColor = "Gray";
            BorderWidth = 1;
            ImageName = "";
            ImageStretch = ImageStretches.None;
        }


        /// <summary>
        /// Получить или установить наименование изображения
        /// </summary>
        #region Attributes
        [DisplayName("Image"), Category(Categories.Appearance)]
        [Description("The image from the collection of scheme images.")]
        [CM.TypeConverter(typeof(ImageConverter)), CM.Editor(typeof(ImageEditor), typeof(UITypeEditor))]
        [CM.DefaultValue("")]
        #endregion
        public string ImageName { get; set; }

        /// <summary>
        /// Получить или установить растяжение изображения
        /// </summary>
        #region Attributes
        [DisplayName("Image stretch"), Category(Categories.Appearance)]
        [Description("Stretch the image.")]
        [CM.DefaultValue(ImageStretches.None)]
        #endregion
        public ImageStretches ImageStretch { get; set; }

        
        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            ImageName = xmlNode.GetChildAsString("ImageName");
            ImageStretch = xmlNode.GetChildAsEnum<ImageStretches>("ImageStretch");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("ImageName", ImageName);
            xmlElem.AppendElem("ImageStretch", ImageStretch);
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(ImageName);
        }
    }
}
