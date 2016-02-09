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
 * Summary  : Export configuration table form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2016
 */

using Scada;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace ScadaAdmin
{
    /// <summary>
    /// Export configuration table form
    /// <para>Форма экспорта таблицы конфигурации</para>
    /// </summary>
    public partial class FrmExport : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmExport()
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


        private void FrmExport_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmExport");
            openFileDialog.Title = AppPhrases.ChooseBaseTableFile;
            openFileDialog.Filter = AppPhrases.BaseTableFileFilter;

            // заполнение выпадающего списка таблиц
            int selInd = 0;

            foreach (Tables.TableInfo tableInfo in Tables.TableInfoList)
            {
                int ind = cbTable.Items.Add(tableInfo);
                if (tableInfo.Name == DefaultTableName)
                    selInd = ind;
            }

            if (cbTable.Items.Count > 0)
                cbTable.SelectedIndex = selInd;
        }

        private void cbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // установка имени файла таблицы
            Tables.TableInfo tableInfo = cbTable.SelectedItem as Tables.TableInfo;
            if (tableInfo != null)
            {
                txtFileName.Text = DefaultDirectory + tableInfo.FileName;
                gbIDs.Enabled = tableInfo.IDColName != "";
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // выбор файла таблицы
            string fileName = txtFileName.Text.Trim();
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            // экспорт выбранной таблицы в формат DAT
            Tables.TableInfo tableInfo = cbTable.SelectedItem as Tables.TableInfo;

            if (tableInfo != null && AppData.Connected)
            {
                int minID = gbIDs.Enabled && chkStartID.Checked ? Convert.ToInt32(numStartID.Value) : 0;
                int maxID = gbIDs.Enabled && chkFinalID.Checked ? Convert.ToInt32(numFinalID.Value) : int.MaxValue;
                string msg;
                if (ImportExport.ExportTable(tableInfo, txtFileName.Text, minID, maxID, out msg))
                    ScadaUiUtils.ShowInfo(msg);
                else
                    AppUtils.ProcError(msg);
            }
        }
    }
}