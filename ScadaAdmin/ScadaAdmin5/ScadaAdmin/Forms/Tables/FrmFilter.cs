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
            public string ValueText { get; set; }
            public int ValueKey { get; set; }
            public bool ValueFlag { get; set; }

            public string GetRowFilter()
            {
                return "";
            }
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
            cbBoolean.Top = txtValue.Top;
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
        /// Gets information of the selected column.
        /// </summary>
        private ColumnInfo SelectedColumnInfo
        {
            get
            {
                return cbColumn.SelectedItem as ColumnInfo;
            }
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
        /// Adjusts the controls depending on the selected column.
        /// </summary>
        private void AdjustControls(ColumnInfo columnInfo)
        {
            cbStringOperation.Visible = false;
            cbMathOperation.Visible = false;
            txtValue.Visible = false;
            cbValue.Visible = false;
            cbBoolean.Visible = false;

            cbStringOperation.SelectedIndex = 0;
            cbMathOperation.SelectedIndex = 0;
            cbBoolean.SelectedIndex = 0;

            if (columnInfo != null)
            {
                if (columnInfo.IsText)
                {
                    cbStringOperation.Visible = true;
                    txtValue.Visible = true;
                }
                else if (columnInfo.IsComboBox)
                {
                    cbValue.Visible = true;
                    cbValue.DataSource = columnInfo.DataSource1;
                    cbValue.DisplayMember = columnInfo.DisplayMember;
                    cbValue.ValueMember = columnInfo.ValueMember;
                }
                else if (columnInfo.IsCheckBox)
                {
                    cbBoolean.Visible = true;
                }
            }
        }

        /// <summary>
        /// Sets the default filter.
        /// </summary>
        private void SetDefaultFilter(ColumnInfo columnInfo)
        {
            txtValue.Text = "";
            cbValue.SelectedIndex = -1;
            DataGridViewCell curCell = dataGridView.CurrentCell;

            if (columnInfo != null && curCell != null)
            {
                if (columnInfo.IsText)
                {
                    if (curCell.EditedFormattedValue != null)
                        txtValue.Text = curCell.EditedFormattedValue.ToString();
                }
                else if (columnInfo.IsComboBox)
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
                else if (columnInfo.IsCheckBox)
                {
                    cbBoolean.SelectedIndex = (bool)curCell.Value ? 1 : 0;
                }
            }
        }

        /// <summary>
        /// Sets the controls according to the current filter.
        /// </summary>
        private void DisplayCurrentFilter(ColumnInfo columnInfo)
        {
            if (columnInfo != null)
            {
                if (columnInfo.IsText)
                {
                    cbStringOperation.SelectedIndex = currentFilter.StringOperation;
                    txtValue.Text = currentFilter.ValueText;
                }
                else if (columnInfo.IsComboBox)
                {
                    cbStringOperation.SelectedIndex = 0;
                    try { cbValue.SelectedValue = currentFilter.ValueKey; }
                    catch { }
                }
                else if (columnInfo.IsCheckBox)
                {
                    cbStringOperation.SelectedIndex = 0;
                    cbBoolean.SelectedIndex = currentFilter.ValueFlag ? 1 : 0;
                }
            }
        }

        /// <summary>
        /// Creates a new filter according to the controls.
        /// </summary>
        private Filter CreateFilter(ColumnInfo columnInfo)
        {
            return new Filter();
        }



        private void FrmFilter_Load(object sender, EventArgs e)
        {
            if (currentFilter == null)
            {
                FillColumnList(dataGridView.CurrentCell?.OwningColumn.Name ?? "");
                SetDefaultFilter(SelectedColumnInfo);
            }
            else
            {
                FillColumnList(currentFilter.ColumnName);
                DisplayCurrentFilter(SelectedColumnInfo);
            }
        }

        private void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustControls(SelectedColumnInfo);
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            currentFilter = null;
            dataTable.DefaultView.RowFilter = "";
            DialogResult = DialogResult.OK;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                currentFilter = CreateFilter(SelectedColumnInfo);
                dataTable.DefaultView.RowFilter = currentFilter == null ? "" : currentFilter.GetRowFilter();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ex.Message);
            }
        }
    }
}
