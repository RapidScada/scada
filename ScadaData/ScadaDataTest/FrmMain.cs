/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : ScadaDataTest
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2017
 */

using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ScadaDataTest
{
    /// <summary>
    /// Main form of the application
    /// </summary>
    public partial class FrmMain : Form
    {
        private DataTable dataTable;
        private SrezTable srezTable;

        public FrmMain()
        {
            InitializeComponent();
            dataTable = null;
            srezTable = null;
        }


        private void ShowRecordCount()
        {
            lblRecordCount.Text = "Record count: " + 
                (dataTable == null ? "-" : dataTable.DefaultView.Count.ToString());
            lblRecordCount.Visible = true;
        }


        private void rbTableType_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnFileName_Click(object sender, EventArgs e)
        {
            string fileName = txtFileName.Text.Trim();
            openFileDialog.InitialDirectory = string.IsNullOrEmpty(fileName) ?
                "" : Path.GetDirectoryName(fileName);
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtFileName.Text = openFileDialog.FileName;

            txtFileName.Focus();
            txtFileName.DeselectAll();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (dataTable != null)
            {
                dataTable.DefaultView.RowFilter = txtFilter.Text;
                ShowRecordCount();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbSnapshot.Checked)
                {
                    dataTable = new DataTable("SrezTable");
                    srezTable = new SrezTable();
                    SrezAdapter sa = new SrezAdapter();
                    sa.FileName = txtFileName.Text;
                    sa.Fill(dataTable);
                    sa.Fill(srezTable);
                    dataTable.RowChanged += DataTable_RowChanged;
                    dataTable.Columns["DateTime"].ReadOnly = true;
                    dataTable.Columns["CnlNum"].ReadOnly = true;
                }
                else if (rbEvent.Checked)
                {
                    dataTable = new DataTable("EventTable");
                    srezTable = null;
                    EventAdapter ea = new EventAdapter();
                    ea.FileName = txtFileName.Text;
                    ea.Fill(dataTable);
                }
                else // rbBase.Checked
                {
                    dataTable = new DataTable("BaseTable");
                    srezTable = null;
                    BaseAdapter ba = new BaseAdapter();
                    ba.FileName = txtFileName.Text;
                    ba.Fill(dataTable, true);
                }

                dataTable.DefaultView.AllowNew = !rbSnapshot.Checked;
                dataTable.DefaultView.AllowEdit = true;
                dataGridView.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                dataTable = null;
                ScadaUiUtils.ShowError(ex.Message);
            }
            finally
            {
                ShowRecordCount();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataTable == null)
                {
                    ScadaUiUtils.ShowWarning("Table is not initialized.");
                }
                else
                {
                    if (rbSnapshot.Checked)
                    {
                        SrezAdapter sa = new SrezAdapter();
                        sa.FileName = txtFileName.Text;
                        sa.Update(srezTable);
                    }
                    else if (rbEvent.Checked)
                    {
                        EventAdapter ea = new EventAdapter();
                        ea.FileName = txtFileName.Text;
                        ea.Update(dataTable);
                    }
                    else // rbBase.Checked
                    {
                        BaseAdapter ba = new BaseAdapter();
                        ba.FileName = txtFileName.Text;
                        ba.Update(dataTable);
                    }

                    ScadaUiUtils.ShowInfo("Data saved successfully.");
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ex.Message);
            }
        }

        private void DataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            // pass row changes from dataTable to srezTable
            if (e.Action == DataRowAction.Change)
            {
                DataRow row = e.Row;
                SrezTableLight.Srez srez;

                if (srezTable.SrezList.TryGetValue((DateTime)row["DateTime"], out srez))
                {
                    SrezTable.CnlData cnlData = new SrezTableLight.CnlData((double)row["Val"], (int)row["Stat"]);
                    srez.SetCnlData((int)row["CnlNum"], cnlData);
                    srezTable.MarkSrezAsModified(srez as SrezTable.Srez);
                }
            }
        }
    }
}
