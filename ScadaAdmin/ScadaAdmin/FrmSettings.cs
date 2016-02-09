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
 * Module   : SCADA-Administrator
 * Summary  : Application settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2016
 */

using Scada;
using Scada.UI;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ScadaAdmin
{
    /// <summary>
    /// Application settings form
    /// <para>Форма настройки приложения</para>
    /// </summary>
    public partial class FrmSettings : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmSettings()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Установить элементы управления в соответствии с параметрами приложения
        /// </summary>
        public void ParamsToControls(Settings.AppSettings appSettings)
        {
            txtBaseSDFFile.Text = appSettings.BaseSDFFile;
            txtBaseDATDir.Text = appSettings.BaseDATDir;
            txtBackupDir.Text = appSettings.BackupDir;
            txtCommDir.Text = appSettings.CommDir;
            chkAutoBackupBase.Checked = appSettings.AutoBackupBase;
        }

        /// <summary>
        /// Установить параметры приложения в соответствии с элементами управления
        /// </summary>
        public void ControlsToParams(Settings.AppSettings appSettings)
        {
            appSettings.BaseSDFFile = txtBaseSDFFile.Text;
            appSettings.BaseDATDir = txtBaseDATDir.Text;
            appSettings.BackupDir = txtBackupDir.Text;
            appSettings.CommDir = txtCommDir.Text;
            appSettings.AutoBackupBase = chkAutoBackupBase.Checked;
        }


        private void FrmSettings_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmSettings");
            openFileDialog.Title = AppPhrases.ChooseBaseSDFFile;
            openFileDialog.Filter = AppPhrases.BaseSDFFileFilter;
        }

        private void FrmSettings_Shown(object sender, EventArgs e)
        {
            txtBaseSDFFile.Focus();
            txtBaseSDFFile.DeselectAll();
        }

        private void btnBaseSDFFile_Click(object sender, EventArgs e)
        {
            string fileName = txtBaseSDFFile.Text.Trim();
            openFileDialog.FileName = fileName == "" ? "ScadaBase.sdf" : fileName;
            if (fileName != "")
                openFileDialog.InitialDirectory = Path.GetDirectoryName(fileName);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtBaseSDFFile.Text = openFileDialog.FileName;
            txtBaseSDFFile.Focus();
            txtBaseSDFFile.DeselectAll();
        }

        private void btnBaseDATDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtBaseDATDir.Text.Trim();
            folderBrowserDialog.Description = CommonPhrases.ChooseBaseDATDir;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtBaseDATDir.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);
            txtBaseDATDir.Focus();
            txtBaseDATDir.DeselectAll();
        }

        private void btnBackupDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtBackupDir.Text.Trim();
            folderBrowserDialog.Description = AppPhrases.ChooseBackupDir;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtBackupDir.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);
            txtBackupDir.Focus();
            txtBackupDir.DeselectAll();
        }

        private void btnCommDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtCommDir.Text.Trim();
            folderBrowserDialog.Description = AppPhrases.ChooseCommDir;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtCommDir.Text = folderBrowserDialog.SelectedPath;
            txtCommDir.Focus();
            txtCommDir.DeselectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // проверка корректности параметров
            StringBuilder sbErr = new StringBuilder();
            bool fatalError = false;

            if (!File.Exists(txtBaseSDFFile.Text))
            {
                sbErr.AppendLine(AppPhrases.BaseSDFFileNotExists);
                fatalError = true;
            }
            if (!Directory.Exists(txtBaseDATDir.Text))
            {
                sbErr.AppendLine(CommonPhrases.BaseDATDirNotExists);
                fatalError = true;
            }
            if (!Directory.Exists(txtBackupDir.Text))
            {
                sbErr.AppendLine(AppPhrases.BackupDirNotExists);
                fatalError = true;
            }
            if (!Directory.Exists(txtCommDir.Text))
                sbErr.AppendLine(AppPhrases.CommDirNotExists);

            if (sbErr.Length > 0)
            {
                string errMsg = sbErr.ToString().TrimEnd();
                if (fatalError)
                    ScadaUiUtils.ShowError(errMsg);
                else
                    ScadaUiUtils.ShowWarning(errMsg);
            }

            if (!fatalError)
                DialogResult = DialogResult.OK;
        }
    }
}
