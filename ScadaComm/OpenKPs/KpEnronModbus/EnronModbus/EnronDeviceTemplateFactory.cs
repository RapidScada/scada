/*
 * Copyright 2021 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpEnronModbus
 * Summary  : Represent a factory for creating Enron Modbus device templates
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Comm.Devices.EnronModbus.Protocol;
using Scada.Comm.Devices.Modbus;
using Scada.Comm.Devices.Modbus.Protocol;

namespace Scada.Comm.Devices.EnronModbus
{
    /// <summary>
    /// Represent a factory for creating Enron Modbus device templates.
    /// <para>Фабрика для для создания шаблонов устройства Enron Modbus.</para>
    /// </summary>
    internal class EnronDeviceTemplateFactory : DeviceTemplateFactory
    {
        /// <summary>
        /// Creates a new device template.
        /// </summary>
        public override DeviceTemplate CreateDeviceTemplate()
        {
            return new EnronDeviceTemplate();
        }
    }
}
