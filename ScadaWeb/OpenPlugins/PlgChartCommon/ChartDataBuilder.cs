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
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Globalization;
using System.Text;
using System.Web;

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
        /// Обеспечивает форматирование данных входных каналов
        /// </summary>
        protected readonly DataFormatter dataFormatter;
        /// <summary>
        /// Объект для доступа к данным
        /// </summary>
        protected readonly DataAccess dataAccess;

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
        /// <remarks>Если период отрицательный, то используется интервал времени влево от начальной даты</remarks>
        protected int period;
        /// <summary>
        /// Расстояние между разделяемыми точками графика, с
        /// </summary>
        protected int chartGap;

        /// <summary>
        /// Свойства каналов отображаемого графика
        /// </summary>
        protected InCnlProps[] cnlPropsArr;
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
        public ChartDataBuilder(int[] cnlNums, DateTime startDate, int period, int chartGap, DataAccess dataAccess)
        {
            dataFormatter = new DataFormatter();
            this.dataAccess = dataAccess;

            this.cnlNums = cnlNums;
            this.startDate = startDate;
            this.period = period;
            this.chartGap = chartGap;
            NormalizeTimeRange(ref this.startDate, ref this.period);

            cnlCnt = cnlNums.Length;
            cnlPropsArr = new InCnlProps[cnlCnt];
            cnlNames = new string[cnlCnt];
            quantityName = "";
            singleTrend = null;
            trendBundle = null;
        }


        /// <summary>
        /// Получить нормализованную начальную дату отображаемых данных
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
        }

        /// <summary>
        /// Получить нормализованный период отображаемых данных
        /// </summary>
        public int Period
        {
            get
            {
                return period;
            }
        }


        /// <summary>
        /// Получить имя величины с указанием размерности
        /// </summary>
        protected string GetQuantityName(string paramName, string singleUnit)
        {
            return !string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(singleUnit) ?
                paramName + ", " + singleUnit :
                paramName + singleUnit;
        }

        /// <summary>
        /// Заполнить свойства каналов и определить имя величины
        /// </summary>
        protected void FillCnlProps()
        {
            quantityName = "";
            bool quantityIsInited = false;
            bool quantitiesAreEqual = true;

            for (int i = 0; i < cnlCnt; i++)
            {
                InCnlProps cnlProps = dataAccess.GetCnlProps(cnlNums[i]);
                cnlPropsArr[i] = cnlProps;

                if (cnlProps == null)
                {
                    cnlNames[i] = "";
                }
                else
                {
                    cnlNames[i] = cnlProps.CnlName;

                    if (quantitiesAreEqual)
                    {
                        string qname = GetQuantityName(cnlProps.ParamName, cnlProps.SingleUnit);

                        if (!quantityIsInited)
                        {
                            quantityName = qname;
                            quantityIsInited = true;
                        }

                        if (quantityName != qname)
                        {
                            quantityName = "";
                            quantitiesAreEqual = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Заполнить данные одиночного тренда
        /// </summary>
        protected void FillSingleTrend()
        {
            singleTrend = cnlCnt > 0 ? dataAccess.DataCache.GetMinTrend(startDate, cnlNums[0]) : null;
            trendBundle = null;
        }

        /// <summary>
        /// Заполнить данные связки трендов
        /// </summary>
        protected void FillTrendBundle()
        {
            singleTrend = null;
            trendBundle = new TrendBundle();

            Trend[] trends = new Trend[cnlCnt];
            for (int i = 0; i < cnlCnt; i++)
                trends[i] = dataAccess.DataCache.GetMinTrend(startDate, cnlNums[i]);

            trendBundle.Init(trends);
        }

        /// <summary>
        /// Преобразовать точку тренда в запись JavaScript
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
        /// Получить точки одиночного тренда в виде JavaScript
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
        /// Получить точки связки трендов в виде JavaScript
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
        /// Получить метки времени одиночного тренда в виде JavaScript
        /// </summary>
        protected string GetTimePointsJs(Trend trend)
        {

            StringBuilder sbTimePoints = new StringBuilder("[");

            if (trend != null)
            {
                foreach (Trend.Point point in trend.Points)
                {
                    double time = point.DateTime.TimeOfDay.TotalDays;
                    sbTimePoints.Append(time.ToString(CultureInfo.InvariantCulture)).Append(", ");
                }
            }

            sbTimePoints.Append("]");
            return sbTimePoints.ToString();
        }

        /// <summary>
        /// Получить метки времени связки трендов в виде JavaScript
        /// </summary>
        protected string GetTimePointsJs(TrendBundle trendBundle)
        {
            StringBuilder sbTimePoints = new StringBuilder("[");

            if (trendBundle != null)
            {
                foreach (TrendBundle.Point point in trendBundle.Series)
                {
                    double time = point.DateTime.TimeOfDay.TotalDays;
                    sbTimePoints.Append(time.ToString(CultureInfo.InvariantCulture)).Append(", ");
                }
            }

            sbTimePoints.Append("]");
            return sbTimePoints.ToString();
        }


        /// <summary>
        /// Заполнить данные графика только за нормализованную начальную дату
        /// </summary>
        public void FillData()
        {
            FillCnlProps();

            if (cnlCnt <= 1)
                FillSingleTrend();
            else
                FillTrendBundle();
        }

        /// <summary>
        /// Преобразовать свойства графика в JavaScript
        /// </summary>
        public string ToJs()
        {
            StringBuilder sbJs = new StringBuilder();

            // настройки отображения
            sbJs
                .AppendLine("var displaySettings = new scada.chart.DisplaySettings();")
                .Append("displaySettings.locale = '").Append(Localization.Culture.Name).AppendLine("';")
                .Append("displaySettings.chartGap = ").Append(chartGap).AppendLine(" / scada.chart.const.SEC_PER_DAY;")
                .AppendLine();

            // интервал времени
            sbJs
                .AppendLine("var timeRange = new scada.chart.TimeRange();")
                .Append("timeRange.startDate = Date.UTC(")
                .AppendFormat("{0}, {1}, {2}", startDate.Year, startDate.Month - 1, startDate.Day).AppendLine(");")
                .AppendLine("timeRange.startTime = 0;")
                .Append("timeRange.endTime = ").Append(period).AppendLine(";")
                .AppendLine();

            // данные графика
            StringBuilder sbTrends = new StringBuilder("[");
            bool single = singleTrend != null;

            for (int i = 0; i < cnlCnt; i++)
            {
                string trendName = "trend" + i;
                sbTrends.Append(trendName).Append(", ");

                sbJs
                    .Append("var ").Append(trendName).AppendLine(" = new scada.chart.TrendExt();")
                    .Append(trendName).Append(".cnlNum = ").Append(cnlNums[i]).AppendLine(";")
                    .Append(trendName).Append(".cnlName = '")
                    .Append(HttpUtility.JavaScriptStringEncode(cnlNames[i])).AppendLine("';")
                    .Append(trendName).Append(".trendPoints = ")
                    .Append(single ? GetTrendPointsJs(singleTrend, cnlPropsArr[i]) : GetTrendPointsJs(trendBundle, i))
                    .AppendLine(";")
                    .AppendLine();
            }

            sbTrends.Append("];");

            sbJs
                .AppendLine("var chartData = new scada.chart.ChartData();")
                .Append("chartData.timePoints = ")
                .Append(single ? GetTimePointsJs(singleTrend) : GetTimePointsJs(trendBundle)).AppendLine(";")
                .Append("chartData.trends = ").Append(sbTrends).AppendLine()
                .Append("chartData.quantityName = '")
                .Append(HttpUtility.JavaScriptStringEncode(quantityName)).AppendLine("';");

            return sbJs.ToString();
        }

        /// <summary>
        /// Нормализовать интервал времени
        /// </summary>
        /// <remarks>Чтобы начальная дата являлась левой границей интервала времени и период был положительным</remarks>
        public static void NormalizeTimeRange(ref DateTime startDate, ref int period)
        {
            // Примеры:
            // период равный -1, 0 или 1 - это одни сутки startDate,
            // период 2 - двое суток, начиная от startDate включительно,
            // период -2 - двое суток, заканчивая startDate включительно
            if (period > -2)
            {
                startDate = startDate.Date;
                if (period < 1)
                    period = 1;
            }
            else
            {
                startDate = startDate.AddDays(period + 1).Date;
                period = -period;
            }
        }
    }
}
