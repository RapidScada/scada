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
 * Summary  : Converter of channel filter for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Converter of channel filter for PropertyGrid
    /// <para>Преобразователь фильтра по каналам для PropertyGrid</para>
    /// </summary>
    internal class CnlFilterConverter : System.ComponentModel.CollectionConverter
    {
        /// <summary>
        /// Основной разделитель номеров каналов
        /// </summary>
        public const string MainCnlSep = " ";
        /// <summary>
        /// Допустимые разделители номеров каналов
        /// </summary>
        public static readonly char[] CnlSep = new char[] { ';', ',', ' ' };

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                List<int> cnlFilter = value as List<int>;
                return cnlFilter == null || cnlFilter.Count == 0 ? "" : string.Join(MainCnlSep, cnlFilter);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
