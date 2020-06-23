/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Modified : 2020
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Builds JavaScript of chart properties and data.
    /// <para>Строит JavaScript свойств и данных графика.</para>
    /// </summary>
    public class ChartDataBuilder
    {
        private readonly int cnlCnt; // количество каналов


        /// <summary>
        /// Обеспечивает форматирование данных входных каналов.
        /// </summary>
        protected readonly DataFormatter dataFormatter;
        /// <summary>
        /// Объект для доступа к данным.
        /// </summary>
        protected readonly DataAccess dataAccess;

        /// <summary>
        /// Номера каналов отображаемого графика.
        /// </summary>
        protected readonly int[] cnlNums;
        /// <summary>
        /// Начальная дата отображаемых данных.
        /// </summary>
        protected readonly DateTime startDate;
        /// <summary>
        /// Период отображаемых данных, дн.
        /// </summary>
        /// <remarks>Если период отрицательный, то используется интервал времени влево от начальной даты.</remarks>
        protected readonly int period;

        /// <summary>
        /// Свойства каналов отображаемого графика.
        /// </summary>
        protected InCnlProps[] cnlPropsArr;
        /// <summary>
        /// Одиночный тренд
        /// </summary>
        /// <remarks>Если количество каналов равно 1.</remarks>
        protected Trend singleTrend;
        /// <summary>
        /// Связка трендов
        /// </summary>
        /// <remarks>Если количество каналов больше 1.</remarks>
        protected TrendBundle trendBundle;


        /// <summary>
        /// Конструктор
        /// </summary>
        public ChartDataBuilder(int[] cnlNums, DateTime startDate, int period, DataAccess dataAccess)
        {
            dataFormatter = new DataFormatter();
            this.dataAccess = dataAccess ?? throw new ArgumentNullException("dataAccess");
            this.cnlNums = cnlNums ?? throw new ArgumentNullException("cnlNums");
            this.startDate = startDate;
            this.period = period;
            RepUtils.NormalizeTimeRange(ref this.startDate, ref this.period);

            cnlCnt = cnlNums.Length;
            cnlPropsArr = new InCnlProps[cnlCnt];
            singleTrend = null;
            trendBundle = null;
        }


        /// <summary>
        /// Получить нормализованную начальную дату отображаемых данных.
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
        }

        /// <summary>
        /// Получить нормализованную конечную дату отображаемых данных.
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return StartDate.AddDays(Period - 1);
            }
        }

        /// <summary>
        /// Получить нормализованный период отображаемых данных.
        /// </summary>
        public int Period
        {
            get
            {
                return period;
            }
        }


        /// <summary>
        /// Получить имя величины с указанием размерности.
        /// </summary>
        protected string GetQuantityName(InCnlProps cnlProps)
        {
            string quantityName = cnlProps.ParamName;
            string singleUnit = cnlProps.SingleUnit;

            return string.IsNullOrEmpty(quantityName) || string.IsNullOrEmpty(singleUnit) ?
                quantityName + singleUnit :
                quantityName + ", " + singleUnit;
        }

        /// <summary>
        /// Заполнить данные одиночного тренда.
        /// </summary>
        protected void FillSingleTrend()
        {
            singleTrend = cnlCnt > 0 ? dataAccess.DataCache.GetMinTrend(startDate, cnlNums[0]) : null;
            trendBundle = null;
        }

        /// <summary>
        /// Заполнить данные связки трендов.
        /// </summary>
        protected void FillTrendBundle()
        {
            singleTrend = null;
            trendBundle = new TrendBundle();

            Trend[] trends = new Trend[cnlCnt];
            for (int i = 0; i < cnlCnt; i++)
            {
                trends[i] = dataAccess.DataCache.GetMinTrend(startDate, cnlNums[i]);
            }

            trendBundle.Init(trends);
        }

        /// <summary>
        /// Преобразовать точку тренда в запись JavaScript.
        /// </summary>
        protected string TrendPointToJs(double val, int stat, InCnlProps cnlProps)
        {
            string text;
            string textWithUnit;
            dataFormatter.FormatCnlVal(val, stat, cnlProps, out text, out textWithUnit);
            CnlStatProps cnlStatProps = dataAccess.GetCnlStatProps(stat);
            string color = dataFormatter.GetCnlValColor(val, stat, cnlProps, cnlStatProps);

            // для text и textWithUnit было бы корректно использовать метод HttpUtility.JavaScriptStringEncode(),
            // но он опускается для повышения скорости
            double chartVal = stat > 0 ? val : double.NaN;
            return (new StringBuilder("[")
                .Append(double.IsNaN(chartVal) ? "NaN" : chartVal.ToString(CultureInfo.InvariantCulture))
                .Append(", \"")
                .Append(text)
                .Append("\", \"")
                .Append(textWithUnit)
                .Append("\", \"")
                .Append(color)
                .Append("\"]")).ToString();
        }

        /// <summary>
        /// Получить точки одиночного тренда в виде JavaScript.
        /// </summary>
        protected string GetTrendPointsJs(Trend trend, InCnlProps cnlProps)
        {
            StringBuilder sbTrendPoints = new StringBuilder("[");

            if (trend != null)
            {
                foreach (Trend.Point point in trend.Points)
                {
                    sbTrendPoints
                        .Append(TrendPointToJs(point.Val, point.Stat, cnlProps))
                        .Append(", ");
                }
            }

            sbTrendPoints.Append("]");
            return sbTrendPoints.ToString();
        }

        /// <summary>
        /// Получить точки связки трендов в виде JavaScript.
        /// </summary>
        protected string GetTrendPointsJs(TrendBundle trendBundle, int trendInd)
        {
            StringBuilder sbTrendPoints = new StringBuilder("[");
            InCnlProps cnlProps = cnlPropsArr[trendInd];

            if (trendBundle != null)
            {
                foreach (TrendBundle.Point point in trendBundle.Series)
                {
                    SrezTableLight.CnlData cnlData = point.CnlData[trendInd];
                    sbTrendPoints
                        .Append(TrendPointToJs(cnlData.Val, cnlData.Stat, cnlProps))
                        .Append(", ");
                }
            }

            sbTrendPoints.Append("]");
            return sbTrendPoints.ToString();
        }

        /// <summary>
        /// Получить метки времени одиночного тренда в виде JavaScript.
        /// </summary>
        protected string GetTimePointsJs(Trend trend)
        {

            StringBuilder sbTimePoints = new StringBuilder("[");

            if (trend != null)
            {
                foreach (Trend.Point point in trend.Points)
                {
                    sbTimePoints.Append(ScadaUtils.EncodeDateTime(point.DateTime)
                        .ToString(CultureInfo.InvariantCulture)).Append(", ");
                }
            }

            sbTimePoints.Append("]");
            return sbTimePoints.ToString();
        }

        /// <summary>
        /// Получить метки времени связки трендов в виде JavaScript.
        /// </summary>
        protected string GetTimePointsJs(TrendBundle trendBundle)
        {
            StringBuilder sbTimePoints = new StringBuilder("[");

            if (trendBundle != null)
            {
                foreach (TrendBundle.Point point in trendBundle.Series)
                {
                    double time = point.DateTime.TimeOfDay.TotalDays;
                    sbTimePoints.Append(ScadaUtils.EncodeDateTime(point.DateTime)
                        .ToString(CultureInfo.InvariantCulture)).Append(", ");
                }
            }

            sbTimePoints.Append("]");
            return sbTimePoints.ToString();
        }


        /// <summary>
        /// Fills the channel properies.
        /// </summary>
        public void FillCnlProps()
        {
            for (int i = 0; i < cnlCnt; i++)
            {
                InCnlProps cnlProps = dataAccess.GetCnlProps(cnlNums[i]);
                cnlPropsArr[i] = cnlProps;
            }
        }

        /// <summary>
        /// Fills chart data for the normalized start date.
        /// </summary>
        public void FillData()
        {
            if (cnlCnt <= 1)
                FillSingleTrend();
            else
                FillTrendBundle();
        }

        /// <summary>
        /// Converts the chart data to JavaScript.
        /// </summary>
        public void ToJs(StringBuilder stringBuilder)
        {
            // интервал времени
            int startDateEnc = (int)ScadaUtils.EncodeDateTime(startDate);
            stringBuilder
                .AppendLine("var timeRange = new scada.chart.TimeRange();")
                .AppendFormat("timeRange.startTime = {0};", startDateEnc).AppendLine()
                .AppendFormat("timeRange.endTime = {0};", startDateEnc + period).AppendLine()
                .AppendLine();

            // данные графика
            StringBuilder sbTrends = new StringBuilder("[");
            bool isSingle = singleTrend != null;

            for (int i = 0; i < cnlCnt; i++)
            {
                string trendName = "trend" + i;
                InCnlProps cnlProps = cnlPropsArr[i] ?? new InCnlProps() { CnlNum = cnlNums[i] };
                sbTrends.Append(trendName).Append(", ");

                stringBuilder
                    .Append("var ").Append(trendName).AppendLine(" = new scada.chart.Trend();")
                    .Append(trendName).AppendFormat(".cnlNum = {0};", cnlProps.CnlNum).AppendLine()
                    .Append(trendName).AppendFormat(".cnlName = '{0}';", 
                        HttpUtility.JavaScriptStringEncode(cnlProps.CnlName)).AppendLine()
                    .Append(trendName).AppendFormat(".quantityID = {0};", cnlProps.ParamID).AppendLine()
                    .Append(trendName).AppendFormat(".quantityName = '{0}';", 
                        HttpUtility.JavaScriptStringEncode(GetQuantityName(cnlProps))).AppendLine()
                    .Append(trendName).AppendFormat(".trendPoints = {0};", 
                        isSingle ? GetTrendPointsJs(singleTrend, cnlPropsArr[i]) : GetTrendPointsJs(trendBundle, i))
                    .AppendLine()
                    .AppendLine();
            }

            sbTrends.Append("];");

            stringBuilder
                .AppendLine("var chartData = new scada.chart.ChartData();")
                .Append("chartData.timePoints = ")
                .Append(isSingle ? GetTimePointsJs(singleTrend) : GetTimePointsJs(trendBundle)).AppendLine(";")
                .Append("chartData.trends = ").Append(sbTrends).AppendLine()
                .AppendLine();
        }
    }
}
