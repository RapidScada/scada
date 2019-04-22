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
 * Summary  : Events report builder
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Table;
using System;
using System.Collections.Generic;
using Utils.Report;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Events report builder
    /// <para>Формирует отчёт по событиям</para>
    /// </summary>
    public class EventsRepBuilder : ExcelRepBuilder
    {
        private readonly DataAccess dataAccess; // объект для доступа к данным

        private BaseView view;        // представление, по которому генерируется отчёт
        private DateTime date;        // дата запрашиваемых данных

        private Row eventRowTemplate; // строка-шаблон таблицы событий
        private DispEvent dispEvent;  // событие для вывода в отчёт


        /// <summary>
        /// Конструктор
        /// </summary>
        public EventsRepBuilder(DataAccess dataAccess)
            : base()
        {
            if (dataAccess == null)
                throw new ArgumentNullException("dataAccess");

            this.dataAccess = dataAccess;

            view = null;
            date = DateTime.MinValue;

            eventRowTemplate = null;
            dispEvent = null;
        }


        /// <summary>
        /// Получить имя отчёта
        /// </summary>
        public override string RepName
        {
            get
            {
                return Localization.UseRussian ?
                    "События" : 
                    "Events";
            }
        }

        /// <summary>
        /// Получить имя файла шаблона
        /// </summary>
        public override string TemplateFileName
        {
            get
            {
                return "EventsRep.xml";
            }
        }


        /// <summary>
        /// Установить параметры отчёта.
        /// repParams[0] - представление типа BaseView,
        /// repParams[1] - дата запрашиваемых данных типа DateTime
        /// </summary>
        public override void SetParams(params object[] repParams)
        {
            view = (BaseView)repParams[0];
            date = (DateTime)repParams[1];
        }


        /// <summary>
        /// Предварительно обработать дерево XML-документа
        /// </summary>
        protected override void StartXmlDocProc()
        {
            eventRowTemplate = null;
            dispEvent = null;
        }

        /// <summary>
        /// Окончательно обработать дерево XML-документа
        /// </summary>
        protected override void FinalXmlDocProc()
        {
            // проверка шаблона
            if (workbook.Worksheets.Count == 0 || eventRowTemplate == null)
                throw new Exception(WebPhrases.IncorrectRepTemplate);

            // перевод наименования листа
            workbook.Worksheets[0].Name = TablePhrases.EventsWorksheet;

            // удаление лишних атрибутов таблицы
            Table table = eventRowTemplate.ParentTable;
            table.RemoveTableNodeAttrs();

            // удаление строки-шаблона
            int eventRowIndex = table.Rows.IndexOf(eventRowTemplate);
            table.RemoveRow(eventRowIndex);

            // выбор событий
            EventTableLight tblEvent = dataAccess.DataCache.GetEventTable(date);
            List<EventTableLight.Event> events = view == null ?
                tblEvent.AllEvents :
                tblEvent.GetFilteredEvents(new EventTableLight.EventFilter(
                    EventTableLight.EventFilters.Cnls) { CnlNums = view.CnlSet });

            // вывод событий в отчёт
            DataFormatter dataFormatter = new DataFormatter();

            foreach (EventTableLight.Event ev in events)
            {
                dispEvent = dataAccess.GetDispEvent(ev, dataFormatter);
                Row newRow = eventRowTemplate.Clone();
                ExcelProc(newRow);
                table.AppendRow(newRow);
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
                nodeText = view == null ?
                    string.Format(TablePhrases.AllEventsTitle, date.ToLocalizedDateString()) :
                    string.Format(TablePhrases.EventsByViewTitle, view.Title, date.ToLocalizedDateString());
            }
            else if (valName == "Gen")
            {
                nodeText = TablePhrases.EventsGen + DateTime.Now.ToLocalizedString();
            }
            else if (valName == "NumCol")
            {
                nodeText = TablePhrases.NumCol;
            }
            else if (valName == "TimeCol")
            {
                nodeText = TablePhrases.TimeCol;
            }
            else if (valName == "ObjCol")
            {
                nodeText = TablePhrases.ObjCol;
            }
            else if (valName == "DevCol")
            {
                nodeText = TablePhrases.DevCol;
            }
            else if (valName == "CnlCol")
            {
                nodeText = TablePhrases.CnlCol;
            }
            else if (valName == "TextCol")
            {
                nodeText = TablePhrases.TextCol;
            }
            else if (valName == "AckCol")
            {
                nodeText = TablePhrases.AckCol;
            }
            else if (dispEvent != null)
            {
                if (valName == "Num")
                {
                    nodeText = dispEvent.Num.ToString();
                    cell.SetNumberType();
                }
                else if (valName == "Time")
                    nodeText = dispEvent.Time;
                else if (valName == "Obj")
                    nodeText = dispEvent.Obj;
                else if (valName == "Dev")
                    nodeText = dispEvent.KP;
                else if (valName == "Cnl")
                    nodeText = dispEvent.Cnl;
                else if (valName == "Text")
                    nodeText = dispEvent.Text;
                else if (valName == "Ack")
                    nodeText = dispEvent.Ack;
            }

            if (nodeText != null)
                cell.DataNode.InnerText = nodeText;
        }

        /// <summary>
        /// Обработать директиву, связанную со строкой таблицы
        /// </summary>
        protected override void ProcRow(Cell cell, string rowName)
        {
            if (rowName == "Event")
                eventRowTemplate = cell.ParentRow;
        }
    }
}