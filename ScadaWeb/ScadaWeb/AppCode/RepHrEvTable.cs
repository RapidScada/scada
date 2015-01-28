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
 * Module   : SCADA-Web
 * Summary  : Generate "Hourly data and events" report
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Xml;
using Scada.Client;
using Scada.Data;
using Utils.Report;

namespace Scada.Web
{
    /// <summary>
    /// Generate "Hourly data and events" report
    /// <para>Генерация отчёта "Часовые срезы и события"</para>
    /// </summary>
    public class RepHrEvTable : ExcelRepBuilder
    {
        private BaseView baseView;            // представление, по которому генерируется отчёт
        private TableView tableView;          // представление, приведённое к типу табличного представления
        private DateTime date;                // дата запрашиваемых данных
        private int eventOut;                 // тип вывода событий
        private DateTime genDT;               // дата и время генерации отчёта

        private Row templItemRow;             // строка-шаблон элемента таблицы часовых срезов
        private Row templEventRow;            // строка-шаблон таблицы событий

        private SrezTableLight hourTable;     // таблица часовых срезов
        private EventTableLight eventTable;   // таблица событий
        private TableView.Item item;          // обрабатываемый элемент представления
        private MainData.EventView eventView; // обрабатываемое событие


        /// <summary>
        /// Конструктор
        /// </summary>
        public RepHrEvTable()
            : base()
        {
        }


        /// <summary>
        /// Получить имя отчёта
        /// </summary>
        public override string RepName
        {
            get
            {
                return "Hour data and events";
            }
        }

        /// <summary>
        /// Получить описание отчёта
        /// </summary>
        public override string RepDescr
        {
            get
            {
                return "Hour data and events";
            }
        }

        /// <summary>
        /// Получить имя файла шаблона
        /// </summary>
        public override string TemplateFileName
        {
            get
            {
                return "RepHrEvTable.xml";
            }
        }
        

        /// <summary>
        /// Установить параметры отчёта.
        /// repParams[0] - табличное представление типа TableView,
        /// repParams[1] - дата запрашиваемых данных типа DateTime,
        /// repParams[2] - вывод событий типа int: 0 и меньше - не выводить, 1 - все события, иначе - по представлению
        /// </summary>
        public override void SetParams(params object[] repParams)
        {
            baseView = (BaseView)repParams[0];
            tableView = baseView as TableView;
            date = (DateTime)repParams[1];
            eventOut = (int)repParams[2];

            if (tableView == null && eventOut <= 0)
                throw new Exception(WebPhrases.NoReportData);
        }

        /// <summary>
        /// Начальная обработка дерева XML-документа
        /// </summary>
        protected override void StartXmlDocProc()
        {
            genDT = DateTime.Now;

            if (tableView == null)
                hourTable = null;
            else
                AppData.MainData.RefreshData(date, out hourTable);

            if (eventOut <= 0)
                eventTable = null;
            else
                AppData.MainData.RefreshEvents(date, out eventTable);

            templItemRow = null;
            templEventRow = null;
            item = null;
            eventView = null;
        }

        /// <summary>
        /// Окончательная обработка дерева XML-документа
        /// </summary>
        protected override void FinalXmlDocProc()
        {
            int hourDataWsInd = 0; // индекс листа часовых срезов
            int eventsWsInd = 1;   // индекс листа событий
            bool hourDataWsExists = workbook != null && workbook.Worksheets.Count > hourDataWsInd;
            bool eventsWsExists = workbook != null && workbook.Worksheets.Count > eventsWsInd;

            // работа с листом часовых срезов
            if (tableView == null)
            {
                // удаление листа часовых срезов
                if (hourDataWsExists)
                {
                    workbook.RemoveWorksheet(hourDataWsInd);
                    eventsWsInd--;
                }
            }
            else
            {
                // перевод наименования листа
                if (hourDataWsExists)
                    workbook.Worksheets[hourDataWsInd].Name = WebPhrases.HourDataPage;

                if (templItemRow != null)
                {
                    // удаление лишних атрибутов таблицы
                    Table table = templItemRow.ParentTable;
                    table.RemoveTableNodeAttrs();

                    // заполнение таблицы часовых срезов
                    for (int i = 0; i < tableView.VisibleCount; i++)
                    {
                        item = tableView.VisibleItems[i];
                        Row rowClone = templItemRow.Clone();
                        ExcelProc(rowClone);
                        table.AppendRow(rowClone);
                    }
                    item = null;

                    // удаление строки-шаблона
                    int rowIndex = table.Rows.IndexOf(templItemRow);
                    table.RemoveRow(rowIndex);
                }
            }

            // работа с листом событий
            if (eventOut <= 0)
            {
                // удаление листа событий
                if (eventsWsExists)
                    workbook.RemoveWorksheet(eventsWsInd);
            }
            else
            {
                // перевод наименования листа
                if (eventsWsExists)
                    workbook.Worksheets[eventsWsInd].Name = WebPhrases.EventsPage;

                if (templEventRow != null)
                {
                    // удаление лишних атрибутов таблицы
                    Table table = templEventRow.ParentTable;
                    table.RemoveTableNodeAttrs();

                    // выбор событий
                    List<MainData.EventView> eventViewList = 
                        AppData.MainData.ConvertEvents(eventOut == 1 ?
                            AppData.MainData.GetEvents(eventTable, null) :
                            AppData.MainData.GetEvents(eventTable, baseView.CnlList));

                    // заполнение таблицы событий
                    for (int i = 0; i < eventViewList.Count; i++)
                    {
                        eventView = eventViewList[i];
                        Row rowClone = templEventRow.Clone();
                        ExcelProc(rowClone);
                        table.AppendRow(rowClone);
                    }

                    // удаление строки-шаблона
                    int rowIndex = table.Rows.IndexOf(templEventRow);
                    table.RemoveRow(rowIndex);
                }
            }
        }

        /// <summary>
        /// Обработка директивы, изменяющей значение элемента
        /// </summary>
        /// <param name="xmlNode">XML-узел, содержащий директиву</param>
        /// <param name="valName">Имя элемента, заданное директивой</param>
        protected override void ProcVal(Cell cell, string valName)
        {
            XmlNode dataNode = cell.DataNode;

            if (valName == "HourDataPage")
            {
                dataNode.InnerText = WebPhrases.HourDataPage;
            }
            else if (valName == "HourDataTitle")
            {
                dataNode.InnerText = string.Format(WebPhrases.HourDataTitle, date.ToString("d", Localization.Culture), 
                    baseView.Title, genDT.ToString("d", Localization.Culture));
            }
            else if (valName == "ItemCol")
            {
                dataNode.InnerText = WebPhrases.ItemColumn;
            }
            else if (valName == "EventsPage")
            {
                dataNode.InnerText = WebPhrases.EventsPage;
            }
            else if (valName == "EventsTitle")
            {
                string dateStr = date.ToString("d", Localization.Culture);
                string genDTStr = genDT.ToString("d", Localization.Culture);
                dataNode.InnerText = eventOut <= 1 ?
                    string.Format(WebPhrases.AllEventsTitle, dateStr, genDTStr) :
                    string.Format(WebPhrases.EventsByViewTitle, dateStr, baseView.Title, genDTStr);
            }
            else if (valName == "NumCol")
            {
                dataNode.InnerText = WebPhrases.NumColumn;
            }
            else if (valName == "DateCol")
            {
                dataNode.InnerText = WebPhrases.DateColumn;
            }
            else if (valName == "TimeCol")
            {
                dataNode.InnerText = WebPhrases.TimeColumn;
            }
            else if (valName == "ObjCol")
            {
                dataNode.InnerText = WebPhrases.ObjColumn;
            }
            else if (valName == "KPCol")
            {
                dataNode.InnerText = WebPhrases.KPColumn;
            }
            else if (valName == "CnlCol")
            {
                dataNode.InnerText = WebPhrases.CnlColumn;
            }
            else if (valName == "EvCol")
            {
                dataNode.InnerText = WebPhrases.EventColumn;
            }
            else if (valName == "ChkCol")
            {
                dataNode.InnerText = WebPhrases.CheckColumn;
            }
            else if (item != null)
            {
                if (valName == "Name")
                {
                    dataNode.InnerText = item.Caption;
                }
                else if (valName.Length >= 2 && valName[0] == 'h')
                {
                    int hour = -1;
                    try { hour = int.Parse(valName.Substring(1)); }
                    catch { }

                    int cnlNum = item.CnlNum;
                    if (hour >= 0 && cnlNum > 0)
                    {
                        DateTime dateTime = date.AddHours(hour);
                        double val;
                        int stat;
                        bool isNumber;
                        string color;

                        AppData.MainData.GetHourData(hourTable, cnlNum, dateTime, out val, out stat);
                        CnlProps cnlProps = AppData.MainData.GetCnlProps(cnlNum);
                        dataNode.InnerText = AppData.MainData.FormatCnlVal(val, stat, cnlProps, false, false,
                            dateTime, genDT, out isNumber, out color, ".", "");

                        if (isNumber)
                            dataNode.Attributes["ss:Type"].Value = "Number";
                    }
                    else
                    {
                        dataNode.InnerText = "";
                    }
                }
            }
            else if (eventView != null)
            {
                if (valName == "Num")
                {
                    dataNode.InnerText = eventView.Num;
                    dataNode.Attributes["ss:Type"].Value = "Number";
                }
                else if (valName == "Date")
                    dataNode.InnerText = eventView.Date;
                else if (valName == "Time")
                    dataNode.InnerText = eventView.Time;
                else if (valName == "Obj")
                    dataNode.InnerText = eventView.Obj;
                else if (valName == "KP")
                    dataNode.InnerText = eventView.KP;
                else if (valName == "Cnl")
                    dataNode.InnerText = eventView.Cnl;
                else if (valName == "Ev")
                    dataNode.InnerText = eventView.Text;
                else if (valName == "Chk")
                    dataNode.InnerText = eventView.User;
                else
                    dataNode.InnerText = "";
            }
        }

        /// <summary>
        /// Обработка директивы, создающей строки таблицы
        /// </summary>
        /// <param name="xmlNode">XML-узел, содержащий директиву</param>
        /// <param name="rowName">Имя строки, заданное директивой</param>
        protected override void ProcRow(Cell cell, string rowName)
        {
            if (rowName == "Item")
                templItemRow = cell.ParentRow;
            else if (rowName == "Event")
                templEventRow = cell.ParentRow;
        }
    }
}