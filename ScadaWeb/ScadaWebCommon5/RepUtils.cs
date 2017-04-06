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
 * Module   : ScadaWebCommon
 * Summary  : The class contains utility methods for generating reports
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Web;
using Utils;
using Utils.Report;

namespace Scada.Web
{
    /// <summary>
    /// The class contains utility methods for generating reports
    /// <para>Класс, содержащий вспомогательные методы для генерации отчётов</para>
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
        /// Генерировать отчёт для загрузки через браузер
        /// </summary>
        [Obsolete("Move the method to RepBuilder class.")]
        public static void GenerateReport(RepBuilder repBuilder, object[] repParams, 
            string templateDir, string fileName, HttpResponse response)
        {
            if (repBuilder == null)
                throw new ArgumentNullException("repBuilder");
            if (response == null)
                throw new ArgumentNullException("response");

            try
            {
                response.ClearHeaders();
                response.ClearContent();

                response.ContentType = "application/octet-stream";
                if (!string.IsNullOrEmpty(fileName))
                    response.AppendHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");

                repBuilder.SetParams(repParams);
                repBuilder.Make(response.OutputStream, templateDir);
            }
            catch
            {
                response.ClearHeaders();
                response.ClearContent();
                response.ContentType = "text/html";
                throw;
            }
        }

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
        [Obsolete("Get rid of RepBuilder dependency here.")]
        public static void WriteGenerationAction(Log log, RepBuilder repBuilder, UserData userData)
        {
            log.WriteAction(string.Format(WebPhrases.GenReport, repBuilder.RepName, userData.UserProps.UserName));
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
