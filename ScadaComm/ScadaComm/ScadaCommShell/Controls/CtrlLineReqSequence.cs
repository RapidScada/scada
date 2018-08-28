/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : Communicator Shell
 * Summary  : Control for editing main request sequence of a communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Shell.Controls
{
    /// <summary>
    /// Control for editing main request sequence of a communication line.
    /// <para>Элемент управления для редактирования последовательности опроса линии связи.</para>
    /// </summary>
    public partial class CtrlLineReqSequence : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineReqSequence()
        {
            InitializeComponent();
            SetColumnNames();
        }


        /// <summary>
        /// Sets the column names for the translation.
        /// </summary>
        private void SetColumnNames()
        {
            colDeviceOrder.Name = "colDeviceOrder";
            colDeviceActive.Name = "colDeviceActive";
            colDeviceBound.Name = "colDeviceBound";
            colDeviceNumber.Name = "colDeviceNumber";
            colDeviceName.Name = "colDeviceName";
            colDeviceDll.Name = "colDeviceDll";
            colDeviceAddress.Name = "colDeviceAddress";
            colDeviceCallNum.Name = "colDeviceCallNum";
            colDeviceTimeout.Name = "colDeviceTimeout";
            colDeviceDelay.Name = "colDeviceDelay";
            colDeviceTime.Name = "colDeviceTime";
            colDevicePeriod.Name = "colDevicePeriod";
            colDeviceCmdLine.Name = "colDeviceCmdLine";
        }
    }
}
