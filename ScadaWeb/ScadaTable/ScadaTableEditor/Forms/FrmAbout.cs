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
 * Module   : Table Editor
 * Summary  : About form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2019
 */

using Scada.UI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Scada.Table.Editor.Forms
{
    /// <summary>
    /// About form.
    /// <para>Форма о программе.</para>
    /// </summary>
    public partial class FrmAbout : Form
    {
        private static FrmAbout frmAbout = null; // the form instance

        private readonly string exeDir; // the directory of the executable file
        private string linkUrl;         // the hyperlink


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmAbout(string exeDir)
        {
            InitializeComponent();

            this.exeDir = exeDir ?? throw new ArgumentNullException("exeDir");
            linkUrl = "";
        }


        /// <summary>
        /// Shows the form.
        /// </summary>
        public static void ShowAbout(string exeDir)
        {
            if (frmAbout == null)
            {
                frmAbout = new FrmAbout(exeDir);
                frmAbout.Init();
            }

            frmAbout.ShowDialog();
        }

        /// <summary>
        /// Initializes the form.
        /// </summary>
        private void Init()
        {
            // setup the controls depending on localization
            PictureBox activePictureBox;

            if (Localization.UseRussian)
            {
                activePictureBox = pbAboutRu;
                pbAboutEn.Visible = false;
                lblVersionEn.Visible = false;
                lblVersionRu.Text = "Версия " + TableUtils.TableVersion;
            }
            else
            {
                activePictureBox = pbAboutEn;
                pbAboutRu.Visible = false;
                lblVersionRu.Visible = false;
                lblVersionEn.Text = "Version " + TableUtils.TableVersion;
            }

            // load image and hyperlink from files if the files exist
            if (ScadaUiUtils.LoadAboutForm(exeDir, this, activePictureBox, lblWebsite,
                out bool imgLoaded, out linkUrl, out string errMsg))
            {
                if (imgLoaded)
                {
                    lblVersionRu.Visible = false;
                    lblVersionEn.Visible = false;
                }
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
            }
        }


        private void FrmAbout_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmAbout_KeyPress(object sender, KeyPressEventArgs e)
        {
            Close();
        }

        private void lblLink_Click(object sender, EventArgs e)
        {
            if (ScadaUtils.IsValidUrl(linkUrl))
            {
                Process.Start(linkUrl);
                Close();
            }
        }
    }
}
