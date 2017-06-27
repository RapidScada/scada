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
 * Summary  : Converter of Point structures for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.DataTypes;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Converter of Point structures for PropertyGrid
    /// <para>Преобразователь структур типа Point для PropertyGrid</para>
    /// </summary>
    internal class PointConverter : TypeConverter
    {
        private static readonly char[] FieldSep = new char[] { ';', ',', ' ' };


        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new Point((int)propertyValues["X"], (int)propertyValues["Y"]);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(value, attributes);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Point point = (Point)value;
            return destinationType == typeof(string) ?
                point.X + "; " + point.Y :
                base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                if (value is string)
                {
                    string[] parts = ((string)value).Split(FieldSep, StringSplitOptions.RemoveEmptyEntries);
                    int x = Convert.ToInt32(parts[0]);
                    int y = Convert.ToInt32(parts[1]);
                    return new Point(x, y);
                }
                else
                {
                    return Point.Default;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(SchemePhrases.StringConvertError + ": " + ex.Message);
            }
        }
    }
}
