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
 * Module   : Communicator Shell
 * Summary  : Provides access to the Communicator shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Comm.Shell.Forms;
using Scada.Comm.Shell.Properties;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                { "comm_lines.png", Resources.comm_lines },
                { "comm_params.png", Resources.comm_params },
                { "comm_stats.png", Resources.comm_stats },
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

            environment.Validate();

            // create nodes
            List<TreeNode> nodes = new List<TreeNode>();

            nodes.Add(new TreeNode(CommShellPhrases.CommonParamsNode)
            {
                ImageKey = "comm_params.png",
                SelectedImageKey = "comm_params.png",
                Tag = new TreeNodeTag()
                {
                    FormType = typeof(FrmCommonParams),
                    FormArgs = new object[] { settings.Params }
                }
            });

            nodes.Add(new TreeNode(CommShellPhrases.DriversNode)
            {
                ImageKey = "comm_driver.png",
                SelectedImageKey = "comm_driver.png",
                Tag = new TreeNodeTag()
                {
                    FormType = typeof(FrmDrivers),
                    FormArgs = new object[] { environment }
                }
            });

            TreeNode linesNode = new TreeNode(CommShellPhrases.LinesNode)
            {
                ImageKey = "comm_lines.png",
                SelectedImageKey = "comm_lines.png"
            };

            nodes.Add(linesNode);

            foreach (Settings.CommLine commLine in settings.CommLines)
            {
                TreeNode lineNode = new TreeNode(string.Format(CommShellPhrases.LineNode, 
                    commLine.Number, commLine.Name))
                {
                    ImageKey = "comm_line.png",
                    SelectedImageKey = "comm_line.png"
                };

                linesNode.Nodes.Add(lineNode);
                TreeNodeCollection lineSubNodes = lineNode.Nodes;

                lineSubNodes.Add(new TreeNode(CommShellPhrases.LineParamsNode)
                {
                    ImageKey = "comm_params.png",
                    SelectedImageKey = "comm_params.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = null,
                        FormArgs = new object[] { commLine }
                    }
                });

                lineSubNodes.Add(new TreeNode(CommShellPhrases.LineStatsNode)
                {
                    ImageKey = "comm_stats.png",
                    SelectedImageKey = "comm_stats.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = null,
                        FormArgs = new object[] { commLine }
                    }
                });

                foreach (Settings.KP kp in commLine.ReqSequence)
                {
                    lineSubNodes.Add(new TreeNode(string.Format(CommShellPhrases.DeviceNode, kp.Number, kp.Name))
                    {
                        ImageKey = "comm_device.png",
                        SelectedImageKey = "comm_device.png",
                        Tag = new TreeNodeTag()
                        {
                            FormType = null,
                            FormArgs = new object[] { kp }
                        }
                    });
                }
            }

            nodes.Add(new TreeNode(CommShellPhrases.StatsNode)
            {
                ImageKey = "comm_stats.png",
                SelectedImageKey = "comm_stats.png",
                Tag = new TreeNodeTag()
                {
                    FormType = null,
                    FormArgs = new object[] { settings }
                }
            });

            return nodes.ToArray();
        }
    }
}
