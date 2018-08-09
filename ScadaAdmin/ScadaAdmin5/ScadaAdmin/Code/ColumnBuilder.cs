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
 * Module   : ScadaAdminCommon
 * Summary  : Creates columns for a DataGridView control
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Data.Models;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Creates columns for a DataGridView control
    /// <para>Создает столбцы для элемента управления DataGridView</para>
    /// </summary>
    internal class ColumnBuilder
    {
        /// <summary>
        /// Creates a new column that hosts text cells.
        /// </summary>
        private DataGridViewTextBoxColumn NewTextBoxColumn(string dataPropertyName)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName
            };
        }

        /// <summary>
        /// Creates columns for the object table.
        /// </summary>
        private DataGridViewColumn[] CreateObjTableColumns()
        {
            return new DataGridViewColumn[]
            {
                    NewTextBoxColumn("ObjNum"),
                    NewTextBoxColumn("Name"),
                    NewTextBoxColumn("Descr")
            };
        }


        /// <summary>
        /// Creates columns for the specified table
        /// </summary>
        public DataGridViewColumn[] CreateColumns(Type itemType)
        {
            if (itemType == typeof(Obj))
                return CreateObjTableColumns();
            else
                return new DataGridViewColumn[0];
        }
    }
}
