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
 * Module   : SCADA-Administrator
 * Summary  : About form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2018
 */

using Scada;
using Scada.UI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ScadaAdmin
{
    /// <summary>
    /// About form
    /// <para>Форма о программе</para>
    /// </summary>
    public partial class FrmAbout : Form
    {
        private const string Version = "5.1.1.0"; // версия приложения
        private static FrmAbout frmAbout = null;  // экземпляр формы о программе
        
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
        public static void ShowAbout()
        {
            if (frmAbout == null)
                frmAbout = new FrmAbout();
            frmAbout.Init();
            frmAbout.ShowDialog();
        }


        /// <summary>
        /// Инициализировать форму
        /// </summary>
        private void Init()
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
                if (ScadaUiUtils.LoadAboutForm(AppData.AppDirs.ExeDir, this, activePictureBox, lblWebsite,
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
                    AppUtils.ProcError(errMsg);
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
