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
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Data;
using System.Globalization;
using System.Text;
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
        /// Represents an extended filter.
        /// <para>Представляет расширенный фильтр.</para>
        /// </summary>
        private class FilterExtended : TableFilter
        {
            private static readonly string[] MathOperations = { "=", "<>", "<", "<=", ">", ">=" };
            public int StringOperation { get; set; }
            public int MathOperation { get; set; }

            public string GetRowFilter()
            {
                if (Value == null)
                {
                    return "";
                }
                else if (Value is string)
                {
                    return string.Format(StringOperation == 0 ?
                        "{0} = '{1}'" :
                        "{0} like '%{1}%'", ColumnName, Value);
                }
                else
                {
                    string valStr = Value is double valDbl ?
                        valDbl.ToString(CultureInfo.InvariantCulture) :
                        Value.ToString();

                    return string.Format("{0} {1} {2}", ColumnName, MathOperations[MathOperation], valStr);
                }
            }
        }

        private readonly DataGridView dataGridView;
        private FilterExtended currentFilter;


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
        public FrmFilter(DataGridView dataGridView)
            : this()
        {
            this.dataGridView = dataGridView ?? throw new ArgumentNullException("dataGridView");
            currentFilter = null;
            DataTable = null;
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
        /// Gets or sets the data table being filtered.
        /// </summary>
        public DataTable DataTable { get; set; }

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
            cbStringOperation.SelectedIndex = 0;

            cbMathOperation.Visible = false;
            cbMathOperation.Enabled = true;
            cbMathOperation.SelectedIndex = 0;

            txtValue.Visible = false;
            txtValue.Text = "";

            cbValue.Visible = false;
            cbValue.DataSource = null;

            cbBoolean.Visible = false;
            cbBoolean.SelectedIndex = 0;

            if (columnInfo != null)
            {
                if (columnInfo.IsText)
                {
                    if (columnInfo.IsNumber)
                        cbMathOperation.Visible = true;
                    else
                        cbStringOperation.Visible = true;

                    txtValue.Visible = true;
                }
                else if (columnInfo.IsComboBox)
                {
                    cbMathOperation.Visible = true;
                    cbMathOperation.Enabled = false;

                    cbValue.Visible = true;
                    cbValue.DataSource = columnInfo.DataSource1;
                    cbValue.DisplayMember = columnInfo.DisplayMember;
                    cbValue.ValueMember = columnInfo.ValueMember;
                }
                else if (columnInfo.IsCheckBox)
                {
                    cbMathOperation.Visible = true;
                    cbMathOperation.Enabled = false;
                    cbBoolean.Visible = true;
                }
            }
        }

        /// <summary>
        /// Sets the default filter.
        /// </summary>
        private void SetDefaultFilter(ColumnInfo columnInfo)
        {
            DataGridViewCell curCell = dataGridView.CurrentCell;

            if (columnInfo != null && curCell != null)
            {
                if (columnInfo.IsText)
                {
                    txtValue.Text = curCell.Value?.ToString() ?? "";
                }
                else if (columnInfo.IsComboBox)
                {
                    cbValue.SelectedValue = curCell.Value;
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
                    cbMathOperation.SelectedIndex = currentFilter.MathOperation;
                    txtValue.Text = currentFilter.Value?.ToString() ?? "";
                }
                else if (columnInfo.IsComboBox)
                {
                    cbStringOperation.SelectedIndex = 0;
                    try { cbValue.SelectedValue = (int)currentFilter.Value; }
                    catch { }
                }
                else if (columnInfo.IsCheckBox)
                {
                    cbStringOperation.SelectedIndex = 0;
                    cbBoolean.SelectedIndex = currentFilter.Value is bool val && val ? 1 : 0;
                }
            }
        }

        /// <summary>
        /// Creates a new filter according to the controls.
        /// </summary>
        private FilterExtended CreateFilter(ColumnInfo columnInfo)
        {
            FilterExtended filter = new FilterExtended
            {
                ColumnName = columnInfo.Column.Name,
                StringOperation = cbStringOperation.SelectedIndex,
                MathOperation = cbMathOperation.SelectedIndex
            };

            if (columnInfo.IsText)
                filter.Value = columnInfo.IsNumber ? (object)ScadaUtils.ParseDouble(txtValue.Text) : txtValue.Text;
            else if (columnInfo.IsComboBox)
                filter.Value = (cbValue.SelectedValue is int val) ? val : -1;
            else if (columnInfo.IsCheckBox)
                filter.Value = cbBoolean.SelectedIndex > 0;

            return filter;
        }



        private void FrmFilter_Load(object sender, EventArgs e)
        {
            if (DataTable == null)
                throw new InvalidOperationException("DataTable must not be null.");

            ActiveControl = cbColumn;

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
            DataTable.DefaultView.RowFilter = "";
            DialogResult = DialogResult.OK;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                currentFilter = CreateFilter(SelectedColumnInfo);
                DataTable.DefaultView.RowFilter = currentFilter == null ? "" : currentFilter.GetRowFilter();
                DialogResult = DialogResult.OK;
            }
            catch
            {
                ScadaUiUtils.ShowError(AppPhrases.IncorrectTableFilter);
            }
        }
    }
}
