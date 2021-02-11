/*
 * Copyright 2021 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpEnronModbus
 * Summary  : Represents an Enron Modbus device template
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Comm.Devices.Modbus.Protocol;

namespace Scada.Comm.Devices.EnronModbus.Protocol
{
    /// <summary>
    /// Represents an Enron Modbus device template.
    /// <para>Представляет шаблон устройства Enron Modbus.</para>
    /// </summary>
    internal class EnronDeviceTemplate : DeviceTemplate
    {
        /// <summary>
        /// Creates a new group of Modbus elements.
        /// </summary>
        public override ElemGroup CreateElemGroup(TableType tableType)
        {
            return new EnronElemGroup(tableType);
        }

        /// <summary>
        /// Creates a new Modbus command.
        /// </summary>
        public override ModbusCmd CreateModbusCmd(TableType tableType, bool multiple)
        {
            return new EnronCmd(tableType, multiple);
        }
    }
}
