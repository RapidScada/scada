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
 * Summary  : Connection settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Agent.Connector;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Connection settings form.
    /// <para>Форма настроек соединения.</para>
    /// </summary>
    public partial class FrmConnSettings : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConnSettings()
        {
            InitializeComponent();
            Profile = null;
            ExistingProfileNames = null;
        }


        /// <summary>
        /// Gets or sets the edited deployment profile.
        /// </summary>
        public DeploymentProfile Profile { get; set; }

        /// <summary>
        /// Gets or sets the names of the existing profiles.
        /// </summary>
        public HashSet<string> ExistingProfileNames { get; set; }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        private void SettingsToControls()
        {
            if (Profile != null)
            {
                ConnectionSettings connSettings = Profile.ConnectionSettings;
                txtProfileName.Text = Profile.Name;
                txtHost.Text = connSettings.Host;
                numPort.SetValue(connSettings.Port);
                txtUsername.Text = connSettings.Username;
                txtPassword.Text = connSettings.Password;
                txtSecretKey.Text = ScadaUtils.BytesToHex(connSettings.SecretKey);
            }
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        private void ControlsToSettings()
        {
            if (Profile != null)
            {
                ConnectionSettings connSettings = Profile.ConnectionSettings;
                Profile.Name = txtProfileName.Text.Trim();
                connSettings.Host = txtHost.Text.Trim();
                connSettings.Port = (int)numPort.Value;
                connSettings.Username = txtUsername.Text.Trim();
                connSettings.Password = txtPassword.Text;
                connSettings.SecretKey = ScadaUtils.HexToBytes(txtSecretKey.Text.Trim());
            }
        }
        
        /// <summary>
        /// Validates the values of the controls.
        /// </summary>
        private bool ValidateControls()
        {
            if (string.IsNullOrWhiteSpace(txtProfileName.Text) ||
                string.IsNullOrWhiteSpace(txtHost.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtSecretKey.Text))
            {
                ScadaUiUtils.ShowError(AppPhrases.EmptyFieldsNotAllowed);
                return false;
            }

            if (ExistingProfileNames != null && ExistingProfileNames.Contains(txtProfileName.Text.Trim()))
            {
                ScadaUiUtils.ShowError(AppPhrases.ProfileNameDuplicated);
                return false;
            }

            if (!ScadaUtils.HexToBytes(txtSecretKey.Text.Trim(), out byte[] bytes))
            {
                ScadaUiUtils.ShowError(AppPhrases.IncorrectSecretKey);
                return false;
            }

            return true;
        }


        private void FrmConnSettings_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Admin.App.Forms.Deployment.FrmConnSettings", toolTip);
            SettingsToControls();
        }

        private void btnGenSecretKey_Click(object sender, EventArgs e)
        {
            // generate a secret key
            txtSecretKey.Text = ScadaUtils.BytesToHex(ScadaUtils.GetRandomBytes(ScadaUtils.SecretKeySize));
            txtSecretKey.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                ControlsToSettings();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
