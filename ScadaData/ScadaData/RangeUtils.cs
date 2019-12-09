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
 * Module   : ScadaData
 * Summary  : The utility methods for converting ranges of numbers.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada
{
    /// <summary>
    /// The utility methods for converting ranges of numbers.
    /// <para>Вспомогательные методы для преобразований диапазонов чисел.</para>
    /// </summary>
    public static class RangeUtils
    {
        /// <summary>
        /// Отображаемый разделитель чисел.
        /// </summary>
        private const string DispSep = ", ";
        /// <summary>
        /// Отображаемый разделитель пары чисел.
        /// </summary>
        private const string DispDash = "-";
        /// <summary>
        /// Разделитель чисел, используемый при разборе строки.
        /// </summary>
        private static readonly char[] ParseSep = new char[] { ',' };
        /// <summary>
        /// Разделитель чисел, используемый при разборе строки.
        /// </summary>
        private const char ParseDash = '-';


        /// <summary>
        /// Преобразовать диапазон целых чисел в строку.
        /// </summary>
        /// <remarks>Например: 1-5, 10</remarks>
        public static string RangeToStr(ICollection<int> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            List<int> list = new List<int>(collection);
            list.Sort();

            StringBuilder sb = new StringBuilder();
            int prevNum = int.MinValue; // предыдущее число
            int bufNum = int.MinValue;  // число в буфере для последующего вывода

            for (int i = 0, last = list.Count - 1; i <= last; i++)
            {
                int curNum = list[i];

                if (bufNum == int.MinValue)
                    bufNum = curNum;

                if (prevNum == curNum - 1)
                {
                    if (i < last)
                        bufNum = curNum;
                    else
                        sb.Append(DispDash).Append(curNum);
                }
                else
                {
                    if (bufNum != curNum)
                        sb.Append(DispDash).Append(bufNum);

                    if (i > 0)
                        sb.Append(DispSep);

                    sb.Append(curNum);
                    bufNum = int.MinValue;
                }

                prevNum = curNum;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Попытаться преобразовать строку в коллекцию целых чисел.
        /// </summary>
        /// <remarks>Например: 1-5, 10</remarks>
        public static bool StrToRange(string s, bool allowEmpty, bool unique, out ICollection<int> collection)
        {
            collection = null;
            string[] parts = (s ?? "").Split(ParseSep, StringSplitOptions.RemoveEmptyEntries);
            List<int> list = new List<int>();
            HashSet<int> set = unique ? new HashSet<int>() : null;

            foreach (string part in parts)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    int dashInd = part.IndexOf(ParseDash);

                    if (dashInd >= 0)
                    {
                        // два числа через тире
                        string s1 = part.Substring(0, dashInd);
                        string s2 = part.Substring(dashInd + 1);

                        if (int.TryParse(s1, out int n1) && int.TryParse(s2, out int n2))
                        {
                            for (int n = n1; n <= n2; n++)
                            {
                                if (set == null || set.Add(n))
                                    list.Add(n);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // одно число
                        if (int.TryParse(part, out int n))
                        {
                            if (set == null || set.Add(n))
                                list.Add(n);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            if (allowEmpty || list.Count > 0)
            {
                list.Sort();
                collection = list;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Преобразовать строку в коллекцию целых чисел.
        /// </summary>
        public static ICollection<int> StrToRange(string s, bool allowEmpty, bool unique, bool throwOnFail = true)
        {
            if (StrToRange(s, allowEmpty, unique, out ICollection<int> collection))
                return collection;
            else if (throwOnFail)
                throw new FormatException("The given string is not a valid range of integers.");
            else
                return null;
        }
    }
}
