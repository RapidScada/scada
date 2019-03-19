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
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Form for adding a device.
    /// <para>Форма для добавления КП.</para>
    /// </summary>
    public partial class FrmDeviceAdd : Form
    {
        private ScadaProject project;            // the project under development
        private RecentSelection recentSelection; // the recently selected objects


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

            numKPNum.Maximum = ushort.MaxValue;
            txtName.MaxLength = ColumnLength.Name;
            numAddress.Maximum = int.MaxValue;
            txtCallNum.MaxLength = ColumnLength.Default;
            txtDescr.MaxLength = ColumnLength.Description;
        }


        /// <summary>
        /// Gets the name of the instance affected in Communicator.
        /// </summary>
        public string InstanceName { get; private set; }


        /// <summary>
        /// Adds an empty row to the table.
        /// </summary>
        private void AddEmptyRow(DataTable dataTable)
        {
            DataRow emptyRow = dataTable.NewRow();
            emptyRow[0] = 0;
            emptyRow[1] = " ";
            dataTable.Rows.Add(emptyRow);
        }

        /// <summary>
        /// Fills the combo box with the device types.
        /// </summary>
        private void FillDeviceTypeList()
        {
            DataTable kpTypeTable = project.ConfigBase.KPTypeTable.ToDataTable();
            AddEmptyRow(kpTypeTable);
            kpTypeTable.DefaultView.Sort = "KPTypeID";

            cbKPType.DataSource = kpTypeTable;
            cbKPType.DisplayMember = "Name";
            cbKPType.ValueMember = "KPTypeID";

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
            AddEmptyRow(commLineTable);
            commLineTable.DefaultView.Sort = "CommLineNum";

            cbCommLine.DataSource = commLineTable;
            cbCommLine.DisplayMember = "Name";
            cbCommLine.ValueMember = "CommLineNum";

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
            try
            {
                cbInstance.BeginUpdate();
                string selectedName = recentSelection.InstanceName;
                int selectedIndex = 0;
                int index = 0;

                foreach (Instance instance in project.Instances)
                {
                    if (instance.Name == selectedName)
                        selectedIndex = index;

                    cbInstance.Items.Add(instance);
                    index++;
                }

                if (cbInstance.Items.Count > 0)
                    cbInstance.SelectedIndex = selectedIndex;
            }
            finally
            {
                cbInstance.EndUpdate();
            }
        }


        private void FrmDeviceAdd_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            FillDeviceTypeList();
            FillCommLineList();
            FillInstanceList();
            txtName.Select();
        }
    }
}
