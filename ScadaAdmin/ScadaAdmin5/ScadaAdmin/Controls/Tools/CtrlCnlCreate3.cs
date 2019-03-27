/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : Administrator
 * Summary  : Channel creation wizard. Step 2
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
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
using Scada.Admin.Project;

namespace Scada.Admin.App.Controls.Tools
{
    /// <summary>
    /// Channel creation wizard. Step 1.
    /// <para>Мастер создания каналов. Шаг 1.</para>
    /// </summary>
    public partial class CtrlCnlCreate3 : UserControl
    {
        private ScadaProject project; // the project under development


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCnlCreate3()
        {
            InitializeComponent();

            numStartInCnl.Maximum = ushort.MaxValue;
            numEndInCnl.Maximum = int.MaxValue;
            numStartOutCnl.Maximum = ushort.MaxValue;
            numEndOutCnl.Maximum = int.MaxValue;
        }


        /// <summary>
        /// Gets or sets the selected device name.
        /// </summary>
        public string DeviceName
        {
            get
            {
                return txtDevice.Text;
            }
            set
            {
                txtDevice.Text = value ?? "";
            }
        }

        /// <summary>
        /// Gets the start input channel number.
        /// </summary>
        public int StartInCnl
        {
            get
            {
                return Convert.ToInt32(numStartInCnl.Value);
            }
        }

        /// <summary>
        /// Gets the start output channel number.
        /// </summary>
        public int StartOutCnl
        {
            get
            {
                return Convert.ToInt32(numStartOutCnl.Value);
            }
        }


        /// <summary>
        /// Sets the channel numbers by default.
        /// </summary>
        private void SetCnlNums()
        {
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(ScadaProject project)
        {
            this.project = project ?? throw new ArgumentNullException("project");
            SetCnlNums();
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            numStartInCnl.Select();
        }
    }
}
