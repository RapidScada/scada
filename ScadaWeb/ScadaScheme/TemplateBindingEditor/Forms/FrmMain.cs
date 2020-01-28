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
 * Module   : Template Binding Editor
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Scheme.Template;
using Scada.Scheme.TemplateBindingEditor.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Scada.Scheme.TemplateBindingEditor.Forms
{
    /// <summary>
    /// Main form of the application.
    /// <para>Главная форма приложения.</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// The default file name for bindings.
        /// </summary>
        private const string DefBindingsFileName = "NewBindings.stb";
        /// <summary>
        /// The pattern to search the interface directory.
        /// </summary>
        private const string InterfaceDirPattern = "Interface";

        private readonly string exeDir;  // the directory of the executable file
        private readonly string langDir; // the directory of language files

        private TemplateBindings templateBindings; // the edited bindings
        private string fileName;                   // the bindings file name
        private string interfaceDir;               // the interface directory of the project
        private bool changing;                     // controls are being changed programmatically
        private bool modified;                     // the bindings were modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            langDir = Path.Combine(exeDir, "Lang");

            templateBindings = null;
            fileName = "";
            interfaceDir = "";
            changing = false;
            modified = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the bindings were modified.
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
                DisplayTitle();
            }
        }


        /// <summary>
        /// Applies localization to the form.
        /// </summary>
        private void LocalizeForm()
        {
            Localization.LoadDictionaries(langDir, "ScadaData", out string errMsg);
            Localization.LoadDictionaries(langDir, "ScadaScheme", out errMsg);
            bool appDictLoaded = Localization.LoadDictionaries(langDir, "TemplateBindingEditor", out errMsg);

            CommonPhrases.Init();
            SchemePhrases.Init();
            AppPhrases.Init();

            if (appDictLoaded)
            {
                Translator.TranslateForm(this, GetType().FullName, null);
                ofdBindings.SetFilter(AppPhrases.BindingsFileFilter);
                sfdBindings.SetFilter(AppPhrases.BindingsFileFilter);
            }
        }

        /// <summary>
        /// Creates new bindings.
        /// </summary>
        private void NewBindings()
        {
            fileName = "";
            interfaceDir = "";
            templateBindings = new TemplateBindings();
            DisplayInterfaceDir();
            DisplayBindings();
            Modified = false;
        }

        /// <summary>
        /// Opens bindings from the specified file.
        /// </summary>
        private void OpenBindings(string fileName)
        {
            this.fileName = fileName;
            templateBindings = new TemplateBindings();

            if (!templateBindings.Load(fileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            FindInterfaceDir();
            DisplayInterfaceDir();
            DisplayBindings();
            Modified = false;
        }

        /// <summary>
        /// Opens or creates bindings.
        /// </summary>
        private void OpenOrCreateBindings(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                NewBindings();
            else
                OpenBindings(fileName);
        }

        /// <summary>
        /// Saves the bindings.
        /// </summary>
        private bool SaveBindings()
        {
            return string.IsNullOrEmpty(fileName) ? SaveBindingsAs() : SaveBindings(fileName);
        }

        /// <summary>
        /// Saves the bindings with the specified file name.
        /// </summary>
        private bool SaveBindings(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name must not be empty.", fileName);

            if (templateBindings.Save(fileName, out string errMsg))
            {
                this.fileName = fileName;
                FindInterfaceDir();
                DisplayInterfaceDir();
                Modified = false;
                return true;
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Saves the bindings with the file name selected by a user.
        /// </summary>
        private bool SaveBindingsAs()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                sfdBindings.FileName = DefBindingsFileName;
            }
            else
            {
                sfdBindings.InitialDirectory = Path.GetDirectoryName(fileName);
                sfdBindings.FileName = Path.GetFileName(fileName);
            }

            return sfdBindings.ShowDialog() == DialogResult.OK && SaveBindings(sfdBindings.FileName);
        }

        /// <summary>
        /// Confirms that the current bindings can be closed.
        /// </summary>
        private bool ConfirmCloseBindings()
        {
            if (Modified)
            {
                switch (MessageBox.Show(AppPhrases.SaveBindingsConfirm, CommonPhrases.QuestionCaption,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        return SaveBindings();
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Displays the form title.
        /// </summary>
        private void DisplayTitle()
        {
            if (templateBindings != null)
            {
                Text = string.Format(AppPhrases.EditorTitle,
                    (string.IsNullOrEmpty(fileName) ? DefBindingsFileName : Path.GetFileName(fileName)) +
                    (Modified ? "*" : ""));
            }
        }

        /// <summary>
        /// Displays the interface directory.
        /// </summary>
        private void DisplayInterfaceDir()
        {
            lblInterfaceDir.Text = string.IsNullOrEmpty(interfaceDir) ?
              AppPhrases.InterfaceDirNotFound :
              interfaceDir;
        }

        /// <summary>
        /// Displays the edited bindings.
        /// </summary>
        private void DisplayBindings()
        {
            changing = true;
            txtTemplateFileName.Text = templateBindings.TemplateFileName;
            txtTemplateFileName.TextChanged += txtTemplateFileName_TextChanged;
            changing = false;

            LoadSchemeTemplate(false);
        }

        /// <summary>
        /// Finds the interface directory, relative to the bindings file name.
        /// </summary>
        private bool FindInterfaceDir()
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(fileName));

                while (dirInfo.Parent != null)
                {
                    dirInfo = dirInfo.Parent;

                    foreach (DirectoryInfo parentDirInfo in
                        dirInfo.EnumerateDirectories(InterfaceDirPattern, SearchOption.TopDirectoryOnly))
                    {
                        interfaceDir = ScadaUtils.NormalDir(parentDirInfo.FullName);
                        return true;
                    }
                }
            }

            interfaceDir = "";
            return false;
        }

        /// <summary>
        /// Loads a scheme template if possible. 
        /// </summary>
        private void LoadSchemeTemplate(bool showMsg)
        {
            // display empty data
            changing = true;
            cbTitleComponent.DataSource = null;
            bsBindings.DataSource = null;

            if (string.IsNullOrEmpty(interfaceDir))
            {
                if (showMsg)
                    ScadaUiUtils.ShowWarning(AppPhrases.UnableLoadTemplate);
            }
            else
            {
                string templateFileName = Path.Combine(interfaceDir, txtTemplateFileName.Text);

                if (File.Exists(templateFileName))
                {
                    if (SchemeParser.Parse(templateFileName, out List<ComponentItem> components, 
                        out List<ComponentBindingItem> newComponentBindings, out string errMsg))
                    {
                        // merge bindings
                        foreach (ComponentBindingItem bindingItem in newComponentBindings)
                        {
                            if (templateBindings.ComponentBindings.TryGetValue(bindingItem.CompID, 
                                out ComponentBinding binding))
                            {
                                bindingItem.InCnlNum = binding.InCnlNum;
                                bindingItem.CtrlCnlNum = binding.CtrlCnlNum;
                            }
                        }

                        templateBindings.ComponentBindings.Clear();
                        newComponentBindings.ForEach(x => { templateBindings.ComponentBindings[x.CompID] = x; });

                        // fill the component combo box
                        components.Sort();
                        cbTitleComponent.ValueMember = "ID";
                        cbTitleComponent.DisplayMember = "DisplayName";
                        cbTitleComponent.DataSource = components;
                        cbTitleComponent.SelectedValue = templateBindings.TitleCompID;

                        // display bindings
                        bsBindings.DataSource = templateBindings.ComponentBindings.Values;

                        if (showMsg)
                            ScadaUiUtils.ShowInfo(AppPhrases.TemplateLoaded);
                    }
                    else
                    {
                        bsBindings.DataSource = null;
                        ScadaUiUtils.ShowError(errMsg);
                    }
                }
                else if (showMsg)
                {
                    ScadaUiUtils.ShowWarning(string.Format(AppPhrases.TemplateNotFound, templateFileName));
                }
            }

            changing = false;
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();
            string[] args = Environment.GetCommandLineArgs();
            OpenOrCreateBindings(args.Length > 1 ? args[1] : "");
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // confirm saving the bindings before closing
            e.Cancel = !ConfirmCloseBindings();
        }

        private void btnFileNew_Click(object sender, EventArgs e)
        {
            if (ConfirmCloseBindings())
                NewBindings();
        }

        private void btnFileOpen_Click(object sender, EventArgs e)
        {
            if (ConfirmCloseBindings())
            {
                ofdBindings.FileName = "";

                if (ofdBindings.ShowDialog() == DialogResult.OK)
                {
                    ofdBindings.InitialDirectory = Path.GetDirectoryName(ofdBindings.FileName);
                    OpenBindings(ofdBindings.FileName);
                }
            }
        }

        private void btnFileSave_Click(object sender, EventArgs e)
        {
            SaveBindings();
        }

        private void btnFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveBindingsAs();
        }

        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {
            ofdScheme.FileName = "";

            if (string.IsNullOrEmpty(interfaceDir))
            {
                ScadaUiUtils.ShowWarning(AppPhrases.UnableOpenTemplate);
            }
            else if (ofdScheme.ShowDialog() == DialogResult.OK)
            {
                if (ofdScheme.FileName.StartsWith(interfaceDir, StringComparison.OrdinalIgnoreCase))
                {
                    txtTemplateFileName.Text = ofdScheme.FileName.Substring(interfaceDir.Length);
                    LoadSchemeTemplate(true);
                }
                else
                {
                    ScadaUiUtils.ShowWarning(AppPhrases.WrongTemplatePath);
                }
            }
        }

        private void btnReloadTemplate_Click(object sender, EventArgs e)
        {
            LoadSchemeTemplate(true);
        }

        private void txtTemplateFileName_TextChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                templateBindings.TemplateFileName = txtTemplateFileName.Text;
                Modified = true;
            }
        }

        private void cbTitleComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                templateBindings.TitleCompID = cbTitleComponent.SelectedValue is int id ? id : 0;
                Modified = true;
            }
        }
    }
}
