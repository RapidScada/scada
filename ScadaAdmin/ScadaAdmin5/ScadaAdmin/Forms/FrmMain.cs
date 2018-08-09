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
using Scada.Admin.Project;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private readonly AppData appData; // the common data of the application
        private readonly Log log;         // the application log
        private readonly ExplorerBuilder explorerBuilder; // the object to manipulate the explorer tree
        private ScadaProject project;     // the project under development


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
            explorerBuilder = new ExplorerBuilder(tvExplorer);
            project = null;
        }


        /// <summary>
        /// Applies localization to the form.
        /// </summary>
        private void LocalizeForm()
        {
            if (Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaData", out string errMsg))
                CommonPhrases.Init();
            else
                log.WriteError(errMsg);

            if (Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaAdmin", out errMsg))
            {
                Translator.TranslateForm(this, "Scada.Admin.App.Forms.FrmMain");
                //ofdScheme.Filter = sfdScheme.Filter = AppPhrases.SchemeFileFilter;
            }
            else
            {
                log.WriteError(errMsg);
            }

            AppPhrases.Init();
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
                        object formObj = tag.Arguments == null ?
                            Activator.CreateInstance(tag.FormType) :
                            Activator.CreateInstance(tag.FormType, tag.Arguments);

                        // display the form
                        if (formObj is Form form)
                        {
                            tag.ExistingForm = form;
                            wctrlMain.AddForm(form, "", null, treeNode);
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
        /// Finds a tree node tag that relates to the specified child form.
        /// </summary>
        private TreeNodeTag FindNodeTag(Form childForm)
        {
            foreach (TreeNode node in TreeViewUtils.IterateNodes(tvExplorer.Nodes))
            {
                if (node.Tag is TreeNodeTag tag && tag.ExistingForm == childForm)
                    return tag;
            }

            return null;
        }

        /// <summary>
        /// Creates and displays a new project
        /// </summary>
        private void CreateProject()
        {
            project = new ScadaProject();
            explorerBuilder.CreateNodes(project);
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();
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
            //MessageBox.Show(e.Clicks.ToString());
        }

        private void tvExplorer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ExecNodeAction(e.Node);
        }

        private void wctrlMain_ChildFormClosed(object sender, ChildFormClosedEventArgs e)
        {
            // clear the form pointer of the node
            TreeNodeTag nodeTag;

            if (e.ChildForm is IChildForm childForm)
            {
                nodeTag = childForm.ChildFormTag.TreeNode.Tag as TreeNodeTag;
            }
            else
            {
                nodeTag = FindNodeTag(e.ChildForm);
            }

            if (nodeTag != null)
                nodeTag.ExistingForm = null;
        }


        private void miFileNew_Click(object sender, EventArgs e)
        {

        }

        private void miFileOpen_Click(object sender, EventArgs e)
        {

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
    }
}
