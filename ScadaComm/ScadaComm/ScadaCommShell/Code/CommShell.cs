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
 * Module   : Communicator Shell
 * Summary  : Provides access to the Communicator shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Comm.Shell.Forms;
using Scada.Comm.Shell.Properties;
using Scada.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Comm.Shell.Code
{
    /// <summary>
    /// Provides access to the Communicator shell.
    /// <para>Обеспечивает доступ к оболочке Коммуникатора.</para>
    /// </summary>
    public class CommShell
    {
        /// <summary>
        /// Gets the images used by the explorer.
        /// </summary>
        public Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { "comm_device.png", Resources.comm_device },
                { "comm_driver.png", Resources.comm_driver },
                { "comm_line.png", Resources.comm_line },
                { "comm_line_inactive.png", Resources.comm_line_inactive },
                { "comm_lines.png", Resources.comm_lines },
                { "comm_params.png", Resources.comm_params },
                { "comm_stats.png", Resources.comm_stats }
            };
        }

        /// <summary>
        /// Gets the tree nodes for the explorer.
        /// </summary>
        public TreeNode[] GetTreeNodes(Settings settings, CommEnvironment environment)
        {
            // check the arguments
            if (settings == null)
                throw new ArgumentNullException("settings");

            if (environment == null)
                throw new ArgumentNullException("environment");

            // create nodes
            List<TreeNode> nodes = new List<TreeNode>();

            nodes.Add(new TreeNode(CommShellPhrases.CommonParamsNode)
            {
                ImageKey = "comm_params.png",
                SelectedImageKey = "comm_params.png",
                Tag = new TreeNodeTag()
                {
                    FormType = typeof(FrmCommonParams),
                    FormArgs = new object[] { settings.Params },
                    NodeType = CommNodeType.CommonParams
                }
            });

            nodes.Add(new TreeNode(CommShellPhrases.DriversNode)
            {
                ImageKey = "comm_driver.png",
                SelectedImageKey = "comm_driver.png",
                Tag = new TreeNodeTag()
                {
                    FormType = typeof(FrmDrivers),
                    FormArgs = new object[] { environment },
                    NodeType = CommNodeType.Drivers
                }
            });

            TreeNode commLinesNode = new TreeNode(CommShellPhrases.CommLinesNode)
            {
                ImageKey = "comm_lines.png",
                SelectedImageKey = "comm_lines.png",
                Tag = new TreeNodeTag()
                {
                    RelatedObject = settings,
                    NodeType = CommNodeType.CommLines
                }
            };

            nodes.Add(commLinesNode);

            foreach (Settings.CommLine commLine in settings.CommLines)
            {
                commLinesNode.Nodes.Add(CreateCommLineNode(commLine, environment));
            }

            nodes.Add(new TreeNode(CommShellPhrases.StatsNode)
            {
                ImageKey = "comm_stats.png",
                SelectedImageKey = "comm_stats.png",
                Tag = new TreeNodeTag()
                {
                    FormType = typeof(FrmStats),
                    FormArgs = new object[] { environment },
                    NodeType = CommNodeType.Stats
                }
            });

            return nodes.ToArray();
        }

        /// <summary>
        /// Creates a node that represents the communication line.
        /// </summary>
        public TreeNode CreateCommLineNode(Settings.CommLine commLine, CommEnvironment environment)
        {
            string commLineImageKey = commLine.Active ? "comm_line.png" : "comm_line_inactive.png";
            TreeNode commLineNode = new TreeNode(string.Format(CommShellPhrases.CommLineNode,
                commLine.Number, commLine.Name))
            {
                ImageKey = commLineImageKey,
                SelectedImageKey = commLineImageKey,
                Tag = new TreeNodeTag()
                {
                    RelatedObject = commLine,
                    NodeType = CommNodeType.CommLine
                }
            };

            TreeNodeCollection lineSubNodes = commLineNode.Nodes;

            lineSubNodes.Add(new TreeNode(CommShellPhrases.LineParamsNode)
            {
                ImageKey = "comm_params.png",
                SelectedImageKey = "comm_params.png",
                Tag = new TreeNodeTag()
                {
                    FormType = typeof(FrmLineParams),
                    FormArgs = new object[] { commLine, environment },
                    RelatedObject = commLine,
                    NodeType = CommNodeType.LineParams
                }
            });

            lineSubNodes.Add(new TreeNode(CommShellPhrases.LineStatsNode)
            {
                ImageKey = "comm_stats.png",
                SelectedImageKey = "comm_stats.png",
                Tag = new TreeNodeTag()
                {
                    FormType = typeof(FrmLineStats),
                    FormArgs = new object[] { commLine, environment },
                    RelatedObject = commLine,
                    NodeType = CommNodeType.LineStats
                }
            });

            foreach (Settings.KP kp in commLine.ReqSequence)
            {
                lineSubNodes.Add(CreateDeviceNode(kp, commLine, environment));
            }

            return commLineNode;
        }

        /// <summary>
        /// Creates a node and a corresponding object that represents the communication line.
        /// </summary>
        public TreeNode CreateCommLineNode(CommEnvironment environment)
        {
            Settings.CommLine commLine = new Settings.CommLine();
            return CreateCommLineNode(commLine, environment);
        }

        /// <summary>
        /// Updates the nodes that represent a communication line.
        /// </summary>
        public void UpdateCommLineNode(TreeNode commLineNode, CommEnvironment environment)
        {
            if (commLineNode == null)
                throw new ArgumentNullException("commLineNode");

            // update text of the line node
            Settings.CommLine commLine = (Settings.CommLine)((TreeNodeTag)commLineNode.Tag).RelatedObject;
            commLineNode.SetImageKey(commLine.Active ? "comm_line.png" : "comm_line_inactive.png");
            commLineNode.Text = string.Format(CommShellPhrases.CommLineNode, commLine.Number, commLine.Name);

            // remove the existing device nodes
            foreach (TreeNode lineSubNode in new ArrayList(commLineNode.Nodes))
            {
                if (lineSubNode.TagIs(CommNodeType.Device))
                    lineSubNode.Remove();
            }

            // add new device nodes
            foreach (Settings.KP kp in commLine.ReqSequence)
            {
                commLineNode.Nodes.Add(CreateDeviceNode(kp, commLine, environment));
            }
        }

        /// <summary>
        /// Updates the text of the node and its subnodes.
        /// </summary>
        public void UpdateNodeText(TreeNode startNode)
        {
            if (startNode == null)
                throw new ArgumentNullException("startNode");

            foreach (TreeNode treeNode in TreeViewUtils.IterateNodes(startNode))
            {
                if (treeNode.Tag is TreeNodeTag tag)
                {
                    if (tag.NodeType == CommNodeType.CommLine && tag.RelatedObject is Settings.CommLine commLine)
                        treeNode.Text = string.Format(CommShellPhrases.CommLineNode, commLine.Number, commLine.Name);
                    else if (tag.NodeType == CommNodeType.Device && tag.RelatedObject is Settings.KP kp)
                        treeNode.Text = string.Format(CommShellPhrases.DeviceNode, kp.Number, kp.Name);
                }
            }
        }

        /// <summary>
        /// Creates a node that represents the device.
        /// </summary>
        public TreeNode CreateDeviceNode(Settings.KP kp, Settings.CommLine commLine, CommEnvironment environment)
        {
            return new TreeNode(string.Format(CommShellPhrases.DeviceNode, kp.Number, kp.Name))
            {
                ImageKey = "comm_device.png",
                SelectedImageKey = "comm_device.png",
                Tag = new TreeNodeTag()
                {
                    FormType = typeof(FrmDeviceData),
                    FormArgs = new object[] { kp, commLine, environment },
                    RelatedObject = kp,
                    NodeType = CommNodeType.Device
                }
            };
        }
    }
}
