/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : SCADA-Scheme Editor
 * Summary  : About form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2015
 */

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Utils;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// About form
    /// <para>Форма о программе</para>
    /// </summary>
    public partial class FrmAbout : Form
    {
        private const string Version = "4.5.0.0"; // версия приложения
        private static FrmAbout frmAbout = null;  // форма о программе

        private string exeDir;  // директория исполняемого файла приложения
        private Log errLog;     // журнал ошибок приложения
        private bool inited;    // форма инициализирована
        private string linkUrl; // гиперссылка


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private FrmAbout()
        {
            InitializeComponent();

            inited = false;
            linkUrl = "";
        }


        /// <summary>
        /// Отобразить форму о программе
        /// </summary>
        public static void ShowAbout(string exeDir, Log errLog)
        {
            if (exeDir == null)
                throw new ArgumentNullException("exeDir");
            if (errLog == null)
                throw new ArgumentNullException("errLog");

            if (frmAbout == null)
            {
                frmAbout = new FrmAbout();
                frmAbout.exeDir = exeDir;
                frmAbout.errLog = errLog;
            }
            frmAbout.ShowDialog();
        }


        private void FrmAbout_Load(object sender, EventArgs e)
        {
            // инициализация формы
            if (!inited)
            {
                inited = true;

                // настройка элементов управления в зависимости от локализации
                PictureBox activePictureBox;

                if (Localization.UseRussian)
                {
                    activePictureBox = pbAboutRu;
                    pbAboutEn.Visible = false;
                    lblVersionEn.Visible = false;
                    lblVersionRu.Text = "Версия " + Version;
                }
                else
                {
                    activePictureBox = pbAboutEn;
                    pbAboutRu.Visible = false;
                    lblVersionRu.Visible = false;
                    lblVersionEn.Text = "Version " + Version;
                }

                // загрузка изображения и гиперссылки из файлов, если они существуют
                bool imgLoaded;
                string errMsg;
                if (ScadaUtils.LoadAboutForm(exeDir, this, activePictureBox, lblWebsite,
                    out imgLoaded, out linkUrl, out errMsg))
                {
                    if (imgLoaded)
                    {
                        lblVersionRu.Visible = false;
                        lblVersionEn.Visible = false;
                    }
                }
                else
                {
                    errLog.WriteAction(errMsg);
                    ScadaUtils.ShowError(errMsg);
                }
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
            if (!string.IsNullOrEmpty(linkUrl))
            {
                Process.Start(linkUrl);
                Close();
            }
        }
    }
}