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
 * Summary  : Channel creation wizard
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Channel creation wizard.
    /// <para>Мастер создания каналов.</para>
    /// </summary>
    public partial class FrmCnlCreate : Form
    {
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected objects

        private int step; // the current step of the wizard


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCnlCreate()
        {
            InitializeComponent();
            step = 1;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlCreate(ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.recentSelection = recentSelection ?? throw new ArgumentNullException("recentSelection");
        }


        /// <summary>
        /// Applies the wizard step.
        /// </summary>
        private void ApplyStep(int offset)
        {
            step += offset;

            if (step < 1)
                step = 1;
            else if (step > 3)
                step = 3;

            switch (step)
            {
                case 1:
                    lblStep.Text = AppPhrases.CreateCnlsStep1;
                    ctrlCnlCreate1.Visible = true;
                    ctrlCnlCreate2.Visible = false;
                    ctrlCnlCreate3.Visible = false;
                    btnCnlMap.Visible = false;
                    btnBack.Visible = false;
                    btnNext.Visible = true;
                    btnCreate.Visible = false;

                    ctrlCnlCreate1.SetFocus();
                    break;
                case 2:
                    lblStep.Text = AppPhrases.CreateCnlsStep2;
                    ctrlCnlCreate1.Visible = false;
                    ctrlCnlCreate2.Visible = true;
                    ctrlCnlCreate3.Visible = false;
                    btnCnlMap.Visible = false;
                    btnBack.Visible = true;
                    btnNext.Visible = true;
                    btnCreate.Visible = false;

                    ctrlCnlCreate2.DeviceName = ctrlCnlCreate1.SelectedDevice?.Name;
                    ctrlCnlCreate2.SetFocus();
                    break;
                case 3:
                    lblStep.Text = AppPhrases.CreateCnlsStep3;
                    ctrlCnlCreate1.Visible = false;
                    ctrlCnlCreate2.Visible = false;
                    ctrlCnlCreate3.Visible = true;
                    btnCnlMap.Visible = true;
                    btnBack.Visible = true;
                    btnNext.Visible = false;
                    btnCreate.Visible = true;

                    ctrlCnlCreate3.DeviceName = ctrlCnlCreate1.SelectedDevice?.Name;
                    ctrlCnlCreate3.SetFocus();
                    break;
            }
        }


        private void FrmCnlCreate_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            ctrlCnlCreate1.Init(project, recentSelection);
            ctrlCnlCreate2.Init(project, recentSelection);
            ApplyStep(0);
        }

        private void btnCnlMap_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ApplyStep(-1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ApplyStep(1);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }
    }
}
