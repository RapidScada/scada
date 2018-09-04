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
 * Summary  : Manipulates the explorer tree
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.App.Forms;
using Scada.Admin.Project;
using Scada.Comm.Shell.Code;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Manipulates the explorer tree.
    /// <para>Манипулирует деревом проводника.</para>
    /// </summary>
    internal class ExplorerBuilder
    {
        private readonly AppData appData;           // the common data of the application
        private readonly ServerShell serverShell;   // the shell to edit Server settings
        private readonly CommShell commShell;       // the shell to edit Communicator settings
        private readonly TreeView treeView;         // the manipulated tree view 
        private readonly ContextMenus contextMenus; // references to the context menus
        private ScadaProject project;               // the current project to build tree


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExplorerBuilder(AppData appData, ServerShell serverShell, CommShell commShell, 
            TreeView treeView, ContextMenus contextMenus)
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            this.serverShell = serverShell ?? throw new ArgumentNullException("serverShell");
            this.commShell = commShell ?? throw new ArgumentNullException("commShell");
            this.treeView = treeView ?? throw new ArgumentNullException("treeView");
            this.contextMenus = contextMenus ?? throw new ArgumentNullException("contextMenus");
            project = null;
        }


        /// <summary>
        /// Creates a node that represents the configuration database.
        /// </summary>
        private TreeNode CreateBaseNode(ConfigBase configBase)
        {
            TreeNode baseNode = TreeViewUtils.CreateNode(AppPhrases.BaseNode, "database.png");
            baseNode.Tag = new TreeNodeTag()
            {
                RelatedObject = project.ConfigBase,
                NodeType = AppNodeType.Base
            };

            TreeNode sysTableNode = TreeViewUtils.CreateNode(AppPhrases.SysTableNode, "folder_closed.png");
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.ObjTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.CommLineTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.KPTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.InCnlTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.CtrlCnlTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.RoleTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.UserTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.InterfaceTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.RightTable));
            baseNode.Nodes.Add(sysTableNode);

            TreeNode dictTableNode = TreeViewUtils.CreateNode(AppPhrases.DictTableNode, "folder_closed.png");
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.CnlTypeTable));
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.CmdTypeTable));
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.EvTypeTable));
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.KPTypeTable));
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.ParamTable));
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.UnitTable));
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.CmdValTable));
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.FormatTable));
            dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.FormulaTable));
            baseNode.Nodes.Add(dictTableNode);

            return baseNode;
        }

        /// <summary>
        /// Creates a node that represents the table of the configuration database.
        /// </summary>
        private TreeNode CreateBaseTableNode<T>(BaseTable<T> baseTable)
        {
            TreeNode baseTableNode = TreeViewUtils.CreateNode(baseTable.Title, "table.png");
            baseTableNode.Tag = new TreeNodeTag()
            {
                FormType = typeof(FrmBaseTableGeneric<T>),
                FormArgs = new object[] { appData, project, baseTable }
            };
            return baseTableNode;
        }


        /// <summary>
        /// Creates tree nodes according to the project structure.
        /// </summary>
        public void CreateNodes(ScadaProject project)
        {
            this.project = project ?? throw new ArgumentNullException("project");

            try
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();

                TreeNode projectNode = TreeViewUtils.CreateNode(project.Name, "project.png", true);
                projectNode.ContextMenuStrip = contextMenus.ProjectMenu;
                projectNode.Tag = new TreeNodeTag()
                {
                    RelatedObject = project,
                    NodeType = AppNodeType.Project
                };
                treeView.Nodes.Add(projectNode);

                TreeNode baseNode = CreateBaseNode(project.ConfigBase);
                projectNode.Nodes.Add(baseNode);

                TreeNode interfaceNode = TreeViewUtils.CreateNode(AppPhrases.InterfaceNode, "ui.png");
                interfaceNode.Tag = new TreeNodeTag()
                {
                    RelatedObject = project.Interface,
                    NodeType = AppNodeType.Interface
                };
                projectNode.Nodes.Add(interfaceNode);

                TreeNode instancesNode = TreeViewUtils.CreateNode(AppPhrases.InstancesNode, "instances.png");
                instancesNode.ContextMenuStrip = contextMenus.InstanceMenu;
                instancesNode.Tag = new TreeNodeTag()
                {
                    RelatedObject = project.Instances,
                    NodeType = AppNodeType.Instances
                };
                projectNode.Nodes.Add(instancesNode);

                foreach (Instance instance in project.Instances)
                {
                    instancesNode.Nodes.Add(CreateInstanceNode(instance));
                }

                projectNode.Expand();
                instancesNode.Expand();
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Creates a node that represents the instance.
        /// </summary>
        public TreeNode CreateInstanceNode(Instance instance)
        {
            TreeNode instanceNode = TreeViewUtils.CreateNode(instance.Name, "instance.png");
            instanceNode.ContextMenuStrip = contextMenus.InstanceMenu;
            instanceNode.Tag = new TreeNodeTag()
            {
                RelatedObject = new LiveInstance(instance),
                NodeType = AppNodeType.Instance
            };

            TreeNode emptyNode = TreeViewUtils.CreateNode(AppPhrases.EmptyNode, "empty.png");
            instanceNode.Nodes.Add(emptyNode);

            return instanceNode;
        }

        /// <summary>
        /// Fills the instance node by child nodes.
        /// </summary>
        public void FillInstanceNode(TreeNode instanceNode)
        {
            LiveInstance liveInstance = (LiveInstance)((TreeNodeTag)instanceNode.Tag).RelatedObject;
            Instance instance = liveInstance.Instance;

            try
            {
                treeView.BeginUpdate();
                instanceNode.Nodes.Clear();

                // create Server nodes
                if (instance.ServerApp.Enabled)
                {
                    TreeNode serverNode = TreeViewUtils.CreateNode(AppPhrases.ServerNode, "server.png");
                    serverNode.Tag = new TreeNodeTag()
                    {
                        RelatedObject = instance.ServerApp,
                        NodeType = AppNodeType.ServerApp
                    };
                    serverNode.Nodes.AddRange(serverShell.GetTreeNodes(
                        instance.ServerApp.Settings, liveInstance.ServerEnvironment));
                    instanceNode.Nodes.Add(serverNode);
                }

                // create Communicator nodes
                if (instance.CommApp.Enabled)
                {
                    TreeNode commNode = TreeViewUtils.CreateNode(AppPhrases.CommNode, "comm.png");
                    commNode.Tag = new TreeNodeTag()
                    {
                        RelatedObject = instance.CommApp,
                        NodeType = AppNodeType.CommApp
                    };
                    commNode.Nodes.AddRange(commShell.GetTreeNodes(
                        instance.CommApp.Settings, liveInstance.CommEnvironment));

                    foreach (TreeNode treeNode in TreeViewUtils.IterateNodes(commNode.Nodes))
                    {
                        if (treeNode.Tag is TreeNodeTag tag && 
                            (tag.NodeType == CommNodeType.CommLines || tag.NodeType == CommNodeType.CommLine))
                        {
                            treeNode.ContextMenuStrip = contextMenus.CommLineMenu;
                        }
                    }

                    instanceNode.Nodes.Add(commNode);
                }

                // create Webstation nodes
                if (instance.WebApp.Enabled)
                {
                    TreeNode webNode = new TreeNode(AppPhrases.WebNode);
                    webNode.ImageKey = webNode.SelectedImageKey = "chrome.png";
                    webNode.Tag = new TreeNodeTag()
                    {
                        RelatedObject = instance.WebApp,
                        NodeType = AppNodeType.WebApp
                    };
                    instanceNode.Nodes.Add(webNode);
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }
    }
}
