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
 * Summary  : Allows to remember that a user is logged on
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Allows to remember that a user is logged on
    /// <para>Позволяет запоминать, что пользователь вошёл в систему</para>
    /// </summary>
    public class RememberMe
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected RememberMe()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RememberMe(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("User name must not be null or empty.", "userName");


        }
    }
}
