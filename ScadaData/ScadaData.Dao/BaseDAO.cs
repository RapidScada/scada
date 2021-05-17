/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Modified : 2021
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
        /// Gets the number of records selected as a result of the last request.
        /// </summary>
        public int SelectedCount
        {
            get
            {
                return selectedCount;
            }
            protected set 
            {
                selectedCount = value;
            }
        }


        /// <summary>
        /// Получить шаблон для поиска с помощью выражения LIKE
        /// </summary>
        protected string BuildLikePattern(string filter)
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
        
        /// <summary>
        /// Получить значение строки для записи в БД
        /// </summary>
        protected object GetParamValue(string s)
        {
            return string.IsNullOrEmpty(s) ? DBNull.Value : (object)s.Trim();
        }

        /// <summary>
        /// Получить значение идентификатора для записи в БД
        /// </summary>
        protected object GetParamValue(int id)
        {
            return id <= 0 ? DBNull.Value : (object)id;
        }

        /// <summary>
        /// Получить значение вещественного числа для записи в БД
        /// </summary>
        protected object GetParamValue(double value)
        {
            return double.IsNaN(value) ? DBNull.Value : (object)value;
        }

        /// <summary>
        /// Получить значение даты и времени для записи в БД
        /// </summary>
        protected object GetParamValue(DateTime value)
        {
            return value == DateTime.MinValue ? DBNull.Value : (object)value;
        }
    }
}
