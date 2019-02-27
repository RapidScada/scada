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
 * Summary  : Form to find and replace in a table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2019
 */

using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Scada.Admin.App.Code;
using Scada.UI;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Form to find and replace in a table.
    /// <para>Форма поиска и замены в таблице.</para>
    /// </summary>
    public partial class FrmFind : Form
    {
        /// <summary>
        /// Represents information associated with a column.
        /// <para>Представляет информацию, связанную со столбцом.</para>
        /// </summary>
        private class ColumnInfo
        {
            private DataTable dataSource1;
            private DataTable dataSource2;

            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public ColumnInfo(DataGridViewColumn column)
            {
                Column = column ?? throw new ArgumentNullException("column");
                dataSource1 = null;
                dataSource2 = null;
            }

            /// <summary>
            /// Gets the column.
            /// </summary>
            public DataGridViewColumn Column { get; private set; }
            /// <summary>
            /// Gets the column header.
            /// </summary>
            public string Header
            {
                get
                {
                    return Column.HeaderText ?? "";
                }
            }
            /// <summary>
            /// Gets a value indicating whether the column contains text.
            /// </summary>
            public bool IsText
            {
                get
                {
                    return Column is DataGridViewTextBoxColumn;
                }
            }
            /// <summary>
            /// Gets the column from which to retrieve strings for display in the combo box.
            /// </summary>
            public string DisplayMember
            {
                get
                {
                    return Column is DataGridViewComboBoxColumn comboBoxColumn ? comboBoxColumn.DisplayMember : "";
                }
            }
            /// <summary>
            /// Gets the column from which to get values that correspond to the selections in the combo box.
            /// </summary>
            public string ValueMember
            {
                get
                {
                    return Column is DataGridViewComboBoxColumn comboBoxColumn ? comboBoxColumn.ValueMember : "";
                }
            }

            /// <summary>
            /// Gets the data source #1 contains columns values.
            /// </summary>
            public DataTable DataSource1
            {
                get
                {
                    if (dataSource1 == null)
                    {
                        dataSource1 = Column is DataGridViewComboBoxColumn comboBoxColumn ? 
                            CopyTable(comboBoxColumn.DataSource as DataTable) : null;
                    }

                    return dataSource1;
                }
            }
            /// <summary>
            /// Gets the data source #2 contains columns values.
            /// </summary>
            public DataTable DataSource2
            {
                get
                {
                    if (dataSource2 == null)
                    {
                        dataSource2 = Column is DataGridViewComboBoxColumn comboBoxColumn ?
                            CopyTable(comboBoxColumn.DataSource as DataTable) : null;
                    }

                    return dataSource2;
                }
            }

            /// <summary>
            /// Makes a table copy having disabled constraints.
            /// </summary>
            private DataTable CopyTable(DataTable dataTable)
            {
                if (dataTable == null)
                {
                    return null;
                }
                else
                {
                    DataTable tableCopy = new DataTable(dataTable.TableName);
                    tableCopy.BeginLoadData();
                    tableCopy.Merge(dataTable, false, MissingSchemaAction.Add);
                    tableCopy.DefaultView.Sort = dataTable.DefaultView.Sort;
                    return tableCopy;
                }
            }
            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            public override string ToString()
            {
                return Header;
            }
        }


        private readonly FrmBaseTable frmBaseTable;
        private readonly DataGridView dataGridView;
        private int startRowIndex;   // the starting point of the search
        private bool foundSomething; // at least one result found


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmFind()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFind(FrmBaseTable frmBaseTable, DataGridView dataGridView)
            : this()
        {
            this.frmBaseTable = frmBaseTable ?? throw new ArgumentNullException("frmBaseTable");
            this.dataGridView = dataGridView ?? throw new ArgumentNullException("dataGridView");
        }


        /// <summary>
        /// Fills the column list.
        /// </summary>
        private void FillColumnList(out ColumnInfo selColumnInfo)
        {
            try
            {
                cbTableColumn.BeginUpdate();
                DataGridViewCell curCell = dataGridView.CurrentCell;
                int curColInd = curCell == null ? -1 : curCell.ColumnIndex;
                int selColInd = 0;
                selColumnInfo = null;

                for (int i = 0, k = 0; i < dataGridView.Columns.Count; i++)
                {
                    DataGridViewColumn column = dataGridView.Columns[i];

                    if ((column is DataGridViewTextBoxColumn || column is DataGridViewComboBoxColumn) &&
                        !column.ReadOnly)
                    {
                        ColumnInfo colInfo = new ColumnInfo(column);
                        cbTableColumn.Items.Add(colInfo);

                        if (i == curColInd)
                        {
                            selColInd = k;
                            selColumnInfo = colInfo;
                        }

                        k++;
                    }
                }

                if (cbTableColumn.Items.Count > 0)
                    cbTableColumn.SelectedIndex = selColInd;
            }
            finally
            {
                cbTableColumn.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the default search condition.
        /// </summary>
        private void SetDefaultSearchCondition(ColumnInfo selColumnInfo)
        {
            DataGridViewCell curCell = dataGridView.CurrentCell;

            if (curCell != null && selColumnInfo != null)
            {
                if (selColumnInfo.IsText)
                {
                    if (curCell.EditedFormattedValue != null)
                        txtFind.Text = curCell.EditedFormattedValue.ToString();
                }
                else
                {
                    if (curCell.IsInEditMode)
                    {
                        if (dataGridView.EditingControl is ComboBox comboBox)
                            cbFind.SelectedValue = comboBox.SelectedValue;
                    }
                    else
                    {
                        cbFind.SelectedValue = curCell.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Adjusts the controls depending on the selected column.
        /// </summary>
        private void AdjustControls(ColumnInfo columnInfo)
        {
            if (columnInfo == null)
            {
                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = false;
            }
            else if (columnInfo.IsText)
            {
                txtFind.Visible = true;
                txtReplaceWith.Visible = true;
                cbFind.Visible = false;
                cbReplaceWith.Visible = false;
                chkCaseSensitive.Enabled = true;
                chkWholeCellOnly.Enabled = true;
                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = 
                    txtFind.Text != "";
            }
            else
            {
                txtFind.Visible = false;
                txtReplaceWith.Visible = false;
                cbFind.Visible = true;
                cbReplaceWith.Visible = true;
                chkCaseSensitive.Enabled = false;
                chkWholeCellOnly.Enabled = false;

                cbFind.DataSource = columnInfo.DataSource1;
                cbReplaceWith.DataSource = columnInfo.DataSource2;
                cbFind.DisplayMember = cbReplaceWith.DisplayMember = columnInfo.DisplayMember;
                cbFind.ValueMember = cbReplaceWith.ValueMember = columnInfo.ValueMember;

                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = 
                    cbFind.Items.Count > 0;
            }
        }

        /// <summary>
        /// Resets the starting point of the search
        /// </summary>
        private void ResetSearch()
        {
            startRowIndex = -1;
            foundSomething = false;
        }

        /// <summary>
        /// Checks that the cell is matched the search condition.
        /// </summary>
        private bool IsMatched(DataGridViewCell cell, string value, bool ignoreCase, bool wholeCellOnly)
        {
            if (cell == null)
            {
                return false;
            }
            else
            {
                StringComparison comparison = ignoreCase ? 
                    StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
                string cellVal = cell.EditedFormattedValue == null ? "" : cell.EditedFormattedValue.ToString();

                return wholeCellOnly && string.Compare(cellVal, value, ignoreCase) == 0 ||
                    !wholeCellOnly && cellVal.IndexOf(value, comparison) >= 0;
            }
        }

        /// <summary>
        /// Checks that the cell is matched the search condition.
        /// </summary>
        private bool IsMatched(DataGridViewCell cell, object value)
        {
            if (cell == null)
            {
                return false;
            }
            else
            {
                object cellVal = cell.IsInEditMode ?
                    (dataGridView.EditingControl as ComboBox)?.SelectedValue :
                    cell.Value;

                return cellVal == value || (cellVal is int) && (value is int) && ((int)cellVal == (int)value);
            }
        }

        /// <summary>
        /// Finds the next match.
        /// </summary>
        private bool FindNext(ColumnInfo columnInfo, bool showMsg)
        {
            bool isText = columnInfo.IsText;
            string findStr = isText ? txtFind.Text : "";
            object findObj = isText ? null : cbFind.SelectedValue;
            bool ignoreCase = !chkCaseSensitive.Checked;
            bool wholeCellOnly = chkWholeCellOnly.Checked;

            DataGridViewCell curCell = dataGridView.CurrentCell;
            bool found;
            int colInd = columnInfo.Column.Index;
            int rowInd = curCell == null ? 0 : curCell.RowIndex;
            int rowCnt = dataGridView.RowCount;

            if (curCell != null && curCell.ColumnIndex >= colInd)
                rowInd = rowInd == rowCnt - 1 ? 0 : rowInd + 1;

            if (startRowIndex < 0)
                startRowIndex = rowInd;

            do
            {
                DataGridViewCell cell = dataGridView[colInd, rowInd];
                found = isText ?
                    IsMatched(cell, findStr, ignoreCase, wholeCellOnly) :
                    IsMatched(cell, findObj);

                if (found)
                {
                    foundSomething = true;
                    try { dataGridView.CurrentCell = cell; }
                    catch { /* unable to leave the current cell because its value incorrect */ }
                }

                if (++rowInd >= rowCnt)
                    rowInd = 0;

            } while (!found && rowInd != startRowIndex);

            if (!found || rowInd == startRowIndex)
            {
                if (showMsg)
                    ScadaUiUtils.ShowInfo(foundSomething ? AppPhrases.SearchComplete : AppPhrases.ValueNotFound);

                ResetSearch();
            }

            return found;
        }

        /// <summary>
        /// Replaces part of the string.
        /// </summary>
        private string Replace(string s, string find, string replaceWith, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(find))
                return s;

            StringComparison comparison = ignoreCase ?
                StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            StringBuilder result = new StringBuilder();
            int len = s.Length;
            int findLen = find.Length;
            int startInd = 0;
            int ind = 0;

            while (ind >= 0 && startInd < len)
            {
                ind = s.IndexOf(find, startInd, comparison);
                if (ind >= 0)
                {
                    result.Append(s.Substring(startInd, ind - startInd));
                    result.Append(replaceWith);
                    startInd = ind + findLen;
                }
                else
                {
                    result.Append(s.Substring(startInd));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Replaces the cell value.
        /// </summary>
        private void ReplaceCell(ColumnInfo columnInfo, bool checkMatch, out bool replaced, out bool noError)
        {
            replaced = false;

            if (columnInfo.IsText)
            {
                if (!checkMatch ||
                    IsMatched(dataGridView.CurrentCell, txtFind.Text, !chkCaseSensitive.Checked, chkWholeCellOnly.Checked))
                {
                    // replace value of the text cell
                    dataGridView.BeginEdit(false);
                    if (dataGridView.EditingControl is TextBox textBox)
                    {
                        replaced = true;
                        textBox.Text = chkWholeCellOnly.Checked ? txtReplaceWith.Text :
                            Replace(textBox.Text, txtFind.Text, txtReplaceWith.Text, !chkCaseSensitive.Checked);
                    }
                }
            }
            else
            {
                if (!checkMatch || IsMatched(dataGridView.CurrentCell, cbFind.SelectedValue))
                {
                    // replace value of the combo box cell
                    dataGridView.BeginEdit(false);
                    if (dataGridView.EditingControl is ComboBox comboBox)
                    {
                        replaced = true;
                        // no exception is thrown if the specified value doesn't exist
                        comboBox.SelectedValue = cbReplaceWith.SelectedValue;
                    }
                }
            }

            noError = !replaced || frmBaseTable.EndEdit();
        }

        /// <summary>
        /// Replaces all cell values match the condition.
        /// </summary>
        private void ReplaceAll(ColumnInfo columnInfo)
        {
            // replace the current cell if it's matched the search condition
            int replacedCnt = 0;
            ReplaceCell(columnInfo, true, out bool replaced, out bool noError);

            if (replaced && noError)
                replacedCnt++;

            // replace other cells
            startRowIndex = dataGridView.CurrentCell == null ? 0 : dataGridView.CurrentCell.RowIndex;
            bool found;

            do
            {
                found = FindNext(columnInfo, false);

                if (found)
                {
                    ReplaceCell(columnInfo, false, out replaced, out noError);

                    if (replaced && noError)
                        replacedCnt++;
                }

            } while (found);

            if (replacedCnt > 0)
                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.ReplaceCount, replacedCnt));
            else if (noError)
                ScadaUiUtils.ShowInfo(AppPhrases.ValueNotFound);
        }


        private void FrmReplace_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);

            FillColumnList(out ColumnInfo selColumnInfo);
            SetDefaultSearchCondition(selColumnInfo);
            ResetSearch();

            if (cbTableColumn.Items.Count == 0 || txtFind.Visible && txtFind.Text == "")
                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = false;
        }

        private void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustControls(cbTableColumn.SelectedItem as ColumnInfo);
            ResetSearch();
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            ResetSearch();
            if (txtFind.Visible)
                btnFindNext.Enabled = btnReplace.Enabled = btnReplaceAll.Enabled = txtFind.Text != "";
        }

        private void cbFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void cbReplaceWith_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            if (cbTableColumn.SelectedItem is ColumnInfo columnInfo)
                FindNext(columnInfo, true);
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (cbTableColumn.SelectedItem is ColumnInfo columnInfo)
            {
                ReplaceCell(columnInfo, true, out bool replaced, out bool noError);
                if (noError)
                    FindNext(columnInfo, true);
            }
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            if (cbTableColumn.SelectedItem is ColumnInfo columnInfo)
                ReplaceAll(columnInfo);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
