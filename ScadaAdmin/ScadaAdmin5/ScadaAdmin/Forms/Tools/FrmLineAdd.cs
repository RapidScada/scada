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
 * Summary  : Form for adding a communication line
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
using Scada.UI;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Form for adding a communication line.
    /// <para>Форма для добавления линии связи.</para>
    /// </summary>
    public partial class FrmLineAdd : Form
    {
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLineAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLineAdd(ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.recentSelection = recentSelection ?? throw new ArgumentNullException("recentSelection");
            InstanceName = "";
            CommLineSettings = null;

            numCommLineNum.Maximum = ushort.MaxValue;
            txtName.MaxLength = ColumnLength.Name;
            txtDescr.MaxLength = ColumnLength.Description;
        }


        /// <summary>
        /// Gets the name of the instance affected in Communicator.
        /// </summary>
        public string InstanceName { get; private set; }

        /// <summary>
        /// Gets a communication line added to Communicator.
        /// </summary>
        public Settings.CommLine CommLineSettings { get; private set; }


        /// <summary>
        /// Fills the combo box with the instances.
        /// </summary>
        private void FillInstanceList()
        {
            cbInstance.DisplayMember = "Name";
            cbInstance.ValueMember = "Name";
            cbInstance.DataSource = project.Instances;

            try
            {
                if (!string.IsNullOrEmpty(recentSelection.InstanceName))
                    cbInstance.SelectedValue = recentSelection.InstanceName;
            }
            catch
            {
                cbInstance.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Sets the communication line number by default.
        /// </summary>
        private void SetCommLineNum()
        {
            if (recentSelection.CommLineNum > 0)
                numCommLineNum.SetValue(recentSelection.CommLineNum + 1);
            else if (project.ConfigBase.CommLineTable.ItemCount > 0)
                numCommLineNum.SetValue(project.ConfigBase.CommLineTable.Items.Values.Last().CommLineNum + 1);
        }

        /// <summary>
        /// Validates the form fields.
        /// </summary>
        private bool ValidateFields()
        {
            StringBuilder sbError = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtName.Text))
                sbError.AppendError(lblName, CommonPhrases.NonemptyRequired);

            if (chkAddToComm.Checked && cbInstance.SelectedItem == null)
                sbError.AppendError(lblInstance, CommonPhrases.NonemptyRequired);

            if (sbError.Length > 0)
            {
                sbError.Insert(0, AppPhrases.CorrectErrors + Environment.NewLine);
                ScadaUiUtils.ShowError(sbError.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks feasibility of adding a line.
        /// </summary>
        private bool CheckFeasibility()
        {
            if (chkAddToComm.Checked && cbInstance.SelectedItem is Instance instance && instance.CommApp.Enabled)
            {
                // load instance settings
                if (!instance.LoadAppSettings(out string errMsg))
                {
                    ScadaUiUtils.ShowError(errMsg);
                    return false;
                }
            }

            if (project.ConfigBase.CommLineTable.PkExists(Convert.ToInt32(numCommLineNum.Value)))
            {
                ScadaUiUtils.ShowError(AppPhrases.LineAlreadyExists);
                return false;
            }

            return true;
        }


        private void FrmLineAdd_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            FillInstanceList();
            SetCommLineNum();
            txtName.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateFields() && CheckFeasibility())
            {
                // create a new communication line
                CommLine commLineEntity = new CommLine
                {
                    CommLineNum = Convert.ToInt32(numCommLineNum.Value),
                    Name = txtName.Text,
                    Descr = txtDescr.Text
                };

                // insert the line in the configuration database
                project.ConfigBase.CommLineTable.AddItem(commLineEntity);
                project.ConfigBase.CommLineTable.Modified = true;

                // insert the line in the Communicator settings
                if (chkAddToComm.Checked && cbInstance.SelectedItem is Instance instance)
                {
                    if (instance.CommApp.Enabled)
                    {
                        CommLineSettings = SettingsConverter.CreateCommLine(commLineEntity);
                        CommLineSettings.Parent = instance.CommApp.Settings;
                        instance.CommApp.Settings.CommLines.Add(CommLineSettings);
                    }

                    InstanceName = recentSelection.InstanceName = instance.Name;
                }

                recentSelection.CommLineNum = commLineEntity.CommLineNum;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
