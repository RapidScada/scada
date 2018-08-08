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
using System;
using System.Collections.Generic;
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
        private readonly TreeView treeView;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExplorerBuilder(TreeView treeView)
        {
            this.treeView = treeView ?? throw new ArgumentNullException("treeView");
        }


        /// <summary>
        /// Creates a node that represents the configuration database.
        /// </summary>
        private TreeNode CreateBaseNode(ConfigBase configBase)
        {
            TreeNode baseNode = new TreeNode(AppPhrases.BaseNode);
            baseNode.ImageKey = baseNode.SelectedImageKey = "database.png";

            TreeNode sysTableNode = new TreeNode("System"); // TODO: phrase
            sysTableNode.ImageKey = sysTableNode.SelectedImageKey = "folder_closed.png";

            sysTableNode.Nodes.Add(CreateBaseTableNode(configBase.ObjTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.CommLineTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.KPTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.InCnlTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.CtrlCnlTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.RoleTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.UserTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.InterfaceTable));
            //sysTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.RightTable));
            baseNode.Nodes.Add(sysTableNode);

            TreeNode dictTableNode = new TreeNode("Dictionaries"); // TODO: phrase
            dictTableNode.ImageKey = dictTableNode.SelectedImageKey = "folder_closed.png";

            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.CnlTypeTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.CmdTypeTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.EvTypeTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.KPTypeTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.ParamTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.UnitTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.CmdValTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.FormatTable));
            //dictTableNode.Nodes.Add(CreateBaseTableNode(CommonPhrases.FormulaTable));
            baseNode.Nodes.Add(dictTableNode);

            return baseNode;
        }

        /// <summary>
        /// Creates a node that represents the table of the configuration database.
        /// </summary>
        private TreeNode CreateBaseTableNode(BaseTable table)
        {
            TreeNode baseTableNode = new TreeNode(table.Title);
            baseTableNode.ImageKey = baseTableNode.SelectedImageKey = "table.png";
            baseTableNode.Tag = new TreeNodeTag()
            {
                FormType = typeof(FrmBaseTable),
                Arguments = table
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

            TreeNode serverNode = new TreeNode("Server");
            serverNode.ImageKey = serverNode.SelectedImageKey = "server.png";
            instanceNode.Nodes.Add(serverNode);

            TreeNode commNode = new TreeNode("Communicator");
            commNode.ImageKey = commNode.SelectedImageKey = "comm.png";
            instanceNode.Nodes.Add(commNode);

            TreeNode webNode = new TreeNode("Webstation");
            webNode.ImageKey = webNode.SelectedImageKey = "webstation.png";
            instanceNode.Nodes.Add(webNode);

            return instanceNode;
        }


        /// <summary>
        /// Creates tree nodes according to the project structure.
        /// </summary>
        public void CreateNodes(ScadaProject project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            try
            {
                treeView.BeginUpdate();
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
