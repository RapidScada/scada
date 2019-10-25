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
 * Summary  : Represents information associated with a column
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Data;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Represents information associated with a column.
    /// <para>Представляет информацию, связанную со столбцом.</para>
    /// </summary>
    internal class ColumnInfo
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
        /// Gets a value indicating whether the text column contains numbers.
        /// </summary>
        public bool IsNumber
        {
            get
            {
                return IsText && (Column.ValueType == typeof(int) || Column.ValueType == typeof(double));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the column contains combo boxes.
        /// </summary>
        public bool IsComboBox
        {
            get
            {
                return Column is DataGridViewComboBoxColumn;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the column contains check boxes.
        /// </summary>
        public bool IsCheckBox
        {
            get
            {
                return Column is DataGridViewCheckBoxColumn;
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
}
