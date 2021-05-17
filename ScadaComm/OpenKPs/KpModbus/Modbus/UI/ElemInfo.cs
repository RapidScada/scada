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
 * Summary  : Information about Modbus element
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using Scada.Comm.Devices.Modbus.Protocol;

namespace Scada.Comm.Devices.Modbus.UI
{
    /// <summary>
    /// Information about Modbus element
    /// <para>Информация об элементе Modbus</para>
    /// </summary>
    public class ElemInfo
    {
        /// <summary>
        /// Получить или установить элемент
        /// </summary>
        public Elem Elem { get; set; }

        /// <summary>
        /// Получить или установить группу элементов, в которую входит элемент
        /// </summary>
        public ElemGroup ElemGroup { get; set; }

        /// <summary>
        /// Получить или установить ссылку настройки шаблона
        /// </summary>
        public DeviceTemplate.Settings Settings { get; set; }

        /// <summary>
        /// Получить или установить адрес, начинающийся от 0
        /// </summary>
        public ushort Address { get; set; }

        /// <summary>
        /// Получить или установить сигнал КП
        /// </summary>
        public int Signal { get; set; }

        /// <summary>
        /// Получить строковую запись диапазона адресов элемента
        /// </summary>
        public string AddressRange
        {
            get
            {
                return ModbusUtils.GetAddressRange(Address, Elem.Quantity, Settings.ZeroAddr, Settings.DecAddr);
            }
        }

        /// <summary>
        /// Получить обозначение элемента в дереве
        /// </summary>
        public string Caption
        {
            get
            {
                return (string.IsNullOrEmpty(Elem.Name) ? KpPhrases.DefElemName : Elem.Name) + 
                    " (" + AddressRange + ")";
            }
        }
    }
}
