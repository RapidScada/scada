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
 * Summary  : Form for creating a new project
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for creating a new project.
    /// <para>Форма создания нового проекта.</para>
    /// </summary>
    public partial class FrmNewProject : Form
    {
        /// <summary>
        /// Item of the project template list.
        /// <para>Элемент списка шаблонов проекта.</para>
        private class TemplateItem
        {
            public string Name { get; set; }
            public string Descr { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private readonly AppData appData; // the common data of the application


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmNewProject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmNewProject(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
        }


        /// <summary>
        /// Fills the list of available templates.
        /// </summary>
        private void FillTemplateList()
        {
            try
            {
                cbTemplate.BeginUpdate();

                // search for project files
                DirectoryInfo templateDirInfo = new DirectoryInfo(appData.AppDirs.TemplateDir);
                int selectedIndex = 0;
                string cultureTemplate = "." + Localization.Culture.Name + ".";

                foreach (DirectoryInfo projectDirInfo in 
                    templateDirInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    foreach (FileInfo projectFileInfo in 
                        projectDirInfo.EnumerateFiles("*" + AdminUtils.ProjectExt, SearchOption.TopDirectoryOnly))
                    {
                        if (ScadaProject.LoadDescription(projectFileInfo.FullName, 
                            out string description, out string errMsg))
                        {
                            cbTemplate.Items.Add(new TemplateItem()
                            {
                                Name = projectFileInfo.Name,
                                Descr = description
                            });

                            if (projectFileInfo.Name.Contains(cultureTemplate))
                                selectedIndex = cbTemplate.Items.Count - 1;
                        }
                        else
                        {
                            appData.ErrLog.WriteError(errMsg);
                        }
                    }
                }

                // select the template by default
                if (cbTemplate.Items.Count > 0)
                    cbTemplate.SelectedIndex = selectedIndex;
            }
            finally
            {
                cbTemplate.EndUpdate();
            }
        }


        private void FrmNewProject_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Admin.App.Forms.FrmNewProject");
            txtName.Text = ScadaProject.DefaultName;
            txtLocation.Text = appData.AppState.ProjectDir;
            FillTemplateList();
        }

        private void cbTemplate_TextChanged(object sender, EventArgs e)
        {
            // show a template description
            if (cbTemplate.SelectedItem is TemplateItem templateItem)
            {
                txtTemplateDescr.Text = templateItem.Descr;
            }
            else if (File.Exists(cbTemplate.Text))
            {
                txtTemplateDescr.Text = 
                    ScadaProject.LoadDescription(cbTemplate.Text, out string description, out string errMsg) ?
                    description : errMsg;
            }
            else
            {
                txtTemplateDescr.Text = "";
            }
        }

        private void btnBrowseLocation_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
