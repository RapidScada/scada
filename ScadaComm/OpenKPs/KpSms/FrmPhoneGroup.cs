/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : KpSms
 * Summary  : Phone group creation or editing form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpSms
{
    /// <summary>
    /// Phone group creation or editing form
    /// <para>Форма создания или редактирования группы телефонных номеров</para>
    /// </summary>
    internal partial class FrmPhoneGroup : Form
    {
        private Phonebook.PhoneGroup newGroup; // созданная группа телефонных номеров
        private Phonebook.PhoneGroup oldGroup; // редактируемая группа телефонных номеров

        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmPhoneGroup()
        {
            InitializeComponent();

            newGroup = null;
            oldGroup = null;
        }


        /// <summary>
        /// Создать группу телефонных номеров
        /// </summary>
        /// <returns>Возвращает новую группу телефонных номеров или null в случае отмены</returns>
        public static Phonebook.PhoneGroup CreatePhoneGroup()
        {
            FrmPhoneGroup frmPhoneGroup = new FrmPhoneGroup();
            frmPhoneGroup.ShowDialog();
            return frmPhoneGroup.newGroup;
        }

        /// <summary>
        /// Редактировать группу телефонных номеров
        /// </summary>
        /// <returns>Возвращает новую группу телефонных номеров, созданную на основе заданной, 
        /// или null в случае отмены</returns>
        public static Phonebook.PhoneGroup EditPhoneGroup(Phonebook.PhoneGroup phoneGroup)
        {
            if (phoneGroup == null)
                throw new ArgumentNullException("phoneGroup");

            FrmPhoneGroup frmPhoneGroup = new FrmPhoneGroup();
            frmPhoneGroup.oldGroup = phoneGroup;
            frmPhoneGroup.ShowDialog();
            return frmPhoneGroup.newGroup;
        }


        private void FrmPhoneGroup_Load(object sender, EventArgs e)
        {
            // перевод формы
            Localization.TranslateForm(this, "Scada.Comm.Devices.KpSms.FrmPhoneGroup");

            // настройка элементов управления
            if (oldGroup == null)
            {
                btnCreate.Visible = true;
                AcceptButton = btnCreate;
            }
            else
            {
                txtName.Text = oldGroup.Name;
                btnChange.Left = btnCreate.Left;
                btnChange.Visible = true;
                AcceptButton = btnChange;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnCreate.Enabled = btnChange.Enabled = txtName.Text.Trim() != "";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // создание группы телефонных номеров
            newGroup = new Phonebook.PhoneGroup(txtName.Text.Trim());
            DialogResult = DialogResult.OK;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            // создание группы телефонных номеров и копирование в неё телефонов
            newGroup = new Phonebook.PhoneGroup(txtName.Text.Trim());

            foreach (Phonebook.PhoneNumber phoneNumber in oldGroup.PhoneNumbers.Values)
                newGroup.PhoneNumbers.Add(phoneNumber.Name, phoneNumber);

            DialogResult = DialogResult.OK;
        }
    }
}
