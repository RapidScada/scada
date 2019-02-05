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
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.App.Forms.Deployment;
using Scada.Admin.App.Forms.Tools;
using Scada.Admin.App.Properties;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent.Connector;
using Scada.Comm;
using Scada.Comm.Devices;
using Scada.Comm.Shell.Code;
using Scada.Comm.Shell.Forms;
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
            explorerBuilder = new ExplorerBuilder(appData, serverShell, commShell, tvExplorer, new ContextMenus() {
                ProjectMenu = cmsProject, DirectoryMenu = cmsDirectory, FileItemMenu = cmsFileItem,
                InstanceMenu = cmsInstance, ServerMenu = cmsServer, CommMenu = cmsComm, CommLineMenu = cmsCommLine,
                DeviceMenu = cmsDevice });
            project = null;
            moduleViews = new Dictionary<string, ModView>();
            kpViews = new Dictionary<string, KPView>();
        }


        /// <summary>
        /// Gets the root node of the explorer.
        /// </summary>
        private TreeNode RootNode
        {
            get
            {
                return tvExplorer.Nodes[0];
            }
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
            bool adminDictLoaded = Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaAdmin", out errMsg);
            if (!adminDictLoaded)
                log.WriteError(errMsg);

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

            if (adminDictLoaded)
            {
                Translator.TranslateForm(this, "Scada.Admin.App.Forms.FrmMain", null,
                    cmsProject, cmsDirectory, cmsFileItem, cmsInstance, cmsServer, cmsComm, cmsCommLine, cmsDevice);
                Text = AppPhrases.EmptyTitle;
                wctrlMain.MessageText = AppPhrases.WelcomeMessage;
                ofdProject.Filter = AppPhrases.ProjectFileFilter;
            }

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
            ilExplorer.Images.Add("file.png", Resources.file);
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
        /// Sets the initial state of menu items and tool buttons.
        /// </summary>
        private void InitMenuItems()
        {
            miFileSave.Enabled = btnFileSave.Enabled = false;
            miFileSaveAll.Enabled = btnFileSaveAll.Enabled = false;

            miEditCut.Enabled = btnEditCut.Enabled = false;
            miEditCopy.Enabled = btnEditCopy.Enabled = false;
            miEditPaste.Enabled = btnEditPaste.Enabled = false;

            miToolsOptions.Enabled = false;
        }

        /// <summary>
        /// Disables the Save All menu item and the corresponding tool button if all the forms are saved.
        /// </summary>
        private void DisableSaveAll()
        {
            if (miFileSaveAll.Enabled)
            {
                bool saveAllEnabled = false;

                foreach (Form form in wctrlMain.Forms)
                {
                    if (form is IChildForm childForm && childForm.ChildFormTag.Modified)
                    {
                        saveAllEnabled = true;
                        break;
                    }
                }

                miFileSaveAll.Enabled = btnFileSaveAll.Enabled = saveAllEnabled;
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
        /// Executes an action to open the file associated with the node.
        /// </summary>
        private void ExecOpenFileAction(TreeNode treeNode)
        {
            if (treeNode.Tag is TreeNodeTag tag && tag.RelatedObject is FileItem fileItem)
            {
                if (tag.ExistingForm == null)
                {
                    // create and display a new text editor form
                    FrmTextEditor form = new FrmTextEditor(appData, fileItem.Path);
                    tag.ExistingForm = form;
                    wctrlMain.AddForm(form, treeNode.FullPath, ilExplorer.Images[treeNode.ImageKey], treeNode);
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
        /// Updates file names of the opened text editors corresponding to the specified node and its children.
        /// </summary>
        private void UpdateTextEditorFileNames(TreeNode treeNode)
        {
            foreach (TreeNode node in TreeViewUtils.IterateNodes(treeNode))
            {
                if (node.Tag is TreeNodeTag tag && tag.RelatedObject is FileItem fileItem &&
                    node.Parent.Tag is TreeNodeTag parentTag && parentTag.RelatedObject is FileItem parenFileItem)
                {
                    fileItem.Path = Path.Combine(parenFileItem.Path, fileItem.Name);

                    if (tag.ExistingForm is FrmTextEditor form)
                    {
                        wctrlMain.UpdateHint(form, node.FullPath);
                        form.ChildFormTag.SendMessage(this, AppMessage.UpdateFileName,
                            new Dictionary<string, object> { { "FileName", fileItem.Path } });
                    }
                }
            }
        }

        /// <summary>
        /// Updates nodes and forms of the communication line.
        /// </summary>
        private void UpdateCommLine(TreeNode commLineNode, CommEnvironment commEnvironment)
        {
            // close or update child forms
            foreach (TreeNode node in commLineNode.Nodes)
            {
                if (node.Tag is TreeNodeTag tag && tag.ExistingForm != null)
                {
                    if (node.TagIs(CommNodeType.Device))
                        wctrlMain.CloseForm(tag.ExistingForm);
                    else if (node.TagIs(CommNodeType.LineStats))
                        ((IChildForm)tag.ExistingForm).ChildFormTag.SendMessage(this, CommMessage.UpdateFileName);
                }
            }

            // update the explorer
            try
            {
                tvExplorer.BeginUpdate();
                commShell.UpdateCommLineNode(commLineNode, commEnvironment);
            }
            finally
            {
                tvExplorer.EndUpdate();
            }

            // update form hints
            UpdateChildFormHints(commLineNode);
        }

        /// <summary>
        /// Updates parameters of the open communication line related to the specified node.
        /// </summary>
        private void UpdateLineParams(TreeNode siblingNode)
        {
            if (siblingNode?.FindSibling(CommNodeType.LineParams) is TreeNode lineParamsNode &&
                ((TreeNodeTag)lineParamsNode.Tag).ExistingForm is IChildForm childForm)
            {
                childForm.ChildFormTag.SendMessage(this, CommMessage.UpdateLineParams);
            }
        }

        /// <summary>
        /// Closes the child forms corresponding to the specified node and its children.
        /// </summary>
        private void CloseChildForms(TreeNode treeNode, bool save = false)
        {
            foreach (TreeNode node in TreeViewUtils.IterateNodes(treeNode))
            {
                if (node.Tag is TreeNodeTag tag && tag.ExistingForm != null)
                {
                    if (save && tag.ExistingForm is IChildForm childForm && childForm.ChildFormTag.Modified)
                        childForm.Save();

                    wctrlMain.CloseForm(tag.ExistingForm);
                }
            }
        }

        /// <summary>
        /// Saves the child forms corresponding to the specified node and its children.
        /// </summary>
        private void SaveChildForms(TreeNode treeNode)
        {
            foreach (TreeNode node in TreeViewUtils.IterateNodes(treeNode))
            {
                if (node.Tag is TreeNodeTag tag && 
                    tag.ExistingForm is IChildForm childForm && childForm.ChildFormTag.Modified)
                {
                    childForm.Save();
                }
            }
        }

        /// <summary>
        /// Gets the path associated with the specified node.
        /// </summary>
        private bool TryGetFilePath(TreeNode treeNode, out string path)
        {
            if (treeNode?.Tag is TreeNodeTag tag)
            {
                if (tag.NodeType == AppNodeType.Project)
                {
                    path = project.ProjectDir;
                    return true;
                }
                else if (tag.NodeType == AppNodeType.Directory || tag.NodeType == AppNodeType.File)
                {
                    FileItem fileItem = (FileItem)tag.RelatedObject;
                    path = fileItem.Path;
                    return true;
                }
                else if (tag.NodeType == AppNodeType.Interface)
                {
                    path = project.Interface.InterfaceDir;
                    return true;
                }
                else if (tag.NodeType == AppNodeType.Instance)
                {
                    if (FindClosestInstance(treeNode, out LiveInstance liveInstance))
                    {
                        path = liveInstance.Instance.InstanceDir;
                        return true;
                    }
                }
                else if (tag.NodeType == AppNodeType.ServerApp)
                {
                    if (FindClosestInstance(treeNode, out LiveInstance liveInstance))
                    {
                        path = liveInstance.Instance.ServerApp.AppDir;
                        return true;
                    }
                }
                else if (tag.NodeType == AppNodeType.CommApp)
                {
                    if (FindClosestInstance(treeNode, out LiveInstance liveInstance))
                    {
                        path = liveInstance.Instance.CommApp.AppDir;
                        return true;
                    }
                }
                else if (tag.NodeType == AppNodeType.WebApp)
                {
                    if (FindClosestInstance(treeNode, out LiveInstance liveInstance))
                    {
                        path = liveInstance.Instance.WebApp.AppDir;
                        return true;
                    }
                }
            }

            path = "";
            return false;
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
        /// Prepares data and fills the instance node.
        /// </summary>
        private void PrepareInstanceNode(TreeNode instanceNode, LiveInstance liveInstance)
        {
            Instance instance = liveInstance.Instance;

            if (!instance.AppSettingsLoaded)
            {
                if (instance.LoadAppSettings(out string errMsg))
                {
                    IAgentClient agentClient = CreateAgentClient(instance);
                    liveInstance.ServerEnvironment = CreateServerEnvironment(instance, agentClient);
                    liveInstance.CommEnvironment = CreateCommEnvironment(instance, agentClient);
                    explorerBuilder.FillInstanceNode(instanceNode);
                }
                else
                {
                    appData.ProcError(errMsg);
                }
            }
        }

        /// <summary>
        /// Refreshes the content of the instance node.
        /// </summary>
        private void RefreshInstanceNode(TreeNode instanceNode, LiveInstance liveInstance)
        {
            Instance instance = liveInstance.Instance;

            if (instance.AppSettingsLoaded)
                explorerBuilder.FillInstanceNode(instanceNode);
            else
                PrepareInstanceNode(instanceNode, liveInstance);
        }

        /// <summary>
        /// Finds an instance selected for deploy.
        /// </summary>
        private bool FindInstanceForDeploy(TreeNode treeNode, out TreeNode instanceNode, out LiveInstance liveInstance)
        {
            if (project != null)
            {
                instanceNode = project.Instances.Count == 1 ?
                    RootNode.FindFirst(AppNodeType.Instance) :
                    treeNode?.FindClosest(AppNodeType.Instance);

                if (instanceNode != null)
                {
                    liveInstance = (LiveInstance)((TreeNodeTag)instanceNode.Tag).RelatedObject;
                    return true;
                }
            }

            instanceNode = null;
            liveInstance = null;
            return false;
        }

        /// <summary>
        /// Creates a new Server environment for the specified instance.
        /// </summary>
        private ServerEnvironment CreateServerEnvironment(Instance instance, IAgentClient agentClient)
        {
            return new ServerEnvironment()
            {
                AppDirs = new ServerDirs(appData.AppSettings.ServerDir, instance),
                ModuleViews = moduleViews,
                AgentClient = agentClient,
                ErrLog = log
            };
        }

        /// <summary>
        /// Creates a new Communicator environment for the specified instance.
        /// </summary>
        private CommEnvironment CreateCommEnvironment(Instance instance, IAgentClient agentClient)
        {
            return new CommEnvironment()
            {
                AppDirs = new CommDirs(appData.AppSettings.CommDir, instance),
                KPViews = kpViews,
                AgentClient = agentClient,
                ErrLog = log
            };
        }

        /// <summary>
        /// Gets the deployment profile by name.
        /// </summary>
        private DeploymentProfile GetDeploymentProfile(string profileName)
        {
            if (!string.IsNullOrEmpty(profileName) &&
                project.DeploymentSettings.Profiles.TryGetValue(profileName, out DeploymentProfile profile))
            {
                return profile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new Agent client.
        /// </summary>
        private IAgentClient CreateAgentClient(Instance instance)
        {
            DeploymentProfile profile = GetDeploymentProfile(instance.DeploymentProfile);

            if (profile == null)
            {
                return null;
            }
            else
            {
                ConnectionSettings connSettings = profile.ConnectionSettings.Clone();
                connSettings.ScadaInstance = instance.Name;
                return new AgentWcfClient(connSettings);
            }
        }

        /// <summary>
        /// Updates the Agent client of the instance.
        /// </summary>
        private void UpdateAgentClient(LiveInstance liveInstance)
        {
            if (liveInstance.ServerEnvironment != null || liveInstance.CommEnvironment != null)
            {
                IAgentClient agentClient = CreateAgentClient(liveInstance.Instance);

                if (liveInstance.ServerEnvironment != null)
                    liveInstance.ServerEnvironment.AgentClient = agentClient;

                if (liveInstance.CommEnvironment != null)
                    liveInstance.CommEnvironment.AgentClient = agentClient;
            }
        }

        /// <summary>
        /// Loads the deployments settings of the current project.
        /// </summary>
        private void LoadDeploymentSettings()
        {
            if (File.Exists(project.DeploymentSettings.FileName) &&
                !project.DeploymentSettings.Load(out string errMsg))
            {
                appData.ProcError(errMsg);
            }
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
        private bool SaveCommSettigns(LiveInstance liveInstance)
        {
            if (liveInstance.Instance.CommApp.SaveSettings(out string errMsg))
            {
                return true;
            }
            else
            {
                appData.ProcError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Loads the configuration database.
        /// </summary>
        private void LoadConfigBase()
        {
            if (!project.ConfigBase.Load(out string errMsg))
                appData.ProcError(errMsg);
        }

        /// <summary>
        /// Saves the configuration database.
        /// </summary>
        private void SaveConfigBase()
        {
            if (!project.ConfigBase.Save(out string errMsg))
                appData.ProcError(errMsg);
        }

        /// <summary>
        /// Save all open forms.
        /// </summary>
        private void SaveAll()
        {
            foreach (Form form in wctrlMain.Forms)
            {
                if (form is IChildForm childForm && childForm.ChildFormTag.Modified)
                    childForm.Save();
            }

            SaveConfigBase();
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();
            TakeExplorerImages();
            InitMenuItems();
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
            // execute a node action on double click
            if (e.Button == MouseButtons.Left)
            {
                TreeNode node = e.Node;
                if (node.TagIs(AppNodeType.File))
                    ExecOpenFileAction(node);
                else
                    ExecNodeAction(node);
            }
        }

        private void tvExplorer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // fill a node on demand
            TreeNode treeNode = e.Node;

            if (treeNode.TagIs(AppNodeType.Interface))
            {
                explorerBuilder.FillInterfaceNode(treeNode);
            }
            else if (treeNode.TagIs(AppNodeType.Instance))
            {
                LiveInstance liveInstance = (LiveInstance)((TreeNodeTag)e.Node.Tag).RelatedObject;
                LoadDeploymentSettings();
                PrepareInstanceNode(treeNode, liveInstance);
            }
            else if (treeNode.TagIs(AppNodeType.WebApp))
            {
                explorerBuilder.FillWebstationNode(treeNode);
            }
        }

        private void tvExplorer_AfterExpand(object sender, TreeViewEventArgs e)
        {
            explorerBuilder.SetFolderImage(e.Node);
        }

        private void tvExplorer_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            explorerBuilder.SetFolderImage(e.Node);
        }


        private void wctrlMain_ActiveFormChanged(object sender, EventArgs e)
        {
            // enable or disable the Save menu item
            miFileSave.Enabled = btnFileSave.Enabled = 
                wctrlMain.ActiveForm is IChildForm childForm && childForm.ChildFormTag.Modified;
        }

        private void wctrlMain_ChildFormClosed(object sender, ChildFormClosedEventArgs e)
        {
            // clear the form pointer of the node
            TreeNode treeNode = FindTreeNode(e.ChildForm);

            if (treeNode?.Tag is TreeNodeTag tag)
                tag.ExistingForm = null;

            // disable the Save All menu item if needed
            if (e.ChildForm is IChildForm childForm && childForm.ChildFormTag.Modified)
                DisableSaveAll();
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
                    if (SaveCommSettigns(liveInstance))
                        UpdateCommLine(commLineNode, liveInstance.CommEnvironment);
                    else
                        e.Cancel = true;
                }
                else if (e.Message == CommMessage.UpdateLineParams)
                {
                    // refresh parameters of the specified line if they are open
                    UpdateLineParams(FindTreeNode(e.Source));
                    SaveCommSettigns(liveInstance);
                }
            }
        }

        private void wctrlMain_ChildFormModifiedChanged(object sender, ChildFormEventArgs e)
        {
            // enable or disable the Save menu items
            miFileSave.Enabled = btnFileSave.Enabled =
                wctrlMain.ActiveForm is IChildForm activeForm && activeForm.ChildFormTag.Modified;

            if (e.ChildForm is IChildForm childForm)
            {
                if (childForm.ChildFormTag.Modified)
                    miFileSaveAll.Enabled = btnFileSaveAll.Enabled = true;
                else
                    DisableSaveAll();
            }
        }


        private void miFile_DropDownOpening(object sender, EventArgs e)
        {
            // enable or disable the menu items
            miFileClose.Enabled = wctrlMain.ActiveForm != null;
            miFileCloseProject.Enabled = project != null;
        }

        private void miFileNewProject_Click(object sender, EventArgs e)
        {
            // create a new project
            FrmProjectNew frmNewProject = new FrmProjectNew(appData);

            if (frmNewProject.ShowDialog() == DialogResult.OK)
            {
                if (ScadaProject.Create(frmNewProject.ProjectName, frmNewProject.ProjectLocation,
                    frmNewProject.ProjectTemplate, out ScadaProject newProject, out string errMsg))
                {
                    project = newProject;
                    Text = string.Format(AppPhrases.ProjectTitle, project.Name);
                    wctrlMain.MessageText = AppPhrases.SelectItemMessage;
                    InitMenuItems();
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
                wctrlMain.CloseAllForms(out bool cancel);
                ofdProject.InitialDirectory = Path.GetDirectoryName(ofdProject.FileName);
                project = new ScadaProject();

                if (!project.Load(ofdProject.FileName, out string errMsg))
                    appData.ProcError(errMsg);

                Text = string.Format(AppPhrases.ProjectTitle, project.Name);
                wctrlMain.MessageText = AppPhrases.SelectItemMessage;
                InitMenuItems();
                explorerBuilder.CreateNodes(project);
            }
        }

        private void miFileSave_Click(object sender, EventArgs e)
        {
            // save the active form
            if (wctrlMain.ActiveForm is IChildForm childForm)
                childForm.Save();
        }

        private void miFileSaveAll_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        private void miFileClose_Click(object sender, EventArgs e)
        {
            // close the active form
            wctrlMain.CloseActiveForm(out bool cancel);
        }

        private void miFileCloseProject_Click(object sender, EventArgs e)
        {
            // close the project
            wctrlMain.CloseAllForms(out bool cancel);

            if (!cancel)
            {
                project = null;
                Text = AppPhrases.EmptyTitle;
                wctrlMain.MessageText = AppPhrases.WelcomeMessage;
                InitMenuItems();
                tvExplorer.Nodes.Clear();
            }
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

        private void miDeploy_DropDownOpening(object sender, EventArgs e)
        {
            // enable or disable the menu items
            bool deployInstanceFound = project != null && (project.Instances.Count == 1 ||
                tvExplorer.SelectedNode?.FindClosest(AppNodeType.Instance) != null);
            miDeployInstanceProfile.Enabled = deployInstanceFound;
            miDeployDownloadConfig.Enabled = deployInstanceFound;
            miDeployUploadConfig.Enabled = deployInstanceFound;
            miDeployInstanceStatus.Enabled = deployInstanceFound;
        }

        private void miDeployInstanceProfile_Click(object sender, EventArgs e)
        {
            // select instance profile
            if (FindInstanceForDeploy(tvExplorer.SelectedNode,
                out TreeNode instanceNode, out LiveInstance liveInstance))
            {
                // load deployment settings
                LoadDeploymentSettings();

                // open an instance profile form
                FrmInstanceProfile frmInstanceProfile = new FrmInstanceProfile(appData, project, liveInstance.Instance);
                frmInstanceProfile.ShowDialog();

                // take the changes into account
                if (frmInstanceProfile.ProfileChanged)
                {
                    UpdateAgentClient(liveInstance);
                    SaveProjectSettings();
                }
                else if (frmInstanceProfile.ConnSettingsModified)
                {
                    UpdateAgentClient(liveInstance);
                }
            }
        }

        private void miDeployDownloadConfig_Click(object sender, EventArgs e)
        {
            // download configuration
            if (FindInstanceForDeploy(tvExplorer.SelectedNode, 
                out TreeNode instanceNode, out LiveInstance liveInstance))
            {
                // save and load the required data
                SaveAll();
                LoadConfigBase();
                LoadDeploymentSettings();

                // open a download configuration form
                FrmDownloadConfig frmDownloadConfig = new FrmDownloadConfig(appData, project, liveInstance.Instance);
                frmDownloadConfig.ShowDialog();

                // take the changes into account
                if (frmDownloadConfig.ProfileChanged)
                {
                    UpdateAgentClient(liveInstance);
                    SaveProjectSettings();
                }
                else if (frmDownloadConfig.ConnSettingsModified)
                {
                    UpdateAgentClient(liveInstance);
                }

                if (frmDownloadConfig.BaseModified)
                {
                    TreeNode baseNode = RootNode.FindFirst(AppNodeType.Base);
                    CloseChildForms(baseNode);
                    SaveConfigBase();
                }

                if (frmDownloadConfig.InterfaceModified)
                {
                    TreeNode interfaceNode = RootNode.FindFirst(AppNodeType.Interface);
                    if (TryGetFilePath(interfaceNode, out string path))
                    {
                        CloseChildForms(interfaceNode);
                        explorerBuilder.FillFileNode(interfaceNode, path);
                    }
                }

                if (frmDownloadConfig.InstanceModified)
                {
                    CloseChildForms(instanceNode);
                    RefreshInstanceNode(instanceNode, liveInstance);
                }
            }
        }

        private void miDeployUploadConfig_Click(object sender, EventArgs e)
        {
            // upload configuration
            if (FindInstanceForDeploy(tvExplorer.SelectedNode, 
                out TreeNode instanceNode, out LiveInstance liveInstance))
            {
                // save and load the required data
                SaveAll();
                LoadConfigBase();
                LoadDeploymentSettings();

                // open an upload configuration form
                FrmUploadConfig frmUploadConfig = new FrmUploadConfig(appData, project, liveInstance.Instance);
                frmUploadConfig.ShowDialog();

                // take the changes into account
                if (frmUploadConfig.ProfileChanged)
                {
                    UpdateAgentClient(liveInstance);
                    SaveProjectSettings();
                }
                else if (frmUploadConfig.ConnSettingsModified)
                {
                    UpdateAgentClient(liveInstance);
                }
            }
        }

        private void miDeployInstanceStatus_Click(object sender, EventArgs e)
        {
            // display instance status
            if (FindInstanceForDeploy(tvExplorer.SelectedNode,
                out TreeNode instanceNode, out LiveInstance liveInstance))
            {
                // load deployment settings
                LoadDeploymentSettings();

                // open an instance status form
                FrmInstanceStatus frmInstanceStatus = 
                    new FrmInstanceStatus(appData, project.DeploymentSettings, liveInstance.Instance);
                frmInstanceStatus.ShowDialog();

                // take the changes into account
                if (frmInstanceStatus.ProfileChanged)
                {
                    UpdateAgentClient(liveInstance);
                    SaveProjectSettings();
                }
                else if (frmInstanceStatus.ConnSettingsModified)
                {
                    UpdateAgentClient(liveInstance);
                }
            }
        }

        private void miTools_DropDownOpening(object sender, EventArgs e)
        {
            miToolsCnlMap.Enabled = project != null;
        }

        private void miToolsCnlMap_Click(object sender, EventArgs e)
        {
            // show a channel map form
            new FrmCnlMap(project.ConfigBase, appData).ShowDialog();
        }

        private void miToolsOptions_Click(object sender, EventArgs e)
        {

        }

        private void miToolsCulture_Click(object sender, EventArgs e)
        {
            // show a form to select culture
            new FrmCulture(appData).ShowDialog();
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
            // show the about form
            FrmAbout.ShowAbout(appData);
        }


        private void miProjectRename_Click(object sender, EventArgs e)
        {
            // rename the selected project
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Project))
            {
                FrmItemName frmItemName = new FrmItemName() { ItemName = project.Name };

                if (frmItemName.ShowDialog() == DialogResult.OK && frmItemName.Modified)
                {
                    if (!project.Rename(frmItemName.ItemName, out string errMsg))
                        appData.ProcError(errMsg);

                    Text = string.Format(AppPhrases.ProjectTitle, project.Name);
                    selectedNode.Text = project.Name;
                    UpdateChildFormHints(selectedNode);
                    SaveProjectSettings();
                }
            }
        }

        private void miProjectProperties_Click(object sender, EventArgs e)
        {
            // edit properties of the selected project
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Project))
            {
                FrmProjectProps frmProjectProps = new FrmProjectProps
                {
                    ProjectName = project.Name,
                    Description = project.Description
                };

                if (frmProjectProps.ShowDialog() == DialogResult.OK && frmProjectProps.Modified)
                {
                    project.Description = frmProjectProps.Description;
                    SaveProjectSettings();
                }
            }
        }


        private void cmsDirectory_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu items
            TreeNode treeNode = tvExplorer.SelectedNode;
            bool isDirectoryNode = treeNode != null && treeNode.TagIs(AppNodeType.Directory);
            miDirectoryDelete.Enabled = isDirectoryNode;
            miDirectoryRename.Enabled = isDirectoryNode;
        }

        private void miDirectoryNewFile_Click(object sender, EventArgs e)
        {
            // create a new file in the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (TryGetFilePath(selectedNode, out string path))
            {
                FrmFileNew frmFileNew = new FrmFileNew();

                if (frmFileNew.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string fileName = Path.Combine(path, frmFileNew.FileName);

                        if (File.Exists(fileName))
                        {
                            ScadaUiUtils.ShowError(AppPhrases.FileAlreadyExists);
                        }
                        else
                        {
                            FileCreator.CreateFile(fileName, frmFileNew.FileType);
                            explorerBuilder.InsertFileNode(selectedNode, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        appData.ProcError(ex, AppPhrases.FileOperationError);
                    }
                }
            }
        }

        private void miDirectoryNewFolder_Click(object sender, EventArgs e)
        {
            // create a new subdirectory of the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (TryGetFilePath(selectedNode, out string path))
            {
                FrmItemName frmItemName = new FrmItemName();

                if (frmItemName.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string newDirectory = Path.Combine(path, frmItemName.ItemName);

                        if (Directory.Exists(newDirectory))
                        {
                            ScadaUiUtils.ShowError(AppPhrases.DirectoryAlreadyExists);
                        }
                        else
                        {
                            Directory.CreateDirectory(newDirectory);
                            explorerBuilder.InsertDirectoryNode(selectedNode, newDirectory);
                        }
                    }
                    catch (Exception ex)
                    {
                        appData.ProcError(ex, AppPhrases.FileOperationError);
                    }
                }
            }
        }

        private void miDirectoryDelete_Click(object sender, EventArgs e)
        {
            // delete the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Directory) &&
                TryGetFilePath(selectedNode, out string path) &&
                MessageBox.Show(AppPhrases.ConfirmDeleteDirectory, CommonPhrases.QuestionCaption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseChildForms(selectedNode);

                try
                {
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                    selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    appData.ProcError(ex, AppPhrases.FileOperationError);
                }
            }
        }

        private void miDirectoryRename_Click(object sender, EventArgs e)
        {
            // rename the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode?.Tag is TreeNodeTag tag && tag.NodeType == AppNodeType.Directory)
            {
                FileItem fileItem = (FileItem)tag.RelatedObject;

                if (Directory.Exists(fileItem.Path))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(fileItem.Path);
                    FrmItemName frmItemName = new FrmItemName() { ItemName = directoryInfo.Name };

                    if (frmItemName.ShowDialog() == DialogResult.OK && frmItemName.Modified)
                    {
                        try
                        {
                            string newDirectory = Path.Combine(directoryInfo.Parent.FullName, frmItemName.ItemName);
                            directoryInfo.MoveTo(newDirectory);
                            fileItem.Update(new DirectoryInfo(newDirectory));
                            selectedNode.Text = fileItem.Name;
                            UpdateTextEditorFileNames(selectedNode);
                        }
                        catch (Exception ex)
                        {
                            appData.ProcError(ex, AppPhrases.FileOperationError);
                        }
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.DirNotExists);
                }
            }
        }

        private void miDirectoryOpenInExplorer_Click(object sender, EventArgs e)
        {
            // open the selected directory in File Explorer
            if (TryGetFilePath(tvExplorer.SelectedNode, out string path))
            {
                if (Directory.Exists(path))
                    Process.Start(path);
                else
                    ScadaUiUtils.ShowError(CommonPhrases.DirNotExists);
            }
        }

        private void miDirectoryRefresh_Click(object sender, EventArgs e)
        {
            // refresh the tree nodes corresponding to the selected directory
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (TryGetFilePath(selectedNode, out string path))
            {
                CloseChildForms(selectedNode, true);
                explorerBuilder.FillFileNode(selectedNode, path);
            }
        }


        private void cmsFileItem_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the Open menu item
            if (tvExplorer.SelectedNode.Tag is TreeNodeTag tag && tag.NodeType == AppNodeType.File)
            {
                FileItem fileItem = (FileItem)tag.RelatedObject;
                miFileItemOpen.Enabled = FileCreator.ExtensionIsKnown(Path.GetExtension(fileItem.Path).TrimStart('.'));
            }
        }

        private void miFileItemOpen_Click(object sender, EventArgs e)
        {
            // open the selected file
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.File))
                ExecOpenFileAction(selectedNode);
        }

        private void miFileItemOpenLocation_Click(object sender, EventArgs e)
        {
            // open the selected file in File Explorer
            if (TryGetFilePath(tvExplorer.SelectedNode, out string path))
            {
                if (File.Exists(path))
                {
                    if (ScadaUtils.IsRunningOnWin)
                        Process.Start("explorer.exe", "/select, \"" + path + "\"");
                    else
                        Process.Start(Path.GetDirectoryName(path));
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.FileNotFound);
                }
            }
        }

        private void miFileItemDelete_Click(object sender, EventArgs e)
        {
            // delete the selected file
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.File) &&
                TryGetFilePath(selectedNode, out string path) &&
                MessageBox.Show(AppPhrases.ConfirmDeleteFile, CommonPhrases.QuestionCaption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseChildForms(selectedNode);

                try
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    selectedNode.Remove();
                }
                catch (Exception ex)
                {
                    appData.ProcError(ex, AppPhrases.FileOperationError);
                }
            }
        }

        private void miFileItemRename_Click(object sender, EventArgs e)
        {
            // rename the selected file
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode?.Tag is TreeNodeTag tag && tag.NodeType == AppNodeType.File)
            {
                FileItem fileItem = (FileItem)tag.RelatedObject;

                if (File.Exists(fileItem.Path))
                {
                    FileInfo fileInfo = new FileInfo(fileItem.Path);
                    FrmItemName frmItemName = new FrmItemName() { ItemName = fileInfo.Name };

                    if (frmItemName.ShowDialog() == DialogResult.OK && frmItemName.Modified)
                    {
                        try
                        {
                            string newFileName = Path.Combine(fileInfo.DirectoryName, frmItemName.ItemName);
                            fileInfo.MoveTo(newFileName);
                            fileItem.Update(new FileInfo(newFileName));
                            selectedNode.Text = fileItem.Name;

                            if (tag.ExistingForm is FrmTextEditor form)
                            {
                                wctrlMain.UpdateHint(tag.ExistingForm, selectedNode.FullPath);
                                form.ChildFormTag.SendMessage(this, AppMessage.UpdateFileName,
                                    new Dictionary<string, object> { { "FileName", newFileName } });
                            }
                        }
                        catch (Exception ex)
                        {
                            appData.ProcError(ex, AppPhrases.FileOperationError);
                        }
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.FileNotFound);
                }
            }
        }


        private void cmsInstance_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu items
            TreeNode treeNode = tvExplorer.SelectedNode;
            bool isInstanceNode = treeNode != null && treeNode.TagIs(AppNodeType.Instance);

            miInstanceMoveUp.Enabled = isInstanceNode && treeNode.PrevNode != null;
            miInstanceMoveDown.Enabled = isInstanceNode && treeNode.NextNode != null;
            miInstanceDelete.Enabled = isInstanceNode;

            bool deployInstanceFound = isInstanceNode || project != null && project.Instances.Count == 1;
            miInstanceProfile.Enabled = deployInstanceFound;
            miInstanceDownloadConfig.Enabled = deployInstanceFound;
            miInstanceUploadConfig.Enabled = deployInstanceFound;
            miInstanceStatus.Enabled = deployInstanceFound;

            miInstanceOpenInExplorer.Enabled = isInstanceNode;
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
                    if (project.ContainsInstance(frmInstanceEdit.InstanceName))
                    {
                        ScadaUiUtils.ShowError(AppPhrases.InstanceAlreadyExists);
                    }
                    else
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
            // rename the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                Instance instance = liveInstance.Instance;
                FrmItemName frmItemName = new FrmItemName()
                {
                    ItemName = instance.Name,
                    ExistingNames = project.GetInstanceNames(true, instance.Name)
                };

                if (frmItemName.ShowDialog() == DialogResult.OK && frmItemName.Modified)
                {
                    if (!instance.Rename(frmItemName.ItemName, out string errMsg))
                        appData.ProcError(errMsg);

                    selectedNode.Text = instance.Name;
                    UpdateChildFormHints(selectedNode);
                    SaveProjectSettings();
                }
            }
        }

        private void miInstanceProperties_Click(object sender, EventArgs e)
        {
            // edit properties of the selected instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                FrmInstanceEdit frmInstanceEdit = new FrmInstanceEdit() { Mode = FormOperatingMode.Edit };
                Instance instance = liveInstance.Instance;
                frmInstanceEdit.Init(instance);

                if (frmInstanceEdit.ShowDialog() == DialogResult.OK)
                {
                    // save the forms corresponding to the instance
                    SaveChildForms(selectedNode);

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
                                scadaApp.ClearSettings();
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
                        RefreshInstanceNode(selectedNode, liveInstance);
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

            miCommLineStart.Enabled = isLineNode;
            miCommLineStop.Enabled = isLineNode;
            miCommLineRestart.Enabled = isLineNode;
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
                SaveCommSettigns(liveInstance);
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
                SaveCommSettigns(liveInstance);
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
                SaveCommSettigns(liveInstance);
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
                SaveCommSettigns(liveInstance);
            }
        }


        private void cmsDevice_Opening(object sender, CancelEventArgs e)
        {
            if (FindClosestInstance(tvExplorer.SelectedNode, out LiveInstance liveInstance))
            {
                IAgentClient agentClient = liveInstance.CommEnvironment.AgentClient;
                miDeviceCommand.Enabled = agentClient != null && agentClient.IsLocal;
            }
            else
            {
                miDeviceCommand.Enabled = false;
                miDeviceProperties.Enabled = false;
            }
        }

        private void miDeviceCommand_Click(object sender, EventArgs e)
        {
            // show a device command form
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.Device) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                Comm.Settings.KP kp = (Comm.Settings.KP)((TreeNodeTag)selectedNode.Tag).RelatedObject;
                new FrmDeviceCommand(kp, liveInstance.CommEnvironment).ShowDialog();
            }
        }

        private void miDeviceProperties_Click(object sender, EventArgs e)
        {
            // show the device properties
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.Device) &&
                selectedNode.FindClosest(CommNodeType.CommLine) is TreeNode commLineNode &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                Comm.Settings.CommLine commLine = (Comm.Settings.CommLine)((TreeNodeTag)commLineNode.Tag).RelatedObject;
                Comm.Settings.KP kp = (Comm.Settings.KP)((TreeNodeTag)selectedNode.Tag).RelatedObject;

                string dllPath = Path.Combine(liveInstance.CommEnvironment.AppDirs.KPDir, kp.Dll);
                KPView kpView = liveInstance.CommEnvironment.GetKPView(dllPath, kp.Number,
                    new KPView.KPProperties(commLine.CustomParams, kp.CmdLine));

                if (kpView.CanShowProps)
                {
                    kpView.ShowProps();

                    if (kpView.KPProps.Modified)
                    {
                        kp.CmdLine = kpView.KPProps.CmdLine;
                        UpdateLineParams(selectedNode);
                        SaveCommSettigns(liveInstance);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowWarning(CommShellPhrases.NoDeviceProps);
                }
            }
        }
    }
}
