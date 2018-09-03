/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : Form for creating or editing an instance
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.Project;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for creating or editing an instance.
    /// <para>Форма создания или редактирования экземпляра.</para>
    /// </summary>
    public partial class FrmInstanceEdit : Form
    {
        /// <summary>
        /// Specifies the form operating mode.
        /// </summary>
        public enum OperatingMode { New, Edit }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInstanceEdit()
        {
            InitializeComponent();
            Mode = OperatingMode.New;
        }


        /// <summary>
        /// Gets or sets the form operating mode.
        /// </summary>
        public OperatingMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the instance name.
        /// </summary>
        public string InstanceName
        {
            get
            {
                return txtName.Text.Trim();
            }
            set
            {
                txtName.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Server is present in the instance.
        /// </summary>
        public bool ServerAppEnabled
        {
            get
            {
                return chkServerApp.Checked;
            }
            set
            {
                chkServerApp.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Communicator is present in the instance.
        /// </summary>
        public bool CommAppEnabled
        {
            get
            {
                return chkCommApp.Checked;
            }
            set
            {
                chkCommApp.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Webstation is present in the instance.
        /// </summary>
        public bool WebAppEnabled
        {
            get
            {
                return chkWebApp.Checked;
            }
            set
            {
                chkWebApp.Checked = value;
            }
        }


        /// <summary>
        /// Initializes the properties based on the specified instance.
        /// </summary>
        public void Init(Instance instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            InstanceName = instance.Name;
            ServerAppEnabled = instance.ServerApp.Enabled;
            CommAppEnabled = instance.CommApp.Enabled;
            WebAppEnabled = instance.WebApp.Enabled;
        }

        /// <summary>
        /// Determines whether the application is enabled or not.
        /// </summary>
        public bool GetAppEnabled(ScadaApp scadaApp)
        {
            if (scadaApp is ServerApp)
                return ServerAppEnabled;
            else if (scadaApp is CommApp)
                return CommAppEnabled;
            else if (scadaApp is WebApp)
                return WebAppEnabled;
            else
                return false;
        }


        private void FrmInstanceEdit_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
