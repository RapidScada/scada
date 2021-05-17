/*
 * Copyright 2021 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpEnronModbus
 * Summary  : Implements UI customization of this driver
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Comm.Devices.Modbus;
using Scada.Comm.Devices.Modbus.UI;

namespace Scada.Comm.Devices.EnronModbus.UI
{
    /// <summary>
    /// Implements UI customization of this driver.
    /// <para>Реализует кастомизацию пользовательского интерфейса данного драйвера.</para>
    /// </summary>
    internal class EnronUiCustomization : UiCustomization
    {
        /// <summary>
        /// Gets the device template factory.
        /// </summary>
        public override DeviceTemplateFactory TemplateFactory
        {
            get
            {
                return KpUtils.TemplateFactory;
            }
        }
    }
}
