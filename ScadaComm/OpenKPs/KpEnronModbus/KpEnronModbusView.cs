/*
 * Copyright 2021 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpEnronModbus
 * Summary  : Device driver user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Comm.Devices.EnronModbus.UI;
using Scada.Comm.Devices.Modbus.UI;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver user interface.
    /// <para>Пользовательский интерфейс драйвера КП.</para>
    /// </summary>
    public class KpEnronModbusView : KpModbusView
    {
        /// <summary>
        /// The driver version.
        /// </summary>
        internal const string KpVersion = "5.0.0.0";

        /// <summary>
        /// The UI customization object.
        /// </summary>
        private static readonly UiCustomization UiCustomization = new EnronUiCustomization();


        /// <summary>
        /// Initializes a new instance of the class. Designed for general configuring.
        /// </summary>
        public KpEnronModbusView()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class. Designed for configuring a particular device.
        /// </summary>
        public KpEnronModbusView(int number)
            : base(number)
        {
        }


        /// <summary>
        /// Gets the driver version.
        /// </summary>
        public override string Version
        {
            get
            {
                return KpVersion;
            }
        }


        /// <summary>
        /// Gets a UI customization object.
        /// </summary>
        protected override UiCustomization GetUiCustomization()
        {
            return UiCustomization;
        }
    }
}
