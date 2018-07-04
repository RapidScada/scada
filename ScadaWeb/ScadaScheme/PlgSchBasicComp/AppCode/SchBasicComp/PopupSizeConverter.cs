/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : Converter of PopupSize structures for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Web.Plugins.SchBasicComp
{
    /// <summary>
    /// Converter of PopupSize structures for PropertyGrid
    /// <para>Преобразователь структур типа PopupSize для PropertyGrid</para>
    /// </summary>
    internal class PopupSizeConverter : TypeConverter
    {
        private readonly EnumConverter enumConverter = new Scheme.Model.PropertyGrid.EnumConverter(typeof(PopupWidth));


        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new PopupSize((PopupWidth)propertyValues["Width"], (int)propertyValues["Height"]);
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
            return destinationType == typeof(string) && value is PopupSize popupSize ?
                enumConverter.ConvertToString(popupSize.Width) + "; " + popupSize.Height :
                base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
