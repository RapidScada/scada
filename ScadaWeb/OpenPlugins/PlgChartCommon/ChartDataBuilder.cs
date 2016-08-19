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
 * Module   : PlgChartCommon
 * Summary  : Builds JavaScript of chart properties and data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data.Tables;
using System;
using System.Text;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Builds JavaScript of chart properties and data
    /// <para>Строит JavaScript свойств и данных графика</para>
    /// </summary>
    public class ChartDataBuilder
    {
        private int cnlCnt; // количество каналов


        /// <summary>
        /// Номера каналов отображаемого графика
        /// </summary>
        protected int[] cnlNums;
        /// <summary>
        /// Начальная дата отображаемых данных
        /// </summary>
        protected DateTime startDate;
        /// <summary>
        /// Период отображаемых данных, дн.
        /// </summary>
        /// <remarks>Если период отрицательный, то используется отрезок времени влево от начальной даты</remarks>
        protected int period;
        /// <summary>
        /// Расстояние между разделяемыми точками графика, с
        /// </summary>
        protected int chartGap;

        /// <summary>
        /// Имена каналов отображаемого графика
        /// </summary>
        protected string[] cnlNames;
        /// <summary>
        /// Имя величины с указанием размерности, общее для всех каналов
        /// </summary>
        protected string quantityName;
        /// <summary>
        /// Одиночный тренд
        /// </summary>
        /// <remarks>Если количество каналов равно 1</remarks>
        protected Trend singleTrend;
        /// <summary>
        /// Связка трендов
        /// </summary>
        /// <remarks>Если количество каналов больше 1</remarks>
        protected TrendBundle trendBundle;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected ChartDataBuilder()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChartDataBuilder(int[] cnlNums, DateTime startDate, int period, int chartGap)
        {
            this.cnlNums = cnlNums;
            this.startDate = startDate;
            this.period = period;
            this.chartGap = chartGap;

            cnlCnt = cnlNums.Length;
            cnlNames = new string[cnlCnt];
            quantityName = "";
            singleTrend = null;
            trendBundle = null;
        }


        /// <summary>
        /// Заполнить данные графика
        /// </summary>
        public void FillData(DataAccess dataAccess)
        {

        }

        /// <summary>
        /// Преобразовать свойства графика в JavaScript
        /// </summary>
        public string ToJs()
        {
            StringBuilder sbJs = new StringBuilder();

            sbJs
                .AppendLine("var displaySettings = new scada.chart.DisplaySettings();")
                .Append("displaySettings.locale = '").Append(Localization.Culture.Name).AppendLine("';")
                .Append("displaySettings.chartGap = ").Append(chartGap).AppendLine(" / scada.chart.const.SEC_PER_DAY;")
                .AppendLine()
                .AppendLine("var timeRange = new scada.chart.TimeRange();")
                .Append("timeRange.startDate = Date.UTC(")
                .AppendFormat("{0}, {1}, {2}", startDate.Year, startDate.Month - 1, startDate.Day).AppendLine(");")
                .AppendLine("timeRange.startTime = 0;")
                .AppendLine("timeRange.endTime = 1;")
                .AppendLine();

            return sbJs.ToString();
        }
    }
}
