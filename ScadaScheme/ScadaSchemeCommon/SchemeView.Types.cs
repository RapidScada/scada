/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Summary  : Scheme view. Derived types
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;

namespace Scada.Scheme
{
    partial class SchemeView
    {
        /// <summary>
        /// Схема
        /// </summary>
        public class Scheme
        {
            /// <summary>
            /// Размер схемы по умолчанию
            /// </summary>
            public static readonly Size DefaultSize = new Size(800, 600);

            /// <summary>
            /// Конструктор
            /// </summary>
            public Scheme()
                : this(null, null)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Scheme(Dictionary<string, Image> imageDict, List<int> cnlFilter)
            {
                Type = Localization.UseRussian ? "Схема" : "Scheme";
                ImageDict = imageDict;
                CnlsFilter = cnlFilter;
                SetToDefault();
            }

            /// <summary>
            /// Получить тип
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Тип объекта."), Category("Дизайн"), DisplayName("Тип")]
#else
            [Description("The object type."), Category("Design"), DisplayName("Type")]
#endif
            [ReadOnly(true)]
            #endregion
            public string Type { get; protected set; }
            /// <summary>
            /// Получить или установить размер
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Размер схемы в пикселях."), Category("Макет"), DisplayName("Размер")]
#else
            [Description("The size of the scheme in pixels."), Category("Layout"), DisplayName("Size")]
#endif
            #endregion
            public Size Size { get; set; }
            /// <summary>
            /// Получить или установить цвет фона
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Цвет фона схемы."), Category("Внешний вид"), DisplayName("Цвет фона")]
#else
            [Description("The background color of the scheme."), Category("Appearance"), DisplayName("Back color")]
#endif
            [DefaultValue("White"), Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string BackColor { get; set; }
            /// <summary>
            /// Получить или установить фоновое изображение
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Фоновое изображение схемы."), Category("Внешний вид"), DisplayName("Фоновое изображение")]
#else
            [Description("The background image used for the scheme."), Category("Appearance"), DisplayName("Background image")]
#endif
            [DefaultValue(null)]
            #endregion
            public Image BackImage { get; set; }
            /// <summary>
            /// Получить или установить основной цвет
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Основной цвет схемы, используемый для отображения текста.")]
            [Category("Внешний вид"), DisplayName("Основной цвет")]
#else
            [Description("The foreground color of the scheme, which is used to display text.")]
            [Category("Appearance"), DisplayName("Fore color")]
#endif
            [DefaultValue("Black"), Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string ForeColor { get; set; }
            /// <summary>
            /// Получить или установить шрифт
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Шрифт, используемый для отображения текста на схеме.")]
            [Category("Внешний вид"), DisplayName("Шрифт")]
#else
            [Description("The font used to display text in the scheme."), Category("Appearance"), DisplayName("Font")]
#endif
            #endregion
            public Font Font { get; set; }
            /// <summary>
            /// Получить или установить заголовок
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Заголовок схемы."), Category("Внешний вид"), DisplayName("Заголовок")]
#else
            [Description("The title of the scheme."), Category("Appearance"), DisplayName("Title")]
#endif
            #endregion
            public string Title { get; set; }
            /// <summary>
            /// Получить словарь изображений схемы
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Коллекция изображений, используемых на схеме.")]
            [Category("Данные"), DisplayName("Изображения")]
#else
            [Description("The collection of images used in the scheme."), Category("Data"), DisplayName("Images")]
#endif
            [TypeConverter(typeof(CollectionConverter)), Editor(typeof(ImageEditor), typeof(UITypeEditor))]
            #endregion
            public Dictionary<string, Image> ImageDict { get; private set; }
            /// <summary>
            /// Получить фильтр по входным каналам
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Входные каналы, используемые как фильтр для вывода событий по схеме.")]
            [Category("Данные"), DisplayName("Фильтр по каналам")]
#else
            [Description("The input channels used as a filter for showing events filtered by view.")]
            [Category("Data"), DisplayName("Channel filter")]
#endif
            [TypeConverter(typeof(CnlsFilterConverter)), Editor(typeof(CnlsFilterEditor), typeof(UITypeEditor))]
            #endregion
            public List<int> CnlsFilter { get; private set; }

            /// <summary>
            /// Установить значения свойств схемы по умолчанию
            /// </summary>
            public void SetToDefault()
            {
                Size = DefaultSize;
                BackColor = "White";
                BackImage = null;
                ForeColor = "Black";
                Font = new Font();
                Title = "";
            }
        }

        /// <summary>
        /// Элемент схемы
        /// </summary>
        public abstract class Element : ICloneable
        {
            /// <summary>
            /// Положение элемента по умолчанию
            /// </summary>
            public static readonly Point DefaultLocation = new Point(0, 0);
            /// <summary>
            /// Размер элемента по умолчанию
            /// </summary>
            public static readonly Size DefaultSize = new Size(100, 100);

            /// <summary>
            /// Конструктор
            /// </summary>
            public Element()
            {
                ID = 0;
                Name = "";
                Type = "";
                Location = DefaultLocation;
                Size = DefaultSize;
                ZIndex = 0;
            }

            /// <summary>
            /// Получить или установить идентификатор
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Уникальный идентификатор элемента схемы."), Category("Дизайн"), DisplayName("Идентификатор")]
#else
            [Description("The unique identifier of the scheme element."), Category("Design"), DisplayName("ID")]
#endif
            [ReadOnly(true)]
            #endregion
            public int ID { get; set; }
            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Наименование элемента схемы."), Category("Дизайн"), DisplayName("Наименование")]
#else
            [Description("The name of the scheme element."), Category("Design"), DisplayName("Name")]
#endif
            #endregion
            public string Name { get; set; }
            /// <summary>
            /// Получить тип
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Тип элемента схемы."), Category("Дизайн"), DisplayName("Тип")]
#else
            [Description("The type of the scheme element."), Category("Design"), DisplayName("Type")]
#endif
            [ReadOnly(true)]
            #endregion
            public string Type { get; protected set; }
            /// <summary>
            /// Получить или установить положение
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Координаты левого верхнего угла элемента схемы.")]
            [Category("Макет"), DisplayName("Положение")]
#else
            [Description("The coordinates of the upper-left corner of the scheme element.")]
            [Category("Layout"), DisplayName("Location")]
#endif
            #endregion
            public Point Location { get; set; }
            /// <summary>
            /// Получить или установить размер
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Размер элемента схемы в пикселях."), Category("Макет"), DisplayName("Размер")]
#else
            [Description("The size of the scheme element in pixels."), Category("Layout"), DisplayName("Size")]
#endif
            #endregion
            public Size Size { get; set; }
            /// <summary>
            /// Получить или установить порядок отображения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Порядок отображения элемента схемы по оси Z.")]
            [Category("Макет"), DisplayName("Порядок")]
#else
            [Description("The stack order of the scheme element."), Category("Layout"), DisplayName("ZIndex")]
#endif
            [DefaultValue(0)]
            #endregion
            public int ZIndex { get; set; }

            /// <summary>
            /// Копировать данные из элемента
            /// </summary>
            protected virtual void CopyFrom(Element srcElem)
            {
                ID = srcElem.ID;
                Name = srcElem.Name;
                Location = srcElem.Location;
                Size = srcElem.Size;
                ZIndex = srcElem.ZIndex;
            }
            /// <summary>
            /// Клонировать объект
            /// </summary>
            public abstract object Clone();
        }

        /// <summary>
        /// Статическая надпись
        /// </summary>
        public class StaticText : Element
        {
            /// <summary>
            /// Текст надписи по умолчанию
            /// </summary>
            public static readonly string DefaultText = 
                Localization.UseRussian ? "Статическая надпись" : "Static text";

            /// <summary>
            /// Конструктор
            /// </summary>
            public StaticText()
                : base()
            {
                Type = Localization.UseRussian ? "Статическая надпись" : "Static text";
                AutoSize = true;
                BackColor = "";
                BorderColor = "";
                ForeColor = "";
                Font = null;
                Text = DefaultText;
                WordWrap = false;
                HAlign = HorizontalAlignment.Left;
                VAlign = VerticalAlignment.Top;
            }

            /// <summary>
            /// Получить или установить признак авто размера
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Автоматическая установка размера надписи по содержимому.")]
            [Category("Макет"), DisplayName("Авто размер")]
#else
            [Description("Automatic resizing based on content size."), Category("Layout"), DisplayName("Auto size")]
#endif
            [DefaultValue(true), TypeConverter(typeof(BooleanConverterEx))]
            #endregion
            public bool AutoSize { get; set; }
            /// <summary>
            /// Получить или установить цвет фона
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Цвет фона надписи."), Category("Внешний вид"), DisplayName("Цвет фона")]
#else
            [Description("The background color of the element."), Category("Appearance"), DisplayName("Back color")]
#endif
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string BackColor { get; set; }
            /// <summary>
            /// Получить или установить цвет рамки
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Цвет рамки надписи."), Category("Внешний вид"), DisplayName("Цвет рамки")]
#else
            [Description("The border color of the element."), Category("Appearance"), DisplayName("Border color")]
#endif
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string BorderColor { get; set; }
            /// <summary>
            /// Получить или установить цвет текста
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Основной цвет надписи, используемый для отображения текста.")]
            [Category("Внешний вид"), DisplayName("Цвет текста")]
#else
            [Description("The foreground color of the element, which is used to display text.")]
            [Category("Appearance"), DisplayName("Fore color")]
#endif
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string ForeColor { get; set; }
            /// <summary>
            /// Получить или установить шрифт
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Шрифт, используемый для отображения текста надписи.")]
            [Category("Внешний вид"), DisplayName("Шрифт")]
#else
            [Description("The font used to display text in the element."), Category("Appearance"), DisplayName("Font")]
#endif
            #endregion
            public Font Font { get; set; }
            /// <summary>
            /// Получить или установить текст
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Текст надписи."), Category("Внешний вид"), DisplayName("Текст")]
#else
            [Description("The text associated with the element."), Category("Appearance"), DisplayName("Text")]
#endif
            #endregion
            public string Text { get; set; }
            /// <summary>
            /// Получить или установить признак переноса текста по словам
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Автоматический перенос текста по словам.")]
            [Category("Внешний вид"), DisplayName("Перенос по словам")]
#else
            [Description("Text is automatically word-wrapped."), Category("Appearance"), DisplayName("Word wrap")]
#endif
            [DefaultValue(false), TypeConverter(typeof(BooleanConverterEx))]
            #endregion
            public bool WordWrap { get; set; }
            /// <summary>
            /// Получить или установить горизонтальное выравнивание
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Горизонтальное выравнивание текста внутри надписи.")]
            [Category("Внешний вид"), DisplayName("Гор. выравнивание")]
#else
            [Description("Horizontal alignment of the text within the element.")]
            [Category("Appearance"), DisplayName("Horizontal alignment")]
#endif
            [DefaultValue(HorizontalAlignment.Left)]
            #endregion
            public HorizontalAlignment HAlign { get; set; }
            /// <summary>
            /// Получить или установить вертикальное выравнивание
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Вертикальное выравнивание текста внутри надписи.")]
            [Category("Внешний вид"), DisplayName("Верт. выравнивание")]
#else
            [Description("Vertical alignment of the text within the element.")]
            [Category("Appearance"), DisplayName("Vertical alignment")]
#endif
            [DefaultValue(VerticalAlignment.Top)]
            #endregion
            public VerticalAlignment VAlign { get; set; }

            /// <summary>
            /// Копировать данные из элемента
            /// </summary>
            protected override void CopyFrom(Element srcElem)
            {
                base.CopyFrom(srcElem);
                StaticText staticText = (StaticText)srcElem;
                AutoSize = staticText.AutoSize;
                BackColor = staticText.BackColor;
                BorderColor = staticText.BorderColor;
                ForeColor = staticText.ForeColor;
                Font = staticText.Font == null ? null : staticText.Font.Clone();
                Text = staticText.Text;
                WordWrap = staticText.WordWrap;
                HAlign = staticText.HAlign;
                VAlign = staticText.VAlign;
            }
            /// <summary>
            /// Клонировать объект
            /// </summary>
            public override object Clone()
            {
                StaticText staticText = new StaticText();
                staticText.CopyFrom(this);
                return staticText;
            }
        }

        /// <summary>
        /// Динамическая надпись
        /// </summary>
        public class DynamicText : StaticText
        {
            /// <summary>
            /// Текст надписи по умолчанию
            /// </summary>
            new public static readonly string DefaultText = 
                Localization.UseRussian ? "Динамическая надпись" : "Dynamic text";

            /// <summary>
            /// Конструктор
            /// </summary>
            public DynamicText()
                : base()
            {
                Type = Localization.UseRussian ? "Динамическая надпись" : "Dynamic text";
                Text = DefaultText;
                ToolTip = "";
                UnderlineOnHover = false;
                BackColorOnHover = "";
                BorderColorOnHover = "";
                ForeColorOnHover = "";
                InCnlNum = 0;
                CtrlCnlNum = 0;
                Action = SchemeView.Action.None;
                ShowValue = SchemeView.ShowValue.ShowWithUnit;
            }

            /// <summary>
            /// Получить или установить подсказку
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Всплывающая подсказка, которая отображается при наведении указателя мыши на надпись.")]
            [Category("Поведение"), DisplayName("Подсказка")]
#else
            [Description("The pop-up hint that displays when user rests the pointer on the element.")]
            [Category("Behavior"), DisplayName("Tooltip")]
#endif
            #endregion
            public string ToolTip { get; set; }
            /// <summary>
            /// Получить или установить признак подчёркивания при наведении указателя мыши
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Подчёркивание надписи при наведени указателя мыши.")]
            [Category("Поведение"), DisplayName("Подчёркивание при наведени")]
#else
            [Description("Underline text when user rests the pointer on the element.")]
            [Category("Behavior"), DisplayName("Underline on hover")]
#endif
            [DefaultValue(false), TypeConverter(typeof(BooleanConverterEx))]
            #endregion
            public bool UnderlineOnHover { get; set; }
            /// <summary>
            /// Получить или установить цвет фона при наведении указателя мыши
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Цвет фона надписи при наведении указателя мыши.")]
            [Category("Поведение"), DisplayName("Цвет фона при наведении")]
#else
            [Description("The background color of the element when user rests the pointer on it.")]
            [Category("Behavior"), DisplayName("Back color on hover")]
#endif
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string BackColorOnHover { get; set; }
            /// <summary>
            /// Получить или установить цвет рамки при наведении указателя мыши
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Цвет рамки надписи при наведени указателя мыши.")]
            [Category("Поведение"), DisplayName("Цвет рамки при наведени")]
#else
            [Description("The border color of the element when user rests the pointer on it.")]
            [Category("Behavior"), DisplayName("Border color on hover")]
#endif
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string BorderColorOnHover { get; set; }
            /// <summary>
            /// Получить или установить цвет текста при наведени указателя мыши
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Основной цвет надписи, используемый для отображения текста при наведени указателя мыши.")]
            [Category("Поведение"), DisplayName("Цвет текста при наведени")]
#else
            [Description("The foreground color of the element, which is used to display text, when user rests the pointer on it.")]
            [Category("Behavior"), DisplayName("Fore color on hover")]
#endif
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string ForeColorOnHover { get; set; }
            /// <summary>
            /// Получить или установить номер входного канала
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Номер входного канала, связанного с надписью.")]
            [Category("Данные"), DisplayName("Входной канал")]
#else
            [Description("The input channel number associated with the element.")]
            [Category("Data"), DisplayName("Input channel")]
#endif
            [DefaultValue(0)]
            #endregion
            public int InCnlNum { get; set; }
            /// <summary>
            /// Получить или установить номер канала управления
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Номер канала управления, связанного с надписью.")]
            [Category("Данные"), DisplayName("Канал управления")]
#else
            [Description("The output channel number associated with the element.")]
            [Category("Data"), DisplayName("Output channel")]
#endif
            [DefaultValue(0)]
            #endregion
            public int CtrlCnlNum { get; set; }
            /// <summary>
            /// Получить или установить действие
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Действие, выполняемое по щелчку левой кнопкой мыши на надписи.")]
            [Category("Поведение"), DisplayName("Действие")]
#else
            [Description("The action executed by clicking the left mouse button on the element.")]
            [Category("Behavior"), DisplayName("Action")]
#endif
            [DefaultValue(Action.None)]
            #endregion
            public Action Action { get; set; }
            /// <summary>
            /// Получить или установить признак вывода значения входного канала
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Вывод значения входного канала, связанного с надписью.")]
            [Category("Поведение"), DisplayName("Вывод значения")]
#else
            [Description("Show a value of the input channel associated with the element.")]
            [Category("Behavior"), DisplayName("Show value")]
#endif
            [DefaultValue(ShowValue.ShowWithUnit)]
            #endregion
            public ShowValue ShowValue { get; set; }

            /// <summary>
            /// Копировать данные из элемента
            /// </summary>
            protected override void CopyFrom(Element srcElem)
            {
                base.CopyFrom(srcElem);
                DynamicText dynamicText = (DynamicText)srcElem;
                ToolTip = dynamicText.ToolTip;
                UnderlineOnHover = dynamicText.UnderlineOnHover;
                BackColorOnHover = dynamicText.BackColorOnHover;
                BorderColorOnHover = dynamicText.BorderColorOnHover;
                ForeColorOnHover = dynamicText.ForeColorOnHover;
                ShowValue = dynamicText.ShowValue;
                InCnlNum = dynamicText.InCnlNum;
                CtrlCnlNum = dynamicText.CtrlCnlNum;
                Action = dynamicText.Action;
            }
            /// <summary>
            /// Клонировать объект
            /// </summary>
            public override object Clone()
            {
                DynamicText dynamicText = new DynamicText();
                dynamicText.CopyFrom(this);
                return dynamicText;
            }
        }

        /// <summary>
        /// Статический рисунок
        /// </summary>
        public class StaticPicture : Element
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public StaticPicture()
            {
                Type = Localization.UseRussian ? "Статический рисунок" : "Static picture";
                BorderColor = "Gray";
                Image = null;
                ImageStretch = SchemeView.ImageStretch.None;
            }

            /// <summary>
            /// Получить или установить цвет рамки
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Цвет рамки рисунка."), Category("Внешний вид"), DisplayName("Цвет рамки")]
#else
            [Description("The border color of the element."), Category("Appearance"), DisplayName("Border color")]
#endif
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string BorderColor { get; set; }
            /// <summary>
            /// Получить или установить изображение
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Изображение из коллекции изображений схемы.")]
            [Category("Внешний вид"), DisplayName("Изображение")]
#else
            [Description("The image from the scheme images collection."), Category("Appearance"), DisplayName("Image")]
#endif
            [DefaultValue(null)]
            #endregion
            public Image Image { get; set; }
            /// <summary>
            /// Получить или установить растяжение изображения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Растяжение изображения.")]
            [Category("Внешний вид"), DisplayName("Растяжение")]
#else
            [Description("Stretch the image."), Category("Appearance"), DisplayName("Image stretch")]
#endif
            [DefaultValue(ImageStretch.None)]
            #endregion
            public ImageStretch ImageStretch { get; set; }

            /// <summary>
            /// Копировать данные из элемента
            /// </summary>
            protected override void CopyFrom(Element srcElem)
            {
                base.CopyFrom(srcElem);
                StaticPicture staticPicture = (StaticPicture)srcElem;
                BorderColor = staticPicture.BorderColor;
                Image = staticPicture.Image == null ? null : staticPicture.Image.CloneWithoutData();
                ImageStretch = staticPicture.ImageStretch;
            }
            /// <summary>
            /// Клонировать объект
            /// </summary>
            public override object Clone()
            {
                StaticPicture staticPicture = new StaticPicture();
                staticPicture.CopyFrom(this);
                return staticPicture;
            }
        }

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
                Type = Localization.UseRussian ? "Динамический рисунок" : "Dynamic picture";
                ToolTip = "";
                ImageOnHover = null;
                BorderColorOnHover = "";
                InCnlNum = 0;
                CtrlCnlNum = 0;
                Action = SchemeView.Action.None;
                Conditions = null;
            }

            /// <summary>
            /// Получить или установить подсказку
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Всплывающая подсказка, которая отображается при наведении указателя мыши на рисунок.")]
            [Category("Поведение"), DisplayName("Подсказка")]
#else
            [Description("The pop-up hint that displays when user rests the pointer on the element.")]
            [Category("Behavior"), DisplayName("Tooltip")]
#endif
            #endregion
            public string ToolTip { get; set; }
            /// <summary>
            /// Получить или установить изображение, отображаемое при наведении указателя мыши
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Изображение, отображаемое при наведении указателя мыши.")]
            [Category("Поведение"), DisplayName("Изображение при наведении")]
#else
            [Description("The image shown when user rests the pointer on the element.")]
            [Category("Behavior"), DisplayName("Image on hover")]
#endif
            [DefaultValue(null)]
            #endregion
            public Image ImageOnHover { get; set; }
            /// <summary>
            /// Получить или установить цвет рамки при наведении указателя мыши
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Цвет рамки рисунка при наведени указателя мыши.")]
            [Category("Поведение"), DisplayName("Цвет рамки при наведени")]
#else
            [Description("The border color of the element when user rests the pointer on it.")]
            [Category("Behavior"), DisplayName("Border color on hover")]
#endif
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            #endregion
            public string BorderColorOnHover { get; set; }
            /// <summary>
            /// Получить или установить номер входного канала
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Номер входного канала, связанного с рисунком.")]
            [Category("Данные"), DisplayName("Входной канал")]
#else
            [Description("The input channel number associated with the element.")]
            [Category("Data"), DisplayName("Input channel")]
#endif
            [DefaultValue(0)]
            #endregion
            public int InCnlNum { get; set; }
            /// <summary>
            /// Получить или установить номер канала управления
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Номер канала управления, связанного с рисунком.")]
            [Category("Данные"), DisplayName("Канал управления")]
#else
            [Description("The output channel number associated with the element.")]
            [Category("Data"), DisplayName("Output channel")]
#endif
            [DefaultValue(0)]
            #endregion
            public int CtrlCnlNum { get; set; }
            /// <summary>
            /// Получить или установить действие
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Действие, выполняемое по щелчку левой кнопкой мыши на рисунке.")]
            [Category("Поведение"), DisplayName("Действие")]
#else
            [Description("The action executed by clicking the left mouse button on the element.")]
            [Category("Behavior"), DisplayName("Action")]
#endif
            [DefaultValue(Action.None)]
            #endregion
            public Action Action { get; set; }
            /// <summary>
            /// Получить или установить условия для изображений
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Условия для вывода изображений в зависимости от значения входного канала.")]
            [Category("Поведение"), DisplayName("Условия")]
#else
            [Description("The conditions for image output depending on the value of the input channel.")]
            [Category("Behavior"), DisplayName("Conditions")]
#endif
            [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
            [Editor(typeof(CondEditor), typeof(UITypeEditor))]
            #endregion
            public List<Condition> Conditions { get; set; }

            /// <summary>
            /// Копировать данные из элемента
            /// </summary>
            protected override void CopyFrom(Element srcElem)
            {
                base.CopyFrom(srcElem);
                DynamicPicture dynamicPicture = (DynamicPicture)srcElem;
                ToolTip = dynamicPicture.ToolTip;
                ImageOnHover = dynamicPicture.ImageOnHover == null ? 
                    null : dynamicPicture.ImageOnHover.CloneWithoutData();
                BorderColorOnHover = dynamicPicture.BorderColorOnHover;
                InCnlNum = dynamicPicture.InCnlNum;
                CtrlCnlNum = dynamicPicture.CtrlCnlNum;
                Action = dynamicPicture.Action;
                if (dynamicPicture.Conditions == null)
                {
                    Conditions = null;
                }
                else
                {
                    Conditions = new List<Condition>();
                    foreach (Condition cond in dynamicPicture.Conditions)
                        Conditions.Add((Condition)cond.Clone());
                }
            }
            /// <summary>
            /// Клонировать объект
            /// </summary>
            public override object Clone()
            {
                DynamicPicture dynamicPicture = new DynamicPicture();
                dynamicPicture.CopyFrom(this);
                return dynamicPicture;
            }
        }

        /// <summary>
        /// Интерфейс для проверки уникальности наименования
        /// </summary>
        public interface ICheckNameUnique
        {
            /// <summary>
            /// Проверить уникальность наименования
            /// </summary>
            bool NameIsUnique(string name);
        }

        /// <summary>
        /// Точка
        /// </summary>
        [TypeConverter(typeof(StructConverter))]
        public struct Point
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Point(int x, int y)
                : this()
            {
                X = x;
                Y = y;
            }

            /// <summary>
            /// Получить или установить координату X
            /// </summary>
            public int X { get; set; }
            /// <summary>
            /// Получить или установить координату Y
            /// </summary>
            public int Y { get; set; }
        }

        /// <summary>
        /// Размер
        /// </summary>
        [TypeConverter(typeof(StructConverter))]
        public struct Size
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Size(int width, int height)
                : this()
            {
                Width = width;
                Height = height;
            }

            /// <summary>
            /// Получить или установить ширину
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [DisplayName("Ширина")]
#endif
            #endregion
            public int Width { get; set; }
            /// <summary>
            /// Получить или установить высоту
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [DisplayName("Высота")]
#endif
            #endregion
            public int Height { get; set; }
        }

        /// <summary>
        /// Шрифт
        /// </summary>
        [TypeConverter(typeof(FontConverter))]
        [Editor(typeof(FontEditor), typeof(UITypeEditor))]
        public class Font
        {
            /// <summary>
            /// Наименование по умолчанию
            /// </summary>
            public const string DefaultName = "Verdana";
            /// <summary>
            /// Размер по умолчанию
            /// </summary>
            public const int DefaultSize = 11;
            /// <summary>
            /// Шрифт по умолчанию
            /// </summary>
            public static readonly Font Default;

            /// <summary>
            /// Статический конструктор
            /// </summary>
            static Font()
            {
                Default = new Font();
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Font()
            {
                Name = DefaultName;
                Size = DefaultSize;
                Bold = false;
                Italic = false;
                Underline = false;
            }

            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить размер
            /// </summary>
            public int Size { get; set; }
            /// <summary>
            /// Получить или установить признак, что шрифт жирный
            /// </summary>
            public bool Bold { get; set; }
            /// <summary>
            /// Получить или установить признак, что шрифт курсив
            /// </summary>
            public bool Italic { get; set; }
            /// <summary>
            /// Получить или установить признак, что шрифт подчёркнутый
            /// </summary>
            public bool Underline { get; set; }

            /// <summary>
            /// Клонировать шрифт
            /// </summary>
            public Font Clone()
            {
                Font font = new Font();
                font.Name = Name;
                font.Size = Size;
                font.Bold = Bold;
                font.Italic = Italic;
                font.Underline = Underline;
                return font;
            }
        }

        /// <summary>
        /// Изображение
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
                Source = null;
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
            /// Источник данных изображения
            /// </summary>
            public object Source { get; set; }
            
            /// <summary>
            /// Клонировать изображение без клонирования данных
            /// </summary>
            public Image CloneWithoutData()
            {
                Image image = new Image();
                image.Name = Name;
                return image;
            }
        }

        /// <summary>
        /// Растяжение изображения
        /// </summary>
        [TypeConverter(typeof(EnumConverterEx))]
        public enum ImageStretch
        {
            /// <summary>
            /// Не задано
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Не задано")]
#endif
            #endregion
            None,
            /// <summary>
            /// Заполнить заданный размер
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Заполнить размер")]
#endif
            #endregion
            Fill,
            /// <summary>
            /// Растянуть пропорционально в рамках заданного размера
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Растянуть пропорционально")]
#endif
            #endregion
            Zoom
        }

        /// <summary>
        /// Горизонтальное выравнивание
        /// </summary>
        [TypeConverter(typeof(EnumConverterEx))]
        public enum HorizontalAlignment
        {
            /// <summary>
            /// Слева
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Слева")]
#endif
            #endregion
            Left,
            /// <summary>
            /// По центру
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("По центру")]
#endif
            #endregion
            Center,
            /// <summary>
            /// Справа
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Справа")]
#endif
            #endregion
            Right
        }

        /// <summary>
        /// Вертикальное выравнивание
        /// </summary>
        [TypeConverter(typeof(EnumConverterEx))]
        public enum VerticalAlignment
        {
            /// <summary>
            /// Сверху
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Сверху")]
#endif
            #endregion
            Top,
            /// <summary>
            /// По центру
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("По центру")]
#endif
            #endregion
            Center,
            /// <summary>
            /// Снизу
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Снизу")]
#endif
            #endregion
            Bottom
        }

        /// <summary>
        /// Действие, связанное с динамическим элементом
        /// </summary>
        [TypeConverter(typeof(EnumConverterEx))]
        public enum Action
        {
            /// <summary>
            /// Не задано
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Не задано")]
#endif
            #endregion
            None,
            /// <summary>
            /// Построить график
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Построить график")]
#else
            [Description("Draw diagram")]
#endif
            #endregion
            DrawDiagram,
            /// <summary>
            /// Отправить команду
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Отправить команду")]
#else
            [Description("Send command")]
#endif
            #endregion
            SendCommand
        }

        /// <summary>
        /// Вывод значения входного канала
        /// </summary>
        [TypeConverter(typeof(EnumConverterEx))]
        public enum ShowValue
        {
            /// <summary>
            /// Нет
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Нет")]
#else
            [Description("No")]
#endif
            #endregion
            NotShow,
            /// <summary>
            /// С размерностью
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("С размерностью")]
#else
            [Description("With unit")]
#endif
            #endregion
            ShowWithUnit,
            /// <summary>
            /// Без размерности
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Без размерности")]
#else
            [Description("Without unit")]
#endif
            #endregion
            ShowWithoutUnit
        }

        /// <summary>
        /// Оператор сравнения
        /// </summary>
        [TypeConverter(typeof(EnumConverterEx))]
        public enum CompareOperator
        {
            /// <summary>
            /// Равно
            /// </summary>
            [Description("=")]
            Equal,
            /// <summary>
            /// Не равно
            /// </summary>
            [Description("<>")]
            NotEqual,
            /// <summary>
            /// Меньше
            /// </summary>
            [Description("<")]
            LessThan,
            /// <summary>
            /// Меньше или равно
            /// </summary>
            [Description("<=")]
            LessThanEqual,
            /// <summary>
            /// Больше
            /// </summary>
            [Description(">")]
            GreaterThan,
            /// <summary>
            /// Больше или равно
            /// </summary>
            [Description(">=")]
            GreaterThanEqual
        }

        /// <summary>
        /// Логический оператор
        /// </summary>
        [TypeConverter(typeof(EnumConverterEx))]
        public enum LogicalOperator
        {
            /// <summary>
            /// Не задан
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Не задан")]
#endif
            #endregion
            None,
            /// <summary>
            /// И
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("И")]
#endif
            #endregion
            And,
            /// <summary>
            /// Или
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Description("Или")]
#endif
            #endregion
            Or
        }

        /// <summary>
        /// Условие
        /// </summary>
        public class Condition : ICloneable
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Condition()
            {
                CompareOperator1 = CompareOperator.LessThan;
                CompareArgument1 = 0.0;
                LogicalOperator = SchemeView.LogicalOperator.None;
                CompareOperator2 = CompareOperator.LessThan;
                CompareArgument2 = 0.0;
                Image = null;
            }

            /// <summary>
            /// Получить или установить 1-й оператор сравнения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Условие"), DisplayName("Опер. сравн. 1")]
#else
            [Category("Condition"), DisplayName("Compare oper. 1")]
#endif
            #endregion
            public CompareOperator CompareOperator1 { get; set; }
            /// <summary>
            /// Получить или установить аргумент для сравнения 1-м оператором
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Условие"), DisplayName("Аргумент 1")]
#else
            [Category("Condition"), DisplayName("Argument 1")]
#endif
            [DefaultValue(0.0)]
            #endregion
            public double CompareArgument1 { get; set; }
            /// <summary>
            /// Получить или установить логический оператор, применяемый к результатам сравнения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Условие"), DisplayName("Логич. опер.")]
#else
            [Category("Condition"), DisplayName("Logical oper.")]
#endif
            [DefaultValue(LogicalOperator.None)]
            #endregion
            public LogicalOperator LogicalOperator { get; set; }
            /// <summary>
            /// Получить или установить 2-й оператор сравнения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Условие"), DisplayName("Опер. сравн. 2")]
#else
            [Category("Condition"), DisplayName("Compare oper. 2")]
#endif
            #endregion
            public CompareOperator CompareOperator2 { get; set; }
            /// <summary>
            /// Получить или установить аргумент для сравнения 2-м оператором
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Условие"), DisplayName("Аргумент 2")]
#else
            [Category("Condition"), DisplayName("Argument 2")]
#endif
            [DefaultValue(0.0)]
            #endregion
            public double CompareArgument2 { get; set; }
            /// <summary>
            /// Получить или установить изображение, отображаемое при выполнении условия
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Внешний вид"), DisplayName("Изображение")]
#else
            [Category("Condition"), DisplayName("Image")]
#endif
            [DefaultValue(null)]
            #endregion
            public Image Image { get; set; }

            /// <summary>
            /// Преобразовать оператор сравнения в строку
            /// </summary>
            private string OperToString(CompareOperator oper)
            {
                switch (oper)
                {
                    case CompareOperator.Equal:
                        return " = ";
                    case CompareOperator.NotEqual:
                        return " <> ";
                    case CompareOperator.LessThan:
                        return " < ";
                    case CompareOperator.LessThanEqual:
                        return " <= ";
                    case CompareOperator.GreaterThan:
                        return " > ";
                    default: // CompareOperator.GreaterThanEqual
                        return " >= ";
                }
            }
            /// <summary>
            /// Преобразовать логический оператор в строку
            /// </summary>
            private string OperToString(LogicalOperator oper)
            {
                switch (oper)
                {
                    case LogicalOperator.And:
                        return Localization.UseRussian ? " И " : " And ";
                    case LogicalOperator.Or:
                        return Localization.UseRussian ? " Или " : " Or ";
                    default: // LogicalOperator.None
                        return "";
                }
            }
            /// <summary>
            /// Получить строковое представление объекта
            /// </summary>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Localization.UseRussian ? "Знач." : "Value");
                sb.Append(OperToString(CompareOperator1));
                sb.Append(CompareArgument1);
                if (LogicalOperator != SchemeView.LogicalOperator.None)
                {
                    sb.Append(OperToString(LogicalOperator));
                    sb.Append(Localization.UseRussian ? "Знач." : "Value");
                    sb.Append(OperToString(CompareOperator2));
                    sb.Append(CompareArgument2);
                }
                return sb.ToString();
            }
            /// <summary>
            /// Клонировать объект
            /// </summary>
            public object Clone()
            {
                Condition cond = new Condition();
                cond.CompareOperator1 = CompareOperator1;
                cond.CompareArgument1 = CompareArgument1;
                cond.LogicalOperator = LogicalOperator;
                cond.CompareOperator2 = CompareOperator2;
                cond.CompareArgument2 = CompareArgument2;
                if (Image != null)
                    cond.Image = Image.CloneWithoutData();
                return cond;
            }
        }

        /// <summary>
        /// Данные входного канала
        /// </summary>
        public class CnlData
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CnlData()
            {
                Val = 0.0;
                ValStr = "";
                ValStrWithUnit = "";
                Stat = 0;
                Color = "";
            }

            /// <summary>
            /// Получить или установить значение
            /// </summary>
            public double Val { get; set; }
            /// <summary>
            /// Получить или установить значение в строковом виде без указания размерности
            /// </summary>
            public string ValStr { get; set; }
            /// <summary>
            /// Получить или установить значение в строковом виде с указанием размерности
            /// </summary>
            public string ValStrWithUnit { get; set; }
            /// <summary>
            /// Получить или установить статус
            /// </summary>
            public int Stat { get; set; }
            /// <summary>
            /// Получить или установить цвет вывода данных, соответствующий статусу
            /// </summary>
            public string Color { get; set; }
        }

        /// <summary>
        /// Тип элемента схемы
        /// </summary>
        public enum ElementTypes
        {
            /// <summary>
            /// Неизвестный
            /// </summary>
            Unknown,
            /// <summary>
            /// Статическая надпись
            /// </summary>
            StaticText,
            /// <summary>
            /// Динамическая надпись
            /// </summary>
            DynamicText,
            /// <summary>
            /// Статический рисунок
            /// </summary>
            StaticPicture,
            /// <summary>
            /// Динамический рисунок
            /// </summary>
            DynamicPicture
        }

        /// <summary>
        /// Данные элемента схемы для передачи с помощью WCF-службы
        /// </summary>
        public class ElementData
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ElementData()
            {
                StaticText = null;
                DynamicText = null;
                StaticPicture = null;
                DynamicPicture = null;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public ElementData(Element element)
                : this()
            {
                if (element is DynamicText)
                {
                    ElementType = ElementTypes.DynamicText;
                    DynamicText = (DynamicText)element;
                }
                else if (element is StaticText)
                {
                    ElementType = ElementTypes.StaticText;
                    StaticText = (StaticText)element;
                }
                else if (element is DynamicPicture)
                {
                    ElementType = ElementTypes.DynamicPicture;
                    DynamicPicture = (DynamicPicture)element;
                }
                else if (element is StaticPicture)
                {
                    ElementType = ElementTypes.StaticPicture;
                    StaticPicture = (StaticPicture)element;
                }
                else
                {
                    ElementType = ElementTypes.Unknown;
                }
            }

            /// <summary>
            /// Получить или установить тип элемента схемы
            /// </summary>
            public ElementTypes ElementType { get; set; }
            /// <summary>
            /// Получить или установить статическую надпись
            /// </summary>
            public StaticText StaticText { get; set; }
            /// <summary>
            /// Получить или установить динамическую надпись
            /// </summary>
            public DynamicText DynamicText { get; set; }
            /// <summary>
            /// Получить или установить статический рисунок
            /// </summary>
            public StaticPicture StaticPicture { get; set; }
            /// <summary>
            /// Получить или установить динамический рисунок
            /// </summary>
            public DynamicPicture DynamicPicture { get; set; }
        }

        /// <summary>
        /// Данные схемы
        /// </summary>
        /// <remarks>Класс необходим для генерации proxy-классов WCF-службы</remarks>
        public class SchemeData
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public SchemeData()
            {
                CtrlRight = false;
                CnlList = null;
                SchemeParams = null;
                ElementDataList = new List<ElementData>();
                ImageDict = null;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public SchemeData(SchemeView schemeView)
                : this()
            {
                CnlList = schemeView.CnlList;
                SchemeParams = schemeView.SchemeParams;
                ImageDict = schemeView.ImageDict;

                foreach (Element elem in schemeView.ElementList)
                    ElementDataList.Add(new ElementData(elem));
            }

            /// <summary>
            /// Получить или установить право на управление
            /// </summary>
            public bool CtrlRight { get; set; }
            /// <summary>
            /// Получить или установить упорядоченный без повторений список номеров входных каналов, используемых схемой
            /// </summary>
            public List<int> CnlList { get; set; }
            /// <summary>
            /// Получить или установить параметры схемы
            /// </summary>
            public Scheme SchemeParams { get; set; }
            /// <summary>
            /// Получить или установить список данных элементов схемы
            /// </summary>
            public List<ElementData> ElementDataList { get; set; }
            /// <summary>
            /// Получить или установить словарь изображений схемы
            /// </summary>
            public Dictionary<string, Image> ImageDict { get; set; }
        }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public enum ChangeType
        {
            /// <summary>
            /// Без изменений
            /// </summary>
            Unchanged,
            /// <summary>
            /// Схема изменена 
            /// </summary>
            SchemeChanged,
            /// <summary>
            /// Элемент добавлен
            /// </summary>
            ElementAdded,
            /// <summary>
            /// Элемент изменён
            /// </summary>
            ElementChanged,
            /// <summary>
            /// Элемент удалён
            /// </summary>
            ElementDeleted,
            /// <summary>
            /// Изображение добавлено
            /// </summary>
            ImageAdded,
            /// <summary>
            /// Изображение переименовано
            /// </summary>
            ImageRenamed,
            /// <summary>
            /// Изображение удалено
            /// </summary>
            ImageDeleted
        }

        /// <summary>
        /// Изменение схемы
        /// </summary>
        public class SchemeChange
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public SchemeChange()
                : this(ChangeType.Unchanged)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public SchemeChange(ChangeType changeType)
            {
                ChangeType = changeType;
                SchemeParams = null;
                ElementData = null;
                ElementID = -1;
                Image = null;
                ImageOldName = "";
                ImageNewName = "";
            }

            /// <summary>
            /// Получить или установить тип изменения
            /// </summary>
            public ChangeType ChangeType { get; set; }
            /// <summary>
            /// Получить или установить параметры схемы, если они изменились
            /// </summary>
            public Scheme SchemeParams { get; set; }
            /// <summary>
            /// Получить или установить данные добавленного или изменённого элемента
            /// </summary>
            public ElementData ElementData { get; set; }
            /// <summary>
            /// Получить или установить идентификатор удалённого элемента
            /// </summary>
            public int ElementID { get; set; }
            /// <summary>
            /// Получить или установить добавленное изображение
            /// </summary>
            public Image Image { get; set; }
            /// <summary>
            /// Получить или установить старое наименование изображения
            /// </summary>
            public string ImageOldName { get; set; }
            /// <summary>
            /// Получить или установить новое наименование изображения или наименование удаляемого изображения
            /// </summary>
            public string ImageNewName { get; set; }
        }
    }
}
