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
 * Module   : KpOpcUa
 * Summary  : Security options form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Opc.Ua;
using Scada.Comm.Devices.OpcUa.Config;
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

namespace Scada.Comm.Devices.OpcUa.UI
{
    /// <summary>
    /// Security options form.
    /// <para>Форма параметров безопасности.</para>
    /// </summary>
    public partial class FrmSecurityOptions : Form
    {
        private readonly ConnectionOptions connectionOptions; // the OPC server connection options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmSecurityOptions()
        {
            InitializeComponent();
            FillComboBoxes();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSecurityOptions(ConnectionOptions connectionOptions)
            : this()
        {
            this.connectionOptions = connectionOptions;
        }


        /// <summary>
        /// Fills the combo boxes.
        /// </summary>
        private void FillComboBoxes()
        {
            cbSecurityMode.Items.AddRange(new object[] {
                MessageSecurityMode.None,
                MessageSecurityMode.Sign,
                MessageSecurityMode.SignAndEncrypt });

            cbSecurityPolicy.Items.AddRange(new object[] {
                SecurityPolicy.None,
                SecurityPolicy.Basic128Rsa15,
                SecurityPolicy.Basic256,
                SecurityPolicy.Basic256Sha256,
                SecurityPolicy.Aes128_Sha256_RsaOaep,
                SecurityPolicy.Aes256_Sha256_RsaPss,
                SecurityPolicy.Https });

            cbAuthenticationMode.Items.AddRange(new object[] {
                AuthenticationMode.Anonymous,
                AuthenticationMode.Username });
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            cbSecurityMode.SelectedIndex = (int)connectionOptions.SecurityMode - 1;
            cbSecurityPolicy.SelectedIndex = (int)connectionOptions.SecurityPolicy;
            cbAuthenticationMode.SelectedIndex = (int)connectionOptions.AuthenticationMode;
            txtUsername.Text = connectionOptions.Username;
            txtPassword.Text = connectionOptions.Password;
        }

        /// <summary>
        /// Sets the configuration parameters according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            connectionOptions.SecurityMode = (MessageSecurityMode)(cbSecurityMode.SelectedIndex + 1);
            connectionOptions.SecurityPolicy = (SecurityPolicy)cbSecurityPolicy.SelectedIndex;
            connectionOptions.AuthenticationMode = (AuthenticationMode)cbAuthenticationMode.SelectedIndex;
            connectionOptions.Username = txtUsername.Text;
            connectionOptions.Password = txtPassword.Text;
        }


        private void FrmSecurityOptions_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            ConfigToControls();
        }

        private void cbSecurityMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fix the security policy
            if (cbSecurityMode.SelectedIndex == 0)
                cbSecurityPolicy.SelectedIndex = 0;
            else if (cbSecurityPolicy.SelectedIndex == 0)
                cbSecurityPolicy.SelectedIndex = 1;
        }

        private void cbAuthenticationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlUsername.Enabled = cbAuthenticationMode.SelectedIndex > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToConfig();
            DialogResult = DialogResult.OK;
        }
    }
}
