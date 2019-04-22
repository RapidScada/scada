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
 * Summary  : The class contains utility methods for the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// The class contains utility methods for the application.
    /// <para>Класс, содержащий вспомогательные методы приложения.</para>
    /// </summary>
    internal static class AppUtils
    {
        /// <summary>
        /// Opens the specified file in the default text editor.
        /// </summary>
        public static void OpenTextFile(string fileName)
        {
            Process.Start(fileName);
        }

        /// <summary>
        /// Sets the check box state according to the cell value.
        /// </summary>
        public static void SetChecked(this CheckBox checkBox, DataGridViewCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException("cell");
            checkBox.Checked = (bool)cell.Value;
        }

        /// <summary>
        /// Sets the text box according to the cell value.
        /// </summary>
        public static void SetText(this TextBox textBox, DataGridViewCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException("cell");
            textBox.Text = cell.Value == null ? "" : cell.Value.ToString();
        }

        /// <summary>
        /// Sets the combp box value according to the cell value.
        /// </summary>
        public static void SetValue(this ComboBox comboBox, DataGridViewCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException("cell");

            if (cell.OwningColumn is DataGridViewComboBoxColumn comboBoxColumn)
            {
                comboBox.DisplayMember = comboBoxColumn.DisplayMember;
                comboBox.ValueMember = comboBoxColumn.ValueMember;
                comboBox.DataSource = comboBoxColumn.DataSource;
                comboBox.SelectedValue = cell.Value;
            }
        }

        /// <summary>
        /// Adds an empty row to the table.
        /// </summary>
        public static void AddEmptyRow(this DataTable dataTable, int id = 0, string text = " ")
        {
            DataRow emptyRow = dataTable.NewRow();
            emptyRow[0] = id;
            emptyRow[1] = text;
            dataTable.Rows.Add(emptyRow);
        }

        /// <summary>
        /// Appends the error to the string builder.
        /// </summary>
        public static void AppendError(this StringBuilder stringBuilder, Label label, string text)
        {
            if (label == null)
                throw new ArgumentNullException("label");

            stringBuilder.Append(label.Text).Append(": ").AppendLine(text);
        }
    }
}
