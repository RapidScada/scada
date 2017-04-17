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
 * Summary  : Scheme component that represents dynamic picture
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.DataTypes;
using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Динамический рисунок
    /// </summary>
    public class DynamicPicture : StaticPicture
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DynamicPicture()
            : base()
        {
            ToolTip = "";
            ImageOnHover = null;
            BorderColorOnHover = "";
            InCnlNum = 0;
            CtrlCnlNum = 0;
            Action = Actions.None;
            Conditions = new List<Condition>();
        }


        /// <summary>
        /// Получить или установить подсказку
        /// </summary>
        #region Attributes
        [DisplayName("Tooltip"), Category(Categories.Behavior)]
        [Description("The pop-up hint that displays when user rests the pointer on the component.")]
        #endregion
        public string ToolTip { get; set; }

        /// <summary>
        /// Получить или установить изображение, отображаемое при наведении указателя мыши
        /// </summary>
        #region Attributes
        [DisplayName("Image on hover"), Category(Categories.Behavior)]
        [Description("The image shown when user rests the pointer on the component.")]
        [CM.DefaultValue(null)]
        #endregion
        public Image ImageOnHover { get; set; }

        /// <summary>
        /// Получить или установить цвет рамки при наведении указателя мыши
        /// </summary>
        #region Attributes
        [DisplayName("Border color on hover"), Category(Categories.Behavior)]
        [Description("The border color of the component when user rests the pointer on it.")]
        //[CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string BorderColorOnHover { get; set; }

        /// <summary>
        /// Получить или установить номер входного канала
        /// </summary>
        #region Attributes
        [DisplayName("Input channel"), Category(Categories.Data)]
        [Description("The input channel number associated with the component.")]
        [CM.DefaultValue(0)]
        #endregion
        public int InCnlNum { get; set; }

        /// <summary>
        /// Получить или установить номер канала управления
        /// </summary>
        #region Attributes
        [DisplayName("Output channel"), Category(Categories.Data)]
        [Description("The output channel number associated with the component.")]
        [CM.DefaultValue(0)]
        #endregion
        public int CtrlCnlNum { get; set; }

        /// <summary>
        /// Получить или установить действие
        /// </summary>
        #region Attributes
        [DisplayName("Action"), Category(Categories.Behavior)]
        [Description("The action executed by clicking the left mouse button on the component.")]
        [CM.DefaultValue(Actions.None)]
        #endregion
        public Actions Action { get; set; }

        /// <summary>
        /// Получить условия вывода изображений
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions for image output depending on the value of the input channel.")]
        [CM.DefaultValue(null), CM.TypeConverter(typeof(CollectionConverter))]
        [CM.Editor(typeof(ConditionEditor), typeof(UITypeEditor))]
        #endregion
        public List<Condition> Conditions { get; protected set; }

        
        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            ToolTip = xmlNode.GetChildAsString("ToolTip");
            string imageName = xmlNode.GetChildAsString("ImageOnHoverName");
            ImageOnHover = imageName == "" ? null : new Image() { Name = imageName };
            BorderColorOnHover = xmlNode.GetChildAsString("BorderColorOnHover");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
            Action = xmlNode.GetChildAsEnum<Actions>("Action");

            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
            if (conditionsNode != null)
            {
                Conditions = new List<Condition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    Condition condition = new Condition();
                    condition.LoadFromXml(conditionNode);
                    Conditions.Add(condition);
                }
            }
        }
    }
}
