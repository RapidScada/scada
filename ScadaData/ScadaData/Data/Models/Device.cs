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
 * Module   : ScadaData
 * Summary  : Device properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Device properties
    /// <para>Свойства КП</para>
    /// </summary>
    [Obsolete("Use Scada.Data.Entities.*")]
    public class Device
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Device()
        {
            DevNum = 0;
            Name = "";
            DevTypeID = 0;
            Address = 0;
            CallNum = "";
            CommLineNum = 0;
            Descr = "";
        }


        /// <summary>
        /// Получить или установить номер КП
        /// </summary>
        public int DevNum { get; set; }
        
        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить или установить ид. типа КП
        /// </summary>
        public int DevTypeID { get; set; }

        /// <summary>
        /// Получить или установить адрес
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Получить или установить позывной
        /// </summary>
        public string CallNum { get; set; }

        /// <summary>
        /// Получить или установить номер линии связи
        /// </summary>
        public int CommLineNum { get; set; }

        /// <summary>
        /// Получить или установить описание
        /// </summary>
        public string Descr { get; set; }
    }
}
