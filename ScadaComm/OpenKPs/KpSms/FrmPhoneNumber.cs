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
 * Summary  : Phone number creation or editing form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpSms
{
    /// <summary>
    /// Phone number creation or editing form
    /// <para>Форма создания или редактирования телефонного номера</para>
    /// </summary>
    internal partial class FrmPhoneNumber : Form
    {
        private Phonebook.PhoneNumber newNumber; // созданный телефонный номер
        private Phonebook.PhoneNumber oldNumber; // редактируемый телефонный номер

        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmPhoneNumber()
        {
            InitializeComponent();

            newNumber = null;
            oldNumber = null;
        }


        /// <summary>
        /// Создать телефонный номер
        /// </summary>
        /// <returns>Возвращает новый телефонный номер или null в случае отмены</returns>
        public static Phonebook.PhoneNumber CreatePhoneNumber()
        {
            FrmPhoneNumber frmPhoneGroup = new FrmPhoneNumber();
            frmPhoneGroup.ShowDialog();
            return frmPhoneGroup.newNumber;
        }

        /// <summary>
        /// Редактировать телефонный номер
        /// </summary>
        /// <returns>Возвращает новый телефонный номер, созданный на основе заданного, 
        /// или null в случае отмены</returns>
        public static Phonebook.PhoneNumber EditPhoneNumber(Phonebook.PhoneNumber phoneNumber)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException("phoneNumber");

            FrmPhoneNumber frmPhoneGroup = new FrmPhoneNumber();
            frmPhoneGroup.oldNumber = phoneNumber;
            frmPhoneGroup.ShowDialog();
            return frmPhoneGroup.newNumber;
        }


        private void FrmPhoneGroup_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Devices.KpSms.FrmPhoneNumber");

            // настройка элементов управления
            if (oldNumber == null)
            {
                btnCreate.Visible = true;
                btnCreate.Enabled = false;
                AcceptButton = btnCreate;
            }
            else
            {
                txtNumber.Text = oldNumber.Number;
                txtName.Text = oldNumber.Name;
                btnChange.Left = btnCreate.Left;
                btnChange.Visible = true;
                AcceptButton = btnChange;
            }
        }

        private void txtNumber_TextChanged(object sender, EventArgs e)
        {
            btnCreate.Enabled = btnChange.Enabled = txtNumber.Text.Trim() != "";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // создание телефонного номера
            newNumber = new Phonebook.PhoneNumber(txtNumber.Text.Trim(), txtName.Text.Trim());
            DialogResult = DialogResult.OK;
        }
    }
}
