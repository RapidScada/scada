/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : KpEmail
 * Summary  : Device properties form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Comm.Devices.AB;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpEmail
{
    /// <summary>
    /// Device properties form
    /// <para>Форма настройки свойств КП</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private AppDirs appDirs;       // директории приложения
        private int kpNum;             // номер настраиваемого КП
        private KpConfig config;         // конфигурация КП
        private string configFileName; // имя файла конфигурации КП


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();

            appDirs = null;
            kpNum = 0;
            config = new KpConfig();
            configFileName = "";
        }


        /// <summary>
        /// Установить элементы управления в соответствии с конфигурацией
        /// </summary>
        private void ConfigToControls()
        {
            txtHost.Text = config.Host;
            numPort.SetValue(config.Port);
            txtUser.Text = config.User;
            txtPassword.Text = config.Password;
            txtUserDisplayName.Text = config.UserDisplayName;
            chkEnableSsl.Checked = config.EnableSsl;
        }

        /// <summary>
        /// Перенести значения элементов управления в конфигурацию
        /// </summary>
        private void ControlsToConfig()
        {
            config.Host = txtHost.Text;
            config.Port = Convert.ToInt32(numPort.Value);
            config.User = txtUser.Text;
            config.Password = txtPassword.Text;
            config.UserDisplayName = txtUserDisplayName.Text;
            config.EnableSsl = chkEnableSsl.Checked;
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(AppDirs appDirs, int kpNum)
        {
            if (appDirs == null)
                throw new ArgumentNullException("appDirs");

            FrmConfig frmConfig = new FrmConfig();
            frmConfig.appDirs = appDirs;
            frmConfig.kpNum = kpNum;
            frmConfig.ShowDialog();
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            // локализация модуля
            string errMsg;
            if (!Localization.UseRussian)
            {
                if (Localization.LoadDictionaries(appDirs.LangDir, "KpEmail", out errMsg))
                    Translator.TranslateForm(this, "Scada.Comm.Devices.KpEmail.FrmConfig");
                else
                    ScadaUiUtils.ShowError(errMsg);
            }

            // вывод заголовка
            Text = string.Format(Text, kpNum);

            // загрузка конфигурации КП
            configFileName = KpConfig.GetFileName(appDirs.ConfigDir, kpNum);
            if (File.Exists(configFileName) && !config.Load(configFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // вывод конфигурации КП
            ConfigToControls();
        }

        private void btnEditAddressBook_Click(object sender, EventArgs e)
        {
            // отображение адресной книги
            FrmAddressBook.ShowDialog(appDirs);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // получение изменений конфигурации КП
            ControlsToConfig();

            // сохранение конфигурации КП
            string errMsg;
            if (config.Save(configFileName, out errMsg))
                DialogResult = DialogResult.OK;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
