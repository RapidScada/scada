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
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Hyperlink to the documentation in English
        /// </summary>
        private const string DocEnUrl = "http://doc.rapidscada.net/content/en/";
        /// <summary>
        /// Hyperlink to the documentation in Russian
        /// </summary>
        private const string DocRuUrl = "http://doc.rapidscada.net/content/ru/";
        /// <summary>
        /// Hyperlink to the support in English
        /// </summary>
        private const string SupportEnUrl = "https://forum.rapidscada.org/";
        /// <summary>
        /// Hyperlink to the support in Russian
        /// </summary>
        private const string SupportRuUrl = "https://forum.rapidscada.ru/";

        private AppData appData;  // common data of the application
        private readonly Log log; // application log


        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        private FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        public FrmMain(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            log = appData.ErrLog;
        }


        /// <summary>
        /// Apply localization to the form
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
                //AppPhrases.Init();
                //ofdScheme.Filter = sfdScheme.Filter = AppPhrases.SchemeFileFilter;
            }
            else
            {
                log.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Fill the explorer tree according to the opened project
        /// </summary>
        private void FillTreeView()
        {
            tvExplorer.BeginUpdate();
            TreeNode node1 = new TreeNode("Root");
            tvExplorer.Nodes.Add(node1);

            TreeNode node2 = new TreeNode("Child 1");
            node2.Tag = new TreeNodeTag()
            {
                FormType = typeof(FrmBaseTable)
            };
            node1.Nodes.Add(node2);

            TreeNode node3 = new TreeNode("Child 2");
            node3.Tag = new TreeNodeTag()
            {
                FormType = typeof(FrmBaseTable)
            };
            node1.Nodes.Add(node3);

            node1.Expand();
            tvExplorer.EndUpdate();
        }

        /// <summary>
        /// Execute an action related to the node
        /// </summary>
        private void ExecNodeAction(TreeNodeTag tag)
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
                        wctrlMain.AddForm(form, "", null);
                    }
                }
            }
            else
            {
                // activate the existing form
                wctrlMain.ActivateForm(tag.ExistingForm);
            }
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();
            FillTreeView();
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
            if (e.Button == MouseButtons.Left && e.Node.Tag is TreeNodeTag tag)
                ExecNodeAction(tag);
        }


        private void miFileNew_Click(object sender, EventArgs e)
        {

        }

        private void miFileOpen_Click(object sender, EventArgs e)
        {

        }

        private void miFileSave_Click(object sender, EventArgs e)
        {

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
