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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Devices.OpcUa.UI
{
    /// <summary>
    /// Device configuration form.
    /// <para>Форма настройки конфигурации КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        /// <summary>
        /// Represents an object associated with a node of the server tree.
        /// <para>Представляет объект, связанный с узлом дерева сервера.</para>
        /// </summary>
        private class ServerNodeTag
        {
            public ServerNodeTag(NodeId opcNodeId)
            {
                OpcNodeId = opcNodeId;
                IsFilled = false;
            }

            public NodeId OpcNodeId { get; private set; }
            public bool IsFilled { get; set; }
        }

        private readonly AppDirs appDirs;           // the application directories
        private readonly int kpNum;                 // the device number
        private readonly DeviceConfig deviceConfig; // the device configuration
        private string configFileName;              // the configuration file name
        private bool modified;                      // the configuration was modified
        private bool changing;                      // controls are being changed programmatically
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
            changing = false;
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
            changing = true;
            txtServerUrl.Text = deviceConfig.ConnectionOptions.ServerUrl;
            changing = false;
        }

        /// <summary>
        /// Connects to the OPC server.
        /// </summary>
        private async Task<bool> ConnectToOpcServer()
        {
            try
            {
                OpcUaHelper helper = new OpcUaHelper(appDirs, kpNum)
                {
                    CertificateValidation = CertificateValidator_CertificateValidation
                };

                if (await helper.ConnectAsync(deviceConfig.ConnectionOptions))
                {
                    opcSession = helper.OpcSession;
                    return true;
                }
                else
                {
                    opcSession = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(KpPhrases.ConnectServerError + ":" + Environment.NewLine + ex.Message);
                return false;
            }
            finally
            {
                SetConnButtonsEnabled();
            }
        }

        /// <summary>
        /// Disconnects from the OPC server.
        /// </summary>
        private void DisconnectFromOpcServer()
        {
            try
            {
                tvServer.Nodes.Clear();

                if (opcSession != null)
                {
                    opcSession.Close();
                    opcSession = null;
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(KpPhrases.DisconnectServerError + ":" + Environment.NewLine + ex.Message);
            }
            finally
            {
                SetConnButtonsEnabled();
            }
        }

        /// <summary>
        /// Browses the server node.
        /// </summary>
        private void BrowseServerNode(TreeNode treeNode)
        {
            try
            {
                tvServer.BeginUpdate();
                bool fillNode = false;
                TreeNodeCollection nodeCollection = null;
                NodeId nodeId = null;

                if (treeNode == null)
                {
                    fillNode = true;
                    nodeCollection = tvServer.Nodes;
                    nodeId = ObjectIds.ObjectsFolder;
                }
                else if (treeNode.Tag is ServerNodeTag serverNodeTag)
                {
                    fillNode = !serverNodeTag.IsFilled;
                    nodeCollection = treeNode.Nodes;
                    nodeId = serverNodeTag.OpcNodeId;
                }

                if (fillNode && nodeId != null && opcSession != null)
                {
                    opcSession.Browse(null, null, nodeId,
                        0, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true,
                        (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method,
                        out byte[] continuationPoint, out ReferenceDescriptionCollection references);
                    nodeCollection.Clear();

                    foreach (ReferenceDescription rd in references)
                    {
                        TreeNode childNode = TreeViewUtils.CreateNode(rd.DisplayName, SelectImageKey(rd.NodeClass));
                        childNode.Tag = new ServerNodeTag(ExpandedNodeId.ToNodeId(rd.NodeId, opcSession.NamespaceUris));

                        if (rd.NodeClass.HasFlag(NodeClass.Object))
                        {
                            TreeNode emptyNode = TreeViewUtils.CreateNode(KpPhrases.EmptyNode, "empty.png");
                            childNode.Nodes.Add(emptyNode);
                        }

                        nodeCollection.Add(childNode);
                    }
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(KpPhrases.BrowseServerError + ":" + Environment.NewLine + ex.Message);
            }
            finally
            {
                tvServer.EndUpdate();
            }
        }

        /// <summary>
        /// Selects an image key depending on the node class.
        /// </summary>
        private string SelectImageKey(NodeClass nodeClass)
        {
            if (nodeClass.HasFlag(NodeClass.Object))
                return "object.png";
            else if (nodeClass.HasFlag(NodeClass.Method))
                return "method.png";
            else
                return "variable.png";
        }

        /// <summary>
        /// Sets the enabled property of the connection buttons.
        /// </summary>
        private void SetConnButtonsEnabled()
        {
            if (opcSession == null)
            {
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
            }
            else
            {
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
            }
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
            KpPhrases.Init();

            // load a configuration
            configFileName = DeviceConfig.GetFileName(appDirs.ConfigDir, kpNum);

            if (File.Exists(configFileName) && !deviceConfig.Load(configFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // display the configuration
            ConfigToControls();
            SetConnButtonsEnabled();
            Modified = false;
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(CommPhrases.SaveKpSettingsConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!deviceConfig.Save(configFileName, out string errMsg))
                        {
                            ScadaUiUtils.ShowError(errMsg);
                            e.Cancel = true;
                        }
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private async void btnConnect_ClickAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(deviceConfig.ConnectionOptions.ServerUrl))
                ScadaUiUtils.ShowError(KpPhrases.ServerUrlRequired);
            else if (await ConnectToOpcServer())
                BrowseServerNode(null);
        }

        private void txtServerUrl_TextChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                deviceConfig.ConnectionOptions.ServerUrl = txtServerUrl.Text;
                Modified = true;
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectFromOpcServer();
        }

        private void btnSecurityOptions_Click(object sender, EventArgs e)
        {
            if (new FrmSecurityOptions(deviceConfig.ConnectionOptions).ShowDialog() == DialogResult.OK)
                Modified = true;
        }

        private void tvServer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            BrowseServerNode(e.Node);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (deviceConfig.Save(configFileName, out string errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
