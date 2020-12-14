/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : The class contains utility methods for the whole system. Collection tools
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Scada
{
    partial class ScadaUtils
    {
        /// <summary>
        /// The separator of array elements.
        /// </summary>
        private static readonly char[] ArraySeparator = { ';', ',', ' ' };
        /// <summary>
        /// The preferred array separator.
        /// </summary>
        private static string PreferredSeparator = ",";


        /// <summary>
        /// Gets the value associated with the specified key as a string.
        /// </summary>
        public static string GetValueAsString(this IDictionary<string, string> dictionary, 
            string key, string defaultValue = "")
        {
            return dictionary.TryGetValue(key, out string val) ?
                val : defaultValue;
        }

        /// <summary>
        /// Gets the value associated with the specified key as a boolean.
        /// </summary>
        public static bool GetValueAsBool(this IDictionary<string, string> dictionary, 
            string key, bool defaultValue = false)
        {
            return dictionary.TryGetValue(key, out string valStr) && bool.TryParse(valStr, out bool val) ?
                val : defaultValue;
        }

        /// <summary>
        /// Gets the value associated with the specified key as an integer.
        /// </summary>
        public static int GetValueAsInt(this IDictionary<string, string> dictionary, 
            string key, int defaultValue = 0)
        {
            return dictionary.TryGetValue(key, out string valStr) && int.TryParse(valStr, out int val) ?
                val : defaultValue;
        }

        /// <summary>
        /// Gets the value associated with the specified key as a double.
        /// </summary>
        public static double GetValueAsDouble(this IDictionary<string, string> dictionary, 
            string key, double defaultValue = 0)
        {
            return dictionary.TryGetValue(key, out string valStr) && 
                double.TryParse(valStr, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double val) ?
                val : defaultValue;
        }

        /// <summary>
        /// Gets the child XML node value as an enumeration element.
        /// </summary>
        public static T GetValueAsEnum<T>(this IDictionary<string, string> dictionary,
            string key, T defaultValue = default(T)) where T : struct
        {
            return dictionary.TryGetValue(key, out string valStr) && Enum.TryParse(valStr, true, out T val) ? 
                val : defaultValue;
        }

        /// <summary>
        /// Converts the string representation of an array of integers to its array equivalent.
        /// </summary>
        public static int[] ParseIntArray(string s)
        {
            try
            {
                string[] elems = (s ?? "").Split(ArraySeparator, StringSplitOptions.RemoveEmptyEntries);
                int len = elems.Length;
                int[] arr = new int[len];

                for (int i = 0; i < len; i++)
                {
                    arr[i] = int.Parse(elems[i]);
                }

                return arr;
            }
            catch (FormatException ex)
            {
                throw new FormatException("The specified string is not array of integers.", ex);
            }
        }

        /// <summary>
        /// Converts the string representation of a set of integers to its hash set equivalent.
        /// </summary>
        public static HashSet<int> ParseIntSet(string s)
        {
            try
            {
                string[] elems = (s ?? "").Split(ArraySeparator, StringSplitOptions.RemoveEmptyEntries);
                int len = elems.Length;
                HashSet<int> hashSet = new HashSet<int>();

                for (int i = 0; i < len; i++)
                {
                    hashSet.Add(int.Parse(elems[i]));
                }

                return hashSet;
            }
            catch (FormatException ex)
            {
                throw new FormatException("The specified string is not set of integers.", ex);
            }
        }

        /// <summary>
        /// Converts the collection of integers to its string representation.
        /// </summary>
        public static string IntCollectionToStr(ICollection<int> collection)
        {
            return collection == null ? "" : string.Join(PreferredSeparator, collection);
        }
    }
}
