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
 * Summary  : Converter of component ID for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using System;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Converter of component ID for PropertyGrid.
    /// <para>Преобразователь идентификатора компонента для PropertyGrid.</para>
    /// </summary>
    public class ComponentConverter : Int32Converter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (context?.Instance is ISchemeViewAvailable schemeViewAvailable &&
                value is int componentID &&
                destinationType == typeof(string))
            {
                if (componentID <= 0)
                    return SchemePhrases.EmptyValue;
                else if (schemeViewAvailable.SchemeView.Components.TryGetValue(componentID, out BaseComponent component))
                    return component.ToString();
                else
                    return "[" + componentID + "] " + SchemePhrases.ComponentNotFound;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value is string strVal ?
                int.TryParse(strVal, out int componentID) ? componentID : 0 :
                base.ConvertFrom(context, culture, value);
        }
    }
}
