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
 * Module   : Server Shell
 * Summary  : Form for editing common parameters of Server settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Windows.Forms;
using WinControl;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for editing common parameters of Server settings.
    /// <para>Форма редактирования общих параметров Сервера.</para>
    /// </summary>
    public partial class FrmCommonParams : Form, IChildForm
    {
        private readonly Settings settings; // the application settings
        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCommonParams()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCommonParams(Settings settings)
            : this()
        {
            this.settings = settings ?? throw new ArgumentNullException("settings");
            changing = false;
        }
        
        
        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        private void SettingsToControls()
        {
            changing = true;

            // connection
            numTcpPort.SetValue(settings.TcpPort);
            chkUseAD.Checked = settings.UseAD;
            txtLdapPath.Text = settings.LdapPath;

            // directories
            txtBaseDATDir.Text = settings.BaseDATDir;
            txtItfDir.Text = settings.ItfDir;
            txtArcDir.Text = settings.ArcDir;
            txtArcCopyDir.Text = settings.ArcCopyDir;

            // logging
            chkDetailedLog.Checked = settings.DetailedLog;

            changing = false;
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        private void ControlsToSettings()
        {
            // connection
            settings.TcpPort = decimal.ToInt32(numTcpPort.Value);
            settings.UseAD = chkUseAD.Checked;
            settings.LdapPath = txtLdapPath.Text;

            // directories
            settings.BaseDATDir = txtBaseDATDir.Text;
            settings.ItfDir = txtItfDir.Text;
            settings.ArcDir = txtArcDir.Text;
            settings.ArcCopyDir = txtArcCopyDir.Text;

            // logging
            settings.DetailedLog = chkDetailedLog.Checked;
        }

        /// <summary>
        /// Sets the default directories.
        /// </summary>
        private void SetDirectoriesToDefault(bool windows)
        {
            string scadaDir = windows ? @"C:\SCADA\" : "/opt/scada/";
            string sepChar = windows ? "\\" : "/";

            txtBaseDATDir.Text = scadaDir + "BaseDAT" + sepChar;
            txtItfDir.Text = scadaDir + "Interface" + sepChar;
            txtArcDir.Text = scadaDir + "ArchiveDAT" + sepChar;
            txtArcCopyDir.Text = scadaDir + "ArchiveDATCopy" + sepChar;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            ControlsToSettings();

            if (ChildFormTag.SendMessage(this, ServerMessage.SaveSettings))
                ChildFormTag.Modified = false;
        }


        private void FrmCommonParams_Load(object sender, System.EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            SettingsToControls();
        }

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                ChildFormTag.Modified = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // choose a text box
            TextBox textBox = null;
            string descr = "";

            if (sender == btnBrowseBaseDATDir)
            {
                textBox = txtBaseDATDir;
                descr = CommonPhrases.ChooseBaseDATDir;
            }
            else if (sender == btnBrowseItfDir)
            {
                textBox = txtItfDir;
                descr = ServerShellPhrases.ChooseItfDir;
            }
            else if (sender == btnBrowseArcDir)
            {
                textBox = txtArcDir;
                descr = ServerShellPhrases.ChooseArcDir;
            }
            else if (sender == btnBrowseArcCopyDir)
            {
                textBox = txtArcCopyDir;
                descr = ServerShellPhrases.ChooseArcCopyDir;
            }

            // browse directory
            if (textBox != null)
            {
                fbdDir.SelectedPath = textBox.Text.Trim();
                fbdDir.Description = descr;

                if (fbdDir.ShowDialog() == DialogResult.OK)
                    textBox.Text = ScadaUtils.NormalDir(fbdDir.SelectedPath);
            }
        }

        private void btnSetToDefault_Click(object sender, EventArgs e)
        {
            SetDirectoriesToDefault(sender == btnSetToDefaultWin);
        }
    }
}
