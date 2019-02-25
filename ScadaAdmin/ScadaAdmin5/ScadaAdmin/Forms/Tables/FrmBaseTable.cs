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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Form for editing a table of the configuration database.
    /// <para>Форма редактирования таблицы базы конфигурации.</para>
    /// </summary>
    public partial class FrmBaseTable : Form
    {
        /// <summary>
        /// Represents a buffer for copying cells.
        /// <para>Представляет буфер для копирования ячеек.</para>
        /// </summary>
        [Serializable]
        private class CellBuffer
        {
            public string ColumnName { get; set; }
            public object CellValue { get; set; }
        }


        /// <summary>
        /// The string to display instead of password.
        /// </summary>
        private const string DisplayPassword = "●●●●●";
        /// <summary>
        /// The format for copying cell values to the clipboard.
        /// </summary>
        private static string ClipboardFormat = typeof(FrmBaseTable).FullName;

        protected readonly AppData appData; // the common data of the application
        private FrmFind frmFind;            // the find and replace form


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
            frmFind = null;
        }


        /// <summary>
        /// Gets the directory of the interface files.
        /// </summary>
        protected virtual string InterfaceDir
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Gets a value indicating whether an item properties form is available.
        /// </summary>
        protected virtual bool ProperiesAvailable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the source data table.
        /// </summary>
        protected DataTable SourceDataTable
        {
            get
            {
                return (DataTable)bindingSource.DataSource ?? throw new ScadaException("Source data table is null.");
            }
        }

        /// <summary>
        /// Gets the data grid view control.
        /// </summary>
        public DataGridView DataGridView
        {
            get
            {
                return dataGridView;
            }
        }


        /// <summary>
        /// Validates a cell after editing.
        /// </summary>
        private bool ValidateCell(int colInd, int rowInd, string cellVal, out string errMsg)
        {
            errMsg = "";

            if (0 <= colInd && colInd < dataGridView.ColumnCount && 
                0 <= rowInd && rowInd < dataGridView.RowCount)
            {
                DataGridViewColumn col = dataGridView.Columns[colInd];

                if (col is DataGridViewTextBoxColumn)
                {
                    if (!string.IsNullOrEmpty(cellVal))
                    {
                        Type valueType = col.ValueType;

                        if (valueType == typeof(int))
                        {
                            if (int.TryParse(cellVal, out int intVal))
                            {
                                // check primary key range
                                if (col.Tag is ColumnOptions options && options.Kind == ColumnKind.PrimaryKey &&
                                    (intVal < options.Minimum || intVal > options.Maximum))
                                {
                                    errMsg = string.Format(CommonPhrases.IntegerRangingRequired,
                                        options.Minimum, options.Maximum);
                                }
                            }
                            else
                            {
                                errMsg = CommonPhrases.IntegerRequired;
                            }
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
        private bool ValidateRow(int rowInd, out string errMsg)
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
        /// Cancels edit mode.
        /// </summary>
        private void CancelEdit()
        {
            if (dataGridView.CancelEdit())
                bindingSource.CancelEdit();
        }

        /// <summary>
        /// Hides the error panel.
        /// </summary>
        private void HideError()
        {
            pnlError.Visible = false;
        }

        /// <summary>
        /// Convers string to color.
        /// </summary>
        private Color StrToColor(string s)
        {
            try
            {
                if (s.Length == 7 && s[0] == '#')
                {
                    int r = int.Parse(s.Substring(1, 2), NumberStyles.HexNumber);
                    int g = int.Parse(s.Substring(3, 2), NumberStyles.HexNumber);
                    int b = int.Parse(s.Substring(5, 2), NumberStyles.HexNumber);
                    return Color.FromArgb(r, g, b);
                }
                else
                {
                    return Color.FromName(s);
                }
            }
            catch
            {
                return Color.Black;
            }
        }

        /// <summary>
        /// Executes an action on cell button click.
        /// </summary>
        private bool ExecCellAction(ColumnOptions dataColumnOptions, ColumnOptions buttonColumnOptions, ref object cellValue)
        {
            ColumnKind dataKind = dataColumnOptions.Kind;
            ColumnKind buttonKind = buttonColumnOptions == null ? ColumnKind.Button : buttonColumnOptions.Kind;

            if (dataKind == ColumnKind.Color)
            {
                // select color
                FrmColorSelect frmColorSelect = new FrmColorSelect
                {
                    SelectedColor = cellValue == null ? Color.Empty : StrToColor(cellValue.ToString())
                };

                if (frmColorSelect.ShowDialog() == DialogResult.OK)
                {
                    cellValue = frmColorSelect.SelectedColor.Name;
                    return true;
                }
            }
            else if (dataKind == ColumnKind.Path)
            {
                // select file or folder
                string path = "";
                string selectedPath = Path.Combine(InterfaceDir, cellValue == null ? "" : cellValue.ToString());

                if (buttonKind == ColumnKind.SelectFolderButton)
                {
                    // select folder
                    selectedPath = Path.GetDirectoryName(selectedPath);
                    folderBrowserDialog.SelectedPath = Directory.Exists(selectedPath) ? selectedPath : InterfaceDir;

                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                        path = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);
                }
                else
                {
                    // select file
                    if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                    {
                        openFileDialog.InitialDirectory = InterfaceDir;
                        openFileDialog.FileName = "";
                    }
                    else
                    {
                        openFileDialog.InitialDirectory = Path.GetDirectoryName(selectedPath);
                        openFileDialog.FileName = Path.GetFileName(selectedPath);
                    }

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        openFileDialog.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                        path = openFileDialog.FileName;
                    }
                }

                if (!string.IsNullOrEmpty(path))
                {
                    string interfaceDir = ScadaUtils.NormalDir(InterfaceDir);
                    if (path.StartsWith(interfaceDir, StringComparison.OrdinalIgnoreCase))
                        path = path.Substring(interfaceDir.Length);

                    cellValue = path;
                    return true;
                }
            }
            else if (dataKind == ColumnKind.SourceCode)
            {
                // edit source code
                FrmSourceCode frmSourceCode = new FrmSourceCode
                {
                    MaxLength = dataColumnOptions.MaxLength,
                    SourceCode = cellValue.ToString()
                };

                if (frmSourceCode.ShowDialog() == DialogResult.OK)
                {
                    cellValue = frmSourceCode.SourceCode;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies or cuts the current cell value.
        /// </summary>
        private void CopyCell(bool cut)
        {
            DataGridViewCell cell = dataGridView.CurrentCell;

            if (cell != null)
            {
                DataGridViewColumn col = dataGridView.Columns[cell.ColumnIndex];

                if (col is DataGridViewTextBoxColumn)
                {
                    if (cell.IsInEditMode)
                    {
                        if (dataGridView.EditingControl is TextBox textBox)
                            textBox.Cut();
                    }
                    else
                    {
                        string valStr = cell.Value?.ToString();

                        if (string.IsNullOrEmpty(valStr))
                            Clipboard.Clear();
                        else
                            Clipboard.SetText(valStr);

                        if (cut)
                            cell.Value = cell.ValueType == typeof(string) ? "" : (object)DBNull.Value;
                    }
                }
                else if (col is DataGridViewComboBoxColumn)
                {
                    Clipboard.SetData(ClipboardFormat, new CellBuffer { ColumnName = col.Name, CellValue = cell.Value });

                    if (cut)
                        cell.Value = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Pastes the current cell value.
        /// </summary>
        private void PasteCell()
        {
            DataGridViewCell cell = dataGridView.CurrentCell;

            if (cell != null)
            {
                DataGridViewColumn col = dataGridView.Columns[cell.ColumnIndex];

                if (col is DataGridViewTextBoxColumn)
                {
                    if (dataGridView.BeginEdit(true))
                    {
                        if (dataGridView.EditingControl is TextBox textBox)
                            textBox.Paste();
                    }
                }
                else if (col is DataGridViewComboBoxColumn comboBoxColumn &&
                    comboBoxColumn.DataSource is DataTable table &&
                    Clipboard.GetData(ClipboardFormat) is CellBuffer cellBuffer &&
                    cellBuffer.ColumnName == col.Name)
                {
                    string sortBuf = table.DefaultView.Sort;

                    try
                    {
                        table.DefaultView.Sort = comboBoxColumn.ValueMember;
                        if (table.DefaultView.Find(cellBuffer.CellValue) >= 0)
                            cell.Value = cellBuffer.CellValue;
                    }
                    finally
                    {
                        table.DefaultView.Sort = sortBuf;
                    }
                }
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
        /// Creates a form to edit item properties.
        /// </summary>
        protected virtual Form CreatePropertiesForm()
        {
            return null;
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
        /// Commits and ends the edit operation on the current cell and row.
        /// </summary>
        public bool EndEdit()
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

        /// <summary>
        /// Prepares to close all forms.
        /// </summary>
        public void PrepareToClose()
        {
            if (!EndEdit())
                CancelEdit();
        }


        private void FrmBaseTable_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, typeof(FrmBaseTable).FullName);

            if (lblCount.Text.Contains("{0}"))
                bindingNavigator.CountItemFormat = lblCount.Text;

            btnProperties.Visible = ProperiesAvailable;

            if (ScadaUtils.IsRunningOnMono)
            {
                // because of the bug in Mono 5.12.0.301
                dataGridView.AllowUserToAddRows = false;
            }
            else
            {
                btnAddNew.Visible = false;
            }
        }

        private void FrmBaseTable_Shown(object sender, EventArgs e)
        {
            LoadTableData();
        }

        private void FrmBaseTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        CopyCell(true);
                        e.Handled = true;
                        break;
                    case Keys.C:
                        CopyCell(false);
                        e.Handled = true;
                        break;
                    case Keys.V:
                        PasteCell();
                        e.Handled = true;
                        break;
                    case Keys.F:
                        btnFind_Click(null, null);
                        e.Handled = true;
                        break;
                }
            }
        }

        private void FrmBaseTable_VisibleChanged(object sender, EventArgs e)
        {
            // close the find and replace form
            if (frmFind != null)
            {
                frmFind.Close();
                frmFind = null;
            }
        }


        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int colInd = e.ColumnIndex;
            string valStr = e.Value?.ToString();

            if (0 <= colInd && colInd < dataGridView.ColumnCount &&
                dataGridView.Columns[colInd].Tag is ColumnOptions options && !string.IsNullOrEmpty(valStr))
            {
                if (options.Kind == ColumnKind.Password)
                    e.Value = DisplayPassword; // hide actual password
                else if (options.Kind == ColumnKind.Color)
                    e.CellStyle.ForeColor = StrToColor(valStr); // set text color of the cell
            }
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView.CurrentCell != null)
            {
                int colInd = dataGridView.CurrentCell.ColumnIndex;
                int rowInd = dataGridView.CurrentCell.RowIndex;

                if (0 <= rowInd && rowInd < dataGridView.RowCount &&
                    0 <= colInd && colInd < dataGridView.ColumnCount &&
                    dataGridView.Columns[colInd].Tag is ColumnOptions options)
                {
                    // display actual password in edit mode
                    if (options.Kind == ColumnKind.Password)
                    {
                        object cellValue = SourceDataTable.DefaultView[rowInd][colInd];
                        if (e.Control is TextBox textBox && cellValue != null && cellValue != DBNull.Value)
                            textBox.Text = cellValue.ToString();
                    }
                }
            }
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

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int colInd = e.ColumnIndex;
            int rowInd = e.RowIndex;

            if (0 <= rowInd && rowInd < dataGridView.RowCount &&
                0 <= colInd && colInd < dataGridView.ColumnCount &&
                dataGridView.Columns[colInd] is DataGridViewButtonColumn buttonColumn)
            {
                // process a command button
                string dataColumnName = buttonColumn.DataPropertyName;
                DataGridViewColumn dataColumn = dataGridView.Columns[dataColumnName];

                if (dataColumn == null)
                {
                    throw new ScadaException("Data column not found.");
                }
                else if (dataColumn.Tag is ColumnOptions dataColumnOptions)
                {
                    DataGridViewCell dataCell = dataGridView.Rows[rowInd].Cells[dataColumnName];
                    object cellValue = dataCell.Value;

                    if (ExecCellAction(dataColumnOptions, buttonColumn.Tag as ColumnOptions, ref cellValue))
                    {
                        dataCell.Value = cellValue;
                        EndEdit();
                    }
                }
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
            CancelEdit();
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

        private void btnCut_Click(object sender, EventArgs e)
        {
            CopyCell(true);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CopyCell(false);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            PasteCell();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (frmFind == null || !frmFind.Visible)
            {
                frmFind = new FrmFind(this);

                // center the form within the bounds of its parent
                frmFind.Left = (Left + Right - frmFind.Width) / 2;
                frmFind.Top = (Top + Bottom - frmFind.Height) / 2;
                frmFind.Show(this);
            }
            else
            {
                frmFind.Activate();
            }
        }

        private void btnAutoSizeColumns_Click(object sender, EventArgs e)
        {
            dataGridView.AutoSizeColumns();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView.CurrentRow != null)
                {
                    Form form = CreatePropertiesForm();
                    if (form != null)
                        form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, AppPhrases.EditItemPropsError);
            }
        }
    }
}
