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
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Manipulates the explorer tree.
    /// <para>Манипулирует деревом проводника.</para>
    /// </summary>
    internal class ExplorerBuilder
    {
        private readonly TreeView treeView; // the manipulated tree view 
        private ScadaProject project;       // the current project to build tree


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExplorerBuilder(TreeView treeView)
        {
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
            //sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.CommLineTable));
            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.KPTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.InCnlTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.CtrlCnlTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.RoleTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.UserTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.InterfaceTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.RightTable));
            baseNode.Nodes.Add(sysTableNode);

            TreeNode dictTableNode = new TreeNode(AppPhrases.DictTableNode);
            dictTableNode.ImageKey = dictTableNode.SelectedImageKey = "folder_closed.png";

            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.CnlTypeTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.CmdTypeTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.EvTypeTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.KPTypeTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.ParamTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.UnitTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.CmdValTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.FormatTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(configBase.FormulaTable));
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
                Arguments = new object[] { baseTable, project }
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

            TreeNode serverNode = new TreeNode(AppPhrases.ServerNode);
            serverNode.ImageKey = serverNode.SelectedImageKey = "server.png";
            instanceNode.Nodes.Add(serverNode);

            TreeNode commNode = new TreeNode(AppPhrases.CommNode);
            commNode.ImageKey = commNode.SelectedImageKey = "comm.png";
            instanceNode.Nodes.Add(commNode);

            TreeNode webNode = new TreeNode(AppPhrases.WebNode);
            webNode.ImageKey = webNode.SelectedImageKey = "webstation.png";
            instanceNode.Nodes.Add(webNode);

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
                treeView.Nodes.Add(projectNode);

                TreeNode baseNode = CreateBaseNode(project.ConfigBase);
                projectNode.Nodes.Add(baseNode);

                TreeNode interfaceNode = new TreeNode(AppPhrases.InterfaceNode);
                interfaceNode.ImageKey = interfaceNode.SelectedImageKey = "interface.png";
                projectNode.Nodes.Add(interfaceNode);

                TreeNode instancesNode = new TreeNode(AppPhrases.InstancesNode);
                instancesNode.ImageKey = instancesNode.SelectedImageKey = "instances.png";
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
    }
}
