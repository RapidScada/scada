/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Summary  : User properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.IO;

namespace Scada.Data.Models
{
    /// <summary>
    /// User properties
    /// <para>Свойства пользователя</para>
    /// </summary>
    public class UserProps
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public UserProps()
            : this(0)
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserProps(int userID)
        {
            UserID = userID;
            UserName = "";
            RoleID = 0;
            RoleName = "";
        }


        /// <summary>
        /// Получить или установить идентификатор пользователя
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Получить или установить имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Получить или установить идентификатор роли пользователя
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// Получить или установить наименование роли пользователя
        /// </summary>
        public string RoleName { get; set; }
    }
}
