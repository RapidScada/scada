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
 * Module   : ScadaAdminCommon
 * Summary  : Administrator settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

namespace Scada.Admin
{
    /// <summary>
    /// Administrator settings.
    /// <para>Настройки Администратора.</para>
    /// </summary>
    public class AdminSettings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AdminSettings()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the directory of the Server application.
        /// </summary>
        public string ServerDir { get; set; }

        /// <summary>
        /// Gets or sets the directory of the Communicator application.
        /// </summary>
        public string CommDir { get; set; }

        /// <summary>
        /// Gets or sets the full file name of a scheme editor.
        /// </summary>
        public string SchemeEditorPath { get; set; }

        /// <summary>
        /// Gets or sets the full file name of a table editor.
        /// </summary>
        public string TableEditorPath { get; set; }

        /// <summary>
        /// Gets or sets the full file name of a text editor.
        /// </summary>
        public string TextEditorPath { get; set; }

        /// <summary>
        /// Gets or sets the multiplicity of the first channel of a device.
        /// </summary>
        public int CnlMult { get; set; }

        /// <summary>
        /// Gets or sets the shift of the first channel of a device.
        /// </summary>
        public int CnlShift { get; set; }

        /// <summary>
        /// Gets or sets the gap between channel numbers of different devices.
        /// </summary>
        public int CnlGap { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            if (ScadaUtils.IsRunningOnWin)
            {
                ServerDir = @"C:\SCADA\ScadaServer\";
                CommDir = @"C:\SCADA\ScadaComm\";
                SchemeEditorPath = @"C:\SCADA\ScadaSchemeEditor\ScadaSchemeEditor.exe";
                TableEditorPath = @"C:\SCADA\ScadaTableEditor\ScadaTableEditor.exe";
                TextEditorPath = "";
            }
            else
            {
                ServerDir = "/opt/scada/ScadaServer/";
                CommDir = "/opt/scada/ScadaComm/";
                SchemeEditorPath = "";
                TableEditorPath = "";
                TextEditorPath = "";
            }

            CnlMult = 100;
            CnlShift = 1;
            CnlGap = 10;
        }
    }
}
