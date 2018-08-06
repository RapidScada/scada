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
        /// Creates tree nodes according to the project structure.
        /// </summary>
        public void CreateNodes(ScadaProject project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            try
            {
                treeView.BeginUpdate();
                TreeNode projectNode = new TreeNode("Project"); // TODO: project.Name
                projectNode.ImageKey = projectNode.SelectedImageKey = "project.png";
                treeView.Nodes.Add(projectNode);

                TreeNode baseNode = new TreeNode("Configuration Database"); // TODO: phrase
                baseNode.ImageKey = baseNode.SelectedImageKey = "database.png";
                projectNode.Nodes.Add(baseNode);

                TreeNode interfaceNode = new TreeNode("Interface"); // TODO: phrase
                interfaceNode.ImageKey = interfaceNode.SelectedImageKey = "interface.png";
                projectNode.Nodes.Add(interfaceNode);

                TreeNode instanceNode = new TreeNode("Instance"); // TODO: foreach
                instanceNode.ImageKey = instanceNode.SelectedImageKey = "instance.png";
                projectNode.Nodes.Add(instanceNode);

                //node3.Tag = new TreeNodeTag()
                //{
                //    FormType = typeof(FrmBaseTable)
                //};

                projectNode.Expand();
            }
            finally
            {
                treeView.EndUpdate();
            }
        }
    }
}
