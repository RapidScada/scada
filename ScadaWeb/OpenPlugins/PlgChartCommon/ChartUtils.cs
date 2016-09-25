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
using System.Text;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// The class contains utility methods for web charts
    /// <para>Класс, содержащий вспомогательные методы для графиков</para>
    /// </summary>
    public static class ChartUtils
    {
        /// <summary>
        /// Макс. длина периода, дн.
        /// </summary>
        public const int MaxPeriodLength = 31;

        /// <summary>
        /// Количество графиков, выше которого могут быть проблемы с производительностью
        /// </summary>
        public const int NormalChartCnt = 10;


        /// <summary>
        /// Распознать даты, введённые пользователем, и вернуть сообщение в случае ошибки
        /// </summary>
        public static bool ParseDates(TextBox txtDateFrom, TextBox txtDateTo, 
            out DateTime dateFrom, out DateTime dateTo, out string errMsg)
        {
            dateFrom = DateTime.MinValue;
            dateTo = DateTime.MinValue;

            if (!ScadaUtils.TryParseDateTime(txtDateFrom.Text, out dateFrom))
            {
                errMsg = ChartPhrases.IncorrectStartDate;
                return false;
            }
            else if (!ScadaUtils.TryParseDateTime(txtDateTo.Text, out dateTo))
            {
                errMsg = ChartPhrases.IncorrectEndDate;
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Проверить даты, введённые пользователем, и вернуть сообщение в случае ошибки
        /// </summary>
        public static bool CheckDates(DateTime dateFrom, DateTime dateTo, out int period, out string errMsg)
        {
            period = (int)(dateTo - dateFrom).TotalDays + 1;

            if (dateFrom > dateTo)
            {
                errMsg = ChartPhrases.IncorrectPeriod;
                return false;
            }
            else if (period > MaxPeriodLength)
            {
                errMsg = string.Format(ChartPhrases.PeriodTooLong, MaxPeriodLength);
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
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
                throw new ArgumentException(ChartPhrases.CnlNumsEmptyError);

            if (cnlNums.Length != viewIDs.Length)
                throw new ScadaException(ChartPhrases.CountMismatchError);
        }

        /// <summary>
        /// Получить выбранные каналы и соответствующие им представления
        /// </summary>
        public static void GetSelection(List<CnlViewPair> selectedCnls, out string cnlNums, out string viewIDs)
        {
            StringBuilder sbCnlNums = new StringBuilder();
            StringBuilder sbViewIDs = new StringBuilder();

            for (int i = 0, lastInd = selectedCnls.Count - 1; i <= lastInd; i++)
            {
                CnlViewPair pair = selectedCnls[i];
                sbCnlNums.Append(pair.CnlNum);
                sbViewIDs.Append(pair.ViewID);

                if (i < lastInd)
                {
                    sbCnlNums.Append(",");
                    sbViewIDs.Append(",");
                }
            }

            cnlNums = sbCnlNums.ToString();
            viewIDs = sbViewIDs.ToString();
        }
    }
}
