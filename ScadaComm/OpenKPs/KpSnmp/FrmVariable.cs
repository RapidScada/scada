/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : KpSnmp
 * Summary  : Variable creation or editing form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpSnmp
{
    /// <summary>
    /// Variable creation or editing form
    /// <para>Форма создания или редактирования переменной</para>
    /// </summary>
    internal partial class FrmVariable : Form
    {
        private Config.Variable variable; // созданная или редактируемая переменная


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmVariable()
        {
            InitializeComponent();

            variable = null;
        }


        /// <summary>
        /// Создать переменную
        /// </summary>
        /// <returns>Возвращает новую переменную или null в случае отмены</returns>
        public static Config.Variable CreateVariable()
        {
            FrmVariable frmVariable = new FrmVariable();
            frmVariable.ShowDialog();
            return frmVariable.variable;
        }

        /// <summary>
        /// Редактировать переменную
        /// </summary>
        /// <returns>Возвращает true, если переменная была изменена</returns>
        public static bool EditVariable(Config.Variable variable, int signal)
        {
            if (variable == null)
                throw new ArgumentNullException("variable");

            string oldName = variable.Name;
            string oldOID = variable.OID;

            FrmVariable frmVariable = new FrmVariable();
            frmVariable.variable = variable;
            frmVariable.txtSignal.Text = signal.ToString();

            return frmVariable.ShowDialog() == DialogResult.OK && !variable.Equals(oldName, oldOID);
        }


        private void FrmPhoneGroup_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Devices.KpSnmp.FrmVariable");

            // настройка элементов управления
            if (variable == null /*создание новой переменной*/)
            {
                btnAdd.Visible = true;
                btnAdd.Enabled = false;
                AcceptButton = btnAdd;
            }
            else
            {
                txtName.Text = variable.Name;
                txtOID.Text = variable.OID;
                btnChange.Left = btnAdd.Left;
                btnChange.Visible = true;
                AcceptButton = btnChange;
            }
        }

        private void txtNameOrOID_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = btnChange.Enabled = 
                txtName.Text.Trim() != "" && txtOID.Text.Trim() != "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // создание переменной
            variable = new Config.Variable() { Name = txtName.Text.Trim(), OID = txtOID.Text.Trim() };
            DialogResult = DialogResult.OK;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            // изменение переменной
            variable.Name = txtName.Text.Trim();
            variable.OID = txtOID.Text.Trim();
            DialogResult = DialogResult.OK;
        }
    }
}
