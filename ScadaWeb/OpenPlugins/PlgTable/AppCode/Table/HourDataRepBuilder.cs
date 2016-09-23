/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Summary  : Hour data report builder
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Xml;
using Utils.Report;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Hour data report builder
    /// <para>Формирует отчёт по часовым данным</para>
    /// </summary>
    public class HourDataRepBuilder : ExcelRepBuilder
    {
        private const int MinHour = -24; // минимально допустимый час
        private const int MaxHour = 23;  // максимально допустимый час

        private DataAccess dataAccess;       // объект для доступа к данным
        private DataFormatter dataFormatter; // объект для форматирования данных

        private TableView tableView;         // табличное представление, по которому генерируется отчёт
        private DateTime date;               // дата запрашиваемых данных
        private int startHour;               // начальный час
        private int endHour;                 // конечный час

        private DateTime genDT;                   // дата и время генерации отчёта
        private Row itemRowTemplate;              // строка-шаблон табличного представления
        private TableView.Item viewItem;          // элемент представления для вывода в отчёт
        private InCnlProps cnlProps;              // свойства канала элемента представления
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
                    "Hour data";
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
        /// Начальная обработка дерева XML-документа
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
        /// Окончательная обработка дерева XML-документа
        /// </summary>
        protected override void FinalXmlDocProc()
        {
            // проверка шаблона
            if (workbook.Worksheets.Count == 0 || itemRowTemplate == null)
                throw new Exception(WebPhrases.IncorrectRepTemplate);

            // перевод наименования листа
            workbook.Worksheets[0].Name = PlgPhrases.HourDataWorksheet;

            // удаление лишних атрибутов таблицы
            Table table = itemRowTemplate.ParentTable;
            table.RemoveTableNodeAttrs();

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
                cnlProps = viewItem.CnlNum > 0 ? dataAccess.GetCnlProps(viewItem.CnlNum) : null;
                Row newRow = itemRowTemplate.Clone();
                ExcelProc(newRow);
                table.AppendRow(newRow);
            }
        }

        /// <summary>
        /// Обработка директивы, изменяющей значение элемента
        /// </summary>
        protected override void ProcVal(Cell cell, string valName)
        {
            string nodeText = null;

            if (valName == "Title")
            {
                nodeText = string.Format(PlgPhrases.HourDataTitle, 
                    tableView.Title, 
                    date.ToLocalizedDateString(), 
                    WFrmTable.GetLocalizedHour(startHour) + " - " + WFrmTable.GetLocalizedHour(endHour));
            }
            else if (valName == "Gen")
            {
                nodeText = PlgPhrases.HourDataGen + genDT.ToLocalizedString();
            }
            else if (valName == "ItemCol")
            {
                nodeText = PlgPhrases.ItemCol;
            }
            else if (valName.StartsWith("H"))
            {
                // заголовок таблицы часовых данных
                int hour;
                if (int.TryParse(valName.Substring(1), out hour))
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
                    int hour;
                    if (int.TryParse(valName.Substring(1), out hour))
                    {
                        if (viewItem.CnlNum > 0 && startHour <= hour && hour <= endHour)
                        {
                            DateTime colDT = date.AddHours(hour);
                            SrezTableLight.Srez snapshot;
                            SrezTableLight hourTable = hour >= 0 ? reqDateHourTable : prevDateHourTable;
                            hourTable.SrezList.TryGetValue(colDT, out snapshot);

                            string emptyVal;
                            if (dataFormatter.HourDataVisible(colDT, genDT, snapshot != null, out emptyVal))
                            {
                                double val;
                                int stat;
                                snapshot.GetCnlData(viewItem.CnlNum, out val, out stat);
                                nodeText = dataFormatter.FormatCnlVal(val, stat, cnlProps, false);
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
        /// Обработка директивы, создающей строки таблицы
        /// </summary>
        /// <param name="xmlNode">XML-узел, содержащий директиву</param>
        /// <param name="rowName">Имя строки, заданное директивой</param>
        protected override void ProcRow(Cell cell, string rowName)
        {
            if (rowName == "Item")
                itemRowTemplate = cell.ParentRow;
        }
    }
}