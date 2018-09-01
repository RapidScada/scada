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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;

namespace Scada.Comm.Devices.AB
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
            rootNode.Tag = addressBook;
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

            foreach (AddressBook.ContactRecord contactRecord in contact.ContactRecords)
            {
                if (contactRecord is AddressBook.PhoneNumber)
                    contactNode.Nodes.Add(CreatePhoneNumberNode(contactRecord));
                else if (contactRecord is AddressBook.Email)
                    contactNode.Nodes.Add(CreateEmailNode(contactRecord));
            }

            return contactNode;
        }

        /// <summary>
        /// Создать узел дерева для телефонного номера
        /// </summary>
        private TreeNode CreatePhoneNumberNode(AddressBook.ContactRecord phoneNumber)
        {
            return TreeViewUtils.CreateNode(phoneNumber, "phone.png");
        }

        /// <summary>
        /// Создать узел дерева для адреса электронной почты
        /// </summary>
        private TreeNode CreateEmailNode(AddressBook.ContactRecord email)
        {
            return TreeViewUtils.CreateNode(email, "email.png");
        }

        /// <summary>
        /// Найти индекс вставки элемента для сохранения упорядоченности списка
        /// </summary>
        private int FindInsertIndex<T>(List<T> list, int currentIndex, out bool duplicated)
        {
            if (list.Count < 2)
            {
                duplicated = false;
                return currentIndex;
            }
            else
            {
                T item = list[currentIndex];

                list.RemoveAt(currentIndex);
                int newIndex = list.BinarySearch(item);
                list.Insert(currentIndex, item);

                if (newIndex >= 0)
                {
                    duplicated = true;
                    return newIndex;
                }
                else
                {
                    duplicated = false;
                    return ~newIndex;
                }
            }
        }

        /// <summary>
        /// Установить доступность кнопок
        /// </summary>
        private void SetButtonsEnabled()
        {
            object selObj = treeView.GetSelectedObject();
            btnAddContact.Enabled = btnEdit.Enabled = btnDelete.Enabled = 
                selObj is AddressBook.AddressBookItem;
            btnAddPhoneNumber.Enabled = btnAddEmail.Enabled = 
                selObj is AddressBook.Contact || selObj is AddressBook.ContactRecord;
        }

        /// <summary>
        /// Проверить корректность формата адреса электронной почты
        /// </summary>
        private bool CheckEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
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
                    Translator.TranslateForm(this, "Scada.Comm.Devices.AB.FrmAddressBook");
                    AbPhrases.Init();
                    rootNode.Text = AbPhrases.AddressBookNode;
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
                DialogResult result = MessageBox.Show(AbPhrases.SavePhonebookConfirm,
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
            // добавление группы контактов
            AddressBook.ContactGroup contactGroup = new AddressBook.ContactGroup(AbPhrases.NewContactGroup);
            TreeNode contactGroupNode = CreateContactGroupNode(contactGroup);

            treeView.Add(rootNode, contactGroupNode);
            contactGroupNode.BeginEdit();
            Modified = true;
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            // добавление контакта
            TreeNode contactGroupNode = treeView.SelectedNode?.FindClosest(typeof(AddressBook.ContactGroup));
            if (contactGroupNode != null)
            {
                AddressBook.Contact contact = new AddressBook.Contact(AbPhrases.NewContact);
                TreeNode contactNode = CreateContactNode(contact);

                treeView.Add(contactGroupNode, contactNode);
                contactNode.BeginEdit();
                Modified = true;
            }
        }

        private void btnAddPhoneNumber_Click(object sender, EventArgs e)
        {
            // добавление телефонного номера
            TreeNode contactNode = treeView.SelectedNode?.FindClosest(typeof(AddressBook.Contact));
            if (contactNode != null)
            {
                AddressBook.PhoneNumber phoneNumber = new AddressBook.PhoneNumber(AbPhrases.NewPhoneNumber);
                TreeNode phoneNumberNode = CreatePhoneNumberNode(phoneNumber);

                treeView.Add(contactNode, phoneNumberNode);
                phoneNumberNode.BeginEdit();
                Modified = true;
            }
        }

        private void btnAddEmail_Click(object sender, EventArgs e)
        {
            // добавление адреса электронной почты
            TreeNode contactNode = treeView.SelectedNode?.FindClosest(typeof(AddressBook.Contact));
            if (contactNode != null)
            {
                AddressBook.Email email = new AddressBook.Email(AbPhrases.NewEmail);
                TreeNode emailNode = CreateEmailNode(email);

                treeView.Add(contactNode, emailNode);
                emailNode.BeginEdit();
                Modified = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // переключение режима редактирования узла дерева
            TreeNode selNode = treeView.SelectedNode;
            if (selNode.IsEditing)
                selNode.EndEdit(false);
            else
                selNode.BeginEdit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // удаление выбранного объекта
            if (treeView.GetSelectedObject() is AddressBook.AddressBookItem)
            {
                treeView.RemoveSelectedNode();
                Modified = true;
            }
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
            // получение изменений после завершения редактирования узла
            if (e.Label != null /*редактирование отменено*/)
            {
                AddressBook.AddressBookItem bookItem = e.Node.Tag as AddressBook.AddressBookItem;

                if (bookItem != null)
                {
                    string oldVal = bookItem.Value;
                    string newVal = e.Label;

                    if (newVal == "")
                    {
                        e.CancelEdit = true;
                        ScadaUiUtils.ShowError(AbPhrases.EmptyValueNotAllowed);
                        e.Node.BeginEdit();
                    }
                    else if (!oldVal.Equals(newVal, StringComparison.Ordinal))
                    {
                        // установка нового значения
                        bookItem.Value = newVal;

                        // определение нового индекса узла, чтобы сохранить упорядоченность, и проверка значения
                        IList list = bookItem.Parent.Children;
                        int curInd = e.Node.Index;
                        int newInd = curInd;
                        bool duplicated;
                        string errMsg = "";

                        if (bookItem is AddressBook.ContactGroup)
                        {
                            newInd = FindInsertIndex<AddressBook.ContactGroup>(
                                (List<AddressBook.ContactGroup>)list, curInd, out duplicated);
                            if (duplicated)
                                errMsg = AbPhrases.ContactGroupExists;
                        }
                        else if (bookItem is AddressBook.Contact)
                        {
                            newInd = FindInsertIndex<AddressBook.Contact>(
                                (List<AddressBook.Contact>)list, curInd, out duplicated);
                            if (duplicated)
                                errMsg = AbPhrases.ContactExists;
                        }
                        else if (bookItem is AddressBook.ContactRecord)
                        {
                            newInd = FindInsertIndex<AddressBook.ContactRecord>(
                                (List<AddressBook.ContactRecord>)list, curInd, out duplicated);

                            if (bookItem is AddressBook.PhoneNumber)
                            {
                                if (duplicated)
                                    errMsg = AbPhrases.PhoneNumberExists;
                            }
                            else
                            {
                                if (duplicated)
                                    errMsg = AbPhrases.EmailExists;
                                if (!CheckEmail(newVal))
                                    errMsg = AbPhrases.IncorrectEmail;
                            }
                        }

                        if (errMsg != "")
                        {
                            // возврат старого значения
                            bookItem.Value = newVal;
                            e.CancelEdit = true;
                            ScadaUiUtils.ShowError(errMsg);
                            e.Node.BeginEdit();
                        }
                        else if (newInd != curInd)
                        {
                            // перемещение узла, чтобы сохранить упорядоченность
                            BeginInvoke(new Action(() => { treeView.MoveSelectedNode(newInd); }));
                        }

                        Modified = true;
                    }
                }
            }
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
