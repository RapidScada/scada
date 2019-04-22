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
 * Summary  : Form for export the configuration database table
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
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for export the configuration database table.
    /// <para>Форма экспорта таблицы базы конфигурации.</para>
    /// </summary>
    public partial class FrmTableExport : Form
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
        private FrmTableExport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableExport(ConfigBase configBase, AppData appData)
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
        /// Gets the output file name.
        /// </summary>
        private string GetOutputFileName(IBaseTable baseTable, out BaseTableFormat format)
        {
            switch (cbFormat.SelectedIndex)
            {
                case 0:
                    format = BaseTableFormat.DAT;
                    return baseTable.Name.ToLowerInvariant() + ".dat";
                case 1:
                    format = BaseTableFormat.XML;
                    return baseTable.Name + ".xml";
                default:
                    format = BaseTableFormat.CSV;
                    return baseTable.Name + ".csv";
            }
        }

        /// <summary>
        /// Exports the table.
        /// </summary>
        private bool Export(IBaseTable baseTable, BaseTableFormat format)
        {
            try
            {
                new ImportExport().ExportBaseTable(saveFileDialog.FileName, format, baseTable,
                    chkStartID.Checked ? Convert.ToInt32(numStartID.Value) : 0, 
                    chkEndID.Checked ? Convert.ToInt32(numEndID.Value) : int.MaxValue);
                return true;
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, string.Format(AdminPhrases.ExportBaseTableError, baseTable.Name));
                return false;
            }
        }


        private void FrmTableExport_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            saveFileDialog.SetFilter(AppPhrases.ExportTableFilter);

            FillTableList();
            cbFormat.SelectedIndex = 0;
        }

        private void chkStartID_CheckedChanged(object sender, EventArgs e)
        {
            numStartID.Enabled = chkStartID.Checked;
        }

        private void chkEndID_CheckedChanged(object sender, EventArgs e)
        {
            numEndID.Enabled = chkEndID.Checked;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (cbTable.SelectedItem is TableItem tableItem)
            {
                SelectedItemType = tableItem.BaseTable.ItemType;
                saveFileDialog.FileName = GetOutputFileName(tableItem.BaseTable, out BaseTableFormat format);

                if (saveFileDialog.ShowDialog() == DialogResult.OK &&
                    Export(tableItem.BaseTable, format))
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
