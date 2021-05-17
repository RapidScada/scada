using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Scada.Web
{
    partial class WebUtils
    {
        /// <summary>
        /// Получить логическое значение параметра из строки запроса.
        /// </summary>
        public static bool GetParamAsBool(this NameValueCollection queryString, string paramName, 
            bool defaultVal = false)
        {
            return bool.TryParse(queryString[paramName], out bool paramVal) ? paramVal : defaultVal;
        }

        /// <summary>
        /// Получить целое значение параметра из строки запроса.
        /// </summary>
        public static int GetParamAsInt(this NameValueCollection queryString, string paramName, int defaultVal = 0)
        {
            return int.TryParse(queryString[paramName], out int paramVal) ? paramVal : defaultVal;
        }

        /// <summary>
        /// Получить значение параметра из строки запроса как дату.
        /// </summary>
        public static DateTime GetParamAsDate(this NameValueCollection queryString, DateTime? defaultVal = null,
            string yearParamName = "year", string monthParamName = "month", string dayParamName = "day")
        {
            int.TryParse(queryString[yearParamName], out int year);
            int.TryParse(queryString[monthParamName], out int month);
            int.TryParse(queryString[dayParamName], out int day);

            if (year == 0 && month == 0 && day == 0)
            {
                return defaultVal ?? DateTime.MinValue;
            }
            else
            {
                try { return new DateTime(year, month, day); }
                catch { throw new ScadaException(WebPhrases.IncorrectDate); }
            }
        }

        /// <summary>
        /// Получить значение параметра из строки запроса как дату.
        /// </summary>
        public static DateTime GetParamAsDate(this NameValueCollection queryString, string paramName, 
            DateTime? defaultVal = null)
        {
            string dateStr = queryString[paramName];

            if (string.IsNullOrEmpty(dateStr))
            {
                return defaultVal ?? DateTime.MinValue;
            }
            else if (DateTime.TryParse(dateStr, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                out DateTime dateTime))
            {
                return dateTime.Date;
            }
            else
            {
                throw new ScadaException(WebPhrases.IncorrectDate);
            }
        }

        /// <summary>
        /// Gets the XML attribute value as an enumeration element.
        /// </summary>
        public static T GetParamAsEnum<T>(this NameValueCollection queryString, string paramName,
            T defaultVal = default) where T : struct
        {
            return Enum.TryParse(queryString[paramName], true, out T paramVal) ?
                paramVal : defaultVal;
        }

        /// <summary>
        /// Получить значение параметра из строки запроса как массив целых чисел.
        /// </summary>
        public static int[] GetParamAsIntArray(this NameValueCollection queryString, string paramName)
        {
            return ScadaUtils.ParseIntArray(queryString[paramName]);
        }

        /// <summary>
        /// Получить значение параметра из строки запроса как множество целых чисел.
        /// </summary>
        public static HashSet<int> GetParamAsIntSet(this NameValueCollection queryString, string paramName)
        {
            return ScadaUtils.ParseIntSet(queryString[paramName]);
        }

        /// <summary>
        /// Преобразовать дату в значение параметра запроса.
        /// </summary>
        public static string DateToQueryParam(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
