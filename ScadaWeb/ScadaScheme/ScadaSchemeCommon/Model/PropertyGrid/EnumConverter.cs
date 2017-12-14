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
 * Summary  : Converter of enumerations for PropertyGrid which reflects element description
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Converter of enumerations for PropertyGrid which reflects element description
    /// <para>Преобразователь перечислений для PropertyGrid, который отображает описание элемента</para>
    /// </summary>
    public class EnumConverter : System.ComponentModel.EnumConverter
    {
        private Type enumType;
        private Localization.Dict enumDict;

        public EnumConverter(Type type)
            : base(type)
        {
            enumType = type;
            Localization.Dictionaries.TryGetValue(enumType.FullName, out enumDict);
        }

        private string TranslateEnumField(string fieldName)
        {
            if (enumDict == null)
            {
                // получение значения из атрибута
                FieldInfo fi = enumType.GetField(fieldName);
                DescriptionAttribute da =
                    (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                return da == null ? fieldName : da.Description;
            }
            else
            {
                // получение значения из словаря
                return enumDict.GetPhrase(fieldName, fieldName);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, 
            Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string fieldName = Enum.GetName(enumType, value);
                return TranslateEnumField(fieldName);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (string fieldName in Enum.GetNames(enumType))
                {
                    if (TranslateEnumField(fieldName) == (string)value)
                        return Enum.Parse(enumType, fieldName);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
