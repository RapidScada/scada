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
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Form for editing a table of the configuration database.
    /// <para>Форма редактирования таблицы базы конфигурации.</para>
    /// </summary>
    public partial class FrmBaseTable : Form, IChildForm
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

        private readonly IBaseTable baseTable;    // the table being edited
        private readonly TableFilter tableFilter; // the table filter
        private readonly ScadaProject project;    // the project under development
        private readonly AppData appData;         // the common data of the application

        private DataTable dataTable; // the table used by a grid view control
        private int maxRowID;        // the maximum ID in the table
        private FrmFind frmFind;     // the find and replace form
        private FrmFilter frmFilter; // the filter form


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
        public FrmBaseTable(IBaseTable baseTable, TableFilter tableFilter, ScadaProject project, AppData appData)
            : this()
        {
            this.baseTable = baseTable ?? throw new ArgumentNullException("baseTable");
            this.tableFilter = tableFilter;
            this.project = project ?? throw new ArgumentNullException("project");
            this.appData = appData ?? throw new ArgumentNullException("appData");

            dataTable = null;
            maxRowID = 0;
            frmFind = null;
            frmFilter = null;

            Text = baseTable.Title + (tableFilter == null ? "" : " - " + tableFilter);
        }


        /// <summary>
        /// Gets a value indicating whether an item properties form is available.
        /// </summary>
        private bool ProperiesAvailable
        {
            get
            {
                Type itemType = baseTable.ItemType;
                return itemType == typeof(InCnl) || itemType == typeof(CtrlCnl);
            }
        }

        /// <summary>
        /// Gets the type of the table items.
        /// </summary>
        public Type ItemType
        {
            get
            {
                return baseTable.ItemType;
            }
        }

        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Creates a form to edit item properties.
        /// </summary>
        private Form CreatePropertiesForm()
        {
            Type itemType = baseTable.ItemType;

            if (itemType == typeof(InCnl))
                return new FrmInCnlProps(dataGridView);
            else if (itemType == typeof(CtrlCnl))
                return new FrmCtrlCnlProps(dataGridView);
            else
                return null;
        }

        /// <summary>
        /// Loads the table data.
        /// </summary>
        private void LoadTableData()
        {
            if (!project.ConfigBase.Load(out string errMsg))
                appData.ProcError(errMsg);

            // save the existing filter
            string rowFilter = dataTable?.DefaultView.RowFilter ?? "";

            // reset the binding source
            bindingSource.DataSource = null;

            // create a data table
            dataTable = tableFilter == null ?
                baseTable.ToDataTable() :
                baseTable.SelectItems(tableFilter).ToDataTable(baseTable.ItemType);
            dataTable.DefaultView.Sort = baseTable.PrimaryKey;
            maxRowID = dataTable.DefaultView.Count > 0 ? 
                (int)dataTable.DefaultView[dataTable.DefaultView.Count - 1][baseTable.PrimaryKey] : 0;
            dataTable.DefaultView.RowFilter = rowFilter;

            // set the binding source before creating grid columns in case of work on Mono
            if (ScadaUtils.IsRunningOnMono)
                bindingSource.DataSource = dataTable;

            // bind table events
            dataTable.TableNewRow += dataTable_TableNewRow;
            dataTable.RowChanged += dataTable_RowChanged;
            dataTable.RowDeleted += dataTable_RowDeleted;

            // create grid columns
            ColumnBuilder columnBuilder = new ColumnBuilder(project.ConfigBase);
            dataGridView.Columns.Clear();
            dataGridView.Columns.AddRange(columnBuilder.CreateColumns(baseTable.ItemType));

            // set default values
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Tag is ColumnOptions options && options.DefaultValue != null)
                    dataTable.Columns[column.Name].DefaultValue = options.DefaultValue;
            }

            if (tableFilter != null)
                dataTable.Columns[tableFilter.ColumnName].DefaultValue = tableFilter.Value;

            // set the binding source
            if (!ScadaUtils.IsRunningOnMono)
                bindingSource.DataSource = dataTable;

            dataGridView.AutoSizeColumns();
            ChildFormTag.Modified = baseTable.Modified;
        }

        /// <summary>
        /// Copies the changes from a one table to another.
        /// </summary>
        private void RetrieveChanges()
        {
            baseTable.RetrieveChanges(dataTable);
            baseTable.Modified = true;
            ChildFormTag.Modified = true;
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
        /// Validates that the primary key value is unique.
        /// </summary>
        private bool PkUnique(int key, out string errMsg)
        {
            if (baseTable.PkExists(key))
            {
                errMsg = string.Format(AppPhrases.UniqueRequired,
                    dataGridView.Columns[baseTable.PrimaryKey].HeaderText);
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Validates that no record refers the primary key.
        /// </summary>
        private bool NoReferencesToPk(int key, out string errMsg)
        {
            foreach (TableRelation relation in baseTable.Dependent)
            {
                if (relation.ChildTable.TryGetIndex(relation.ChildColumn, out TableIndex index) &&
                    index.IndexKeyExists(key))
                {
                    errMsg = string.Format(AppPhrases.KeyReferenced, relation.ChildTable.Title);
                    return false;
                }
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Validates that the primary key exists.
        /// </summary>
        private bool PkExists(IBaseTable parentTable, int key, string childColumn, out string errMsg)
        {
            if (parentTable.PkExists(key))
            {
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = string.Format(AppPhrases.DataNotExist, dataGridView.Columns[childColumn].HeaderText);
                return false;
            }
        }

        /// <summary>
        /// Deletes the selected rows.
        /// </summary>
        private void DeleteSelectedRows()
        {
            try
            {
                bool notDeleted = false;

                try
                {
                    dataTable.RowDeleted -= dataTable_RowDeleted;
                    DataView dataView = dataTable.DefaultView;
                    DataGridViewSelectedRowCollection selectedRows = dataGridView.SelectedRows;

                    if (selectedRows.Count > 0)
                    {
                        for (int i = selectedRows.Count - 1; i >= 0; i--)
                        {
                            if (!DeleteRow(dataView, selectedRows[i].Index))
                                notDeleted = true;
                        }
                    }
                    else if (dataGridView.CurrentRow != null)
                    {
                        if (!DeleteRow(dataView, dataGridView.CurrentRow.Index))
                            notDeleted = true;
                    }
                }
                finally
                {
                    dataTable.RowDeleted += dataTable_RowDeleted;
                }

                RetrieveChanges();

                if (notDeleted)
                    ScadaUiUtils.ShowInfo(AppPhrases.RowsNotDeleted);
            }
            catch (Exception ex)
            {
                appData.ErrLog.WriteException(ex, AppPhrases.DataChangeError);
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Clears the table data.
        /// </summary>
        private void ClearTableData()
        {
            try
            {
                bool notDeleted = false;

                try
                {
                    bindingSource.DataSource = null;
                    dataTable.RowDeleted -= dataTable_RowDeleted;
                    DataView dataView = dataTable.DefaultView;

                    for (int rowIndex = dataView.Count - 1; rowIndex >= 0; rowIndex--)
                    {
                        if (!DeleteRow(dataView, rowIndex))
                            notDeleted = true;
                    }
                }
                finally
                {
                    dataTable.RowDeleted += dataTable_RowDeleted;
                    bindingSource.DataSource = dataTable;
                }

                RetrieveChanges();

                if (notDeleted)
                    ScadaUiUtils.ShowInfo(AppPhrases.RowsNotDeleted);
            }
            catch (Exception ex)
            {
                appData.ErrLog.WriteException(ex, AppPhrases.DataChangeError);
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a row with the specified index if it is not referenced.
        /// </summary>
        private bool DeleteRow(DataView dataView, int rowIndex)
        {
            if (0 <= rowIndex && rowIndex < dataView.Count)
            {
                DataRowView rowView = dataView[rowIndex];

                if (!rowView.IsNew)
                {
                    int key = (int)rowView.Row[baseTable.PrimaryKey];

                    if (NoReferencesToPk(key, out string errMsg))
                    {
                        dataView.Delete(rowIndex);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
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
        /// Shows error message in the error panel.
        /// </summary>
        private void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
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
                string interfaceDir = ScadaUtils.NormalDir(project.Interface.InterfaceDir);
                string selectedPath = Path.Combine(interfaceDir, cellValue == null ? "" : cellValue.ToString());

                if (buttonKind == ColumnKind.SelectFolderButton)
                {
                    // select folder
                    selectedPath = Path.GetDirectoryName(selectedPath);
                    folderBrowserDialog.SelectedPath = Directory.Exists(selectedPath) ? selectedPath : interfaceDir;

                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                        path = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);
                }
                else
                {
                    // select file
                    if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                    {
                        openFileDialog.InitialDirectory = interfaceDir;
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
                        {
                            if (cut)
                                textBox.Cut();
                            else
                                textBox.Copy();
                        }
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
                    // do nothing if the cell is already in edit mode
                    if (!cell.IsInEditMode && dataGridView.BeginEdit(true))
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

        /// <summary>
        /// Saves the table.
        /// </summary>
        public void Save()
        {
            if (project.ConfigBase.SaveTable(baseTable, out string errMsg))
                ChildFormTag.Modified = false;
            else
                appData.ProcError(errMsg);
        }


        private void FrmBaseTable_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName, null, cmsTable);

            if (lblCount.Text.Contains("{0}"))
                bindingNavigator.CountItemFormat = lblCount.Text;

            ChildFormTag.MainFormMessage += ChildFormTag_MainFormMessage;
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

        private void ChildFormTag_MainFormMessage(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AppMessage.RefreshData)
                btnRefresh_Click(null, null);
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
                        object cellValue = dataTable.DefaultView[rowInd][colInd];
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

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int colInd = e.ColumnIndex;
            int rowInd = e.RowIndex;

            if (0 <= rowInd && rowInd < dataGridView.RowCount &&
                0 <= colInd && colInd < dataGridView.ColumnCount &&
                e.Button == MouseButtons.Right && e.Clicks == 1 &&
                (dataGridView.CurrentCell == null || !dataGridView.CurrentCell.IsInEditMode))
            {
                // select cell on right-click
                dataGridView.CurrentCell = dataGridView[colInd, rowInd];

                // show context menu
                if (ProperiesAvailable)
                    cmsTable.Show(MousePosition);
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


        private void dataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            // generate a new ID
            int newRowID = maxRowID + 1;
            if (!baseTable.PkExists(newRowID))
                e.Row[0] = newRowID;
        }

        private void dataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;

            try
            {
                if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
                {
                    bool rowIsValid = true;
                    string errMsg = "";
                    int curKey; // the current key of the row

                    if (e.Action == DataRowAction.Add)
                    {
                        curKey = (int)row[baseTable.PrimaryKey];
                        rowIsValid = PkUnique(curKey, out errMsg);
                    }
                    else // DataRowAction.Change
                    {
                        curKey = (int)row[baseTable.PrimaryKey, DataRowVersion.Current];
                        int origKey = (int)row[baseTable.PrimaryKey, DataRowVersion.Original];
                        if (origKey != curKey)
                            rowIsValid = PkUnique(curKey, out errMsg) && NoReferencesToPk(origKey, out errMsg);
                    }

                    // check the table dependencies
                    if (rowIsValid)
                    {
                        foreach (TableRelation relation in baseTable.DependsOn)
                        {
                            if (!row.IsNull(relation.ChildColumn))
                            {
                                int keyVal = (int)row[relation.ChildColumn];
                                if (!PkExists(relation.ParentTable, keyVal, relation.ChildColumn, out errMsg))
                                {
                                    rowIsValid = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (rowIsValid)
                    {
                        RetrieveChanges();

                        if (maxRowID < curKey)
                            maxRowID = curKey;
                    }
                    else
                    {
                        row.RejectChanges();
                        ScadaUiUtils.ShowError(errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                row.RejectChanges();
                appData.ErrLog.WriteException(ex, AppPhrases.DataChangeError);
                ShowError(ex.Message);
            }
        }

        private void dataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;

            try
            {
                if (e.Action == DataRowAction.Delete)
                {
                    int key = (int)row[baseTable.PrimaryKey, DataRowVersion.Original];

                    if (NoReferencesToPk(key, out string errMsg))
                    {
                        RetrieveChanges();
                    }
                    else
                    {
                        row.RejectChanges();
                        ScadaUiUtils.ShowError(errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                row.RejectChanges();
                appData.ErrLog.WriteException(ex, AppPhrases.DataChangeError);
                ShowError(ex.Message);
            }
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
                frmFind = new FrmFind(this, dataGridView);

                // center the form within the bounds of its parent
                frmFind.Left = (ParentForm.Left + ParentForm.Right - frmFind.Width) / 2;
                frmFind.Top = (ParentForm.Top + ParentForm.Bottom - frmFind.Height) / 2;
                frmFind.Show(this);
            }
            else
            {
                frmFind.Activate();
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            frmFilter = frmFilter ?? new FrmFilter(dataGridView);
            frmFilter.DataTable = dataTable;

            if (frmFilter.ShowDialog() == DialogResult.OK)
            {
                btnFilter.Image = frmFilter.FilterIsEmpty ? 
                    Properties.Resources.filter : 
                    Properties.Resources.filter_set;
            }
        }

        private void btnAutoSizeColumns_Click(object sender, EventArgs e)
        {
            dataGridView.AutoSizeColumns();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            // show an item properties form
            if (dataGridView.CurrentRow != null)
            {
                Form form = CreatePropertiesForm();
                if (form != null && form.ShowDialog() == DialogResult.OK)
                    EndEdit();
            }
        }
    }
}
