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
 * Summary  : Form for selecting an instance deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Agent.Connector;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Form for selecting an instance deployment profile.
    /// <para>Форма для выбора профиля развёртывания экземпляра.</para>
    /// </summary>
    public partial class FrmInstanceProfile : Form
    {
        private readonly AppData appData;      // the common data of the application
        private readonly ScadaProject project; // the project under development
        private readonly Instance instance;    // the affected instance
        private bool profileChanged;           // the selected profile is changed


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInstanceProfile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInstanceProfile(AppData appData, ScadaProject project, Instance instance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            this.project = project ?? throw new ArgumentNullException("project");
            this.instance = instance ?? throw new ArgumentNullException("instance");
        }


        private void FrmInstanceProfile_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Admin.App.Controls.Deployment.CtrlProfileSelector");
            Translator.TranslateForm(this, "Scada.Admin.App.Forms.Deployment.FrmInstanceProfile");

            if (ScadaUtils.IsRunningOnMono)
                ctrlProfileSelector.Width = btnClose.Right - ctrlProfileSelector.Left;

            ctrlProfileSelector.Init(appData, project.DeploymentSettings, instance);
            profileChanged = false;
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            profileChanged = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            // test connection
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile != null)
            {
                ConnectionSettings connSettings = profile.ConnectionSettings.Clone();
                connSettings.ScadaInstance = instance.Name;
                IAgentClient agentClient = new AgentWcfClient(connSettings);

                if (agentClient.GetServiceStatus(ServiceApp.Server, out ServiceStatus serviceStatus))
                    ScadaUiUtils.ShowInfo("OK"); // TODO: message
                else
                    ScadaUiUtils.ShowError("Error");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // save the settings changes
            if (profileChanged && !project.DeploymentSettings.Save(out string errMsg))
                appData.ProcError(errMsg);
        }
    }
}
