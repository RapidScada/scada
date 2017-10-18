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
 * Summary  : Codes of the supported Modbus functions
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Codes of the supported Modbus functions
    /// <para>Коды поддерживаемых функций Modbus</para>
    /// </summary>
    public static class FuncCodes
    {
        /// <summary>
        /// Считать дискретные входы
        /// </summary>
        public const byte ReadDiscreteInputs = 0x02;

        /// <summary>
        /// Считать флаги
        /// </summary>
        public const byte ReadCoils = 0x01;

        /// <summary>
        /// Считать входные регистры
        /// </summary>
        public const byte ReadInputRegisters = 0x04;

        /// <summary>
        /// Считать регистры хранения
        /// </summary>
        public const byte ReadHoldingRegisters = 0x03;

        /// <summary>
        /// Записать флаг
        /// </summary>
        public const byte WriteSingleCoil = 0x05;

        /// <summary>
        /// Записать регистр хранения
        /// </summary>
        public const byte WriteSingleRegister = 0x06;

        /// <summary>
        /// Записать множество флагов
        /// </summary>
        public const byte WriteMultipleCoils = 0x0F;

        /// <summary>
        /// Записать множество регистров хранения
        /// </summary>
        public const byte WriteMultipleRegisters = 0x10;
    }
}
