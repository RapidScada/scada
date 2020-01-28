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
 * Module   : Administrator
 * Summary  : Application settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2020
 */

using Scada.Admin.App.Code;
using Scada.Admin.Config;
using Scada.UI;
using System;
using System.Collections.Generic;
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
            colExt.Name = "colExt";
            colPath.Name = "colPath";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSettings(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            settings = appData.AppSettings;

            serverDir = settings.PathOptions.ServerDir;
            commDir = settings.PathOptions.CommDir;
            modified = false;
        }


        /// <summary>
        /// Gets a value whether to reopen a project for the settings to take effect.
        /// </summary>
        public bool ReopenNeeded
        {
            get
            {
                return serverDir != settings.PathOptions.ServerDir || 
                    commDir != settings.PathOptions.CommDir;
            }
        }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        private void SettingsToControls()
        {
            PathOptions pathOptions = settings.PathOptions;
            txtServerDir.Text = pathOptions.ServerDir;
            txtCommDir.Text = pathOptions.CommDir;

            ChannelOptions channelOptions = settings.ChannelOptions;
            numCnlMult.SetValue(channelOptions.CnlMult);
            numCnlShift.SetValue(channelOptions.CnlShift);
            numCnlGap.SetValue(channelOptions.CnlGap);
            chkPrependDeviceName.Checked = channelOptions.PrependDeviceName;

            try
            {
                lvAssociations.BeginUpdate();
                lvAssociations.Items.Clear();

                foreach (KeyValuePair<string, string> pair in settings.FileAssociations)
                {
                    lvAssociations.Items.Add(CreateAssociationItem(pair.Key, pair.Value));
                }

                if (lvAssociations.Items.Count > 0)
                    lvAssociations.Items[0].Selected = true;
            }
            finally
            {
                lvAssociations.EndUpdate();
            }

            modified = false;
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        private void ControlsToSettings()
        {
            PathOptions pathOptions = settings.PathOptions;
            pathOptions.ServerDir = txtServerDir.Text;
            pathOptions.CommDir = txtCommDir.Text;

            ChannelOptions channelOptions = settings.ChannelOptions;
            channelOptions.CnlMult = Convert.ToInt32(numCnlMult.Value);
            channelOptions.CnlShift = Convert.ToInt32(numCnlShift.Value);
            channelOptions.CnlGap = Convert.ToInt32(numCnlGap.Value);
            channelOptions.PrependDeviceName = chkPrependDeviceName.Checked;

            SortedList<string, string> fileAssociations = settings.FileAssociations;
            fileAssociations.Clear();

            foreach (ListViewItem item in lvAssociations.Items)
            {
                string ext = item.SubItems[0].Text;
                string path = item.SubItems[1].Text;
                fileAssociations[ext] = path;
            }
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
        /// Creates a new list view item represents a file association.
        /// </summary>
        private ListViewItem CreateAssociationItem(string ext, string path, bool selected = false)
        {
            return new ListViewItem(new string[] { ext, path }) { Selected = selected };
        }

        /// <summary>
        /// Enables or disables the file association buttons.
        /// </summary>
        private void SetAssociationButtonsEnabled()
        {
            btnEditAssociation.Enabled = btnDeleteAssociation.Enabled =
                lvAssociations.SelectedItems.Count > 0;
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

            foreach (ListViewItem item in lvAssociations.Items)
            {
                string path = item.SubItems[1].Text;
                if (!string.IsNullOrWhiteSpace(path) && !File.Exists(path))
                    sbWarn.AppendLine(string.Format(CommonPhrases.NamedFileNotFound, path));
            }

            if (sbWarn.Length > 0)
                ScadaUiUtils.ShowWarning(sbWarn.ToString());

            return true;
        }


        private void FrmSettings_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            SettingsToControls();
            SetAssociationButtonsEnabled();
        }

        private void btnBrowseServerDir_Click(object sender, EventArgs e)
        {
            SelectDirectory(txtServerDir, AppPhrases.ChooseServerDir);
        }

        private void btnBrowseCommDir_Click(object sender, EventArgs e)
        {
            SelectDirectory(txtCommDir, AppPhrases.ChooseCommDir);
        }

        private void btnAddAssociation_Click(object sender, EventArgs e)
        {
            // add a new file association
            FrmFileAssociation form = new FrmFileAssociation();

            if (form.ShowDialog() == DialogResult.OK)
            {
                lvAssociations.Items.Add(CreateAssociationItem(form.FileExtension, form.ExecutablePath, true));
                lvAssociations.Focus();
                modified = true;
            }
        }

        private void btnEditAssociation_Click(object sender, EventArgs e)
        {
            // edit the selected file association
            if (lvAssociations.SelectedItems.Count > 0)
            {
                ListViewItem item = lvAssociations.SelectedItems[0];
                FrmFileAssociation form = new FrmFileAssociation
                {
                    FileExtension = item.SubItems[0].Text,
                    ExecutablePath = item.SubItems[1].Text
                };

                if (form.ShowDialog() == DialogResult.OK)
                {
                    lvAssociations.Items.RemoveAt(item.Index);
                    lvAssociations.Items.Add(CreateAssociationItem(form.FileExtension, form.ExecutablePath, true));
                    lvAssociations.Focus();
                    modified = true;
                }
            }
        }

        private void btnDeleteAssociation_Click(object sender, EventArgs e)
        {
            // delete the selected file association
            if (lvAssociations.SelectedItems.Count > 0)
            {
                int index = lvAssociations.SelectedIndices[0];
                lvAssociations.Items.RemoveAt(index);

                if (lvAssociations.Items.Count > 0)
                {
                    index = Math.Min(index, lvAssociations.Items.Count - 1);
                    lvAssociations.Items[index].Selected = true;
                }

                lvAssociations.Focus();
                modified = true;
            }
        }

        private void lvAssociations_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAssociationButtonsEnabled();
        }

        private void lvAssociations_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnEditAssociation_Click(null, null);
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
