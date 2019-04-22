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
 * Module   : Server Shell
 * Summary  : Provides access to the Server shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Server.Shell.Forms;
using Scada.Server.Shell.Properties;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Server.Shell.Code
{
    /// <summary>
    /// Provides access to the Server shell.
    /// <para>Обеспечивает доступ к оболочке Сервера.</para>
    /// </summary>
    public class ServerShell
    {
        /// <summary>
        /// Gets the images used by the explorer.
        /// </summary>
        public Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { "server_archive.png", Resources.server_archive },
                { "server_data.png", Resources.server_data },
                { "server_event.png", Resources.server_event },
                { "server_generator.png", Resources.server_generator },
                { "server_module.png", Resources.server_module },
                { "server_params.png", Resources.server_params },
                { "server_save.png", Resources.server_save },
                { "server_stats.png", Resources.server_stats }
            };
        }

        /// <summary>
        /// Gets the tree nodes for the explorer.
        /// </summary>
        public TreeNode[] GetTreeNodes(Settings settings, ServerEnvironment environment)
        {
            // check the arguments
            if (settings == null)
                throw new ArgumentNullException("settings");

            if (environment == null)
                throw new ArgumentNullException("environment");

            // create nodes
            return new TreeNode[]
            {
                new TreeNode(ServerShellPhrases.CommonParamsNode)
                {
                    ImageKey = "server_params.png",
                    SelectedImageKey = "server_params.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmCommonParams),
                        FormArgs = new object[] { settings }
                    }
                },
                new TreeNode(ServerShellPhrases.SaveParamsNode)
                {
                    ImageKey = "server_save.png",
                    SelectedImageKey = "server_save.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmSaveParams),
                        FormArgs = new object[] { settings }
                    }
                },
                new TreeNode(ServerShellPhrases.ModulesNode)
                {
                    ImageKey = "server_module.png",
                    SelectedImageKey = "server_module.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmModules),
                        FormArgs = new object[] { settings, environment }
                    }
                },
                new TreeNode(ServerShellPhrases.ArchiveNode, 
                    new TreeNode[] 
                    {
                        new TreeNode(ServerShellPhrases.CurDataNode)
                        {
                            ImageKey = "server_data.png",
                            SelectedImageKey = "server_data.png",
                            Tag = new TreeNodeTag()
                            {
                                FormType = typeof(FrmArchive),
                                FormArgs = new object[] { settings, environment, ArcType.CurData }
                            }
                        },
                        new TreeNode(ServerShellPhrases.MinDataNode)
                        {
                            ImageKey = "server_data.png",
                            SelectedImageKey = "server_data.png",
                            Tag = new TreeNodeTag()
                            {
                                FormType = typeof(FrmArchive),
                                FormArgs = new object[] { settings, environment, ArcType.MinData }
                            }
                        },
                        new TreeNode(ServerShellPhrases.HourDataNode)
                        {
                            ImageKey = "server_data.png",
                            SelectedImageKey = "server_data.png",
                            Tag = new TreeNodeTag()
                            {
                                FormType = typeof(FrmArchive),
                                FormArgs = new object[] { settings, environment, ArcType.HourData }
                            }
                        },
                        new TreeNode(ServerShellPhrases.EventsNode)
                        {
                            ImageKey = "server_event.png",
                            SelectedImageKey = "server_event.png",
                            Tag = new TreeNodeTag()
                            {
                                FormType = typeof(FrmArchive),
                                FormArgs = new object[] { settings, environment, ArcType.Events }
                            }
                        }
                    })
                {
                    ImageKey = "server_archive.png",
                    SelectedImageKey = "server_archive.png"
                },
                new TreeNode(ServerShellPhrases.GeneratorNode)
                {
                    ImageKey = "server_generator.png",
                    SelectedImageKey = "server_generator.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmGenerator),
                        FormArgs = new object[] { settings, environment }
                    }
                },
                new TreeNode(ServerShellPhrases.StatsNode)
                {
                    ImageKey = "server_stats.png",
                    SelectedImageKey = "server_stats.png",
                    Tag = new TreeNodeTag()
                    {
                        FormType = typeof(FrmStats),
                        FormArgs = new object[] { environment }
                    }
                },
            };
        }
    }
}
