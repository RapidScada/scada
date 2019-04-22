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
 * Summary  : Form for editing a deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Agent.Connector;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Form for editing a deployment profile.
    /// <para>Форма для редактирования профиля развёртывания.</para>
    /// </summary>
    public partial class FrmProfileEdit : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmProfileEdit()
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
                txtProfileName.Text = Profile.Name;
                txtWebUrl.Text = Profile.WebUrl;

                ConnectionSettings connSettings = Profile.ConnectionSettings;
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
                Profile.Name = txtProfileName.Text.Trim();
                Profile.WebUrl = txtWebUrl.Text.Trim();

                ConnectionSettings connSettings = Profile.ConnectionSettings;
                connSettings.Host = txtHost.Text.Trim();
                connSettings.Port = (int)numPort.Value;
                connSettings.Username = txtUsername.Text.Trim();
                connSettings.Password = txtPassword.Text;
                connSettings.SecretKey = ScadaUtils.HexToBytes(txtSecretKey.Text.Trim());
            }
        }

        /// <summary>
        /// Validates the form fields.
        /// </summary>
        private bool ValidateFields()
        {
            StringBuilder sbError = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtProfileName.Text))
                sbError.AppendError(lblProfileName, CommonPhrases.NonemptyRequired);

            if (!string.IsNullOrWhiteSpace(txtWebUrl.Text) && !ScadaUtils.IsValidUrl(txtWebUrl.Text))
                sbError.AppendError(lblWebUrl, AppPhrases.ValidUrlRequired);

            if (string.IsNullOrWhiteSpace(txtHost.Text))
                sbError.AppendError(lblHost, CommonPhrases.NonemptyRequired);

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
                sbError.AppendError(lblUsername, CommonPhrases.NonemptyRequired);

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
                sbError.AppendError(lblPassword, CommonPhrases.NonemptyRequired);

            if (string.IsNullOrWhiteSpace(txtSecretKey.Text))
                sbError.AppendError(lblSecretKey, CommonPhrases.NonemptyRequired);

            if (sbError.Length > 0)
            {
                sbError.Insert(0, AppPhrases.CorrectErrors + Environment.NewLine);
                ScadaUiUtils.ShowError(sbError.ToString());
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


        private void FrmProfileEdit_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName, toolTip);
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
            if (ValidateFields())
            {
                ControlsToSettings();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
