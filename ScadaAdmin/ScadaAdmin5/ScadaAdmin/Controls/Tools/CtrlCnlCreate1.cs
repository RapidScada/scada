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
 * Summary  : Channel creation wizard. Step 1
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Admin.Project;
using Scada.Admin.App.Code;
using Scada.Data.Tables;
using Scada.Data.Entities;
using System.Collections;

namespace Scada.Admin.App.Controls.Tools
{
    /// <summary>
    /// Channel creation wizard. Step 1.
    /// <para>Мастер создания каналов. Шаг 1.</para>
    /// </summary>
    public partial class CtrlCnlCreate1 : UserControl
    {
        private ScadaProject project;            // the project under development
        private RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCnlCreate1()
        {
            InitializeComponent();

            cbDevice.DisplayMember = "Name";
            cbDevice.ValueMember = "KPNum";
        }


        /// <summary>
        /// Gets the selected device.
        /// </summary>
        public KP SelectedDevice
        {
            get
            {
                return cbDevice.SelectedItem as KP;
            }
        }


        /// <summary>
        /// Fills the combo box with the communication lines.
        /// </summary>
        private void FillCommLineList()
        {
            DataTable commLineTable = project.ConfigBase.CommLineTable.ToDataTable();
            commLineTable.AddEmptyRow(0, AppPhrases.AllCommLines);
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
        /// Initializes the control.
        /// </summary>
        public void Init(ScadaProject project, RecentSelection recentSelection)
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.recentSelection = recentSelection ?? throw new ArgumentNullException("recentSelection");
            FillCommLineList();
        }
        
        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            cbCommLine.Select();
        }


        private void cbCommLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            // filter devices by the selected communication line
            int commLineNum = (int)cbCommLine.SelectedValue;
            IEnumerable kps = commLineNum > 0 ?
                project.ConfigBase.KPTable.SelectItems(new TableFilter("CommLineNum", commLineNum)) :
                project.ConfigBase.KPTable.EnumerateItems();
            cbDevice.DataSource = kps.Cast<KP>().ToList();

            try
            {
                cbDevice.SelectedValue = recentSelection.KPNum;
            }
            catch
            {
                cbDevice.SelectedValue = null;
            }
        }

        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
