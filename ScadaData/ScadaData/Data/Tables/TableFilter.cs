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
 * Module   : ScadaData
 * Summary  : Represents a filter for selecting data from a table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a filter for selecting data from a table.
    /// <para>Представляет фильтр для выбора данных из таблицы.</para>
    /// </summary>
    public class TableFilter
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableFilter()
            : this("", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableFilter(string columnName, object value)
        {
            ColumnName = columnName;
            Value = value;
            Title = "";
        }


        /// <summary>
        /// Gets or sets the column name to filter.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the filter value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the filter title for displaying.
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Title) ?
                ColumnName + " = " + Value :
                Title;
        }
    }
}
