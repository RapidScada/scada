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
 * Summary  : Converter of Size structures for PropertyGrid
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
    /// Converter of Size structures for PropertyGrid
    /// <para>Преобразователь структур типа Size для PropertyGrid</para>
    /// </summary>
    internal class SizeConverter : TypeConverter
    {
        private static readonly char[] FieldSep = new char[] { ';', ',', ' ' };


        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new Size((int)propertyValues["Width"], (int)propertyValues["Height"]);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(value, attributes);
            return props.Sort(new string[] { "Width", "Height" });
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Size size = (Size)value;
            return destinationType == typeof(string) ?
                size.Width + "; " + size.Height :
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
                    int width = Convert.ToInt32(parts[0]);
                    int height = Convert.ToInt32(parts[1]);
                    return new Size(width, height);
                }
                else
                {
                    return Size.Default;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(SchemePhrases.StringConvertError + ": " + ex.Message);
            }
        }
    }
}
