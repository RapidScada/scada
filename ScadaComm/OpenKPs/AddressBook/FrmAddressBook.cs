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
 * Module   : AddressBook
 * Summary  : Address book form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.AddressBook
{
    /// <summary>
    /// Address book form
    /// <para>Форма адресной книги</para>
    /// </summary>
    public partial class FrmAddressBook : Form
    {
        private AppDirs appDirs;         // директории приложения
        private AddressBook addressBook; // адресная книга
        private bool modified;           // признак изменения адресной книги
        private TreeNode rootNode;       // корневой узел дерева


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmAddressBook()
        {
            InitializeComponent();

            appDirs = null;
            addressBook = new AddressBook();
            modified = false;
            rootNode = treeView.Nodes["rootNode"];
        }


        /// <summary>
        /// Получить или установить признак изменения адресной книги
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
            }
        }


        /// <summary>
        /// Построить дерево адресной книги
        /// </summary>
        private void BuildTree()
        {
            try
            {
                treeView.BeginUpdate();
                rootNode.Nodes.Clear();

                foreach (AddressBook.ContactGroup contactGroup in addressBook.ContactGroups)
                    rootNode.Nodes.Add(CreateContactGroupNode(contactGroup));

                rootNode.Expand();

                if (rootNode.Nodes.Count > 0)
                    treeView.SelectedNode = rootNode.Nodes[0];
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Создать узел дерева для группы контактов
        /// </summary>
        private TreeNode CreateContactGroupNode(AddressBook.ContactGroup contactGroup)
        {
            string imageKey = contactGroup.Contacts.Count > 0 ? "folder_open.png" : "folder_closed.png";
            TreeNode contactGroupNode = TreeViewUtils.CreateNode(contactGroup, imageKey, true);

            foreach (AddressBook.Contact contact in contactGroup.Contacts)
                contactGroupNode.Nodes.Add(CreateContactNode(contact));

            return contactGroupNode;
        }

        /// <summary>
        /// Создать узел дерева для контакта
        /// </summary>
        private TreeNode CreateContactNode(AddressBook.Contact contact)
        {
            TreeNode contactNode = TreeViewUtils.CreateNode(contact, "contact.png", true);

            foreach (AddressBook.PhoneNumber phoneNumber in contact.PhoneNumbers)
                contactNode.Nodes.Add(CreatePhoneNumberNode(phoneNumber));

            foreach (AddressBook.Email email in contact.Emails)
                contactNode.Nodes.Add(CreateEmailNode(email));

            return contactNode;
        }

        /// <summary>
        /// Создать узел дерева для телефонного номера
        /// </summary>
        private TreeNode CreatePhoneNumberNode(AddressBook.PhoneNumber phoneNumber)
        {
            return TreeViewUtils.CreateNode(phoneNumber, "phone.png");
        }

        /// <summary>
        /// Создать узел дерева для адреса электронной почты
        /// </summary>
        private TreeNode CreateEmailNode(AddressBook.Email email)
        {
            return TreeViewUtils.CreateNode(email, "email.png");
        }

        /// <summary>
        /// Установить доступность кнопок
        /// </summary>
        private void SetButtonsEnabled()
        {
            /*object selObj = GetSelectedObject();
            btnEdit.Enabled = btnDelete.Enabled = selObj != null;
            btnCutNumber.Enabled = btnCopyNumber.Enabled = selObj is Phonebook.PhoneNumber;
            btnPasteNumber.Enabled = copiedNumber != null;*/
        }

        
        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(AppDirs appDirs)
        {
            if (appDirs == null)
                throw new ArgumentNullException("appDirs");

            FrmAddressBook frmAddressBook = new FrmAddressBook();
            frmAddressBook.appDirs = appDirs;
            frmAddressBook.ShowDialog();
        }


        private void FrmAddressBook_Load(object sender, EventArgs e)
        {
            // локализация библиотеки
            string errMsg;
            if (!Localization.UseRussian)
            {
                if (Localization.LoadDictionaries(appDirs.LangDir, "AddressBook", out errMsg))
                {
                    Translator.TranslateForm(this, "Scada.Comm.Devices.AddressBook.FrmAddressBook");
                    //KpPhrases.Init();
                    //rootNode.Text = KpPhrases.PhonebookNode;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            // загрузка адресной книги
            string fileName = appDirs.ConfigDir + AddressBook.DefFileName;
            if (File.Exists(fileName) && !addressBook.Load(fileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);
            Modified = false;

            // вывод дерева адресной книги
            BuildTree();

            // установка доступности кнопок
            SetButtonsEnabled();
        }

        private void FrmAddressBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show("???", // !!! KpPhrases.SavePhonebookConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        string errMsg;
                        if (!addressBook.Save(appDirs.ConfigDir + AddressBook.DefFileName, out errMsg))
                        {
                            ScadaUiUtils.ShowError(errMsg);
                            e.Cancel = true;
                        }
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }


        private void btnAddContactGroup_Click(object sender, EventArgs e)
        {
            // создание группы телефонных номеров
            /*Phonebook.PhoneGroup newGroup = FrmPhoneGroup.CreatePhoneGroup();
            if (newGroup != null)
            {
                if (addressBook.PhoneGroups.ContainsKey(newGroup.Name))
                {
                    ScadaUiUtils.ShowWarning(KpPhrases.PhoneGroupExists);
                }
                else
                {
                    InsertGroup(newGroup);
                    Modified = true;
                }
            }*/
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {

        }

        private void btnAddPhoneNumber_Click(object sender, EventArgs e)
        {
            // создание телефонного номера
            /*Phonebook.PhoneGroup group;
            TreeNode groupNode;
            GetCurrentGroup(out group, out groupNode);

            if (group != null)
            {
                Phonebook.PhoneNumber newNumber = FrmPhoneNumber.CreatePhoneNumber();
                InsertOrUpdateNumber(group, groupNode, newNumber);
            }*/
        }

        private void btnAddEmail_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;
            if (selectedNode.IsEditing)
            {
                selectedNode.EndEdit(false);
            }
            else
            {
                selectedNode.BeginEdit();
            }

            /*
            // редактирование выбранного объекта
            object selObj = GetSelectedObject();
            Phonebook.PhoneGroup group = selObj as Phonebook.PhoneGroup;
            Phonebook.PhoneNumber number = selObj as Phonebook.PhoneNumber;

            if (group != null)
            {
                // редактирование группы телефонных номеров
                Phonebook.PhoneGroup newGroup = FrmPhoneGroup.EditPhoneGroup(group);
                if (newGroup != null && !group.Equals(newGroup))
                {
                    RemoveGroup(group);
                    InsertGroup(newGroup);
                    Modified = true;
                }
            }
            else if (number != null)
            {
                // редактирование телефонного номера
                TreeNode groupNode;
                GetCurrentGroup(out group, out groupNode);

                if (group != null)
                {
                    Phonebook.PhoneNumber newNumber = FrmPhoneNumber.EditPhoneNumber(number);
                    if (newNumber != null && !number.Equals(newNumber))
                    {
                        RemoveNumber(group, groupNode, number);
                        InsertNumber(group, groupNode, newNumber);
                        Modified = true;
                    }
                }
            }*/
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {/*
            // удаление выбранного объекта
            object selObj = GetSelectedObject();
            Phonebook.PhoneGroup group = selObj as Phonebook.PhoneGroup;
            Phonebook.PhoneNumber number = selObj as Phonebook.PhoneNumber;

            if (group != null)
            {
                // удаление группы телефонных номеров
                RemoveGroup(group);
                Modified = true;
            }
            else if (number != null)
            {
                // удаление телефонного номера
                TreeNode phoneGroupNode;
                GetCurrentGroup(out group, out phoneGroupNode);
                if (group != null)
                {
                    RemoveNumber(group, phoneGroupNode, number);
                    Modified = true;
                }
            }*/
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // установка доступности кнопок
            SetButtonsEnabled();
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // установить иконку, если группа была развёрнута
            if (e.Node.Tag is AddressBook.ContactGroup)
                e.Node.SetImageKey("folder_open.png");
        }

        private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // установить иконку, если группа была свёрнута
            if (e.Node.Tag is AddressBook.ContactGroup)
                e.Node.SetImageKey("folder_closed.png");
        }

        private void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // запрет редактирования корневого узла дерева
            if (e.Node == rootNode)
                e.CancelEdit = true;
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение адресной книги
            string errMsg;
            if (addressBook.Save(appDirs.ConfigDir + AddressBook.DefFileName, out errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
