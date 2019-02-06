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
 * Summary  : Form for generating channel map
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.UI;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Form for generating channel map.
    /// <para>Форма для генерации карты каналов.</para>
    /// </summary>
    public partial class FrmCnlMap : Form
    {
        /// <summary>
        /// The file name of newly created maps.
        /// </summary>
        private const string MapFileName = "ScadaAdminCnlMap.txt";

        private readonly ConfigBase configBase; // the configuration database
        private readonly AppData appData;       // the common data of the application


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCnlMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlMap(ConfigBase configBase, AppData appData)
            : this()
        {
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
            this.appData = appData ?? throw new ArgumentNullException("appData");
        }


        /// <summary>
        /// Generates a channel map.
        /// </summary>
        private void GenerateCnlMap(bool inCnlMap, bool groupByDevices)
        {
            try
            {
                string mapFileName = Path.Combine(appData.AppDirs.LogDir, MapFileName);

                using (StreamWriter writer = new StreamWriter(mapFileName, false, Encoding.UTF8))
                {
                    string title = inCnlMap ? AppPhrases.InCnlMapTitle : AppPhrases.OutCnlMapTitle;
                    writer.WriteLine(title);
                    writer.WriteLine(new string('-', title.Length));
                }

                AppUtils.OpenTextFile(mapFileName);
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, AppPhrases.GenerateCnlMapError);
            }
        }


        private void FrmCnlMap_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Admin.App.Forms.Tools.FrmCnlMap");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GenerateCnlMap(rbInCnls.Checked, rbGroupByDevices.Checked);
            DialogResult = DialogResult.OK;
        }
    }
}
