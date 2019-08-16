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
 * Module   : Administrator
 * Summary  : Form for import Communicator settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Comm;
using Scada.Comm.Devices;
using Scada.Comm.Shell.Code;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Form for import Communicator settings.
    /// <para>Форма импорта настроек Коммуникатора.</para>
    /// </summary>
    public partial class FrmCommImport : Form
    {
        private readonly ScadaProject project;            // the project under development
        private readonly Instance instance;               // the import destination instance
        private readonly CommEnvironment commEnvironment; // the Communicator environment


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCommImport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCommImport(ScadaProject project, Instance instance, CommEnvironment commEnvironment)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.instance = instance ?? throw new ArgumentNullException("instance");
            this.commEnvironment = commEnvironment ?? throw new ArgumentNullException("commEnvironment");

            CommLineSettings = null;
            ImportedCommLines = null;
            ImportedDevices = null;
        }


        /// <summary>
        /// Gets or sets the communication line which devices are imported.
        /// </summary>
        public Settings.CommLine CommLineSettings { get; set; }

        /// <summary>
        /// Gets the imported communication lines.
        /// </summary>
        public List<Settings.CommLine> ImportedCommLines { get; private set; }

        /// <summary>
        /// Gets the imported devices.
        /// </summary>
        public List<Settings.KP> ImportedDevices { get; private set; }


        /// <summary>
        /// Fills the tree by communication lines and devices.
        /// </summary>
        private void FillTreeView()
        {
            try
            {
                treeView.BeginUpdate();

                if (CommLineSettings == null)
                {
                    foreach (CommLine commLineEntity in project.ConfigBase.CommLineTable.Items.Values)
                    {
                        AddCommLineNode(commLineEntity);
                    }
                }
                else
                {
                    if (project.ConfigBase.CommLineTable.Items.TryGetValue(CommLineSettings.Number, 
                        out CommLine commLineEntity))
                    {
                        AddCommLineNode(commLineEntity).Expand();
                    }
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Adds a node corresponding to the communication line.
        /// </summary>
        private TreeNode AddCommLineNode(CommLine commLineEntity)
        {
            TreeNode commLineNode = new TreeNode(string.Format(CommShellPhrases.CommLineNode,
                commLineEntity.CommLineNum, commLineEntity.Name))
            {
                Tag = commLineEntity
            };

            foreach (KP kp in project.ConfigBase.KPTable.SelectItems(
                new TableFilter("CommLineNum", commLineEntity.CommLineNum)))
            {
                commLineNode.Nodes.Add(new TreeNode(string.Format(CommShellPhrases.DeviceNode, kp.KPNum, kp.Name))
                {
                    Tag = kp
                });
            }

            treeView.Nodes.Add(commLineNode);
            return commLineNode;
        }

        /// <summary>
        /// Imports communication lines and devices.
        /// </summary>
        private void Import(out bool noData)
        {
            noData = true;
            ImportedCommLines = new List<Settings.CommLine>();
            ImportedDevices = new List<Settings.KP>();
            Settings settings = instance.CommApp.Settings;

            foreach (TreeNode commLineNode in treeView.Nodes)
            {
                if (commLineNode.Checked)
                {
                    CommLine commLineEntity = (CommLine)commLineNode.Tag;
                    Settings.CommLine commLineSettings = CommLineSettings;

                    if (commLineSettings == null)
                    {
                        // import communication line
                        noData = false;
                        commLineSettings = SettingsConverter.CreateCommLine(commLineEntity);
                        commLineSettings.Parent = settings;
                        settings.CommLines.Add(commLineSettings);
                        ImportedCommLines.Add(commLineSettings);
                    }

                    foreach (TreeNode kpNode in commLineNode.Nodes)
                    {
                        if (kpNode.Checked)
                        {
                            // import device
                            noData = false;
                            KP kpEntity = (KP)kpNode.Tag;
                            Settings.KP kpSettings = SettingsConverter.CreateKP(kpEntity, 
                                project.ConfigBase.KPTypeTable);
                            kpSettings.Parent = commLineSettings;

                            if (commEnvironment.TryGetKPView(kpSettings, true, null, out KPView kpView, out string errMsg))
                                kpSettings.SetReqParams(kpView.DefaultReqParams);

                            commLineSettings.ReqSequence.Add(kpSettings);
                            ImportedDevices.Add(kpSettings);
                        }
                    }
                }
            }
        }


        private void FrmCommImport_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            FillTreeView();
        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView.AfterCheck -= treeView_AfterCheck;
            TreeNode node = e.Node;
            bool nodeChecked = node.Checked;

            // select a parent node
            if (node.Parent != null && nodeChecked)
                node.Parent.Checked = true;

            // select child nodes
            foreach (TreeNode childNode in node.Nodes)
            {
                childNode.Checked = nodeChecked;
            }

            treeView.AfterCheck += treeView_AfterCheck;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Import(out bool noData);

            if (noData)
                ScadaUiUtils.ShowWarning(AppPhrases.NoDataSelected);
            else
                DialogResult = DialogResult.OK;
        }
    }
}
