/*
 * Copyright 2021 Elena Shiryaeva
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModDbExport
 * Summary  : Module configuration form
 * 
 * Author   : Elena Shiryaeva
 * Created  : 2021
 * Modified : 2021
 */

using System;
using Scada.Server.Modules.DbExport.Config;
using Scada.UI;
using Scada.Config;
using System.IO;
using System.Collections.Generic;
using System.Linq; 
using System.Windows.Forms;

namespace Scada.Server.Modules.DbExport.UI
{
    /// <summary>
    /// Module configuration form.
    /// <para>Форма конфигурации модуля.</para>
    /// </summary>
    public partial class FrmDbExportConfig : Form
    {
        private readonly AppDirs appDirs; // application directories
        private bool modified;            // sign of configuration change
        private TreeNode selTargetNode;   // tree node of the selected target
        private TreeNode selTriggerNode;  // tree node of the selected trigger
        private ModConfig config;         // configuration
        private ModConfig configCopy;     // copy of configuration to implement rollback    
        private string configFileName;    // configuration file name


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDbExportConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDbExportConfig(AppDirs appDirs)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            modified = false;
            selTargetNode = null;
            selTriggerNode = null;
            config = null;
            configCopy = null;
            configFileName = "";
        }


        /// <summary>
        /// Gets or sets a sign of a configuration change.
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
                btnCancel.Enabled = modified;
            }
        }


        /// <summary>
        /// Returns an icon, depending on the selected node. 
        /// </summary>
        private string SelectDbIcon(ExportTargetConfig exportTargetConfig)
        {
            string suffix = exportTargetConfig.GeneralOptions.Active ? "" : "_inactive";

            switch (exportTargetConfig.ConnectionOptions.KnownDBMS)
            {
                case KnownDBMS.Oracle: 
                    return $"db_oracle{suffix}.png";

                case KnownDBMS.PostgreSQL:
                    return $"db_postgresql{suffix}.png";

                case KnownDBMS.MySQL:
                    return $"db_mysql{suffix}.png";

                case KnownDBMS.MSSQL:
                    return $"db_mssql{suffix}.png";

                case KnownDBMS.OLEDB:
                    return $"db_oledb{suffix}.png";

                default:
                    return "db_oledb.png";
            }
        }

        /// <summary>
        /// Selects an object image key.
        /// </summary>
        private string ChooseImageKey(object obj, bool expanded = false)
        {
            if (obj is ExportTargetConfig config)
                return SelectDbIcon(config);       
            else if (obj is TriggerOptionList)
                return (expanded ? "folder_open.png" : "folder_closed.png");
            else if (obj is DbConnectionOptions)
                return "connect.png";
            else if (obj is CurDataTriggerOptions options1)
                return options1.Active ? "tr_cur_data.png" : "tr_data_inactive.png";
            else if (obj is ArcDataTriggerOptions options2)
                return options2.Active ? "tr_arc_data.png" : "tr_data_inactive.png";
            else if (obj is EventTriggerOptions options3)
                return options3.Active ? "tr_event.png" : "tr_event_inactive.png";
            else if (obj is ArcUploadOptions)
                return "arc_upload.png";
            else
                return "options.png";
        }

        /// <summary>
        /// Creates a tree node corresponding to the target.
        /// </summary>
        private TreeNode CreateTargetNode(ExportTargetConfig exportTargetConfig)
        {
            return TreeViewUtils.CreateNode(exportTargetConfig, ChooseImageKey(exportTargetConfig));
        }

        /// <summary>
        /// Creates a tree node corresponding to the trigger.
        /// </summary>
        private TreeNode CreateTriggerNode(TriggerOptions triggerOptions)
        {
            return TreeViewUtils.CreateNode(triggerOptions, ChooseImageKey(triggerOptions));
        }

        /// <summary>
        /// Downloads information from the configuration file.
        /// </summary>
        private void LoadConfig()
        {
            // initialize common data
            configFileName = appDirs.ConfigDir + ModConfig.ConfigFileName;
            ctrlGeneralOptions.Visible = true;
            ctrlConnectionOptions.Visible = false;
            ctrlArcUploadOptions.Visible = false;
            ctrlTrigger.Visible = false;
            ctrlEventTrigger.Visible = false;

            config = new ModConfig();
            if (File.Exists(configFileName) && !config.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            FillTree();

            // creating a copy of the configuration
            configCopy = config.Clone();
        }

        /// <summary>
        /// Gets target ID.
        /// </summary>
        private int GetTagetID()
        {
            return config.ExportTargets.Count > 0 ? config.ExportTargets.Max(x => x.GeneralOptions.ID) + 1 : 1;
        }

        /// <summary>
        /// Adds a branch to the tree.
        /// </summary>
        private void AddBranch(ExportTargetConfig exportTargetConfig, bool insertNewTarget)
        {
            TreeNode targetNode = CreateTargetNode(exportTargetConfig);
            targetNode.Text = exportTargetConfig.GeneralOptions.Name;

            if (!insertNewTarget)
                tvTargets.Nodes.Add(targetNode);
            else
                tvTargets.Insert(null, targetNode, config.ExportTargets, exportTargetConfig);

            TreeNode connectionNode = TreeViewUtils.CreateNode(exportTargetConfig.ConnectionOptions,
                ChooseImageKey(exportTargetConfig.ConnectionOptions));
            connectionNode.Text = LibPhrases.ConnectionOptionsNode;
            targetNode.Nodes.Add(connectionNode);

            TreeNode triggerGroupNode = TreeViewUtils.CreateNode(exportTargetConfig.Triggers,
                ChooseImageKey(exportTargetConfig.Triggers));
            triggerGroupNode.Text = LibPhrases.TriggerGrNode;
            targetNode.Nodes.Add(triggerGroupNode);

            foreach (TriggerOptions triggerOptions in exportTargetConfig.Triggers)
            {
                TreeNode triggerNode = TreeViewUtils.CreateNode(triggerOptions,
                    ChooseImageKey(triggerOptions));
                triggerNode.Text = triggerOptions.Name;
                triggerGroupNode.Nodes.Add(triggerNode);
            }
      
            TreeNode arcUploadNode = TreeViewUtils.CreateNode(exportTargetConfig.ArcUploadOptions,
                ChooseImageKey(exportTargetConfig.ArcUploadOptions));
            arcUploadNode.Text = LibPhrases.ArcUploadOptionsNode;
            targetNode.Nodes.Add(arcUploadNode);

            if (insertNewTarget)
                tvTargets.SelectedNode.Expand();
        }

        /// <summary>
        /// Fills the tree according to the configuration file.
        /// </summary>
        private void FillTree()
        {
            selTargetNode = null;
            selTriggerNode = null;

            // tree cleaning and filling
            tvTargets.BeginUpdate();
            tvTargets.Nodes.Clear();

            foreach (ExportTargetConfig exportTargetConfig in config.ExportTargets)
            {
                AddBranch(exportTargetConfig, false);
            }

            tvTargets.ExpandAll();
            tvTargets.EndUpdate();

            // first target selection
            if (tvTargets.Nodes.Count > 0)
                tvTargets.SelectedNode = tvTargets.Nodes[0];
            
             SetActionButtonsEnabled();
        }

        /// <summary>
        /// Hides properties of objects.
        /// </summary>
        private void HideProps()
        {
            ctrlGeneralOptions.Visible = false;
            ctrlConnectionOptions.Visible = false;
            ctrlTrigger.Visible = false;
            ctrlEventTrigger.Visible = false;
            ctrlArcUploadOptions.Visible = false;
        }

        /// <summary>
        /// Set the input focus of the trigger edit control. 
        /// </summary>
        public void SetTriggerFocus()
        {
            if (ctrlTrigger.Visible)
                ctrlTrigger.SetFocus();
            else if (ctrlEventTrigger.Visible)
                ctrlEventTrigger.SetFocus();
        }

        /// <summary>
        /// Sets availability move and delete elements buttons.
        /// </summary>
        private void SetActionButtonsEnabled()
        {
            btnMoveUp.Enabled = tvTargets.MoveUpSelectedNodeIsEnabled(
                TreeViewUtils.MoveBehavior.ThroughSimilarParents) && tvTargets.SelectedNode.Parent == null;
            btnMoveDown.Enabled = tvTargets.MoveDownSelectedNodeIsEnabled(
                TreeViewUtils.MoveBehavior.ThroughSimilarParents) && tvTargets.SelectedNode.Parent == null;
            btnDelete.Enabled = (tvTargets.SelectedNode != null && tvTargets.SelectedNode.Parent == null) ||
                (selTriggerNode != null && ((tvTargets.SelectedNode?.Tag as DataTriggerOptions) != null ||
                (tvTargets.SelectedNode?.Tag as EventTriggerOptions) != null));
            btnAddArcTrigger.Enabled = selTargetNode != null;
            btnAddCurTrigger.Enabled = selTargetNode != null;
            btnAddEventTrigger.Enabled = selTargetNode != null;
        }

        /// <summary>
        /// Saves module configuration.
        /// </summary>
        private bool SaveConfig()
        {
            if (Modified)
            {
                if (ValidateConfig())
                {
                    if (config.Save(configFileName, out string errMsg))
                    {
                        configCopy = config.Clone();
                        Modified = false;
                        return true;
                    }
                    else
                    {
                        ScadaUiUtils.ShowError(errMsg);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Applies localization to the form.
        /// </summary>
        private void LocalizeForm()
        {
            if (Localization.LoadDictionaries(appDirs.LangDir, "ModDbExport", out string errMsg))
                Translator.TranslateForm(this, GetType().FullName, null, cmsTree);
            else
                ScadaUiUtils.ShowError(errMsg);

            LibPhrases.Init();
        }

        /// <summary>
        /// Checks target name for uniqueness.
        /// </summary>
        private bool CheckGateNamesUniqueness()
        {
            HashSet<string> targetNames = new HashSet<string>(
                from ExportTargetConfig exportTargetConfig in config.ExportTargets
                select exportTargetConfig.GeneralOptions.Name.ToLowerInvariant());

            return targetNames.Count >= config.ExportTargets.Count;
        }

        /// <summary>
        /// Checks names for empty value.
        /// </summary>
        private bool CheckNamesNull()
        {
            bool checkNamesNull = true;
            HashSet<string> names = new HashSet<string>(
                from ExportTargetConfig exportTargetConfig in config.ExportTargets
                select exportTargetConfig.GeneralOptions.Name);

            if (names.Contains(string.Empty))
                checkNamesNull = false;

            return checkNamesNull;
        }

        /// <summary>
        /// Checks the correctness of the target.
        /// </summary>
        private bool ValidateConfig()
        {
            if (!CheckNamesNull())
            {
                ScadaUiUtils.ShowError(LibPhrases.NameEmpty);
                return false;
            }
            else if (!CheckGateNamesUniqueness())
            {
                ScadaUiUtils.ShowError(LibPhrases.TargetNameNotUnique);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Shows target properties.
        /// </summary>
        private void ShowGeneralOptions(ExportTargetConfig exportTargetConfig)
        {
            if (exportTargetConfig == null)
            {
                ctrlGeneralOptions.Visible = false;
            }
            else
            {
                ctrlGeneralOptions.Visible = true;
                ctrlGeneralOptions.GeneralOptions = exportTargetConfig.GeneralOptions;
                ctrlTrigger.Visible = false;
                ctrlConnectionOptions.Visible = false;
                ctrlArcUploadOptions.Visible = false;
                ctrlEventTrigger.Visible = false;
            }
        }

        /// <summary>
        /// Shows connect properties.
        /// </summary>
        private void ShowConnectionOptions(DbConnectionOptions connectionOptions)
        {
            if (connectionOptions == null)
            {
                ctrlConnectionOptions.Visible = false;
            }
            else
            {
                ctrlConnectionOptions.Visible = true;
                ctrlConnectionOptions.ConnectionOptions = connectionOptions;
                ctrlTrigger.Visible = false;
                ctrlGeneralOptions.Visible = false;
                ctrlArcUploadOptions.Visible = false;
                ctrlEventTrigger.Visible = false;
            }
        }

        /// <summary>
        /// Shows data cur triggers.
        /// </summary>
        private void ShowTriggersProps(DataTriggerOptions triggerOptions, ExportTargetConfig exportTargetConfig)
        {
            if (triggerOptions == null)
            {
                ctrlTrigger.Visible = false;
            }
            else
            {
                ctrlTrigger.DbmsType = exportTargetConfig.ConnectionOptions.KnownDBMS;
                ctrlTrigger.Visible = true;
                ctrlTrigger.Clear();
                ctrlTrigger.DataTriggerOptions = triggerOptions;
                ctrlConnectionOptions.Visible = false;
                ctrlGeneralOptions.Visible = false;
                ctrlArcUploadOptions.Visible = false;
                ctrlEventTrigger.Visible = false;
            }
        }

        /// <summary>
        /// Shows data event triggers options.
        /// </summary>
        private void ShowEventTriggersProps(EventTriggerOptions eventTriggerOptions, ExportTargetConfig exportTargetConfig)
        {
            if (eventTriggerOptions == null)
            {
                ctrlEventTrigger.Visible = false;
            }
            else
            {
                ctrlEventTrigger.DbmsType = exportTargetConfig.ConnectionOptions.KnownDBMS;
                ctrlEventTrigger.Visible = true;
                ctrlEventTrigger.Clear();
                ctrlEventTrigger.EventTriggerOptions = eventTriggerOptions;
                ctrlConnectionOptions.Visible = false;
                ctrlGeneralOptions.Visible = false;
                ctrlArcUploadOptions.Visible = false;
                ctrlTrigger.Visible = false;
            }
        }

        /// <summary>
        /// Shows archive upload options.
        /// </summary>
        private void ShowArcUploadOptionsProps(ArcUploadOptions arcUploadOptions)
        {
            if (arcUploadOptions == null)
            {
                ctrlArcUploadOptions.Visible = false;
            }
            else
            {
                ctrlArcUploadOptions.Visible = true;
                ctrlArcUploadOptions.ArcUploadOptions = arcUploadOptions;
                ctrlTrigger.Visible = false;
                ctrlEventTrigger.Visible = false;
                ctrlGeneralOptions.Visible = false;
                ctrlConnectionOptions.Visible = false;
            }
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            LocalizeForm();
            LoadConfig();
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            Modified = false;
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(CommonPhrases.SaveSettingsConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!SaveConfig())
                            e.Cancel = true;
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void tvTargets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // definition of a chain of selected nodes and objects
            TreeNode selNode = tvTargets.SelectedNode;
            selTargetNode = selNode.FindClosest(typeof(ExportTargetConfig));
            selTriggerNode = selNode.FindClosest(typeof(TriggerOptionList));

            // display properties of the selected object
            object selObj = selNode?.Tag;
            object selTarget = selTargetNode?.Tag;

            if (selObj is ExportTargetConfig exportTargetConfig)
                ShowGeneralOptions(exportTargetConfig);
            else if (selObj is DbConnectionOptions dbConnectionOptions)
                ShowConnectionOptions(dbConnectionOptions);
            else if (selObj is DataTriggerOptions dataTriggerOptions)
                ShowTriggersProps(dataTriggerOptions, selTarget as ExportTargetConfig);
            else if (selObj is EventTriggerOptions eventTriggerOptions)
                ShowEventTriggersProps(eventTriggerOptions, selTarget as ExportTargetConfig);
            else if (selObj is ArcUploadOptions arcUploadOptions)
                ShowArcUploadOptionsProps(arcUploadOptions);
            else
                HideProps();

            // setting available of buttons for moving and deleting elements
            SetActionButtonsEnabled();
        }

        private void tvTargets_AfterExpand(object sender, TreeViewEventArgs e)
        {
            // set the icon if the group was expanded 
            if (e.Node.Tag is TriggerOptionList)
                e.Node.SetImageKey(ChooseImageKey(e.Node.Tag, true));
        }

        private void tvTargets_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            // set the icon if the group was collaosed
            if (e.Node.Tag is TriggerOptionList)
                e.Node.SetImageKey(ChooseImageKey(e.Node.Tag, false));
        }

        private void btnAddTagret_Click(object sender, EventArgs e)
        {
            // add target
            ExportTargetConfig target = new ExportTargetConfig { Parent = config };
            target.GeneralOptions.ID = GetTagetID();
            target.GeneralOptions.Name = LibPhrases.TargetName + " " + target.GeneralOptions.ID;

            // add dbconnection setting
            if (sender == btnSqlServer)
                target.ConnectionOptions.KnownDBMS = KnownDBMS.MSSQL;
            else if (sender == btnOracle)
                target.ConnectionOptions.KnownDBMS = KnownDBMS.Oracle;
            else if (sender == btnPostgreSql)
                target.ConnectionOptions.KnownDBMS = KnownDBMS.PostgreSQL;
            else if (sender == btnMySql)
                target.ConnectionOptions.KnownDBMS = KnownDBMS.MySQL;
            else if (sender == btnOleDb)
                target.ConnectionOptions.KnownDBMS = KnownDBMS.OLEDB;
            else
                throw new ScadaException("Unknown DBMS.");

            AddBranch(target, true);
            Modified = true;
            ctrlGeneralOptions.SetFocus();
        }

        private void btnAddTrigger_Click(object sender, EventArgs e)
        {
            // add trigger
            if (selTargetNode != null)
            {
                TriggerOptions trigger;

                if (sender == btnAddCurTrigger)
                    trigger = new CurDataTriggerOptions { Name = LibPhrases.CurDataTrigger };
                else if (sender == btnAddArcTrigger)
                    trigger = new ArcDataTriggerOptions { Name = LibPhrases.ArcDataTrigger };
                else if (sender == btnAddEventTrigger)
                    trigger = new EventTriggerOptions { Name = LibPhrases.EventTrigger };
                else
                    trigger = null;

                if (trigger != null)
                {
                    TreeNode triggerNode = CreateTriggerNode(trigger);
                    triggerNode.Text = trigger.Name;

                    if (selTriggerNode != null)
                        tvTargets.Insert(selTriggerNode, triggerNode);
                    else if (selTargetNode.FindFirst(typeof(TriggerOptionList)) is TreeNode treeNodeInsertTriggers)
                        tvTargets.Add(treeNodeInsertTriggers, triggerNode);

                    SetTriggerFocus();
                    Modified = true;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // delete selected item
            if (selTargetNode != null)
            {
                tvTargets.RemoveSelectedNode();

                tvTargets.Select();
                Modified = true;

                if (tvTargets.Nodes.Count == 0)
                    HideProps();

                // setting available  of buttons for moving and deleting elements
                SetActionButtonsEnabled();
            }
        }

        private void btnMoveUpDown_Click(object sender, EventArgs e)
        {
            // move the selected item up or down
            if (sender == btnMoveUp)
                tvTargets.MoveUpSelectedNode(TreeViewUtils.MoveBehavior.ThroughSimilarParents);
            else
                tvTargets.MoveDownSelectedNode(TreeViewUtils.MoveBehavior.ThroughSimilarParents);

            Modified = true;
            tvTargets.Select();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // save module configuration
            SaveConfig();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // cancel configuration changes
            config = configCopy;
            configCopy = config.Clone();
            LoadConfig();
            Modified = false;
        }

        private void miExpandAll_Click(object sender, EventArgs e)
        {
            tvTargets.ExpandAll();
        }

        private void miCollapseAll_Click(object sender, EventArgs e)
        {
            tvTargets.CollapseAll();
        }

        private void ctrlGeneralOptions_GeneralOptionsChanged(object sender, ObjectChangedEventArgs e)
        {
            Modified = true;

            if (selTargetNode != null)
            {
                selTargetNode.Text = (selTargetNode?.Tag as ExportTargetConfig).GeneralOptions.Name;
                selTargetNode.SetImageKey(ChooseImageKey(selTargetNode?.Tag as ExportTargetConfig));
            }
        }

        private void ctrlConnectionOptions_ConnectChanged(object sender, ObjectChangedEventArgs e)
        {
            Modified = true;
        }

        private void ctrlArcUploadOptions_ArcUploadOptionsChanged(object sender, ObjectChangedEventArgs e)
        {
            Modified = true;
        }

        private void ctrlTriggers_TriggerOptionsChanged(object sender, ObjectChangedEventArgs e)
        {
            Modified = true;

            // updating the trigger name in the tree
            if (selTriggerNode != null)
            {
                tvTargets.SelectedNode.Text = (tvTargets.SelectedNode?.Tag as DataTriggerOptions).Name;
                tvTargets.SelectedNode.SetImageKey(ChooseImageKey(e.ChangedObject));
            }
        }

        private void ctrlEventTriggers_TriggerEventOptionsChanged(object sender, ObjectChangedEventArgs e)
        {
            Modified = true;

            // updating the trigger name in the tree  
            if (selTriggerNode != null)
            {
                tvTargets.SelectedNode.Text = (tvTargets.SelectedNode?.Tag as TriggerOptions).Name;
                tvTargets.SelectedNode.SetImageKey(ChooseImageKey(e.ChangedObject));
            }
        }
    }
}
