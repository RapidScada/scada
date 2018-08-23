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
        private readonly AppData appData;         // the common data of the application
        private readonly ServerShell serverShell; // the shell to edit Server settings
        private readonly TreeView treeView;       // the manipulated tree view 
        private ScadaProject project;             // the current project to build tree


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExplorerBuilder(AppData appData, ServerShell serverShell, TreeView treeView)
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            this.serverShell = serverShell ?? throw new ArgumentNullException("serverShell");
            this.treeView = treeView ?? throw new ArgumentNullException("treeView");
            project = null;
        }


        /// <summary>
        /// Creates a node that represents the configuration database.
        /// </summary>
        private TreeNode CreateBaseNode(ConfigBase configBase)
        {
            TreeNode baseNode = new TreeNode(AppPhrases.BaseNode);
            baseNode.ImageKey = baseNode.SelectedImageKey = "database.png";

            TreeNode sysTableNode = new TreeNode(AppPhrases.SysTableNode);
            sysTableNode.ImageKey = sysTableNode.SelectedImageKey = "folder_closed.png";

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

            TreeNode dictTableNode = new TreeNode(AppPhrases.DictTableNode);
            dictTableNode.ImageKey = dictTableNode.SelectedImageKey = "folder_closed.png";

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
            TreeNode baseTableNode = new TreeNode(baseTable.Title);
            baseTableNode.ImageKey = baseTableNode.SelectedImageKey = "table.png";
            baseTableNode.Tag = new TreeNodeTag()
            {
                FormType = typeof(FrmBaseTableGeneric<T>),
                FormArgs = new object[] { appData, project, baseTable }
            };
            return baseTableNode;
        }

        /// <summary>
        /// Creates a node that represents the instance.
        /// </summary>
        private TreeNode CreateInstanceNode(Instance instance)
        {
            TreeNode instanceNode = new TreeNode(instance.Name);
            instanceNode.ImageKey = instanceNode.SelectedImageKey = "instance.png";
            instanceNode.Tag = new TreeNodeTag() { RelatedObject = instance };

            TreeNode emptyNode = new TreeNode(AppPhrases.EmptyNode);
            emptyNode.ImageKey = emptyNode.SelectedImageKey = "empty.png";
            instanceNode.Nodes.Add(emptyNode);

            return instanceNode;
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

                TreeNode projectNode = new TreeNode(project.Name);
                projectNode.ImageKey = projectNode.SelectedImageKey = "project.png";
                projectNode.Tag = new TreeNodeTag() { RelatedObject = project };
                treeView.Nodes.Add(projectNode);

                TreeNode baseNode = CreateBaseNode(project.ConfigBase);
                baseNode.Tag = new TreeNodeTag() { RelatedObject = project.ConfigBase };
                projectNode.Nodes.Add(baseNode);

                TreeNode interfaceNode = new TreeNode(AppPhrases.InterfaceNode);
                interfaceNode.ImageKey = interfaceNode.SelectedImageKey = "ui.png";
                interfaceNode.Tag = new TreeNodeTag() { RelatedObject = project.Interface };
                projectNode.Nodes.Add(interfaceNode);

                TreeNode instancesNode = new TreeNode(AppPhrases.InstancesNode);
                instancesNode.ImageKey = instancesNode.SelectedImageKey = "instances.png";
                instancesNode.Tag = new TreeNodeTag() { RelatedObject = project.Instances };
                projectNode.Nodes.Add(instancesNode);

                foreach (Instance instance in project.Instances)
                {
                    TreeNode instanceNode = CreateInstanceNode(instance);
                    instancesNode.Nodes.Add(instanceNode);
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
        /// Fills the instance node by child nodes.
        /// </summary>
        public void FillInstanceNode(TreeNode instanceNode, Instance instance, ServerEnvironment serverEnvironment)
        {
            try
            {
                treeView.BeginUpdate();
                instanceNode.Nodes.Clear();

                if (instance.ServerApp.Enabled)
                {
                    TreeNode serverNode = new TreeNode(AppPhrases.ServerNode);
                    serverNode.ImageKey = serverNode.SelectedImageKey = "server.png";
                    serverNode.Tag = new TreeNodeTag() { RelatedObject = instance.ServerApp };
                    serverNode.Nodes.AddRange(serverShell.GetTreeNodes(
                        instance.ServerApp.Settings, serverEnvironment));
                    instanceNode.Nodes.Add(serverNode);
                }

                if (instance.CommApp.Enabled)
                {
                    TreeNode commNode = new TreeNode(AppPhrases.CommNode);
                    commNode.ImageKey = commNode.SelectedImageKey = "comm.png";
                    commNode.Tag = new TreeNodeTag() { RelatedObject = instance.CommApp };
                    instanceNode.Nodes.Add(commNode);
                }

                if (instance.WebApp.Enabled)
                {
                    TreeNode webNode = new TreeNode(AppPhrases.WebNode);
                    webNode.ImageKey = webNode.SelectedImageKey = "chrome.png";
                    webNode.Tag = new TreeNodeTag() { RelatedObject = instance.WebApp };
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
