/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ScadaCommCommon
 * Summary  : The class contains utility methods for SCADA-Communicator and its libraries. UI utilities
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Comm
{
    partial class CommUtils
    {
        /// <summary>
        /// Выбрать элемент выпадающего списка, используя карту соответствия значений и индексов элементов списка
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static void SelectItem(this ComboBox comboBox, object value, 
            Dictionary<string, int> valueToItemIndex, int defaultIndex = -1)
        {
            string valStr = value.ToString();
            if (valueToItemIndex.ContainsKey(valStr))
                comboBox.SelectedIndex = valueToItemIndex[valStr];
            else if (defaultIndex >= 0)
                comboBox.SelectedIndex = defaultIndex;
        }

        /// <summary>
        /// Получить выбранный элемент выпадающего списка, используя карту соответствия индексов элементов списка и значений
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static object GetSelectedItem(this ComboBox comboBox, Dictionary<int, object> indexToValue)
        {
            object val;
            if (indexToValue.TryGetValue(comboBox.SelectedIndex, out val))
                return val;
            else
                throw new InvalidOperationException("Unable to find combo box selected index in the dictionary.");
        }

        /// <summary>
        /// Распознать текст в поле выпадающего списка
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static T ParseText<T>(this ComboBox comboBox) where T : struct
        {
            T val;
            if (Enum.TryParse<T>(comboBox.Text, true, out val))
                return val;
            else
                throw new FormatException("Unable to parse combo box text.");
        }
    }
}
