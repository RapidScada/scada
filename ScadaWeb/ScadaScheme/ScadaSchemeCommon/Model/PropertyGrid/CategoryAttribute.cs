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
 * Summary  : Specifies the category displayed in PropertyGrid which can be changed programmatically
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Specifies the category displayed in PropertyGrid which can be changed programmatically
    /// <para>Задаёт категорию, отображаемую в PropertyGrid, которая может быть изменена программно</para>
    /// </summary>
    public class CategoryAttribute : System.ComponentModel.CategoryAttribute
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CategoryAttribute()
            : base()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CategoryAttribute(string category)
        {
            CategoryName = category;
        }


        /// <summary>
        /// Получить или установить наименование категории
        /// </summary>
        public string CategoryName { get; set; }


        /// <summary>
        /// Вывести локализованное имя категории
        /// </summary>
        protected override string GetLocalizedString(string value)
        {
            return CategoryName;
        }
    }
}
