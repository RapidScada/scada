/*
 * Copyright 2021 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpEnronModbus
 * Summary  : Represents an Enron Modbus element
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Comm.Devices.Modbus.Protocol;

namespace Scada.Comm.Devices.EnronModbus.Protocol
{
    /// <summary>
    /// Represents an Enron Modbus element.
    /// <para>Представляет элемент Enron Modbus.</para>
    /// </summary>
    internal class EnronElem : Elem
    {
        /// <summary>
        /// Gets the quantity of addresses.
        /// </summary>
        public override int Quantity
        {
            get
            {
                return 1;
            }
        }
    }
}
