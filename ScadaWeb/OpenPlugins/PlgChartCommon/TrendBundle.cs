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
 * Summary  : Trend bundle that has a single timeline
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Data.Tables;
using System;
using System.Collections.Generic;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Trend bundle that has a single timeline
    /// <para>Связка трендов, имеющая единую шкалу времени</para>
    /// </summary>
    public class TrendBundle
    {
        /// <summary>
        /// Точка по всем трендам с отметкой времени
        /// </summary>
        public class Point
        {
            /// <summary>
            /// Конструктор, ограничивающий создание объекта без параметров
            /// </summary>
            protected Point()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Point(DateTime dateTime, int cnlCnt)
            {
                DateTime = dateTime;
                CnlData = new SrezTableLight.CnlData[cnlCnt];
            }

            /// <summary>
            /// Получить метку времени
            /// </summary>
            public DateTime DateTime { get; private set; }
            /// <summary>
            /// Получить данные
            /// </summary>
            public SrezTableLight.CnlData[] CnlData { get; private set; }
        }


        /// <summary>
        /// Разность двух меток времени, менее которой они считаются равными, мс
        /// </summary>
        protected const int TimeDifference = 1000;


        /// <summary>
        /// Конструктор
        /// </summary>
        public TrendBundle()
        {
            Series = null;
        }


        /// <summary>
        /// Получить временной ряд
        /// </summary>
        public List<Point> Series { get; protected set; }

        /// <summary>
        /// Инициализировать связку трендов
        /// </summary>
        public void Init(Trend[] trends)
        {
            Init(trends, DateTime.MinValue);
        }

        /// <summary>
        /// Инициализировать связку трендов, включив в неё данные с указанного начального времени
        /// </summary>
        public void Init(Trend[] trends, DateTime startDT)
        {
            // формирование данных графиков
            Series = new List<Point>();

            int cnlCnt = trends.Length;
            int[] trendPosArr = new int[cnlCnt]; // позиции получения данных из трендов
            for (int i = 0; i < cnlCnt; i++)
                trendPosArr[i] = 0;

            while (true)
            {
                // определение минимального времени среди обрабатываемых точек трендов
                DateTime minDateTime = DateTime.MaxValue;
                bool complete = true;

                for (int i = 0; i < cnlCnt; i++)
                {
                    List<Trend.Point> trendPoints = trends[i].Points;
                    int trendPos = trendPosArr[i];
                    int pointCnt = trendPoints.Count;
                    bool pointCompared = false;

                    while (trendPos < pointCnt && !pointCompared)
                    {
                        DateTime pointDT = trendPoints[trendPos].DateTime;

                        if (pointDT < startDT)
                        {
                            trendPos++;
                            trendPosArr[i]++;
                        }
                        else 
                        {
                            pointCompared = true;
                            if (minDateTime > pointDT)
                                minDateTime = pointDT;
                        }
                    }

                    complete = complete && trendPos >= pointCnt;
                }

                // выход из цикла, если обработка данных завершена
                if (complete)
                    break;

                // копирование данных из трендов на момент времени minDateTime
                Point bundlePoint = new Point(minDateTime, cnlCnt);

                for (int i = 0; i < cnlCnt; i++)
                {
                    SrezTableLight.CnlData cnlData = SrezTableLight.CnlData.Empty;
                    List<Trend.Point> trendPoints = trends[i].Points;
                    int trendPos = trendPosArr[i];

                    if (trendPos < trendPoints.Count)
                    {
                        Trend.Point trendPoint = trendPoints[trendPos];
                        if ((trendPoint.DateTime - minDateTime).TotalMilliseconds < TimeDifference)
                        {
                            cnlData = new SrezTableLight.CnlData(trendPoint.Val, trendPoint.Stat);
                            trendPosArr[i]++;
                        }
                    }

                    bundlePoint.CnlData[i] = cnlData;
                }

                Series.Add(bundlePoint);
            }
        }
    }
}