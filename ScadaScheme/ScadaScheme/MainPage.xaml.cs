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
 * Module   : SCADA-Scheme
 * Summary  : Main page of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2013
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Scada.Scheme.ScadaSchemeSvc;

namespace Scada.Scheme
{
    /// <summary>
    /// Main page of the application
    /// <para>Основная страница приложения</para>
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Информация об элементе схемы
        /// </summary>
        private class ElementInfo : IComparable<ElementInfo>
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            private ElementInfo()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public ElementInfo(SchemeViewElementData elementData)
            {
                StoreIndex = -1;
                ViewIndex = -1;
                Element = null;
                CnlData = null;

                if (elementData != null)
                {
                    ElementType = elementData.ElementType;

                    if (ElementType == SchemeViewElementTypes.StaticText)
                        Element = elementData.StaticText;
                    else if (ElementType == SchemeViewElementTypes.DynamicText)
                        Element = elementData.DynamicText;
                    else if (ElementType == SchemeViewElementTypes.StaticPicture)
                        Element = elementData.StaticPicture;
                    else if (ElementType == SchemeViewElementTypes.DynamicPicture)
                        Element = elementData.DynamicPicture;
                }

                if (Element == null)
                {
                    ZIndex = 0;
                    ID = 0;
                    ElementType = SchemeViewElementTypes.Unknown;
                }
                else
                {
                    ZIndex = Element.ZIndex;
                    ID = Element.ID;
                }
            }

            /// <summary>
            /// Получить или установить индекс, соответствующий порядку хранения элементов
            /// </summary>
            public int StoreIndex { get; set; }
            /// <summary>
            /// Получить или установить индекс, соответствующий порядку отображения элементов
            /// </summary>
            public int ViewIndex { get; set; }
            /// <summary>
            /// Получить порядок отображения
            /// </summary>
            public int ZIndex { get; private set; }
            /// <summary>
            /// Получить идентификатор элемента схемы
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// Получить тип элемента схемы
            /// </summary>
            public SchemeViewElementTypes ElementType { get; private set; }
            /// <summary>
            /// Получить элемент схемы
            /// </summary>
            public SchemeViewElement Element { get; private set; }
            /// <summary>
            /// Получить или установить данные входного канала, связанного с элементом
            /// </summary>
            public SchemeViewCnlData CnlData { get; set; }

            /// <summary>
            /// Сравнить текущий объект с другим объектом такого же типа
            /// </summary>
            public int CompareTo(ElementInfo other)
            {
                int compare = ZIndex.CompareTo(other.ZIndex);
                return compare == 0 ? StoreIndex.CompareTo(other.StoreIndex) : compare;
            }
        }


        /// <summary>
        /// Максимальный размер принимаемых от WCF-службы данных по умолчанию, 10 МБ
        /// </summary>
        private const int DefMaxMsgSize = 10485760;
        /// <summary>
        /// Частота выполнения запросов к WCF-службе по умолчанию, мс
        /// </summary>
        private const int DefReqFreq = 1000;
        /// <summary>
        /// Частота выполнения запросов к WCF-службе на получение изменения схемы, мс
        /// </summary>
        private const int ChangeReqFreq = 300;
        /// <summary>
        /// Продолжительность отображения сообщения об ошибке, с
        /// </summary>
        private const int ShowErrorDur = 30;
        /// <summary>
        /// Максимальная длина отображаемого сообщения об ошибке
        /// </summary>
        private const int MaxErrMsgLen = 200;
        /// <summary>
        /// Толщина рамки вокруг элементов
        /// </summary>
        private const double BorderWidth = 1.0;
        /// <summary>
        /// Шаг сетки в режиме редактирования
        /// </summary>
        private const int GridStep = 5;
        /// <summary>
        /// Обозначение цвета определяется статусом входного канала
        /// </summary>
        private const string StatusColor = "Status";
        /// <summary>
        /// Основной цвет схемы по умолчанию
        /// </summary>
        private static readonly Color DefSchemeForeColor = Colors.Black;
        /// <summary>
        /// Цвет фона схемы по умолчанию
        /// </summary>
        private static readonly Color DefSchemeBackColor = Colors.White;
        /// <summary>
        /// Цвет фона элемента схемы по умолчанию
        /// </summary>
        private static readonly Color DefElemBackColor = Colors.Transparent;
        /// <summary>
        /// Цвет рамки элемента схемы по умолчанию
        /// </summary>
        private static readonly Color DefElemBorderColor = Colors.Transparent;
        /// <summary>
        /// Цвет фона страницы по умолчанию
        /// </summary>
        private static readonly Color DefPageBackColor = Color.FromArgb(0xFF, 0xF7, 0xF7, 0xE7);
        /// <summary>
        /// Цвет фона страницы в режиме редактирования
        /// </summary>
        private static readonly Color PageEditBackColor = Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0);
        /// <summary>
        /// Цвет фона сообщения в режиме редактирования
        /// </summary>
        private static readonly Color MsgEditBackColor = Colors.White;
        /// <summary>
        /// Словарь известных цветов
        /// </summary>
        private static readonly Dictionary<string, Color> KnownColors;


        private readonly string clientID;             // идентификатор клиента
        private Action drawSchemeMethod;              // ссылка на метод DrawScheme
        private Action<SchemeViewSchemeChange> changeSchemeMethod; // ссылка на метод ChangeScheme
        private Action<string> showErrorMethod;       // ссылка на метод ShowError
        private Action hideErrorMethod;               // ссылка на метод HideError

        private ScadaSchemeSvcClient schemeSvcClient; // клиент WCF-службы для взаимодействия с сервером
        private SchemeSettings schemeSettings;        // настройки приложения
        private SchemePhrases schemePhrases;          // фразы, используемые приложением
        private SchemeViewSchemeData schemeData;      // представление схемы
        private Color schemeForeColor;                // основной цвет схемы
        private bool schemeFсIsStatus;                // основной цвет схемы определяется статусом входного канала
        private SchemeViewFont schemeFont;            // шрифт схемы
        private List<ElementInfo> elemInfoList;       // упорядоченный список информации об элементах схемы
        private Dictionary<int, ElementInfo> elemInfoDict; // словарь для доступа к элементам схемы по идентификатору
        private bool cnlNumsExists;                   // номера входных каналов, используемых схемой, существуют
        private bool cnlNumsNeeded;                   // требуется передача номеров входных каналов для запроса данных
        private List<SchemeViewCnlData> cnlDataList;  // данные входных каналов
        private ElementInfo hovElemInfo;              // информация об элементе схемы, на который наведён указатель мыши
        private ElementInfo selElemInfo;              // информация о выбранном элементе схемы в режиме редактирования
        private ScadaSchemeSvc.Point cursorPos;       // позиция указателя мыши на схеме в режиме редактирования
        private volatile int reqFreq;                 // частота выполнения запросов к WCF-службе, мс
        private volatile bool getSettingsCompleted;   // завершён асинхронный вызов метода GetSettings
        private volatile bool loadSchemeCompleted;    // завершён асинхронный вызов метода LoadScheme
        private volatile bool refrDataCompleted;      // завершён асинхронный вызов метода LoadCnlData или GetChange
        private object dataLock;                      // объект для синхронизации доступа к данным
        private bool errorVisible;                    // отображается ошибка
        private DateTime errorDT;                     // время последней ошибки


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static MainPage()
        {
            // заполнение словаря цветов
            KnownColors = new Dictionary<string, Color>();
            KnownColors.Add("transparent", Color.FromArgb(0, 255, 255, 255));
            KnownColors.Add("aliceblue", Color.FromArgb(255, 240, 248, 255));
            KnownColors.Add("antiquewhite", Color.FromArgb(255, 250, 235, 215));
            KnownColors.Add("aqua", Color.FromArgb(255, 0, 255, 255));
            KnownColors.Add("aquamarine", Color.FromArgb(255, 127, 255, 212));
            KnownColors.Add("azure", Color.FromArgb(255, 240, 255, 255));
            KnownColors.Add("beige", Color.FromArgb(255, 245, 245, 220));
            KnownColors.Add("bisque", Color.FromArgb(255, 255, 228, 196));
            KnownColors.Add("black", Color.FromArgb(255, 0, 0, 0));
            KnownColors.Add("blanchedalmond", Color.FromArgb(255, 255, 235, 205));
            KnownColors.Add("blue", Color.FromArgb(255, 0, 0, 255));
            KnownColors.Add("blueviolet", Color.FromArgb(255, 138, 43, 226));
            KnownColors.Add("brown", Color.FromArgb(255, 165, 42, 42));
            KnownColors.Add("burlywood", Color.FromArgb(255, 222, 184, 135));
            KnownColors.Add("cadetblue", Color.FromArgb(255, 95, 158, 160));
            KnownColors.Add("chartreuse", Color.FromArgb(255, 127, 255, 0));
            KnownColors.Add("chocolate", Color.FromArgb(255, 210, 105, 30));
            KnownColors.Add("coral", Color.FromArgb(255, 255, 127, 80));
            KnownColors.Add("cornflowerblue", Color.FromArgb(255, 100, 149, 237));
            KnownColors.Add("cornsilk", Color.FromArgb(255, 255, 248, 220));
            KnownColors.Add("crimson", Color.FromArgb(255, 220, 20, 60));
            KnownColors.Add("cyan", Color.FromArgb(255, 0, 255, 255));
            KnownColors.Add("darkblue", Color.FromArgb(255, 0, 0, 139));
            KnownColors.Add("darkcyan", Color.FromArgb(255, 0, 139, 139));
            KnownColors.Add("darkgoldenrod", Color.FromArgb(255, 184, 134, 11));
            KnownColors.Add("darkgray", Color.FromArgb(255, 169, 169, 169));
            KnownColors.Add("darkgreen", Color.FromArgb(255, 0, 100, 0));
            KnownColors.Add("darkkhaki", Color.FromArgb(255, 189, 183, 107));
            KnownColors.Add("darkmagenta", Color.FromArgb(255, 139, 0, 139));
            KnownColors.Add("darkolivegreen", Color.FromArgb(255, 85, 107, 47));
            KnownColors.Add("darkorange", Color.FromArgb(255, 255, 140, 0));
            KnownColors.Add("darkorchid", Color.FromArgb(255, 153, 50, 204));
            KnownColors.Add("darkred", Color.FromArgb(255, 139, 0, 0));
            KnownColors.Add("darksalmon", Color.FromArgb(255, 233, 150, 122));
            KnownColors.Add("darkseagreen", Color.FromArgb(255, 143, 188, 139));
            KnownColors.Add("darkslateblue", Color.FromArgb(255, 72, 61, 139));
            KnownColors.Add("darkslategray", Color.FromArgb(255, 47, 79, 79));
            KnownColors.Add("darkturquoise", Color.FromArgb(255, 0, 206, 209));
            KnownColors.Add("darkviolet", Color.FromArgb(255, 148, 0, 211));
            KnownColors.Add("deeppink", Color.FromArgb(255, 255, 20, 147));
            KnownColors.Add("deepskyblue", Color.FromArgb(255, 0, 191, 255));
            KnownColors.Add("dimgray", Color.FromArgb(255, 105, 105, 105));
            KnownColors.Add("dodgerblue", Color.FromArgb(255, 30, 144, 255));
            KnownColors.Add("firebrick", Color.FromArgb(255, 178, 34, 34));
            KnownColors.Add("floralwhite", Color.FromArgb(255, 255, 250, 240));
            KnownColors.Add("forestgreen", Color.FromArgb(255, 34, 139, 34));
            KnownColors.Add("fuchsia", Color.FromArgb(255, 255, 0, 255));
            KnownColors.Add("gainsboro", Color.FromArgb(255, 220, 220, 220));
            KnownColors.Add("ghostwhite", Color.FromArgb(255, 248, 248, 255));
            KnownColors.Add("gold", Color.FromArgb(255, 255, 215, 0));
            KnownColors.Add("goldenrod", Color.FromArgb(255, 218, 165, 32));
            KnownColors.Add("gray", Color.FromArgb(255, 128, 128, 128));
            KnownColors.Add("green", Color.FromArgb(255, 0, 128, 0));
            KnownColors.Add("greenyellow", Color.FromArgb(255, 173, 255, 47));
            KnownColors.Add("honeydew", Color.FromArgb(255, 240, 255, 240));
            KnownColors.Add("hotpink", Color.FromArgb(255, 255, 105, 180));
            KnownColors.Add("indianred", Color.FromArgb(255, 205, 92, 92));
            KnownColors.Add("indigo", Color.FromArgb(255, 75, 0, 130));
            KnownColors.Add("ivory", Color.FromArgb(255, 255, 255, 240));
            KnownColors.Add("khaki", Color.FromArgb(255, 240, 230, 140));
            KnownColors.Add("lavender", Color.FromArgb(255, 230, 230, 250));
            KnownColors.Add("lavenderblush", Color.FromArgb(255, 255, 240, 245));
            KnownColors.Add("lawngreen", Color.FromArgb(255, 124, 252, 0));
            KnownColors.Add("lemonchiffon", Color.FromArgb(255, 255, 250, 205));
            KnownColors.Add("lightblue", Color.FromArgb(255, 173, 216, 230));
            KnownColors.Add("lightcoral", Color.FromArgb(255, 240, 128, 128));
            KnownColors.Add("lightcyan", Color.FromArgb(255, 224, 255, 255));
            KnownColors.Add("lightgoldenrodyellow", Color.FromArgb(255, 250, 250, 210));
            KnownColors.Add("lightgray", Color.FromArgb(255, 211, 211, 211));
            KnownColors.Add("lightgreen", Color.FromArgb(255, 144, 238, 144));
            KnownColors.Add("lightpink", Color.FromArgb(255, 255, 182, 193));
            KnownColors.Add("lightsalmon", Color.FromArgb(255, 255, 160, 122));
            KnownColors.Add("lightseagreen", Color.FromArgb(255, 32, 178, 170));
            KnownColors.Add("lightskyblue", Color.FromArgb(255, 135, 206, 250));
            KnownColors.Add("lightslategray", Color.FromArgb(255, 119, 136, 153));
            KnownColors.Add("lightsteelblue", Color.FromArgb(255, 176, 196, 222));
            KnownColors.Add("lightyellow", Color.FromArgb(255, 255, 255, 224));
            KnownColors.Add("lime", Color.FromArgb(255, 0, 255, 0));
            KnownColors.Add("limegreen", Color.FromArgb(255, 50, 205, 50));
            KnownColors.Add("linen", Color.FromArgb(255, 250, 240, 230));
            KnownColors.Add("magenta", Color.FromArgb(255, 255, 0, 255));
            KnownColors.Add("maroon", Color.FromArgb(255, 128, 0, 0));
            KnownColors.Add("mediumaquamarine", Color.FromArgb(255, 102, 205, 170));
            KnownColors.Add("mediumblue", Color.FromArgb(255, 0, 0, 205));
            KnownColors.Add("mediumorchid", Color.FromArgb(255, 186, 85, 211));
            KnownColors.Add("mediumpurple", Color.FromArgb(255, 147, 112, 219));
            KnownColors.Add("mediumseagreen", Color.FromArgb(255, 60, 179, 113));
            KnownColors.Add("mediumslateblue", Color.FromArgb(255, 123, 104, 238));
            KnownColors.Add("mediumspringgreen", Color.FromArgb(255, 0, 250, 154));
            KnownColors.Add("mediumturquoise", Color.FromArgb(255, 72, 209, 204));
            KnownColors.Add("mediumvioletred", Color.FromArgb(255, 199, 21, 133));
            KnownColors.Add("midnightblue", Color.FromArgb(255, 25, 25, 112));
            KnownColors.Add("mintcream", Color.FromArgb(255, 245, 255, 250));
            KnownColors.Add("mistyrose", Color.FromArgb(255, 255, 228, 225));
            KnownColors.Add("moccasin", Color.FromArgb(255, 255, 228, 181));
            KnownColors.Add("navajowhite", Color.FromArgb(255, 255, 222, 173));
            KnownColors.Add("navy", Color.FromArgb(255, 0, 0, 128));
            KnownColors.Add("oldlace", Color.FromArgb(255, 253, 245, 230));
            KnownColors.Add("olive", Color.FromArgb(255, 128, 128, 0));
            KnownColors.Add("olivedrab", Color.FromArgb(255, 107, 142, 35));
            KnownColors.Add("orange", Color.FromArgb(255, 255, 165, 0));
            KnownColors.Add("orangered", Color.FromArgb(255, 255, 69, 0));
            KnownColors.Add("orchid", Color.FromArgb(255, 218, 112, 214));
            KnownColors.Add("palegoldenrod", Color.FromArgb(255, 238, 232, 170));
            KnownColors.Add("palegreen", Color.FromArgb(255, 152, 251, 152));
            KnownColors.Add("paleturquoise", Color.FromArgb(255, 175, 238, 238));
            KnownColors.Add("palevioletred", Color.FromArgb(255, 219, 112, 147));
            KnownColors.Add("papayawhip", Color.FromArgb(255, 255, 239, 213));
            KnownColors.Add("peachpuff", Color.FromArgb(255, 255, 218, 185));
            KnownColors.Add("peru", Color.FromArgb(255, 205, 133, 63));
            KnownColors.Add("pink", Color.FromArgb(255, 255, 192, 203));
            KnownColors.Add("plum", Color.FromArgb(255, 221, 160, 221));
            KnownColors.Add("powderblue", Color.FromArgb(255, 176, 224, 230));
            KnownColors.Add("purple", Color.FromArgb(255, 128, 0, 128));
            KnownColors.Add("red", Color.FromArgb(255, 255, 0, 0));
            KnownColors.Add("rosybrown", Color.FromArgb(255, 188, 143, 143));
            KnownColors.Add("royalblue", Color.FromArgb(255, 65, 105, 225));
            KnownColors.Add("saddlebrown", Color.FromArgb(255, 139, 69, 19));
            KnownColors.Add("salmon", Color.FromArgb(255, 250, 128, 114));
            KnownColors.Add("sandybrown", Color.FromArgb(255, 244, 164, 96));
            KnownColors.Add("seagreen", Color.FromArgb(255, 46, 139, 87));
            KnownColors.Add("seashell", Color.FromArgb(255, 255, 245, 238));
            KnownColors.Add("sienna", Color.FromArgb(255, 160, 82, 45));
            KnownColors.Add("silver", Color.FromArgb(255, 192, 192, 192));
            KnownColors.Add("skyblue", Color.FromArgb(255, 135, 206, 235));
            KnownColors.Add("slateblue", Color.FromArgb(255, 106, 90, 205));
            KnownColors.Add("slategray", Color.FromArgb(255, 112, 128, 144));
            KnownColors.Add("snow", Color.FromArgb(255, 255, 250, 250));
            KnownColors.Add("springgreen", Color.FromArgb(255, 0, 255, 127));
            KnownColors.Add("steelblue", Color.FromArgb(255, 70, 130, 180));
            KnownColors.Add("tan", Color.FromArgb(255, 210, 180, 140));
            KnownColors.Add("teal", Color.FromArgb(255, 0, 128, 128));
            KnownColors.Add("thistle", Color.FromArgb(255, 216, 191, 216));
            KnownColors.Add("tomato", Color.FromArgb(255, 255, 99, 71));
            KnownColors.Add("turquoise", Color.FromArgb(255, 64, 224, 208));
            KnownColors.Add("violet", Color.FromArgb(255, 238, 130, 238));
            KnownColors.Add("wheat", Color.FromArgb(255, 245, 222, 179));
            KnownColors.Add("white", Color.FromArgb(255, 255, 255, 255));
            KnownColors.Add("whitesmoke", Color.FromArgb(255, 245, 245, 245));
            KnownColors.Add("yellow", Color.FromArgb(255, 255, 255, 0));
            KnownColors.Add("yellowgreen", Color.FromArgb(255, 154, 205, 50));
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // инициализация полей и свойств
            clientID = Guid.NewGuid().ToString();
            drawSchemeMethod = DrawScheme;
            changeSchemeMethod = ChangeScheme;
            showErrorMethod = ShowError;
            hideErrorMethod = HideError;
            schemeSvcClient = null;
            schemeSettings = null;
            schemeData = null;
            schemeForeColor = DefSchemeForeColor;
            schemeFсIsStatus = false;
            schemeFont = null;
            elemInfoList = null;
            elemInfoDict = null;
            cnlNumsExists = false;
            cnlNumsNeeded = false;
            cnlDataList = null;
            hovElemInfo = null;
            selElemInfo = null;
            cursorPos = new ScadaSchemeSvc.Point() { x = -1, y = -1 };
            reqFreq = DefReqFreq;
            getSettingsCompleted = false;
            loadSchemeCompleted = false;
            refrDataCompleted = false;
            dataLock = new object();
            errorVisible = false;
            errorDT = DateTime.MinValue;

            SchemeSvcURI = "";
            MaxMsgSize = 0;
            ViewSetIndex = 0;
            ViewIndex = 0;
            DiagDate = DateTime.Today;
            EditMode = false;

            // инициализация фраз
            SetPhrasesToDefault();

            // инициализация элементов управления
            bMessage.Visibility = Visibility.Collapsed;
            grdScheme.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// Получить или установить адрес WCF-службы для взаимодействия с сервером
        /// </summary>
        public string SchemeSvcURI { get; set; }

        /// <summary>
        /// Получить или установить максимальный размер принимаемых от WCF-службы данных
        /// </summary>
        public int MaxMsgSize { get; set; }

        /// <summary>
        /// Получить или установить индекс набора представлений, выбранного пользователем
        /// </summary>
        public int ViewSetIndex { get; set; }

        /// <summary>
        /// Получить или установить индекс представления, выбранного пользователем
        /// </summary>
        public int ViewIndex { get; set; }

        /// <summary>
        /// Получить или установить дату, используемую для построения графиков
        /// </summary>
        public DateTime DiagDate { get; set; }

        /// <summary>
        /// Получить или установить признак работы в режиме редактирования
        /// </summary>
        public bool EditMode { get; set; }


        /// <summary>
        /// Преобразовать строку в цвет
        /// </summary>
        private Color ConvertToColor(string colorName, Color defaultColor)
        {
            colorName = colorName == null ? "" : colorName.Trim();

            if (colorName == "" || ColorIsStatus(colorName))
            {
                return defaultColor;
            }
            else if (colorName.StartsWith("#"))
            {
                try
                {
                    int len = colorName.Length;
                    byte b = byte.Parse(colorName.Substring(len - 2, 2), NumberStyles.HexNumber);
                    byte g = byte.Parse(colorName.Substring(len - 4, 2), NumberStyles.HexNumber);
                    byte r = byte.Parse(colorName.Substring(len - 6, 2), NumberStyles.HexNumber);
                    byte a = len < 9 ? (byte)0xFF : byte.Parse(colorName.Substring(len - 8, 2), NumberStyles.HexNumber);
                    return Color.FromArgb(a, r, g, b);
                }
                catch
                {
                    return defaultColor;
                }
            }
            else
            {
                Color color;
                return KnownColors.TryGetValue(colorName.ToLower(), out color) ? color : defaultColor;
            }
        }

        /// <summary>
        /// Проверить определяется ли цвет статусом входного канала
        /// </summary>
        private bool ColorIsStatus(string colorName)
        {
            return string.Compare(colorName, StatusColor, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Получить источник данных изображения
        /// </summary>
        private BitmapImage GetImageSource(SchemeViewImage image)
        {
            if (image == null)
            {
                return null;
            }
            else if (image.Source == null)
            {
                SchemeViewImage imageFromDict;

                if (schemeData.ImageDict != null && schemeData.ImageDict.TryGetValue(image.Name, out imageFromDict))
                {
                    if (imageFromDict.Source == null)
                    {
                        try
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(new MemoryStream(imageFromDict.Data));
                            image.Source = imageFromDict.Source = bitmapImage;
                            return bitmapImage;
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    else
                    {
                        image.Source = imageFromDict.Source;
                        return image.Source as BitmapImage;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return image.Source as BitmapImage;
            }
        }

        /// <summary>
        /// Получить кисть для использования при наведении мыши на элемент
        /// </summary>
        private Brush GetBrush(ElementInfo elemInfo, string colorName, Color defColor)
        {
            Brush brush = null;

            if (ColorIsStatus(colorName))
            {
                if (elemInfo.CnlData != null)
                    brush = new SolidColorBrush(ConvertToColor(elemInfo.CnlData.Color, defColor));
            }
            else if (!string.IsNullOrEmpty(colorName))
            {
                brush = new SolidColorBrush(ConvertToColor(colorName, defColor));
            }

            return brush;
        }

        /// <summary>
        /// Сравнить значения
        /// </summary>
        private bool Compare(double val1, double val2, SchemeViewCompareOperator oper)
        {
            switch (oper)
            {
                case SchemeViewCompareOperator.Equal:
                    return val1 == val2;
                case SchemeViewCompareOperator.NotEqual:
                    return val1 != val2;
                case SchemeViewCompareOperator.GreaterThan:
                    return val1 > val2;
                case SchemeViewCompareOperator.GreaterThanEqual:
                    return val1 >= val2;
                case SchemeViewCompareOperator.LessThan:
                    return val1 < val2;
                case SchemeViewCompareOperator.LessThanEqual:
                    return val1 <= val2;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Определить, являются ли шрифты идентичными
        /// </summary>
        private bool FontsAreEqual(SchemeViewFont font1, SchemeViewFont font2)
        {
            if (font1 == null && font2 == null)
                return true;
            else if (font1 == null && font2 != null || font1 != null && font2 == null)
                return false;
            else
                return font1.Name == font2.Name && font1.Size == font2.Size && 
                    font1.Bold == font2.Bold && font1.Italic == font2.Italic && font1.Underline != font2.Underline;

        }

        /// <summary>
        /// Притянуть точку к сетке
        /// </summary>
        private void PullPoint(System.Windows.Point p, out int x, out int y)
        {
            x = (int)p.X;
            int xMod = x % GridStep;
            x -= xMod;
            if (xMod > (double)GridStep / 2)
                x += GridStep;

            y = (int)p.Y;
            int yMod = y % GridStep;
            y -= yMod;
            if (yMod > (double)GridStep / 2)
                y += GridStep;
        }
        

        /// <summary>
        /// Выполнение работы приложения
        /// </summary>
        private void Execute()
        {
            while (true)
            {
                DateTime stepDT = DateTime.Now;
                reqFreq = DefReqFreq;

                // загрузка настроек приложения
                if (schemeSettings == null)
                {
                    getSettingsCompleted = false;
                    schemeSvcClient.GetSettingsAsync();
                }
                else
                {
                    getSettingsCompleted = true;
                }

                // загрузка схемы
                if (schemeSettings != null && schemeData == null)
                {
                    loadSchemeCompleted = false;
                    schemeSvcClient.LoadSchemeAsync(clientID, ViewSetIndex, ViewIndex);
                }
                else
                {
                    loadSchemeCompleted = true;
                }

                if (EditMode)
                {
                    // получение изменения схемы
                    reqFreq = ChangeReqFreq;
                    refrDataCompleted = false;
                    schemeSvcClient.GetChangeAsync(clientID, cursorPos);
                }
                else
                {
                    // загрузка данных входных каналов
                    if (cnlNumsExists)
                    {
                        reqFreq = schemeSettings.RefrFreq * 1000;
                        refrDataCompleted = false;
                        schemeSvcClient.LoadCnlDataAsync(clientID, cnlNumsNeeded ? schemeData.CnlList : null);
                    }
                    else
                    {
                        refrDataCompleted = true;
                    }
                }

                // скрытие сообщения об ошибке
                if (errorVisible && (stepDT - errorDT).Seconds > ShowErrorDur)
                    Dispatcher.BeginInvoke(hideErrorMethod);

                // пауза для соблюдения частоты обновления данных, 
                // а также ожидание завершения вызовов асинхронных методов
                do
                {
                    Thread.Sleep(100);
                }
                while (!((DateTime.Now - stepDT).TotalMilliseconds >= reqFreq &&
                    getSettingsCompleted && loadSchemeCompleted && refrDataCompleted));
            }
        }

        /// <summary>
        /// Заполнить список информации об элементах схемы
        /// </summary>
        private void FillElemInfoList()
        {
            elemInfoList = new List<ElementInfo>();
            elemInfoDict = new Dictionary<int, ElementInfo>();

            if (schemeData.ElementDataList != null)
            {
                // заполнение списка, вычисление индексов хранения
                int storeIndex = 0;

                foreach (SchemeViewElementData elemData in schemeData.ElementDataList)
                {
                    ElementInfo elemInfo = new ElementInfo(elemData);

                    if (elemInfo.ElementType != SchemeViewElementTypes.Unknown &&
                        elemInfo.ID > 0 && !elemInfoDict.ContainsKey(elemInfo.ID))
                    {
                        elemInfo.StoreIndex = storeIndex++;
                        elemInfoList.Add(elemInfo);
                        elemInfoDict.Add(elemInfo.ID, elemInfo);
                    }
                }

                // сортировка списка, вычисление индексов отображения
                elemInfoList.Sort();

                if (EditMode)
                    CalcViewIndexes();
            }
        }

        /// <summary>
        /// Вычислить индексы отображения в списке информации об элементах схемы
        /// </summary>
        private void CalcViewIndexes()
        {
            int viewIndex = 0;
            foreach (ElementInfo elemInfo in elemInfoList)
                elemInfo.ViewIndex = viewIndex++;
        }

        /// <summary>
        /// Уменьшить на единицу индексы хранения в списке информации об элементах схемы
        /// </summary>
        private void DecStoreIndexes(int startIndex)
        {
            foreach (ElementInfo elemInfo in elemInfoList)
            {
                if (elemInfo.StoreIndex >= startIndex)
                    elemInfo.StoreIndex--;
            }
        }

        /// <summary>
        /// Создать элемент для отображения статической или динамической надписи
        /// </summary>
        private FrameworkElement CreateText(SchemeViewStaticText textElem)
        {
            // создание рамки
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(ConvertToColor(textElem.BorderColor, DefElemBorderColor));
            border.Background = new SolidColorBrush(ConvertToColor(textElem.BackColor, DefElemBackColor));
            border.BorderThickness = new Thickness(BorderWidth);
            border.SetValue(Canvas.LeftProperty, (double)textElem.Location.X - BorderWidth);
            border.SetValue(Canvas.TopProperty, (double)textElem.Location.Y - BorderWidth);

            if (!textElem.AutoSize)
            {
                border.Width = textElem.Size.Width + BorderWidth * 2;
                border.Height = textElem.Size.Height + BorderWidth * 2;
            }

            // создание текстового блока
            TextBlock textBlock = new TextBlock();
            textBlock.Tag = textElem;
            textBlock.Text = textElem.Text;
            textBlock.Foreground = new SolidColorBrush(ConvertToColor(textElem.ForeColor, schemeForeColor));

            if (!textElem.AutoSize)
            {
                if (textElem.HAlign == SchemeViewHorizontalAlignment.Center)
                {
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.TextAlignment = TextAlignment.Center;
                }
                else if (textElem.HAlign == SchemeViewHorizontalAlignment.Right)
                {
                    textBlock.HorizontalAlignment = HorizontalAlignment.Right;
                    textBlock.TextAlignment = TextAlignment.Right;
                }
                else
                {
                    textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    textBlock.TextAlignment = TextAlignment.Left;
                }

                if (textElem.VAlign == SchemeViewVerticalAlignment.Center)
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                else if (textElem.VAlign == SchemeViewVerticalAlignment.Bottom)
                    textBlock.VerticalAlignment = VerticalAlignment.Bottom;
                else
                    textBlock.VerticalAlignment = VerticalAlignment.Top;

                if (textElem.WordWrap)
                    textBlock.TextWrapping = TextWrapping.Wrap;
            }

            // установка шрифта
            SchemeViewFont font = textElem.Font == null ? schemeFont : textElem.Font;

            if (font != null)
            {
                if (!string.IsNullOrEmpty(font.Name))
                    textBlock.FontFamily = new FontFamily(font.Name);
                if (font.Size > 0)
                    textBlock.FontSize = font.Size;
                if (font.Italic)
                    textBlock.FontStyle = FontStyles.Italic;
                if (font.Bold)
                    textBlock.FontWeight = FontWeights.Bold;
                if (font.Underline)
                    textBlock.TextDecorations = TextDecorations.Underline;
            }

            // дополнительная настройка текстового блока для динамической надписи
            if (textElem is SchemeViewDynamicText)
            {
                SchemeViewDynamicText dynamicText = (SchemeViewDynamicText)textElem;

                if (!string.IsNullOrEmpty(dynamicText.ToolTip))
                    ToolTipService.SetToolTip(border, dynamicText.ToolTip);

                if (dynamicText.UnderlineOnHover || 
                    !string.IsNullOrEmpty(dynamicText.BackColorOnHover) ||
                    !string.IsNullOrEmpty(dynamicText.BorderColorOnHover) || 
                    !string.IsNullOrEmpty(dynamicText.ForeColorOnHover))
                {
                    border.MouseEnter += textBorder_MouseEnter;
                    border.MouseLeave += textBorder_MouseLeave;
                }

                if (dynamicText.Action != SchemeViewAction.None)
                    border.Cursor = Cursors.Hand;
            }

            border.Child = textBlock;
            border.MouseLeftButtonDown += elemBorder_MouseLeftButtonDown;
            border.MouseLeftButtonUp += elemBorder_MouseLeftButtonUp;

            return border;
        }

        /// <summary>
        /// Создать элемент для отображения статического или динамического рисунка
        /// </summary>
        private FrameworkElement CreatePicture(SchemeViewStaticPicture picElem)
        {
            // создание рамки
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(ConvertToColor(picElem.BorderColor, DefElemBorderColor));
            border.Background = new SolidColorBrush(DefElemBackColor);
            border.BorderThickness = new Thickness(BorderWidth);
            border.SetValue(Canvas.LeftProperty, (double)picElem.Location.X - BorderWidth);
            border.SetValue(Canvas.TopProperty, (double)picElem.Location.Y - BorderWidth);

            // создание изображения
            Image image = new Image();
            image.Tag = picElem;
            image.Width = picElem.Size.Width;
            image.Height = picElem.Size.Height;
            image.Source = GetImageSource(picElem.Image);

            if (picElem.ImageStretch == SchemeViewImageStretch.Fill)
                image.Stretch = Stretch.Fill;
            else if (picElem.ImageStretch == SchemeViewImageStretch.Zoom)
                image.Stretch = Stretch.Uniform;
            else
                image.Stretch = Stretch.None;

            // дополнительная настройка изображения для динамического рисунка
            if (picElem is SchemeViewDynamicPicture)
            {
                SchemeViewDynamicPicture dynamicPicture = (SchemeViewDynamicPicture)picElem;

                if (!string.IsNullOrEmpty(dynamicPicture.ToolTip))
                    ToolTipService.SetToolTip(border, dynamicPicture.ToolTip);

                if (!string.IsNullOrEmpty(dynamicPicture.BorderColorOnHover) || dynamicPicture.ImageOnHover != null)
                {
                    border.MouseEnter += imageBorder_MouseEnter;
                    border.MouseLeave += imageBorder_MouseLeave;
                }

                if (dynamicPicture.Action != SchemeViewAction.None)
                    border.Cursor = Cursors.Hand;
            }

            border.Child = image;
            border.MouseLeftButtonDown += elemBorder_MouseLeftButtonDown;
            border.MouseLeftButtonUp += elemBorder_MouseLeftButtonUp;

            return border;
        }
        
        /// <summary>
        /// Создать графический элемент для отображения на полотне
        /// </summary>
        private FrameworkElement CreateFrElem(ElementInfo elemInfo)
        {
            FrameworkElement frElem;

            if (elemInfo.ElementType == SchemeViewElementTypes.StaticText ||
                elemInfo.ElementType == SchemeViewElementTypes.DynamicText)
            {
                frElem = CreateText((SchemeViewStaticText)elemInfo.Element);
            }
            else // StaticPicture или DynamicPicture
            {
                frElem = CreatePicture((SchemeViewStaticPicture)elemInfo.Element);
            }

            frElem.Tag = elemInfo;
            return frElem;
        }

        /// <summary>
        /// Настроить область схемы
        /// </summary>
        private void TuneScheme()
        {
            // получение параметров схемы
            SchemeViewScheme schemeParams = schemeData.SchemeParams;

            if (schemeParams != null)
            {
                // получение основного цвета и шрифта схемы
                schemeForeColor = ConvertToColor(schemeParams.ForeColor, DefSchemeForeColor);
                schemeFсIsStatus = ColorIsStatus(schemeParams.ForeColor);
                schemeFont = schemeParams.Font;

                // настройка размеров и фона схемы
                grdScheme.Visibility = Visibility.Visible;
                grdScheme.Width = cnvScheme.Width = schemeParams.Size.Width;
                grdScheme.Height = cnvScheme.Height = schemeParams.Size.Height;
                ((RectangleGeometry)grdScheme.Clip).Rect = new Rect(0, 0, cnvScheme.Width, cnvScheme.Height);
                grdScheme.Background = new SolidColorBrush(ConvertToColor(schemeParams.BackColor, DefSchemeBackColor));

                if (schemeParams.BackImage == null)
                {
                    cnvScheme.Background = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = GetImageSource(schemeParams.BackImage);
                    cnvScheme.Background = imageBrush;
                }

                if (!EditMode)
                    LayoutRoot.Background = new SolidColorBrush(
                        ConvertToColor(schemeParams.BackColor, DefPageBackColor));
            }
        }

        /// <summary>
        /// Создать графические элементы на полотне схемы
        /// </summary>
        public void CreateElements()
        {
            foreach (ElementInfo elemInfo in elemInfoList)
                cnvScheme.Children.Add(CreateFrElem(elemInfo));
        }

        /// <summary>
        /// Отобразить данные входных каналов на схеме
        /// </summary>
        private void ShowCnlData()
        {
            List<int> cnlList = schemeData.CnlList;
            int cnlDataListCnt = cnlDataList == null ? 0 : cnlDataList.Count;

            if (cnlList != null && cnlDataListCnt > 0)
            {
                foreach (FrameworkElement frElem in cnvScheme.Children)
                {
                    Border border = frElem as Border;
                    ElementInfo elemInfo = frElem.Tag as ElementInfo;

                    if (border != null && elemInfo != null)
                    {
                        if (elemInfo.ElementType == SchemeViewElementTypes.DynamicText)
                        {
                            // изменение текстового блока для динамической надписи
                            TextBlock textBlock = (TextBlock)border.Child;
                            SchemeViewDynamicText dynamicText = (SchemeViewDynamicText)elemInfo.Element;
                            int cnlNumInd = dynamicText.InCnlNum > 0 ? cnlList.BinarySearch(dynamicText.InCnlNum) : -1;

                            if (0 <= cnlNumInd && cnlNumInd < cnlDataListCnt)
                            {
                                // получение данных входного канала, связанного с динамической надписью
                                SchemeViewCnlData cnlData = cnlDataList[cnlNumInd];
                                elemInfo.CnlData = cnlData;

                                // установка текста
                                if (dynamicText.ShowValue == SchemeViewShowValue.ShowWithUnit)
                                    textBlock.Text = cnlData.ValStrWithUnit;
                                else if (dynamicText.ShowValue == SchemeViewShowValue.ShowWithoutUnit)
                                    textBlock.Text = cnlData.ValStr;

                                // установка цветов, если они определяются статусом
                                string backColor;
                                string borderColor;
                                string foreColor;

                                if (elemInfo == hovElemInfo)
                                {
                                    backColor = dynamicText.BackColorOnHover;
                                    borderColor = dynamicText.BorderColorOnHover;
                                    foreColor = dynamicText.ForeColorOnHover;
                                }
                                else
                                {
                                    backColor = dynamicText.BackColor;
                                    borderColor = dynamicText.BorderColor;
                                    foreColor = dynamicText.ForeColor;
                                }

                                if (ColorIsStatus(backColor))
                                    border.Background = new SolidColorBrush(ConvertToColor(cnlData.Color, DefElemBackColor));

                                if (ColorIsStatus(borderColor))
                                    border.BorderBrush = new SolidColorBrush(ConvertToColor(cnlData.Color, DefElemBorderColor));

                                if (ColorIsStatus(foreColor) || 
                                    string.IsNullOrEmpty(dynamicText.ForeColor) && schemeFсIsStatus)
                                    textBlock.Foreground = new SolidColorBrush(ConvertToColor(cnlData.Color, schemeForeColor));
                            }
                        }
                        else if (elemInfo.ElementType == SchemeViewElementTypes.DynamicPicture)
                        {
                            // изменение изображения для динамического рисунка
                            Image image = (Image)border.Child;
                            SchemeViewDynamicPicture dynamicPicture = (SchemeViewDynamicPicture)elemInfo.Element;
                            int cnlNumInd = dynamicPicture.InCnlNum > 0 ? 
                                cnlList.BinarySearch(dynamicPicture.InCnlNum) : -1;

                            if (0 <= cnlNumInd && cnlNumInd < cnlDataListCnt)
                            {
                                // получение данных входного канала, связанного с динамическим рисунком
                                SchemeViewCnlData cnlData = cnlDataList[cnlNumInd];
                                elemInfo.CnlData = cnlData;

                                // выбор изображения в зависимости от выполнения условий
                                if (dynamicPicture.Conditions != null && dynamicPicture.Conditions.Count > 0)
                                {
                                    BitmapSource bitmapSource = GetImageSource(dynamicPicture.Image);

                                    if (cnlData.Stat > 0)
                                    {
                                        double cnlVal = cnlData.Val;

                                        foreach (SchemeViewCondition cond in dynamicPicture.Conditions)
                                        {
                                            bool result = Compare(cnlVal, cond.CompareArgument1, cond.CompareOperator1);

                                            if (cond.LogicalOperator == SchemeViewLogicalOperator.And)
                                                result = result && Compare(cnlVal, cond.CompareArgument2, cond.CompareOperator2);
                                            else if (cond.LogicalOperator == SchemeViewLogicalOperator.Or)
                                                result = result || Compare(cnlVal, cond.CompareArgument2, cond.CompareOperator2);

                                            if (result)
                                            {
                                                bitmapSource = GetImageSource(cond.Image);
                                                break;
                                            }
                                        }
                                    }

                                    if (image.Source != bitmapSource)
                                        image.Source = bitmapSource;
                                }

                                // установка цвета рамки, если он определяются статусом
                                string borderColor = elemInfo == hovElemInfo ? 
                                    dynamicPicture.BorderColorOnHover : dynamicPicture.BorderColor;

                                if (ColorIsStatus(borderColor))
                                    border.BorderBrush = new SolidColorBrush(ConvertToColor(cnlData.Color, DefElemBorderColor));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Отобразить схему
        /// </summary>
        private void DrawScheme()
        {
            try
            {
                lock (dataLock)
                {
                    if (elemInfoList == null /*первое отображение схемы*/)
                    {
                        // скрытие сообщения об отсутствии данных
                        tbNoDataMessage.Visibility = Visibility.Collapsed;

                        // заполнение списка информации об элементах схемы
                        FillElemInfoList();

                        // настройка полотна и создание графических элементов
                        TuneScheme();
                        CreateElements();
                        cnvScheme.MouseMove += cnvScheme_MouseMove;
                        cnvScheme.MouseLeave += cnvScheme_MouseLeave;
                        cnvScheme.MouseLeftButtonDown += cnvScheme_MouseLeftButtonDown;
                    }
                    else
                    {
                        // отображение данных входных каналов на схеме
                        ShowCnlData();
                    }
                }
            }
            catch (Exception ex)
            {
                string s = schemePhrases.ErrorDrawingScheme + ": " + ex.Message;
                Dispatcher.BeginInvoke(showErrorMethod, s);
                schemeSvcClient.WriteExceptionAsync(s);
            }
        }

        /// <summary>
        /// Обновить статические и динамические надписи
        /// </summary>
        private void UpdateTexts()
        {
            int cnt = cnvScheme.Children.Count;

            for (int i = 0; i < cnt; i++)
            {
                FrameworkElement frElem = (FrameworkElement)cnvScheme.Children[i];
                ElementInfo elemInfo = frElem.Tag as ElementInfo;
                
                if (elemInfo != null && (elemInfo.ElementType == SchemeViewElementTypes.StaticText || 
                    elemInfo.ElementType == SchemeViewElementTypes.DynamicText))
                {
                    // создание элемента для отображения надписи
                    FrameworkElement newFrElem = CreateText((SchemeViewStaticText)elemInfo.Element);
                    newFrElem.Tag = elemInfo;
                    cnvScheme.Children[i] = newFrElem;

                    // обновление выделения элемента
                    if (elemInfo == selElemInfo)
                        SelectFrElem(newFrElem);
                }
            }
        }

        /// <summary>
        /// Обновить статические и динамические рисунки, а также фон схемы, если они используют заданные изображения
        /// </summary>
        private void UpdatePictures(string imageName1, string imageName2 = "")
        {
            // обновление рисунков
            int cnt = cnvScheme.Children.Count;

            for (int i = 0; i < cnt; i++)
            {
                FrameworkElement frElem = (FrameworkElement)cnvScheme.Children[i];
                ElementInfo elemInfo = frElem.Tag as ElementInfo;

                if (elemInfo != null && (elemInfo.ElementType == SchemeViewElementTypes.StaticPicture ||
                    elemInfo.ElementType == SchemeViewElementTypes.DynamicPicture))
                {
                    SchemeViewStaticPicture staticPicture = (SchemeViewStaticPicture)elemInfo.Element;
                    SchemeViewDynamicPicture dynamicPicture = elemInfo.Element as SchemeViewDynamicPicture;
                    SchemeViewImage image1 = staticPicture.Image;
                    SchemeViewImage image2 = dynamicPicture == null || dynamicPicture.ImageOnHover == null ? 
                        null : dynamicPicture.ImageOnHover;

                    if (image1 != null && (image1.Name == imageName1 || image1.Name == imageName2) ||
                        image2 != null && (image2.Name == imageName1 || image2.Name == imageName2))
                    {
                        if (image1 != null)
                            image1.Source = null;

                        if (image2 != null)
                            image2.Source = null;

                        FrameworkElement newFrElem = CreatePicture(staticPicture);
                        newFrElem.Tag = elemInfo;
                        cnvScheme.Children[i] = newFrElem;
                    }
                }
            }

            // обновление фона схемы
            SchemeViewImage backImage = schemeData.SchemeParams == null ? null : schemeData.SchemeParams.BackImage;

            if (backImage != null && (backImage.Name == imageName1 || backImage.Name == imageName2))
            {
                backImage.Source = null;
                TuneScheme();
            }
        }

        /// <summary>
        /// Выбрать графический элемент, выделив его рамкой
        /// </summary>
        private void SelectFrElem(FrameworkElement frElem)
        {
            ElementInfo elemInfo = frElem == null ? null : frElem.Tag as ElementInfo;

            if (elemInfo == null)
            {
                // скрытие рамки
                rctSelElem.Visibility = Visibility.Collapsed;
                selElemInfo = null;
            }
            else
            {
                // установка положения, размеров и отображение рамки
                frElem.UpdateLayout();
                rctSelElem.Margin = new Thickness((double)frElem.GetValue(Canvas.LeftProperty) - 3, 
                    (double)frElem.GetValue(Canvas.TopProperty) - 3, 0, 0);
                rctSelElem.Width = frElem.ActualWidth + 6;
                rctSelElem.Height = frElem.ActualHeight + 6;
                rctSelElem.Visibility = Visibility.Visible;
                selElemInfo = elemInfo;
            }
        }

        /// <summary>
        /// Изменить схему
        /// </summary>
        private void ChangeScheme(SchemeViewSchemeChange change)
        {
            try
            {
                if (change != null)
                {
                    if (change.ChangeType == SchemeViewChangeType.SchemeChanged)
                    {
                        // изменение полотна схемы
                        SchemeViewScheme oldSchemeParams = schemeData.SchemeParams;
                        SchemeViewScheme newSchemeParams = change.SchemeParams;

                        if (oldSchemeParams != null && newSchemeParams != null)
                        {
                            schemeData.SchemeParams = newSchemeParams;
                            TuneScheme();

                            if (oldSchemeParams.ForeColor != newSchemeParams.ForeColor ||
                                !FontsAreEqual(oldSchemeParams.Font, newSchemeParams.Font))
                            {
                                UpdateTexts();
                            }
                        }
                    }
                    else if (change.ChangeType == SchemeViewChangeType.ElementAdded)
                    {
                        // добавление элемента схемы
                        ElementInfo newElemInfo = new ElementInfo(change.ElementData);

                        if (newElemInfo.ID > 0)
                        {
                            // установка индекса хранения
                            newElemInfo.StoreIndex = elemInfoList.Count;

                            // вставка нового элемента
                            int insertIndex = ~elemInfoList.BinarySearch(newElemInfo);
                            elemInfoList.Insert(insertIndex, newElemInfo);
                            FrameworkElement newFrElem = CreateFrElem(newElemInfo);
                            cnvScheme.Children.Insert(insertIndex, newFrElem);
                            elemInfoDict[newElemInfo.ID] = newElemInfo;

                            // пересчёт индексов отображения
                            CalcViewIndexes();

                            // выделение элемента
                            SelectFrElem(newFrElem);
                        }
                    }
                    else if (change.ChangeType == SchemeViewChangeType.ElementChanged)
                    {
                        // изменение элемента схемы
                        ElementInfo newElemInfo = new ElementInfo(change.ElementData);
                        ElementInfo oldElemInfo;

                        if (elemInfoDict.TryGetValue(newElemInfo.ID, out oldElemInfo))
                        {
                            // установка индекса хранения
                            newElemInfo.StoreIndex = oldElemInfo.StoreIndex;

                            // удаление старого элемента
                            elemInfoList.RemoveAt(oldElemInfo.ViewIndex);
                            cnvScheme.Children.RemoveAt(oldElemInfo.ViewIndex);

                            // вставка нового элемента
                            int insertIndex = oldElemInfo.ZIndex == newElemInfo.ZIndex ?
                                oldElemInfo.ViewIndex : ~elemInfoList.BinarySearch(newElemInfo);

                            elemInfoList.Insert(insertIndex, newElemInfo);
                            FrameworkElement newFrElem = CreateFrElem(newElemInfo);
                            cnvScheme.Children.Insert(insertIndex, newFrElem);
                            elemInfoDict[newElemInfo.ID] = newElemInfo;

                            // пересчёт индексов отображения
                            CalcViewIndexes();

                            // обновление выделения элемента
                            if (oldElemInfo == selElemInfo)
                                SelectFrElem(newFrElem);
                        }
                    }
                    else if (change.ChangeType == SchemeViewChangeType.ElementDeleted)
                    {
                        // удаление элемента схемы
                        ElementInfo delElemInfo;

                        if (elemInfoDict.TryGetValue(change.ElementID, out delElemInfo))
                        {
                            elemInfoList.RemoveAt(delElemInfo.ViewIndex);
                            elemInfoDict.Remove(delElemInfo.ID);
                            cnvScheme.Children.RemoveAt(delElemInfo.ViewIndex);

                            // пересчёт индексов
                            CalcViewIndexes();
                            DecStoreIndexes(delElemInfo.StoreIndex);

                            // снятие выделения элемента
                            if (delElemInfo == selElemInfo)
                                SelectFrElem(null);
                        }
                    }
                    else if (change.ChangeType == SchemeViewChangeType.ImageAdded)
                    {
                        // добавление изображения в словарь
                        SchemeViewImage image = change.Image;
                        if (image != null)
                        {
                            schemeData.ImageDict[image.Name] = image;
                            UpdatePictures(image.Name);
                        }
                    }
                    else if (change.ChangeType == SchemeViewChangeType.ImageRenamed)
                    {
                        // переменование изображения в словаре
                        SchemeViewImage image;

                        if (change.ImageOldName != null && 
                            schemeData.ImageDict.TryGetValue(change.ImageOldName, out image))
                        {
                            schemeData.ImageDict.Remove(change.ImageOldName);
                            image.Name = change.ImageNewName;
                            schemeData.ImageDict[image.Name] = image;
                            UpdatePictures(change.ImageOldName, change.ImageNewName);
                        }
                    }
                    else if (change.ChangeType == SchemeViewChangeType.ImageDeleted)
                    {
                        // удаление изображения из словаря
                        if (change.ImageOldName != null)
                        {
                            schemeData.ImageDict.Remove(change.ImageOldName);
                            UpdatePictures(change.ImageOldName);
                        }
                    }
                }
            }
            finally
            {
                // очистить информацию об изменении
                schemeSvcClient.ClearChangeAsync(clientID);
            }
        }
        
        /// <summary>
        /// Отобразить сообщение об ошибке
        /// </summary>
        private void ShowError(string message)
        {
            errorVisible = true;
            errorDT = DateTime.Now;

            tbMessage.Text = message.Length > MaxErrMsgLen ? message.Substring(0, MaxErrMsgLen) + "..." : message;
            bMessage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Скрыть сообщение об ошибке
        /// </summary>
        private void HideError()
        {
            errorVisible = false;
            bMessage.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Обработать исключение при работе WCF-службы
        /// </summary>
        private void ProcWcfError(Exception ex)
        {
            string s = schemePhrases.WcfError + ": " + ex.Message;
            Dispatcher.BeginInvoke(showErrorMethod, s);
            schemeSvcClient.WriteExceptionAsync(s);
        }

        /// <summary>
        /// Установить по умолчанию фразы, используемые приложением
        /// </summary>
        private void SetPhrasesToDefault()
        {
            schemePhrases = new SchemePhrases();
            schemePhrases.Loading = "Loading...";
            schemePhrases.SchemeNotLoaded = "Scheme is not loaded";
            schemePhrases.ErrorDrawingScheme = "Error drawing scheme";
            schemePhrases.WcfError = "WCF service error";
            schemePhrases.UnableGetSettings = "Unable to get application settings";
            schemePhrases.UnableLoadScheme = "Unable to load scheme";
            schemePhrases.UnableLoadCnlData = "Unable to load input channels data";
            schemePhrases.ErrorExecutingAction = "Error executing action";
            schemePhrases.WcfAddrUndefined = "WCF service addres is undefined";
        }


        /// <summary>
        /// Обработка события завершения получения настроек приложения
        /// </summary>
        void schemeSvcClient_GetSettingsCompleted(object sender, GetSettingsCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    schemeSettings = e.Result;
                    schemePhrases = schemeSettings.SchemePhrases;
                    Dispatcher.BeginInvoke(() => tbNoDataMessage.Text = schemePhrases.Loading );

                    if (schemeSettings == null)
                        Dispatcher.BeginInvoke(showErrorMethod, schemePhrases.UnableGetSettings);
                    else
                        reqFreq = 0; // чтобы быстро выполнить следующий запрос
                }
                else
                {
                    ProcWcfError(e.Error);
                }
            }
            finally
            {
                getSettingsCompleted = true;
            }
        }

        /// <summary>
        /// Обработка события завершения загрузки схемы
        /// </summary>
        void schemeClient_LoadSchemeCompleted(object sender, LoadSchemeCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result)
                    {
                        lock (dataLock)
                        {
                            schemeData = e.schemeData;
                            cnlNumsExists = schemeData != null && schemeData.CnlList != null && 
                                schemeData.CnlList.Count > 0;
                        }

                        Dispatcher.BeginInvoke(drawSchemeMethod);
                        reqFreq = 0; // чтобы быстро выполнить следующий запрос
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(showErrorMethod, schemePhrases.UnableLoadScheme);
                    }
                }
                else
                {
                    ProcWcfError(e.Error);
                }
            }
            finally
            {
                loadSchemeCompleted = true;
            }
        }

        /// <summary>
        /// Обработка события завершения загрузки данных входных каналов
        /// </summary>
        void schemeSvcClient_LoadCnlDataCompleted(object sender, LoadCnlDataCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    bool cnlDataExists;

                    lock (dataLock)
                    {
                        cnlDataList = e.Result ? e.cnlDataList : null;
                        cnlDataExists = cnlDataList != null && cnlDataList.Count > 0;
                    }

                    if (cnlDataExists)
                    {
                        cnlNumsNeeded = false;
                        Dispatcher.BeginInvoke(drawSchemeMethod);
                    }
                    else
                    {
                        if (cnlNumsNeeded)
                        {
                            Dispatcher.BeginInvoke(showErrorMethod, schemePhrases.UnableLoadCnlData);
                        }
                        else
                        {
                            cnlNumsNeeded = true;
                            reqFreq = 0; // чтобы быстро выполнить следующий запрос
                        }
                    }
                }
                else
                {
                    ProcWcfError(e.Error);
                }
            }
            finally
            {
                refrDataCompleted = true;
            }
        }

        /// <summary>
        /// Обработка события завершения получения изменения схемы
        /// </summary>
        void schemeSvcClient_GetChangeCompleted(object sender, GetChangeCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result)
                        Dispatcher.BeginInvoke(changeSchemeMethod, e.schemeChange);
                }
                else
                {
                    ProcWcfError(e.Error);
                }
            }
            finally
            {
                refrDataCompleted = true;
            }
        }

        /// <summary>
        /// Обработка события завершения выбора элемента схемы
        /// </summary>
        void schemeSvcClient_SelectElementCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                ProcWcfError(e.Error);
        }

        /// <summary>
        /// Обработка события наведения указателя мыши в границы текстового блока
        /// </summary>
        void textBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            // реакция динамической надписи при наведении мыши
            Border border = (Border)sender;
            TextBlock textBlock = (TextBlock)border.Child;
            SchemeViewDynamicText dynamicText = textBlock.Tag as SchemeViewDynamicText;
            hovElemInfo = (ElementInfo)border.Tag;

            if (dynamicText != null)
            {
                // подчёркивание
                if (dynamicText.UnderlineOnHover)
                    textBlock.TextDecorations = TextDecorations.Underline;

                // изменение цвета фона
                Brush brush = GetBrush(hovElemInfo, dynamicText.BackColorOnHover, DefElemBackColor);
                if (brush != null)
                    border.Background = brush;

                // изменение цвета рамки
                brush = GetBrush(hovElemInfo, dynamicText.BorderColorOnHover, DefElemBorderColor);
                if (brush != null)
                    border.BorderBrush = brush;

                // изменение цвета текста
                brush = GetBrush(hovElemInfo, dynamicText.ForeColorOnHover, DefSchemeForeColor);
                if (brush != null)
                    textBlock.Foreground = brush;
            }
        }

        /// <summary>
        /// Обработка события покидания указателя мыши границ текстового блока
        /// </summary>
        void textBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            // возврат динамической надписи в исходное состояние
            Border border = (Border)sender;
            TextBlock textBlock = (TextBlock)border.Child;
            SchemeViewDynamicText dynamicText = textBlock.Tag as SchemeViewDynamicText;
            hovElemInfo = null;

            if (dynamicText != null)
            {
                // отмена подчёркивания
                if (dynamicText.UnderlineOnHover)
                {
                    SchemeViewFont font = dynamicText.Font == null ? schemeData.SchemeParams.Font : dynamicText.Font;
                    if (font == null || !font.Underline)
                        textBlock.TextDecorations = null;
                }

                // возврат цвета фона
                if (!string.IsNullOrEmpty(dynamicText.BackColorOnHover))
                    border.Background = new SolidColorBrush(ConvertToColor(dynamicText.BackColor, DefElemBackColor));

                // возврат цвета рамки
                if (!string.IsNullOrEmpty(dynamicText.BorderColorOnHover))
                    border.BorderBrush = new SolidColorBrush(
                        ConvertToColor(dynamicText.BorderColor, DefElemBorderColor));

                // возврат цвета текста
                if (!string.IsNullOrEmpty(dynamicText.ForeColorOnHover))
                    textBlock.Foreground = new SolidColorBrush(
                        ConvertToColor(dynamicText.ForeColor, DefSchemeForeColor));
            }
        }

        /// <summary>
        /// Обработка события наведения указателя мыши в границы изображения
        /// </summary>
        void imageBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            // реакция динамического рисунка при наведении мыши
            Border border = (Border)sender;
            Image image = (Image)border.Child;
            SchemeViewDynamicPicture dynamicPicture = image.Tag as SchemeViewDynamicPicture;
            hovElemInfo = (ElementInfo)border.Tag;

            if (dynamicPicture != null)
            {
                // изменение изображения
                if (dynamicPicture.ImageOnHover != null && dynamicPicture.InCnlNum <= 0)
                    image.Source = GetImageSource(dynamicPicture.ImageOnHover);

                // выделение рамкой
                Brush brush = GetBrush(hovElemInfo, dynamicPicture.BorderColorOnHover, DefElemBorderColor);
                if (brush != null)
                    border.BorderBrush = brush;
            }
        }

        /// <summary>
        /// Обработка события покидания указателя мыши границ изображения
        /// </summary>
        void imageBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            // возврат динамического рисунка в исходное состояние
            Border border = (Border)sender;
            Image image = (Image)border.Child;
            SchemeViewDynamicPicture dynamicPicture = image.Tag as SchemeViewDynamicPicture;
            hovElemInfo = null;

            if (dynamicPicture != null)
            {
                // возврат изображения
                if (dynamicPicture.ImageOnHover != null && dynamicPicture.InCnlNum <= 0)
                    image.Source = GetImageSource(dynamicPicture.Image);

                // возврат цвета рамки
                if (!string.IsNullOrEmpty(dynamicPicture.BorderColorOnHover))
                    border.BorderBrush = new SolidColorBrush(
                        ConvertToColor(dynamicPicture.BorderColor, DefElemBorderColor));
            }
        }

        /// <summary>
        /// Обработка события щелчка левой кнопкой мыши в границах элемента схемы
        /// </summary>
        void elemBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (EditMode)
            {
                // получение информации об элементе схемы
                Border border = (Border)sender;
                ElementInfo elemInfo = (ElementInfo)border.Tag;

                // выбор элемента в режиме редактирования
                SelectFrElem(border);
                int x, y;
                PullPoint(e.GetPosition(cnvScheme), out x, out y);
                schemeSvcClient.SelectElementAsync(clientID, elemInfo.ID, x, y);
            }
        }

        /// <summary>
        /// Обработка события отпускания левой кнопки мыши в границах элемента схемы
        /// </summary>
        void elemBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (!EditMode)
            {
                // получение информации об элементе схемы
                Border border = (Border)sender;
                ElementInfo elemInfo = (ElementInfo)border.Tag;

                // определение действия, связанного с элементом
                SchemeViewAction action = SchemeViewAction.None;
                int inCnlNum = 0;
                int ctrlCnlNum = 0;

                if (elemInfo.ElementType == SchemeViewElementTypes.DynamicText)
                {
                    SchemeViewDynamicText dynamicText = (SchemeViewDynamicText)elemInfo.Element;
                    action = dynamicText.Action;
                    inCnlNum = dynamicText.InCnlNum;
                    ctrlCnlNum = dynamicText.CtrlCnlNum;
                }
                else if (elemInfo.ElementType == SchemeViewElementTypes.DynamicPicture)
                {
                    SchemeViewDynamicPicture dynamicPicture = (SchemeViewDynamicPicture)elemInfo.Element;
                    action = dynamicPicture.Action;
                    inCnlNum = dynamicPicture.InCnlNum;
                    ctrlCnlNum = dynamicPicture.CtrlCnlNum;
                }

                // выполнение действия, связанного с элементом
                try
                {
                    if (action == SchemeViewAction.DrawDiagram && inCnlNum > 0)
                        HtmlPage.Window.Invoke("ShowDiag", new object[] { ViewSetIndex, ViewIndex, 
                            DiagDate.Year, DiagDate.Month, DiagDate.Day, inCnlNum, "../" });
                    else if (action == SchemeViewAction.SendCommand && ctrlCnlNum > 0)
                        HtmlPage.Window.Invoke("SendCmd", new object[] { ViewSetIndex, ViewIndex, ctrlCnlNum, "../" });
                }
                catch (Exception ex)
                {
                    string s = schemePhrases.ErrorExecutingAction + ": " + ex.Message;
                    Dispatcher.BeginInvoke(showErrorMethod, s);
                    schemeSvcClient.WriteExceptionAsync(s);
                }
            }
        }

        /// <summary>
        /// Обработка события перемещения указателя мыши над схемой
        /// </summary>
        void cnvScheme_MouseMove(object sender, MouseEventArgs e)
        {
            if (EditMode)
            {
                int mouseX, mouseY;
                PullPoint(e.GetPosition(cnvScheme), out mouseX, out mouseY);
                cursorPos = new ScadaSchemeSvc.Point() { x = mouseX, y = mouseY };
            }
        }

        /// <summary>
        /// Обработка события покидания указателя мыши границ схемы
        /// </summary>
        void cnvScheme_MouseLeave(object sender, MouseEventArgs e)
        {
            if (EditMode)
                cursorPos = new ScadaSchemeSvc.Point() { x = -1, y = -1 };
        }

        /// <summary>
        /// Обработка события щелчка левой кнопкой мыши по схеме
        /// </summary>
        void cnvScheme_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // выбор схемы в режиме редактирования
            e.Handled = true;

            if (EditMode)
            {
                SelectFrElem(null);
                int x, y;
                PullPoint(e.GetPosition(cnvScheme), out x, out y);
                schemeSvcClient.SelectElementAsync(clientID, 0, x, y);
            }
        }

        /// <summary>
        /// Обработка события загрузки основной страницы приложения 
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // настройка цветов в режиме редактирования
            if (EditMode)
            {
                LayoutRoot.Background = new SolidColorBrush(PageEditBackColor);
                bMessage.Background = new SolidColorBrush(MsgEditBackColor);
            }

            // попытка установки адреса WCF-службы по умолчанию
            if (string.IsNullOrEmpty(SchemeSvcURI))
            {
                string uri = HtmlPage.Document.DocumentUri.AbsoluteUri;
                int ind = uri.IndexOf("/scheme/");
                if (ind >= 0)
                    SchemeSvcURI = uri.Substring(0, ind) + "/scheme/ScadaSchemeSvc.svc";
            }

            if (string.IsNullOrEmpty(SchemeSvcURI))
            {
                ShowError(schemePhrases.WcfAddrUndefined);
                tbNoDataMessage.Text = schemePhrases.SchemeNotLoaded;
            }
            else
            {
                // инициализация WCF-службы, взаимодействующей с сервером
                BasicHttpBinding binding = new BasicHttpBinding();

                if (MaxMsgSize > 0)
                {
                    if (MaxMsgSize > binding.MaxReceivedMessageSize)
                        binding.MaxReceivedMessageSize = MaxMsgSize;
                }
                else
                {
                    binding.MaxReceivedMessageSize = DefMaxMsgSize;
                }

                EndpointAddress endpointAddr = new EndpointAddress(SchemeSvcURI);
                schemeSvcClient = new ScadaSchemeSvcClient(binding, endpointAddr);
                schemeSvcClient.GetSettingsCompleted += schemeSvcClient_GetSettingsCompleted;
                schemeSvcClient.LoadSchemeCompleted += schemeClient_LoadSchemeCompleted;
                schemeSvcClient.LoadCnlDataCompleted += schemeSvcClient_LoadCnlDataCompleted;
                schemeSvcClient.GetChangeCompleted += schemeSvcClient_GetChangeCompleted;
                schemeSvcClient.SelectElementCompleted += schemeSvcClient_SelectElementCompleted;

                // запуск потока выполнения работы приложения
                Thread thread = new Thread(Execute);
                thread.Start();
            }
        }
    }
}