/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : SCADA-Administrator
 * Summary  : About form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2010
 */

using System;
using System.Windows.Forms;
using Scada;

namespace ScadaAdmin
{
    /// <summary>
    /// About form
    /// <para>Форма о программе</para>
    /// </summary>
    public partial class FrmAbout : Form
    {
        private static FrmAbout frmAbout = null; // форма о программе
        private static string link;              // гиперссылка


        public FrmAbout()
        {
            InitializeComponent();
            link = "";
        }


        /// <summary>
        /// Отобразить форму о программе, загрузив заставку и гиперссылку из файлов
        /// </summary>
        public static void ShowAbout()
        {
            if (frmAbout == null)
            {
                frmAbout = new FrmAbout();
                string errMsg;

                if (!ScadaUtils.LoadAboutForm(AppData.ExeDir + "About.jpg", AppData.ExeDir + "About.txt",
                    frmAbout, frmAbout.pictureBox, frmAbout.lblLink, out link, out errMsg))
                {
                    frmAbout = null;
                    AppData.ErrLog.WriteAction(errMsg);
                    ScadaUtils.ShowError(errMsg);
                }
            }

            if (frmAbout != null)
                frmAbout.ShowDialog();
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
            if (!string.IsNullOrEmpty(link))
            {
                System.Diagnostics.Process.Start(link);
                Close();
            }
        }
    }
}
