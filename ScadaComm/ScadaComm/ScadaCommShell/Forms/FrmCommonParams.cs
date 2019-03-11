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
 * Module   : Communicator Shell
 * Summary  : Form for editing common parameters of Communicator settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Comm.Shell.Code;
using Scada.UI;
using System;
using System.Windows.Forms;
using WinControl;

namespace Scada.Comm.Shell.Forms
{
    /// <summary>
    /// Form for editing common parameters of Communicator settings.
    /// <para>Форма редактирования общих параметров Коммуникатора.</para>
    /// </summary>
    public partial class FrmCommonParams : Form, IChildForm
    {
        private Settings.CommonParams commonParams; // the common parameters to edit
        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCommonParams()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCommonParams(Settings.CommonParams commonParams)
            : this()
        {
            this.commonParams = commonParams ?? throw new ArgumentNullException("commonParams");
            changing = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        private void SettingsToControls()
        {
            changing = true;

            // connection
            chkServerUse.Checked = commonParams.ServerUse;
            txtServerHost.Text = commonParams.ServerHost;
            numServerPort.SetValue(commonParams.ServerPort);
            numServerTimeout.SetValue(commonParams.ServerTimeout);
            txtServerUser.Text = commonParams.ServerUser;
            txtServerPwd.Text = commonParams.ServerPwd;

            // runtime options
            numWaitForStop.SetValue(commonParams.WaitForStop);
            chkSendModData.Checked = commonParams.SendModData;
            numSendAllDataPer.SetValue(commonParams.SendAllDataPer);
            numSendAllDataPer.Enabled = chkSendModData.Checked;

            changing = false;
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        private void ControlsToSettings()
        {
            // connection
            commonParams.ServerUse = chkServerUse.Checked;
            commonParams.ServerHost = txtServerHost.Text;
            commonParams.ServerPort = decimal.ToInt32(numServerPort.Value);
            commonParams.ServerTimeout = decimal.ToInt32(numServerTimeout.Value);
            commonParams.ServerUser = txtServerUser.Text;
            commonParams.ServerPwd = txtServerPwd.Text;

            // runtime options
            commonParams.WaitForStop = decimal.ToInt32(numWaitForStop.Value);
            commonParams.SendModData = chkSendModData.Checked;
            commonParams.SendAllDataPer = decimal.ToInt32(numSendAllDataPer.Value);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            ControlsToSettings();

            if (ChildFormTag.SendMessage(this, CommMessage.SaveSettings))
                ChildFormTag.Modified = false;
        }


        private void FrmCommonParams_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            SettingsToControls();
        }

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                ChildFormTag.Modified = true;
        }

        private void chkSendModData_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                numSendAllDataPer.Enabled = chkSendModData.Checked;
                ChildFormTag.Modified = true;
            }
        }
    }
}
