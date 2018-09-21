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
 * Module   : Administrator
 * Summary  : Download configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Agent.Connector;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Download configuration form.
    /// <para>Форма скачивания конфигурации.</para>
    /// </summary>
    public partial class FrmDownloadConfig : Form
    {
        private readonly AppData appData;      // the common data of the application
        private readonly ScadaProject project; // the project under development
        private readonly Instance instance;    // the affected instance
        private bool downloadSettingsModified; // the selected download settings were modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDownloadConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDownloadConfig(AppData appData, ScadaProject project, Instance instance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            this.project = project ?? throw new ArgumentNullException("project");
            this.instance = instance ?? throw new ArgumentNullException("instance");
        }


        /// <summary>
        /// Gets a value indicating whether the configuration database was modified
        /// </summary>
        public bool BaseModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the interface files were modified
        /// </summary>
        public bool InterfaceModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the instance was modified
        /// </summary>
        public bool InstanceModified { get; protected set; }
        
        
        /// <summary>
        /// Validate the download configuration settings.
        /// </summary>
        private bool ValidateDownloadSettings()
        {
            if (ctrlTransferSettings.Empty)
            {
                ScadaUiUtils.ShowError(AppPhrases.NothingToDownload);
                return false;
            }
            else
            {
                return true;
            }
        }
        
        /// <summary>
        /// Save the deployments settings.
        /// </summary>
        private void SaveDeploymentSettings()
        {
            if (!project.DeploymentSettings.Save(out string errMsg))
                appData.ProcError(errMsg);
        }

        /// <summary>
        /// Gets a name for a temporary file.
        /// </summary>
        private string GetTempFileName()
        {
            return Path.Combine(appData.AppDirs.TempDir,
                string.Format("download-config_{0}.zip", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
        }

        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        private bool DownloadConfig(DeploymentProfile profile)
        {
            string configFileName = GetTempFileName();

            try
            {
                Cursor = Cursors.WaitCursor;
                DateTime t0 = DateTime.UtcNow;

                // download the configuration
                ConnectionSettings connSettings = profile.ConnectionSettings.Clone();
                connSettings.ScadaInstance = instance.Name;
                AgentWcfClient agentClient = new AgentWcfClient(connSettings);
                agentClient.DownloadConfig(configFileName, profile.DownloadSettings.ToConfigOpions());

                // import the configuration
                ImportExport importExport = new ImportExport();
                importExport.ImportArchive(configFileName, project, instance, out ConfigParts foundConfigParts);
                FileInfo configFileInfo = new FileInfo(configFileName);
                long configFileSize = configFileInfo.Length;

                // set the modification flags
                BaseModified = foundConfigParts.HasFlag(ConfigParts.Base);
                InterfaceModified = foundConfigParts.HasFlag(ConfigParts.Interface);
                InstanceModified = foundConfigParts.HasFlag(ConfigParts.Server) ||
                    foundConfigParts.HasFlag(ConfigParts.Comm) || foundConfigParts.HasFlag(ConfigParts.Web);

                // show result
                Cursor = Cursors.Default;
                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.DownloadConfigComplete,
                    Math.Round((DateTime.UtcNow - t0).TotalSeconds), configFileSize));
                return true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                appData.ProcError(ex, AppPhrases.DownloadConfigError);
                return false;
            }
            finally
            {
                // delete temporary file
                try { File.Delete(configFileName); }
                catch { }
            }
        }


        private void FrmDownloadConfig_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Admin.App.Controls.Deployment.CtrlProfileSelector");
            Translator.TranslateForm(this, "Scada.Admin.App.Controls.Deployment.CtrlTransferSettings");
            Translator.TranslateForm(this, "Scada.Admin.App.Forms.Deployment.FrmDownloadConfig");

            BaseModified = false;
            InterfaceModified = false;
            InstanceModified = false;

            downloadSettingsModified = false;
            ctrlTransferSettings.Disable();
            ctrlProfileSelector.Init(appData, project.DeploymentSettings, instance);
        }

        private void ctrlProfileSelector_SelectedProfileChanged(object sender, EventArgs e)
        {
            // display download settings of the selected profile
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile == null)
            {
                ctrlTransferSettings.Disable();
                btnDownload.Enabled = false;
            }
            else
            {
                ctrlTransferSettings.SettingsToControls(profile.DownloadSettings);
                btnDownload.Enabled = true;
            }

            downloadSettingsModified = false;
        }

        private void ctrlTransferSettings_SettingsChanged(object sender, EventArgs e)
        {
            downloadSettingsModified = true;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // validate settings and download
            DeploymentProfile profile = ctrlProfileSelector.SelectedProfile;

            if (profile != null && ValidateDownloadSettings())
            {
                // save the settings changes
                if (downloadSettingsModified)
                {
                    ctrlTransferSettings.ControlsToSettings(profile.DownloadSettings);
                    SaveDeploymentSettings();
                }

                // download
                instance.DeploymentProfile = profile.Name;
                if (DownloadConfig(profile))
                    DialogResult = DialogResult.OK;
            }
        }
    }
}
