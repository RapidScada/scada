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
 * Summary  : Editing or viewing snapshot table form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2020
 */

using Scada.Data.Tables;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Utils;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for viewing and editing a snapshot table.
    /// <para>Форма для просмотра и редактирования таблицы срезов.</para>
    /// </summary>
    public partial class FrmSnapshotTable : Form
    {
        private readonly Log errLog;              // the application error log
        private readonly SrezAdapter srezAdapter; // the adapter to load and save a snapshot table

        private SrezTable srezTable;     // the snapshot table to view and edit
        private DataTable dataTable1;    // the table contains date and time of snapshots
        private DataTable dataTable2;    // the table contains numbers and data of input channels
        private SrezTable.Srez selSrez;  // the selected snapshot


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmSnapshotTable()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSnapshotTable(Log errLog)
            : this()
        {
            this.errLog = errLog ?? throw new ArgumentNullException("errLog");
            srezAdapter = new SrezAdapter();

            srezTable = new SrezTable();
            dataTable1 = null;
            dataTable2 = null;
            selSrez = null;

            FileName = "";
            AllowEdit = false;
        }


        /// <summary>
        /// Gets or sets the file name of the snapshot table.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the snapshot table can be edited.
        /// </summary>
        public bool AllowEdit { get; set; }


        /// <summary>
        /// Loads the snapshot table.
        /// </summary>
        private bool LoadSnapshotTable(SrezTable table)
        {
            try
            {
                srezAdapter.FileName = FileName;
                srezAdapter.Fill(table);
                return true;
            }
            catch (Exception ex)
            {
                errLog.WriteException(ex, ServerShellPhrases.LoadSnapshotTableError);
                ScadaUiUtils.ShowError(ServerShellPhrases.LoadSnapshotTableError + ": " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Saves the snapshot table.
        /// </summary>
        private bool SaveSnapshotTable()
        {
            try
            {
                srezAdapter.FileName = FileName;
                srezAdapter.Update(srezTable);
                return true;
            }
            catch (Exception ex)
            {
                errLog.WriteException(ex, ServerShellPhrases.SaveSnapshotTableError);
                ScadaUiUtils.ShowError(ServerShellPhrases.SaveSnapshotTableError + ": " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Initializes and fills dataTable1.
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

            if (ScadaUtils.IsRunningOnMono)
                CommShellUtils.RefreshColumns(dataGridView1);
        }

        /// <summary>
        /// Initializes and fills dataTable2.
        /// </summary>
        private void FillDataTable2(DateTime srezDT)
        {
            DataTable newDataTable2 = new DataTable();
            newDataTable2.Columns.Add("CnlNum", typeof(int));
            newDataTable2.Columns.Add("Val", typeof(double));
            newDataTable2.Columns.Add("Stat", typeof(int));
            newDataTable2.DefaultView.AllowNew = false;
            newDataTable2.DefaultView.AllowEdit = AllowEdit;
            newDataTable2.DefaultView.AllowDelete = false;

            selSrez = srezDT > DateTime.MinValue ? srezTable.GetSrez(srezDT) : null;

            if (selSrez != null)
            {
                newDataTable2.BeginLoadData();
                bindingSource2.DataSource = null; // to speed up data changes in the table
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

                if (ScadaUtils.IsRunningOnMono)
                    CommShellUtils.RefreshColumns(dataGridView2);
            }
        }

        /// <summary>
        /// Validates a value of the specified cell.
        /// </summary>
        private bool ValidateCell(int colInd, int rowInd, object cellVal)
        {
            if (0 <= colInd && colInd < dataGridView2.ColumnCount &&
                0 <= rowInd && rowInd < dataGridView2.RowCount &&
                cellVal != null)
            {
                Type valType = dataGridView2.Columns[colInd].ValueType;
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
                    csvConverter.ConvertToCsv(srezTable);
                }
            }
            catch (Exception ex)
            {
                errLog.WriteException(ex, ServerShellPhrases.ExportToCsvError);
                ScadaUiUtils.ShowError(ServerShellPhrases.ExportToCsvError + ": " + ex.Message);
            }
        }


        private void FrmSrezTableEdit_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            sfdCsv.SetFilter(ServerShellPhrases.CsvFileFilter);

            if (lblCount1.Text.Contains("{0}"))
                bindingNavigator1.CountItemFormat = lblCount1.Text;
            if (lblCount2.Text.Contains("{0}"))
                bindingNavigator2.CountItemFormat = lblCount2.Text;

            Text = string.Format(AllowEdit ? 
                ServerShellPhrases.EditSnapshotsTitle : ServerShellPhrases.ViewSnapshotsTitle,
                Path.GetFileName(FileName));
            btnSave.Visible = AllowEdit;

            if (LoadSnapshotTable(srezTable))
            {
                FillDataTable1();
            }
            else
            {
                pnlMain.Enabled = false;
                pnlBottom.Enabled = false;
            }
        }

        private void FrmSrezTableEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose.Focus(); // to finish cell editing

            if (srezTable.Modified)
            {
                DialogResult dlgRes = MessageBox.Show(ServerShellPhrases.SaveSnapshotsConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgRes == DialogResult.Yes)
                    e.Cancel = !SaveSnapshotTable();
                else if (dlgRes != DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void FrmSrezTableEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // close the form by Escape if no table cell is editing
            if (e.KeyCode == Keys.Escape &&
                dataGridView2.CurrentCell != null && !dataGridView2.CurrentCell.IsInEditMode)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // reload snapshots
            SrezTable newSrezTable = new SrezTable();

            if (LoadSnapshotTable(newSrezTable))
            {
                srezTable = newSrezTable;
                FillDataTable1();

                try { dataTable2.DefaultView.RowFilter = txtFilter.Text; }
                catch { txtFilter.Text = ""; }
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            // set table filter
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    dataTable2.DefaultView.RowFilter = txtFilter.Text;
                }
                catch
                {
                    ScadaUiUtils.ShowError(ServerShellPhrases.IncorrectSnapshotFilter);
                }
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            // show channel data for the selected time
            DateTime srezDT;
            try { srezDT = (DateTime)dataGridView1.CurrentCell.Value; }
            catch { srezDT = DateTime.MinValue;}

            FillDataTable2(srezDT);
            try { dataTable2.DefaultView.RowFilter = txtFilter.Text; }
            catch { txtFilter.Text = ""; }
        }

        private void dataGridView2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // validate data entered in the cell
            e.Cancel = dataGridView2.CurrentCell != null && dataGridView2.CurrentCell.IsInEditMode && 
                !ValidateCell(e.ColumnIndex, e.RowIndex, e.FormattedValue);
        }
        
        private void dataTable2_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            // transfer the changes to the snapshot table
            if (e.Action == DataRowAction.Change && selSrez != null)
            {
                DataRow row = e.Row;
                int cnlNum = (int)row["CnlNum"];
                SrezTable.CnlData cnlData = new SrezTableLight.CnlData((double)row["Val"], (int)row["Stat"]);
                selSrez.SetCnlData(cnlNum, cnlData);
                srezTable.MarkSrezAsModified(selSrez);
            }
        }

        private void btnExportToCsv_Click(object sender, EventArgs e)
        {
            ExportToCsv();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSnapshotTable();
        }
    }
}
