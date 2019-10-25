/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : KpOpcUa
 * Summary  : Types of updating the device configuration tree
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;

namespace Scada.Comm.Devices.OpcUa.UI
{
    /// <summary>
    /// Types of updating the device configuration tree.
    /// <para>Типы обновления дерева конфигурации КП.</para>
    /// </summary>
    [Flags]
    public enum TreeUpdateTypes
    {
        /// <summary>
        /// No update required.
        /// </summary>
        None = 0,

        /// <summary>
        /// Current node update is required.
        /// </summary>
        CurrentNode = 1,

        /// <summary>
        /// Signals update is required.
        /// </summary>
        UpdateSignals = 2
    }
}
