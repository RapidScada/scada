/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Modified : 2020
 */

using Scada.Comm.Devices.AB;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpEmail
{
    /// <summary>
    /// Device properties form.
    /// <para>Форма настройки свойств КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private readonly AppDirs appDirs; // директории приложения
        private readonly int kpNum;       // номер настраиваемого КП
        private readonly KpConfig config; // конфигурация КП
        private string configFileName;    // имя файла конфигурации КП
        private string prevUser;          // the previous user name


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig(AppDirs appDirs, int kpNum)
            : this()
        {
            config = new KpConfig();
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.kpNum = kpNum;
            configFileName = "";
            prevUser = "";
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
            chkEnableSsl.Checked = config.EnableSsl;
            txtSenderAddress.Text = config.SenderAddress;
            txtSenderDisplayName.Text = config.SenderDisplayName;
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
            config.EnableSsl = chkEnableSsl.Checked;
            config.SenderAddress = txtSenderAddress.Text;
            config.SenderDisplayName = txtSenderDisplayName.Text;
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(AppDirs appDirs, int kpNum)
        {
            new FrmConfig(appDirs, kpNum).ShowDialog();
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            // translate the form
            if (Localization.LoadDictionaries(appDirs.LangDir, "KpEmail", out string errMsg))
                Translator.TranslateForm(this, GetType().FullName);
            else
                ScadaUiUtils.ShowError(errMsg);

            Text = string.Format(Text, kpNum);

            // load a configuration
            configFileName = KpConfig.GetFileName(appDirs.ConfigDir, kpNum);

            if (File.Exists(configFileName) && !config.Load(configFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // display the configuration
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
            if (config.Save(configFileName, out string errMsg))
                DialogResult = DialogResult.OK;
            else
                ScadaUiUtils.ShowError(errMsg);
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSenderAddress.Text) || txtSenderAddress.Text == prevUser)
                txtSenderAddress.Text = txtUser.Text;

            prevUser = txtUser.Text;
        }
    }
}
