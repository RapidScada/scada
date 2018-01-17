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
 * Summary  : Scheme component that represents led
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using Scada.Scheme;
using Scada.Scheme.Model;
using Scada.Scheme.Model.DataTypes;
using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Web.Plugins.SchBasicComp
{
    /// <summary>
    /// Scheme component that represents led
    /// <para>Компонент схемы, представляющий светодиод</para>
    /// </summary>
    [Serializable]
    public class Led : BaseComponent, IDynamicComponent
    {
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public static readonly Size DefaultSize = new Size(30, 30);


        /// <summary>
        /// Конструктор
        /// </summary>
        public Led()
            : base()
        {
            serBinder = PlgUtils.SerializationBinder;

            BorderColor = "Black";
            BorderOpacity = 30;
            BorderWidth = 3;
            FillColor = "Silver";
            Action = Actions.None;
            Conditions = new List<Condition>();
            AddDefaultConditions();
            ToolTip = "";
            InCnlNum = 0;
            CtrlCnlNum = 0;
            Size = DefaultSize;
        }


        /// <summary>
        /// Получить или установить цвет границы
        /// </summary>
        #region Attributes
        [DisplayName("Border color"), Category(Categories.Appearance)]
        [Description("The border color of the component.")]
        [CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string BorderColor { get; set; }

        /// <summary>
        /// Получить или установить непрозрачность границы
        /// </summary>
        #region Attributes
        [DisplayName("Border opacity"), Category(Categories.Appearance)]
        [Description("The border opacity percentage of the component.")]
        [CM.DefaultValue(100)]
        #endregion
        public int BorderOpacity { get; set; }

        /// <summary>
        /// Получить или установить толщину границы
        /// </summary>
        #region Attributes
        [DisplayName("Border width"), Category(Categories.Appearance)]
        [Description("The border width of the component in pixels.")]
        [CM.DefaultValue(0)]
        #endregion
        public int BorderWidth { get; set; }

        /// <summary>
        /// Получить или установить цвет заливки
        /// </summary>
        #region Attributes
        [DisplayName("Fill color"), Category(Categories.Appearance)]
        [Description("The fill color of the component.")]
        [CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string FillColor { get; set; }

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
        /// Получить условия, определяющие цвет заливки
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions determining the fill color depending on the value of the input channel.")]
        [CM.DefaultValue(null), CM.TypeConverter(typeof(CollectionConverter))]
        [CM.Editor(typeof(ColorConditionEditor), typeof(UITypeEditor))]
        #endregion
        public List<Condition> Conditions { get; protected set; }

        /// <summary>
        /// Получить или установить подсказку
        /// </summary>
        #region Attributes
        [DisplayName("Tooltip"), Category(Categories.Behavior)]
        [Description("The pop-up hint that displays when user rests the pointer on the component.")]
        #endregion
        public string ToolTip { get; set; }

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
        /// Добавить условия по умолчанию
        /// </summary>
        protected void AddDefaultConditions()
        {
            Conditions.Add(new ColorCondition()
            {
                CompareOperator1 = CompareOperators.LessThanEqual,
                Color = "Red"
            });

            Conditions.Add(new ColorCondition()
            {
                CompareOperator1 = CompareOperators.GreaterThan,
                Color = "Green"
            });
        }

        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            BorderColor = xmlNode.GetChildAsString("BorderColor");
            BorderOpacity = xmlNode.GetChildAsInt("BorderOpacity");
            BorderWidth = xmlNode.GetChildAsInt("BorderWidth");
            FillColor = xmlNode.GetChildAsString("FillColor");
            Action = xmlNode.GetChildAsEnum<Actions>("Action");

            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
            if (conditionsNode != null)
            {
                Conditions = new List<Condition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    Condition condition = new ColorCondition();
                    condition.SchemeDoc = SchemeDoc;
                    condition.LoadFromXml(conditionNode);
                    Conditions.Add(condition);
                }
            }

            ToolTip = xmlNode.GetChildAsString("ToolTip");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("BorderColor", BorderColor);
            xmlElem.AppendElem("BorderOpacity", BorderOpacity);
            xmlElem.AppendElem("BorderWidth", BorderWidth);
            xmlElem.AppendElem("FillColor", FillColor);
            xmlElem.AppendElem("Action", Action);

            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }

            xmlElem.AppendElem("ToolTip", ToolTip);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
        }

        /// <summary>
        /// Клонировать объект
        /// </summary>
        public override BaseComponent Clone()
        {
            Led clonedComponent = (Led)base.Clone();

            foreach (Condition condition in clonedComponent.Conditions)
            {
                condition.SchemeDoc = SchemeDoc;
            }

            return clonedComponent;
        }
    }
}