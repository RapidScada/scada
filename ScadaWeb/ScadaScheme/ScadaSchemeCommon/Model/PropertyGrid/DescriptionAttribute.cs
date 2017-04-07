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
 * Module   : Scheme Editor
 * Summary  : Specifies the description for a property displayed in PropertyGrid which can be changed programmatically
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Specifies the description for a property displayed in PropertyGrid which can be changed programmatically
    /// <para>Задаёт описание свойства, отображаемое в PropertyGrid, которое может быть изменено программно</para>
    /// </summary>
    public class DescriptionAttribute : System.ComponentModel.DescriptionAttribute
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DescriptionAttribute()
            : base()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DescriptionAttribute(string description)
            : base(description)
        {
        }


        /// <summary>
        /// Получить или установить описание
        /// </summary>
        public new string DescriptionValue
        {
            get
            {
                return base.DescriptionValue;
            }
            set
            {
                base.DescriptionValue = value;
            }
        }
    }
}
