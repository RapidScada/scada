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
 * Summary  : Converter of fonts for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.DataTypes;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Converter of fonts for PropertyGrid
    /// <para>Преобразователь шрифтов для PropertyGrid</para>
    /// </summary>
    internal class FontConverter : TypeConverter
    {
        private static readonly char[] FieldSep = new char[] { ';' };


        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Font font = value as Font;

                if (font == null)
                {
                    return "";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    if (!string.IsNullOrEmpty(font.Name))
                        sb.Append(font.Name);
                    if (sb.Length > 0)
                        sb.Append("; ");
                    sb.Append(font.Size);
                    if (font.Bold)
                        sb.Append("; ").Append(SchemePhrases.BoldSymbol);
                    if (font.Italic)
                        sb.Append("; ").Append(SchemePhrases.ItalicSymbol);
                    if (font.Underline)
                        sb.Append("; ").Append(SchemePhrases.UnderlineSymbol);
                    return sb.ToString();
                }
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
                string[] parts = ((string)value).Split(FieldSep, StringSplitOptions.RemoveEmptyEntries);
                int partsLen = parts.Length;

                if (partsLen > 0)
                {
                    Font font = new Font();
                    font.Name = parts[0];

                    for (int i = 1; i < partsLen; i++)
                    {
                        string part = parts[i].Trim();
                        int size;

                        if (part.Equals(SchemePhrases.BoldSymbol, StringComparison.OrdinalIgnoreCase))
                            font.Bold = true;
                        else if (part.Equals(SchemePhrases.ItalicSymbol, StringComparison.OrdinalIgnoreCase))
                            font.Italic = true;
                        else if (part.Equals(SchemePhrases.UnderlineSymbol, StringComparison.OrdinalIgnoreCase))
                            font.Underline = true;
                        else if (int.TryParse(part, out size))
                            font.Size = size;
                    }

                    return font;
                }
            }

            return null;
        }
    }
}
