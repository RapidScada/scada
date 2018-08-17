/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Modified : 2018
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using System;
using System.Data;
using WinControl;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for editing a table of the configuration database that has a particular type.
    /// <para>Форма редактирования таблицы базы конфигурации, которая имеет определенный тип.</para>
    /// </summary>
    public class FrmBaseTableGeneric<T> : FrmBaseTable, IChildForm
    {
        private readonly ScadaProject project;   // the project under development
        private readonly BaseTable<T> baseTable; // the table being edited
        private DataTable dataTable;             // the table used by a grid view control


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBaseTableGeneric(AppData appData, ScadaProject project, BaseTable<T> baseTable)
            : base(appData)
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.baseTable = baseTable ?? throw new ArgumentNullException("baseTable");
            dataTable = null;
            Text = baseTable.Title;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Loads the table data.
        /// </summary>
        protected override void LoadTableData()
        {
            base.LoadTableData();

            if (!project.ConfigBase.Load(out string errMsg))
                appData.ProcError(errMsg);

            dataTable = baseTable.ToDataTable();
            dataTable.RowChanged += dataTable_RowChanged;
            dataTable.RowDeleted += dataTable_RowDeleted;
            bindingSource.DataSource = dataTable;

            ColumnBuilder columnBuilder = new ColumnBuilder(project.ConfigBase);
            dataGridView.Columns.AddRange(columnBuilder.CreateColumns(baseTable.ItemType));
            dataGridView.AutoResizeColumns();
        }

        /// <summary>
        /// Copies the changes from a one table to another.
        /// </summary>
        private void RetrieveChanges()
        {
            baseTable.RetrieveChanges(dataTable);
            ChildFormTag.Modified = true;
        }

        /// <summary>
        /// Saves the table.
        /// </summary>
        public void Save()
        {
            if (baseTable.Save(project.ConfigBase.BaseDir, out string errMsg))
                ChildFormTag.Modified = false;
            else
                appData.ProcError(errMsg);
        }


        private void dataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
                RetrieveChanges();
        }

        private void dataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Delete)
                RetrieveChanges();
        }
    }
}
