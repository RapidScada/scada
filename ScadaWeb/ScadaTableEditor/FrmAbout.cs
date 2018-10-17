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
 * Module   : SCADA-Table Editor
 * Summary  : About form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2018
 */

using Scada;
using Scada.UI;
using Scada.Web.Plugins.Table;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ScadaTableEditor
{
    /// <summary>
    /// About form
    /// <para>Форма о программе</para>
    /// </summary>
    public partial class FrmAbout : Form
    {
        private static FrmAbout frmAbout = null;  // форма о программе

        private string exeDir;  // директория исполняемого файла приложения
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
        public static void ShowAbout(string exeDir)
        {
            if (exeDir == null)
                throw new ArgumentNullException("exeDir");

            if (frmAbout == null)
            {
                frmAbout = new FrmAbout();
                frmAbout.exeDir = exeDir;
            }

            frmAbout.Init();
            frmAbout.ShowDialog();
        }


        /// <summary>
        /// Инициализировать форму
        /// </summary>
        private void Init()
        {
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
                    lblVersionRu.Text = "Версия " + TableUtils.TableVersion;
                }
                else
                {
                    activePictureBox = pbAboutEn;
                    pbAboutRu.Visible = false;
                    lblVersionRu.Visible = false;
                    lblVersionEn.Text = "Version " + TableUtils.TableVersion;
                }

                // загрузка изображения и гиперссылки из файлов, если они существуют
                bool imgLoaded;
                string errMsg;
                if (ScadaUiUtils.LoadAboutForm(exeDir, this, activePictureBox, lblWebsite,
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
                    ScadaUiUtils.ShowError(errMsg);
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
            if (ScadaUtils.IsValidUrl(linkUrl))
            {
                Process.Start(linkUrl);
                Close();
            }
        }
    }
}