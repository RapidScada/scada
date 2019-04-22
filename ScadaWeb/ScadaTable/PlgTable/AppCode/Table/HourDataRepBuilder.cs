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
 * Module   : PlgTable
 * Summary  : Hourly data report builder
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Tables;
using Scada.Table;
using System;
using System.Xml;
using Utils.Report;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Hourly data report builder
    /// <para>Формирует отчёт по часовым данным</para>
    /// </summary>
    public class HourDataRepBuilder : ExcelRepBuilder
    {
        private const int MinHour = -24; // минимально допустимый час
        private const int MaxHour = 23;  // максимально допустимый час

        private readonly DataAccess dataAccess;       // объект для доступа к данным
        private readonly DataFormatter dataFormatter; // объект для форматирования данных

        private TableView tableView;              // табличное представление, по которому генерируется отчёт
        private DateTime date;                    // дата запрашиваемых данных
        private int startHour;                    // начальный час
        private int endHour;                      // конечный час

        private DateTime genDT;                   // дата и время генерации отчёта
        private Row itemRowTemplate;              // строка-шаблон табличного представления
        private TableView.Item viewItem;          // элемент представления для вывода в отчёт
        private SrezTableLight reqDateHourTable;  // таблица часовых данных за запрошенную дату
        private SrezTableLight prevDateHourTable; // таблица часовых данных за предыдущую дату


        /// <summary>
        /// Конструктор
        /// </summary>
        public HourDataRepBuilder(DataAccess dataAccess)
            : base()
        {
            if (dataAccess == null)
                throw new ArgumentNullException("dataAccess");

            this.dataAccess = dataAccess;
            dataFormatter = new DataFormatter();

            tableView = null;
            date = DateTime.MinValue;
            startHour = 0;
            endHour = 23;

            genDT = DateTime.MinValue;
            itemRowTemplate = null;
            viewItem = null;
            reqDateHourTable = null;
            prevDateHourTable = null;
        }


        /// <summary>
        /// Получить имя отчёта
        /// </summary>
        public override string RepName
        {
            get
            {
                return Localization.UseRussian ?
                    "Часовые данные" : 
                    "Hourly data";
            }
        }

        /// <summary>
        /// Получить имя файла шаблона
        /// </summary>
        public override string TemplateFileName
        {
            get
            {
                return "HourDataRep.xml";
            }
        }


        /// <summary>
        /// Скрыть неиспользуемые столбцы часовых данных
        /// </summary>
        protected void HideUnusedColumns(Table table)
        {
            // в шаблоне один узел Column отвечает за все столбцы часовых данных,
            // в генерируемом отчёте создаётся свой узел Column для каждого столбца
            const int HourColInd = 1; // индекс описанного выше исходного узла

            if (table.Columns.Count == HourColInd + 1)
            {
                Column srcColumn = table.Columns[HourColInd];
                srcColumn.Node.Attributes.RemoveNamedItem("ss:Span");
                table.RemoveColumn(HourColInd);

                Column visColTemplate = srcColumn.Clone();
                Column hidColTemplate = srcColumn.Clone();
                XmlAttribute hiddenAttr = xmlDoc.CreateAttribute("ss:Hidden", XmlNamespaces.ss);
                hiddenAttr.Value = "1";
                hidColTemplate.Node.Attributes.Append(hiddenAttr);

                for (int hour = MinHour; hour <= MaxHour; hour++)
                {
                    table.AppendColumn(startHour <= hour && hour <= endHour ?
                        visColTemplate.Clone() : hidColTemplate.Clone());
                }
            }
        }

        /// <summary>
        /// Установить параметры отчёта.
        /// repParams[0] - представление типа TableView,
        /// repParams[1] - дата запрашиваемых данных типа DateTime
        /// repParams[2] - начальный час типа int
        /// repParams[3] - конечный час типа int
        /// </summary>
        public override void SetParams(params object[] repParams)
        {
            tableView = (TableView)repParams[0];
            date = ((DateTime)repParams[1]).Date;
            startHour = (int)repParams[2];
            endHour = (int)repParams[3];

            // приведение начального и конечного часа в допустимые границы
            startHour = Math.Min(MaxHour, Math.Max(MinHour, startHour));
            endHour = Math.Min(MaxHour, Math.Max(MinHour, endHour));
            if (endHour < startHour)
                endHour = startHour;
        }


        /// <summary>
        /// Предварительно обработать дерево XML-документа
        /// </summary>
        protected override void StartXmlDocProc()
        {
            genDT = DateTime.Now;
            itemRowTemplate = null;
            viewItem = null;
            reqDateHourTable = null;
            prevDateHourTable = null;
        }

        /// <summary>
        /// Окончательно обработать дерево XML-документа
        /// </summary>
        protected override void FinalXmlDocProc()
        {
            // проверка шаблона
            if (workbook.Worksheets.Count == 0 || itemRowTemplate == null)
                throw new Exception(WebPhrases.IncorrectRepTemplate);

            // перевод наименования листа
            workbook.Worksheets[0].Name = TablePhrases.HourDataWorksheet;

            // удаление лишних атрибутов таблицы
            Table table = itemRowTemplate.ParentTable;
            table.RemoveTableNodeAttrs();

            // скрытие неиспользуемых столбцов
            HideUnusedColumns(table);

            // удаление строки-шаблона
            int itemRowIndex = table.Rows.IndexOf(itemRowTemplate);
            table.RemoveRow(itemRowIndex);

            // получение часовых данных
            reqDateHourTable = dataAccess.DataCache.GetHourTable(date);
            if (startHour < 0)
                prevDateHourTable = dataAccess.DataCache.GetHourTable(date.AddDays(-1));

            // вывод данных в отчёт
            for (int i = 0, cnt = tableView.Items.Count; i < cnt; i++)
            {
                viewItem = tableView.Items[i];
                if (!viewItem.Hidden)
                {
                    Row newRow = itemRowTemplate.Clone();
                    ExcelProc(newRow);
                    table.AppendRow(newRow);
                }
            }
        }

        /// <summary>
        /// Обработать директиву, связанную со значением ячейки
        /// </summary>
        protected override void ProcVal(Cell cell, string valName)
        {
            string nodeText = null;

            if (valName == "Title")
            {
                nodeText = string.Format(TablePhrases.HourDataTitle, 
                    tableView.Title, 
                    date.ToLocalizedDateString(), 
                    WFrmTable.GetLocalizedHour(startHour) + " - " + WFrmTable.GetLocalizedHour(endHour));
            }
            else if (valName == "Gen")
            {
                nodeText = TablePhrases.HourDataGen + genDT.ToLocalizedString();
            }
            else if (valName == "ItemCol")
            {
                nodeText = TablePhrases.ItemCol;
            }
            else if (valName.StartsWith("H"))
            {
                // заголовок таблицы часовых данных
                if (int.TryParse(valName.Substring(1), out int hour))
                    nodeText = WFrmTable.GetLocalizedHour(hour);
            }
            else if (viewItem != null)
            {
                if (valName == "Name")
                {
                    nodeText = viewItem.Caption;
                }
                else if (valName.StartsWith("h"))
                {
                    // часовые данные
                    if (int.TryParse(valName.Substring(1), out int hour))
                    {
                        if (viewItem.CnlNum > 0 && startHour <= hour && hour <= endHour)
                        {
                            DateTime colDT = date.AddHours(hour);
                            SrezTableLight hourTable = hour >= 0 ? reqDateHourTable : prevDateHourTable;
                            hourTable.SrezList.TryGetValue(colDT, out SrezTableLight.Srez snapshot);

                            if (dataFormatter.HourDataVisible(colDT, genDT, snapshot != null, out string emptyVal))
                            {
                                // получение данных
                                snapshot.GetCnlData(viewItem.CnlNum, out double val, out int stat);

                                // форматирование данных
                                dataFormatter.FormatCnlVal(val, stat, viewItem.CnlProps, ".", "", 
                                    out string text, out string textWithUnit, out bool textIsNumber);
                                string color = dataFormatter.GetCnlValColor(val, stat, viewItem.CnlProps, 
                                    dataAccess.GetCnlStatProps(stat));

                                // вывод данных
                                nodeText = text;
                                workbook.SetColor(cell.Node, null, color);
                                if (textIsNumber)
                                    cell.SetNumberType();
                            }
                            else
                            {
                                nodeText = emptyVal;
                            }
                        }
                        else
                        {
                            nodeText = "";
                        }
                    }
                }
            }

            if (nodeText != null)
                cell.DataNode.InnerText = nodeText;
        }

        /// <summary>
        /// Обработать директиву, связанную со строкой таблицы
        /// </summary>
        protected override void ProcRow(Cell cell, string rowName)
        {
            if (rowName == "Item")
                itemRowTemplate = cell.ParentRow;
        }
    }
}