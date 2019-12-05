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
 * Module   : ScadaSchemeCommon
 * Summary  : Converter of collections for PropertyGrid which reflects existence of items
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Converter of collections for PropertyGrid which reflects existence of items.
    /// <para>Преобразователь коллекций для PropertyGrid, который отображает существование элементов.</para>
    /// </summary>
    public class CollectionConverter : System.ComponentModel.CollectionConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return value is ICollection collection && collection.Count > 0 ? 
                    SchemePhrases.CollectionValue : 
                    SchemePhrases.EmptyValue;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
