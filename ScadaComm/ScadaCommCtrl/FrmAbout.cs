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
 * Module   : SCADA-Communicator Control
 * Summary  : About form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2014
 */

using System;
using System.Windows.Forms;

namespace Scada.Comm.Ctrl
{
    /// <summary>
    /// About form
    /// <para>Форма о программе</para>
    /// </summary>
    public partial class FrmAbout : Form
    {
        private static FrmAbout frmAbout = null; // форма о программе
        private static string link = "";         // гиперссылка

        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmAbout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отобразить форму о программе, загрузив заставку и гиперссылку из файлов
        /// </summary>
        public static bool ShowAbout(string exeDir, out string errMsg)
        {
            if (frmAbout == null)
            {
                frmAbout = new FrmAbout();

                if (ScadaUtils.LoadAboutForm(exeDir + "About.jpg", exeDir + "About.txt",
                    frmAbout, frmAbout.pictureBox, frmAbout.lblLink, out link, out errMsg))
                {
                    frmAbout.ShowDialog();
                    return true;
                }
                else
                {
                    frmAbout = null;
                    return false;
                }
            }
            else
            {
                frmAbout.ShowDialog();
                errMsg = "";
                return true;
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
            if (!string.IsNullOrEmpty(link))
            {
                System.Diagnostics.Process.Start(link);
                Close();
            }
        }
    }
}