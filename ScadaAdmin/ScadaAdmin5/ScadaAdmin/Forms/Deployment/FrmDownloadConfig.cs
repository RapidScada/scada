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
 * Summary  : Download configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Download configuration form.
    /// <para>Форма скачивания конфигурации.</para>
    /// </summary>
    public partial class FrmDownloadConfig : Form
    {
        private readonly AppData appData;      // the common data of the application
        private readonly ScadaProject project; // the project under development
        private readonly Instance instance;    // the affected instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDownloadConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDownloadConfig(AppData appData, ScadaProject project, Instance instance)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            this.project = project ?? throw new ArgumentNullException("project");
            this.instance = instance ?? throw new ArgumentNullException("instance");
        }


        /// <summary>
        /// Gets a value indicating whether the configuration database was modified
        /// </summary>
        public bool BaseModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the interface files were modified
        /// </summary>
        public bool InterfaceModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the instance was modified
        /// </summary>
        public bool InstanceModified { get; protected set; }


        private void FrmDownloadConfig_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Admin.App.Controls.Deployment.CtrlProfileSelector");
            Translator.TranslateForm(this, "Scada.Admin.App.Controls.Deployment.CtrlTransferSettings");
            Translator.TranslateForm(this, "Scada.Admin.App.Forms.Deployment.FrmDownloadConfig");

            BaseModified = false;
            InterfaceModified = false;
            InstanceModified = false;

            ctrlProfileSelector.Init(appData, project.DeploymentSettings, instance);
        }
    }
}
