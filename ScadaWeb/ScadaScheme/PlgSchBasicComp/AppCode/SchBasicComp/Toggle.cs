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
 * Summary  : Scheme component that represents a toggle switch
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Scheme;
using Scada.Scheme.Model;
using Scada.Scheme.Model.DataTypes;
using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Drawing.Design;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Web.Plugins.SchBasicComp
{
    /// <summary>
    /// Scheme component that represents a toggle switch
    /// <para>Компонент схемы, представляющий тумблер</para>
    /// </summary>
    [Serializable]
    public class Toggle : BaseComponent, IDynamicComponent
    {
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public static readonly Size DefaultSize = new Size(50, 25);


        /// <summary>
        /// Конструктор
        /// </summary>
        public Toggle()
            : base()
        {
            serBinder = PlgUtils.SerializationBinder;

            BackColor = SchemeUtils.StatusColor;
            BorderColor = SchemeUtils.StatusColor;
            BorderWidth = 2;
            LeverColor = "White";
            Padding = 0;
            Action = Actions.SendCommand;
            InCnlNum = 0;
            CtrlCnlNum = 0;
            Size = DefaultSize;
        }


        /// <summary>
        /// Получить или установить цвет рычажка тумблера
        /// </summary>
        #region Attributes
        [DisplayName("Lever color"), Category(Categories.Appearance)]
        [Description("The color of the switch lever.")]
        [CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string LeverColor { get; set; }

        /// <summary>
        /// Получить или установить отступ
        /// </summary>
        #region Attributes
        [DisplayName("Padding"), Category(Categories.Appearance)]
        [Description("The space around the switch lever in pixels.")]
        #endregion
        public int Padding { get; set; }

        /// <summary>
        /// Получить или установить действие
        /// </summary>
        #region Attributes
        [DisplayName("Action"), Category(Categories.Behavior)]
        [Description("The action executed by clicking the left mouse button on the component.")]
        [CM.DefaultValue(Actions.SendCommand)]
        #endregion
        public Actions Action { get; set; }

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
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            LeverColor = xmlNode.GetChildAsString("LeverColor");
            Padding = xmlNode.GetChildAsInt("Padding");
            Action = xmlNode.GetChildAsEnum<Actions>("Action");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("LeverColor", LeverColor);
            xmlElem.AppendElem("Padding", Padding);
            xmlElem.AppendElem("Action", Action);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
        }
    }
}