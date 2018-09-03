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
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.App.Code;
using Scada.Admin.App.Properties;
using Scada.Admin.Project;
using Scada.Comm;
using Scada.Comm.Devices;
using Scada.Comm.Shell.Code;
using Scada.Server.Modules;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Utils;
using WinControl;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Main form of the application.
    /// <para>Главная форма приложения.</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// The hyperlink to the documentation in English.
        /// </summary>
        private const string DocEnUrl = "http://doc.rapidscada.net/content/en/";
        /// <summary>
        /// The hyperlink to the documentation in Russian.
        /// </summary>
        private const string DocRuUrl = "http://doc.rapidscada.net/content/ru/";
        /// <summary>
        /// The hyperlink to the support in English.
        /// </summary>
        private const string SupportEnUrl = "https://forum.rapidscada.org/";
        /// <summary>
        /// The hyperlink to the support in Russian.
        /// </summary>
        private const string SupportRuUrl = "https://forum.rapidscada.ru/";

        private readonly AppData appData;                 // the common data of the application
        private readonly Log log;                         // the application log
        private readonly ServerShell serverShell;         // the shell to edit Server settings
        private readonly CommShell commShell;             // the shell to edit Communicator settings
        private readonly ExplorerBuilder explorerBuilder; // the object to manipulate the explorer tree
        private ScadaProject project;                     // the project under development
        private Dictionary<string, ModView> moduleViews;  // the user interface of the modules
        private Dictionary<string, KPView> kpViews;       // the user interface of the drivers


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmMain(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            log = appData.ErrLog;
            serverShell = new ServerShell();
            commShell = new CommShell();
            explorerBuilder = new ExplorerBuilder(appData, serverShell, commShell, tvExplorer,
                new ContextMenus() { InstanceMenu = cmsInstance, CommLineMenu = cmsCommLine });
            project = null;
            moduleViews = new Dictionary<string, ModView>();
            kpViews = new Dictionary<string, KPView>();
        }


        /// <summary>
        /// Applies localization to the form.
        /// </summary>
        private void LocalizeForm()
        {
            // load common dictionaries
            if (!Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaData", out string errMsg))
                log.WriteError(errMsg);

            // load Administrator dictionaries
            if (Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaAdmin", out errMsg))
            {
                Translator.TranslateForm(this, "Scada.Admin.App.Forms.FrmMain");
                ofdProject.Filter = AppPhrases.ProjectFileFilter;
            }
            else
            {
                log.WriteError(errMsg);
            }

            // load Server dictionaries
            if (!Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaServer", out errMsg))
                log.WriteError(errMsg);

            // load Communicator dictionaries
            if (!Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaComm", out errMsg))
                log.WriteError(errMsg);

            // read phrases from the dictionaries
            CommonPhrases.Init();

            AdminPhrases.Init();
            AppPhrases.Init();

            ModPhrases.InitFromDictionaries();
            ServerShellPhrases.Init();

            CommPhrases.InitFromDictionaries();
            CommShellPhrases.Init();
        }

        /// <summary>
        /// Takes the explorer images and loads them into an image list.
        /// </summary>
        private void TakeExplorerImages()
        {
            // loading images from resources instead of storing in image list prevents them from corruption
            ilExplorer.Images.Add("chrome.png", Resources.chrome);
            ilExplorer.Images.Add("comm.png", Resources.comm);
            ilExplorer.Images.Add("database.png", Resources.database);
            ilExplorer.Images.Add("empty.png", Resources.empty);
            ilExplorer.Images.Add("folder_closed.png", Resources.folder_closed);
            ilExplorer.Images.Add("folder_open.png", Resources.folder_open);
            ilExplorer.Images.Add("instance.png", Resources.instance);
            ilExplorer.Images.Add("instances.png", Resources.instances);
            ilExplorer.Images.Add("project.png", Resources.project);
            ilExplorer.Images.Add("server.png", Resources.server);
            ilExplorer.Images.Add("table.png", Resources.table);
            ilExplorer.Images.Add("ui.png", Resources.ui);

            // add Server images
            foreach (KeyValuePair<string, Image> pair in serverShell.GetTreeViewImages())
            {
                ilExplorer.Images.Add(pair.Key, pair.Value);
            }

            // add Communicator images
            foreach (KeyValuePair<string, Image> pair in commShell.GetTreeViewImages())
            {
                ilExplorer.Images.Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Executes an action related to the node.
        /// </summary>
        private void ExecNodeAction(TreeNode treeNode)
        {
            if (treeNode.Tag is TreeNodeTag tag)
            {
                if (tag.ExistingForm == null)
                {
                    if (tag.FormType != null)
                    {
                        // create a new form
                        object formObj = tag.FormArgs == null ?
                            Activator.CreateInstance(tag.FormType) :
                            Activator.CreateInstance(tag.FormType, tag.FormArgs);

                        // display the form
                        if (formObj is Form form)
                        {
                            tag.ExistingForm = form;
                            wctrlMain.AddForm(form, treeNode.FullPath, ilExplorer.Images[treeNode.ImageKey], treeNode);
                        }
                    }
                }
                else
                {
                    // activate the existing form
                    wctrlMain.ActivateForm(tag.ExistingForm);
                }
            }
        }

        /// <summary>
        /// Finds a tree node that relates to the specified child form.
        /// </summary>
        private TreeNode FindTreeNode(Form form)
        {
            if (form is IChildForm childForm)
            {
                return childForm.ChildFormTag.TreeNode;
            }
            else
            {
                foreach (TreeNode node in TreeViewUtils.IterateNodes(tvExplorer.Nodes))
                {
                    if (node.Tag is TreeNodeTag tag && tag.ExistingForm == form)
                        return node;
                }

                return null;
            }
        }

        /// <summary>
        /// Updates hints of the child forms corresponding to the specified node and its children.
        /// </summary>
        private void UpdateChildFormHints(TreeNode treeNode)
        {
            foreach (TreeNode node in TreeViewUtils.IterateNodes(treeNode))
            {
                if (node.Tag is TreeNodeTag tag && tag.ExistingForm != null)
                {
                    wctrlMain.UpdateHint(tag.ExistingForm, node.FullPath);
                }
            }
        }

        /// <summary>
        /// Closes the child forms corresponding to the specified node and its children.
        /// </summary>
        private void CloseChildForms(TreeNode treeNode)
        {
            foreach (TreeNode node in TreeViewUtils.IterateNodes(treeNode))
            {
                if (node.Tag is TreeNodeTag tag && tag.ExistingForm != null)
                {
                    wctrlMain.CloseForm(tag.ExistingForm);
                }
            }
        }

        /// <summary>
        /// Finds the closest instance in the explorer starting from the specified node and traversing up.
        /// </summary>
        private bool FindClosestInstance(TreeNode treeNode, out LiveInstance liveInstance)
        {
            TreeNode instanceNode = treeNode?.FindClosest(AppNodeType.Instance);

            if (instanceNode == null)
            {
                liveInstance = null;
                return false;
            }
            {
                liveInstance = (LiveInstance)((TreeNodeTag)instanceNode.Tag).RelatedObject;
                return true;
            }
        }

        /// <summary>
        /// Creates and displays a new project.
        /// </summary>
        private void CreateProject()
        {
            project = new ScadaProject();
            explorerBuilder.CreateNodes(project);
        }

        /// <summary>
        /// Creates a new Server environment for the specified instance.
        /// </summary>
        private ServerEnvironment CreateServerEnvironment(Instance instance)
        {
            return new ServerEnvironment()
            {
                AppDirs = new ServerDirs(appData.AppSettings.ServerDir, instance),
                ModuleViews = moduleViews,
                ErrLog = log
            };
        }

        /// <summary>
        /// Creates a new Communicator environment for the specified instance.
        /// </summary>
        private CommEnvironment CreateCommEnvironment(Instance instance)
        {
            return new CommEnvironment()
            {
                AppDirs = new CommDirs(appData.AppSettings.CommDir, instance),
                KPViews = kpViews,
                ErrLog = log
            };
        }

        /// <summary>
        /// Saves the current project settings.
        /// </summary>
        private void SaveProjectSettings()
        {
            if (!project.Save(project.FileName, out string errMsg))
                appData.ProcError(errMsg);
        }

        /// <summary>
        /// Saves the Communicator settings and optionally updates the explorer.
        /// </summary>
        private bool SaveCommSettigns(Instance instance, TreeNode commLineNode)
        {
            if (instance.CommApp.SaveSettings(out string errMsg))
            {
                if (commLineNode != null)
                {
                    try
                    {
                        tvExplorer.BeginUpdate();
                        commShell.UpdateCommLineNode(commLineNode);
                        UpdateChildFormHints(commLineNode);
                    }
                    finally
                    {
                        tvExplorer.EndUpdate();
                    }
                }

                return true;
            }
            else
            {
                appData.ProcError(errMsg);
                return false;
            }
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();
            TakeExplorerImages();
            CreateProject();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            appData.FinalizeApp();
        }


        private void tvExplorer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // select a tree node on right click
            if (e.Button == MouseButtons.Right && e.Node != null)
                tvExplorer.SelectedNode = e.Node;
        }

        private void tvExplorer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ExecNodeAction(e.Node);
        }

        private void tvExplorer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // load application settings of the instance
            if (e.Node.TagIs(AppNodeType.Instance))
            {
                LiveInstance liveInstance = (LiveInstance)((TreeNodeTag)e.Node.Tag).RelatedObject;
                Instance instance = liveInstance.Instance;

                if (!instance.AppSettingsLoaded)
                {
                    if (instance.LoadAppSettings(out string errMsg))
                    {
                        liveInstance.ServerEnvironment = CreateServerEnvironment(instance);
                        liveInstance.CommEnvironment = CreateCommEnvironment(instance);
                        explorerBuilder.FillInstanceNode(e.Node);
                    }
                    else
                    {
                        appData.ProcError(errMsg);
                    }
                }
            }
        }

        private void tvExplorer_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {

        }

        private void wctrlMain_ChildFormClosed(object sender, ChildFormClosedEventArgs e)
        {
            // clear the form pointer of the node
            TreeNode treeNode = FindTreeNode(e.ChildForm);

            if (treeNode?.Tag is TreeNodeTag tag)
                tag.ExistingForm = null;
        }

        private void wctrlMain_ChildFormMessage(object sender, FormMessageEventArgs e)
        {
            TreeNode sourceNode = FindTreeNode(e.Source);

            if (FindClosestInstance(sourceNode, out LiveInstance liveInstance))
            {
                if (e.Message == ServerMessage.SaveSettings)
                {
                    // save the Server settings
                    if (!liveInstance.Instance.ServerApp.SaveSettings(out string errMsg))
                    {
                        appData.ProcError(errMsg);
                        e.Cancel = true;
                    }
                }
                else if (e.Message == CommMessage.SaveSettings)
                {
                    // save the Communicator settings
                    TreeNode commLineNode = sourceNode.FindClosest(CommNodeType.CommLine);
                    if (!SaveCommSettigns(liveInstance.Instance, commLineNode))
                        e.Cancel = true;
                }
            }
        }


        private void miFileNewProject_Click(object sender, EventArgs e)
        {
            // create a new project
            FrmNewProject frmNewProject = new FrmNewProject(appData);

            if (frmNewProject.ShowDialog() == DialogResult.OK)
            {
                if (ScadaProject.Create(frmNewProject.ProjectName, frmNewProject.ProjectLocation,
                    frmNewProject.ProjectTemplate, out ScadaProject newProject, out string errMsg))
                {
                    project = newProject;
                    explorerBuilder.CreateNodes(project);
                }
                else
                {
                    appData.ProcError(errMsg);
                }
            }
        }

        private void miFileOpenProject_Click(object sender, EventArgs e)
        {
            // open project
            ofdProject.FileName = "";

            if (ofdProject.ShowDialog() == DialogResult.OK)
            {
                ofdProject.InitialDirectory = Path.GetDirectoryName(ofdProject.FileName);
                project = new ScadaProject();

                if (!project.Load(ofdProject.FileName, out string errMsg))
                    appData.ProcError(errMsg);

                explorerBuilder.CreateNodes(project);
            }
        }

        private void miFileSave_Click(object sender, EventArgs e)
        {
            if (wctrlMain.ActiveForm is IChildForm childForm)
            {
                childForm.Save();
            }
        }

        private void miFileSaveAs_Click(object sender, EventArgs e)
        {

        }

        private void miFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miEditCut_Click(object sender, EventArgs e)
        {

        }

        private void miEditCopy_Click(object sender, EventArgs e)
        {

        }

        private void miEditPaste_Click(object sender, EventArgs e)
        {

        }

        private void miToolsOptions_Click(object sender, EventArgs e)
        {

        }

        private void miHelpDoc_Click(object sender, EventArgs e)
        {
            // open the documentation
            Process.Start(Localization.UseRussian ? DocRuUrl : DocEnUrl);
        }

        private void miHelpSupport_Click(object sender, EventArgs e)
        {
            // open the support forum
            Process.Start(Localization.UseRussian ? SupportRuUrl : SupportEnUrl);
        }

        private void miHelpAbout_Click(object sender, EventArgs e)
        {

        }


        private void cmsInstance_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu items
            TreeNode treeNode = tvExplorer.SelectedNode;
            bool isInstanceNode = treeNode != null && treeNode.TagIs(AppNodeType.Instance);
            miInstanceMoveUp.Enabled = isInstanceNode && treeNode.PrevNode != null;
            miInstanceMoveDown.Enabled = isInstanceNode && treeNode.NextNode != null;
            miInstanceDelete.Enabled = isInstanceNode;
            miInstanceRename.Enabled = isInstanceNode;
            miInstanceProperties.Enabled = isInstanceNode;
        }

        private void miInstanceAdd_Click(object sender, EventArgs e)
        {
            // add a new instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null &&
                (selectedNode.TagIs(AppNodeType.Instances) || selectedNode.TagIs(AppNodeType.Instance)))
            {
                FrmInstanceEdit frmInstanceEdit = new FrmInstanceEdit();

                if (frmInstanceEdit.ShowDialog() == DialogResult.OK)
                {
                    Instance instance = project.CreateInstance(frmInstanceEdit.InstanceName);
                    instance.ServerApp.Enabled = frmInstanceEdit.ServerAppEnabled;
                    instance.CommApp.Enabled = frmInstanceEdit.CommAppEnabled;
                    instance.WebApp.Enabled = frmInstanceEdit.WebAppEnabled;

                    if (instance.CreateInstanceFiles(out string errMsg))
                    {
                        TreeNode instancesNode = selectedNode.FindClosest(AppNodeType.Instances);
                        TreeNode instanceNode = explorerBuilder.CreateInstanceNode(instance);
                        instanceNode.Expand();
                        tvExplorer.Insert(instancesNode, instanceNode, project.Instances, instance);
                        SaveProjectSettings();
                    }
                    else
                    {
                        appData.ProcError(errMsg);
                    }
                }
            }
        }

        private void miInstanceMoveUp_Click(object sender, EventArgs e)
        {
            // move up the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Instance))
            {
                tvExplorer.MoveUpSelectedNode(project.Instances);
                SaveProjectSettings();
            }
        }

        private void miInstanceMoveDown_Click(object sender, EventArgs e)
        {
            // move down the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Instance))
            {
                tvExplorer.MoveDownSelectedNode(project.Instances);
                SaveProjectSettings();
            }
        }

        private void miInstanceDelete_Click(object sender, EventArgs e)
        {
            // delete the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance) &&
                MessageBox.Show(AppPhrases.ConfirmDeleteInstance, CommonPhrases.QuestionCaption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseChildForms(selectedNode);
                tvExplorer.RemoveNode(selectedNode, project.Instances);

                if (!liveInstance.Instance.DeleteInstanceFiles(out string errMsg))
                    appData.ProcError(errMsg);

                SaveProjectSettings();
            }
        }

        private void miInstanceRename_Click(object sender, EventArgs e)
        {

        }

        private void miInstanceProperties_Click(object sender, EventArgs e)
        {
            // edit properties of the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                FrmInstanceEdit frmInstanceEdit = new FrmInstanceEdit() { Mode = FrmInstanceEdit.OperatingMode.Edit };
                Instance instance = liveInstance.Instance;
                frmInstanceEdit.Init(instance);

                if (frmInstanceEdit.ShowDialog() == DialogResult.OK)
                {
                    // enable or disable the applications
                    bool projectModified = false;
                    ScadaApp[] scadaApps = new ScadaApp[] { instance.ServerApp, instance.CommApp, instance.WebApp };

                    foreach (ScadaApp scadaApp in scadaApps)
                    {
                        bool prevState = scadaApp.Enabled;
                        bool curState = frmInstanceEdit.GetAppEnabled(scadaApp);

                        if (!prevState && curState)
                        {
                            if (scadaApp.CreateAppFiles(out string errMsg))
                            {
                                scadaApp.Enabled = true;
                                projectModified = true;
                            }
                            else
                            {
                                appData.ProcError(errMsg);
                            }
                        }

                        if (prevState && !curState)
                        {
                            if (scadaApp.DeleteAppFiles(out string errMsg))
                            {
                                scadaApp.Enabled = false;
                                projectModified = true;
                            }
                            else
                            {
                                appData.ProcError(errMsg);
                            }
                        }
                    }

                    // save the changes and update the explorer
                    if (projectModified)
                    {
                        CloseChildForms(selectedNode);
                        explorerBuilder.FillInstanceNode(selectedNode);
                        SaveProjectSettings();
                    }
                }
            }
        }


        private void cmsCommLine_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu items
            TreeNode treeNode = tvExplorer.SelectedNode;
            bool isLineNode = treeNode != null && treeNode.TagIs(CommNodeType.CommLine);
            miCommLineMoveUp.Enabled = isLineNode && treeNode.PrevNode != null;
            miCommLineMoveDown.Enabled = isLineNode && treeNode.NextNode != null;
            miCommLineDelete.Enabled = isLineNode;
        }

        private void miCommLineAdd_Click(object sender, EventArgs e)
        {
            // add a new communication line
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null &&
                (selectedNode.TagIs(CommNodeType.CommLines) || selectedNode.TagIs(CommNodeType.CommLine)) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                TreeNode commLinesNode = selectedNode.FindClosest(CommNodeType.CommLines);
                TreeNode commLineNode = commShell.CreateCommLineNode(liveInstance.CommEnvironment);
                commLineNode.ContextMenuStrip = cmsCommLine;
                commLineNode.Expand();
                tvExplorer.Insert(commLinesNode, commLineNode);
                SaveCommSettigns(liveInstance.Instance, null);
            }
        }

        private void miCommLineMoveUp_Click(object sender, EventArgs e)
        {
            // move up the selected communication line
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                tvExplorer.MoveUpSelectedNode(TreeViewUtils.MoveBehavior.WithinParent);
                SaveCommSettigns(liveInstance.Instance, null);
            }
        }

        private void miCommLineMoveDown_Click(object sender, EventArgs e)
        {
            // move up the selected communication line
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                tvExplorer.MoveDownSelectedNode(TreeViewUtils.MoveBehavior.WithinParent);
                SaveCommSettigns(liveInstance.Instance, null);
            }
        }

        private void miCommLineDelete_Click(object sender, EventArgs e)
        {
            // delete the selected communication line
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance) &&
                MessageBox.Show(AppPhrases.ConfirmDeleteCommLine, CommonPhrases.QuestionCaption, 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseChildForms(selectedNode);
                tvExplorer.RemoveSelectedNode();
                SaveCommSettigns(liveInstance.Instance, null);
            }
        }
    }
}
