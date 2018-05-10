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
 * Summary  : Import configuration table form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2018
 */

using Scada;
using Scada.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ScadaAdmin
{
    /// <summary>
    /// Import configuration table form
    /// <para>Форма импорта таблицы базы конфигурации</para>
    /// </summary>
    public partial class FrmImport : Form
    {
        /// <summary>
        /// Элемент выпадающего списка для иморта всех таблиц
        /// </summary>
        private class ImportAllTablesItem
        {
            public override string ToString()
            {
                return AppPhrases.AllTablesItem;
            }
        }

        /// <summary>
        /// Элемент выпадающего списка для иморта из архива
        /// </summary>
        private class ImportArchiveItem
        {
            public override string ToString()
            {
                return AppPhrases.ArchiveItem;
            }
        }

        /// <summary>
        /// Выбранный элемент при открытии формы
        /// </summary>
        public enum SelectedItem { Table, AllTables, Archive };


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmImport()
        {
            InitializeComponent();

            DefaultSelection = SelectedItem.Table;
            DefaultTableName = "";
            DefaultArcFileName = "";
            DefaultBaseDATDir = "";
        }


        /// <summary>
        /// Выбранный элемент по умолчанию
        /// </summary>
        public SelectedItem DefaultSelection { get; set; }

        /// <summary>
        /// Получить или установить имя таблицы, выбранной по умолчанию
        /// </summary>
        public string DefaultTableName { get; set; }

        /// <summary>
        /// Получить или установить имя файла архива конфигурации по умолчанию
        /// </summary>
        public string DefaultArcFileName { get; set; }

        /// <summary>
        /// Получить или установить директорию по умолчанию
        /// </summary>
        public string DefaultBaseDATDir { get; set; }


        private void FrmImport_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmImport");

            // настройка элементов управления
            lblDirectory.Left = lblFileName.Left;

            // определение имени файла архива по умолчанию
            if (string.IsNullOrEmpty(DefaultArcFileName) && !string.IsNullOrEmpty(DefaultBaseDATDir))
                DefaultArcFileName = Path.GetFullPath(DefaultBaseDATDir + @"..\config.zip");

            // заполнение выпадающего списка таблиц
            int tableInd = 0;

            foreach (Tables.TableInfo tableInfo in Tables.TableInfoList)
            {
                int ind = cbTable.Items.Add(tableInfo);
                if (tableInfo.Name == DefaultTableName)
                    tableInd = ind;
            }

            int allTablesInd = cbTable.Items.Add(new ImportAllTablesItem());
            int archiveInd = cbTable.Items.Add(new ImportArchiveItem());

            // выбор элемента списка
            switch (DefaultSelection)
            {
                case SelectedItem.AllTables:
                    cbTable.SelectedIndex = allTablesInd;
                    break;
                case SelectedItem.Archive:
                    cbTable.SelectedIndex = archiveInd;
                    break;
                default: // SelectedItem.Table
                    cbTable.SelectedIndex = tableInd;
                    break;
            }
        }

        private void cbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // установка имени файла таблицы
            object selItem = cbTable.SelectedItem;
            
            if (selItem is ImportAllTablesItem)
            {
                lblFileName.Visible = false;
                lblDirectory.Visible = true;
                txtFileName.Text = DefaultBaseDATDir;
                gbIDs.Enabled = false;
            }
            else if (selItem is ImportArchiveItem)
            {
                lblFileName.Visible = true;
                lblDirectory.Visible = false;
                txtFileName.Text = DefaultArcFileName;
                gbIDs.Enabled = false;
            }
            else if (selItem is Tables.TableInfo tableInfo)
            {
                lblFileName.Visible = true;
                lblDirectory.Visible = false;
                txtFileName.Text = DefaultBaseDATDir + tableInfo.FileName;
                gbIDs.Enabled = tableInfo.HasIntID;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (lblFileName.Visible)
            {
                // выбор импортируемого файла
                if (cbTable.SelectedItem is ImportArchiveItem)
                {
                    openFileDialog.Title = AppPhrases.ChooseArchiveFile;
                    openFileDialog.Filter = AppPhrases.ArchiveFileFilter;
                }
                else
                {
                    openFileDialog.Title = AppPhrases.ChooseBaseTableFile;
                    openFileDialog.Filter = AppPhrases.BaseTableFileFilter;
                }

                string fileName = txtFileName.Text.Trim();
                openFileDialog.FileName = fileName;

                if (fileName != "")
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(fileName);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    txtFileName.Text = openFileDialog.FileName;

                txtFileName.Focus();
                txtFileName.DeselectAll();
            }
            else
            {
                // выбор директории
                folderBrowserDialog.SelectedPath = txtFileName.Text.Trim();
                folderBrowserDialog.Description = CommonPhrases.ChooseBaseDATDir;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    txtFileName.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);

                txtFileName.Focus();
                txtFileName.DeselectAll();
            }
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
            if (AppData.Connected)
            {
                object selItem = cbTable.SelectedItem;
                string logFileName = AppData.AppDirs.LogDir + "ScadaAdminImport.txt";
                bool importOK;
                bool logCreated;
                string msg;

                if (selItem is ImportAllTablesItem)
                {
                    // импорт всех таблиц из директории
                    importOK = ImportExport.ImportAllTables(txtFileName.Text, Tables.TableInfoList,
                        logFileName, out logCreated, out msg);
                }
                else if (selItem is ImportArchiveItem)
                {
                    // импорт архива
                    importOK = ImportExport.ImportArchive(txtFileName.Text, Tables.TableInfoList, 
                        logFileName, out logCreated, out msg);
                }
                else
                {
                    // импорт таблицы
                    Tables.TableInfo tableInfo = (Tables.TableInfo)selItem;
                    int minID = gbIDs.Enabled && chkStartID.Checked ? Convert.ToInt32(numStartID.Value) : 0;
                    int maxID = gbIDs.Enabled && chkFinalID.Checked ? Convert.ToInt32(numFinalID.Value) : int.MaxValue;
                    int newMinID = gbIDs.Enabled && chkNewStartID.Checked ? Convert.ToInt32(numNewStartID.Value) : 0;
                    importOK = ImportExport.ImportTable(txtFileName.Text, tableInfo, minID, maxID, newMinID,
                        logFileName, out logCreated, out msg);
                }

                // отображение сообщения о результате
                if (importOK)
                {
                    ScadaUiUtils.ShowInfo(msg);
                }
                else
                {
                    AppUtils.ProcError(msg);

                    // отображение журнала в блокноте
                    if (logCreated)
                        Process.Start(logFileName);
                }
            }
        }
    }
}