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

using Scada.Admin.Project;
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Creates columns for a DataGridView control
    /// <para>Создает столбцы для элемента управления DataGridView</para>
    /// </summary>
    internal class ColumnBuilder
    {
        private static DataGridViewTextBoxColumn NewTextBoxColumn(string dataPropertyName)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName
            };
        }

        public DataGridViewColumn[] CreateColumns<T>(BaseTable<T> baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException("table");

            if (typeof(T) == typeof(Obj))
            {
                return new DataGridViewColumn[]
                {
                    NewTextBoxColumn("ObjNum"),
                    NewTextBoxColumn("Name"),
                    NewTextBoxColumn("Descr")
                };
            }
            else
            {
                return new DataGridViewColumn[0];
            }
        }
    }
}
