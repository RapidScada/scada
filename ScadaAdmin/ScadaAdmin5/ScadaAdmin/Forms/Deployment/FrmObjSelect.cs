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
 * Summary  : Form for selecting objects
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Form for selecting objects.
    /// <para>Форма для выбора объектов.</para>
    /// </summary>
    public partial class FrmObjSelect : Form
    {
        private readonly ConfigBase configBase; // the configuration database
        private DataTable dataTable;            // the table used by a grid view control


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmObjSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmObjSelect(ConfigBase configBase)
            : this()
        {
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
            dataTable = null;
            ObjNums = null;
        }


        /// <summary>
        /// Gets or sets the object numbers.
        /// </summary>
        public ICollection<int> ObjNums { get; set; }


        /// <summary>
        /// Shows available objects.
        /// </summary>
        private void ShowObjects()
        {
            // create table columns
            dataGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Select",
                HeaderText = "Select",
                DataPropertyName = "Select",
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            dataGridView.Columns.AddRange(new ColumnBuilder(configBase).CreateColumns(typeof(Obj)));

            // prepare table data
            dataTable = configBase.ObjTable.ToDataTable();

            foreach (DataColumn column in dataTable.Columns)
            {
                column.ReadOnly = true;
            }

            dataTable.Columns.Add("Select", typeof(bool));

            // select rows
            if (ObjNums != null)
            {
                HashSet<int> objNumSet = new HashSet<int>(ObjNums);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (objNumSet.Contains((int)dataRow["ObjNum"]))
                        dataRow["Select"] = true;
                }
            }

            // display data
            dataGridView.DataSource = dataTable;
            dataGridView.AutoSizeColumns();
        }


        private void FrmObjSelect_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            ShowObjects();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // get selected rows
            List<int> objNums = new List<int>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                if ((bool)dataRow["Select"])
                    objNums.Add((int)dataRow["ObjNum"]);
            }

            ObjNums = objNums;
            DialogResult = DialogResult.OK;
        }
    }
}
