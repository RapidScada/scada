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
 * Summary  : Rights of the web application user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Rights of the web application user
    /// <para>Права пользователя веб-приложения</para>
    /// </summary>
    public class UserRights
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public UserRights()
        {
            ConfigRight = false;
        }


        /// <summary>
        /// Инициализировать права пользователя
        /// </summary>
        public void Init(int roleID)
        {
            if (roleID == BaseValues.Roles.Admin)
            {
                ConfigRight = true;
            }
        }


        /// <summary>
        /// Получить право конфигурирования системы
        /// </summary>
        public bool ConfigRight { get; protected set; }
    }
}