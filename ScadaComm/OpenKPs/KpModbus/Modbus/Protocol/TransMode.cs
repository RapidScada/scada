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
 * Module   : KpModbus
 * Summary  : Data transfer modes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2018
 */

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Data transfer modes
    /// <para>Режимы передачи данных</para>
    /// </summary>
    public enum TransMode
    {
        /// <summary>
        /// Передача данных в бинарном формате
        /// </summary>
        RTU,

        /// <summary>
        /// Передача данных в символьном формате
        /// </summary>
        ASCII,

        /// <summary>
        /// Передача данных по протоколу TCP/IP
        /// </summary>
        TCP
    }
}
