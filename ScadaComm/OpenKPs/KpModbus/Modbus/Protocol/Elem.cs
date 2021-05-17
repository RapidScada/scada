/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Summary  : Represents a Modbus element (register)
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2021
 */

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Represents a Modbus element (register).
    /// <para>Представляет элемент (регистр) Modbus.</para>
    /// </summary>
    public class Elem
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Elem()
        {
            Name = "";
            ElemType = ElemType.Bool;
            ByteOrder = null;
            ByteOrderStr = "";
        }


        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить или установить тип
        /// </summary>
        public ElemType ElemType { get; set; }

        /// <summary>
        /// Gets the quantity of addresses.
        /// </summary>
        public virtual int Quantity
        {
            get
            {
                return ModbusUtils.GetQuantity(ElemType);
            }
        }

        /// <summary>
        /// Gets the element data length.
        /// </summary>
        public virtual int DataLength
        {
            get
            {
                return ModbusUtils.GetDataLength(ElemType);
            }
        }

        /// <summary>
        /// Получить или установить массив, определяющий порядок байт
        /// </summary>
        public int[] ByteOrder { get; set; }

        /// <summary>
        /// Получить или установить строковую запись порядка байт
        /// </summary>
        public string ByteOrderStr { get; set; }
    }
}
