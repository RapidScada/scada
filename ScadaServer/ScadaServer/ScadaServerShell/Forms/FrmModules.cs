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
 * Module   : Server Shell
 * Summary  : Form for editing a list of Server modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Server.Modules;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;
using WinControl;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for editing a list of Server modules.
    /// <para>Форма редактирования списка модулей Сервера.</para>
    /// </summary>
    public partial class FrmModules : Form, IChildForm
    {
        /// <summary>
        /// List item representing a module.
        /// <para>Элемент списка, представляющий модуль.</para>
        /// </summary>
        private class ModuleItem
        {
            public string FileName { get; set; }
            public string Descr { get; set; }
            public ModView ModView { get; set; }

            public override string ToString()
            {
                return FileName;
            }
        }

        private readonly Settings settings; // the application settings
        private readonly ServerEnvironment environment; // the application environment


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmModules()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmModules(Settings settings, ServerEnvironment environment)
            : this()
        {
            this.settings = settings ?? throw new ArgumentNullException("settings");
            this.environment = environment ?? throw new ArgumentNullException("environment");
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Fills the lists of modules.
        /// </summary>
        private void FillModuleLists()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(environment.AppDirs.ModDir);

            FileInfo[] fileInfoArr = dirInfo.Exists ? 
                dirInfo.GetFiles("Mod*.dll", SearchOption.TopDirectoryOnly) :
                new FileInfo[0];

            foreach (FileInfo fileInfo in fileInfoArr)
            {
                ModuleItem moduleItem = new ModuleItem() { FileName = fileInfo.Name };

                try
                {
                    ModView modView = ModFactory.GetModView(fileInfo.FullName);
                    modView.AppDirs = environment.AppDirs;

                    moduleItem.Descr = modView.Descr;
                    moduleItem.ModView = modView;
                }
                catch (Exception ex)
                {
                    moduleItem.Descr = ex.Message;
                    moduleItem.ModView = null;
                }

                if (settings.ModuleFileNames.Contains(moduleItem.FileName))
                    lbActiveModules.Items.Add(moduleItem);
                else
                    lbUnusedModules.Items.Add(moduleItem);
            }
        }

        /// <summary>
        /// Shows a description of the specified item.
        /// </summary>
        private void ShowItemDescr(object item)
        {
            if (item is ModuleItem moduleItem)
                txtDescr.Text = moduleItem.Descr;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            ChildFormTag.Modified = false;
        }


        private void FrmModules_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Server.Shell.Forms.FrmModules");
            FillModuleLists();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {

        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {

        }

        private void btnProperties_Click(object sender, EventArgs e)
        {

        }

        private void lbUnusedModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowItemDescr(lbUnusedModules.SelectedItem);
        }

        private void lbActiveModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowItemDescr(lbActiveModules.SelectedItem);
        }
    }
}
