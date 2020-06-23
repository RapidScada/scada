/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Modified : 2020
 */

using Scada.Admin.App.Code;
using Scada.Admin.App.Forms.Deployment;
using Scada.Admin.App.Forms.Tables;
using Scada.Admin.App.Forms.Tools;
using Scada.Admin.App.Properties;
using Scada.Admin.Config;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent.Connector;
using Scada.Comm;
using Scada.Comm.Devices;
using Scada.Comm.Shell.Code;
using Scada.Comm.Shell.Forms;
using Scada.Data.Entities;
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
        private const string DocEnUrl = "https://rapidscada.net/doc/content/latest/en/";
        /// <summary>
        /// The hyperlink to the documentation in Russian.
        /// </summary>
        private const string DocRuUrl = "https://rapidscada.net/doc/content/latest/ru/";
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
        private FrmStartPage frmStartPage;                // the start page
        private bool preventNodeExpand;                   // prevent a tree node from expanding or collapsing


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
                ProjectMenu = cmsProject, CnlTableMenu = cmsCnlTable, DirectoryMenu = cmsDirectory,
                FileItemMenu = cmsFileItem, InstanceMenu = cmsInstance, ServerMenu = cmsServer,
                CommMenu = cmsComm, CommLineMenu = cmsCommLine, DeviceMenu = cmsDevice });
            project = null;
            frmStartPage = null;
            preventNodeExpand = false;
        }


        /// <summary>
        /// Gets or sets the explorer width.
        /// </summary>
        public int ExplorerWidth
        {
            get
            {
                return pnlLeft.Width;
            }
            set
            {
                pnlLeft.Width = Math.Max(value, splVert.MinSize);
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
                Translator.TranslateForm(this, GetType().FullName, null,
                    cmsProject, cmsCnlTable, cmsDirectory, cmsFileItem, cmsInstance, 
                    cmsServer, cmsComm, cmsCommLine, cmsDevice);
                Text = AppPhrases.EmptyTitle;
                wctrlMain.MessageText = AppPhrases.WelcomeMessage;
                ofdProject.SetFilter(AppPhrases.ProjectFileFilter);
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
        /// Enables or disables menu items and tool buttons.
        /// </summary>
        private void SetMenuItemsEnabled()
        {
            bool projectIsOpen = project != null;

            // file
            miFileSave.Enabled = btnFileSave.Enabled = false;
            miFileSaveAll.Enabled = btnFileSaveAll.Enabled = false;
            miFileImportTable.Enabled = projectIsOpen;
            miFileExportTable.Enabled = projectIsOpen;
            miFileCloseProject.Enabled = projectIsOpen;

            // deploy
            SetDeployMenuItemsEnabled();

            // tools
            miToolsAddLine.Enabled = btnToolsAddLine.Enabled = projectIsOpen;
            miToolsAddDevice.Enabled = btnToolsAddDevice.Enabled = projectIsOpen;
            miToolsCreateCnls.Enabled = btnToolsCreateCnls.Enabled = projectIsOpen;
            miToolsCloneCnls.Enabled = projectIsOpen;
            miToolsCnlMap.Enabled = projectIsOpen;
            miToolsCheckIntegrity.Enabled = projectIsOpen;
        }

        /// <summary>
        /// Loads the application settings.
        /// </summary>
        private void LoadAppSettings()
        {
            if (!appData.AppSettings.Load(Path.Combine(appData.AppDirs.ConfigDir, AdminSettings.DefFileName),
                out string errMsg))
            {
                appData.ProcError(errMsg);
            }
        }

        /// <summary>
        /// Loads the application state.
        /// </summary>
        private void LoadAppState()
        {
            if (appData.AppState.Load(Path.Combine(appData.AppDirs.ConfigDir, AppState.DefFileName), 
                out string errMsg))
            {
                appData.AppState.MainFormState.Apply(this);
                ofdProject.InitialDirectory = appData.AppState.ProjectDir;
            }
            else
            {
                appData.ProcError(errMsg);
            }
        }

        /// <summary>
        /// Saves the application state.
        /// </summary>
        private void SaveAppState()
        {
            appData.AppState.MainFormState.Retrieve(this);

            if (!appData.AppState.Save(Path.Combine(appData.AppDirs.ConfigDir, AppState.DefFileName),
                out string errMsg))
            {
                appData.ProcError(errMsg);
            }
        }

        /// <summary>
        /// Enables or disables the deployment menu items and tool buttons.
        /// </summary>
        private void SetDeployMenuItemsEnabled()
        {
            bool instanceExists = project != null && project.Instances.Count > 0;
            miDeployInstanceProfile.Enabled = btnDeployInstanceProfile.Enabled = instanceExists;
            miDeployDownloadConfig.Enabled = btnDeployDownloadConfig.Enabled = instanceExists;
            miDeployUploadConfig.Enabled = btnDeployUploadConfig.Enabled = instanceExists;
            miDeployInstanceStatus.Enabled = btnDeployInstanceStatus.Enabled = instanceExists;
        }

        /// <summary>
        /// Disables the Save All menu item and tool button if all data is saved.
        /// </summary>
        private void DisableSaveAll()
        {
            if (miFileSaveAll.Enabled && !project.ConfigBase.Modified)
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
                    string ext = Path.GetExtension(fileItem.Name).TrimStart('.').ToLowerInvariant();

                    if (appData.AppSettings.FileAssociations.TryGetValue(ext, out string exePath) && 
                        File.Exists(exePath))
                    {
                        // run external editor
                        Process.Start(exePath, string.Format("\"{0}\"", fileItem.Path));
                    }
                    else
                    {
                        // create and display a new text editor form
                        FrmTextEditor form = new FrmTextEditor(appData, fileItem.Path);
                        tag.ExistingForm = form;
                        wctrlMain.AddForm(form, treeNode.FullPath, ilExplorer.Images[treeNode.ImageKey], treeNode);
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
        /// Finds a tree node that contains the related object.
        /// </summary>
        private TreeNode FindTreeNode(object relatedObject, TreeNode startNode)
        {
            foreach (TreeNode node in TreeViewUtils.IterateNodes(startNode))
            {
                if (node.Tag is TreeNodeTag tag && tag.RelatedObject == relatedObject)
                    return node;
            }

            return null;
        }

        /// <summary>
        /// Gets the related object of the tree node.
        /// </summary>
        private T GetRelatedObject<T>(TreeNode treeNode, bool throwOnError = true) where T : class
        {
            return throwOnError ?
                (T)((TreeNodeTag)treeNode.Tag).RelatedObject :
                ((TreeNodeTag)treeNode.Tag).RelatedObject as T;
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
                explorerBuilder.SetContextMenus(commLineNode);
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
        /// Refreshes data of the table forms having the specified item type.
        /// </summary>
        private void RefreshBaseTables(Type itemType)
        {
            foreach (Form form in wctrlMain.Forms)
            {
                if (form is FrmBaseTable frmBaseTable && frmBaseTable.ItemType == itemType)
                    frmBaseTable.ChildFormTag.SendMessage(this, AppMessage.RefreshData);
            }
        }

        /// <summary>
        /// Prepares to close all forms.
        /// </summary>
        private void PrepareToCloseAll()
        {
            foreach (Form form in wctrlMain.Forms)
            {
                if (form is FrmBaseTable frmBaseTable)
                    frmBaseTable.PrepareToClose();
            }
        }

        /// <summary>
        /// Closes the child forms corresponding to the specified node and its children.
        /// </summary>
        private void CloseChildForms(TreeNode treeNode, bool save = false, bool skipRoot = false)
        {
            foreach (TreeNode node in 
                skipRoot ? TreeViewUtils.IterateNodes(treeNode.Nodes) : TreeViewUtils.IterateNodes(treeNode))
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
            else
            {
                liveInstance = GetRelatedObject<LiveInstance>(instanceNode);
                return true;
            }
        }

        /// <summary>
        /// Finds an instance selected for deploy.
        /// </summary>
        private bool FindInstanceForDeploy(TreeNode treeNode, out TreeNode instanceNode, out LiveInstance liveInstance)
        {
            if (project != null)
            {
                instanceNode = treeNode?.FindClosest(AppNodeType.Instance);

                if (instanceNode == null && project.Instances.Count > 0)
                    instanceNode = explorerBuilder.InstancesNode.FindFirst(AppNodeType.Instance);

                if (instanceNode != null)
                {
                    liveInstance = GetRelatedObject<LiveInstance>(instanceNode);
                    return true;
                }
            }

            instanceNode = null;
            liveInstance = null;
            return false;
        }

        /// <summary>
        /// Finds the instance and its node by name.
        /// </summary>
        private bool FindInstance(string instanceName, out TreeNode instanceNode, out LiveInstance liveInstance)
        {
            if (project != null)
            {
                foreach (TreeNode treeNode in explorerBuilder.InstancesNode.Nodes)
                {
                    if (treeNode.Tag is TreeNodeTag tag && tag.RelatedObject is LiveInstance liveInst &&
                        string.Equals(liveInst.Instance.Name, instanceName, StringComparison.OrdinalIgnoreCase))
                    {
                        instanceNode = treeNode;
                        liveInstance = liveInst;
                        return true;
                    }
                }
            }

            instanceNode = null;
            liveInstance = null;
            return false;
        }

        /// <summary>
        /// Prepares data and fills the instance node.
        /// </summary>
        private void PrepareInstanceNode(TreeNode instanceNode, LiveInstance liveInstance)
        {
            if (!liveInstance.IsReady)
            {
                if (liveInstance.Instance.LoadAppSettings(out string errMsg))
                {
                    LoadDeploymentSettings();
                    InitAgentClient(liveInstance);
                    liveInstance.ServerEnvironment = CreateServerEnvironment(liveInstance);
                    liveInstance.CommEnvironment = CreateCommEnvironment(liveInstance);
                    explorerBuilder.FillInstanceNode(instanceNode);
                    liveInstance.IsReady = true;
                }
                else
                {
                    appData.ProcError(errMsg);
                }
            }
        }

        /// <summary>
        /// Prepares data and fills the instance node.
        /// </summary>
        private void PrepareInstanceNode(TreeNode instanceNode)
        {
            if (instanceNode.Tag is TreeNodeTag tag && tag.RelatedObject is LiveInstance liveInstance)
                PrepareInstanceNode(instanceNode, liveInstance);
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
        /// Recreates Server and Communicator environments of the instance, if the instance is ready.
        /// </summary>
        private void RefreshEnvironments(LiveInstance liveInstance)
        {
            if (liveInstance.IsReady)
            {
                liveInstance.ServerEnvironment = CreateServerEnvironment(liveInstance);
                liveInstance.CommEnvironment = CreateCommEnvironment(liveInstance);
            }
        }

        /// <summary>
        /// Creates a new Server environment for the specified instance.
        /// </summary>
        private ServerEnvironment CreateServerEnvironment(LiveInstance liveInstance)
        {
            return new ServerEnvironment(
                new ServerDirs(appData.AppSettings.PathOptions.ServerDir, liveInstance.Instance), log)
            {
                AgentClient = liveInstance.AgentClient
            };
        }

        /// <summary>
        /// Creates a new Communicator environment for the specified instance.
        /// </summary>
        private CommEnvironment CreateCommEnvironment(LiveInstance liveInstance)
        {
            return new CommEnvironment(
                new CommDirs(appData.AppSettings.PathOptions.CommDir, liveInstance.Instance), log)
            {
                AgentClient = liveInstance.AgentClient
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
        /// Initializes an Agent client of the specified instance.
        /// </summary>
        private void InitAgentClient(LiveInstance liveInstance)
        {
            DeploymentProfile profile = GetDeploymentProfile(liveInstance.Instance.DeploymentProfile);

            if (profile == null)
            {
                liveInstance.AgentClient = null;
            }
            else
            {
                ConnectionSettings connSettings = profile.ConnectionSettings.Clone();
                connSettings.ScadaInstance = liveInstance.Instance.Name;
                liveInstance.AgentClient = new AgentWcfClient(connSettings);
            }
        }

        /// <summary>
        /// Updates the Agent client of the specified instance.
        /// </summary>
        private void UpdateAgentClient(LiveInstance liveInstance)
        {
            if (liveInstance.ServerEnvironment != null || liveInstance.CommEnvironment != null)
            {
                InitAgentClient(liveInstance);

                if (liveInstance.ServerEnvironment != null)
                    liveInstance.ServerEnvironment.AgentClient = liveInstance.AgentClient;

                if (liveInstance.CommEnvironment != null)
                    liveInstance.CommEnvironment.AgentClient = liveInstance.AgentClient;
            }
        }

        /// <summary>
        /// Loads the deployments settings of the current project.
        /// </summary>
        private void LoadDeploymentSettings()
        {
            if (!project.DeploymentSettings.Loaded && File.Exists(project.DeploymentSettings.FileName) &&
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
        private bool SaveConfigBase()
        {
            if (project.ConfigBase.Save(out string errMsg))
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

        /// <summary>
        /// Creates a new project.
        /// </summary>
        private void NewProject()
        {
            FrmProjectNew frmNewProject = new FrmProjectNew(appData);

            if (frmNewProject.ShowDialog() == DialogResult.OK && CloseProject())
            {
                CloseStartPage();

                if (ScadaProject.Create(frmNewProject.ProjectName, frmNewProject.ProjectLocation,
                    frmNewProject.ProjectTemplate, out ScadaProject newProject, out string errMsg))
                {
                    appData.AppState.AddRecentProject(newProject.FileName);
                    appData.AppState.RecentSelection.Reset();
                    project = newProject;
                    LoadConfigBase();
                    Text = string.Format(AppPhrases.ProjectTitle, project.Name);
                    wctrlMain.MessageText = AppPhrases.SelectItemMessage;
                    SetMenuItemsEnabled();
                    explorerBuilder.CreateNodes(project);
                }
                else
                {
                    appData.ProcError(errMsg);
                }
            }
        }

        /// <summary>
        /// Opens the project.
        /// </summary>
        private void OpenProject(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                ofdProject.FileName = "";

                if (ofdProject.ShowDialog() == DialogResult.OK)
                    fileName = ofdProject.FileName;
            }

            if (!string.IsNullOrEmpty(fileName) && CloseProject())
            {
                CloseStartPage();
                ofdProject.InitialDirectory = Path.GetDirectoryName(fileName);
                project = new ScadaProject();

                if (project.Load(fileName, out string errMsg))
                    appData.AppState.AddRecentProject(project.FileName);
                else
                    appData.ProcError(errMsg);

                LoadConfigBase();
                Text = string.Format(AppPhrases.ProjectTitle, project.Name);
                wctrlMain.MessageText = AppPhrases.SelectItemMessage;
                SetMenuItemsEnabled();
                explorerBuilder.CreateNodes(project);
            }
        }

        /// <summary>
        /// Closes the project.
        /// </summary>
        private bool CloseProject()
        {
            if (project == null)
            {
                return true;
            }
            else
            {
                PrepareToCloseAll();
                wctrlMain.CloseAllForms(out bool cancel);

                if (!cancel && project.ConfigBase.Modified)
                {
                    switch (MessageBox.Show(AppPhrases.SaveConfigBaseConfirm,
                        CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            cancel = !SaveConfigBase();
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            cancel = true;
                            break;
                    }
                }

                if (cancel)
                {
                    return false;
                }
                else
                {
                    project = null;
                    Text = AppPhrases.EmptyTitle;
                    wctrlMain.MessageText = AppPhrases.WelcomeMessage;
                    SetMenuItemsEnabled();
                    tvExplorer.Nodes.Clear();
                    ShowStatus(null);
                    return true;
                }
            }
        }

        /// <summary>
        /// Shows the start page.
        /// </summary>
        private void ShowStartPage()
        {
            if (frmStartPage == null)
            {
                frmStartPage = new FrmStartPage(appData.AppState);
                wctrlMain.AddForm(frmStartPage, "", miFileShowStartPage.Image, null);
            }
            else
            {
                wctrlMain.ActivateForm(frmStartPage);
            }
        }

        /// <summary>
        /// Closes the start page.
        /// </summary>
        private void CloseStartPage()
        {
            frmStartPage?.Close();
        }

        /// <summary>
        /// Shows information in the status bar.
        /// </summary>
        private void ShowStatus(Instance instance)
        {
            if (instance == null)
            {
                lblSelectedInstance.Text = "";
                lblSelectedProfile.Text = "";
                lblSelectedProfile.Visible = false;
            }
            else
            {
                lblSelectedInstance.Text = instance.Name;
                lblSelectedProfile.Text = instance.DeploymentProfile;
                lblSelectedProfile.Visible = !string.IsNullOrEmpty(instance.DeploymentProfile);
            }
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();
            TakeExplorerImages();
            SetMenuItemsEnabled();
            LoadAppSettings();
            LoadAppState();
            ShowStartPage();
            ShowStatus(null);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // confirm saving the project before closing
            e.Cancel = !CloseProject();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveAppState();
            appData.FinalizeApp();
        }


        private void tvExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            // execute a node action on press Enter
            if (e.KeyCode == Keys.Enter && tvExplorer.SelectedNode != null)
            {
                TreeNode selectedNode = tvExplorer.SelectedNode;
                if (selectedNode.TagIs(AppNodeType.File))
                {
                    ExecOpenFileAction(selectedNode);
                }
                else if (selectedNode.Tag is TreeNodeTag tag && tag.FormType != null)
                {
                    ExecNodeAction(selectedNode);
                }
                else if (selectedNode.Nodes.Count > 0)
                {
                    if (selectedNode.IsExpanded)
                        selectedNode.Collapse(true);
                    else
                        selectedNode.Expand();
                }
            }
        }

        private void tvExplorer_MouseDown(object sender, MouseEventArgs e)
        {
            // check whether to prevent a node from expanding
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                TreeNode node = tvExplorer.GetNodeAt(e.Location);
                preventNodeExpand = node != null && node.Nodes.Count > 0 && 
                    node.Tag is TreeNodeTag tag && tag.FormType != null;
            }
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
            // prevent the node from expanding
            if (preventNodeExpand)
            {
                e.Cancel = true;
                preventNodeExpand = false;
                return;
            }

            // fill a node on demand
            TreeNode node = e.Node;

            if (node.TagIs(AppNodeType.Interface))
                explorerBuilder.FillInterfaceNode(node);
            else if (node.TagIs(AppNodeType.Instance))
                PrepareInstanceNode(node);
            else if (node.TagIs(AppNodeType.WebApp))
                explorerBuilder.FillWebstationNode(node);
        }

        private void tvExplorer_AfterExpand(object sender, TreeViewEventArgs e)
        {
            explorerBuilder.SetFolderImage(e.Node);
        }

        private void tvExplorer_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // prevent the node from collapsing
            if (preventNodeExpand)
            {
                e.Cancel = true;
                preventNodeExpand = false;
            }
        }

        private void tvExplorer_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            explorerBuilder.SetFolderImage(e.Node);
        }

        private void tvExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // show information about the selected instance
            if (FindInstanceForDeploy(tvExplorer.SelectedNode,
                out TreeNode instanceNode, out LiveInstance liveInstance))
            {
                ShowStatus(liveInstance.Instance);
            }
            else
            {
                ShowStatus(null);
            }
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

            // clear the pointer to the start page
            if (e.ChildForm is FrmStartPage)
                frmStartPage = null;

            // disable the Save All menu item if needed
            if (e.ChildForm is IChildForm childForm && childForm.ChildFormTag.Modified)
                DisableSaveAll();
        }

        private void wctrlMain_ChildFormMessage(object sender, FormMessageEventArgs e)
        {
            TreeNode sourceNode = FindTreeNode(e.Source);

            if (e.Message == AppMessage.NewProject)
            {
                NewProject();
            }
            else if (e.Message == AppMessage.OpenProject)
            {
                OpenProject(e.GetArgument("Path") as string);
            }
            else if (FindClosestInstance(sourceNode, out LiveInstance liveInstance))
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
                    if (SaveCommSettigns(liveInstance))
                    {
                        TreeNode commLineNode = sourceNode.FindClosest(CommNodeType.CommLine);
                        if (commLineNode != null)
                            UpdateCommLine(commLineNode, liveInstance.CommEnvironment);
                    }
                    else
                    {
                        e.Cancel = true;
                    }
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
            miFileClose.Enabled = wctrlMain.ActiveForm != null;
        }

        private void miFileNewProject_Click(object sender, EventArgs e)
        {
            NewProject();
        }

        private void miFileOpenProject_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void miFileShowStartPage_Click(object sender, EventArgs e)
        {
            ShowStartPage();
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

        private void miFileImportTable_Click(object sender, EventArgs e)
        {
            // import a configuration database table
            if (project != null)
            {
                FrmTableImport frmTableImport = new FrmTableImport(project.ConfigBase, appData)
                {
                    SelectedItemType = (wctrlMain.ActiveForm as FrmBaseTable)?.ItemType
                };

                if (frmTableImport.ShowDialog() == DialogResult.OK)
                    RefreshBaseTables(frmTableImport.SelectedItemType);
            }
        }

        private void miFileExportTable_Click(object sender, EventArgs e)
        {
            // export a configuration database table
            if (project != null)
            {
                new FrmTableExport(project.ConfigBase, appData)
                {
                    SelectedItemType = (wctrlMain.ActiveForm as FrmBaseTable)?.ItemType
                }
                .ShowDialog();
            }
        }

        private void miFileClose_Click(object sender, EventArgs e)
        {
            miWindowCloseActive_Click(null, null);
        }

        private void miFileCloseProject_Click(object sender, EventArgs e)
        {
            CloseProject();
            ShowStartPage();
        }

        private void miFileExit_Click(object sender, EventArgs e)
        {
            Close();
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
                    ShowStatus(liveInstance.Instance);
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
                // save all changes and load the deployment settings
                SaveAll();
                LoadDeploymentSettings();

                // open a download configuration form
                FrmDownloadConfig frmDownloadConfig = new FrmDownloadConfig(appData, project, liveInstance.Instance);
                frmDownloadConfig.ShowDialog();

                // take the changes into account
                if (frmDownloadConfig.ProfileChanged)
                {
                    UpdateAgentClient(liveInstance);
                    SaveProjectSettings();
                    ShowStatus(liveInstance.Instance);
                }
                else if (frmDownloadConfig.ConnSettingsModified)
                {
                    UpdateAgentClient(liveInstance);
                }

                if (frmDownloadConfig.BaseModified)
                {
                    CloseChildForms(explorerBuilder.BaseNode);
                    SaveConfigBase();
                }

                if (frmDownloadConfig.InterfaceModified && 
                    TryGetFilePath(explorerBuilder.InterfaceNode, out string path))
                {
                    CloseChildForms(explorerBuilder.InterfaceNode);
                    explorerBuilder.FillFileNode(explorerBuilder.InterfaceNode, path);
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
                // save all changes and load the deployment settings
                SaveAll();
                LoadDeploymentSettings();

                // open an upload configuration form
                FrmUploadConfig frmUploadConfig = new FrmUploadConfig(appData, project, liveInstance.Instance);
                frmUploadConfig.ShowDialog();

                // take the changes into account
                if (frmUploadConfig.ProfileChanged)
                {
                    UpdateAgentClient(liveInstance);
                    SaveProjectSettings();
                    ShowStatus(liveInstance.Instance);
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
                    ShowStatus(liveInstance.Instance);
                }
                else if (frmInstanceStatus.ConnSettingsModified)
                {
                    UpdateAgentClient(liveInstance);
                }
            }
        }

        private void miToolsAddLine_Click(object sender, EventArgs e)
        {
            // show a line add form
            if (project != null)
            {
                FrmLineAdd frmLineAdd = new FrmLineAdd(project, appData.AppState.RecentSelection);

                if (frmLineAdd.ShowDialog() == DialogResult.OK)
                {
                    RefreshBaseTables(typeof(CommLine));

                    // add the communication line to the explorer
                    if (frmLineAdd.CommLineSettings != null && 
                        FindInstance(frmLineAdd.InstanceName, out TreeNode instanceNode, out LiveInstance liveInstance))
                    {
                        if (liveInstance.IsReady)
                        {
                            TreeNode commLinesNode = instanceNode.FindFirst(CommNodeType.CommLines);
                            TreeNode commLineNode = commShell.CreateCommLineNode(frmLineAdd.CommLineSettings,
                                liveInstance.CommEnvironment);
                            commLineNode.ContextMenuStrip = cmsCommLine;
                            commLinesNode.Nodes.Add(commLineNode);
                            tvExplorer.SelectedNode = commLineNode;
                        }
                        else
                        {
                            PrepareInstanceNode(instanceNode, liveInstance);
                            tvExplorer.SelectedNode = FindTreeNode(frmLineAdd.CommLineSettings, instanceNode);
                        }

                        SaveCommSettigns(liveInstance);
                    }
                }
            }
        }

        private void miToolsAddDevice_Click(object sender, EventArgs e)
        {
            // show a device add form
            if (project != null)
            {
                FrmDeviceAdd frmDeviceAdd = new FrmDeviceAdd(project, appData.AppState.RecentSelection);

                if (frmDeviceAdd.ShowDialog() == DialogResult.OK)
                {
                    RefreshBaseTables(typeof(KP));

                    if (frmDeviceAdd.KPSettings != null &&
                        FindInstance(frmDeviceAdd.InstanceName, out TreeNode instanceNode, out LiveInstance liveInstance))
                    {
                        // add the device to the explorer
                        if (liveInstance.IsReady)
                        {
                            TreeNode commLineNode = FindTreeNode(frmDeviceAdd.CommLineSettings, instanceNode);
                            TreeNode kpNode = commShell.CreateDeviceNode(frmDeviceAdd.KPSettings, 
                                frmDeviceAdd.CommLineSettings, liveInstance.CommEnvironment);
                            kpNode.ContextMenuStrip = cmsDevice;
                            commLineNode.Nodes.Add(kpNode);
                            tvExplorer.SelectedNode = kpNode;
                            UpdateLineParams(kpNode);
                        }
                        else
                        {
                            PrepareInstanceNode(instanceNode, liveInstance);
                            tvExplorer.SelectedNode = FindTreeNode(frmDeviceAdd.KPSettings, instanceNode);
                        }

                        // set the device request parameters by default
                        if (liveInstance.CommEnvironment.TryGetKPView(frmDeviceAdd.KPSettings, true, null,
                            out KPView kpView, out string errMsg))
                        {
                            frmDeviceAdd.KPSettings.SetReqParams(kpView.DefaultReqParams);
                        }
                        else
                        {
                            ScadaUiUtils.ShowError(errMsg);
                        }

                        SaveCommSettigns(liveInstance);
                    }
                }
            }
        }

        private void miToolsCreateCnls_Click(object sender, EventArgs e)
        {
            // show a channel creation wizard
            if (project != null)
            {
                FrmCnlCreate frmCnlCreate = new FrmCnlCreate(project, appData.AppState.RecentSelection, appData);

                if (frmCnlCreate.ShowDialog() == DialogResult.OK)
                {
                    RefreshBaseTables(typeof(InCnl));
                    RefreshBaseTables(typeof(CtrlCnl));
                }
            }
        }

        private void miToolsCloneCnls_Click(object sender, EventArgs e)
        {
            // show a cloning channels form
            if (project != null)
            {
                FrmCnlClone frmCnlClone = new FrmCnlClone(project.ConfigBase, appData)
                {
                    InCnlsSelected = !(wctrlMain.ActiveForm is FrmBaseTable frmBaseTable &&
                        frmBaseTable.ItemType == typeof(CtrlCnl))
                };

                frmCnlClone.ShowDialog();

                if (frmCnlClone.InCnlsCloned)
                    RefreshBaseTables(typeof(InCnl));

                if (frmCnlClone.OutCnlsCloned)
                    RefreshBaseTables(typeof(CtrlCnl));
            }
        }

        private void miToolsCnlMap_Click(object sender, EventArgs e)
        {
            // show a channel map form
            if (project != null)
            {
                Type itemType = (wctrlMain.ActiveForm as FrmBaseTable)?.ItemType;

                new FrmCnlMap(project.ConfigBase, appData)
                {
                    IncludeInCnls = itemType != typeof(CtrlCnl),
                    IncludeOutCnls = itemType != typeof(InCnl)
                }
                .ShowDialog();
            }
        }

        private void miToolsCheckIntegrity_Click(object sender, EventArgs e)
        {
            // check integrity
            if (project != null)
                new IntegrityCheck(project.ConfigBase, appData).Execute();
        }

        private void miToolsOptions_Click(object sender, EventArgs e)
        {
            // edit the application settings
            FrmSettings frmSettings = new FrmSettings(appData);

            if (frmSettings.ShowDialog() == DialogResult.OK && frmSettings.ReopenNeeded)
                ScadaUiUtils.ShowInfo(AppPhrases.ReopenProject);
        }

        private void miToolsCulture_Click(object sender, EventArgs e)
        {
            // show a form to select culture
            new FrmCulture(appData).ShowDialog();
        }

        private void miWindow_DropDownOpening(object sender, EventArgs e)
        {
            int formCount = wctrlMain.FormCount;
            miWindowCloseActive.Enabled = formCount > 0;
            miWindowCloseAll.Enabled = formCount > 0;
            miWindowCloseAllButActive.Enabled = formCount > 1;
        }

        private void miWindowCloseActive_Click(object sender, EventArgs e)
        {
            if (wctrlMain.ActiveForm is FrmBaseTable frmBaseTable)
                frmBaseTable.PrepareToClose();

            wctrlMain.CloseActiveForm(out bool cancel);
        }

        private void miWindowCloseAll_Click(object sender, EventArgs e)
        {
            PrepareToCloseAll();
            wctrlMain.CloseAllForms(out bool cancel);
        }

        private void miWindowCloseAllButActive_Click(object sender, EventArgs e)
        {
            wctrlMain.CloseAllButActive(out bool cancel);
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
                    CloseChildForms(selectedNode);
                    explorerBuilder.FillInstancesNode();
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
                    Version = project.Version,
                    Description = project.Description
                };

                if (frmProjectProps.ShowDialog() == DialogResult.OK && frmProjectProps.Modified)
                {
                    project.Version = frmProjectProps.Version;
                    project.Description = frmProjectProps.Description;
                    SaveProjectSettings();
                }
            }
        }


        private void cmsCnlTable_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu item
            TreeNode selectedNode = tvExplorer.SelectedNode;
            miCnlTableComm.Enabled = selectedNode != null && selectedNode.Tag is TreeNodeTag tag && 
                tag.RelatedObject is BaseTableItem baseTableItem && baseTableItem.KPNum > 0;
        }

        private void miCnlTableComm_Click(object sender, EventArgs e)
        {
            // find a device tree node of Communicator
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.Tag is TreeNodeTag tag1 &&
                tag1.RelatedObject is BaseTableItem baseTableItem && baseTableItem.KPNum > 0)
            {
                int kpNum = baseTableItem.KPNum;
                bool nodeFound = false;

                foreach (TreeNode treeNode in TreeViewUtils.IterateNodes(explorerBuilder.InstancesNode.Nodes))
                {
                    if (treeNode.Tag is TreeNodeTag tag2)
                    {
                        if (tag2.NodeType == AppNodeType.Instance)
                        {
                            PrepareInstanceNode(treeNode);
                        }
                        else if (tag2.RelatedObject is Comm.Settings.KP kp && kp.Number == kpNum)
                        {
                            tvExplorer.SelectedNode = treeNode;
                            nodeFound = true;
                            break;
                        }
                    }
                }

                if (!nodeFound)
                    ScadaUiUtils.ShowWarning(AppPhrases.DeviceNotFoundInComm);
            }
        }

        private void miCnlTableRefresh_Click(object sender, EventArgs e)
        {
            // refresh channel table subnodes
            if (project != null)
            {
                TreeNode inCnlTableNode = explorerBuilder.BaseTableNodes[project.ConfigBase.InCnlTable.Name];
                TreeNode ctrlCnlTableNode = explorerBuilder.BaseTableNodes[project.ConfigBase.CtrlCnlTable.Name];
                CloseChildForms(inCnlTableNode, true, true);
                CloseChildForms(ctrlCnlTableNode, true, true);
                explorerBuilder.FillChannelTableNodes(inCnlTableNode, ctrlCnlTableNode, project.ConfigBase);
            }
        }


        private void cmsDirectory_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable the menu items
            TreeNode selectedNode = tvExplorer.SelectedNode;
            bool isDirectoryNode = selectedNode != null && selectedNode.TagIs(AppNodeType.Directory);
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
            TreeNode selectedNode = tvExplorer.SelectedNode;
            bool isInstanceNode = selectedNode != null && selectedNode.TagIs(AppNodeType.Instance);
            bool instanceExists = project != null && project.Instances.Count > 0;

            miInstanceMoveUp.Enabled = isInstanceNode && selectedNode.PrevNode != null;
            miInstanceMoveDown.Enabled = isInstanceNode && selectedNode.NextNode != null;
            miInstanceDelete.Enabled = isInstanceNode;

            miInstanceProfile.Enabled = instanceExists;
            miInstanceDownloadConfig.Enabled = instanceExists;
            miInstanceUploadConfig.Enabled = instanceExists;
            miInstanceStatus.Enabled = instanceExists;

            miInstanceOpenInExplorer.Enabled = isInstanceNode;
            miInstanceOpenInBrowser.Enabled = isInstanceNode;
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
                            SetDeployMenuItemsEnabled();
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

                project.DeploymentSettings.RemoveProfilesByInstance(liveInstance.Instance.ID, out bool affected);
                if (affected && !project.DeploymentSettings.Save(out errMsg))
                    appData.ProcError(errMsg);

                SetDeployMenuItemsEnabled();
                SaveProjectSettings();
            }
        }

        private void miInstanceOpenInBrowser_Click(object sender, EventArgs e)
        {
            // open the web application of the instance
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(AppNodeType.Instance) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                LoadDeploymentSettings();
                DeploymentProfile profile = GetDeploymentProfile(liveInstance.Instance.DeploymentProfile);

                if (profile != null && ScadaUtils.IsValidUrl(profile.WebUrl))
                    Process.Start(profile.WebUrl);
                else
                    ScadaUiUtils.ShowWarning(AppPhrases.WebUrlNotSet);
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
                    CloseChildForms(selectedNode);
                    RefreshEnvironments(liveInstance);
                    RefreshInstanceNode(selectedNode, liveInstance);
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
            TreeNode selectedNode = tvExplorer.SelectedNode;
            bool isCommLineNode = selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine);
            bool isLocal = FindClosestInstance(selectedNode, out LiveInstance liveInstance) &&
                liveInstance.CommEnvironment.AgentClient != null && liveInstance.CommEnvironment.AgentClient.IsLocal;

            miCommLineMoveUp.Enabled = isCommLineNode && selectedNode.PrevNode != null;
            miCommLineMoveDown.Enabled = isCommLineNode && selectedNode.NextNode != null;
            miCommLineDelete.Enabled = isCommLineNode;

            miCommLineStart.Enabled = isCommLineNode && isLocal;
            miCommLineStop.Enabled = isCommLineNode && isLocal;
            miCommLineRestart.Enabled = isCommLineNode && isLocal;
        }

        private void miCommLineImport_Click(object sender, EventArgs e)
        {
            // import Communicator settings
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                CommEnvironment commEnv = liveInstance.CommEnvironment;
                FrmCommImport frmCommImport = new FrmCommImport(project, liveInstance.Instance, commEnv);
                TreeNode lastAddedNode = null;

                if (selectedNode.TagIs(CommNodeType.CommLines))
                {
                    // import communication lines and devices
                    if (frmCommImport.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Comm.Settings.CommLine commLineSettings in frmCommImport.ImportedCommLines)
                        {
                            TreeNode commLineNode = commShell.CreateCommLineNode(commLineSettings, commEnv);
                            selectedNode.Nodes.Add(commLineNode);
                            lastAddedNode = commLineNode;
                        }
                    }
                }
                else if (selectedNode.TagIs(CommNodeType.CommLine))
                {
                    // import only devices
                    Comm.Settings.CommLine commLineSettings = GetRelatedObject<Comm.Settings.CommLine>(selectedNode);
                    frmCommImport.CommLineSettings = commLineSettings;

                    if (frmCommImport.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Comm.Settings.KP kpSettings in frmCommImport.ImportedDevices)
                        {
                            TreeNode kpNode = commShell.CreateDeviceNode(kpSettings, commLineSettings, commEnv);
                            selectedNode.Nodes.Add(kpNode);
                            lastAddedNode = kpNode;
                        }

                        UpdateLineParams(lastAddedNode);
                    }
                }

                if (lastAddedNode != null)
                {
                    explorerBuilder.SetContextMenus(selectedNode);
                    tvExplorer.SelectedNode = lastAddedNode;
                    SaveCommSettigns(liveInstance);
                }
            }
        }

        private void miCommLineSync_Click(object sender, EventArgs e)
        {
            // synchronize Communicator settings
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null &&
                (selectedNode.TagIs(CommNodeType.CommLines) || selectedNode.TagIs(CommNodeType.CommLine)) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                FrmCommSync frmCommSync = new FrmCommSync(project, liveInstance.Instance)
                {
                    CommLineSettings = GetRelatedObject<Comm.Settings.CommLine>(selectedNode, false)
                };

                if (frmCommSync.ShowDialog() == DialogResult.OK)
                {
                    TreeNode commLinesNode = selectedNode.FindClosest(CommNodeType.CommLines);

                    if (frmCommSync.CommLineSettings == null)
                    {
                        commShell.UpdateNodeText(commLinesNode);
                        UpdateChildFormHints(commLinesNode);

                        foreach (TreeNode commLineNode in commLinesNode.Nodes)
                        {
                            UpdateLineParams(commLineNode.FindFirst(CommNodeType.LineParams));
                        }
                    }
                    else
                    {
                        TreeNode commLineNode = FindTreeNode(frmCommSync.CommLineSettings, commLinesNode);
                        commShell.UpdateNodeText(commLineNode);
                        UpdateChildFormHints(commLineNode);
                        UpdateLineParams(commLineNode.FindFirst(CommNodeType.LineParams));
                    }

                    SaveCommSettigns(liveInstance);
                }
            }
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

        private void miCommLineStartStop_Click(object sender, EventArgs e)
        {
            // start, stop or restart communication line
            TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                Comm.Settings.CommLine commLine = GetRelatedObject<Comm.Settings.CommLine>(selectedNode);
                CommLineCmd commLineCmd;

                if (sender == miCommLineStart)
                    commLineCmd = CommLineCmd.StartLine;
                else if (sender == miCommLineStop)
                    commLineCmd = CommLineCmd.StopLine;
                else
                    commLineCmd = CommLineCmd.RestartLine;

                if (new CommLineCommand(commLine, liveInstance.CommEnvironment).Send(commLineCmd, out string msg))
                    ScadaUiUtils.ShowInfo(msg);
                else
                    ScadaUiUtils.ShowError(msg);
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
                Comm.Settings.KP kp = GetRelatedObject<Comm.Settings.KP>(selectedNode);
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
                Comm.Settings.CommLine commLine = GetRelatedObject<Comm.Settings.CommLine>(commLineNode);
                Comm.Settings.KP kp = GetRelatedObject<Comm.Settings.KP>(selectedNode);

                if (liveInstance.CommEnvironment.TryGetKPView(kp, false, commLine.CustomParams, 
                    out KPView kpView, out string errMsg))
                {
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
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }
    }
}
