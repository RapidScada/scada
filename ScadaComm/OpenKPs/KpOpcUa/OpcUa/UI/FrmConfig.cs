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
 * Created  : 2019
 * Modified : 2019
 */

using Opc.Ua;
using Opc.Ua.Client;
using Scada.Comm.Devices.OpcUa.Config;
using Scada.UI;
using System;
using System.IO;
using System.Linq;
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
            public ServerNodeTag(ReferenceDescription rd, NamespaceTable namespaceTable)
            {
                DisplayName = rd.DisplayName.Text;
                OpcNodeId = ExpandedNodeId.ToNodeId(rd.NodeId, namespaceTable);
                NodeClass = rd.NodeClass;
                DataType = null;
                IsFilled = false;
            }

            public string DisplayName { get; private set; }
            public NodeId OpcNodeId { get; private set; }
            public NodeClass NodeClass { get; private set; }
            public Type DataType { get; private set; }
            public bool IsFilled { get; set; }
        }

        private const string FolderOpenImageKey = "folder_open.png";
        private const string FolderClosedImageKey = "folder_closed.png";

        private readonly AppDirs appDirs;           // the application directories
        private readonly int kpNum;                 // the device number
        private readonly DeviceConfig deviceConfig; // the device configuration
        private string configFileName;              // the configuration file name
        private bool modified;                      // the configuration was modified
        private bool changing;                      // controls are being changed programmatically
        private int? maxCmdNum;                     // the maximum command number
        private Session opcSession;                 // the OPC session
        private TreeNode subscriptionsNode;         // the tree node of the subscriptions
        private TreeNode commandsNode;              // the tree node of the commands


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();
            ctrlSubscription.Visible = false;
            ctrlItem.Visible = false;
            ctrlCommand.Visible = false;
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
            maxCmdNum = null;
            opcSession = null;
            subscriptionsNode = null;
            commandsNode = null;
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
            FillDeviceTree();
            changing = false;
        }

        /// <summary>
        /// Fills the device tree.
        /// </summary>
        private void FillDeviceTree()
        {
            try
            {
                tvDevice.BeginUpdate();
                tvDevice.Nodes.Clear();

                subscriptionsNode = TreeViewUtils.CreateNode("Subscriptions", FolderClosedImageKey);
                commandsNode = TreeViewUtils.CreateNode("Commands", FolderClosedImageKey);

                foreach (SubscriptionConfig subscriptionConfig in deviceConfig.Subscriptions)
                {
                    TreeNode subscriptionNode = CreateSubscriptionNode(subscriptionConfig);
                    subscriptionsNode.Nodes.Add(subscriptionNode);

                    foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                    {
                        subscriptionNode.Nodes.Add(CreateItemNode(itemConfig));
                    }
                }

                foreach (CommandConfig commandConfig in deviceConfig.Commands)
                {
                    commandsNode.Nodes.Add(CreateCommandNode(commandConfig));
                }

                tvDevice.Nodes.Add(subscriptionsNode);
                tvDevice.Nodes.Add(commandsNode);
                subscriptionsNode.Expand();
                commandsNode.Expand();
            }
            finally
            {
                tvDevice.EndUpdate();
            }
        }

        /// <summary>
        /// Creates a new subscription node according to the subscription configuration.
        /// </summary>
        private TreeNode CreateSubscriptionNode(SubscriptionConfig subscriptionConfig)
        {
            TreeNode subscriptionNode = TreeViewUtils.CreateNode(subscriptionConfig.DisplayName, FolderClosedImageKey);
            subscriptionNode.Tag = subscriptionConfig;
            return subscriptionNode;
        }

        /// <summary>
        /// Creates a new monitored item node according to the item configuration.
        /// </summary>
        private TreeNode CreateItemNode(ItemConfig itemConfig)
        {
            TreeNode itemNode = TreeViewUtils.CreateNode(itemConfig.DisplayName, "variable.png");
            itemNode.Tag = itemConfig;
            return itemNode;
        }

        /// <summary>
        /// Creates a new command node according to the command configuration.
        /// </summary>
        private TreeNode CreateCommandNode(CommandConfig commandConfig)
        {
            TreeNode commandNode = TreeViewUtils.CreateNode(commandConfig.DisplayName, "command.png");
            commandNode.Tag = commandConfig;
            return commandNode;
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
                btnAddItem.Enabled = false;
                subscriptionsNode = null;
                commandsNode = null;

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
                        childNode.Tag = new ServerNodeTag(rd, opcSession.NamespaceUris); 

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
        /// Gets the data type name of the node.
        /// </summary>
        private string GetDataTypeName(NodeId nodeId)
        {
            return "System.Double";
        }

        /// <summary>
        /// Gets the next command number.
        /// </summary>
        private int GetNextCmdNum()
        {
            if (maxCmdNum == null)
            {
                maxCmdNum = deviceConfig.Commands.Any() ? 
                    deviceConfig.Commands.Max(x => x.CmdNum) : 
                    0;
            }

            return (++maxCmdNum).Value;
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
        /// Sets the enabled property of the buttons that manipulate the device tree.
        /// </summary>
        private void SetDeviceButtonsEnabled()
        {
            ServerNodeTag serverNodeTag = tvServer.SelectedNode?.Tag as ServerNodeTag;
            bool deviceNodeTagDefined = tvDevice.SelectedNode?.Tag != null;

            btnAddItem.Enabled = serverNodeTag != null && serverNodeTag.NodeClass == NodeClass.Variable;
            btnMoveUpItem.Enabled = deviceNodeTagDefined && tvDevice.SelectedNode.PrevNode != null;
            btnMoveDownItem.Enabled = deviceNodeTagDefined && tvDevice.SelectedNode.NextNode != null;
            btnDeleteItem.Enabled = deviceNodeTagDefined;
        }

        /// <summary>
        /// Sets the node image as open or closed folder.
        /// </summary>
        private void SetFolderImage(TreeNode treeNode)
        {
            if (treeNode.ImageKey.StartsWith("folder_"))
                treeNode.SetImageKey(treeNode.IsExpanded ? FolderOpenImageKey : FolderClosedImageKey);
        }

        /// <summary>
        /// Gets the top parent of the specified node.
        /// </summary>
        private TreeNode GetTopParentNode(TreeNode treeNode)
        {
            if (treeNode == null)
            {
                return null;
            }
            else
            {
                TreeNode parentNode = treeNode.Parent;

                while (parentNode != null)
                {
                    treeNode = parentNode;
                    parentNode = treeNode.Parent;
                }

                return treeNode;
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
            SetDeviceButtonsEnabled();
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

        private void tvServer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetDeviceButtonsEnabled();
        }

        private void tvServer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            BrowseServerNode(e.Node);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            // add a new item
            if (tvServer.SelectedNode?.Tag is ServerNodeTag serverNodeTag &&
                serverNodeTag.NodeClass == NodeClass.Variable)
            {
                TreeNode deviceNode = tvDevice.SelectedNode;
                object deviceNodeTag = deviceNode?.Tag;

                if (GetTopParentNode(tvDevice.SelectedNode) == commandsNode)
                {
                    // add a new command
                    CommandConfig commandConfig = new CommandConfig
                    {
                        NodeID = serverNodeTag.OpcNodeId.ToString(),
                        DisplayName = serverNodeTag.DisplayName,
                        DataTypeName = GetDataTypeName(serverNodeTag.OpcNodeId),
                        CmdNum = GetNextCmdNum()
                    };

                    tvDevice.Insert(commandsNode, CreateCommandNode(commandConfig), 
                        deviceConfig.Commands, commandConfig);
                }
                else
                {
                    // create a new monitored item
                    ItemConfig itemConfig = new ItemConfig
                    {
                        NodeID = serverNodeTag.OpcNodeId.ToString(),
                        DisplayName = serverNodeTag.DisplayName
                    };

                    // find a subscription
                    TreeNode subscriptionNode = deviceNode?.FindClosest(typeof(SubscriptionConfig));
                    SubscriptionConfig subscriptionConfig;

                    if (subscriptionNode == null && subscriptionsNode.Nodes.Count > 0)
                        subscriptionNode = subscriptionsNode.Nodes[subscriptionsNode.Nodes.Count - 1];

                    // add a new subscription
                    if (subscriptionNode == null)
                    {
                        subscriptionConfig = new SubscriptionConfig();
                        subscriptionNode = CreateSubscriptionNode(subscriptionConfig);
                        tvDevice.Insert(subscriptionsNode, subscriptionNode, 
                            deviceConfig.Subscriptions, subscriptionConfig);
                    }
                    else
                    {
                        subscriptionConfig = (SubscriptionConfig)subscriptionNode.Tag;
                    }

                    // add the monitored item
                    tvDevice.Insert(subscriptionNode, CreateItemNode(itemConfig),
                       subscriptionConfig.Items, itemConfig);
                }

                Modified = true;
            }
        }

        private void btnAddSubscription_Click(object sender, EventArgs e)
        {
            // add a new subscription
            SubscriptionConfig subscriptionConfig = new SubscriptionConfig();
            TreeNode subscriptionNode = CreateSubscriptionNode(subscriptionConfig);
            tvDevice.Insert(subscriptionsNode, subscriptionNode,
                deviceConfig.Subscriptions, subscriptionConfig);
            ctrlSubscription.SetFocus();
            Modified = true;
        }

        private void btnMoveUpItem_Click(object sender, EventArgs e)
        {
            // move up the selected item
            TreeNode selectedNode = tvDevice.SelectedNode;
            object deviceNodeTag = selectedNode?.Tag;

            if (deviceNodeTag is SubscriptionConfig)
            {
                tvDevice.MoveUpSelectedNode(deviceConfig.Subscriptions);
            }
            else if (deviceNodeTag is ItemConfig)
            {
                if (selectedNode.Parent.Tag is SubscriptionConfig subscriptionConfig)
                    tvDevice.MoveUpSelectedNode(subscriptionConfig.Items);
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.MoveUpSelectedNode(deviceConfig.Commands);
            }

            Modified = true;
        }

        private void btnMoveDownItem_Click(object sender, EventArgs e)
        {
            // move down the selected item
            TreeNode selectedNode = tvDevice.SelectedNode;
            object deviceNodeTag = tvDevice.SelectedNode?.Tag;

            if (deviceNodeTag is SubscriptionConfig)
            {
                tvDevice.MoveDownSelectedNode(deviceConfig.Subscriptions);
            }
            else if (deviceNodeTag is ItemConfig)
            {
                if (selectedNode.Parent.Tag is SubscriptionConfig subscriptionConfig)
                    tvDevice.MoveDownSelectedNode(subscriptionConfig.Items);
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.MoveDownSelectedNode(deviceConfig.Commands);
            }

            Modified = true;
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            // delete the selected item
            TreeNode selectedNode = tvDevice.SelectedNode;
            object deviceNodeTag = selectedNode?.Tag;

            if (deviceNodeTag is SubscriptionConfig)
            {
                tvDevice.RemoveNode(selectedNode, deviceConfig.Subscriptions);
            }
            else if (deviceNodeTag is ItemConfig)
            {
                if (selectedNode.Parent.Tag is SubscriptionConfig subscriptionConfig)
                    tvDevice.RemoveNode(selectedNode, subscriptionConfig.Items);
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.RemoveNode(selectedNode, deviceConfig.Commands);
                maxCmdNum = null; // need to recalculate maximum command number
            }

            Modified = true;
        }

        private void tvDevice_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetDeviceButtonsEnabled();

            // show parameters of the selected item
            gbEmptyItem.Visible = false;
            ctrlSubscription.Visible = false;
            ctrlItem.Visible = false;
            ctrlCommand.Visible = false;
            object deviceNodeTag = e.Node?.Tag;

            if (deviceNodeTag is SubscriptionConfig subscriptionConfig)
            {
                ctrlSubscription.SubscriptionConfig = subscriptionConfig;
                ctrlSubscription.Visible = true;
            }
            else if (deviceNodeTag is ItemConfig itemConfig)
            {
                ctrlItem.ItemConfig = itemConfig;
                ctrlItem.Visible = true;
            }
            else if (deviceNodeTag is CommandConfig commandConfig)
            {
                ctrlCommand.CommandConfig = commandConfig;
                ctrlCommand.Visible = true;
            }
            else
            {
                gbEmptyItem.Visible = true;
            }
        }

        private void tvDevice_AfterExpand(object sender, TreeViewEventArgs e)
        {
            SetFolderImage(e.Node);
        }

        private void tvDevice_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            SetFolderImage(e.Node);
        }

        private void ctrlItem_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            Modified = true;
            TreeNode selectedNode = tvDevice.SelectedNode;
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (e.ChangedObject is SubscriptionConfig subscriptionConfig)
            {
                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                    selectedNode.Text = subscriptionConfig.DisplayName;
            }
            else if (e.ChangedObject is ItemConfig itemConfig)
            {
                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                    selectedNode.Text = itemConfig.DisplayName;
            }
            else if (e.ChangedObject is CommandConfig commandConfig)
            {
                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                    selectedNode.Text = commandConfig.DisplayName;
            }
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
