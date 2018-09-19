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
 * Module   : ScadaAgentCommon
 * Summary  : Parts of the configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;

namespace Scada.Agent
{
    /// <summary>
    /// Parts of the configuration
    /// <para>Части конфигурации</para>
    /// </summary>
    [Flags]
    public enum ConfigParts
    {
        /// <summary>
        /// Не задано
        /// </summary>
        None = 0,

        /// <summary>
        /// База конфигурации
        /// </summary>
        Base = 1,

        /// <summary>
        /// Интерфейс
        /// </summary>
        Interface = 2,

        /// <summary>
        /// Сервер
        /// </summary>
        Server = 4,

        /// <summary>
        /// Коммуникатор
        /// </summary>
        Comm = 8,

        /// <summary>
        /// Вебстанция
        /// </summary>
        Web = 16,

        /// <summary>
        /// Вся конфигурация
        /// </summary>
        All = Base | Interface | Server | Comm | Web
    }
}
