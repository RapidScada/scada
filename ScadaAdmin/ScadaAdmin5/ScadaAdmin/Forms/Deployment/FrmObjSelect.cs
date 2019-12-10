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
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Form for selecting objects.
    /// <para>Форма для выбора объектов.</para>
    /// </summary>
    public partial class FrmObjSelect : Form
    {
        /// <summary>
        /// Represents an object that can be selected.
        /// <para>Представляет объект, который может быть выбран.</para>
        /// </summary>
        private class SelectableObject : Obj
        {
            public SelectableObject(Obj obj)
            {
                ObjNum = obj.ObjNum;
                Name = obj.Name;
                Descr = obj.Descr;
            }

            public bool Selected { get; set; }
        }

        private readonly ConfigBase configBase; // the configuration database
        private List<SelectableObject> objects; // the objects to select


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
            objects = null;
            ObjNums = null;
        }


        /// <summary>
        /// Gets or sets the numbers of selected objects.
        /// </summary>
        public ICollection<int> ObjNums { get; set; }


        /// <summary>
        /// Shows available objects.
        /// </summary>
        private void ShowObjects()
        {
            // create table columns
            List<DataGridViewColumn> columns = new ColumnBuilder(configBase).CreateColumns(typeof(Obj)).ToList();
            columns.ForEach(column => column.ReadOnly = true);
            columns.Insert(0, new DataGridViewCheckBoxColumn
            {
                Name = "Selected",
                HeaderText = AppPhrases.SelectedColumn,
                DataPropertyName = "Selected",
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            // prepare table data
            HashSet<int> objNumSet = ObjNums == null ? 
                new HashSet<int>() : 
                new HashSet<int>(ObjNums);
            objects = new List<SelectableObject>();

            foreach (Obj obj in configBase.ObjTable.Items.Values)
            {
                objects.Add(new SelectableObject(obj)
                {
                    Selected = objNumSet.Contains(obj.ObjNum)
                });
            }

            // display data
            if (ScadaUtils.IsRunningOnMono)
            {
                bindingSource.DataSource = objects;
                dataGridView.Columns.AddRange(columns.ToArray());
            }
            else
            {
                dataGridView.Columns.AddRange(columns.ToArray());
                bindingSource.DataSource = objects;
            }

            dataGridView.AutoSizeColumns();
        }

        /// <summary>
        /// Applies the object filter.
        /// </summary>
        private void ApplyFilter()
        {
            string filterText = txtFilter.Text.Trim();
            bindingSource.DataSource = filterText == "" ?
                objects :
                objects.Where(obj => StringContains(obj.Name, filterText) || StringContains(obj.Descr, filterText));
        }

        /// <summary>
        /// Determines whether the string contains the specified text.
        /// </summary>
        private bool StringContains(string s, string text)
        {
            return (s ?? "").IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }


        private void FrmObjSelect_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            ShowObjects();
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ApplyFilter();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // get selected objects
            ObjNums = (from obj in objects
                       where obj.Selected
                       select obj.ObjNum).ToArray();

            DialogResult = DialogResult.OK;
        }
    }
}
