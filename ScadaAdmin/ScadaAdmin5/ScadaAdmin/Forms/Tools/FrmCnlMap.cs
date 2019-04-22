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
        private const string MapFileName = "ScadaAdmin_CnlMap.txt";

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

            IncludeInCnls = true;
            IncludeOutCnls = true;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to include input channels in a map.
        /// </summary>
        public bool IncludeInCnls { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include output channels in a map.
        /// </summary>
        public bool IncludeOutCnls { get; set; }


        private void FrmCnlMap_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            chkInCnls.Checked = IncludeInCnls;
            chkOutCnls.Checked = IncludeOutCnls;
        }

        private void chkCnls_CheckedChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = chkInCnls.Checked || chkOutCnls.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            new CnlMap(configBase, appData)
            {
                IncludeInCnls = chkInCnls.Checked,
                IncludeOutCnls = chkOutCnls.Checked,
                GroupByDevices = rbGroupByDevices.Checked
            }.Generate();

            IncludeInCnls = chkInCnls.Checked;
            IncludeOutCnls = chkOutCnls.Checked;
            DialogResult = DialogResult.OK;
        }
    }
}
