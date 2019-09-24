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
 * Module   : KpOpcUa
 * Summary  : Device configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Comm.Devices.OpcUa.Config;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.OpcUa.UI
{
    /// <summary>
    /// Device configuration form.
    /// <para>Форма настройки конфигурации КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private AppDirs appDirs;           // the application directories
        private int kpNum;                 // the device number
        private DeviceConfig deviceConfig; // the device configuration
        private string configFileName;     // the configuration file name
        private bool modified;             // the configuration was modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();
        }

        public FrmConfig(AppDirs appDirs, int kpNum)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.kpNum = kpNum;
            deviceConfig = new DeviceConfig();
            configFileName = "";
            modified = false;
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            // translate the form
            if (Localization.LoadDictionaries(appDirs.LangDir, "KpOpcUa", out string errMsg))
                Translator.TranslateForm(this, GetType().FullName, toolTip);
            else
                ScadaUiUtils.ShowError(errMsg);

            Text = string.Format(Text, kpNum);

            // load a configuration
            configFileName = DeviceConfig.GetFileName(appDirs.ConfigDir, kpNum);

            if (File.Exists(configFileName) && !deviceConfig.Load(configFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // display the configuration
            //ConfigToControls();
            //Modified = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

        }

        private void btnSecurityOptions_Click(object sender, EventArgs e)
        {

        }
    }
}
