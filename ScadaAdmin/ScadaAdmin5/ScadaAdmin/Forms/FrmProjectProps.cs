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
 * Summary  : Form for editing project properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.Project;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for editing project properties.
    /// <para>Форма редактирования свойств проекта.</para>
    /// </summary>
    public partial class FrmProjectProps : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmProjectProps()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string ProjectName
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
        /// Gets or sets the project version.
        /// </summary>
        public ProjectVersion Version
        {
            get
            {
                return new ProjectVersion(
                    Convert.ToInt32(numMajorVersion.Value), 
                    Convert.ToInt32(numMinorVersion.Value));
            }
            set
            {
                numMajorVersion.SetValue(value.Major);
                numMinorVersion.SetValue(value.Minor);
            }
        }

        /// <summary>
        /// Gets or sets the project description.
        /// </summary>
        public string Description
        {
            get
            {
                return txtDescr.Text.Trim();
            }
            set
            {
                txtDescr.Text = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the project properties was modified.
        /// </summary>
        public bool Modified { get; private set; }


        private void FrmProjectProps_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            Modified = false;
            ActiveControl = numMajorVersion;
        }

        private void control_Changed(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
