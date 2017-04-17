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
 * Summary  : Scheme document properties
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
    /// Scheme document properties
    /// <para>Свойства документа схемы</para>
    /// </summary>
    public class SchemeDocument : IObservableItem
    {
        /// <summary>
        /// Размер схемы по умолчанию
        /// </summary>
        public static readonly Size DefaultSize = new Size(800, 600);


        /// <summary>
        /// Конструктор
        /// </summary>
        public SchemeDocument()
        {
            CnlFilter = new List<int>();
            SetToDefault();
        }


        /// <summary>
        /// Получить или установить размер
        /// </summary>
        #region Attributes
        [DisplayName("Size"), Category(Categories.Layout)]
        [Description("The size of the scheme in pixels.")]
        #endregion
        public Size Size { get; set; }

        /// <summary>
        /// Получить или установить цвет фона
        /// </summary>
        #region Attributes
        [DisplayName("Back color"), Category(Categories.Appearance)]
        [Description("The background color of the scheme.")]
        [CM.DefaultValue("White")/*, CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))*/]
        #endregion
        public string BackColor { get; set; }

        /// <summary>
        /// Получить или установить фоновое изображение
        /// </summary>
        #region Attributes
        [DisplayName("Background image"), Category(Categories.Appearance)]
        [Description("The background image used for the scheme.")]
        [CM.DefaultValue(null)]
        #endregion
        public Image BackImage { get; set; }

        /// <summary>
        /// Получить или установить основной цвет
        /// </summary>
        #region Attributes
        [DisplayName("Fore color"), Category(Categories.Appearance)]
        [Description("The foreground color of the scheme, which is used to display text.")]
        [CM.DefaultValue("Black")/*, CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))*/]
        #endregion
        public string ForeColor { get; set; }

        /// <summary>
        /// Получить или установить шрифт
        /// </summary>
        #region Attributes
        [DisplayName("Font"), Category(Categories.Appearance)]
        [Description("The font used to display text in the scheme.")]
        #endregion
        public Font Font { get; set; }

        /// <summary>
        /// Получить или установить заголовок
        /// </summary>
        #region Attributes
        [DisplayName("Title"), Category(Categories.Appearance)]
        [Description("The title of the scheme.")]
        #endregion
        public string Title { get; set; }

        /// <summary>
        /// Получить фильтр по входным каналам
        /// </summary>
        #region Attributes
        [DisplayName("Channel filter"), Category(Categories.Data)]
        [Description("The input channels used as a filter for showing events filtered by view.")]
        [CM.TypeConverter(typeof(CnlFilterConverter)), CM.Editor(typeof(CnlFilterEditor), typeof(UITypeEditor))]
        #endregion
        public List<int> CnlFilter { get; protected set; }

        /// <summary>
        /// Получить словарь изображений схемы
        /// </summary>
        #region Attributes
        [DisplayName("Images"), Category(Categories.Data)]
        [Description("The collection of images used in the scheme.")]
        [CM.TypeConverter(typeof(CollectionConverter))/*, CM.Editor(typeof(ImageEditor), typeof(UITypeEditor))*/]
        #endregion
        public Dictionary<string, Image> Images { get; protected set; }


        /// <summary>
        /// Установить значения свойств документа схемы по умолчанию
        /// </summary>
        public void SetToDefault()
        {
            Size = DefaultSize;
            BackColor = "White";
            BackImage = null;
            ForeColor = "Black";
            Font = new Font();
            Title = "";
            CnlFilter.Clear();
            Images = null;
        }

        /// <summary>
        /// Загрузить свойства документа схемы из XML-узла
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            SetToDefault();

            Size = Size.GetChildAsSize(xmlNode, "Size");
            BackColor = xmlNode.GetChildAsString("BackColor");
            //BackImage = 
            ForeColor = xmlNode.GetChildAsString("ForeColor");
            Font = Font.GetChildAsFont(xmlNode, "Font");
            Title = xmlNode.GetChildAsString("Title");
            //CnlFilter = 
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return SchemePhrases.SchemeItemName;
        }

        /// <summary>
        /// Вызвать событие ItemChanged
        /// </summary>
        public void OnItemChanged()
        {
            if (ItemChanged != null)
                ItemChanged(this, EventArgs.Empty);
        }


        /// <summary>
        /// Событие возникающее при изменении документа схемы
        /// </summary>
        public event EventHandler ItemChanged;
    }
}
