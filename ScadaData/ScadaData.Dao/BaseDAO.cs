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
 * Module   : ScadaData.Dao
 * Summary  : The base class for database access
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;

namespace Scada.Dao
{
    /// <summary>
    /// The base class for database access
    /// <para>Базовый класс для доступа к базе данных</para>
    /// </summary>
    public abstract class BaseDAO
    {
        /// <summary>
        /// Количество выбраных записей
        /// </summary>
        protected int selectedCount;


        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseDAO()
        {
            selectedCount = 0;
        }


        /// <summary>
        /// Удалить все начальные и конечные знаки пробелов, заменить null на пустую строку
        /// </summary>
        protected void Trim(ref string s)
        {
            s = s == null ? "" : s.Trim();
        }

        /// <summary>
        /// Получить шаблон для поиска с помощью выражения LIKE
        /// </summary>
        protected string GetLikePattern(string filter)
        {
            return filter == null ?
                "" :
                "%" + string.Join("%", filter.Split((string[])null, StringSplitOptions.RemoveEmptyEntries)) + "%";
        }

        /// <summary>
        /// Преобразовать полученный из БД объект в целое число
        /// </summary>
        protected int ConvertToInt(object value)
        {
            return value == null || value == DBNull.Value ? -1 : (int)value;
        }

        /// <summary>
        /// Преобразовать полученный из БД объект в вещественное число
        /// </summary>
        protected double ConvertToDouble(object value)
        {
            return value == null || value == DBNull.Value ? double.NaN : (double)value;
        }

        /// <summary>
        /// Преобразовать полученный из БД объект в дату и время
        /// </summary>
        protected DateTime ConvertToDateTime(object value)
        {
            return value == null || value == DBNull.Value ? DateTime.MinValue : (DateTime)value;
        }
    }
}
