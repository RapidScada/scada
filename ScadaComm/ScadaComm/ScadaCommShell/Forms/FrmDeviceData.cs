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
 * Summary  : Form to monitor a device.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Agent;
using Scada.Agent.UI;
using Scada.Comm.Devices;
using Scada.Comm.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Comm.Shell.Forms
{
    /// <summary>
    /// Form to monitor a device.
    /// <para>Форма для мониторинга устройства.</para>
    /// </summary>
    public partial class FrmDeviceData : Form, IChildForm
    {
        private readonly Settings.KP kp;              // the device to control
        private readonly Settings.CommLine commLine;  // the communication line that contains the device
        private readonly CommEnvironment environment; // the application environment
        private RemoteLogBox dataBox;                 // object to refresh device data
        private FrmDeviceCommand frmDeviceCommand;    // the form to send command


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
        public FrmDeviceData(Settings.KP kp, Settings.CommLine commLine, CommEnvironment environment)
            : this()
        {
            this.kp = kp ?? throw new ArgumentNullException("kp");
            this.commLine = commLine ?? throw new ArgumentNullException("commLine");
            this.environment = environment ?? throw new ArgumentNullException("environment");
            dataBox = new RemoteLogBox(lbDeviceData) { FullLogView = true };
            frmDeviceCommand = null;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Initializes the device data refresh process.
        /// </summary>
        private void InitRefresh()
        {
            dataBox.AgentClient = environment.AgentClient;

            if (dataBox.AgentClient == null)
            {
                dataBox.SetFirstLine(CommShellPhrases.SetProfile);
                tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
                btnSendCommand.Enabled = false;
                lblCommandInfo.Visible = false;
            }
            else
            {
                dataBox.SetFirstLine(CommShellPhrases.Loading);
                string dataFileName = CommUtils.GetDeviceDataFileName(kp.Number);

                if (dataBox.AgentClient.IsLocal)
                {
                    tmrRefresh.Interval = ScadaUiUtils.LogLocalRefreshInterval;
                    dataBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, dataFileName);
                    btnSendCommand.Enabled = true;
                    lblCommandInfo.Visible = false;
                }
                else
                {
                    tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
                    dataBox.LogPath = new RelPath(ConfigParts.Comm, AppFolder.Log, dataFileName);
                    btnSendCommand.Enabled = false;
                    lblCommandInfo.Visible = true;
                }
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void FrmDeviceData_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            Text = string.Format(Text, kp.Number);
            InitRefresh();
            tmrRefresh.Start();
        }

        private void FrmDeviceData_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmrRefresh.Stop();
        }

        private void FrmDeviceData_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Interval = dataBox.AgentClient != null && dataBox.AgentClient.IsLocal ?
                    ScadaUiUtils.LogLocalRefreshInterval :
                    ScadaUiUtils.LogRemoteRefreshInterval;
            }
            else
            {
                tmrRefresh.Interval = ScadaUiUtils.LogInactiveTimerInterval;
            }
        }

        private async void tmrRefresh_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Stop();

                if (dataBox.AgentClient == environment.AgentClient)
                    await Task.Run(() => dataBox.Refresh());
                else
                    InitRefresh();

                tmrRefresh.Start();
            }
        }

        private void btnDeviceProps_Click(object sender, EventArgs e)
        {
            // show the device properties if possible
            if (environment.TryGetKPView(kp, false, commLine.CustomParams, out KPView kpView, out string errMsg))
            {
                if (kpView.CanShowProps)
                {
                    kpView.ShowProps();

                    if (kpView.KPProps.Modified)
                    {
                        kp.CmdLine = kpView.KPProps.CmdLine;
                        ChildFormTag.SendMessage(this, CommMessage.UpdateLineParams);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowWarning(CommShellPhrases.NoDeviceProps);
                }
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            // show the device command form
            if (frmDeviceCommand == null)
                frmDeviceCommand = new FrmDeviceCommand(kp, environment);

            frmDeviceCommand.ShowDialog();
        }
    }
}
