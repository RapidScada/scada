/*
 * Copyright 2021 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpEnronModbus
 * Summary  : Represents an Enron Modbus command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Comm.Devices.Modbus.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Devices.EnronModbus.Protocol
{
    /// <summary>
    /// Represents an Enron Modbus command.
    /// <para>Представляет команду Enron Modbus.</para>
    /// </summary>
    internal class EnronCmd : ModbusCmd
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnronCmd(TableType tableType, bool multiple)
            : base(tableType, multiple)
        {
        }


        /// <summary>
        /// Gets the element type depending on the data table type.
        /// </summary>
        public override ElemType GetDefElemType(TableType tableType)
        {
            return tableType == TableType.DiscreteInputs || tableType == TableType.Coils ?
                ElemType.Bool : ElemType.Float;
        }
    }
}
