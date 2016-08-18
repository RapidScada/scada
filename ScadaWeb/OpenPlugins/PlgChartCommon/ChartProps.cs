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
 * Summary  : Chart properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using System;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Chart properties
    /// <para>Свойства графика</para>
    /// </summary>
    public class ChartProps
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected ChartProps()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChartProps(int[] cnlNums, DateTime startDate, int period, int chartGap)
        {
            ChartGap = chartGap;
        }


        /// <summary>
        /// Получить расстояние между разделяемыми точками графика, с
        /// </summary>
        public int ChartGap { get; protected set; }

        /// <summary>
        /// Получить имя величины с указанием размерности, общее для всех каналов
        /// </summary>
        public string QuantityName { get; protected set; }


        protected DateTime startDate;  // дата отображаемых данных
        protected int cnlNum;          // номер канала отображаемого графика
        protected string cnlName;      // имя канала

        protected string timePoints;   // массив меток времени тренда
        protected string trendPoints;  // массив значений тренда


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
            return "";
        }
    }
}
