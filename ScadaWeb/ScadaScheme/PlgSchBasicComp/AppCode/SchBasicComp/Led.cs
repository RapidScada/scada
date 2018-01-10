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
 * Module   : PlgSchBasicComp
 * Summary  : Scheme component that represents led
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using Scada.Scheme.Model;
using Scada.Scheme.Model.DataTypes;
using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
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
        /// Конструктор
        /// </summary>
        public Led()
            : base()
        {
            serBinder = PlgUtils.SerializationBinder;

            Action = Actions.None;
            Conditions = new List<Condition>();
            ToolTip = "";
            InCnlNum = 0;
            CtrlCnlNum = 0;
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
        #endregion
        public int BorderOpacity { get; set; }

        /// <summary>
        /// Получить или установить толщину границы
        /// </summary>
        #region Attributes
        [DisplayName("Border width"), Category(Categories.Appearance)]
        [Description("The border width of the component in pixels.")]
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
        [CM.Editor(typeof(ConditionEditor), typeof(UITypeEditor))]
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
    }
}