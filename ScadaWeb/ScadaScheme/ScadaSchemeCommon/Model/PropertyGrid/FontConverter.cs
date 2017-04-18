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
                        sb.Append(Localization.UseRussian ? "; Ж" : "; B");
                    if (font.Italic)
                        sb.Append(Localization.UseRussian ? "; К" : "; I");
                    if (font.Underline)
                        sb.Append(Localization.UseRussian ? "; П" : "; U");
                    return sb.ToString();
                }
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
