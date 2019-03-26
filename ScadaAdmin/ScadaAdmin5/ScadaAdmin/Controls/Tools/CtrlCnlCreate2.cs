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
 * Summary  : Channel creation wizard. Step 2
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Admin.Project;
using Scada.Admin.App.Code;
using Scada.Data.Tables;
using Scada.Data.Entities;

namespace Scada.Admin.App.Controls.Tools
{
    /// <summary>
    /// Channel creation wizard. Step 2.
    /// <para>Мастер создания каналов. Шаг 2.</para>
    /// </summary>
    public partial class CtrlCnlCreate2 : UserControl
    {
        private ScadaProject project;            // the project under development
        private RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCnlCreate2()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the selected device name.
        /// </summary>
        public string DeviceName
        {
            get
            {
                return txtDevice.Text;
            }
            set
            {
                txtDevice.Text = value ?? "";
            }
        }
        
        /// <summary>
        /// Gets the selected object number.
        /// </summary>
        public int? ObjNum
        {
            get
            {
                int objNum = (int)cbObj.SelectedValue;
                return objNum > 0 ? (int?)objNum : null;
            }
        }


        /// <summary>
        /// Fills the combo box with the device types.
        /// </summary>
        private void FillObjectList()
        {
            DataTable objTable = project.ConfigBase.ObjTable.ToDataTable();
            objTable.AddEmptyRow();
            objTable.DefaultView.Sort = "ObjNum";

            cbObj.DataSource = objTable;
            cbObj.DisplayMember = "Name";
            cbObj.ValueMember = "ObjNum";

            try
            {
                cbObj.SelectedValue = recentSelection.ObjNum;
            }
            catch
            {
                cbObj.SelectedValue = 0;
            }
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(ScadaProject project, RecentSelection recentSelection)
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.recentSelection = recentSelection ?? throw new ArgumentNullException("recentSelection");
            FillObjectList();
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            cbObj.Select();
        }
    }
}
