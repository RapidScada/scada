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
 * Summary  : Form for setting a table filter.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Form for setting a table filter.
    /// <para>Форма для установки фильтра по таблице.</para>
    /// </summary>
    public partial class FrmFilter : Form
    {
        /// <summary>
        /// Represents a filter.
        /// <para>Представляет фильтр.</para>
        /// </summary>
        private class Filter
        {
            public string ColumnName { get; set; }
            public int StringOperation { get; set; }
            public int MathOperation { get; set; }
            public int ValueKey { get; set; }
            public string ValueText { get; set; }
        }

        private readonly DataGridView dataGridView;
        private readonly DataTable dataTable;
        private Filter currentFilter;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmFilter()
        {
            InitializeComponent();

            cbMathOperation.Top = cbStringOperation.Top;
            cbValue.Top = txtValue.Top;
            Translator.TranslateForm(this, GetType().FullName);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFilter(DataGridView dataGridView, DataTable dataTable)
            : this()
        {
            this.dataGridView = dataGridView ?? throw new ArgumentNullException("dataGridView");
            this.dataTable = dataTable ?? throw new ArgumentNullException("dataTable");
            currentFilter = null;
        }


        /// <summary>
        /// Gets a value indicating whether the filter is not set.
        /// </summary>
        public bool FilterIsEmpty
        {
            get
            {
                return currentFilter == null;
            }
        }
        
        
        /// <summary>
        /// Fills the column list.
        /// </summary>
        private void FillColumnList(string selectedColumnName)
        {
            try
            {
                cbColumn.BeginUpdate();
                cbColumn.Items.Clear();
                int selectedIndex = 0;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column is DataGridViewTextBoxColumn || 
                        column is DataGridViewComboBoxColumn ||
                        column is DataGridViewCheckBoxColumn)
                    {
                        ColumnInfo columnInfo = new ColumnInfo(column);
                        int index = cbColumn.Items.Add(columnInfo);

                        if (column.Name == selectedColumnName)
                            selectedIndex = index;
                   }
                }

                if (cbColumn.Items.Count > 0)
                    cbColumn.SelectedIndex = selectedIndex;
            }
            finally
            {
                cbColumn.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the default filter.
        /// </summary>
        private void SetDefaultFilter(ColumnInfo selColumnInfo)
        {
            DataGridViewCell curCell = dataGridView.CurrentCell;

            if (curCell != null && selColumnInfo != null)
            {
                if (selColumnInfo.IsText)
                {
                    if (curCell.EditedFormattedValue != null)
                        txtValue.Text = curCell.EditedFormattedValue.ToString();
                }
                else
                {
                    if (curCell.IsInEditMode)
                    {
                        if (dataGridView.EditingControl is ComboBox comboBox)
                            cbValue.SelectedValue = comboBox.SelectedValue;
                    }
                    else
                    {
                        cbValue.SelectedValue = curCell.Value;
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
                //
            }
            else if (columnInfo.IsText)
            {
                txtValue.Visible = true;
                cbValue.Visible = false;
            }
            else
            {
                txtValue.Visible = false;
                cbValue.Visible = true;

                cbValue.DataSource = columnInfo.DataSource1;
                cbValue.DisplayMember = columnInfo.DisplayMember;
                cbValue.ValueMember = columnInfo.ValueMember;
            }
        }


        private void FrmFilter_Load(object sender, EventArgs e)
        {
            if (currentFilter == null)
            {
                FillColumnList(dataGridView.CurrentCell?.OwningColumn.Name ?? "");
                SetDefaultFilter(cbColumn.SelectedItem as ColumnInfo);
            }
            else
            {
                FillColumnList(currentFilter.ColumnName);
            }
        }

        private void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustControls(cbColumn.SelectedItem as ColumnInfo);
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtValue.Text = "";
            dataTable.DefaultView.RowFilter = "";
            DialogResult = DialogResult.OK;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                dataTable.DefaultView.RowFilter = txtValue.Text;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ex.Message);
            }
        }
    }
}
