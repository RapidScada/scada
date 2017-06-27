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
 * Summary  : The base class for accessing PostgreSQL database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Npgsql;

namespace Scada.Dao
{
    /// <summary>
    /// The base class for accessing PostgreSQL database
    /// <para>Базовый класс для доступа к базе данных PostgreSQL</para>
    /// </summary>
    public class PgSqlDAO : BaseDAO
    {
        /// <summary>
        /// Соединение с БД
        /// </summary>
        protected NpgsqlConnection conn;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected PgSqlDAO()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public PgSqlDAO(NpgsqlConnection conn)
            : base()
        {
            this.conn = conn;
        }
    }
}
