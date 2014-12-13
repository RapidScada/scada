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
 * Summary  : Scheme view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Scada.Client;

namespace Scada.Scheme
{
    /// <summary>
    /// Scheme view
    /// <para>Представление схемы</para>
    /// </summary>
    public partial class SchemeView : BaseView
    {
        /// <summary>
        /// Разделитель элементов списков
        /// </summary>
        internal static readonly char[] Separator = new char[] { ';', ',', ' ' };


        /// <summary>
        /// Конструктор
        /// </summary>
        public SchemeView()
            : base()
        {
            maxID = 0;
            ElementList = new List<Element>();
            ElementDict = new Dictionary<int, Element>();
            ImageDict = new Dictionary<string, Image>();
            CnlsFilter = new List<int>();
            SchemeParams = new Scheme(ImageDict, CnlsFilter);
        }


        private int maxID; // максимальный идентификатор элемента схемы


        /// <summary>
        /// Получить параметры схемы
        /// </summary>
        public Scheme SchemeParams { get; private set; }

        /// <summary>
        /// Получить список элементов схемы
        /// </summary>
        public List<Element> ElementList { get; private set; }

        /// <summary>
        /// Получить словарь для доступа к элементам схемы по идентификатору
        /// </summary>
        public Dictionary<int, Element> ElementDict { get; private set; }
        
        /// <summary>
        /// Получить словарь изображений схемы
        /// </summary>
        public Dictionary<string, Image> ImageDict { get; private set; }

        /// <summary>
        /// Получить фильтр по входным каналам
        /// </summary>
        /// <remarks>Упорядоченный по возрастанию без повторений список номеров входных каналов,
        /// используемых как фильтр для вывода событий по схеме</remarks>
        public List<int> CnlsFilter { get; private set; }


        /// <summary>
        /// Считать значение строкового свойства
        /// </summary>
        private string ReadStringProp(XmlNode parentNode, string propName, string defaultVal = "")
        {
            XmlNode propNode = parentNode.SelectSingleNode(propName);
            return propNode == null ? defaultVal : propNode.InnerText.Trim();
        }

        /// <summary>
        /// Считать значение двоичного свойства
        /// </summary>
        private bool ReadBoolProp(XmlNode parentNode, string propName, bool defaultVal = false)
        {
            XmlNode propNode = parentNode.SelectSingleNode(propName);
            return propNode == null ? defaultVal : bool.Parse(propNode.InnerText.Trim());
        }

        /// <summary>
        /// Считать значение целочисленного свойства
        /// </summary>
        private int ReadIntProp(XmlNode parentNode, string propName, int defaultVal = 0)
        {
            XmlNode propNode = parentNode.SelectSingleNode(propName);
            return propNode == null ? defaultVal : int.Parse(propNode.InnerText.Trim());
        }

        /// <summary>
        /// Считать значение свойства, в котором записано вещественное число
        /// </summary>
        private double ReadDoubleProp(XmlNode parentNode, string propName, double defaultVal = 0.0)
        {
            XmlNode propNode = parentNode.SelectSingleNode(propName);
            return propNode == null ? defaultVal : ScadaUtils.StrToDoubleExc(propNode.InnerText.Trim());
        }

        /// <summary>
        /// Считать значение свойства, в котором записано расположение
        /// </summary>
        private Point ReadLocationProp(XmlNode parentNode, Point defaultLocation)
        {
            XmlNode locationNode = parentNode.SelectSingleNode("Location");
            if (locationNode == null)
            {
                return defaultLocation;
            }
            else
            {
                Point location = new Point();
                location.X = ReadIntProp(locationNode, "X", defaultLocation.X);
                location.Y = ReadIntProp(locationNode, "Y", defaultLocation.Y);
                return location;
            }
        }

        /// <summary>
        /// Считать значение свойства, в котором записан размер
        /// </summary>
        private Size ReadSizeProp(XmlNode parentNode, Size defaultSize)
        {
            XmlNode sizeNode = parentNode.SelectSingleNode("Size");
            if (sizeNode == null)
            {
                return defaultSize;
            }
            else
            {
                Size size = new Size();
                size.Width = ReadIntProp(sizeNode, "Width", defaultSize.Width);
                size.Height = ReadIntProp(sizeNode, "Height", defaultSize.Height);
                return size;
            }
        }

        /// <summary>
        /// Считать значение свойства, в котором записан шрифт
        /// </summary>
        private Font ReadFontProp(XmlNode parentNode, bool notNull = false)
        {
            XmlNode fontNode = parentNode.SelectSingleNode("Font");
            if (fontNode == null)
            {
                return notNull ? new Font() : null;
            }
            else
            {
                Font font = new Font();
                font.Name = ReadStringProp(fontNode, "Name", Font.Default.Name);
                font.Size = ReadIntProp(fontNode, "Size", Font.Default.Size);
                font.Bold = ReadBoolProp(fontNode, "Bold", Font.Default.Bold);
                font.Italic = ReadBoolProp(fontNode, "Italic", Font.Default.Italic);
                font.Underline = ReadBoolProp(fontNode, "Underline", Font.Default.Underline);
                return font;
            }
        }

        /// <summary>
        /// Считать значение свойства, в котором записано имя изображения
        /// </summary>
        private Image ReadImageNameProp(XmlNode parentNode, string propName)
        {
            Image image = null;
            string imageName = ReadStringProp(parentNode, propName);
            if (imageName != "")
            {
                image = new Image();
                image.Name = imageName;
            }
            return image;
        }

        /// <summary>
        /// Считать значение свойства, в котором записано горизонтальное выравнивание
        /// </summary>
        private HorizontalAlignment ReadHAlignProp(XmlNode parentNode)
        {
            XmlNode propNode = parentNode.SelectSingleNode("HAlign");
            return propNode == null ? HorizontalAlignment.Left : 
                (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), propNode.InnerText.Trim(), true);
        }

        /// <summary>
        /// Считать значение свойства, в котором записано вертикальное выравнивание
        /// </summary>
        private VerticalAlignment ReadVAlignProp(XmlNode parentNode)
        {
            XmlNode propNode = parentNode.SelectSingleNode("VAlign");
            return propNode == null ? VerticalAlignment.Top :
                (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), propNode.InnerText.Trim(), true);
        }

        /// <summary>
        /// Считать значение свойства, в котором записано действие
        /// </summary>
        private Action ReadActionProp(XmlNode parentNode)
        {
            XmlNode propNode = parentNode.SelectSingleNode("Action");
            return propNode == null ? Action.None :
                (Action)Enum.Parse(typeof(Action), propNode.InnerText.Trim(), true);
        }

        /// <summary>
        /// Считать значение свойства, в котором записан признак вывода значения входного канала
        /// </summary>
        private ShowValue ReadShowValueProp(XmlNode parentNode)
        {
            XmlNode propNode = parentNode.SelectSingleNode("ShowValue");
            return propNode == null ? ShowValue.ShowWithUnit :
                (ShowValue)Enum.Parse(typeof(ShowValue), propNode.InnerText.Trim(), true);
        }

        /// <summary>
        /// Считать значение свойства, в котором записано растяжение изображения
        /// </summary>
        private ImageStretch ReadImageStretchProp(XmlNode parentNode)
        {
            XmlNode propNode = parentNode.SelectSingleNode("ImageStretch");
            return propNode == null ? ImageStretch.None :
                (ImageStretch)Enum.Parse(typeof(ImageStretch), propNode.InnerText.Trim(), true);
        }

        /// <summary>
        /// Считать значение свойства, в котором записан оператор сравнения
        /// </summary>
        private CompareOperator ReadCompareOperatorProp(XmlNode parentNode, string propName)
        {
            XmlNode propNode = parentNode.SelectSingleNode(propName);
            return propNode == null ? CompareOperator.LessThan :
                (CompareOperator)Enum.Parse(typeof(CompareOperator), propNode.InnerText.Trim(), true);
        }

        /// <summary>
        /// Считать значение свойства, в котором записан логический оператор
        /// </summary>
        private LogicalOperator ReadLogicalOperatorProp(XmlNode parentNode, string propName)
        {
            XmlNode propNode = parentNode.SelectSingleNode(propName);
            return propNode == null ? LogicalOperator.None :
                (LogicalOperator)Enum.Parse(typeof(LogicalOperator), propNode.InnerText.Trim(), true);
        }

        /// <summary>
        /// Записать значение свойства
        /// </summary>
        private void WriteProp(XmlNode parentNode, string propName, object propVal)
        {
            if (propVal != null)
            {
                XmlElement propElem = parentNode.OwnerDocument.CreateElement(propName);
                bool isNotEmpty = true;

                if (propVal is Point)
                {
                    Point point = (Point)propVal;
                    WriteProp(propElem, "X", point.X);
                    WriteProp(propElem, "Y", point.Y);
                }
                else if (propVal is Size)
                {
                    Size size = (Size)propVal;
                    WriteProp(propElem, "Width", size.Width);
                    WriteProp(propElem, "Height", size.Height);
                }
                else if (propVal is Font)
                {
                    Font font = (Font)propVal;
                    WriteProp(propElem, "Name", font.Name);
                    WriteProp(propElem, "Size", font.Size);
                    WriteProp(propElem, "Bold", font.Bold);
                    WriteProp(propElem, "Italic", font.Italic);
                    WriteProp(propElem, "Underline", font.Underline);
                }
                else if (propVal is Image)
                {
                    Image image = (Image)propVal;
                    WriteProp(propElem, "Name", image.Name);
                    WriteProp(propElem, "Data", image.Data);
                }
                else if (propVal is Condition)
                {
                    Condition cond = (Condition)propVal;
                    WriteProp(propElem, "CompareOperator1", cond.CompareOperator1);
                    WriteProp(propElem, "CompareArgument1", cond.CompareArgument1);
                    WriteProp(propElem, "LogicalOperator", cond.LogicalOperator);
                    WriteProp(propElem, "CompareOperator2", cond.CompareOperator2);
                    WriteProp(propElem, "CompareArgument2", cond.CompareArgument2);
                    if (cond.Image != null)
                        WriteProp(propElem, "ImageName", cond.Image.Name);
                }
                else if (propVal is byte[])
                {
                    byte[] arr = (byte[])propVal;
                    isNotEmpty = arr.Length > 0;
                    if (isNotEmpty)
                        propElem.InnerText = Convert.ToBase64String(arr, Base64FormattingOptions.None);
                }
                else
                {
                    string s = propVal.ToString().Trim();
                    isNotEmpty = s != "";
                    if (isNotEmpty)
                        propElem.InnerText = s;
                }

                if (isNotEmpty)
                    parentNode.AppendChild(propElem);
            }
        }


        /// <summary>
        /// Загрузить представление из потока
        /// </summary>
        public override void LoadFromStream(Stream stream)
        {
            // очистка представления
            Clear();

            // загрузка XML-документа
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);

            // проверка типа представления
            XmlElement rootElem = xmlDoc.DocumentElement;
            if (!rootElem.Name.Equals("SchemeView", StringComparison.OrdinalIgnoreCase))
                return;

            // получение заголовка представления
            Title = rootElem.GetAttribute("title");
            SchemeParams.Title = Title;

            // загрузка параметров схемы
            XmlNode schemeNode = rootElem.SelectSingleNode("Scheme");
            if (schemeNode != null)
            {
                SchemeParams.Size = ReadSizeProp(schemeNode, Scheme.DefaultSize);
                SchemeParams.BackColor = ReadStringProp(schemeNode, "BackColor");
                SchemeParams.BackImage = ReadImageNameProp(schemeNode, "BackImageName");
                SchemeParams.ForeColor = ReadStringProp(schemeNode, "ForeColor");
                SchemeParams.Font = ReadFontProp(schemeNode, true);                
            }

            // загрузка элементов схемы
            XmlNode elemsNode = rootElem.SelectSingleNode("Elements");
            if (elemsNode != null)
            {
                foreach (XmlNode elemNode in elemsNode.ChildNodes)
                {
                    string tagName = elemNode.Name.ToLower();
                    Element element = null;
                    int inCnlNum = 0;
                    int ctrlCnlNum = 0;

                    if (tagName == "statictext" || tagName == "dynamictext")
                    {
                        DynamicText dynamicText = tagName == "dynamictext" ? new DynamicText() : null;
                        StaticText staticText = dynamicText == null ? new StaticText() : dynamicText;
                        element = staticText;

                        staticText.AutoSize = ReadBoolProp(elemNode, "AutoSize");
                        staticText.BackColor = ReadStringProp(elemNode, "BackColor");
                        staticText.BorderColor = ReadStringProp(elemNode, "BorderColor");
                        staticText.ForeColor = ReadStringProp(elemNode, "ForeColor");
                        staticText.Font = ReadFontProp(elemNode);
                        staticText.Text = ReadStringProp(elemNode, "Text");
                        staticText.WordWrap = ReadBoolProp(elemNode, "WordWrap");
                        staticText.HAlign = ReadHAlignProp(elemNode);
                        staticText.VAlign = ReadVAlignProp(elemNode);

                        if (dynamicText != null)
                        {
                            dynamicText.ToolTip = ReadStringProp(elemNode, "ToolTip");
                            dynamicText.UnderlineOnHover = ReadBoolProp(elemNode, "UnderlineOnHover");
                            dynamicText.BackColorOnHover = ReadStringProp(elemNode, "BackColorOnHover");
                            dynamicText.BorderColorOnHover = ReadStringProp(elemNode, "BorderColorOnHover");
                            dynamicText.ForeColorOnHover = ReadStringProp(elemNode, "ForeColorOnHover");
                            inCnlNum = ReadIntProp(elemNode, "InCnlNum");
                            ctrlCnlNum = ReadIntProp(elemNode, "CtrlCnlNum");
                            dynamicText.InCnlNum = inCnlNum;
                            dynamicText.CtrlCnlNum = ctrlCnlNum;
                            dynamicText.Action = ReadActionProp(elemNode);
                            dynamicText.ShowValue = ReadShowValueProp(elemNode);
                        }
                    }
                    else if (tagName == "staticpicture" || tagName == "dynamicpicture")
                    {
                        DynamicPicture dynamicPicture = tagName == "dynamicpicture" ? new DynamicPicture() : null;
                        StaticPicture staticPicture = dynamicPicture == null ? new StaticPicture() : dynamicPicture;
                        element = staticPicture;

                        staticPicture.BorderColor = ReadStringProp(elemNode, "BorderColor");
                        staticPicture.Image = ReadImageNameProp(elemNode, "ImageName");
                        staticPicture.ImageStretch = ReadImageStretchProp(elemNode);

                        if (dynamicPicture != null)
                        {
                            dynamicPicture.ToolTip = ReadStringProp(elemNode, "ToolTip");
                            dynamicPicture.ImageOnHover = ReadImageNameProp(schemeNode, "ImageNameOnHover");
                            dynamicPicture.BorderColorOnHover = ReadStringProp(elemNode, "BorderColorOnHover");
                            inCnlNum = ReadIntProp(elemNode, "InCnlNum");
                            ctrlCnlNum = ReadIntProp(elemNode, "CtrlCnlNum");
                            dynamicPicture.InCnlNum = inCnlNum;
                            dynamicPicture.CtrlCnlNum = ctrlCnlNum;
                            dynamicPicture.Action = ReadActionProp(elemNode);

                            XmlNode condsNode = elemNode.SelectSingleNode("Conditions");
                            if (condsNode != null)
                            {
                                dynamicPicture.Conditions = new List<Condition>();
                                XmlNodeList condNodes = condsNode.SelectNodes("Condition");
                                foreach (XmlNode condNode in condNodes)
                                {
                                    Condition condition = new Condition();
                                    condition.CompareOperator1 = ReadCompareOperatorProp(condNode, "CompareOperator1");
                                    condition.CompareArgument1 = ReadDoubleProp(condNode, "CompareArgument1");
                                    condition.CompareOperator2 = ReadCompareOperatorProp(condNode, "CompareOperator2");
                                    condition.CompareArgument2 = ReadDoubleProp(condNode, "CompareArgument2");
                                    condition.LogicalOperator = ReadLogicalOperatorProp(condNode, "LogicalOperator");
                                    condition.Image = ReadImageNameProp(condNode, "ImageName");
                                    dynamicPicture.Conditions.Add(condition);
                                }
                            }
                        }
                    }

                    if (element != null)
                    {
                        element.ID = ReadIntProp(elemNode, "ID");

                        if (element.ID > 0)
                        {
                            element.Name = ReadStringProp(elemNode, "Name");
                            element.Location = ReadLocationProp(elemNode, Element.DefaultLocation);
                            element.Size = ReadSizeProp(elemNode, Element.DefaultSize);
                            element.ZIndex = ReadIntProp(elemNode, "ZIndex");
                            ElementList.Add(element);
                            ElementDict[element.ID] = element;

                            if (inCnlNum > 0)
                                AddCnlNum(inCnlNum);
                            if (ctrlCnlNum > 0)
                                AddCtrlCnlNum(ctrlCnlNum);
                            if (maxID < element.ID)
                                maxID = element.ID;
                        }
                    }
                }
            }

            // загрузка словаря изображений схемы
            XmlNode imagesNode = rootElem.SelectSingleNode("Images");
            if (imagesNode != null)
            {
                XmlNodeList imageNodes = imagesNode.SelectNodes("Image");
                foreach (XmlNode imageNode in imageNodes)
                {
                    Image image = new Image();
                    string name = ReadStringProp(imageNode, "Name");
                    image.Name = name;
                    image.Data = Convert.FromBase64String(ReadStringProp(imageNode, "Data"));
                    if (name != "")
                        ImageDict[name] = image;
                }
            }

            // загрузка фильтра по входным каналам
            XmlNode cnlsFilterNode = rootElem.SelectSingleNode("CnlsFilter");
            if (cnlsFilterNode != null)
            {
                string[] cnlNums = cnlsFilterNode.InnerText.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

                foreach (string cnlNumStr in cnlNums)
                {
                    int cnlNum;
                    if (int.TryParse(cnlNumStr, out cnlNum))
                    {
                        int ind = CnlsFilter.BinarySearch(cnlNum);
                        if (ind < 0)
                        {
                            CnlsFilter.Insert(~ind, cnlNum);
                            AddCnlNum(cnlNum);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Сохранить представление в поток
        /// </summary>
        public void SaveToStream(Stream stream)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            // запись заголовка представления
            XmlElement rootElem = xmlDoc.CreateElement("SchemeView");
            rootElem.SetAttribute("title", SchemeParams.Title);
            xmlDoc.AppendChild(rootElem);

            // запись параметров схемы
            XmlElement schemeElem = xmlDoc.CreateElement("Scheme");
            rootElem.AppendChild(schemeElem);

            WriteProp(schemeElem, "Size", SchemeParams.Size);
            WriteProp(schemeElem, "BackColor", SchemeParams.BackColor);
            if (SchemeParams.BackImage != null)
                WriteProp(schemeElem, "BackImageName", SchemeParams.BackImage.Name);
            WriteProp(schemeElem, "ForeColor", SchemeParams.ForeColor);
            WriteProp(schemeElem, "Font", SchemeParams.Font);

            // запись элементов схемы
            XmlElement elemsElem = xmlDoc.CreateElement("Elements");
            rootElem.AppendChild(elemsElem);

            foreach (Element elem in ElementList)
            {
                XmlElement elemElem = xmlDoc.CreateElement(elem.GetType().Name);
                elemsElem.AppendChild(elemElem);

                WriteProp(elemElem, "ID", elem.ID);
                WriteProp(elemElem, "Name", elem.Name);
                WriteProp(elemElem, "Location", elem.Location);
                WriteProp(elemElem, "Size", elem.Size);
                WriteProp(elemElem, "ZIndex", elem.ZIndex);

                if (elem is StaticText)
                {
                    StaticText staticText = (StaticText)elem;
                    WriteProp(elemElem, "AutoSize", staticText.AutoSize);
                    WriteProp(elemElem, "BackColor", staticText.BackColor);
                    WriteProp(elemElem, "BorderColor", staticText.BorderColor);
                    WriteProp(elemElem, "ForeColor", staticText.ForeColor);
                    WriteProp(elemElem, "Font", staticText.Font);
                    WriteProp(elemElem, "Text", staticText.Text);
                    WriteProp(elemElem, "WordWrap", staticText.WordWrap);
                    WriteProp(elemElem, "HAlign", staticText.HAlign);
                    WriteProp(elemElem, "VAlign", staticText.VAlign);

                    if (elem is DynamicText)
                    {
                        DynamicText dynamicText = (DynamicText)elem;
                        WriteProp(elemElem, "ToolTip", dynamicText.ToolTip);
                        WriteProp(elemElem, "UnderlineOnHover", dynamicText.UnderlineOnHover);
                        WriteProp(elemElem, "BackColorOnHover", dynamicText.BackColorOnHover);
                        WriteProp(elemElem, "BorderColorOnHover", dynamicText.BorderColorOnHover);
                        WriteProp(elemElem, "ForeColorOnHover", dynamicText.ForeColorOnHover);
                        WriteProp(elemElem, "InCnlNum", dynamicText.InCnlNum);
                        WriteProp(elemElem, "CtrlCnlNum", dynamicText.CtrlCnlNum);
                        WriteProp(elemElem, "Action", dynamicText.Action);
                        WriteProp(elemElem, "ShowValue", dynamicText.ShowValue);
                    }

                    elemsElem.AppendChild(elemElem);
                }
                else if (elem is StaticPicture)
                {
                    StaticPicture staticPicture = (StaticPicture)elem;
                    WriteProp(elemElem, "BorderColor", staticPicture.BorderColor);
                    if (staticPicture.Image != null)
                        WriteProp(elemElem, "ImageName", staticPicture.Image.Name);
                    WriteProp(elemElem, "ImageStretch", staticPicture.ImageStretch);

                    if (elem is DynamicPicture)
                    {
                        DynamicPicture dynamicPicture = (DynamicPicture)elem;
                        WriteProp(elemElem, "ToolTip", dynamicPicture.ToolTip);
                        if (dynamicPicture.ImageOnHover != null)
                            WriteProp(elemElem, "ImageOnHoverName", dynamicPicture.ImageOnHover.Name);
                        WriteProp(elemElem, "BorderColorOnHover", dynamicPicture.BorderColorOnHover);
                        WriteProp(elemElem, "InCnlNum", dynamicPicture.InCnlNum);
                        WriteProp(elemElem, "CtrlCnlNum", dynamicPicture.CtrlCnlNum);
                        WriteProp(elemElem, "Action", dynamicPicture.Action);

                        if (dynamicPicture.Conditions != null && dynamicPicture.Conditions.Count > 0)
                        {
                            XmlElement condsElem = xmlDoc.CreateElement("Conditions");
                            elemElem.AppendChild(condsElem);

                            foreach (Condition cond in dynamicPicture.Conditions)
                                WriteProp(condsElem, "Condition", cond);
                        }
                    }

                }
            }

            // запись словаря изображений схемы
            XmlElement imagesElem = xmlDoc.CreateElement("Images");
            rootElem.AppendChild(imagesElem);

            foreach (Image image in ImageDict.Values)
                WriteProp(imagesElem, "Image", image);

            // запись фильтра по входным каналам
            XmlElement cnlsFilterElem = xmlDoc.CreateElement("CnlsFilter");
            cnlsFilterElem.InnerText = string.Join<int>(" ", CnlsFilter);
            rootElem.AppendChild(cnlsFilterElem);

            xmlDoc.Save(stream);
        }

        /// <summary>
        /// Очистить представление
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            maxID = 0;
            SchemeParams.SetToDefault();
            ElementList.Clear();
            ElementDict.Clear();
            ImageDict.Clear();
            CnlsFilter.Clear();
        }

        /// <summary>
        /// Получить следующий идентификатор элемента схемы
        /// </summary>
        public int GetNextElementID()
        {
            maxID++;
            return maxID;
        }
    }
}
