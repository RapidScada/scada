/*
 * Copyright 2021 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpEnronModbus
 * Summary  : Represents a group of Enron Modbus elements
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Comm.Devices.Modbus.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices.EnronModbus.Protocol
{
    /// <summary>
    /// Represents a group of Enron Modbus elements.
    /// <para>Представляет группу элементов Enron Modbus.</para>
    /// </summary>
    internal class EnronElemGroup : ElemGroup
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnronElemGroup(TableType tableType)
            : base(tableType)
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

        /// <summary>
        /// Creates a new Modbus element.
        /// </summary>
        public override Elem CreateElem()
        {
            return new EnronElem();
        }
    }
}
