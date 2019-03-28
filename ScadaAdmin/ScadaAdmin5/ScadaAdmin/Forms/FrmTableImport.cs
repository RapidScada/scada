/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : Administrator
 * Summary  : Form for import the configuration database table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for import the configuration database table.
    /// <para>Форма импорта таблицы базы конфигурации.</para>
    /// </summary>
    public partial class FrmTableImport : Form
    {
        /// <summary>
        /// Item of the table list.
        /// <para>Элемент списка таблиц.</para>
        private class TableItem
        {
            public IBaseTable BaseTable { get; set; }

            public override string ToString()
            {
                return BaseTable.Title;
            }
        }

        private readonly ConfigBase configBase; // the configuration database
        private readonly AppData appData;       // the common data of the application


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTableImport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableImport(ConfigBase configBase, AppData appData)
            : this()
        {
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
            this.appData = appData ?? throw new ArgumentNullException("appData");
            SelectedItemType = null;
        }


        /// <summary>
        /// Gets or sets the item type to select the default table.
        /// </summary>
        public Type SelectedItemType { get; set; }


        /// <summary>
        /// Fills the combo box with the tables.
        /// </summary>
        private void FillTableList()
        {
            try
            {
                cbTable.BeginUpdate();
                int selectedIndex = 0;
                int index = 0;

                foreach (IBaseTable baseTable in configBase.AllTables)
                {
                    if (baseTable.ItemType == SelectedItemType)
                        selectedIndex = index;

                    cbTable.Items.Add(new TableItem { BaseTable = baseTable });
                    index++;
                }

                cbTable.SelectedIndex = selectedIndex;
            }
            finally
            {
                cbTable.EndUpdate();
            }
        }

        /// <summary>
        /// Calculates the end destination ID.
        /// </summary>
        private void CalcDestEndID()
        {
            numDestEndID.SetValue(chkSrcEndID.Checked ? chkDestStartID.Checked ? 
                numSrcEndID.Value - numSrcStartID.Value + numDestStartID.Value : 
                numSrcEndID.Value :
                0);
        }

        /// <summary>
        /// Imports the table.
        /// </summary>
        private bool Import(IBaseTable baseTable, BaseTableFormat format)
        {
            try
            {
                string srcfileName = openFileDialog.FileName;
                int srcStartID = chkSrcStartID.Checked ? Convert.ToInt32(numSrcStartID.Value) : 0;

                if (File.Exists(srcfileName))
                {
                    new ImportExport().ImportBaseTable(srcfileName, format, baseTable,
                        srcStartID,
                        chkSrcEndID.Checked ? Convert.ToInt32(numSrcEndID.Value) : int.MaxValue,
                        chkDestStartID.Checked ? Convert.ToInt32(numDestStartID.Value) : srcStartID,
                        out int affectedRows);
                    ScadaUiUtils.ShowInfo(string.Format(AppPhrases.ImportTableComplete, affectedRows));
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.FileNotFound);
                    return false;
                }
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, string.Format(AdminPhrases.ImportBaseTableError, baseTable.Name));
                return false;
            }
        }


        private void FrmTableImport_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            openFileDialog.SetFilter(AppPhrases.ImportTableFilter);
            FillTableList();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtSrcFile.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(txtSrcFile.Text);
                openFileDialog.FileName = Path.GetFileName(txtSrcFile.Text);
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtSrcFile.Text = openFileDialog.FileName;
        }

        private void chkSrcStartID_CheckedChanged(object sender, EventArgs e)
        {
            numSrcStartID.Enabled = chkSrcStartID.Checked;
            CalcDestEndID();
        }

        private void chkSrcEndID_CheckedChanged(object sender, EventArgs e)
        {
            numSrcEndID.Enabled = chkSrcEndID.Checked;
            CalcDestEndID();
        }

        private void chkDestStartID_CheckedChanged(object sender, EventArgs e)
        {
            numDestStartID.Enabled = chkDestStartID.Checked;
            CalcDestEndID();
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            CalcDestEndID();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (cbTable.SelectedItem is TableItem tableItem)
            {
                SelectedItemType = tableItem.BaseTable.ItemType;
                BaseTableFormat format = 
                    string.Equals(Path.GetExtension(txtSrcFile.Text), ".xml", StringComparison.OrdinalIgnoreCase) ?
                        BaseTableFormat.XML : BaseTableFormat.DAT;

                if (Import(tableItem.BaseTable, format))
                    DialogResult = DialogResult.OK;
            }
        }
    }
}
