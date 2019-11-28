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
 * Summary  : The class contains utility methods for the whole system. Collection tools
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System.Collections.Generic;
using System.Globalization;

namespace Scada
{
    partial class ScadaUtils
    {
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
    }
}
