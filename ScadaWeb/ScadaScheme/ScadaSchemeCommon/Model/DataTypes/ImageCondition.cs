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
 * Summary  : Condition that defines an image
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Drawing.Design;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model.DataTypes
{
    /// <summary>
    /// Condition that defines an image
    /// <para>Условие, которое определяет изображение</para>
    /// </summary>
    [Serializable]
    public class ImageCondition : Condition
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ImageCondition()
            : base()
        {
            ImageName = "";
        }


        /// <summary>
        /// Получить или установить наименование изображения, отображаемого при выполнении условия
        /// </summary>
        #region Attributes
        [DisplayName("Image"), Category(Categories.Appearance)]
        [CM.TypeConverter(typeof(ImageConverter)), CM.Editor(typeof(ImageEditor), typeof(UITypeEditor))]
        [CM.DefaultValue("")]
        #endregion
        public string ImageName { get; set; }
        
        
        /// <summary>
        /// Загрузить условие из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            ImageName = xmlNode.GetChildAsString("ImageName");
        }

        /// <summary>
        /// Сохранить условие в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("ImageName", ImageName);
        }
    }
}
