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
 * Module   : PlgChart
 * Summary  : Minute data report builder
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using Utils.Report;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Minute data report builder
    /// <para>Формирует отчёт по минутным данным</para>
    /// </summary>
    public class MinDataRepBuilder : ExcelRepBuilder
    {
        private readonly DataAccess dataAccess; // объект для доступа к данным

        private int[] cnlNums;           // номера каналов, по которым строится отчёт
        private DateTime startDate;      // начальная дата данных отчёта
        private int period;              // период отображаемых данных
        private string viewTitle;        // заголовок представления

        private Row cnlRowTemplate;      // строка-шаблон списка входных каналов
        private Row dataHdrRow;          // строка шапки таблицы минутных данных
        private Row dateRowTemplate;     // строка-шаблон даты в таблице минутных данных
        private Row dataRowTemplate;     // строка-шаблон данных в таблице минутных данных
        private Cell valColCellTemplate; // ячейка-шаблон шапки столбца минутных данных
        private Cell valCellTemplate;    // ячейка-шаблон столбца минутных данных

        private bool procCnlRow;                   // производится обработка строки входного канала
        private InCnlProps[] cnlPropsArr;          // массив свойств входных каналов отчёта
        private InCnlProps curCnlProps;            // свойства текущего обрабатываемого входного канала

        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected MinDataRepBuilder()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MinDataRepBuilder(DataAccess dataAccess)
            : base()
        {
            if (dataAccess == null)
                throw new ArgumentNullException("dataAccess");

            this.dataAccess = dataAccess;

            ClearRepParams();
            ClearXmlObjects();
            ClearDataObjects();
        }


        /// <summary>
        /// Очистить параметры отчёта
        /// </summary>
        private void ClearRepParams()
        {
            startDate = DateTime.MinValue;
            period = 1;
            cnlNums = null;
            viewTitle = "";
        }

        /// <summary>
        /// Очистить XML-объекты строк и ячеек
        /// </summary>
        private void ClearXmlObjects()
        {
            cnlRowTemplate = null;
            dataHdrRow = null;
            dateRowTemplate = null;
            dataRowTemplate = null;
            valColCellTemplate = null;
            valCellTemplate = null;
        }

        /// <summary>
        /// Очистить объекты данных
        /// </summary>
        private void ClearDataObjects()
        {
            procCnlRow = false;
            cnlPropsArr = null;
            curCnlProps = null;
        }

        /// <summary>
        /// Создать и заполнить массив свойств входных каналов отчёта
        /// </summary>
        private void CreateCnlPropsArr()
        {
            int cnlCnt = cnlNums.Length;
            cnlPropsArr = new InCnlProps[cnlCnt];

            for (int i = 0; i < cnlCnt; i++)
            {
                int cnlNum = cnlNums[i];
                InCnlProps cnlProps = dataAccess.GetCnlProps(cnlNum);
                cnlPropsArr[i] = cnlProps == null ? new InCnlProps() { CnlNum = cnlNum } : cnlProps;
            }
        }

        /// <summary>
        /// Вывести в отчёт список входных каналов
        /// </summary>
        private void WriteCnlList(Table table, int cnlRowIndex)
        {
            try
            {
                procCnlRow = true;

                foreach (InCnlProps cnlProps in cnlPropsArr)
                {
                    curCnlProps = cnlProps;
                    Row cnlRow = cnlRowTemplate.Clone();
                    ExcelProc(cnlRow);
                    table.InsertRow(cnlRowIndex, cnlRow);
                    cnlRowIndex++;
                }
            }
            finally
            {
                procCnlRow = false;
            }
        }

        /// <summary>
        /// Добавить столбцы в таблицу минутных данных
        /// </summary>
        private void AddMinTableColumns(Table table)
        {
            Column colTemplate = table.Columns[1];
            int colIndex = colTemplate.Index + 1;

            for (int i = 1, cnlCnt = cnlNums.Length; i < cnlCnt; i++)
            {
                Column col = colTemplate.Clone();
                table.InsertColumn(colIndex, col);
                colIndex++;

                Cell valColCell = valColCellTemplate.Clone();
                valColCell.DataNode.InnerText = string.Format(MinDataRepPhrases.ValColTitle, cnlNums[i]);
                dataHdrRow.AppendCell(valColCell);

                Cell emptyCell = valCellTemplate.Clone();
                dateRowTemplate.AppendCell(emptyCell);

                Cell valCell = valCellTemplate.Clone();
                dataRowTemplate.AppendCell(valCell);
            }
        }

        /// <summary>
        /// Вывести в отчёт минутные данные
        /// </summary>
        private void WriteMinData(Table table)
        {
            // получение трендов
            int cnlCnt = cnlNums.Length;
            Trend[] trends = new Trend[cnlCnt];
            Trend trend;

            for (int i = 0; i < cnlCnt; i++)
            {
                int cnlNum = cnlNums[i];

                if (period == 1)
                {
                    trend = dataAccess.DataCache.GetMinTrend(startDate, cnlNum);
                }
                else
                {
                    trend = new Trend(cnlNum);
                    for (int d = 0; d < period; d++)
                    {
                        Trend dailyTrend = dataAccess.DataCache.GetMinTrend(startDate.AddDays(d), cnlNum);
                        trend.Points.AddRange(dailyTrend.Points);
                    }
                }

                trends[i] = trend;
            }

            // создание связки трендов
            TrendBundle trendBundle = new TrendBundle();
            trendBundle.Init(trends);

            // вывод в отчёт
            DataFormatter dataFormatter = new DataFormatter();
            DateTime prevDate = period > 1 ? DateTime.MinValue : DateTime.MaxValue /*не выводить даты*/;

            foreach (TrendBundle.Point point in trendBundle.Series)
            {
                DateTime pointDT = point.DateTime;

                // вывод строки с новой датой
                if (prevDate < pointDT.Date)
                {
                    prevDate = pointDT.Date;
                    Row dateRow = dateRowTemplate.Clone();
                    dateRow.Cells[0].DataNode.InnerText = pointDT.ToLocalizedDateString();
                    table.AppendRow(dateRow);
                }

                // вывод строки с минутными данными
                Row dataRow = dataRowTemplate.Clone();
                dataRow.Cells[0].DataNode.InnerText = pointDT.ToLocalizedTimeString();

                for (int i = 0; i < cnlCnt; i++)
                {
                    SrezTableLight.CnlData cnlData = point.CnlData[i];

                    string text;
                    string textWithUnit;
                    bool textIsNumber;
                    dataFormatter.FormatCnlVal(cnlData.Val, cnlData.Stat, cnlPropsArr[i], ".", "", 
                        out text, out textWithUnit, out textIsNumber);

                    Cell cell = dataRow.Cells[i + 1];
                    cell.DataNode.InnerText = text;
                    if (textIsNumber)
                        cell.SetNumberType();
                }

                table.AppendRow(dataRow);
            }
        }


        /// <summary>
        /// Получить имя отчёта
        /// </summary>
        public override string RepName
        {
            get
            {
                return Localization.UseRussian ?
                    "Минутные данные" :
                    "Minute data";
            }
        }

        /// <summary>
        /// Получить имя файла шаблона
        /// </summary>
        public override string TemplateFileName
        {
            get
            {
                return "MinDataRep.xml";
            }
        }


        /// <summary>
        /// Установить параметры отчёта.
        /// repParams[0] - номера каналов, по которым строится отчёт, int[],
        /// repParams[1] - начальная дата данных отчёта, DateTime,
        /// repParams[2] - период отображаемых данных, int,
        /// repParams[3] - заголовок представления, string
        /// </summary>
        public override void SetParams(params object[] repParams)
        {
            cnlNums = (int[])repParams[0];
            startDate = (DateTime)repParams[1];
            period = (int)repParams[2];
            viewTitle = (string)repParams[3];

            RepUtils.NormalizeTimeRange(ref startDate, ref period);
        }

        /// <summary>
        /// Предварительно обработать дерево XML-документа
        /// </summary>
        protected override void StartXmlDocProc()
        {
            ClearXmlObjects();
            ClearDataObjects();
        }

        /// <summary>
        /// Окончательно обработать дерево XML-документа
        /// </summary>
        protected override void FinalXmlDocProc()
        {
            // проверка шаблона
            Table table = cnlRowTemplate == null ? null : cnlRowTemplate.ParentTable;
            if (workbook.Worksheets.Count == 0 ||
                cnlRowTemplate == null ||
                dataHdrRow == null || dataHdrRow.Cells.Count < 2 ||
                dataRowTemplate == null || dataRowTemplate.Cells.Count < 2 ||
                table.Columns.Count < 2)
            {
                throw new Exception(WebPhrases.IncorrectRepTemplate);
            }

            // перевод наименования листа
            workbook.Worksheets[0].Name = MinDataRepPhrases.MinDataWorksheet;

            // удаление лишних атрибутов таблицы
            table.RemoveTableNodeAttrs();

            // удаление строк-шаблонов из таблицы
            int cnlRowIndex = table.Rows.IndexOf(cnlRowTemplate);
            table.RemoveRow(cnlRowIndex);
            int dateRowIndex = table.Rows.IndexOf(dateRowTemplate);
            table.RemoveRow(dateRowIndex);
            int dataRowIndex = table.Rows.IndexOf(dataRowTemplate);
            table.RemoveRow(dataRowIndex);

            // формирование отчёта
            CreateCnlPropsArr();
            WriteCnlList(table, cnlRowIndex);
            AddMinTableColumns(table);
            WriteMinData(table);
            workbook.Worksheets[0].SplitHorizontal(dataRowIndex + cnlNums.Length);
        }

        /// <summary>
        /// Обработать директиву, связанную со значением ячейки
        /// </summary>
        protected override void ProcVal(Cell cell, string valName)
        {
            string nodeText = null;

            if (valName == "Title")
            {
                string periodStr = startDate.ToLocalizedDateString() +
                    (period > 1 ? " - " + startDate.AddDays(period - 1).ToLocalizedDateString() : "");
                nodeText = string.IsNullOrEmpty(viewTitle) ? 
                    periodStr : viewTitle + ", " + periodStr;
            }
            else if (valName == "Gen")
            {
                nodeText = MinDataRepPhrases.MinDataGen + DateTime.Now.ToLocalizedString();
            }
            else if (valName == "CnlsCaption")
            {
                nodeText = MinDataRepPhrases.CnlsCaption;
            }
            else if (procCnlRow)
            {
                if (valName == "CnlNum")
                {
                    nodeText = curCnlProps.CnlNum.ToString();
                    cell.SetNumberType();
                }
                else if (valName == "CnlName")
                {
                    nodeText = curCnlProps.CnlName +
                        (curCnlProps.ShowNumber && curCnlProps.UnitArr != null && curCnlProps.UnitArr.Length == 1 ?
                            ", " + curCnlProps.UnitArr[0] : "");
                }
            }
            else if (valName == "TimeCol")
            {
                nodeText = MinDataRepPhrases.TimeColTitle;
                dataHdrRow = cell.ParentRow;
            }
            else if (valName == "ValCol")
            {
                valColCellTemplate = cell;
                nodeText = string.Format(MinDataRepPhrases.ValColTitle, cnlNums[0]);
            }
            else if (valName == "Date")
            {
                dateRowTemplate = cell.ParentRow;
            }
            else if (valName == "Time")
            {
                dataRowTemplate = cell.ParentRow;
            }
            else if (valName == "Val")
            {
                valCellTemplate = cell;
                nodeText = "";
            }

            if (nodeText != null)
                cell.DataNode.InnerText = nodeText;
        }

        /// <summary>
        /// Обработать директиву, связанную со строкой таблицы
        /// </summary>
        protected override void ProcRow(Cell cell, string rowName)
        {
            if (rowName == "CnlRow")
                cnlRowTemplate = cell.ParentRow;
        }
    }
}