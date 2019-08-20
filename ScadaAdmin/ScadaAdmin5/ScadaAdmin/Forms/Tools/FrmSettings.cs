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
 * Summary  : Application settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Config;
using Scada.UI;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Application settings form.
    /// <para>Форма настроек приложения.</para>
    /// </summary>
    public partial class FrmSettings : Form
    {
        private readonly AppData appData;        // the common data of the application
        private readonly AdminSettings settings; // the application settings

        private string serverDir; // the initial Server directory
        private string commDir;   // the initial Communicator directory
        private bool modified;    // the settings were modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSettings(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            settings = appData.AppSettings;

            serverDir = settings.ServerDir;
            commDir = settings.CommDir;
            modified = false;
        }


        /// <summary>
        /// Gets a value whether to reopen a project for the settings to take effect.
        /// </summary>
        public bool ReopenNeeded
        {
            get
            {
                return serverDir != settings.ServerDir || commDir != settings.CommDir;
            }
        }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        private void SettingsToControls()
        {
            txtServerDir.Text = settings.ServerDir;
            txtCommDir.Text = settings.CommDir;
            txtSchemeEditorPath.Text = settings.SchemeEditorPath;
            txtTableEditorPath.Text = settings.TableEditorPath;
            txtTextEditorPath.Text = settings.TextEditorPath;
            numCnlMult.SetValue(settings.CnlMult);
            numCnlShift.SetValue(settings.CnlShift);
            numCnlGap.SetValue(settings.CnlGap);
            modified = false;
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        private void ControlsToSettings()
        {
            settings.ServerDir = txtServerDir.Text;
            settings.CommDir = txtCommDir.Text;
            settings.SchemeEditorPath = txtSchemeEditorPath.Text;
            settings.TableEditorPath = txtTableEditorPath.Text;
            settings.TextEditorPath = txtTextEditorPath.Text;
            settings.CnlMult = Convert.ToInt32(numCnlMult.Value);
            settings.CnlShift = Convert.ToInt32(numCnlShift.Value);
            settings.CnlGap = Convert.ToInt32(numCnlGap.Value);
        }

        /// <summary>
        /// Shows a folder browser dialog to select a directory.
        /// </summary>
        private void SelectDirectory(TextBox textBox, string descr)
        {
            folderBrowserDialog.Description = descr;
            folderBrowserDialog.SelectedPath = textBox.Text.Trim();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBox.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);
        }

        /// <summary>
        /// Shows a file open dialog to select a file.
        /// </summary>
        private void SelectFile(TextBox textBox)
        {
            if (File.Exists(textBox.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(textBox.Text);
                openFileDialog.FileName = Path.GetFileName(textBox.Text);
            }
            else
            {
                openFileDialog.FileName = "";
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBox.Text = openFileDialog.FileName;
        }
        
        /// <summary>
        /// Validates the form fields.
        /// </summary>
        private bool ValidateFields()
        {
            // unacceptable errors
            StringBuilder sbError = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtServerDir.Text))
                sbError.AppendError(lblServerDir, CommonPhrases.NonemptyRequired);
            else if (!Directory.Exists(txtServerDir.Text))
                sbError.AppendError(lblServerDir, CommonPhrases.DirNotExists);

            if (string.IsNullOrWhiteSpace(txtCommDir.Text))
                sbError.AppendError(lblCommDir, CommonPhrases.NonemptyRequired);
            else if (!Directory.Exists(txtCommDir.Text))
                sbError.AppendError(lblCommDir, CommonPhrases.DirNotExists);

            if (sbError.Length > 0)
            {
                sbError.Insert(0, AppPhrases.CorrectErrors + Environment.NewLine);
                ScadaUiUtils.ShowError(sbError.ToString());
                return false;
            }

            // warnings
            StringBuilder sbWarn = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(txtSchemeEditorPath.Text) && !File.Exists(txtSchemeEditorPath.Text))
                sbWarn.AppendError(lblSchemeEditorPath, CommonPhrases.FileNotFound);

            if (!string.IsNullOrWhiteSpace(txtTableEditorPath.Text) && !File.Exists(txtTableEditorPath.Text))
                sbWarn.AppendError(lblTableEditorPath, CommonPhrases.FileNotFound);

            if (!string.IsNullOrWhiteSpace(txtTextEditorPath.Text) && !File.Exists(txtTextEditorPath.Text))
                sbWarn.AppendError(lblTextEditorPath, CommonPhrases.FileNotFound);

            if (sbWarn.Length > 0)
                ScadaUiUtils.ShowWarning(sbWarn.ToString());

            return true;
        }


        private void FrmSettings_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            openFileDialog.SetFilter(AppPhrases.ExecutableFileFilter);
            SettingsToControls();

            if (!ScadaUtils.IsRunningOnWin)
                openFileDialog.FilterIndex = 2; // all files
        }

        private void btnBrowseServerDir_Click(object sender, EventArgs e)
        {
            SelectDirectory(txtServerDir, AppPhrases.ChooseServerDir);
        }

        private void btnBrowseCommDir_Click(object sender, EventArgs e)
        {
            SelectDirectory(txtCommDir, AppPhrases.ChooseCommDir);
        }

        private void btnBrowseSchemeEditorPath_Click(object sender, EventArgs e)
        {
            SelectFile(txtSchemeEditorPath);
        }

        private void btnBrowseTableEditorPath_Click(object sender, EventArgs e)
        {
            SelectFile(txtTableEditorPath);
        }

        private void btnBrowseTextEditorPath_Click(object sender, EventArgs e)
        {
            SelectFile(txtTextEditorPath);
        }

        private void control_Changed(object sender, EventArgs e)
        {
            modified = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                if (modified)
                {
                    ControlsToSettings();

                    if (!settings.Save(Path.Combine(appData.AppDirs.ConfigDir, AdminSettings.DefFileName), 
                        out string errMsg))
                    {
                        appData.ProcError(errMsg);
                        return;
                    }
                }

                DialogResult = DialogResult.OK;
            }
        }
    }
}
