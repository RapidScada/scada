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
 * Module   : Report Builder
 * Summary  : The class contains utility methods for reports
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;

namespace Utils.Report
{
    /// <summary>
    /// The class contains utility methods for reports.
    /// <para>Класс, содержащий вспомогательные методы для отчётов.</para>
    /// </summary>
    public static class RepUtils
    {
        /// <summary>
        /// Gets the actual report start date.
        /// </summary>
        public static DateTime GetStartDate(RepStartDate repStartDate, DateTime currentDate)
        {
            switch (repStartDate)
            {
                case RepStartDate.Yesterday:
                    return currentDate.AddDays(-1.0).Date;
                default: // RepStartDate.Today:
                    return currentDate.Date;
            }
        }

        /// <summary>
        /// Normalizes the time range.
        /// </summary>
        /// <remarks>
        /// Makes the startDate a left point of the time range, and makes the period positive.
        /// </remarks>
        public static void NormalizeTimeRange(ref DateTime startDate, ref int period, 
            RepPeriodUnit unit = RepPeriodUnit.Day)
        {
            startDate = startDate > DateTime.MinValue ? startDate.Date : DateTime.Today;

            if (unit == RepPeriodUnit.Month)
            {
                if (period < 0)
                {
                    startDate = startDate.AddMonths(period).Date;
                    period = -period;
                }
            }
            else
            {
                // Examples:
                // If the period is -1, 0 or 1, it means the single day, the startDate.
                // If the period is 2, it means 2 days starting from the startDate.
                // If the period is -2, it means 2 days ending with the startDate and including it.
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
            RepPeriodUnit unit = RepPeriodUnit.Day)
        {
            bool periodInMonths = unit == RepPeriodUnit.Month;

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
                NormalizeTimeRange(ref startDate, ref period, unit);
                endDate = periodInMonths ?
                    startDate.AddMonths(period) :
                    startDate.AddDays(period - 1);
            }
            else if (endDate > DateTime.MinValue)
            {
                period = Math.Abs(period);
                NormalizeTimeRange(ref endDate, ref period, unit);
                startDate = periodInMonths ?
                    endDate.AddMonths(-period) :
                    endDate.AddDays(-period + 1);
            }
            else
            {
                NormalizeTimeRange(ref startDate, ref period, unit);
                endDate = periodInMonths ?
                    startDate.AddMonths(period) :
                    startDate.AddDays(period - 1);
            }
        }
    }
}
