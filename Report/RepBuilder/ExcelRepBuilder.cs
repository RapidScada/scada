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
 * Module   : Report Builder
 * Summary  : The base class for building reports in SpreadsheetML (Microsoft Excel 2003) format
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Utils.Report
{
    /// <summary>
    /// The base class for building reports in SpreadsheetML (Microsoft Excel 2003) format.
    /// <para>Базовый класс для построения отчётов в формате SpreadsheetML (Microsoft Excel 2003).</para>
    /// </summary>
    public abstract class ExcelRepBuilder : RepBuilder
    {
        /// <summary>
        /// Пространства имён, которые используются в SpreadsheetML
        /// </summary>
        protected static class XmlNamespaces
        {
            /// <summary>
            /// Пространстро имён xmlns
            /// </summary>
            public const string noprefix = "urn:schemas-microsoft-com:office:spreadsheet";
            /// <summary>
            /// Пространстро имён xmlns:o
            /// </summary>
            public const string o = "urn:schemas-microsoft-com:office:office";
            /// <summary>
            /// Пространстро имён xmlns:x
            /// </summary>
            public const string x = "urn:schemas-microsoft-com:office:excel";
            /// <summary>
            /// Пространстро имён xmlns:ss
            /// </summary>
            public const string ss = "urn:schemas-microsoft-com:office:spreadsheet";
            /// <summary>
            /// Пространстро имён xmlns:html
            /// </summary>
            public const string html = "http://www.w3.org/TR/REC-html40";
        }

        /// <summary>
        /// Книга Excel
        /// </summary>
        protected class Workbook
        {
            /// <summary>
            /// Ссылка на XML-узел, соответствующий книге Excel
            /// </summary>
            protected XmlNode node;
            /// <summary>
            /// Ссылка на XML-узел, содержащий стили книги Excel
            /// </summary>
            protected XmlNode stylesNode;
            /// <summary>
            /// Список стилей книги Excel с возможностью доступа по ID стиля
            /// </summary>
            protected SortedList<string, Style> styles;
            /// <summary>
            /// Список листов книги Excel
            /// </summary>
            protected List<Worksheet> worksheets;


            /// <summary>
            /// Конструктор
            /// </summary>
            protected Workbook()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="xmlNode">Ссылка на XML-узел, соответствующий книге Excel</param>
            public Workbook(XmlNode xmlNode)
            {
                node = xmlNode;
                styles = new SortedList<string, Style>();
                worksheets = new List<Worksheet>();
            }


            /// <summary>
            /// Получить ссылку на XML-узел, соответствующий книге Excel
            /// </summary>
            public XmlNode Node
            {
                get
                {
                    return node;
                }
            }

            /// <summary>
            /// Получить или установить ссылку на XML-узел, содержащий стили книги Excel
            /// </summary>
            public XmlNode StylesNode
            {
                get
                {
                    return stylesNode;
                }
                set
                {
                    stylesNode = value;
                }
            }

            /// <summary>
            /// Получить список стилей книги Excel с возможностью доступа по ID стиля
            /// </summary>
            public SortedList<string, Style> Styles
            {
                get
                {
                    return styles;
                }
            }

            /// <summary>
            /// Получить список листов книги Excel
            /// </summary>
            public List<Worksheet> Worksheets
            {
                get
                {
                    return worksheets;
                }
            }


            /// <summary>
            /// Добавить стиль в конец списка стилей книги Excel и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="style">Добавляемый стиль</param>
            public void AppendStyle(Style style)
            {
                styles.Add(style.ID, style);
                stylesNode.AppendChild(style.Node);
            }

            /// <summary>
            /// Удалить стиль из списка стилей книги Excel и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="listIndex">Индекс удаляемого стиля в списке</param>
            public void RemoveStyle(int listIndex)
            {
                Style style = styles.Values[listIndex];
                stylesNode.RemoveChild(style.Node);
                styles.RemoveAt(listIndex);
            }

            /// <summary>
            /// Найти лист в списке листов Excel по имени без учёта регистра
            /// </summary>
            /// <param name="worksheetName">Имя листа Excel</param>
            /// <returns>Лист Excel или null, если он не найден</returns>
            public Worksheet FindWorksheet(string worksheetName)
            {
                foreach (Worksheet worksheet in worksheets)
                    if (worksheet.Name.Equals(worksheetName, StringComparison.OrdinalIgnoreCase))
                        return worksheet;

                return null;
            }

            /// <summary>
            /// Добавить лист в конец списка листов Excel и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="worksheet">Добавляемый лист</param>
            public void AppendWorksheet(Worksheet worksheet)
            {
                worksheets.Add(worksheet);
                node.AppendChild(worksheet.Node);
            }

            /// <summary>
            /// Вставить лист в список листов Excel и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="listIndex">Индекс вставляемого листа в списке</param>
            /// <param name="worksheet">Вставляемый лист</param>
            public void InsertWorksheet(int listIndex, Worksheet worksheet)
            {
                worksheets.Insert(listIndex, worksheet);

                if (worksheets.Count == 1)
                    node.AppendChild(worksheet.Node);
                else if (listIndex == 0)
                    node.PrependChild(worksheet.Node);
                else
                    node.InsertAfter(worksheet.Node, worksheets[listIndex - 1].Node);
            }

            /// <summary>
            /// Удалить лист из списка листов Excel и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="listIndex">Индекс удаляемого листа в списке</param>
            public void RemoveWorksheet(int listIndex)
            {
                Worksheet worksheet = worksheets[listIndex];
                node.RemoveChild(worksheet.Node);
                worksheets.RemoveAt(listIndex);
            }


            /// <summary>
            /// Sets the object color, creating a new style if necessary.
            /// </summary>
            /// <param name="targetNode">Reference to the XML node of the object which color is set.</param>
            /// <param name="backColor">Background color to set.</param>
            /// <param name="fontColor">Font color to set.</param>
            public void SetColor(XmlNode targetNode, string backColor, string fontColor)
            {
                XmlDocument xmlDoc = targetNode.OwnerDocument;
                string namespaceURI = targetNode.NamespaceURI;

                XmlAttribute styleAttr = targetNode.Attributes["ss:StyleID"];
                if (styleAttr == null)
                {
                    styleAttr = xmlDoc.CreateAttribute("ss:StyleID");
                    targetNode.Attributes.Append(styleAttr);
                }

                string oldStyleID = styleAttr == null ? "" : styleAttr.Value;
                string newStyleID = oldStyleID + "_" + 
                    (string.IsNullOrEmpty(backColor) ? "none" : backColor) + "_" +
                    (string.IsNullOrEmpty(fontColor) ? "none" : fontColor);

                if (styles.ContainsKey(newStyleID))
                {
                    // set the previously created style having the specified color
                    styleAttr.Value = newStyleID;
                }
                else
                {
                    // create a new style
                    Style newStyle;
                    if (styleAttr == null)
                    {
                        XmlNode newStyleNode = xmlDoc.CreateNode(XmlNodeType.Element, "Style", namespaceURI);
                        newStyleNode.Attributes.Append(xmlDoc.CreateAttribute("ss", "ID", namespaceURI));
                        newStyle = new Style(newStyleNode);
                    }
                    else
                    {
                        newStyle = styles[oldStyleID].Clone();
                    }
                    newStyle.ID = newStyleID;

                    // set background color of the style
                    if (!string.IsNullOrEmpty(backColor))
                    {
                        XmlNode interNode = newStyle.Node.SelectSingleNode("Interior");
                        if (interNode == null)
                        {
                            interNode = xmlDoc.CreateNode(XmlNodeType.Element, "Interior", namespaceURI);
                            newStyle.Node.AppendChild(interNode);
                        }
                        else
                        {
                            interNode.Attributes.RemoveNamedItem("ss:Color");
                            interNode.Attributes.RemoveNamedItem("ss:Pattern");
                        }

                        XmlAttribute xmlAttr = xmlDoc.CreateAttribute("ss", "Color", namespaceURI);
                        xmlAttr.Value = backColor;
                        interNode.Attributes.Append(xmlAttr);
                        xmlAttr = xmlDoc.CreateAttribute("ss", "Pattern", namespaceURI);
                        xmlAttr.Value = "Solid";
                        interNode.Attributes.Append(xmlAttr);
                    }

                    // set font color of the style
                    if (!string.IsNullOrEmpty(fontColor))
                    {
                        XmlNode fontNode = newStyle.Node.SelectSingleNode("Font");
                        if (fontNode == null)
                        {
                            fontNode = xmlDoc.CreateNode(XmlNodeType.Element, "Font", namespaceURI);
                            newStyle.Node.AppendChild(fontNode);
                        }
                        else
                        {
                            fontNode.Attributes.RemoveNamedItem("ss:Color");
                        }

                        XmlAttribute xmlAttr = xmlDoc.CreateAttribute("ss", "Color", namespaceURI);
                        xmlAttr.Value = fontColor;
                        fontNode.Attributes.Append(xmlAttr);
                    }

                    // set the new style to the node and add the style to the workbook
                    styleAttr.Value = newStyleID;
                    styles.Add(newStyleID, newStyle);
                    stylesNode.AppendChild(newStyle.Node);
                }
            }
        }

        /// <summary>
        /// Стиль книги Excel
        /// </summary>
        protected class Style
        {
            /// <summary>
            /// Ссылка на XML-узел, соответствующий стилю книги Excel
            /// </summary>
            protected XmlNode node;
            /// <summary>
            /// Идентификатор стиля
            /// </summary>
            protected string id;


            /// <summary>
            /// Конструктор
            /// </summary>
            protected Style()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="xmlNode">Ссылка на XML-узел, соответствующий стилю книги Excel</param>
            public Style(XmlNode xmlNode)
            {
                node = xmlNode;
                id = xmlNode.Attributes["ss:ID"].Value;
            }


            /// <summary>
            /// Получить ссылку на XML-узел, соответствующий стилю книги Excel
            /// </summary>
            public XmlNode Node
            {
                get
                {
                    return node;
                }
            }

            /// <summary>
            /// Получить или установить идентификатор стиля, при установке модифицируется дерево XML-документа
            /// </summary>
            public string ID
            {
                get
                {
                    return id;
                }
                set
                {
                    id = value;
                    node.Attributes["ss:ID"].Value = id;
                }
            }


            /// <summary>
            /// Клонировать стиль
            /// </summary>
            /// <returns>Копия стиля</returns>
            public Style Clone()
            {
                return new Style(node.Clone());
            }
        }

        /// <summary>
        /// Лист книги Excel
        /// </summary>
        protected class Worksheet
        {
            /// <summary>
            /// Ссылка на XML-узел, соответствующий листу книги Excel
            /// </summary>
            protected XmlNode node;
            /// <summary>
            /// Имя листа
            /// </summary>
            protected string name;
            /// <summary>
            /// Таблица с содержимым листа
            /// </summary>
            protected Table table;
            /// <summary>
            /// Родительская книга данного листа
            /// </summary>
            protected Workbook parentWorkbook;


            /// <summary>
            /// Конструктор
            /// </summary>
            protected Worksheet()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="xmlNode">Ссылка на XML-узел, соответствующий листу книги Excel</param>
            public Worksheet(XmlNode xmlNode)
            {
                node = xmlNode;
                name = xmlNode.Attributes["ss:Name"].Value;
                table = null;
            }


            /// <summary>
            /// Получить ссылку на XML-узел, соответствующий листу книги Excel
            /// </summary>
            public XmlNode Node
            {
                get
                {
                    return node;
                }
            }

            /// <summary>
            /// Получить или установить имя листа, при установке модифицируется дерево XML-документа
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                    node.Attributes["ss:Name"].Value = name;
                }
            }

            /// <summary>
            /// Получить или установить таблицу с содержимым листа
            /// </summary>
            public Table Table
            {
                get
                {
                    return table;
                }
                set
                {
                    table = value;
                    table.ParentWorksheet = this;
                }
            }

            /// <summary>
            /// Получить или установить родительскую книгу данного листа
            /// </summary>
            public Workbook ParentWorkbook
            {
                get
                {
                    return parentWorkbook;
                }
                set
                {
                    parentWorkbook = value;
                }
            }


            /// <summary>
            /// Установить горизонтальный разделитель области прокрутки
            /// </summary>
            public void SplitHorizontal(int rowIndex)
            {
                XmlDocument xmlDoc = node.OwnerDocument;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("report", XmlNamespaces.x);

                XmlNode optionsNode = node.SelectSingleNode("report:WorksheetOptions", nsmgr);

                if (optionsNode != null)
                {
                    string rowIndexStr = rowIndex.ToString();

                    XmlNode splitNode = optionsNode.SelectSingleNode("report:SplitHorizontal", nsmgr);
                    if (splitNode == null)
                    {
                        splitNode = xmlDoc.CreateElement("SplitHorizontal");
                        optionsNode.AppendChild(splitNode);
                    }
                    splitNode.InnerText = rowIndexStr;

                    XmlNode paneNode = optionsNode.SelectSingleNode("report:TopRowBottomPane", nsmgr);
                    if (paneNode == null)
                    {
                        paneNode = xmlDoc.CreateElement("TopRowBottomPane");
                        optionsNode.AppendChild(paneNode);
                    }
                    paneNode.InnerText = rowIndexStr;
                }
            }

            /// <summary>
            /// Клонировать лист
            /// </summary>
            /// <returns>Копия листа</returns>
            public Worksheet Clone()
            {
                XmlNode nodeClone = node.CloneNode(false);
                Worksheet worksheetClone = new Worksheet(nodeClone);

                Table tableClone = table.Clone();
                worksheetClone.table = tableClone;
                nodeClone.AppendChild(tableClone.Node);

                foreach (XmlNode childNode in node.ChildNodes)
                    if (childNode.Name != "Table")
                        nodeClone.AppendChild(childNode.Clone());

                return worksheetClone;
            }
        }

        /// <summary>
        /// Таблица листа Excel
        /// </summary>
        protected class Table
        {
            /// <summary>
            /// Ссылка на XML-узел, соответствующий таблице листа Excel
            /// </summary>
            protected XmlNode node;
            /// <summary>
            /// Список столбцов таблицы
            /// </summary>
            protected List<Column> columns;
            /// <summary>
            /// Список строк таблицы
            /// </summary>
            protected List<Row> rows;
            /// <summary>
            /// Родительский лист данной таблицы
            /// </summary>
            protected Worksheet parentWorksheet;


            /// <summary>
            /// Конструктор
            /// </summary>
            protected Table()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="xmlNode">Ссылка на XML-узел, соответствующий таблице листа Excel</param>
            public Table(XmlNode xmlNode)
            {
                node = xmlNode;
                columns = new List<Column>();
                rows = new List<Row>();
                parentWorksheet = null;
            }


            /// <summary>
            /// Получить ссылку на XML-узел, соответствующий таблице листа Excel
            /// </summary>
            public XmlNode Node
            {
                get
                {
                    return node;
                }
            }

            /// <summary>
            /// Получить список столбцов таблицы
            /// </summary>
            public List<Column> Columns
            {
                get
                {
                    return columns;
                }
            }

            /// <summary>
            /// Получить список строк таблицы
            /// </summary>
            public List<Row> Rows
            {
                get
                {
                    return rows;
                }
            }

            /// <summary>
            /// Получить или установить родительский лист данной таблицы
            /// </summary>
            public Worksheet ParentWorksheet
            {
                get
                {
                    return parentWorksheet;
                }
                set
                {
                    parentWorksheet = value;
                }
            }


            /// <summary>
            /// Удалить атрибуты XML-узла таблицы, необходимые для корректного отображения книги Excel
            /// </summary>
            public void RemoveTableNodeAttrs()
            {
                node.Attributes.RemoveNamedItem("ss:ExpandedColumnCount");
                node.Attributes.RemoveNamedItem("ss:ExpandedRowCount");
            }

            /// <summary>
            /// Найти столбец в таблице по индексу
            /// </summary>
            /// <param name="columnIndex">Индекс искомого столбца, начиная с 1</param>
            /// <returns>Столбец, удовлетворяющий критерию поиска, или null, если столбец не найден</returns>
            public Column FindColumn(int columnIndex)
            {
                int index = 0;
                foreach (Column column in columns)
                {
                    index = column.Index > 0 ? column.Index : index + 1;
                    int endIndex = index + column.Span;

                    if (index <= columnIndex && columnIndex <= endIndex)
                        return column;

                    index = endIndex;
                }

                return null;
            }

            /// <summary>
            /// Добавить столбец в конец списка столбцов таблицы и модифицировать дерево XML-документа
            /// </summary>
            public void AppendColumn(Column column)
            {
                if (columns.Count > 0)
                    node.InsertAfter(column.Node, columns[columns.Count - 1].Node);
                else
                    node.PrependChild(column.Node);

                column.ParentTable = this;
                columns.Add(column);
            }

            /// <summary>
            /// Вставить столбец в список столбцов таблицы и модифицировать дерево XML-документа
            /// </summary>
            public void InsertColumn(int listIndex, Column column)
            {
                if (columns.Count == 0 || listIndex == 0)
                    node.PrependChild(column.Node);
                else
                    node.InsertAfter(column.Node, columns[listIndex - 1].Node);

                column.ParentTable = this;
                columns.Insert(listIndex, column);
            }

            /// <summary>
            /// Удалить столбец из списка столбцов таблицы и модифицировать дерево XML-документа
            /// </summary>
            public void RemoveColumn(int listIndex)
            {
                Column column = columns[listIndex];
                column.ParentTable = null;
                node.RemoveChild(column.Node);
                columns.RemoveAt(listIndex);
            }

            /// <summary>
            /// Удалить все столбцы из списка столбцов таблицы и модифицировать дерево XML-документа
            /// </summary>
            public void RemoveAllColumns()
            {
                while (columns.Count > 0)
                    RemoveColumn(0);
            }

            /// <summary>
            /// Добавить строку в конец списка строк таблицы и модифицировать дерево XML-документа
            /// </summary>
            public void AppendRow(Row row)
            {
                node.AppendChild(row.Node);
                row.ParentTable = this;
                rows.Add(row);
            }

            /// <summary>
            /// Вставить строку в список строк таблицы и модифицировать дерево XML-документа
            /// </summary>
            public void InsertRow(int listIndex, Row row)
            {
                if (rows.Count == 0)
                    node.AppendChild(row.Node);
                else if (listIndex == 0)
                    node.InsertBefore(row.Node, rows[0].Node);
                else
                    node.InsertAfter(row.Node, rows[listIndex - 1].Node);

                row.ParentTable = this;
                rows.Insert(listIndex, row);
            }

            /// <summary>
            /// Удалить строку из списка строк таблицы и модифицировать дерево XML-документа
            /// </summary>
            public void RemoveRow(int listIndex)
            {
                Row row = rows[listIndex];
                row.ParentTable = null;
                node.RemoveChild(row.Node);
                rows.RemoveAt(listIndex);
            }

            /// <summary>
            /// Удалить все строки из списка строк таблицы и модифицировать дерево XML-документа
            /// </summary>
            public void RemoveAllRows()
            {
                while (rows.Count > 0)
                    RemoveRow(0);
            }

            /// <summary>
            /// Клонировать таблицу
            /// </summary>
            /// <returns>Копия таблицы</returns>
            public Table Clone()
            {
                XmlNode nodeClone = node.CloneNode(false);
                Table tableClone = new Table(nodeClone);
                tableClone.parentWorksheet = parentWorksheet;

                foreach (Column column in columns)
                {
                    Column columnClone = column.Clone();
                    tableClone.columns.Add(columnClone);
                    nodeClone.AppendChild(columnClone.Node);
                }

                foreach (Row row in rows)
                {
                    Row rowClone = row.Clone();
                    tableClone.rows.Add(rowClone);
                    nodeClone.AppendChild(rowClone.Node);
                }

                return tableClone;
            }
        }

        /// <summary>
        /// Столбец таблицы Excel
        /// </summary>
        protected class Column
        {
            /// <summary>
            /// Ссылка на XML-узел, соответствующий столбцу таблицы Excel
            /// </summary>
            protected XmlNode node;
            /// <summary>
            /// Родительская таблица данного столбца
            /// </summary>
            protected Table parentTable;
            /// <summary>
            /// Индекс столбца, 0 - неопределён
            /// </summary>
            protected int index;


            /// <summary>
            /// Конструктор
            /// </summary>
            protected Column()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="xmlNode">Ссылка на XML-узел, соответствующий столбцу таблицы Excel</param>
            public Column(XmlNode xmlNode)
            {
                node = xmlNode;
                parentTable = null;

                XmlAttribute attr = node.Attributes["ss:Index"];
                index = attr == null ? 0 : int.Parse(attr.Value);
            }


            /// <summary>
            /// Получить ссылку на XML-узел, соответствующий столбцу таблицы Excel
            /// </summary>
            public XmlNode Node
            {
                get
                {
                    return node;
                }
            }

            /// <summary>
            /// Получить или установить родительскую таблицу данного столбца
            /// </summary>
            public Table ParentTable
            {
                get
                {
                    return parentTable;
                }
                set
                {
                    parentTable = value;
                }
            }

            /// <summary>
            /// Получить или установить индекс столбца (0 - неопределён), при установке модифицируется дерево XML-документа
            /// </summary>
            public int Index
            {
                get
                {
                    return index;
                }
                set
                {
                    index = value;
                    SetAttribute(node, "Index", XmlNamespaces.ss, index <= 0 ? null : index.ToString(), true);
                }
            }

            /// <summary>
            /// Получить или установить ширину
            /// </summary>
            public double Width
            {
                get
                {
                    string widthStr = GetAttribute(node, "ss:Width");
                    double width;
                    return double.TryParse(widthStr, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out width) ?
                        width : 0;
                }
                set
                {
                    SetColumnWidth(node, value);
                }
            }

            /// <summary>
            /// Получить или установить количество объединямых колонок справа
            /// </summary>
            public int Span
            {
                get
                {
                    string valStr = GetAttribute(node, "ss:Span");
                    return valStr == "" ? 0 : int.Parse(valStr);
                }
                set
                {
                    SetAttribute(node, "Span", XmlNamespaces.ss, value < 1 ? "" : value.ToString(), true);
                }
            }


            /// <summary>
            /// Клонировать столбец
            /// </summary>
            /// <returns>Копия столбца</returns>
            public Column Clone()
            {
                Column columnClone = new Column(node.Clone());
                columnClone.parentTable = parentTable;
                return columnClone;
            }

            /// <summary>
            /// Установить ширину столбца
            /// </summary>
            public static void SetColumnWidth(XmlNode columnNode, double width)
            {
                SetAttribute(columnNode, "AutoFitWidth", XmlNamespaces.ss, "0");
                SetAttribute(columnNode, "Width", XmlNamespaces.ss, 
                    width > 0 ? width.ToString(NumberFormatInfo.InvariantInfo) : "", true);
            }
        }

        /// <summary>
        /// Строка таблицы Excel
        /// </summary>
        protected class Row
        {
            /// <summary>
            /// Ссылка на XML-узел, соответствующий строке таблицы Excel
            /// </summary>
            protected XmlNode node;
            /// <summary>
            /// Список ячеек строки
            /// </summary>
            protected List<Cell> cells;
            /// <summary>
            /// Родительская таблица данной строки
            /// </summary>
            protected Table parentTable;


            /// <summary>
            /// Конструктор
            /// </summary>
            protected Row()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="xmlNode">Ссылка на XML-узел, соответствующий строке таблицы Excel</param>
            public Row(XmlNode xmlNode)
            {
                node = xmlNode;
                cells = new List<Cell>();
                parentTable = null;
            }


            /// <summary>
            /// Получить ссылку на XML-узел, соответствующий строке таблицы Excel
            /// </summary>
            public XmlNode Node
            {
                get
                {
                    return node;
                }
            }

            /// <summary>
            /// Получить список ячеек строки
            /// </summary>
            public List<Cell> Cells
            {
                get
                {
                    return cells;
                }
            }

            /// <summary>
            /// Получить или установить родительскую таблицу данной строки
            /// </summary>
            public Table ParentTable
            {
                get
                {
                    return parentTable;
                }
                set
                {
                    parentTable = value;
                }
            }

            /// <summary>
            /// Получить или установить высоту
            /// </summary>
            public double Height
            {
                get
                {
                    string heightStr = GetAttribute(node, "ss:Height");
                    double height;
                    return double.TryParse(heightStr, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out height) ?
                        height : 0;
                }
                set
                {
                    SetRowHeight(node, value);
                }
            }


            /// <summary>
            /// Клонировать строку
            /// </summary>
            /// <returns>Копия строки</returns>
            public Row Clone()
            {
                XmlNode nodeClone = node.CloneNode(false);
                Row rowClone = new Row(nodeClone);
                rowClone.parentTable = parentTable;

                foreach (Cell cell in cells)
                {
                    Cell cellClone = cell.Clone();
                    rowClone.cells.Add(cellClone);
                    nodeClone.AppendChild(cellClone.Node);
                }

                return rowClone;
            }

            /// <summary>
            /// Найти ячейку в строке по индексу
            /// </summary>
            /// <param name="cellIndex">Индекс искомой ячейки, начиная с 1</param>
            /// <returns>Ячейка, удовлетворяющая критерию поиска, или null, если ячейка не найдена</returns>
            public Cell FindCell(int cellIndex)
            {
                int index = 0;
                foreach (Cell cell in cells)
                {
                    index = cell.Index > 0 ? cell.Index : index + 1;
                    int endIndex = index + cell.MergeAcross;

                    if (index <= cellIndex && cellIndex <= endIndex)
                        return cell;

                    index = endIndex;
                }

                return null;
            }

            /// <summary>
            /// Добавить ячейку в конец списка ячеек строки и модифицировать дерево XML-документа
            /// </summary>
            public void AppendCell(Cell cell)
            {
                cells.Add(cell);
                cell.ParentRow = this;
                node.AppendChild(cell.Node);
            }

            /// <summary>
            /// Вставить ячейку в список ячеек строки и модифицировать дерево XML-документа
            /// </summary>
            public void InsertCell(int listIndex, Cell cell)
            {
                cells.Insert(listIndex, cell);
                cell.ParentRow = this;

                if (cells.Count == 1)
                    node.AppendChild(cell.Node);
                else if (listIndex == 0)
                    node.PrependChild(cell.Node);
                else
                    node.InsertAfter(cell.Node, cells[listIndex - 1].Node);
            }

            /// <summary>
            /// Удалить ячейку из списка ячеек строки и модифицировать дерево XML-документа
            /// </summary>
            public void RemoveCell(int listIndex)
            {
                Cell cell = cells[listIndex];
                cell.ParentRow = null;
                node.RemoveChild(cell.Node);
                cells.RemoveAt(listIndex);
            }


            /// <summary>
            /// Установить высоту строки
            /// </summary>
            public static void SetRowHeight(XmlNode rowNode, double height)
            {
                SetAttribute(rowNode, "AutoFitHeight", XmlNamespaces.ss, "0");
                SetAttribute(rowNode, "Height", XmlNamespaces.ss,
                    height > 0 ? height.ToString(NumberFormatInfo.InvariantInfo) : "", true);
            }
        }

        /// <summary>
        /// Ячейка строки таблицы Excel
        /// </summary>
        protected class Cell
        {
            /// <summary>
            /// Типы данных ячеек
            /// </summary>
            public static class DataTypes
            {
                /// <summary>
                /// Строковый тип
                /// </summary>
                public const string String = "String";
                /// <summary>
                /// Числовой тип
                /// </summary>
                public const string Number = "Number";
            }

            /// <summary>
            /// Ссылка на XML-узел, соответствующий ячейке строки таблицы Excel
            /// </summary>
            protected XmlNode node;
            /// <summary>
            /// Ссылка на XML-узел, соответствующий данным ячейки
            /// </summary>
            protected XmlNode dataNode;
            /// <summary>
            /// Родительская строка данной ячейки
            /// </summary>
            protected Row parentRow;
            /// <summary>
            /// Индекс ячейки, 0 - неопределён
            /// </summary>
            protected int index;


            /// <summary>
            /// Конструктор
            /// </summary>
            protected Cell()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="xmlNode">Ссылка на XML-узел, соответствующий ячейке строки таблицы Excel</param>
            public Cell(XmlNode xmlNode)
            {
                node = xmlNode;
                dataNode = null;
                parentRow = null;

                XmlAttribute attr = node.Attributes["ss:Index"];
                index = attr == null ? 0 : int.Parse(attr.Value);
            }


            /// <summary>
            /// Получить ссылку на XML-узел, соответствующий ячейке строки таблицы Excel
            /// </summary>
            public XmlNode Node
            {
                get
                {
                    return node;
                }
            }

            /// <summary>
            /// Получить или установить ссылку на XML-узел, соответствующий данным ячейки
            /// </summary>
            public XmlNode DataNode
            {
                get
                {
                    return dataNode;
                }
                set
                {
                    dataNode = value;
                }
            }

            /// <summary>
            /// Получить или установить родительскую строку данной ячейки
            /// </summary>
            public Row ParentRow
            {
                get
                {
                    return parentRow;
                }
                set
                {
                    parentRow = value;
                }
            }

            /// <summary>
            /// Получить или установить индекс ячейки (0 - неопределён), при установке модифицируется дерево XML-документа
            /// </summary>
            public int Index
            {
                get
                {
                    return index;
                }
                set
                {
                    index = value;
                    SetAttribute(node, "Index", XmlNamespaces.ss, index <= 0 ? null : index.ToString(), true);
                }
            }

            /// <summary>
            /// Получить или установить тип данных (формат) ячейки
            /// </summary>
            public string DataType
            {
                get
                {
                    return GetAttribute(dataNode, "ss:Type");
                }
                set
                {
                    SetAttribute(dataNode, "Type", XmlNamespaces.ss, 
                        string.IsNullOrEmpty(value) ? DataTypes.String : value);
                }
            }

            /// <summary>
            /// Получить или установить текст
            /// </summary>
            public string Text
            {
                get
                {
                    return dataNode == null ? "" : dataNode.InnerText;
                }
                set
                {
                    if (dataNode != null)
                        dataNode.InnerText = value;
                }
            }

            /// <summary>
            /// Получить или установить формулу
            /// </summary>
            public string Formula
            {
                get
                {
                    return GetAttribute(node, "ss:Formula");
                }
                set
                {
                    SetAttribute(node, "Formula", XmlNamespaces.ss, value, true);
                }
            }

            /// <summary>
            /// Получить или установить ид. стиля
            /// </summary>
            public string StyleID
            {
                get
                {
                    return GetAttribute(node, "ss:StyleID");
                }
                set
                {
                    SetAttribute(node, "StyleID", XmlNamespaces.ss, value, true);
                }
            }

            /// <summary>
            /// Получить или установить количество объединямых ячеек справа
            /// </summary>
            public int MergeAcross
            {
                get
                {
                    string valStr = GetAttribute(node, "ss:MergeAcross");
                    return valStr == "" ? 0 : int.Parse(valStr);
                }
                set
                {
                    SetAttribute(node, "MergeAcross", XmlNamespaces.ss, value < 1 ? "" : value.ToString(), true);
                }
            }

            /// <summary>
            /// Получить или установить количество объединямых ячеек вниз
            /// </summary>
            public int MergeDown
            {
                get
                {
                    string valStr = GetAttribute(node, "ss:MergeDown");
                    return valStr == "" ? 0 : int.Parse(valStr);
                }
                set
                {
                    SetAttribute(node, "MergeDown", XmlNamespaces.ss, value < 1 ? "" : value.ToString(), true);
                }
            }


            /// <summary>
            /// Рассчитать индекс в строке
            /// </summary>
            public int CalcIndex()
            {
                if (index > 0)
                {
                    return index;
                }
                else
                {
                    int index = 0;
                    foreach (Cell cell in parentRow.Cells)
                    {
                        index = cell.Index > 0 ? cell.Index : index + 1;
                        if (cell == this)
                            return index;
                        index += cell.MergeAcross;
                    }
                    return 0;
                }
            }

            /// <summary>
            /// Клонировать ячейку
            /// </summary>
            /// <returns>Копия ячейки</returns>
            public Cell Clone()
            {
                XmlNode nodeClone = node.CloneNode(false);
                Cell cellClone = new Cell(nodeClone);
                cellClone.parentRow = parentRow;

                if (dataNode != null)
                {
                    cellClone.dataNode = dataNode.Clone();
                    nodeClone.AppendChild(cellClone.dataNode);

                    foreach (XmlNode childNode in node.ChildNodes)
                        if (childNode.Name != "Data")
                            nodeClone.AppendChild(childNode.Clone());
                }

                return cellClone;
            }

            /// <summary>
            /// Установить числовой тип данных ячейки
            /// </summary>
            public void SetNumberType()
            {
                DataType = DataTypes.Number;
            }
        }


        /// <summary>
        /// Обозначение перехода на новую строку в SpreadsheetML
        /// </summary>
        protected const string Break = "&#10;";
        /// <summary>
        /// Имя XML-элемента в шаблоне, который может содержать директивы преобразований
        /// </summary>
        protected const string DirectiveElem = "Data";


        /// <summary>
        /// Обрабатываемый XML-документ
        /// </summary>
        protected XmlDocument xmlDoc;
        /// <summary>
        /// Книга Excel, формируемая на основе XML-документа
        /// </summary>
        protected Workbook workbook;
        /// <summary>
        /// Обрабатываемый лист Excel
        /// </summary>
        protected Worksheet procWorksheet;
        /// <summary>
        /// Обрабатываемая таблица листа Excel
        /// </summary>
        protected Table procTable;
        /// <summary>
        /// Обрабатываемая строка таблицы Excel
        /// </summary>
        protected Row procRow;
        /// <summary>
        /// Обрабатываемая ячейка строки таблицы Excel
        /// </summary>
        protected Cell procCell;
        /// <summary>
        /// XML-узлы могут иметь текст, содержащий переносы строк
        /// </summary>
        protected bool textBroken;


        /// <summary>
        /// Конструктор
        /// </summary>
        public ExcelRepBuilder()
            : base()
        {
            xmlDoc = null;
        }


        /// <summary>
        /// Получить формат отчёта
        /// </summary>
        public override string RepFormat
        {
            get
            {
                return "SpreadsheetML";
            }
        }


        /// <summary>
        /// Получить значение атрибута XML-узла
        /// </summary>
        protected static string GetAttribute(XmlNode xmlNode, string name)
        {
            if (xmlNode == null)
            {
                return "";
            }
            else
            {
                XmlAttribute xmlAttr = xmlNode.Attributes[name];
                return xmlAttr == null ? "" : xmlAttr.Value;
            }
        }

        /// <summary>
        /// Установить атрибут XML-узла, создав его при необходимости
        /// </summary>
        protected static void SetAttribute(XmlNode xmlNode, string localName, string namespaceURI, string value, 
            bool removeEmpty = false)
        {
            if (xmlNode != null)
            {
                if (string.IsNullOrEmpty(value) && removeEmpty)
                {
                    xmlNode.Attributes.RemoveNamedItem(localName, namespaceURI);
                }
                else
                {
                    XmlAttribute xmlAttr = xmlNode.Attributes.GetNamedItem(localName, namespaceURI) as XmlAttribute;
                    if (xmlAttr == null)
                    {
                        xmlAttr = xmlNode.OwnerDocument.CreateAttribute("", localName, namespaceURI);
                        xmlAttr.Value = value;
                        xmlNode.Attributes.SetNamedItem(xmlAttr);
                    }
                    else
                    {
                        xmlAttr.Value = value;
                    }
                }
            }
        }

        /// <summary>
        /// Найти директиву в строке, получить её значение и остаток строки
        /// </summary>
        protected bool FindDirective(string s, string attrName, out string attrVal, out string rest)
        {
            // "attrName=attrVal", вместо '=' может быть любой символ
            int valStartInd = attrName.Length + 1;
            if (valStartInd <= s.Length && s.StartsWith(attrName, StringComparison.Ordinal))
            {
                int valEndInd = s.IndexOf(" ", valStartInd);
                if (valEndInd < 0)
                {
                    attrVal = s.Substring(valStartInd);
                    rest = "";
                }
                else
                {
                    attrVal = s.Substring(valStartInd, valEndInd - valStartInd);
                    rest = s.Substring(valEndInd + 1);
                }
                return true;
            }
            else
            {
                attrVal = "";
                rest = s;
                return false;
            }
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк, разбив при необходимости элемент на несколько
        /// </summary>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, string text, string textBreak)
        {
            if (text == null)
                text = "";
            xmlNode.InnerText = text.Replace(textBreak, Break);
            textBroken = true;
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк, разбив при необходимости элемент на несколько
        /// </summary>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, object text, string textBreak)
        {
            string textStr = text == null ? "" : text.ToString();
            SetNodeTextWithBreak(xmlNode, textStr, textBreak);
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк "\n", разбив при необходимости элемент на несколько
        /// </summary>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, string text)
        {
            SetNodeTextWithBreak(xmlNode, text, "\n");
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк "\n", разбив при необходимости элемент на несколько
        /// </summary>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, object text)
        {
            SetNodeTextWithBreak(xmlNode, text, "\n");
        }


        /// <summary>
        /// Предварительно обработать дерево XML-документа
        /// </summary>
        protected virtual void StartXmlDocProc()
        {
        }

        /// <summary>
        /// Рекурсивно обработать дерево XML-документа согласно директивам
        /// </summary>
        protected virtual void XmlDocProc(XmlNode xmlNode)
        {
            if (xmlNode.Name == DirectiveElem)
            {
                procCell.DataNode = xmlNode;
                FindDirectives(procCell); // поиск и обработка директив
            }
            else
            {
                // формирование книги Excel на основе XML-документа
                if (xmlNode.Name == "Workbook")
                {
                    workbook = new Workbook(xmlNode);
                }
                else if (xmlNode.Name == "Styles")
                {
                    workbook.StylesNode = xmlNode;
                }
                else if (xmlNode.Name == "Style")
                {
                    Style style = new Style(xmlNode);
                    workbook.Styles.Add(style.ID, style);
                }
                else if (xmlNode.Name == "Worksheet")
                {
                    procWorksheet = new Worksheet(xmlNode);
                    procWorksheet.ParentWorkbook = workbook;
                    workbook.Worksheets.Add(procWorksheet);
                }
                else if (xmlNode.Name == "Table")
                {
                    procTable = new Table(xmlNode);
                    procTable.ParentWorksheet = procWorksheet;
                    procWorksheet.Table = procTable;
                }
                else if (xmlNode.Name == "Column")
                {
                    Column column = new Column(xmlNode);
                    column.ParentTable = procTable;
                    procTable.Columns.Add(column);
                }
                else if (xmlNode.Name == "Row")
                {
                    procRow = new Row(xmlNode);
                    procRow.ParentTable = procTable;
                    procTable.Rows.Add(procRow);
                }
                else if (xmlNode.Name == "Cell")
                {
                    procCell = new Cell(xmlNode);
                    procCell.ParentRow = procRow;
                    procRow.Cells.Add(procCell);
                }

                // рекурсивный перебор потомков текущего элемента
                XmlNodeList children = xmlNode.ChildNodes;
                foreach (XmlNode node in children)
                    XmlDocProc(node);
            }
        }

        /// <summary>
        /// Окончательно обработать дерево XML-документа
        /// </summary>
        protected virtual void FinalXmlDocProc()
        {
        }

        /// <summary>
        /// Обработать структуры, представляющие таблицу листа Excel
        /// </summary>
        protected virtual void ExcelProc(Table table)
        {
            foreach (Row row in table.Rows)
                ExcelProc(row);
        }

        /// <summary>
        /// Обработать структуры, представляющие строку таблицы листа Excel
        /// </summary>
        protected virtual void ExcelProc(Row row)
        {
            // поиск и обработка директив для ячеек строки
            foreach (Cell cell in row.Cells)
                FindDirectives(cell);
        }

        /// <summary>
        /// Найти и обработать директивы, которые могут содержаться в заданной ячейке
        /// </summary>
        protected virtual void FindDirectives(Cell cell)
        {
            XmlNode dataNode = cell.DataNode;
            if (dataNode != null)
            {
                string attrVal;
                string rest;
                if (FindDirective(dataNode.InnerText, "repRow", out attrVal, out rest))
                {
                    dataNode.InnerText = rest;
                    ProcRow(cell, attrVal);
                }
                else if (FindDirective(dataNode.InnerText, "repVal", out attrVal, out rest))
                {
                    ProcVal(cell, attrVal);
                }
            }
        }

        /// <summary>
        /// Обработать директиву, связанную со значением ячейки
        /// </summary>
        protected virtual void ProcVal(Cell cell, string valName)
        {
        }

        /// <summary>
        /// Обработать директиву, связанную со строкой таблицы
        /// </summary>
        protected virtual void ProcRow(Cell cell, string rowName)
        {
        }


        /// <summary>
        /// Генерировать отчёт в поток в формате SpreadsheetML
        /// </summary>
        /// <remarks>Директория шаблона должна оканчиваться на слеш</remarks>
        public override void Make(Stream outStream, string templateDir)
        {
            // имя файла шаблона отчёта
            string templFileName = templateDir + TemplateFileName;

            // загрузка и разбор файла шаблона в формате XML
            xmlDoc = new XmlDocument();
            xmlDoc.Load(templFileName);

            // инициализация полей
            workbook = null;
            procWorksheet = null;
            procTable = null;
            procRow = null;
            procCell = null;
            textBroken = false;

            // создание отчёта - модификация xmlDoc
            StartXmlDocProc();
            XmlDocProc(xmlDoc.DocumentElement);
            FinalXmlDocProc();

            // запись в выходной поток
            if (textBroken)
            {
                StringWriter stringWriter = new StringWriter();
                xmlDoc.Save(stringWriter);
                string xmlText = stringWriter.GetStringBuilder()
                    .Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"")
                    .Replace("&amp;#10", "&#10").ToString();
                byte[] bytes = Encoding.UTF8.GetBytes(xmlText);
                outStream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                xmlDoc.Save(outStream);
            }
        }
    }
}
