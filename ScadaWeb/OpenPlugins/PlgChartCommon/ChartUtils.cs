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
 * Summary  : The class contains utility methods for charts
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// The class contains utility methods for web charts
    /// <para>Класс, содержащий вспомогательные методы для графиков</para>
    /// </summary>
    public static class ChartUtils
    {
        /// <summary>
        /// Проверить корректность заданных массивов
        /// </summary>
        public static void CheckArrays(int[] cnlNums, int[] viewIDs)
        {
            if (cnlNums == null)
                throw new ArgumentNullException("cnlNums");

            if (viewIDs == null)
                throw new ArgumentNullException("viewIDs");

            if (cnlNums.Length == 0)
                throw new ArgumentException(Localization.UseRussian ?
                    "Номера каналов не заданы." :
                    "Channel numbers are not specified.");

            if (cnlNums.Length != viewIDs.Length)
                throw new ScadaException(Localization.UseRussian ?
                    "Несоответствие количества каналов и ид. представлений." :
                    "Mismatch in number of channels and view IDs.");
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
