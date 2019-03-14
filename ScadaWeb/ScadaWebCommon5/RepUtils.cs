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
 * Module   : ScadaWebCommon
 * Summary  : The class contains utility methods for generating reports
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using System;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// The class contains utility methods for generating reports.
    /// <para>Класс, содержащий вспомогательные методы для генерации отчётов.</para>
    /// </summary>
    public static class RepUtils
    {
        /// <summary>
        /// Макс. длина периода, который задаётся в днях
        /// </summary>
        public const int MaxDayPeriodLength = 32;
        /// <summary>
        /// Макс. длина периода, который задаётся в месяцах
        /// </summary>
        public const int MaxMonthPeriodLength = 12;


        /// <summary>
        /// Сформировать имя файла отчёта
        /// </summary>
        public static string BuildFileName(string prefix, string extension)
        {
            return prefix + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "." + extension;
        }

        /// <summary>
        /// Записать сообщение о генерации отчёта в журнал
        /// </summary>
        public static void WriteGenerationAction(Log log, string repName, UserData userData)
        {
            log.WriteAction(string.Format(WebPhrases.GenReport, repName, userData.UserProps.UserName));
        }
        
        
        /// <summary>
        /// Распознать даты, введённые пользователем, и вернуть сообщение в случае ошибки
        /// </summary>
        public static bool ParseDates(string dateFromStr, string dateToStr,
            out DateTime dateFrom, out DateTime dateTo, out string errMsg)
        {
            dateFrom = DateTime.MinValue;
            dateTo = DateTime.MinValue;

            if (!ScadaUtils.TryParseDateTime(dateFromStr, out dateFrom))
            {
                errMsg = WebPhrases.IncorrectStartDate;
                return false;
            }
            else if (!ScadaUtils.TryParseDateTime(dateToStr, out dateTo))
            {
                errMsg = WebPhrases.IncorrectEndDate;
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Проверить период, указанный в днях, рассчитать его длительность и вернуть сообщение в случае ошибки
        /// </summary>
        public static bool CheckDayPeriod(DateTime dateFrom, DateTime dateTo, out int period, out string errMsg)
        {
            period = (int)(dateTo - dateFrom).TotalDays + 1;

            if (dateFrom > dateTo)
            {
                errMsg = WebPhrases.IncorrectPeriod;
                return false;
            }
            else if (period > MaxDayPeriodLength)
            {
                errMsg = string.Format(WebPhrases.DayPeriodTooLong, MaxDayPeriodLength);
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Проверить период, указанный в днях, и вернуть сообщение в случае ошибки
        /// </summary>
        public static bool CheckDayPeriod(DateTime dateFrom, DateTime dateTo, out string errMsg)
        {
            int period;
            return CheckDayPeriod(dateFrom, dateTo, out period, out errMsg);
        }

        /// <summary>
        /// Проверить период, указанный в месяцах, и вернуть сообщение в случае ошибки
        /// </summary>
        public static bool CheckMonthPeriod(DateTime dateFrom, DateTime dateTo, out string errMsg)
        {
            int period = dateTo.Year * 12 + dateTo.Month - (dateFrom.Year * 12 + dateFrom.Month) + 1;

            if (dateFrom > dateTo)
            {
                errMsg = WebPhrases.IncorrectPeriod;
                return false;
            }
            else if (period > MaxMonthPeriodLength)
            {
                errMsg = string.Format(WebPhrases.MonthPeriodTooLong, MaxMonthPeriodLength);
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
        public static void NormalizeTimeRange(ref DateTime startDate, ref int period, bool periodInMonths = false)
        {
            startDate = startDate > DateTime.MinValue ? startDate.Date : DateTime.Today;

            if (periodInMonths)
            {
                if (period < 0)
                {
                    startDate = startDate.AddMonths(period).Date;
                    period = -period;
                }
            }
            else
            {
                // Примеры:
                // период равный -1, 0 или 1 - это одни сутки startDate,
                // период 2 - двое суток, начиная от startDate включительно,
                // период -2 - двое суток, заканчивая startDate включительно.
                if (period <= -2)
                {
                    startDate = startDate.AddDays(period + 1).Date;
                    period = -period;
                }
                else if (period < 1)
                {
                    period = 1;
                }
            }
        }

        /// <summary>
        /// Normalizes the time range.
        /// </summary>
        public static void NormalizeTimeRange(ref DateTime startDate, ref DateTime endDate, ref int period, 
            bool periodInMonths = false)
        {
            if (startDate > DateTime.MinValue && endDate > DateTime.MinValue)
            {
                if (endDate < startDate)
                    endDate = startDate;
                period = periodInMonths ?
                    ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month : 
                    (int)(endDate - startDate).TotalDays + 1;
            }
            else if (startDate > DateTime.MinValue)
            {
                NormalizeTimeRange(ref startDate, ref period, periodInMonths);
                endDate = periodInMonths ? 
                    startDate.AddMonths(period) : 
                    startDate.AddDays(period - 1);
            }
            else if (endDate > DateTime.MinValue)
            {
                period = Math.Abs(period);
                NormalizeTimeRange(ref endDate, ref period, periodInMonths);
                startDate = periodInMonths ?
                    endDate.AddMonths(-period) :
                    endDate.AddDays(-period + 1);
            }
            else
            {
                NormalizeTimeRange(ref startDate, ref period, periodInMonths);
                endDate = periodInMonths ?
                    startDate.AddMonths(period) :
                    startDate.AddDays(period - 1);
            }
        }
    }
}
