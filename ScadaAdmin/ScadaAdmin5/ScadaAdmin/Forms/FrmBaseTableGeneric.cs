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
using System.IO;
using WinControl;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for editing a table of the configuration database that has a particular type.
    /// <para>Форма редактирования таблицы базы конфигурации, которая имеет определенный тип.</para>
    /// </summary>
    public class FrmBaseTableGeneric<T> : FrmBaseTable, IChildForm
    {
        private readonly AppData appData;        // the common data of the application
        private readonly ScadaProject project;   // the project under development
        private readonly BaseTable<T> baseTable; // the table being edited


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmBaseTableGeneric(AppData appData, ScadaProject project, BaseTable<T> baseTable)
            : base()
        {
            this.appData = appData ?? throw new ArgumentNullException("appData");
            this.project = project ?? throw new ArgumentNullException("project");
            this.baseTable = baseTable ?? throw new ArgumentNullException("baseTable");
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Performs actions on load.
        /// </summary>
        protected override void LoadForm()
        {
            base.LoadForm();
            LoadBaseTable();
            Text = baseTable.Title;
            bindingSource.DataSource = baseTable.Items.ToDataTable();
            ColumnBuilder columnBuilder = new ColumnBuilder(project.ConfigBase);
            dataGridView.Columns.AddRange(columnBuilder.CreateColumns(baseTable.ItemType));
            dataGridView.AutoResizeColumns();
        }

        /// <summary>
        /// Loads a table from file.
        /// </summary>
        private void LoadBaseTable()
        {
            try
            {
                string fileName = baseTable.GetFileName(project.ConfigBase.BaseDir);

                if (File.Exists(fileName))
                    baseTable.Load(fileName);
            }
            catch (Exception ex)
            {
                appData.ProcError(ex.Message); // TODO: message
            }
        }

        /// <summary>
        /// Saves the table.
        /// </summary>
        public void Save()
        {
            try
            {
                string fileName = baseTable.GetFileName(project.ConfigBase.BaseDir);
                baseTable.Save(fileName);
            }
            catch (Exception ex)
            {
                appData.ProcError(ex.Message); // TODO: message
            }
        }
    }
}
