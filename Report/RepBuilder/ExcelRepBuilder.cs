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
 * Module   : Report Builder
 * Summary  : The base class for building reports in SpreadsheetML (Microsoft Excel 2003) format
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2009
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
    /// The base class for building reports in SpreadsheetML (Microsoft Excel 2003) format
    /// <para>Базовый класс для построения отчётов в формате SpreadsheetML (Microsoft Excel 2003)</para>
    /// </summary>
    public abstract class ExcelRepBuilder : RepBuilder
    {
        /// <summary>
        /// Книга Excel
        /// </summary>
        protected class Workbook
        {
            protected XmlNode node;                     // ссылка на XML-узел, соответствующий книге Excel
            protected XmlNode stylesNode;               // ссылка на XML-узел, содержащий стили книги Excel
            protected SortedList<string, Style> styles; // список стилей книги Excel с возможностью доступа по ID стиля
            protected List<Worksheet> worksheets;       // список листов книги Excel


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
                string name = worksheetName.ToLower();
                foreach (Worksheet worksheet in worksheets)
                    if (worksheet.Name.ToLower() == name)
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
            /// Установить цвет объекта, создав при необходимости новый стиль
            /// </summary>
            /// <param name="targetNode">Ссылка на XML-узел объекта, которому устанавливается цвет</param>
            /// <param name="color">Устанавливаемый цвет</param>
            public void SetColor(XmlNode targetNode, string color)
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
                string newStyleID = oldStyleID + "_" + color;

                if (styles.ContainsKey(newStyleID))
                {
                    // установка созданного ранее стиля с заданным цветом
                    styleAttr.Value = newStyleID;
                }
                else
                {
                    // создание нового стиля
                    Style newStyle;
                    if (styleAttr == null)
                    {
                        XmlNode newStyleNode = xmlDoc.CreateNode(XmlNodeType.Element, "Style", namespaceURI);
                        newStyleNode.Attributes.Append(xmlDoc.CreateAttribute("ss", "ID", namespaceURI));
                        newStyle = new Style(newStyleNode);
                    }
                    else
                        newStyle = styles[oldStyleID].Clone();
                    newStyle.ID = newStyleID;

                    // уставнока цвета в созданном стиле
                    XmlNode interNode = newStyle.Node.FirstChild;
                    while (interNode != null && interNode.Name != "Interior")
                        interNode = interNode.NextSibling;

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
                    xmlAttr.Value = color;
                    interNode.Attributes.Append(xmlAttr);
                    xmlAttr = xmlDoc.CreateAttribute("ss", "Pattern", namespaceURI);
                    xmlAttr.Value = "Solid";
                    interNode.Attributes.Append(xmlAttr);

                    // установка нового стиля объекту и добавление стиля в книгу
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
            protected XmlNode node; // ссылка на XML-узел, соответствующий стилю книги Excel
            protected string id;    // идентификатор стиля


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
            protected XmlNode node;            // ссылка на XML-узел, соответствующий листу книги Excel
            protected string name;             // имя листа
            protected Table table;             // таблица с содержимым листа
            protected Workbook parentWorkbook; // родительский книга данного листа


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
            protected XmlNode node;              // ссылка на XML-узел, соответствующий таблице листа Excel
            protected List<Column> columns;      // список столбцов таблицы
            protected List<Row> rows;            // список строк таблицы
            protected Worksheet parentWorksheet; // родительский лист данной таблицы


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
            /// <param name="columnIndex">Индекс искомого столбца</param>
            /// <param name="exact">Получен столбец с указанным индексом (true) или с ближайшим большим (false)</param>
            /// <returns>Столбец, удовлетворяющий критерию поиска, или null, если индексы всех столбцов меньше заданного</returns>
            public Column FindColumn(int columnIndex, out bool exact)
            {
                int index = 0;
                foreach (Column column in columns)
                {
                    if (column.Index > 0)
                        index = column.Index;
                    else
                        index++;

                    if (index == columnIndex)
                    {
                        exact = true;
                        return column;
                    }
                    else if (index > columnIndex)
                    {
                        exact = false;
                        return column;
                    }
                }

                exact = false;
                return null;
            }

            /// <summary>
            /// Добавить столбец в конец списка столбцов таблицы и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="column">Добавляемый столбец</param>
            public void AppendColumn(Column column)
            {
                columns.Add(column);
                node.AppendChild(column.Node);
            }

            /// <summary>
            /// Вставить столбец в список столбцов таблицы и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="listIndex">Индекс вставляемого столбца в списке</param>
            /// <param name="column">Вставляемый столбец</param>
            public void InsertColumn(int listIndex, Column column)
            {
                columns.Insert(listIndex, column);

                if (columns.Count == 1)
                    node.AppendChild(column.Node);
                else if (listIndex == 0)
                    node.PrependChild(column.Node);
                else
                    node.InsertAfter(column.Node, columns[listIndex - 1].Node);
            }

            /// <summary>
            /// Удалить столбец из списка столбцов таблицы и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="listIndex">Индекс удаляемого столбца в списке</param>
            public void RemoveColumn(int listIndex)
            {
                Column column = columns[listIndex];
                node.RemoveChild(column.Node);
                columns.RemoveAt(listIndex);
            }

            /// <summary>
            /// Добавить строку в конец списка строк таблицы и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="row">Добавляемая строка</param>
            public void AppendRow(Row row)
            {
                rows.Add(row);
                node.AppendChild(row.Node);
            }

            /// <summary>
            /// Вставить строку в список строк таблицы и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="listIndex">Индекс вставляемой строки в списке</param>
            /// <param name="row">Вставляемая строка</param>
            public void InsertRow(int listIndex, Row row)
            {
                rows.Insert(listIndex, row);

                if (rows.Count == 1)
                    node.AppendChild(row.Node);
                else if (listIndex == 0)
                    node.PrependChild(row.Node);
                else
                    node.InsertAfter(row.Node, rows[listIndex - 1].Node);
            }

            /// <summary>
            /// Удалить строку из списка строк таблицы и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="listIndex">Индекс удаляемой строки в списке</param>
            public void RemoveRow(int listIndex)
            {
                Row row = rows[listIndex];
                node.RemoveChild(row.Node);
                rows.RemoveAt(listIndex);
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
            protected XmlNode node;      // ссылка на XML-узел, соответствующий столбцу таблицы Excel
            protected int index;         // индекс столбца, 0 - неопределён
            protected Table parentTable; // родительская таблица данного столбца


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
                    if (index <= 0)
                    {
                        if (value <= 0)
                            index = value;
                        else
                        {
                            XmlAttribute attr = node.OwnerDocument.CreateAttribute("ss:Index");
                            attr.Value = index.ToString();
                            node.Attributes.SetNamedItem(attr);
                        }
                    }
                    else
                    {
                        if (value <= 0)
                            node.Attributes.RemoveNamedItem("ss:Index");
                        else
                            node.Attributes["ss:Index"].Value = index.ToString();
                    }
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
                XmlAttribute attr = node.Attributes["ss:Index"];
                index = attr == null ? 0 : int.Parse(attr.Value);
                parentTable = null;
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
        }

        /// <summary>
        /// Строка таблицы Excel
        /// </summary>
        protected class Row
        {
            protected XmlNode node;      // ссылка на XML-узел, соответствующий строке таблицы Excel
            protected List<Cell> cells;  // список ячеек строки
            protected Table parentTable; // родительская таблица данной строки


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
            /// <param name="cellIndex">Индекс искомой ячейки</param>
            /// <param name="exact">Получена ячейка с указанным индексом (true) или с ближайшим большим (false)</param>
            /// <returns>Ячейка, удовлетворяющая критерию поиска, или null, если индексы всех ячеек меньше заданного</returns>
            public Cell FindCell(int cellIndex, out bool exact)
            {
                int index = 0;
                foreach (Cell cell in cells)
                {
                    XmlAttribute attr = cell.Node.Attributes["ss:MergeAcross"];
                    int merge = attr == null ? 0 : int.Parse(attr.Value);

                    if (cell.Index > 0)
                        index = cell.Index;
                    else
                        index++;

                    if (index == cellIndex || index < cellIndex && cellIndex <= index + merge)
                    {
                        exact = true;
                        return cell;
                    }
                    else if (index > cellIndex)
                    {
                        exact = false;
                        return cell;
                    }

                    index += merge;
                }

                exact = false;
                return null;
            }

            /// <summary>
            /// Добавить ячейку в конец списка ячеек строки и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="cell">Добавляемая ячейка</param>
            public void AppendCell(Cell cell)
            {
                cells.Add(cell);
                node.AppendChild(cell.Node);
            }

            /// <summary>
            /// Вставить ячейку в список ячеек строки и модифицировать дерево XML-документа
            /// </summary>
            /// <param name="listIndex">Индекс вставляемой ячейки в списке</param>
            /// <param name="cell">Вставляемая ячейка</param>
            public void InsertCell(int listIndex, Cell cell)
            {
                cells.Insert(listIndex, cell);

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
            /// <param name="listIndex">Индекс удаляемой ячейки в списке</param>
            public void RemoveCell(int listIndex)
            {
                Cell cell = cells[listIndex];
                node.RemoveChild(cell.Node);
                cells.RemoveAt(listIndex);
            }


            /// <summary>
            /// Установить высоту строки
            /// </summary>
            /// <param name="height">Высота строки</param>
            public void SetRowHeight(double height)
            {
                SetRowHeight(node, height);
            }

            /// <summary>
            /// Установить высоту строки
            /// </summary>
            /// <param name="rowNode">Ссылка на XML-узел, соответствующий строке таблицы Excel</param>
            /// <param name="height">Высота строки</param>
            public static void SetRowHeight(XmlNode rowNode, double height)
            {
                rowNode.Attributes.RemoveNamedItem("ss:AutoFitHeight");
                rowNode.Attributes.RemoveNamedItem("ss:Height");

                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";

                XmlAttribute attr = rowNode.OwnerDocument.CreateAttribute("ss", "Height", rowNode.NamespaceURI);
                attr.Value = height.ToString(nfi);
                rowNode.Attributes.Append(attr);
            }
        }

        /// <summary>
        /// Ячейка строки таблицы Excel
        /// </summary>
        protected class Cell
        {
            protected XmlNode node;     // ссылка на XML-узел, соответствующий ячейке строки таблицы Excel
            protected XmlNode dataNode; // ссылка на XML-узел, соответствующий данным ячейки
            protected int index;        // индекс ячейки, 0 - неопределён
            protected Row parentRow;    // родительская строка данной ячейки


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
            /// Получить ссылку на XML-узел, соответствующий данным ячейки
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
                    if (index <= 0)
                    {
                        if (value <= 0)
                            index = value;
                        else
                        {
                            XmlAttribute attr = node.OwnerDocument.CreateAttribute("ss:Index");
                            attr.Value = index.ToString();
                            node.Attributes.SetNamedItem(attr);
                        }
                    }
                    else
                    {
                        if (value <= 0)
                            node.Attributes.RemoveNamedItem("ss:Index");
                        else
                            node.Attributes["ss:Index"].Value = index.ToString();
                    }
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
                XmlAttribute attr = node.Attributes["ss:Index"];
                index = attr == null ? 0 : int.Parse(attr.Value);
                parentRow = null;
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
        }


        /// <summary>
        /// Обозначение перехода на новую строку в SpreadsheetML
        /// </summary>
        protected const string Break = "&#10;";
        /// <summary>
        /// Имя с префиксом XML-элемента в шаблоне, значение которого может содержать директивы изменения
        /// </summary>
        protected const string ElemName = "Data";


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
        protected Worksheet worksheet;
        /// <summary>
        /// Обрабатываемая таблица листа Excel
        /// </summary>
        protected Table table;
        /// <summary>
        /// Обрабатываемая строка таблицы Excel
        /// </summary>
        protected Row row;
        /// <summary>
        /// Обрабатываемая ячейка строки таблицы Excel
        /// </summary>
        protected Cell cell;
        /// <summary>
        /// XML-узлы могут иметь текст, содержащий переносы строк
        /// </summary>
        protected bool textBreaked;


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
        /// Найти атрибут (директиву) в строке и получить его значение
        /// </summary>
        /// <param name="str">Строка для поиска</param>
        /// <param name="attrName">Имя искомого атрибута</param>
        /// <param name="attrVal">Значение атрибута</param>
        /// <returns>Найден ли атрибут</returns>
        protected bool FindAttr(string str, string attrName, out string attrVal)
        {
            // "attrName=attrVal", вместо '=' может быть любой символ
            attrVal = "";
            if (str.StartsWith(attrName))
            {
                int start = attrName.Length + 1;
                if (start < str.Length)
                {
                    int end = str.IndexOf(" ", start);
                    if (end < 0) end = str.Length;
                    attrVal = str.Substring(start, end - start);
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк, разбив при необходимости элемент на несколько
        /// </summary>
        /// <param name="xmlNode">XML-узел</param>
        /// <param name="text">Устанавливаемый текст</param>
        /// <param name="textBreak">Обозначение переноса строки в устанавливаемом тексте</param>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, string text, string textBreak)
        {
            if (text == null) text = "";
            xmlNode.InnerText = text.Replace(textBreak, Break);
            textBreaked = true;
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк, разбив при необходимости элемент на несколько
        /// </summary>
        /// <param name="xmlNode">XML-узел</param>
        /// <param name="text">Устанавливаемый текст</param>
        /// <param name="textBreak">Обозначение переноса строки в устанавливаемом тексте</param>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, object text, string textBreak)
        {
            string textStr = text == null ? "" : text.ToString();
            SetNodeTextWithBreak(xmlNode, textStr, textBreak);
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк "\n", разбив при необходимости элемент на несколько
        /// </summary>
        /// <param name="xmlNode">XML-узел</param>
        /// <param name="text">Устанавливаемый текст</param>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, string text)
        {
            SetNodeTextWithBreak(xmlNode, text, "\n");
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк "\n", разбив при необходимости элемент на несколько
        /// </summary>
        /// <param name="xmlNode">XML-узел</param>
        /// <param name="text">Устанавливаемый текст</param>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, object text)
        {
            SetNodeTextWithBreak(xmlNode, text, "\n");
        }


        /// <summary>
        /// Начальная обработка дерева XML-документа
        /// </summary>
        protected virtual void StartXmlDocProc()
        {
        }

        /// <summary>
        /// Рекурсивный обход, распознавание и обработка дерева XML-документа согласно директивам для получения отчёта
        /// </summary>
        /// <param name="xmlNode">Обрабатываемый XML-узел</param>
        protected virtual void XmlDocProc(XmlNode xmlNode)
        {
            if (xmlNode.Name == ElemName)
            {
                cell.DataNode = xmlNode;

                // поиск директив преобразований элементов
                string nodeVal = xmlNode.InnerText;
                string attrVal;
                if (FindAttr(nodeVal, "repRow", out attrVal))
                {
                    if (nodeVal.Length < 8 /*"repRow=".Length + 1*/ + attrVal.Length)
                        xmlNode.InnerText = "";
                    else
                        xmlNode.InnerText = nodeVal.Substring(8 + attrVal.Length);
                    ProcRow(cell, attrVal);
                }
                else if (FindAttr(nodeVal, "repVal", out attrVal))
                    ProcVal(cell, attrVal);
            }
            else
            {
                // формирование книги Excel на основе XML-документа
                if (xmlNode.Name == "Workbook")
                    workbook = new Workbook(xmlNode);
                else if (xmlNode.Name == "Styles")
                    workbook.StylesNode = xmlNode;
                else if (xmlNode.Name == "Style")
                {
                    Style style = new Style(xmlNode);
                    workbook.Styles.Add(style.ID, style);
                }
                else if (xmlNode.Name == "Worksheet")
                {
                    worksheet = new Worksheet(xmlNode);
                    worksheet.ParentWorkbook = workbook;
                    workbook.Worksheets.Add(worksheet);
                }
                else if (xmlNode.Name == "Table")
                {
                    table = new Table(xmlNode);
                    table.ParentWorksheet = worksheet;
                    worksheet.Table = table;
                }
                else if (xmlNode.Name == "Column")
                {
                    Column column = new Column(xmlNode);
                    column.ParentTable = table;
                    table.Columns.Add(column);
                }
                else if (xmlNode.Name == "Row")
                {
                    row = new Row(xmlNode);
                    row.ParentTable = table;
                    table.Rows.Add(row);
                }
                else if (xmlNode.Name == "Cell")
                {
                    cell = new Cell(xmlNode);
                    cell.ParentRow = row;
                    row.Cells.Add(cell);
                }

                // рекурсивный перебор потомков текущего элемента
                XmlNodeList children = xmlNode.ChildNodes;
                foreach (XmlNode node in children)
                    XmlDocProc(node);
            }
        }

        /// <summary>
        /// Окончательная обработка дерева XML-документа
        /// </summary>
        protected virtual void FinalXmlDocProc()
        {
        }

        /// <summary>
        /// Обработка структур, представляющих книгу Excel
        /// </summary>
        /// <param name="table">Таблица листа Excel</param>
        protected virtual void ExcelProc(Table table)
        {
            foreach (Row row in table.Rows)
                ExcelProc(row);
        }

        /// <summary>
        /// Обработка структур, представляющих книгу Excel
        /// </summary>
        /// <param name="row">Строка таблицы Excel</param>
        protected virtual void ExcelProc(Row row)
        {
            foreach (Cell cell in row.Cells)
            {
                // поиск директив преобразований элементов
                XmlNode dataNode = cell.DataNode;
                if (dataNode != null)
                {
                    string nodeVal = cell.DataNode.InnerText;
                    string attrVal;
                    if (FindAttr(nodeVal, "repRow", out attrVal))
                    {
                        if (nodeVal.Length < 8 /*"repRow=".Length + 1*/ + attrVal.Length)
                            dataNode.InnerText = "";
                        else
                            dataNode.InnerText = nodeVal.Substring(8 + attrVal.Length);
                        ProcRow(cell, attrVal);
                    }
                    else if (FindAttr(nodeVal, "repVal", out attrVal))
                        ProcVal(cell, attrVal);
                }
            }
        }

        /// <summary>
        /// Обработка директивы, изменяющей значение элемента
        /// </summary>
        /// <param name="cell">Ячейка строки таблицы Excel, содержащая директиву</param>
        /// <param name="valName">Имя элемента, заданное директивой</param>
        protected virtual void ProcVal(Cell cell, string valName)
        {
        }

        /// <summary>
        /// Обработка директивы, создающей строки таблицы
        /// </summary>
        /// <param name="cell">Ячейка строки таблицы Excel, содержащая директиву</param>
        /// <param name="rowName">Имя строки, заданное директивой</param>
        protected virtual void ProcRow(Cell cell, string rowName)
        {
        }


        /// <summary>
        /// Генерировать отчёт в поток в формате SpreadsheetML
        /// </summary>
        /// <param name="outStream">Выходной поток</param>
        /// <param name="templateDir">Директория шаблона со '\' на конце</param>
        public override void Make(Stream outStream, string templateDir)
        {
            // имя файла шаблона отчёта
            string templFileName = templateDir + TemplateFileName;

            // загрузка и разбор файла шаблона в формате XML
            xmlDoc = new XmlDocument();
            xmlDoc.Load(templFileName);

            // инициализация полей
            workbook = null;
            worksheet = null;
            table = null;
            row = null;
            cell = null;
            textBreaked = false;

            // создание отчёта - модификация xmlDoc
            StartXmlDocProc();
            XmlDocProc(xmlDoc.DocumentElement);
            FinalXmlDocProc();

            // запись в выходной поток
            if (textBreaked)
            {
                StringWriter stringWriter = new StringWriter();
                xmlDoc.Save(stringWriter);
                string xmlText = stringWriter.GetStringBuilder().Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"").
                    Replace("&amp;#10", "&#10").ToString();
                byte[] bytes = Encoding.UTF8.GetBytes(xmlText);
                outStream.Write(bytes, 0, bytes.Length);
            }
            else
                xmlDoc.Save(outStream);
        }
    }
}
