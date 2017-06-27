/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : ScadaSchemeCommon
 * Summary  : Extension methods for channel filter
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;
using System.Collections.Generic;

namespace Scada.Scheme.Model.DataTypes
{
    /// <summary>
    /// Extension methods for channel filter
    /// <para>Методы расширения для фильтра по каналам</para>
    /// </summary>
    internal static class CnlFilterExtenstions
    {
        /// <summary>
        /// Основной разделитель номеров каналов
        /// </summary>
        public const string MainCnlSep = " ";
        /// <summary>
        /// Допустимые разделители номеров каналов
        /// </summary>
        public static readonly char[] CnlSep = new char[] { ';', ',', ' ' };

        /// <summary>
        /// Разобрать строку и заполнить фильтр по каналам
        /// </summary>
        public static void ParseCnlFilter(this List<int> cnlFilter, string s)
        {
            cnlFilter.Clear();
            string[] cnlNums = (s ?? "").Split(CnlSep, StringSplitOptions.RemoveEmptyEntries);

            foreach (string cnlNumStr in cnlNums)
            {
                int cnlNum;
                if (int.TryParse(cnlNumStr, out cnlNum))
                {
                    int ind = cnlFilter.BinarySearch(cnlNum);
                    if (ind < 0)
                        cnlFilter.Insert(~ind, cnlNum);
                }
            }
        }

        /// <summary>
        /// Преобразовать фильтр по каналам в строку
        /// </summary>
        public static string CnlFilterToString(this List<int> cnlFilter)
        {
            return string.Join(MainCnlSep, cnlFilter);
        }
    }
}
