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
 * Summary  : Form for synchronize Communicator settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Comm;
using Scada.Comm.Shell.Code;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Form for synchronize Communicator settings.
    /// <para>Форма синхронизации настроек Коммуникатора.</para>
    /// </summary>
    public partial class FrmCommSync : Form
    {
        private readonly ScadaProject project; // the project under development
        private readonly Instance instance;    // the import destination instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCommSync()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCommSync(ScadaProject project, Instance instance)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.instance = instance ?? throw new ArgumentNullException("instance");

            CommLineSettings = null;
        }


        /// <summary>
        /// Gets or sets the communication line to synchronize.
        /// </summary>
        public Settings.CommLine CommLineSettings { get; set; }


        /// <summary>
        /// Fills the combo box with the communication lines.
        /// </summary>
        private void FillCommLineList()
        {
            List<Settings.CommLine> commLines = new List<Settings.CommLine>(instance.CommApp.Settings.CommLines);
            commLines.Insert(0, new Settings.CommLine { Number = 0, Name = AppPhrases.AllCommLines });

            cbCommLine.ValueMember = "Number";
            cbCommLine.DisplayMember = "Name";
            cbCommLine.DataSource = commLines;
            cbCommLine.SelectedValue = CommLineSettings == null ? 0 : CommLineSettings.Number;
        }

        /// <summary>
        /// Synchronizes the communication line.
        /// </summary>
        private void SyncCommLine(Settings.CommLine commLineSettings)
        {
            if (project.ConfigBase.CommLineTable.Items.TryGetValue(commLineSettings.Number, 
                out CommLine commLineEntity))
            {
                commLineSettings.Name = commLineEntity.Name;
            }

            BaseTable<KP> kpTable = project.ConfigBase.KPTable;
            BaseTable<KPType> kpTypeTable = project.ConfigBase.KPTypeTable;

            foreach (Settings.KP kpSettings in commLineSettings.ReqSequence)
            {
                if (kpTable.Items.TryGetValue(kpSettings.Number, out KP kpEntity))
                {
                    SettingsConverter.Copy(kpEntity, kpSettings, kpTypeTable);
                }
            }
        }


        private void FrmCommSync_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            FillCommLineList();
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            CommLineSettings = cbCommLine.SelectedValue as Settings.CommLine;

            if (CommLineSettings == null)
            {
                // synchronize all lines
                foreach (Settings.CommLine commLineSettings in instance.CommApp.Settings.CommLines)
                {
                    SyncCommLine(commLineSettings);
                }
            }
            else
            {
                // synchronize selected line
                SyncCommLine(CommLineSettings);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
