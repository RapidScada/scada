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
 * Module   : ScadaWebCommon
 * Summary  : User activity monitor
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// User activity monitor
    /// <para>Монитор активности пользователей</para>
    /// </summary>
    public class UserMonitor
    {
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected UserMonitor()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserMonitor(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this.log = log;
        }
    }
}
