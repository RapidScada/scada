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
 * Summary  : Form for editing a table of the configuration database.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for editing a table of the configuration database.
    /// <para>Форма редактирования таблицы базы конфигурации.</para>
    /// </summary>
    public partial class FrmBaseTable : Form
    {
        protected readonly AppData appData; // the common data of the application


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmBaseTable()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected FrmBaseTable(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
        }


        /// <summary>
        /// Gets the source data table.
        /// </summary>
        private DataTable SourceDataTable
        {
            get
            {
                return (DataTable)bindingSource.DataSource;
            }
        }


        /// <summary>
        /// Loads the table data.
        /// </summary>
        protected virtual void LoadTableData()
        {
        }

        /// <summary>
        /// Deletes the selected rows.
        /// </summary>
        protected virtual void DeleteSelectedRows()
        {
        }

        /// <summary>
        /// Clears the table data.
        /// </summary>
        protected virtual void ClearTableData()
        {
        }

        /// <summary>
        /// Shows error message in the error panel.
        /// </summary>
        protected void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
        }

        /// <summary>
        /// Hides the error panel.
        /// </summary>
        protected void HideError()
        {
            pnlError.Visible = false;
        }

        /// <summary>
        /// Validates a cell after editing.
        /// </summary>
        protected bool ValidateCell(int colInd, int rowInd, string cellVal, out string errMsg)
        {
            errMsg = "";

            if (0 <= colInd && colInd < dataGridView.ColumnCount && 0 <= rowInd && rowInd < dataGridView.RowCount)
            {
                DataGridViewColumn col = dataGridView.Columns[colInd];

                if (col is DataGridViewTextBoxColumn)
                {
                    if (!string.IsNullOrEmpty(cellVal))
                    {
                        Type valueType = col.ValueType;

                        if (valueType == typeof(int))
                        {
                            if (!int.TryParse(cellVal, out int intVal))
                                errMsg = CommonPhrases.IntegerRequired;
                        }
                        else if (valueType == typeof(double))
                        {
                            if (!double.TryParse(cellVal, out double doubleVal))
                                errMsg = CommonPhrases.RealRequired;
                        }
                    }
                }

                dataGridView.Rows[rowInd].ErrorText = errMsg;
            }

            return string.IsNullOrEmpty(errMsg);
        }

        /// <summary>
        /// Validates a row after editing.
        /// </summary>
        protected bool ValidateRow(int rowInd, out string errMsg)
        {
            errMsg = "";

            if (0 <= rowInd && rowInd < dataGridView.RowCount)
            {
                DataGridViewRow row = dataGridView.Rows[rowInd];

                if (!row.IsNewRow)
                {
                    // check for nulls
                    DataTable dataTable = SourceDataTable;

                    for (int colInd = 0, colCnt = dataGridView.Columns.Count; colInd < colCnt; colInd++)
                    {
                        DataGridViewColumn col = dataGridView.Columns[colInd];
                        object cellVal = row.Cells[colInd].Value;

                        if (cellVal == DBNull.Value && !dataTable.Columns[col.Name].AllowDBNull)
                            errMsg = string.Format(AppPhrases.ColumnNotNull, col.HeaderText);
                    }
                }

                dataGridView.Rows[rowInd].ErrorText = errMsg;
            }

            return string.IsNullOrEmpty(errMsg);
        }

        /// <summary>
        /// Commits and ends the edit operation on the current cell and row.
        /// </summary>
        protected bool EndEdit()
        {
            if (dataGridView.EndEdit())
            {
                if (dataGridView.CurrentRow == null || 
                    ValidateRow(dataGridView.CurrentRow.Index, out string errMsg))
                {
                    bindingSource.EndEdit();
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            return false;
        }


        private void FrmBaseTable_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Admin.App.Forms.FrmBaseTable");

            if (lblCount.Text.Contains("{0}"))
                bindingNavigator.CountItemFormat = lblCount.Text;

            if (ScadaUtils.IsRunningOnMono)
            {
                // because of the bug in Mono 5.12.0.301
                dataGridView.AllowUserToAddRows = false;
            }
        }

        private void FrmBaseTable_Shown(object sender, EventArgs e)
        {
            LoadTableData();
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!ValidateCell(e.ColumnIndex, e.RowIndex, e.FormattedValue?.ToString(), out string errMsg))
            {
                ScadaUiUtils.ShowError(errMsg);
                e.Cancel = true;
            }
        }

        private void dataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!ValidateRow(e.RowIndex, out string errMsg))
            {
                ScadaUiUtils.ShowError(errMsg);
                e.Cancel = true;
            }
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // write and display a error
            string columnName = e.ColumnIndex >= 0 ? dataGridView.Columns[e.ColumnIndex].HeaderText : "";
            string columnPhrase = e.ColumnIndex >= 0 ? Environment.NewLine + AppPhrases.ColumnLabel + columnName : "";

            appData.ErrLog.WriteException(e.Exception, string.Format(AppPhrases.GridViewError, columnName));
            ShowError(e.Exception.Message + columnPhrase);
            e.ThrowException = false;
        }

        private void btnCloseError_Click(object sender, EventArgs e)
        {
            HideError();
        }

        private void btnApplyEdit_Click(object sender, EventArgs e)
        {
            EndEdit();
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.CancelEdit())
                bindingSource.CancelEdit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // refresh data of the table and the combo box columns
            if (EndEdit())
                LoadTableData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    dataGridView.SelectedRows.Count > 1 ? AppPhrases.DeleteRowsConfirm : AppPhrases.DeleteRowConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == 
                    DialogResult.Yes)
            {
                DeleteSelectedRows();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppPhrases.ClearTableConfirm, CommonPhrases.QuestionCaption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearTableData();
            }
        }

        private void btnAutoSizeColumns_Click(object sender, EventArgs e)
        {
            dataGridView.AutoSizeColumns();
        }
    }
}
