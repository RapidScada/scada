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
 * Summary  : Scheme component that represents a button
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

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
    /// Scheme component that represents a button
    /// <para>Компонент схемы, представляющий кнопку</para>
    /// </summary>
    [Serializable]
    public class Button : BaseComponent, IDynamicComponent
    {
        /// <summary>
        /// Размер кнопки по умолчанию
        /// </summary>
        public static readonly Size DefaultSize = new Size(100, 30);
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public static readonly Size DefaultImageSize = new Size(16, 16);
        /// <summary>
        /// Текст кнопки по умолчанию
        /// </summary>
        public static readonly string DefaultText =
            Localization.UseRussian ? "Кнопка" : "Button";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Button()
            : base()
        {
            serBinder = PlgUtils.SerializationBinder;

            ForeColor = "";
            Font = null;
            ImageName = "";
            ImageSize = DefaultImageSize;
            Text = DefaultText;
            Action = Actions.None;
            BoundProperty = BoundProperties.None;
            InCnlNum = 0;
            CtrlCnlNum = 0;
            Size = DefaultSize;
        }


        /// <summary>
        /// Получить или установить цвет текста
        /// </summary>
        #region Attributes
        [DisplayName("Foreground color"), Category(Categories.Appearance)]
        [Description("The foreground color of the component, which is used to display text.")]
        [CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string ForeColor { get; set; }

        /// <summary>
        /// Получить или установить шрифт
        /// </summary>
        #region Attributes
        [DisplayName("Font"), Category(Categories.Appearance)]
        [Description("The font used to display text in the component.")]
        #endregion
        public Font Font { get; set; }

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
        /// Получить или установить размер изображения
        /// </summary>
        #region Attributes
        [DisplayName("Image size"), Category(Categories.Appearance)]
        [Description("The size of the button image in pixels.")]
        #endregion
        public Size ImageSize { get; set; }

        /// <summary>
        /// Получить или установить текст
        /// </summary>
        #region Attributes
        [DisplayName("Text"), Category(Categories.Appearance)]
        [Description("The text associated with the component.")]
        #endregion
        public string Text { get; set; }

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
        /// Получить или установить свойство, привязанное ко входному каналу
        /// </summary>
        #region Attributes
        [DisplayName("Bound Property"), Category(Categories.Behavior)]
        [Description("The button property that is bound to the input channel associated with the component.")]
        [CM.DefaultValue(BoundProperties.None)]
        #endregion
        public BoundProperties BoundProperty { get; set; }

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

            ForeColor = xmlNode.GetChildAsString("ForeColor");
            Font = Font.GetChildAsFont(xmlNode, "Font");
            ImageName = xmlNode.GetChildAsString("ImageName");
            ImageSize = Size.GetChildAsSize(xmlNode, "ImageSize");
            Text = xmlNode.GetChildAsString("Text");
            Action = xmlNode.GetChildAsEnum<Actions>("Action");
            BoundProperty = xmlNode.GetChildAsEnum<BoundProperties>("BoundProperty");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("ForeColor", ForeColor);
            Font.AppendElem(xmlElem, "Font", Font);
            xmlElem.AppendElem("ImageName", ImageName);
            Size.AppendElem(xmlElem, "ImageSize", ImageSize);
            xmlElem.AppendElem("Text", Text);
            xmlElem.AppendElem("Action", Action);
            xmlElem.AppendElem("BoundProperty", BoundProperty);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(Text);
        }
    }
}