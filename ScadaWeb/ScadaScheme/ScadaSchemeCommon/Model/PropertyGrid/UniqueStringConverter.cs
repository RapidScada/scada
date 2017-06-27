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
 * Summary  : Converter of strings for PropertyGrid which checks value uniqueness
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using System;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Converter of strings for PropertyGrid which checks value uniqueness
    /// <para>Преобразователь строк для PropertyGrid, который проверяет уникальность значения</para>
    /// </summary>
    public class UniqueStringConverter : StringConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (context.Instance is IUniqueItem)
            {
                string key = (value ?? "").ToString().Trim();

                if (((IUniqueItem)context.Instance).KeyIsUnique(key))
                    return key;
                else
                    throw new Exception(SchemePhrases.StringUniqueError);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
