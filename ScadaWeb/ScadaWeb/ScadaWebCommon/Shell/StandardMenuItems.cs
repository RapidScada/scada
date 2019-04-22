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
 * Module   : ScadaWebCommon
 * Summary  : Standard menu items
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2017
 */

using System;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Standard menu items
    /// <para>Стандартные элементы меню</para>
    /// </summary>
    [Flags]
    public enum StandardMenuItems
    {
        /// <summary>
        /// Элементы отсутствуют
        /// </summary>
        None = 0,
        /// <summary>
        /// Отчёты
        /// </summary>
        Reports = 1,
        /// <summary>
        ///  Администрирование
        /// </summary>
        Admin = 2,
        /// <summary>
        ///  Конфигурация
        /// </summary>
        Config = 4,
        /// <summary>
        ///  Регистрация
        /// </summary>
        Reg = 8,
        /// <summary>
        ///  Плагины
        /// </summary>
        Plugins = 16,
        /// <summary>
        ///  О приложении
        /// </summary>
        About = 32
    }
}
