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
 * Summary  : Modbus element (register)
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Modbus element (register)
    /// <para>Элемент (регистр) Modbus</para>
    /// </summary>
    public class Elem
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Elem()
        {
            Name = "";
            ElemType = ElemTypes.Bool;
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
        public ElemTypes ElemType { get; set; }

        /// <summary>
        /// Получить длину элемента (количество адресов)
        /// </summary>
        public int Length
        {
            get
            {
                return ModbusUtils.GetElemCount(ElemType);
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
