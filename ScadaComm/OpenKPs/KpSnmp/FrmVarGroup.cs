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
 * Summary  : Variable group creation or editing form
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
    /// Variable group creation or editing form
    /// <para>Форма создания или редактирования группы переменных</para>
    /// </summary>
    internal partial class FrmVarGroup : Form
    {
        private KpConfig.VarGroup varGroup; // созданная или редактируемая группа переменных


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmVarGroup()
        {
            InitializeComponent();
            varGroup = null;
        }


        /// <summary>
        /// Создать группу переменных
        /// </summary>
        /// <returns>Возвращает новую группу переменных или null в случае отмены</returns>
        public static KpConfig.VarGroup CreateVarGroup()
        {
            FrmVarGroup frmVarGroup = new FrmVarGroup();
            frmVarGroup.ShowDialog();
            return frmVarGroup.varGroup;
        }

        /// <summary>
        /// Редактировать группу переменных
        /// </summary>
        /// <returns>Возвращает true, если группа переменных была изменена</returns>
        public static bool EditVarGroup(KpConfig.VarGroup varGroup)
        {
            if (varGroup == null)
                throw new ArgumentNullException("varGroup");

            string oldName = varGroup.Name;
            FrmVarGroup frmVarGroup = new FrmVarGroup();
            frmVarGroup.varGroup = varGroup;
            return frmVarGroup.ShowDialog() == DialogResult.OK && !varGroup.Equals(oldName);
        }


        private void FrmPhoneGroup_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Devices.KpSnmp.FrmVarGroup");

            // настройка элементов управления
            if (varGroup == null /*создание новой группы*/)
            {
                btnAdd.Visible = true;
                btnAdd.Enabled = false;
                AcceptButton = btnAdd;
            }
            else
            {
                txtName.Text = varGroup.Name;
                btnChange.Left = btnAdd.Left;
                btnChange.Visible = true;
                AcceptButton = btnChange;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = btnChange.Enabled = txtName.Text.Trim() != "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // создание группы переменных
            varGroup = new KpConfig.VarGroup() { Name = txtName.Text.Trim() };
            DialogResult = DialogResult.OK;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            // изменение имени группы переменных
            varGroup.Name = txtName.Text.Trim();
            DialogResult = DialogResult.OK;
        }
    }
}
