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
 * Module   : KpModbus
 * Summary  : Types of updating the Modbus template tree
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;

namespace Scada.Comm.Devices.Modbus.UI
{
    /// <summary>
    /// Types of updating the Modbus template tree
    /// <para>Типы обновления дерева шаблона Modbus</para>
    /// </summary>
    [Flags]
    public enum TreeUpdateTypes
    {
        /// <summary>
        /// Обновление не требуется
        /// </summary>
        None = 0,

        /// <summary>
        /// Текущий узел
        /// </summary>
        CurrentNode = 1,

        /// <summary>
        /// Дочерние узлы
        /// </summary>
        ChildNodes = 2,

        /// <summary>
        /// Узлы того же уровня, следующие за текущим
        /// </summary>
        NextSiblings = 4,

        /// <summary>
        /// Обновить сигналы
        /// </summary>
        UpdateSignals = 8
    }
}
