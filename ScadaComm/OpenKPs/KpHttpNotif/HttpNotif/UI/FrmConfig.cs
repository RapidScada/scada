/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : KpHttpNotif
 * Summary  : Represents a device configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Devices.AB;
using Scada.Comm.Devices.HttpNotif.Config;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Devices.HttpNotif.UI
{
    /// <summary>
    /// Represents a device configuration form.
    /// <para>Представляет форму конфигурации КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private readonly AppDirs appDirs;     // the application directories
        private readonly int kpNum;           // the device number
        private readonly DeviceConfig config; // the device configuration
        private string configFileName;    // the configuration file name
        private bool modified;            // the configuration was modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConfig(AppDirs appDirs, int kpNum)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.kpNum = kpNum;
            config = new DeviceConfig();
            configFileName = "";
            modified = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the configuration was modified.
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
            }
        }

        /// <summary>
        /// Gets or sets the default request URI.
        /// </summary>
        public string DefaultUri { get; set; }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            cbMethod.SelectedIndex = (int)config.Method;
            txtUri.Text = string.IsNullOrEmpty(config.Uri) ? DefaultUri : config.Uri;
            chkParamEnabled.Checked = config.ParamEnabled;
            txtParamBegin.Text = config.ParamBegin.ToString();
            txtParamEnd.Text = config.ParamEnd.ToString();
            cbContentType.Text = config.ContentType;
            cbContentEscaping.SelectedIndex = (int)config.ContentEscaping;
            txtContent.Text = config.Content;
            dgvHeaders.DataSource = new BindingList<Header>(config.Headers);
        }

        /// <summary>
        /// Sets the configuration parameters according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            config.Method = (RequestMethod)cbMethod.SelectedIndex;
            config.Uri = txtUri.Text;
            config.ParamEnabled = chkParamEnabled.Checked;
            config.SetParamBegin(txtParamBegin.Text);
            config.SetParamEnd(txtParamEnd.Text);
            config.ContentType = cbContentType.Text;
            config.ContentEscaping = (EscapingMethod)cbContentEscaping.SelectedIndex;
            config.Content = txtContent.Text;
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            // translate the form
            if (Localization.LoadDictionaries(appDirs.LangDir, "KpHttpNotif", out string errMsg))
                Translator.TranslateForm(this, GetType().FullName);
            else
                ScadaUiUtils.ShowError(errMsg);

            Text = string.Format(Text, kpNum);

            // load a configuration
            configFileName = DeviceConfig.GetFileName(appDirs.ConfigDir, kpNum);

            if (File.Exists(configFileName) && !config.Load(configFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // display the configuration
            ConfigToControls();
            Modified = false;
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(CommPhrases.SaveKpSettingsConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!config.Save(configFileName, out string errMsg))
                        {
                            ScadaUiUtils.ShowError(errMsg);
                            e.Cancel = true;
                        }
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void control_Changed(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void dgvHeaders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                Modified = true;
        }

        private void dgvHeaders_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            Modified = true;
        }

        private void btnAddressBook_Click(object sender, EventArgs e)
        {
            FrmAddressBook.ShowDialog(appDirs);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // retrieve the configuration
            ControlsToConfig();

            // save the configuration
            if (config.Save(configFileName, out string errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
