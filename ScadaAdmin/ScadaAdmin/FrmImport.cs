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
 * Summary  : Import configuration table form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2015
 */

using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Scada;
using Scada.Data;
using Utils;

namespace ScadaAdmin
{
    /// <summary>
    /// Import configuration table form
    /// <para>Форма импорта таблицы базы конфигурации</para>
    /// </summary>
    public partial class FrmImport : Form
    {
        /// <summary>
        /// Имя файла архива базы конфигурации
        /// </summary>
        public const string BaseDATArcFileName = "BaseDAT.zip";

        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmImport()
        {
            InitializeComponent();

            DefaultTableName = "";
            DefaultDirectory = "";
        }


        /// <summary>
        /// Получить или установить имя таблицы, выбранной по умолчанию
        /// </summary>
        public string DefaultTableName { get; set; }

        /// <summary>
        /// Получить или установить директорию по умолчанию
        /// </summary>
        public string DefaultDirectory { get; set; }


        private void FrmImport_Load(object sender, EventArgs e)
        {
            // перевод формы
            Localization.TranslateForm(this, "ScadaAdmin.FrmImport");

            // заполнение выпадающего списка таблиц
            int selInd = 0;

            foreach (Tables.TableInfo tableInfo in Tables.TableInfoList)
            {
                int ind = cbTable.Items.Add(tableInfo);
                if (tableInfo.Name == DefaultTableName)
                    selInd = ind;
            }

            cbTable.Items.Add(AppPhrases.ArchiveItem);

            if (cbTable.Items.Count > 0)
                cbTable.SelectedIndex = selInd;
        }

        private void cbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // установка имени файла таблицы
            Tables.TableInfo tableInfo = cbTable.SelectedItem as Tables.TableInfo;
            
            if (tableInfo == null)
            {
                txtFileName.Text = DefaultDirectory + BaseDATArcFileName;
                gbIDs.Enabled = false;
            }
            else
            {
                txtFileName.Text = DefaultDirectory + tableInfo.FileName;
                gbIDs.Enabled = tableInfo.IDColName != "";
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // настройка диалога открытия файла
            string fileName = txtFileName.Text.Trim();
            if (fileName.EndsWith("zip", StringComparison.OrdinalIgnoreCase))
            {
                openFileDialog.Title = AppPhrases.ChooseBaseArchiveFile;
                openFileDialog.Filter = AppPhrases.BaseArchiveFileFilter;
            }
            else
            {
                openFileDialog.Title = AppPhrases.ChooseBaseTableFile;
                openFileDialog.Filter = AppPhrases.BaseTableFileFilter;
            }

            // выбор файла таблицы
            openFileDialog.FileName = fileName;
            if (fileName != "")
                openFileDialog.InitialDirectory = Path.GetDirectoryName(fileName);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtFileName.Text = openFileDialog.FileName;
            txtFileName.Focus();
            txtFileName.DeselectAll();
        }

        private void chkStartID_CheckedChanged(object sender, EventArgs e)
        {
            numStartID.Enabled = chkStartID.Checked;
        }

        private void chkFinalID_CheckedChanged(object sender, EventArgs e)
        {
            numFinalID.Enabled = chkFinalID.Checked;
        }

        private void chkNewStartID_CheckedChanged(object sender, EventArgs e)
        {
            numNewStartID.Enabled = chkNewStartID.Checked;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // импорт выбранной таблицы из формата DAT
            Tables.TableInfo tableInfo = cbTable.SelectedItem as Tables.TableInfo;

            if (AppData.Connected)
            {
                string logFileName = chkImportLog.Checked ? AppData.ExeDir + "ScadaAdminImport.txt" : "";
                bool importOK;
                bool logCreated;
                string msg;

                if (tableInfo == null)
                {
                    // импорт архива
                    importOK = ImportExport.ImportArchive(txtFileName.Text, Tables.TableInfoList, 
                        logFileName, out logCreated, out msg);
                }
                else
                {
                    // импорт таблицы
                    int minID = gbIDs.Enabled && chkStartID.Checked ? Convert.ToInt32(numStartID.Value) : 0;
                    int maxID = gbIDs.Enabled && chkFinalID.Checked ? Convert.ToInt32(numFinalID.Value) : int.MaxValue;
                    int newMinID = gbIDs.Enabled && chkNewStartID.Checked ? Convert.ToInt32(numNewStartID.Value) : 0;
                    importOK = ImportExport.ImportTable(txtFileName.Text, tableInfo, minID, maxID, newMinID,
                        logFileName, out logCreated, out msg);
                }

                // отображение сообщения о результате импорта
                if (importOK)
                    ScadaUtils.ShowInfo(msg);
                else
                    AppUtils.ProcError(msg);

                // отображение журанала в блокноте
                if (logCreated)
                    Process.Start(logFileName);
            }
        }
    }
}