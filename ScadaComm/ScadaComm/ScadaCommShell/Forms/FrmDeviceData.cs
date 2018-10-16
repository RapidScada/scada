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
 * Summary  : Form to monitor a device.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Comm.Shell.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Shell.Forms
{
    /// <summary>
    /// Form to monitor a device.
    /// <para>Форма для мониторинга устройства.</para>
    /// </summary>
    public partial class FrmDeviceData : Form
    {
        private readonly Settings.KP kp;              // the device to control
        private readonly CommEnvironment environment; // the application environment


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceData(Settings.KP kp, CommEnvironment environment)
            : this()
        {
            this.kp = kp ?? throw new ArgumentNullException("kp");
            this.environment = environment ?? throw new ArgumentNullException("environment");
        }
    }
}
