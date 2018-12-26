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
 * Module   : SCADA-Server Control
 * Summary  : Editing or viewing snapshot table form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2016
 */

using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Data;
using System.Windows.Forms;
using Utils;

namespace Scada.Server.Ctrl
{
    /// <summary>
    /// Editing or viewing snapshot table form
    /// <para>Форма редактирования или просмотра таблицы срезов</para>
    /// </summary>
    public partial class FrmSrezTableEdit : Form
    {
        private Log errLog;              // журнал ошибок приложения
        private SrezAdapter srezAdapter; // адаптер таблицы срезов
        private SrezTable srezTable;     // таблица срезов
        private DataTable dataTable1;    // таблица даты и времени срезов
        private DataTable dataTable2;    // таблица номеров и данных каналов
        private SrezTable.Srez selSrez;  // выбранный срез
        private bool editMode;           // режим редактирования


        /// <summary>
        /// Конструктор
        /// </summary>
        private FrmSrezTableEdit()
        {
            InitializeComponent();
            errLog = null;
            srezAdapter = null;
            srezTable = null;
            dataTable1 = null;
            dataTable2 = null;
            selSrez = null;
            editMode = false;
        }
        

        /// <summary>
        /// Проверить корректность вещественного значения ячейки
        /// </summary>
        private bool ValidateCell(int colInd, int rowInd, object cellVal)
        {
            if (0 <= colInd && colInd < dataGridView2.ColumnCount && 0 <= rowInd && rowInd < dataGridView2.RowCount &&
                cellVal != null)
            {
                Type valType = dataGridView2.Columns[colInd].ValueType;
                string valStr = cellVal.ToString();

                if (valType == typeof(int))
                {
                    int intVal;
                    if (!int.TryParse(valStr, out intVal))
                    {
                        ScadaUiUtils.ShowError(CommonPhrases.IntegerRequired);
                        return false;
                    }
                }
                else if (valType == typeof(double))
                {
                    double doubleVal;
                    if (!double.TryParse(valStr, out doubleVal))
                    {
                        ScadaUiUtils.ShowError(CommonPhrases.RealRequired);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Сохранить изменения таблицы срезов
        /// </summary>
        private bool Save()
        {
            try
            {
                srezAdapter.Update(srezTable);
                return true;
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.SaveSrezTableError + ":\r\n" + ex.Message;
                if (errLog != null)
                    errLog.WriteAction(errMsg, Log.ActTypes.Exception);
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Создать и заполнить dataTable1 данными из srezTable
        /// </summary>
        private void FillDataTable1()
        {
            DataTable newDataTable1 = new DataTable();
            newDataTable1.Columns.Add("DateTime", typeof(DateTime));
            newDataTable1.DefaultView.AllowNew = false;
            newDataTable1.DefaultView.AllowEdit = false;
            newDataTable1.DefaultView.AllowDelete = false;
            newDataTable1.BeginLoadData();

            foreach (DateTime srezDT in srezTable.SrezList.Keys)
            {
                DataRow row = newDataTable1.NewRow();
                row["DateTime"] = srezDT;
                newDataTable1.Rows.Add(row);
            }

            newDataTable1.EndLoadData();
            dataTable1 = newDataTable1;
            bindingSource1.DataSource = dataTable1;
        }
        
        /// <summary>
        /// Создать и заполнить dataTable2 данными из srezTable
        /// </summary>
        private void FillDataTable2(DateTime srezDT)
        {
            DataTable newDataTable2 = new DataTable();
            newDataTable2.Columns.Add("CnlNum", typeof(int));
            newDataTable2.Columns.Add("Val", typeof(double));
            newDataTable2.Columns.Add("Stat", typeof(int));
            newDataTable2.DefaultView.AllowNew = false;
            newDataTable2.DefaultView.AllowEdit = editMode;
            newDataTable2.DefaultView.AllowDelete = false;

            selSrez = srezDT > DateTime.MinValue ? srezTable.GetSrez(srezDT) : null;

            if (selSrez != null)
            {
                newDataTable2.BeginLoadData();
                bindingSource2.DataSource = null; // для ускорения изменения данных в таблице
                int cnlCnt = selSrez.CnlNums.Length;

                for (int i = 0; i < cnlCnt; i++)
                {
                    DataRow row = newDataTable2.NewRow();
                    row["CnlNum"] = selSrez.CnlNums[i];
                    SrezTable.CnlData cnlData = selSrez.CnlData[i];
                    row["Val"] = cnlData.Val;
                    row["Stat"] = cnlData.Stat;
                    newDataTable2.Rows.Add(row);
                }

                newDataTable2.EndLoadData();
                dataTable2 = newDataTable2;
                dataTable2.RowChanged += dataTable2_RowChanged;
                bindingSource2.DataSource = dataTable2;
            }
        }

        /// <summary>
        /// Загрузить таблицу срезов
        /// </summary>
        private static bool LoadSrezTable(SrezAdapter srezAdapter, Log errLog, ref SrezTable srezTable)
        {
            try
            {
                srezAdapter.Fill(srezTable);
                return true;
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.LoadSrezTableError + ":\r\n" + ex.Message;
                if (errLog != null)
                    errLog.WriteAction(errMsg, Log.ActTypes.Exception);
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Отобразить форму редактирования или просмотра таблицы срезов
        /// </summary>
        public static void Show(string directory, string tableName, bool editMode, Log errLog)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("directory");
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException("tableName");
            if (errLog == null)
                throw new ArgumentNullException("errLog");

            SrezAdapter srezAdapter = new SrezAdapter();
            srezAdapter.Directory = directory;
            srezAdapter.TableName = tableName;
            SrezTable srezTable = new SrezTable();

            if (LoadSrezTable(srezAdapter, errLog, ref srezTable))
            {
                FrmSrezTableEdit frmSrezTableEdit = new FrmSrezTableEdit();
                frmSrezTableEdit.errLog = errLog;
                frmSrezTableEdit.srezAdapter = srezAdapter;
                frmSrezTableEdit.srezTable = srezTable;
                frmSrezTableEdit.editMode = editMode;
                frmSrezTableEdit.ShowDialog();
            }
        }


        private void FrmSrezTableEdit_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Server.Ctrl.FrmSrezTableEdit");
            if (lblCount1.Text.Contains("{0}"))
                bindingNavigator1.CountItemFormat = lblCount1.Text;
            if (lblCount2.Text.Contains("{0}"))
                bindingNavigator2.CountItemFormat = lblCount2.Text;

            // настройка элементов управления
            Text = (editMode ? AppPhrases.EditSrezTableTitle : AppPhrases.ViewSrezTableTitle) + 
                " - " + srezAdapter.TableName;
            btnSave.Visible = editMode;
            FillDataTable1();
        }

        private void FrmSrezTableEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose.Focus(); // для завершения редактирования ячейки таблицы

            if (srezTable.Modified)
            {
                DialogResult dlgRes = MessageBox.Show(AppPhrases.SaveSrezTableConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgRes == DialogResult.Yes)
                    e.Cancel = !Save();
                else if (dlgRes != DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void FrmSrezTableEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // закрытие формы по Escape, если ячейка таблицы не редактируется
            if (e.KeyCode == Keys.Escape && 
                dataGridView2.CurrentCell != null && !dataGridView2.CurrentCell.IsInEditMode)
                DialogResult = DialogResult.Cancel;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // перезагрузка таблицы
            SrezTable newSrezTable = new SrezTable();

            if (LoadSrezTable(srezAdapter, errLog, ref newSrezTable))
            {
                srezTable = newSrezTable;
                FillDataTable1();
                try { dataTable2.DefaultView.RowFilter = txtFilter.Text; }
                catch { txtFilter.Text = ""; }
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            // установка фильтра таблицы
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    dataTable2.DefaultView.RowFilter = txtFilter.Text;
                }
                catch
                {
                    ScadaUiUtils.ShowError(AppPhrases.IncorrectFilter);
                }
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            // заполнение номеров и данных каналов за выбранную дату и время
            DateTime srezDT;
            try { srezDT = (DateTime)dataGridView1.CurrentCell.Value; }
            catch { srezDT = DateTime.MinValue;}

            FillDataTable2(srezDT);
            try { dataTable2.DefaultView.RowFilter = txtFilter.Text; }
            catch { txtFilter.Text = ""; }
        }

        private void dataGridView2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // проверка вводимых в ячейку данных
            e.Cancel = dataGridView2.CurrentCell != null && dataGridView2.CurrentCell.IsInEditMode && 
                !ValidateCell(e.ColumnIndex, e.RowIndex, e.FormattedValue);
        }
        
        private void dataTable2_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            // перенос изменений в таблицу срезов
            if (e.Action == DataRowAction.Change && selSrez != null)
            {
                DataRow row = e.Row;
                int cnlNum = (int)row["CnlNum"];
                SrezTable.CnlData cnlData = new SrezTableLight.CnlData((double)row["Val"], (int)row["Stat"]);
                selSrez.SetCnlData(cnlNum, cnlData);
                srezTable.MarkSrezAsModified(selSrez);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение изменений таблицы срезов
            Save();
        }
    }
}
