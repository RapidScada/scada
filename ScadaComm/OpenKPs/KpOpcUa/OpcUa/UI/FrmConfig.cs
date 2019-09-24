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

using Opc.Ua;
using Opc.Ua.Client;
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
        private readonly AppDirs appDirs;           // the application directories
        private readonly int kpNum;                 // the device number
        private readonly DeviceConfig deviceConfig; // the device configuration
        private string configFileName;              // the configuration file name
        private bool modified;                      // the configuration was modified
        private Session opcSession;                 // the OPC session


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConfig(AppDirs appDirs, int kpNum)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.kpNum = kpNum;
            deviceConfig = new DeviceConfig();
            configFileName = "";
            modified = false;
            opcSession = null;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the configuration was modified.
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
            }
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            txtServerUrl.Text = deviceConfig.ConnectionOptions.ServerUrl;
        }

        /// <summary>
        /// Validates the certificate.
        /// </summary>
        private void CertificateValidator_CertificateValidation(CertificateValidator validator,
            CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
                e.Accept = true;
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
            ConfigToControls();
            Modified = false;
        }

        private async void btnConnect_ClickAsync(object sender, EventArgs e)
        {
            OpcUaHelper helper = new OpcUaHelper(appDirs, kpNum)
            {
                CertificateValidation = CertificateValidator_CertificateValidation
            };

            bool connected = await helper.ConnectAsync(deviceConfig.ConnectionOptions);
            opcSession = helper.OpcSession;

            if (connected)
            {
                opcSession.Browse(null, null, ObjectIds.ObjectsFolder, 
                    0, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true,
                    (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method,
                    out byte[] continuationPoint, out ReferenceDescriptionCollection references);

                try
                {
                    tvServer.BeginUpdate();
                    tvServer.Nodes.Clear();

                    foreach (ReferenceDescription rd in references)
                    {
                        TreeNode treeNode = tvServer.Nodes.Add(rd.DisplayName + " - " + rd.BrowseName + " - " + rd.NodeClass);

                        opcSession.Browse(null, null, ExpandedNodeId.ToNodeId(rd.NodeId, opcSession.NamespaceUris), 
                            0, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true,
                            (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method,
                            out byte[] nextCp, out ReferenceDescriptionCollection nextRefs);

                        foreach (ReferenceDescription nextRd in nextRefs)
                        {
                            treeNode.Nodes.Add(nextRd.DisplayName + " - " + nextRd.BrowseName + " - " + nextRd.NodeClass);
                        }
                    }
                }
                finally
                {
                    tvServer.EndUpdate();
                }
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (opcSession != null)
                opcSession.Close();
        }

        private void btnSecurityOptions_Click(object sender, EventArgs e)
        {

        }
    }
}
