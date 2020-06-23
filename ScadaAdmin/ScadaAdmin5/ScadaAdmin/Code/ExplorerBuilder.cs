/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Modified : 2020
 */

using Scada.Admin.App.Forms.Tables;
using Scada.Admin.Project;
using Scada.Comm.Shell.Code;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO;
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

            ProjectNode = null;
            BaseNode = null;
            BaseTableNodes = null;
            InterfaceNode = null;
            InstancesNode = null;
        }


        /// <summary>
        /// Gets the project node.
        /// </summary>
        public TreeNode ProjectNode { get; private set; }

        /// <summary>
        /// Gets the configuration database node.
        /// </summary>
        public TreeNode BaseNode { get; private set; }

        /// <summary>
        /// Gets the configuration database table nodes by names.
        /// </summary>
        public Dictionary<string, TreeNode> BaseTableNodes { get; private set; }

        /// <summary>
        /// Gets the interface node.
        /// </summary>
        public TreeNode InterfaceNode { get; private set; }

        /// <summary>
        /// Gets the instances node.
        /// </summary>
        public TreeNode InstancesNode { get; private set; }


        /// <summary>
        /// Creates a node that represents the configuration database.
        /// </summary>
        private TreeNode CreateBaseNode(ConfigBase configBase)
        {
            TreeNode baseNode = TreeViewUtils.CreateNode(AppPhrases.BaseNode, "database.png");
            baseNode.Tag = new TreeNodeTag
            {
                RelatedObject = project.ConfigBase,
                NodeType = AppNodeType.Base
            };

            TreeNode sysTableNode = TreeViewUtils.CreateNode(AppPhrases.SysTableNode, "folder_closed.png");
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.ObjTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.CommLineTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.KPTable));

            TreeNode inCnlTableNode = CreateBaseTableNode(configBase.InCnlTable);
            TreeNode ctrlCnlTableNode = CreateBaseTableNode(configBase.CtrlCnlTable);
            inCnlTableNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            ctrlCnlTableNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            sysTableNode.Nodes.Add(inCnlTableNode);
            sysTableNode.Nodes.Add(ctrlCnlTableNode);
            FillCnlTableNodes(inCnlTableNode, ctrlCnlTableNode, configBase);

            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.RoleTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.RoleRefTable));
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
        private TreeNode CreateBaseTableNode(IBaseTable baseTable)
        {
            TreeNode baseTableNode = TreeViewUtils.CreateNode(baseTable.Title, "table.png");
            baseTableNode.Tag = CreateBaseTableTag(baseTable);
            BaseTableNodes.Add(baseTable.Name, baseTableNode);
            return baseTableNode;
        }

        /// <summary>
        /// Creates a tag to associate with a tree node representing a table.
        /// </summary>
        private TreeNodeTag CreateBaseTableTag(IBaseTable baseTable, TableFilter tableFilter = null)
        {
            return new TreeNodeTag
            {
                FormType = typeof(FrmBaseTable),
                FormArgs = new object[] { baseTable, tableFilter, project, appData },
                RelatedObject = new BaseTableItem(baseTable, tableFilter),
                NodeType = AppNodeType.BaseTable
            };
        }

        /// <summary>
        /// Creates a table filter for filtering by device.
        /// </summary>
        private TableFilter CreateFilterByDevice(KP kp)
        {
            return kp == null ?
                new TableFilter("KPNum", null) { Title = AppPhrases.EmptyDeviceFilter } :
                new TableFilter("KPNum", kp.KPNum) { Title = string.Format(AppPhrases.DeviceFilter, kp.KPNum)};
        }

        /// <summary>
        /// Fills the channel table nodes.
        /// </summary>
        private void FillCnlTableNodes(TreeNode inCnlTableNode, TreeNode ctrlCnlTableNode, ConfigBase configBase)
        {
            foreach (KP kp in configBase.KPTable.EnumerateItems())
            {
                string nodeText = string.Format(AppPhrases.TableByDeviceNode, kp.KPNum, kp.Name);

                TreeNode inCnlsByDeviceNode = TreeViewUtils.CreateNode(nodeText, "table.png");
                inCnlsByDeviceNode.ContextMenuStrip = contextMenus.CnlTableMenu;
                inCnlsByDeviceNode.Tag = CreateBaseTableTag(configBase.InCnlTable, CreateFilterByDevice(kp));
                inCnlTableNode.Nodes.Add(inCnlsByDeviceNode);

                TreeNode ctrlCnlsByDeviceNode = TreeViewUtils.CreateNode(nodeText, "table.png");
                ctrlCnlsByDeviceNode.ContextMenuStrip = contextMenus.CnlTableMenu;
                ctrlCnlsByDeviceNode.Tag = CreateBaseTableTag(configBase.CtrlCnlTable, CreateFilterByDevice(kp));
                ctrlCnlTableNode.Nodes.Add(ctrlCnlsByDeviceNode);
            }

            TreeNode inCnlsEmptyDeviceNode = TreeViewUtils.CreateNode(AppPhrases.EmptyDeviceNode, "table.png");
            inCnlsEmptyDeviceNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            inCnlsEmptyDeviceNode.Tag = CreateBaseTableTag(configBase.InCnlTable, CreateFilterByDevice(null));
            inCnlTableNode.Nodes.Add(inCnlsEmptyDeviceNode);

            TreeNode ctrlCnlsEmptyDeviceNode = TreeViewUtils.CreateNode(AppPhrases.EmptyDeviceNode, "table.png");
            ctrlCnlsEmptyDeviceNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            ctrlCnlsEmptyDeviceNode.Tag = CreateBaseTableTag(configBase.CtrlCnlTable, CreateFilterByDevice(null));
            ctrlCnlTableNode.Nodes.Add(ctrlCnlsEmptyDeviceNode);
        }

        /// <summary>
        /// Creates a node that represents the specified directory.
        /// </summary>
        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            TreeNode directoryNode = TreeViewUtils.CreateNode(directoryInfo.Name, "folder_closed.png");
            directoryNode.ContextMenuStrip = contextMenus.DirectoryMenu;
            directoryNode.Tag = new TreeNodeTag
            {
                RelatedObject = new FileItem(directoryInfo),
                NodeType = AppNodeType.Directory
            };
            return directoryNode;
        }

        /// <summary>
        /// Creates a node that represents the specified file.
        /// </summary>
        private TreeNode CreateFileNode(FileInfo fileInfo)
        {
            TreeNode fileNode = TreeViewUtils.CreateNode(fileInfo.Name, "file.png");
            fileNode.ContextMenuStrip = contextMenus.FileItemMenu;
            fileNode.Tag = new TreeNodeTag
            {
                RelatedObject = new FileItem(fileInfo),
                NodeType = AppNodeType.File
            };
            return fileNode;
        }

        /// <summary>
        /// Fills the tree node according to the file system.
        /// </summary>
        private void FillFileNode(TreeNode treeNode, DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] dirInfoArr = directoryInfo.GetDirectories();
            TreeNodeCollection nodes = treeNode.Nodes;

            foreach (DirectoryInfo subdirInfo in directoryInfo.GetDirectories())
            {
                TreeNode subdirNode = CreateDirectoryNode(subdirInfo);
                FillFileNode(subdirNode, subdirInfo);
                nodes.Add(subdirNode);
            }

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                TreeNode fileNode = CreateFileNode(fileInfo);
                nodes.Add(fileNode);
            }
        }


        /// <summary>
        /// Creates tree nodes according to the project structure.
        /// </summary>
        public void CreateNodes(ScadaProject project)
        {
            this.project = project ?? throw new ArgumentNullException("project");
            ProjectNode = null;
            BaseNode = null;
            BaseTableNodes = new Dictionary<string, TreeNode>();
            InterfaceNode = null;
            InstancesNode = null;

            try
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();

                ProjectNode = TreeViewUtils.CreateNode(project.Name, "project.png", true);
                ProjectNode.ContextMenuStrip = contextMenus.ProjectMenu;
                ProjectNode.Tag = new TreeNodeTag
                {
                    RelatedObject = project,
                    NodeType = AppNodeType.Project
                };
                treeView.Nodes.Add(ProjectNode);

                BaseNode = CreateBaseNode(project.ConfigBase);
                ProjectNode.Nodes.Add(BaseNode);

                InterfaceNode = TreeViewUtils.CreateNode(AppPhrases.InterfaceNode, "ui.png");
                InterfaceNode.ContextMenuStrip = contextMenus.DirectoryMenu;
                InterfaceNode.Tag = new TreeNodeTag
                {
                    RelatedObject = project.Interface,
                    NodeType = AppNodeType.Interface
                };
                ProjectNode.Nodes.Add(InterfaceNode);

                TreeNode emptyNode = TreeViewUtils.CreateNode(AppPhrases.EmptyNode, "empty.png");
                InterfaceNode.Nodes.Add(emptyNode);

                InstancesNode = TreeViewUtils.CreateNode(AppPhrases.InstancesNode, "instances.png");
                InstancesNode.ContextMenuStrip = contextMenus.InstanceMenu;
                InstancesNode.Tag = new TreeNodeTag
                {
                    RelatedObject = project.Instances,
                    NodeType = AppNodeType.Instances
                };
                ProjectNode.Nodes.Add(InstancesNode);

                foreach (Instance instance in project.Instances)
                {
                    InstancesNode.Nodes.Add(CreateInstanceNode(instance));
                }

                ProjectNode.Expand();
                InstancesNode.Expand();
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
            instanceNode.Tag = new TreeNodeTag
            {
                RelatedObject = new LiveInstance(instance),
                NodeType = AppNodeType.Instance
            };

            TreeNode emptyNode = TreeViewUtils.CreateNode(AppPhrases.EmptyNode, "empty.png");
            instanceNode.Nodes.Add(emptyNode);

            return instanceNode;
        }

        /// <summary>
        /// Fills the channel table nodes, creating child nodes.
        /// </summary>
        public void FillChannelTableNodes(TreeNode inCnlTableNode, TreeNode ctrlCnlTableNode, ConfigBase configBase)
        {
            try
            {
                treeView.BeginUpdate();
                inCnlTableNode.Nodes.Clear();
                ctrlCnlTableNode.Nodes.Clear();
                FillCnlTableNodes(inCnlTableNode, ctrlCnlTableNode, configBase);
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the interface node, creating child nodes.
        /// </summary>
        public void FillInterfaceNode(TreeNode interfaceNode)
        {
            Project.Interface interfaceObj = (Project.Interface)((TreeNodeTag)interfaceNode.Tag).RelatedObject;
            FillFileNode(interfaceNode, interfaceObj.InterfaceDir);
        }

        /// <summary>
        /// Fills the instances node without creating child nodes.
        /// </summary>
        public void FillInstancesNode()
        {
            try
            {
                treeView.BeginUpdate();
                InstancesNode.Nodes.Clear();

                foreach (Instance instance in project.Instances)
                {
                    InstancesNode.Nodes.Add(CreateInstanceNode(instance));
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the instance node, creating child nodes.
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
                    serverNode.ContextMenuStrip = contextMenus.ServerMenu;
                    serverNode.Tag = new TreeNodeTag
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
                    commNode.ContextMenuStrip = contextMenus.CommMenu;
                    commNode.Tag = new TreeNodeTag
                    {
                        RelatedObject = instance.CommApp,
                        NodeType = AppNodeType.CommApp
                    };
                    commNode.Nodes.AddRange(commShell.GetTreeNodes(
                        instance.CommApp.Settings, liveInstance.CommEnvironment));

                    SetContextMenus(commNode);
                    instanceNode.Nodes.Add(commNode);
                }

                // create Webstation nodes
                if (instance.WebApp.Enabled)
                {
                    TreeNode webNode = TreeViewUtils.CreateNode(AppPhrases.WebNode, "chrome.png");
                    webNode.ContextMenuStrip = contextMenus.DirectoryMenu;
                    webNode.Tag = new TreeNodeTag
                    {
                        RelatedObject = instance.WebApp,
                        NodeType = AppNodeType.WebApp
                    };
                    instanceNode.Nodes.Add(webNode);

                    TreeNode emptyNode = TreeViewUtils.CreateNode(AppPhrases.EmptyNode, "empty.png");
                    webNode.Nodes.Add(emptyNode);
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the web application node, creating child nodes.
        /// </summary>
        public void FillWebstationNode(TreeNode webNode)
        {
            WebApp webApp = (WebApp)((TreeNodeTag)webNode.Tag).RelatedObject;
            FillFileNode(webNode, webApp.AppDir);
        }

        /// <summary>
        /// Fills the tree node according to the file system.
        /// </summary>
        public void FillFileNode(TreeNode treeNode, string directory)
        {
            try
            {
                treeView.BeginUpdate();
                treeNode.Nodes.Clear();
                FillFileNode(treeNode, new DirectoryInfo(directory));
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Inserts a newly created directory node.
        /// </summary>
        public void InsertDirectoryNode(TreeNode parentNode, string directory)
        {
            try
            {
                treeView.BeginUpdate();
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                string name = directoryInfo.Name;
                int index = 0;

                foreach (TreeNode treeNode in parentNode.Nodes)
                {
                    if (treeNode.TagIs(AppNodeType.Directory))
                    {
                        if (string.Compare(name, treeNode.Text, StringComparison.Ordinal) < 0)
                            break;
                        else
                            index = treeNode.Index + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                TreeNode directoryNode = CreateDirectoryNode(directoryInfo);
                parentNode.Nodes.Insert(index, directoryNode);
                treeView.SelectedNode = directoryNode;
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Inserts a newly created file node.
        /// </summary>
        public void InsertFileNode(TreeNode parentNode, string fileName)
        {
            try
            {
                treeView.BeginUpdate();
                FileInfo fileInfo = new FileInfo(fileName);
                string name = fileInfo.Name;
                int index = 0;

                foreach (TreeNode treeNode in parentNode.Nodes)
                {
                    if (treeNode.TagIs(AppNodeType.Directory))
                    {
                        index = treeNode.Index + 1;
                    }
                    else if (treeNode.TagIs(AppNodeType.File))
                    {
                        if (string.Compare(name, treeNode.Text, StringComparison.Ordinal) < 0)
                            break;
                        else
                            index = treeNode.Index + 1;
                    }
                    else
                    {
                        index = treeNode.Index;
                        break;
                    }
                }

                TreeNode fileNode = CreateFileNode(fileInfo);
                parentNode.Nodes.Insert(index, fileNode);
                treeView.SelectedNode = fileNode;
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the node image as open or closed folder.
        /// </summary>
        public void SetFolderImage(TreeNode treeNode)
        {
            if (treeNode.ImageKey.StartsWith("folder_"))
                treeNode.SetImageKey(treeNode.IsExpanded ? "folder_open.png" : "folder_closed.png");
        }

        /// <summary>
        /// Defines context menus of the node and its child nodes.
        /// </summary>
        /// <remarks>The method works for communication line.</remarks>
        public void SetContextMenus(TreeNode parentNode)
        {
            foreach (TreeNode treeNode in TreeViewUtils.IterateNodes(parentNode))
            {
                if (treeNode.Tag is TreeNodeTag tag)
                {
                    if (tag.NodeType == CommNodeType.CommLines || tag.NodeType == CommNodeType.CommLine)
                        treeNode.ContextMenuStrip = contextMenus.CommLineMenu;
                    else if (tag.NodeType == CommNodeType.Device)
                        treeNode.ContextMenuStrip = contextMenus.DeviceMenu;
                }
            }
        }
    }
}
