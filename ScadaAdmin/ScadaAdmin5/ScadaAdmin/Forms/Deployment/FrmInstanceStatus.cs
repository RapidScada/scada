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
 * Summary  : Form that displays instance status
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Agent.Connector;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Form that displays instance status.
    /// <para>Форма, отображающая статус экземпляра.</para>
    /// </summary>
    public partial class FrmInstanceStatus : Form
    {
        private readonly AppData appData;   // the common data of the application
        private readonly DeploymentSettings deploymentSettings; // the deployment settings
        private readonly Instance instance; // the affected instance
        private DeploymentProfile initialProfile;       // the initial deployment profile
        private ConnectionSettings initialConnSettings; // copy of the initial connection settings
        private IAgentClient agentClient;               // the Agent client


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmInstanceStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInstanceStatus(AppData appData, DeploymentSettings deploymentSettings, Instance instance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            this.deploymentSettings = deploymentSettings ?? throw new ArgumentNullException("deploymentSettings");
            this.instance = instance ?? throw new ArgumentNullException("instance");
        }


        /// <summary>
        /// Gets a value indicating whether the selected profile changed.
        /// </summary>
        public bool ProfileChanged { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the connection settings were modified.
        /// </summary>
        public bool ConnSettingsModified { get; protected set; }


        /// <summary>
        /// Connects to a remote server.
        /// </summary>
        private void Connect()
        {
            if (!timer.Enabled)
            {
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                gbStatus.Enabled = true;
                timer.Start();
            }
        }

        /// <summary>
        /// Disconnects from the remote server.
        /// </summary>
        private void Disconnect()
        {
            timer.Stop();
            agentClient = null;
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            gbStatus.Enabled = false;
            txtServerStatus.Text = "";
            txtCommStatus.Text = "";
            txtUpdateTime.Text = "";
        }

        /// <summary>
        /// Gets the string representation of the service status.
        /// </summary>
        private string StatusToString(ServiceStatus status)
        {
            switch (status)
            {
                case ServiceStatus.Normal:
                    return AppPhrases.NormalSvcStatus;
                case ServiceStatus.Stopped:
                    return AppPhrases.StoppedSvcStatus;
                case ServiceStatus.Error:
                    return AppPhrases.ErrorSvcStatus;
                default: // ServiceStatus.Undefined
                    return AppPhrases.UndefinedSvcStatus;
            }
        }

        /// <summary>
        /// Gets the Server status asynchronously.
        /// </summary>
        private async Task GetServerStatusAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    txtServerStatus.Text = agentClient.GetServiceStatus(ServiceApp.Server, out ServiceStatus status) ?
                        StatusToString(status) : "---";
                    txtUpdateTime.Text = DateTime.Now.ToLocalizedString();
                }
                catch (Exception ex)
                {
                    txtServerStatus.Text = ex.Message;
                }
            });
        }

        /// <summary>
        /// Gets the Communicator status asynchronously.
        /// </summary>
        private async Task GetCommStatusAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    txtCommStatus.Text = agentClient.GetServiceStatus(ServiceApp.Comm, out ServiceStatus status) ?
                        StatusToString(status) : "---";
                    txtUpdateTime.Text = DateTime.Now.ToLocalizedString();
                }
                catch (Exception ex)
                {
                    txtCommStatus.Text = ex.Message;
                }
            });
        }

        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        private void ControlService(ServiceApp serviceApp, ServiceCommand command)
        {
            if (agentClient != null)
            {
                try
                {
                    if (agentClient.ControlService(serviceApp, command))
                        ScadaUiUtils.ShowInfo(AppPhrases.ServiceCommandComplete);
                    else
                        ScadaUiUtils.ShowError(AppPhrases.UnableControlService);
                }
                catch (Exception ex)
                {
                    appData.ProcError(ex, AppPhrases.ControlServiceError);
                }
            }
        }


        private void FrmInstanceStatus_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, ctrlProfileSelector.GetType().FullName);
            Translator.TranslateForm(this, GetType().FullName);

            if (ScadaUtils.IsRunningOnMono)
                ctrlProfileSelector.Width = gbStatus.Width;

            ProfileChanged = false;
            ConnSettingsModified = false;

            ctrlProfileSelector.Init(appData, deploymentSettings, instance);
            initialProfile = ctrlProfileSelector.SelectedProfile;
            initialConnSettings = initialProfile?.ConnectionSettings.Clone();
            agentClient = null;

            if (ctrlProfileSelector.SelectedProfile != null)
                Connect();
        }

        private void FrmInstanceStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
            ConnSettingsModified = !ProfileChanged &&
                !ConnectionSettings.Equals(initialConnSettings, initialProfile?.ConnectionSettings);
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            Disconnect();
            gbAction.Enabled = ctrlProfileSelector.SelectedProfile != null;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile != null)
            {
                instance.DeploymentProfile = profile.Name;
                ProfileChanged = initialProfile != profile;
                Connect();
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            ControlService(ServiceApp.Server, ServiceCommand.Start);
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            ControlService(ServiceApp.Server, ServiceCommand.Stop);
        }

        private void btnRestartServer_Click(object sender, EventArgs e)
        {
            ControlService(ServiceApp.Server, ServiceCommand.Restart);
        }

        private void btnStartComm_Click(object sender, EventArgs e)
        {
            ControlService(ServiceApp.Comm, ServiceCommand.Start);
        }

        private void btnStopComm_Click(object sender, EventArgs e)
        {
            ControlService(ServiceApp.Comm, ServiceCommand.Stop);
        }

        private void btnRestartComm_Click(object sender, EventArgs e)
        {
            ControlService(ServiceApp.Comm, ServiceCommand.Restart);
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            // initialize a client
            if (agentClient == null)
            {
                DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;
                if (profile != null)
                {
                    ConnectionSettings connSettings = profile.ConnectionSettings.Clone();
                    connSettings.ScadaInstance = instance.Name;
                    agentClient = new AgentWcfClient(connSettings);
                }
            }

            // request status
            if (agentClient != null)
            {
                await GetServerStatusAsync();
                await GetCommStatusAsync();

                if (agentClient != null)
                    timer.Start();
            }
        }
    }
}
