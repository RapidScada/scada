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
 * Module   : SCADA-Administrator
 * Summary  : Editing configuration table form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2019
 */

using Scada;
using Scada.UI;
using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WinControl;

namespace ScadaAdmin
{
    /// <summary>
    /// Editing configuration table form
    /// <para>Форма редактирования таблицы конфигурации</para>
    /// </summary>
    public partial class FrmTable : Form, IChildForm
    {
        /// <summary>
        /// Макс. отображаемая длина текста в ячейке исходного кода
        /// </summary>
        private const int MaxSourceLenght = 50;

        private bool saveOccured;    // сохранение изменений произошло
        private bool saveOk;         // сохранение изменений выполнено успешно
        private bool pwdColExists;   // существует столбец пароля
        private bool clrColExists;   // существует столбец цвета
        private bool srcColExists;   // существует столбец исходного кода
        private bool modDTColExists; // существует столбец времени изменения


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmTable()
        {
            InitializeComponent();

            saveOccured = false;
            saveOk = false;
            pwdColExists = false;
            clrColExists = false;
            srcColExists = false;
            modDTColExists = false;

            ChildFormTag = null;
            Table = null;
            GridContextMenu = null;
        }


        #region IChildForm Members

        /// <summary>
        /// Получить или установить информацию о дочерней форме
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }

        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        public void Save()
        {
            string errMsg;
            saveOk = Tables.UpdateData(Table, out errMsg);
            if (saveOk)
            {
                SetModified(false);
            }
            else
            {
                SetModified(true);
                AppUtils.ProcError(errMsg);
            }
            saveOccured = true;
        }

        #endregion


        /// <summary>
        /// Установить признак изменения данных и соответствующую доступность кнопок
        /// </summary>
        private void SetModified(bool value)
        {
            if (ChildFormTag != null)
                ChildFormTag.Modified = value;

            bindingNavigatorUpdateItem.Enabled = bindingNavigatorCancelItem.Enabled = value;
            bindingNavigatorDeleteItem.Enabled = bindingNavigatorClearItem.Enabled =
                bindingNavigatorAutoResizeItem.Enabled = 
                Table == null ? false : Table.DefaultView.Count > 0;
        }

        /// <summary>
        /// Проверить корректность значения ячейки
        /// </summary>
        private bool ValidateCell(DataGridViewCell cell, bool showError)
        {
            return cell == null || !cell.IsInEditMode ? 
                true : ValidateCell(cell.ColumnIndex, cell.RowIndex, cell.EditedFormattedValue, showError);
        }

        /// <summary>
        /// Проверить корректность значения ячейки
        /// </summary>
        private bool ValidateCell(int colInd, int rowInd, object cellVal, bool showError)
        {
            bool result = true;

            if (0 <= colInd && colInd < dataGridView.ColumnCount && 0 <= rowInd && rowInd < dataGridView.RowCount &&
                cellVal != null)
            {
                string errMsg;
                result = Tables.ValidateCell(Table, Table.Columns[dataGridView.Columns[colInd].DataPropertyName], 
                    cellVal.ToString(), out errMsg);

                if (result)
                {
                    dataGridView.Rows[rowInd].ErrorText = "";
                }
                else
                {
                    dataGridView.Rows[rowInd].ErrorText = errMsg;
                    if (errMsg != "" && showError)
                        ScadaUiUtils.ShowError(errMsg);
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Преобразовать строку в цвет
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
        /// Получить или установить редактируемую таблицу
        /// </summary>
        public DataTable Table { get; set; }

        /// <summary>
        /// Получить элемент управления редактируемой таблицы
        /// </summary>
        public DataGridView GridView
        {
            get
            {
                return dataGridView;
            }
        }

        /// <summary>
        /// Получить или установить контекстное меню редактируемой таблицы
        /// </summary>
        public ContextMenuStrip GridContextMenu { get; set; }


        /// <summary>
        /// Подготовить форму к закрытию, отменив редактирование ячейки, если её значение некорректно
        /// </summary>
        public void PrepareClose(bool showError)
        {
            if (!ValidateCell(dataGridView.CurrentCell, showError))
                dataGridView.CancelEdit();
        }

        /// <summary>
        /// Вырезать в буфер значение текущей ячейки
        /// </summary>
        public void CellCut()
        {
            DataGridViewCell cell = dataGridView.CurrentCell;
            if (cell != null)
            {
                DataGridViewColumn col = dataGridView.Columns[cell.ColumnIndex];
                if (col is DataGridViewTextBoxColumn)
                {
                    if (cell.IsInEditMode)
                    {
                        TextBox txt = dataGridView.EditingControl as TextBox;
                        if (txt != null)
                            txt.Cut();
                    }
                    else
                    {
                        string val = cell.FormattedValue == null ? "" : cell.FormattedValue.ToString();
                        if (val == "")
                            Clipboard.Clear();
                        else
                            Clipboard.SetText(val);                        
                        cell.Value = cell.ValueType == typeof(string) ? "" : (object)DBNull.Value;
                    }
                }
                else if (col is DataGridViewComboBoxColumn)
                {
                    Clipboard.SetData("ScadaAdminCell", col.Name + ":" + cell.Value);
                    cell.Value = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Копировать в буфер значение текущей ячейки
        /// </summary>
        public void CellCopy()
        {
            DataGridViewCell cell = dataGridView.CurrentCell;
            if (cell != null)
            {
                DataGridViewColumn col = dataGridView.Columns[cell.ColumnIndex];
                if (col is DataGridViewTextBoxColumn)
                {
                    if (cell.IsInEditMode)
                    {
                        TextBox txt = dataGridView.EditingControl as TextBox;
                        if (txt != null)
                            txt.Copy();
                    }
                    else
                    {
                        string val = cell.FormattedValue == null ? "" : cell.FormattedValue.ToString();
                        if (val == "")
                            Clipboard.Clear();
                        else
                            Clipboard.SetText(val);
                    }
                }
                else if (col is DataGridViewComboBoxColumn)
                {
                    Clipboard.SetData("ScadaAdminCell", col.Name + ":" + cell.Value);
                }
            }
        }

        /// <summary>
        /// Копировать в текущую ячейку значение из буфера
        /// </summary>
        public void CellPaste()
        {
            DataGridViewCell cell = dataGridView.CurrentCell;
            if (cell != null)
            {
                DataGridViewColumn col = dataGridView.Columns[cell.ColumnIndex];
                if (col is DataGridViewTextBoxColumn)
                {
                    if (dataGridView.BeginEdit(true))
                    {
                        TextBox txt = dataGridView.EditingControl as TextBox;
                        if (txt != null)
                            txt.Paste();
                    }
                }
                else if (col is DataGridViewComboBoxColumn)
                {
                    if (Clipboard.ContainsData("ScadaAdminCell"))
                    {
                        object obj = Clipboard.GetData("ScadaAdminCell");
                        if (obj != null)
                        {
                            string str = obj.ToString();
                            int pos = str.IndexOf(":");
                            if (pos > 0)
                            {
                                string colName = str.Substring(0, pos);
                                if (col.Name == colName)
                                {
                                    string strVal = pos < str.Length - 1 ? 
                                        str.Substring(pos + 1, str.Length - pos - 1) : "";
                                    int intVal;
                                    if (int.TryParse(strVal, out intVal))
                                    {
                                        DataTable tbl = (col as DataGridViewComboBoxColumn).DataSource as DataTable;
                                        if (tbl != null && tbl.DefaultView.Find(intVal) >= 0)
                                            cell.Value = intVal;
                                    }
                                    else
                                        cell.Value = DBNull.Value;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Сохранить изменения таблицы в БД
        /// </summary>
        public bool UpdateTable()
        {
            if (ValidateCell(dataGridView.CurrentCell, true))
            {
                // если ячейка находится в режиме редактирования и её значение изменено,
                // то вызывается dataTable_RowChanged и происходит сохранение данных
                saveOccured = false;
                dataGridView.EndEdit();
                bindingSource.EndEdit();

                if (!saveOccured)
                    Save();

                dataGridView.Invalidate();
                return saveOk;
            }
            else
                return false;
        }


        private void FrmTable_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmTable");
            if (bindingNavigatorCountItem.Text.Contains("{0}"))
                bindingNavigator.CountItemFormat = bindingNavigatorCountItem.Text;
        }

        private void FrmObj_Shown(object sender, EventArgs e)
        {
            if (Table == null)
            {
                bindingNavigatorUpdateItem.Enabled = false;
                bindingNavigatorCancelItem.Enabled = false;
                bindingNavigatorRefreshItem.Enabled = false;
                bindingNavigatorDeleteItem.Enabled = false;
                bindingNavigatorClearItem.Enabled = false;
                bindingNavigatorAutoResizeItem.Enabled = false;
            }
            else
            {
                Table.RowChanged += dataTable_RowChanged;
                Table.RowDeleted += dataTable_RowDeleted;

                string tableName = Table.TableName;
                if (tableName == "User")
                    pwdColExists = true;
                else if (tableName == "EvType")
                    clrColExists = true;
                else if (tableName == "Formula")
                    srcColExists = true;
                else if (tableName == "InCnl" || tableName == "CtrlCnl")
                    modDTColExists = true;

                bindingSource.DataSource = Table;
                dataGridView.Columns.AddRange(Table.ExtendedProperties["Columns"] as DataGridViewColumn[]);
                dataGridView.AutoSizeColumns();

                SetModified(false);
            }
        }


        private void dataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
                Save();
        }

        private void dataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Delete)
                Save();
        }


        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            e.Cancel = !ValidateCell(e.ColumnIndex, e.RowIndex, e.FormattedValue, true);
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            AppUtils.ProcError(CommonPhrases.GridDataError + ":\r\n" + e.Exception.Message);
            e.ThrowException = false;
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SetModified(true);

                if (modDTColExists)
                {
                    DataGridViewColumn modDTCol = dataGridView.Columns["ModifiedDT"];
                    if (modDTCol != null && modDTCol != dataGridView.Columns[e.ColumnIndex])
                        dataGridView.Rows[e.RowIndex].Cells[modDTCol.Index].Value = DateTime.Now;
                }
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int colInd = e.ColumnIndex;

            if (0 <= colInd && colInd < dataGridView.Columns.Count && e.Value != null)
            {
                string colName = dataGridView.Columns[colInd].DataPropertyName;

                // скрытие пароля
                if (pwdColExists && colName == "Password")
                {
                    string passowrd = e.Value.ToString();
                    if (passowrd.Length > 0)
                        e.Value = new string('●', passowrd.Length);
                }

                // отображение текста ячейки соответствующим цветом
                if (clrColExists && colName == "Color")
                    e.CellStyle.ForeColor = StrToColor(e.Value.ToString());

                // ограничение отображаемой длины исходного кода
                if (srcColExists && colName == "Source")
                {
                    string source = e.Value.ToString();
                    if (source.Length > MaxSourceLenght)
                        e.Value = source.Substring(0, MaxSourceLenght) + "...";
                }
            }
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView.CurrentCell != null)
            {
                int colInd = dataGridView.CurrentCell.ColumnIndex;
                int rowInd = dataGridView.CurrentCell.RowIndex;

                if (0 <= colInd && colInd < dataGridView.Columns.Count)
                {
                    string colName = dataGridView.Columns[colInd].DataPropertyName;

                    // отображение пароля в режиме редактирования
                    if (pwdColExists && colName == "Password")
                    {
                        TextBox txt = e.Control as TextBox;
                        object val = Table.DefaultView[rowInd]["Password"];
                        if (txt != null && val != null && val != DBNull.Value)
                            txt.Text = val.ToString();
                    }

                    // отображение исходного кода полностью в режиме редактирования
                    if (srcColExists && colName == "Source")
                    {
                        TextBox txt = e.Control as TextBox;
                        object val = Table.DefaultView[rowInd]["Source"];
                        if (txt != null && val != null && val != DBNull.Value)
                            txt.Text = val.ToString();
                    }
                }
            }
        }

        private void dataGridView_Leave(object sender, EventArgs e)
        {
            PrepareClose(true);
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // вызов контекстного меню редактируемой таблицы
            if (GridContextMenu != null && e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                try
                {
                    DataGridViewCell cell = dataGridView[e.ColumnIndex, e.RowIndex];
                    if (!cell.IsInEditMode)
                    {
                        dataGridView.CurrentCell = cell;
                        GridContextMenu.Show(MousePosition);
                    }
                }
                catch
                {
                    // щёлкнули по шапке или левому полю таблицы или 
                    // невозможно изменить текущую ячейку
                }
            }
        }

        private void dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // вызов формы для редактирования значения ячейки
            if ((clrColExists || srcColExists) && e.Button == MouseButtons.Left)
            {
                int colInd = e.ColumnIndex;
                int rowInd = e.RowIndex;

                if (0 <= colInd && colInd < dataGridView.Columns.Count && rowInd >= 0)
                {
                    DataGridViewCell cell = dataGridView[colInd, rowInd];
                    string colName = dataGridView.Columns[colInd].DataPropertyName;

                    if (colName == "Color")
                    {
                        // вызов формы выбора цвета
                        dataGridView.EndEdit();
                        FrmSelectColor frmSelectColor = new FrmSelectColor();
                        frmSelectColor.SelectedColor = cell.Value == null ? 
                            Color.Empty : StrToColor(cell.Value.ToString());

                        if (frmSelectColor.ShowDialog() == DialogResult.OK)
                        {
                            cell.Value = frmSelectColor.SelectedColorName;
                            dataGridView.EndEdit();
                        }
                    }
                    else if (colName == "Source")
                    {
                        // вызов формы редактирования исходного кода
                        dataGridView.EndEdit();
                        FrmEditSource frmEditSource = new FrmEditSource()
                        {
                            MaxLength = Table.Columns[colName].MaxLength,
                            Source = cell.Value.ToString()
                        };

                        if (frmEditSource.ShowDialog() == DialogResult.OK)
                        {
                            cell.Value = frmEditSource.Source;
                            dataGridView.EndEdit();
                        }
                    }
                }
            }
        }


        private void bindingNavigatorUpdateItem_Click(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void bindingNavigatorCancelItem_Click(object sender, EventArgs e)
        {
            // отмена изменений таблицы  
            dataGridView.CancelEdit();
            bindingSource.CancelEdit();
            Table.RejectChanges();

            // очистка ошибок
            if (Table.HasErrors)
            {
                DataRow[] rowsInError = Table.GetErrors();
                foreach (DataRow row in rowsInError)
                    row.ClearErrors();
            }

            dataGridView.Invalidate();
            bindingSource.ResetBindings(false);
            SetModified(false);
        }

        private void bindingNavigatorRefreshItem_Click(object sender, EventArgs e)
        {
            if (UpdateTable())
            {
                Table.RowChanged -= dataTable_RowChanged;
                Table.RowDeleted -= dataTable_RowDeleted;
                bindingSource.DataSource = null; // для ускорения изменения данных в таблице
                

                try
                {
                    // обновление редактируемой таблицы
                    Table.Clear();
                    SqlCeDataAdapter adapter = Table.ExtendedProperties["DataAdapter"] as SqlCeDataAdapter;
                    adapter.Fill(Table);
                    Table.BeginLoadData();

                    // обновление таблиц, которые являются источниками данных для значений ячеек
                    foreach (DataGridViewColumn col in dataGridView.Columns)
                    {
                        DataGridViewComboBoxColumn cbCol = col as DataGridViewComboBoxColumn;
                        if (cbCol != null)
                        {
                            DataTable tbl = cbCol.DataSource as DataTable;
                            if (tbl != null)
                            {
                                bool emtyRowExists = tbl.DefaultView.Find(DBNull.Value) >= 0;
                                tbl.Clear();
                                adapter = tbl.ExtendedProperties["DataAdapter"] as SqlCeDataAdapter;
                                adapter.Fill(tbl);
                                if (emtyRowExists)
                                {
                                    tbl.BeginLoadData();
                                    tbl.Rows.Add(DBNull.Value, " ");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppUtils.ProcError(AppPhrases.RefreshDataError + ":\r\n" + ex.Message);
                }

                Table.RowChanged += dataTable_RowChanged;
                Table.RowDeleted += dataTable_RowDeleted;
                bindingSource.DataSource = Table;
                bindingSource.ResetBindings(false);

                SetModified(ChildFormTag.Modified); // установка доступности кнопок
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dataGridView.SelectedRows;

            if (MessageBox.Show(selectedRows.Count > 1 ? AppPhrases.DeleteRowsConfirm : AppPhrases.DeleteRowConfirm, 
                CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Table.RowDeleted -= dataTable_RowDeleted;

                if (selectedRows.Count > 0)
                {
                    for (int i = selectedRows.Count - 1; i >= 0; i--)
                    {
                        int ind = selectedRows[i].Index;
                        if (0 <= ind && ind < Table.DefaultView.Count)
                            Table.DefaultView.Delete(ind);
                    }
                }
                else if (dataGridView.CurrentRow != null)
                {
                    Table.DefaultView.Delete(dataGridView.CurrentRow.Index);
                }

                Save();
                Table.RowDeleted += dataTable_RowDeleted;
                bindingSource.ResetBindings(false);
            }
        }

        private void bindingNavigatorClearItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppPhrases.ClearTableConfirm, CommonPhrases.QuestionCaption, 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Table.RowDeleted -= dataTable_RowDeleted;
                bindingSource.DataSource = null; // для ускорения изменения данных в таблице

                for (int i = Table.DefaultView.Count - 1; i >= 0; i--)
                    Table.DefaultView.Delete(i);
                Save();

                Table.RowDeleted += dataTable_RowDeleted;
                bindingSource.DataSource = Table;
                bindingSource.ResetBindings(false);
            }
        }

        private void bindingNavigatorAutoResizeItem_Click(object sender, EventArgs e)
        {
            dataGridView.AutoSizeColumns();
        }
    }
}
