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
 * Summary  : Form for editing a table of the configuration database that has a particular type
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for editing a table of the configuration database that has a particular type.
    /// <para>Форма редактирования таблицы базы конфигурации, которая имеет определенный тип.</para>
    /// </summary>
    public class FrmBaseTableGeneric<T> : FrmBaseTable, IChildForm
    {
        private readonly BaseTable<T> baseTable;  // the table being edited
        private readonly TableFilter tableFilter; // the table filter
        private readonly ConfigBase configBase;   // the configuration database
        private DataTable dataTable;              // the table used by a grid view control


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBaseTableGeneric(BaseTable<T> baseTable, TableFilter tableFilter, ConfigBase configBase, AppData appData)
            : base(appData)
        {
            this.baseTable = baseTable ?? throw new ArgumentNullException("baseTable");
            this.tableFilter = tableFilter;
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
            dataTable = null;
            Text = baseTable.Title + (tableFilter == null ? "" : " - " + tableFilter);
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Validates that the primary key value is unique.
        /// </summary>
        private bool PkUnique(int key, out string errMsg)
        {
            if (baseTable.PkExists(key))
            {
                errMsg = string.Format(AppPhrases.UniqueRequired, 
                    dataGridView.Columns[baseTable.PrimaryKey].HeaderText);
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Validates that no record refers the primary key.
        /// </summary>
        private bool NoReferencesToPk(int key, out string errMsg)
        {
            foreach (TableRelation relation in baseTable.Dependent)
            {
                if (relation.ChildTable.TryGetIndex(relation.ChildColumn, out TableIndex index) &&
                    index.IndexKeyExists(key))
                {
                    errMsg = string.Format(AppPhrases.KeyReferenced, relation.ChildTable.Title);
                    return false;
                }
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Validates that the primary key exists.
        /// </summary>
        private bool PkExists(IBaseTable parentTable, int key, string childColumn, out string errMsg)
        {
            if (parentTable.PkExists(key))
            {
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = string.Format(AppPhrases.DataNotExist, dataGridView.Columns[childColumn].HeaderText);
                return false;
            }
        }

        /// <summary>
        /// Deletes a row with the specified index if it is not referenced.
        /// </summary>
        private bool DeleteRow(DataView dataView, int rowIndex)
        {
            if (0 <= rowIndex && rowIndex < dataView.Count)
            {
                DataRowView rowView = dataView[rowIndex];

                if (!rowView.IsNew)
                {
                    int key = (int)rowView.Row[baseTable.PrimaryKey];

                    if (NoReferencesToPk(key, out string errMsg))
                    {
                        dataView.Delete(rowIndex);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Copies the changes from a one table to another.
        /// </summary>
        private void RetrieveChanges()
        {
            baseTable.RetrieveChanges(dataTable);
            baseTable.Modified = true;
            ChildFormTag.Modified = true;
        }

        /// <summary>
        /// Loads the table data.
        /// </summary>
        protected override void LoadTableData()
        {
            if (!configBase.Load(out string errMsg))
                appData.ProcError(errMsg);

            // create and setup a data table
            bindingSource.DataSource = null;
            ICollection<T> tableItems = tableFilter == null ?
                baseTable.Items.Values :
                baseTable.GetFilteredItems(tableFilter);
            dataTable = tableItems.ToDataTable();
            if (tableFilter != null)
                dataTable.Columns[tableFilter.ColumnName].DefaultValue = tableFilter.Value;
            dataTable.RowChanged += dataTable_RowChanged;
            dataTable.RowDeleted += dataTable_RowDeleted;
            bindingSource.DataSource = dataTable;

            // create grid columns
            ColumnBuilder columnBuilder = new ColumnBuilder(configBase);
            dataGridView.Columns.Clear();
            dataGridView.Columns.AddRange(columnBuilder.CreateColumns(baseTable.ItemType));
            dataGridView.AutoSizeColumns();

            ChildFormTag.Modified = baseTable.Modified;
        }

        /// <summary>
        /// Deletes the selected rows.
        /// </summary>
        protected override void DeleteSelectedRows()
        {
            try
            {
                bool notDeleted = false;

                try
                {
                    dataTable.RowDeleted -= dataTable_RowDeleted;
                    DataView dataView = dataTable.DefaultView;
                    DataGridViewSelectedRowCollection selectedRows = dataGridView.SelectedRows;

                    if (selectedRows.Count > 0)
                    {
                        for (int i = selectedRows.Count - 1; i >= 0; i--)
                        {
                            if (!DeleteRow(dataView, selectedRows[i].Index))
                                notDeleted = true;
                        }
                    }
                    else if (dataGridView.CurrentRow != null)
                    {
                        if (!DeleteRow(dataView, dataGridView.CurrentRow.Index))
                            notDeleted = true;
                    }
                }
                finally
                {
                    dataTable.RowDeleted += dataTable_RowDeleted;
                }

                RetrieveChanges();

                if (notDeleted)
                    ScadaUiUtils.ShowInfo(AppPhrases.RowsNotDeleted);
            }
            catch (Exception ex)
            {
                appData.ErrLog.WriteException(ex, AppPhrases.DataChangeError);
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Clears the table data.
        /// </summary>
        protected override void ClearTableData()
        {
            try
            {
                bool notDeleted = false;

                try
                {
                    bindingSource.DataSource = null;
                    dataTable.RowDeleted -= dataTable_RowDeleted;
                    DataView dataView = dataTable.DefaultView;

                    for (int rowIndex = dataView.Count - 1; rowIndex >= 0; rowIndex--)
                    {
                        if (!DeleteRow(dataView, rowIndex))
                            notDeleted = true;
                    }
                }
                finally
                {
                    dataTable.RowDeleted += dataTable_RowDeleted;
                    bindingSource.DataSource = dataTable;
                }

                RetrieveChanges();

                if (notDeleted)
                    ScadaUiUtils.ShowInfo(AppPhrases.RowsNotDeleted);
            }
            catch (Exception ex)
            {
                appData.ErrLog.WriteException(ex, AppPhrases.DataChangeError);
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Saves the table.
        /// </summary>
        public void Save()
        {
            if (configBase.SaveTable(baseTable, out string errMsg))
                ChildFormTag.Modified = false;
            else
                appData.ProcError(errMsg);
        }


        private void dataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;

            try
            {
                if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
                {
                    bool rowIsValid = true;
                    string errMsg = "";

                    if (e.Action == DataRowAction.Add)
                    {
                        int key = (int)row[baseTable.PrimaryKey];
                        rowIsValid = PkUnique(key, out errMsg);
                    }
                    else // DataRowAction.Change
                    {
                        int origKey = (int)row[baseTable.PrimaryKey, DataRowVersion.Original];
                        int curKey = (int)row[baseTable.PrimaryKey, DataRowVersion.Current];
                        if (origKey != curKey)
                            rowIsValid = PkUnique(curKey, out errMsg) && NoReferencesToPk(origKey, out errMsg);
                    }

                    // check the table dependencies
                    foreach (TableRelation relation in baseTable.DependsOn)
                    {
                        if (!row.IsNull(relation.ChildColumn))
                        {
                            int keyVal = (int)row[relation.ChildColumn];
                            if (!PkExists(relation.ParentTable, keyVal, relation.ChildColumn, out errMsg))
                            {
                                rowIsValid = false;
                                break;
                            }
                        }
                    }

                    if (rowIsValid)
                    {
                        RetrieveChanges();
                    }
                    else
                    {
                        row.RejectChanges();
                        ScadaUiUtils.ShowError(errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                row.RejectChanges();
                appData.ErrLog.WriteException(ex, AppPhrases.DataChangeError);
                ShowError(ex.Message);
            }
        }

        private void dataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;

            try
            {
                if (e.Action == DataRowAction.Delete)
                {
                    int key = (int)row[baseTable.PrimaryKey, DataRowVersion.Original];

                    if (NoReferencesToPk(key, out string errMsg))
                    {
                        RetrieveChanges();
                    }
                    else
                    {
                        row.RejectChanges();
                        ScadaUiUtils.ShowError(errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                row.RejectChanges();
                appData.ErrLog.WriteException(ex, AppPhrases.DataChangeError);
                ShowError(ex.Message);
            }
        }
    }
}
