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
 * Summary  : The class contains utility methods for web applications. Query string utilities
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Scada.Web
{
    partial class WebUtils
    {
        /// <summary>
        /// Получить значение параметра из строки запроса как целое число
        /// </summary>
        public static int GetParamAsInt(this NameValueCollection queryString, string paramName)
        {
            int paramVal;
            return int.TryParse(queryString[paramName], out paramVal) ? paramVal : 0;
        }

        /// <summary>
        /// Получить значение параметра из строки запроса как дату
        /// </summary>
        public static DateTime GetParamAsDate(this NameValueCollection queryString,
            string yearParamName = "year", string monthParamName = "month", string dayParamName = "day")
        {
            int year, month, day;
            int.TryParse(queryString[yearParamName], out year);
            int.TryParse(queryString[monthParamName], out month);
            int.TryParse(queryString[dayParamName], out day);

            if (year == 0 && month == 0 && day == 0)
            {
                return DateTime.Today;
            }
            else
            {
                try { return new DateTime(year, month, day); }
                catch { throw new ScadaException(WebPhrases.IncorrectDate); }
            }
        }

        /// <summary>
        /// Получить значение параметра из строки запроса как дату
        /// </summary>
        public static DateTime GetParamAsDate(this NameValueCollection queryString, string paramName)
        {
            string dateStr = queryString[paramName];
            DateTime dateTime;

            if (string.IsNullOrEmpty(dateStr))
                return DateTime.Today;
            else if (DateTime.TryParse(dateStr, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dateTime))
                return dateTime.Date;
            else
                throw new ScadaException(WebPhrases.IncorrectDate);
        }

        /// <summary>
        /// Получить значение параметра из строки запроса как массив целых чисел
        /// </summary>
        public static int[] GetParamAsIntArray(this NameValueCollection queryString, string paramName)
        {
            return QueryParamToIntArray(queryString[paramName]);
        }

        /// <summary>
        /// Получить значение параметра из строки запроса как множество целых чисел
        /// </summary>
        public static HashSet<int> GetParamAsIntSet(this NameValueCollection queryString, string paramName)
        {
            return QueryParamToIntSet(queryString[paramName]);
        }

        /// <summary>
        /// Преобразовать значение параметра запроса в массив целых чисел
        /// </summary>
        public static int[] QueryParamToIntArray(string paramVal)
        {
            try
            {
                string[] elems = (paramVal ?? "").Split(new char[] { ' ', ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                int len = elems.Length;
                int[] arr = new int[len];

                for (int i = 0; i < len; i++)
                    arr[i] = int.Parse(elems[i]);

                return arr;
            }
            catch (FormatException ex)
            {
                throw new FormatException("Query parameter is not array of integers.", ex);
            }
        }

        /// <summary>
        /// Преобразовать значение параметра запроса в множество целых чисел
        /// </summary>
        public static HashSet<int> QueryParamToIntSet(string paramVal)
        {
            try
            {
                string[] elems = (paramVal ?? "").Split(new char[] { ' ', ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                int len = elems.Length;
                HashSet<int> hashSet = new HashSet<int>();

                for (int i = 0; i < len; i++)
                    hashSet.Add(int.Parse(elems[i]));

                return hashSet;
            }
            catch (FormatException ex)
            {
                throw new FormatException("Query parameter is not set of integers.", ex);
            }
        }

        /// <summary>
        /// Преобразовать дату в значение параметра запроса
        /// </summary>
        public static string DateToQueryParam(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
