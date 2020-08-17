/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : Server Shell
 * Summary  : Form for viewing and editing an event table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2020
 */

using Scada.Data.Tables;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Utils;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for viewing and editing an event table.
    /// <para>Форма для просмотра и редактирования таблицы событий.</para>
    /// </summary>
    public partial class FrmEventTable : Form
    {
        private readonly Log errLog;                // the application error log
        private readonly EventAdapter eventAdapter; // the adapter to load and save an event table
        private DataTable dataTable; // the table containing events


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmEventTable()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmEventTable(Log errLog)
            : this()
        {
            this.errLog = errLog ?? throw new ArgumentNullException("errLog");
            eventAdapter = new EventAdapter();
            dataTable = new DataTable();

            FileName = "";
            AllowEdit = false;
        }


        /// <summary>
        /// Gets or sets the file name of the event table.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event table can be edited.
        /// </summary>
        public bool AllowEdit { get; set; }


        /// <summary>
        /// Loads the event table.
        /// </summary>
        private bool LoadEventTable(DataTable table)
        {
            try
            {
                eventAdapter.FileName = FileName;
                eventAdapter.Fill(table);
                return true;
            }
            catch (Exception ex)
            {
                errLog.WriteException(ex, ServerShellPhrases.LoadEventTableError);
                ScadaUiUtils.ShowError(ServerShellPhrases.LoadEventTableError + ": " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Saves the event table.
        /// </summary>
        private bool SaveEventTable()
        {
            try
            {
                eventAdapter.FileName = FileName;
                eventAdapter.Update(dataTable);
                dataGridView.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                errLog.WriteException(ex, ServerShellPhrases.SaveEventTableError);
                ScadaUiUtils.ShowError(ServerShellPhrases.SaveEventTableError + ": " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Validates a value of the specified cell.
        /// </summary>
        private bool ValidateCell(int colInd, int rowInd, object cellVal)
        {
            if (0 <= colInd && colInd < dataGridView.ColumnCount &&
                0 <= rowInd && rowInd < dataGridView.RowCount &&
                cellVal != null)
            {
                Type valType = dataGridView.Columns[colInd].ValueType;
                string valStr = cellVal.ToString();

                if (valType == typeof(int))
                {
                    if (!int.TryParse(valStr, out int intVal))
                    {
                        ScadaUiUtils.ShowError(CommonPhrases.IntegerRequired);
                        return false;
                    }
                }
                else if (valType == typeof(double))
                {
                    if (!double.TryParse(valStr, out double doubleVal))
                    {
                        ScadaUiUtils.ShowError(CommonPhrases.RealRequired);
                        return false;
                    }
                }
                else if (valType == typeof(DateTime))
                {
                    if (!DateTime.TryParse(valStr, out DateTime dtVal))
                    {
                        ScadaUiUtils.ShowError(CommonPhrases.DateTimeRequired);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Exports data to CSV.
        /// </summary>
        private void ExportToCsv()
        {
            try
            {
                sfdCsv.FileName = Path.GetFileNameWithoutExtension(FileName) + ".csv";

                if (sfdCsv.ShowDialog() == DialogResult.OK)
                {
                    CsvConverter csvConverter = new CsvConverter(sfdCsv.FileName);
                    csvConverter.ConvertToCsv(dataTable);
                }
            }
            catch (Exception ex)
            {
                errLog.WriteException(ex, ServerShellPhrases.ExportToCsvError);
                ScadaUiUtils.ShowError(ServerShellPhrases.ExportToCsvError + ": " + ex.Message);
            }
        }


        private void FrmEventTableEdit_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            sfdCsv.SetFilter(ServerShellPhrases.CsvFileFilter);

            if (lblCount.Text.Contains("{0}"))
                bindingNavigator.CountItemFormat = lblCount.Text;

            Text = string.Format(AllowEdit ?
                ServerShellPhrases.EditEventsTitle : ServerShellPhrases.ViewEventsTitle,
                Path.GetFileName(FileName));
            btnSave.Visible = AllowEdit;

            if (LoadEventTable(dataTable))
            {
                dataTable.DefaultView.AllowNew = AllowEdit;
                dataTable.DefaultView.AllowEdit = AllowEdit;
                bindingSource.DataSource = dataTable;

                if (ScadaUtils.IsRunningOnMono)
                    CommShellUtils.RefreshColumns(dataGridView);
            }
            else
            {
                bindingNavigator.Enabled = false;
                dataGridView.Enabled = false;
                pnlBottom.Enabled = false;
            }
        }

        private void FrmEventTableEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose.Focus(); // to finish cell editing
            DataView dataView = new DataView(dataTable, "", "", 
                DataViewRowState.ModifiedCurrent | DataViewRowState.Added);

            if (dataView.Count > 0)
            {
                DialogResult dlgRes = MessageBox.Show(ServerShellPhrases.SaveEventsConfirm, 
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgRes == DialogResult.Yes)
                    e.Cancel = !SaveEventTable();
                else if (dlgRes != DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void FrmEventTableEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // close the form by Escape if no table cell is editing
            if (e.KeyCode == Keys.Escape &&
                dataGridView.CurrentCell != null && !dataGridView.CurrentCell.IsInEditMode)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // reload events
            DataTable newDataTable = new DataTable();

            if (LoadEventTable(newDataTable))
            {
                dataTable = newDataTable;
                dataTable.DefaultView.AllowNew = AllowEdit;
                dataTable.DefaultView.AllowEdit = AllowEdit;

                try { dataTable.DefaultView.RowFilter = txtFilter.Text; }
                catch { txtFilter.Text = ""; }

                bindingSource.DataSource = dataTable;
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            // set table filter
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    dataTable.DefaultView.RowFilter = txtFilter.Text;
                }
                catch
                {
                    ScadaUiUtils.ShowError(ServerShellPhrases.IncorrectEventFilter);
                }
            }
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // validate data entered in the cell
            e.Cancel = dataGridView.CurrentCell != null && dataGridView.CurrentCell.IsInEditMode &&
                !ValidateCell(e.ColumnIndex, e.RowIndex, e.FormattedValue);
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // show error message
            ScadaUiUtils.ShowError(CommonPhrases.GridDataError + ": " + e.Exception.Message);
            e.ThrowException = false;
        }

        private void btnExportToCsv_Click(object sender, EventArgs e)
        {
            ExportToCsv();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveEventTable();
        }
    }
}
