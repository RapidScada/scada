/*
 * Copyright 2021 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpEnronModbus
 * Summary  : Device driver communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Comm.Devices.EnronModbus;
using Scada.Comm.Devices.Modbus;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver communication logic.
    /// <para>Логика работы драйвера КП.</para>
    /// </summary>
    public class KpEnronModbusLogic : KpModbusLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public KpEnronModbusLogic(int number)
            : base(number)
        {
        }


        /// <summary>
        /// Gets the key of the template dictionary.
        /// </summary>
        protected override string TemplateDictKey
        {
            get
            {
                return "EnronModbus.Templates";
            }
        }

        /// <summary>
        /// Gets a device template factory.
        /// </summary>
        protected override DeviceTemplateFactory GetTemplateFactory()
        {
            return KpUtils.TemplateFactory;
        }
    }
}
