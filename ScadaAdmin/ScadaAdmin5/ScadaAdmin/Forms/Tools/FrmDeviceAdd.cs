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
 * Summary  : Form for adding a device
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
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Form for adding a device.
    /// <para>Форма для добавления КП.</para>
    /// </summary>
    public partial class FrmDeviceAdd : Form
    {
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceAdd(ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.recentSelection = recentSelection ?? throw new ArgumentNullException("recentSelection");
            InstanceName = "";
            KPSettings = null;
            CommLineSettings = null;

            numKPNum.Maximum = ushort.MaxValue;
            txtName.MaxLength = ColumnLength.Name;
            txtCallNum.MaxLength = ColumnLength.Default;
            txtDescr.MaxLength = ColumnLength.Description;
        }


        /// <summary>
        /// Gets the name of the instance affected in Communicator.
        /// </summary>
        public string InstanceName { get; private set; }

        /// <summary>
        /// Gets a device added to Communicator.
        /// </summary>
        public Settings.KP KPSettings { get; private set; }

        /// <summary>
        /// Gets a communication line of the device added to Communicator.
        /// </summary>
        public Settings.CommLine CommLineSettings { get; private set; }


        /// <summary>
        /// Fills the combo box with the device types.
        /// </summary>
        private void FillDeviceTypeList()
        {
            DataTable kpTypeTable = project.ConfigBase.KPTypeTable.ToDataTable();
            kpTypeTable.AddEmptyRow();
            kpTypeTable.DefaultView.Sort = "KPTypeID";

            cbKPType.DisplayMember = "Name";
            cbKPType.ValueMember = "KPTypeID";
            cbKPType.DataSource = kpTypeTable;

            try
            {
                cbKPType.SelectedValue = recentSelection.KPTypeID;
            }
            catch
            {
                cbKPType.SelectedValue = 0;
            }
        }

        /// <summary>
        /// Fills the combo box with the communication lines.
        /// </summary>
        private void FillCommLineList()
        {
            DataTable commLineTable = project.ConfigBase.CommLineTable.ToDataTable();
            commLineTable.AddEmptyRow();
            commLineTable.DefaultView.Sort = "CommLineNum";

            cbCommLine.DisplayMember = "Name";
            cbCommLine.ValueMember = "CommLineNum";
            cbCommLine.DataSource = commLineTable;

            try
            {
                cbCommLine.SelectedValue = recentSelection.CommLineNum;
            }
            catch
            {
                cbCommLine.SelectedValue = 0;
            }
        }

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
        /// Sets the device number by default.
        /// </summary>
        private void SetDeviceNum()
        {
            if (recentSelection.KPNum > 0)
                numKPNum.SetValue(recentSelection.KPNum + 1);
            else if (project.ConfigBase.KPTable.ItemCount > 0)
                numKPNum.SetValue(project.ConfigBase.KPTable.Items.Values.Last().KPNum + 1);
        }

        /// <summary>
        /// Validates the form fields.
        /// </summary>
        private bool ValidateFields()
        {
            StringBuilder sbError = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtName.Text))
                sbError.AppendError(lblName, CommonPhrases.NonemptyRequired);

            if ((int)cbKPType.SelectedValue <= 0)
                sbError.AppendError(lblKPType, CommonPhrases.NonemptyRequired);

            if (txtAddress.Text != "" && !int.TryParse(txtAddress.Text, out int address))
                sbError.AppendError(lblAddress, CommonPhrases.IntegerRequired);

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
        /// Checks feasibility of adding a device.
        /// </summary>
        private bool CheckFeasibility(out Settings.CommLine commLineSettings)
        {
            commLineSettings = null;
            int kpNum = Convert.ToInt32(numKPNum.Value);
            int commLineNum = (int)cbCommLine.SelectedValue;

            if (project.ConfigBase.KPTable.PkExists(kpNum))
            {
                ScadaUiUtils.ShowError(AppPhrases.DeviceAlreadyExists);
                return false;
            }

            if (chkAddToComm.Checked && cbInstance.SelectedItem is Instance instance && instance.CommApp.Enabled)
            {
                // load instance settings
                if (!instance.LoadAppSettings(out string errMsg))
                {
                    ScadaUiUtils.ShowError(errMsg);
                    return false;
                }

                // reverse search communication line
                for (int i = instance.CommApp.Settings.CommLines.Count - 1; i >= 0; i--)
                {
                    Settings.CommLine commLine = instance.CommApp.Settings.CommLines[i];
                    if (commLine.Number == commLineNum)
                    {
                        commLineSettings = commLine;
                        break;
                    }
                }

                if (commLineSettings == null)
                {
                    ScadaUiUtils.ShowError(AppPhrases.CommLineNotFound);
                    return false;
                }
            }

            return true;
        }


        private void FrmDeviceAdd_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            FillDeviceTypeList();
            FillCommLineList();
            FillInstanceList();
            SetDeviceNum();
            txtName.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateFields() && CheckFeasibility(out Comm.Settings.CommLine commLineSettings))
            {
                // create a new device
                int commLineNum = (int)cbCommLine.SelectedValue;
                KP kpEntity = new KP
                {
                    KPNum = Convert.ToInt32(numKPNum.Value),
                    Name = txtName.Text,
                    KPTypeID = (int)cbKPType.SelectedValue,
                    Address = txtAddress.Text == "" ? null : (int?)int.Parse(txtAddress.Text),
                    CallNum = txtCallNum.Text,
                    CommLineNum = commLineNum > 0 ? (int?)commLineNum : null,
                    Descr = txtDescr.Text
                };

                // insert the device in the configuration database
                project.ConfigBase.KPTable.AddItem(kpEntity);
                project.ConfigBase.KPTable.Modified = true;

                // insert the line in the Communicator settings
                if (chkAddToComm.Checked && cbInstance.SelectedItem is Instance instance)
                {
                    if (instance.CommApp.Enabled)
                    {
                        KPSettings = SettingsConverter.CreateKP(kpEntity, project.ConfigBase.KPTypeTable);
                        KPSettings.Parent = commLineSettings;
                        commLineSettings.ReqSequence.Add(KPSettings);
                        CommLineSettings = commLineSettings;
                    }

                    InstanceName = recentSelection.InstanceName = instance.Name;
                }

                recentSelection.CommLineNum = commLineNum;
                recentSelection.KPNum = kpEntity.KPNum;
                recentSelection.KPTypeID = kpEntity.KPTypeID;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
