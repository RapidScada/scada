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
 * Summary  : Form for selecting the culture
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Form for selecting the culture.
    /// <para>Форма для выбора культуры.</para>
    /// </summary>
    public partial class FrmCulture : Form
    {
        /// <summary>
        /// List item representing a culture.
        /// <para>Элемент списка, представляющий культуру.</para>
        /// </summary>
        private class CultureItem
        {
            public string CultureName { get; set; }
            public string DisplayName { get; set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private static string lastCultureName = null; // the last selected culture
        private readonly AppData appData; // the common data of the application


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCulture()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCulture(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
        }


        /// <summary>
        /// Loads available cultures and fills the combo box.
        /// </summary>
        private void LoadCultures()
        {
            try
            {
                // retrieve culture names from file names
                SortedSet<string> cultureNames = new SortedSet<string>();
                DirectoryInfo dirInfo = new DirectoryInfo(appData.AppDirs.LangDir);
                FileInfo[] langFiles = dirInfo.GetFiles("*.xml", SearchOption.TopDirectoryOnly);

                foreach (FileInfo fileInfo in langFiles)
                {
                    string s = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    int dotInd = s.LastIndexOf('.');
                    if (dotInd >= 0)
                        cultureNames.Add(s.Substring(dotInd + 1));
                }

                // fill the combo box
                try
                {
                    cbCulture.BeginUpdate();
                    cbCulture.Items.Clear();
                    string curCultureName = lastCultureName ?? Localization.Culture.Name;
                    int selectedIndex = -1;

                    foreach (string cultureName in cultureNames)
                    {
                        try
                        {
                            int index = cbCulture.Items.Add(new CultureItem
                            {
                                CultureName = cultureName,
                                DisplayName = cultureName + ", " + CultureInfo.GetCultureInfo(cultureName).NativeName
                            });

                            if (cultureName == curCultureName)
                                selectedIndex = index;
                        }
                        catch (CultureNotFoundException)
                        {
                        }
                    }

                    if (selectedIndex >= 0)
                        cbCulture.SelectedIndex = selectedIndex;
                    else
                        cbCulture.Text = curCultureName;
                }
                finally
                {
                    cbCulture.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, AppPhrases.LoadCulturesError);
            }
        }


        private void FrmCulture_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            LoadCultures();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cbCulture.Text == "")
            {
                ScadaUiUtils.ShowError(AppPhrases.CultureRequired);
                return;
            }

            // apply the selected culture
            string cultureName = cbCulture.SelectedItem is CultureItem cultureItem ? 
                cultureItem.CultureName : cbCulture.Text;

            try
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(cultureName);

                if (cultureInfo.EnglishName.StartsWith("Unknown"))
                {
                    ScadaUiUtils.ShowError(AppPhrases.CultureNotFound);
                }
                else if (Localization.WriteCulture(cultureName, out string errMsg))
                {
                    lastCultureName = cultureName;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    appData.ProcError(errMsg);
                }
            }
            catch (CultureNotFoundException)
            {
                ScadaUiUtils.ShowError(AppPhrases.CultureNotFound);
            }
        }
    }
}
