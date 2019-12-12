/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : Condition that defines a color
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Scheme.Model.DataTypes;
using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Drawing.Design;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Web.Plugins.SchBasicComp
{
    /// <summary>
    /// Condition that defines a color.
    /// <para>Условие, которое определяет цвет.</para>
    /// </summary>
    [Serializable]
    public class ColorCondition : Condition
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public ColorCondition()
            : base()
        {
            Color = "";
        }


        /// <summary>
        /// Получить или установить цвет, отображаемый при выполнении условия.
        /// </summary>
        #region Attributes
        [DisplayName("Color"), Category(Categories.Appearance)]
        [CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string Color { get; set; }
        
        
        /// <summary>
        /// Загрузить условие из XML-узла.
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            Color = xmlNode.GetChildAsString("Color");
        }

        /// <summary>
        /// Сохранить условие в XML-узле.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("Color", Color);
        }

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        public override object Clone()
        {
            Condition clonedCondition = ScadaUtils.DeepClone(this, PlgUtils.SerializationBinder);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }
    }
}
