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
 * Module   : KpSms
 * Summary  : Phonebook form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2016
 */

using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpSms
{
    /// <summary>
    /// Phonebook form
    /// <para>Форма телефонного справочника</para>
    /// </summary>
    public partial class FrmPhonebook : Form
    {
        private AppDirs appDirs;                    // директории приложения
        private Phonebook phonebook;                // телефонный справочник
        private bool modified;                      // признак изменения справочника
        private Phonebook.PhoneNumber copiedNumber; // скопированный телефонный номер
        private TreeNode nodePhonebook;             // узел справочника


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmPhonebook()
        {
            InitializeComponent();

            appDirs = null;
            phonebook = new Phonebook();
            modified = false;
            copiedNumber = null;
            nodePhonebook = tvPhonebook.Nodes["nodePhonebook"];
        }


        /// <summary>
        /// Получить или установить признак изменения справочника
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
        /// Построить дерево телефонного справочника
        /// </summary>
        private void BuildTree()
        {
            try
            {
                tvPhonebook.BeginUpdate();
                nodePhonebook.Nodes.Clear();

                foreach (Phonebook.PhoneGroup group in phonebook.PhoneGroups.Values)
                    nodePhonebook.Nodes.Add(CreateGroupNode(group));

                nodePhonebook.Expand();
            }
            finally
            {
                tvPhonebook.EndUpdate();
            }
        }

        /// <summary>
        /// Создать узел дерева для группы телефонных номеров
        /// </summary>
        private TreeNode CreateGroupNode(Phonebook.PhoneGroup group)
        {
            string imageKey = group.PhoneNumbers.Count > 0 ? "folder_open.png" : "folder_closed.png";
            TreeNode nodeGroup = new TreeNode(group.Name)
            {                
                ImageKey = imageKey,
                SelectedImageKey = imageKey,
                Tag = group
            };

            foreach (Phonebook.PhoneNumber number in group.PhoneNumbers)
                nodeGroup.Nodes.Add(CreateNumberNode(number));

            nodeGroup.Expand();
            return nodeGroup;
        }

        /// <summary>
        /// Создать узел дерева для группы телефонных номеров
        /// </summary>
        private TreeNode CreateNumberNode(Phonebook.PhoneNumber number)
        {
            return new TreeNode()
            {
                Text = number.Name == "" ? 
                    number.Number : 
                    number.Number + " (" + number.Name + ")",
                ImageKey = "phone.png",
                SelectedImageKey = "phone.png",
                Tag = number
            };
        }

        /// <summary>
        /// Вставить группу телефонных номеров в справочник и в дерево
        /// </summary>
        private void InsertGroup(Phonebook.PhoneGroup group)
        {
            phonebook.PhoneGroups.Add(group.Name, group);
            int ind = phonebook.PhoneGroups.IndexOfKey(group.Name);
            TreeNode node = CreateGroupNode(group);
            nodePhonebook.Nodes.Insert(ind, node);
            tvPhonebook.SelectedNode = node;
        }

        /// <summary>
        /// Удалить группу телефонных номеров из справочника и из дерева
        /// </summary>
        private void RemoveGroup(Phonebook.PhoneGroup group)
        {
            int ind = phonebook.PhoneGroups.IndexOfKey(group.Name);
            phonebook.PhoneGroups.RemoveAt(ind);
            nodePhonebook.Nodes.RemoveAt(ind);
        }

        /// <summary>
        /// Вставить телефонный номер в справочник и в дерево, проверив уникальность номера
        /// </summary>
        private void InsertNumber(Phonebook.PhoneGroup group, TreeNode groupNode, Phonebook.PhoneNumber number)
        {
            int ind = group.FindPhoneNumberInsertIndex(number);
            group.PhoneNumbers.Insert(ind, number);
            TreeNode node = CreateNumberNode(number);
            groupNode.Nodes.Insert(ind, node);
            groupNode.ImageKey = groupNode.SelectedImageKey = "folder_open.png";
            tvPhonebook.SelectedNode = node;
        }

        /// <summary>
        /// Вставить телефонный номер в справочник и в дерево или обновить существующий номер
        /// </summary>
        private void InsertOrUpdateNumber(Phonebook.PhoneGroup group, TreeNode groupNode, Phonebook.PhoneNumber number)
        {
            if (number != null)
            {
                int ind = group.FindPhoneNumberIndex(number.Number);
                if (ind < 0)
                {
                    InsertNumber(group, groupNode, number);
                    Modified = true;
                }
                else if (MessageBox.Show(KpPhrases.UpdatePhoneNumberConfirm, CommonPhrases.QuestionCaption,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    RemoveNumber(group, groupNode, ind);
                    InsertNumber(group, groupNode, number);
                    Modified = true;
                }
            }
        }

        /// <summary>
        /// Удалить телефонный номер из справочника и из дерева
        /// </summary>
        private void RemoveNumber(Phonebook.PhoneGroup group, TreeNode groupNode, int numberInd)
        {
            group.PhoneNumbers.RemoveAt(numberInd);
            groupNode.Nodes.RemoveAt(numberInd);
            if (groupNode.Nodes.Count == 0)
                groupNode.ImageKey = groupNode.SelectedImageKey = "folder_closed.png";
        }

        /// <summary>
        /// Удалить телефонный номер из справочника и из дерева
        /// </summary>
        private void RemoveNumber(Phonebook.PhoneGroup group, TreeNode groupNode, Phonebook.PhoneNumber number)
        {
            int ind = group.FindPhoneNumberIndex(number.Number);
            if (ind >= 0)
                RemoveNumber(group, groupNode, ind);
        }

        /// <summary>
        /// Получить выбранный в дереве объект справочника
        /// </summary>
        private object GetSelectedObject()
        {
            return tvPhonebook.SelectedNode == null ? null : tvPhonebook.SelectedNode.Tag;
        }

        /// <summary>
        /// Получить группу телефонных номеров, соответствующую выбранному в дереве узлу
        /// </summary>
        private void GetCurrentGroup(out Phonebook.PhoneGroup group, out TreeNode groupNode)
        {
            TreeNode node = tvPhonebook.SelectedNode;
            while (node != null && !(node.Tag is Phonebook.PhoneGroup))
                node = node.Parent;

            if (node != null && node.Tag is Phonebook.PhoneGroup)
            {
                groupNode = node;
                group = (Phonebook.PhoneGroup)groupNode.Tag;
            }
            else if (nodePhonebook.Nodes.Count > 0)
            {
                groupNode = nodePhonebook.Nodes[0];
                group = (Phonebook.PhoneGroup)groupNode.Tag;
            }
            else
            {
                groupNode = null;
                group = null;
            }
        }

        /// <summary>
        /// Установить доступность кнопок
        /// </summary>
        private void SetButtonsEnabled()
        {
            object selObj = GetSelectedObject();
            btnEdit.Enabled = btnDelete.Enabled = selObj != null;
            btnCutNumber.Enabled = btnCopyNumber.Enabled = selObj is Phonebook.PhoneNumber;
            btnPasteNumber.Enabled = copiedNumber != null;
        }

        
        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(AppDirs appDirs)
        {
            if (appDirs == null)
                throw new ArgumentNullException("appDirs");

            FrmPhonebook frmPhonebook = new FrmPhonebook();
            frmPhonebook.appDirs = appDirs;
            frmPhonebook.ShowDialog();
        }


        private void FrmPhonebook_Load(object sender, EventArgs e)
        {
            // локализация модуля
            string errMsg;
            if (!Localization.UseRussian)
            {
                if (Localization.LoadDictionaries(appDirs.LangDir, "KpSms", out errMsg))
                {
                    Translator.TranslateForm(this, "Scada.Comm.Devices.KpSms.FrmPhonebook");
                    KpPhrases.Init();
                    nodePhonebook.Text = KpPhrases.PhonebookNode;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            // загрузка телефонного справочника
            string fileName = appDirs.ConfigDir + Phonebook.DefFileName;
            if (File.Exists(fileName) && !phonebook.Load(fileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);
            Modified = false;

            // вывод телефонного справочника
            BuildTree();

            // установка доступности кнопок
            SetButtonsEnabled();
        }

        private void FrmPhonebook_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(KpPhrases.SavePhonebookConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        string errMsg;
                        if (!phonebook.Save(appDirs.ConfigDir + Phonebook.DefFileName, out errMsg))
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


        private void btnCreateGroup_Click(object sender, EventArgs e)
        {
            // создание группы телефонных номеров
            Phonebook.PhoneGroup newGroup = FrmPhoneGroup.CreatePhoneGroup();
            if (newGroup != null)
            {
                if (phonebook.PhoneGroups.ContainsKey(newGroup.Name))
                {
                    ScadaUiUtils.ShowWarning(KpPhrases.PhoneGroupExists);
                }
                else
                {
                    InsertGroup(newGroup);
                    Modified = true;
                }
            }
        }

        private void btnCreateNumber_Click(object sender, EventArgs e)
        {
            // создание телефонного номера
            Phonebook.PhoneGroup group;
            TreeNode groupNode;
            GetCurrentGroup(out group, out groupNode);

            if (group != null)
            {
                Phonebook.PhoneNumber newNumber = FrmPhoneNumber.CreatePhoneNumber();
                InsertOrUpdateNumber(group, groupNode, newNumber);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
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
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
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
            }
        }

        private void btnCutNumber_Click(object sender, EventArgs e)
        {
            // вырезать телефонный номер
            Phonebook.PhoneNumber number = GetSelectedObject() as Phonebook.PhoneNumber;
            if (number != null)
            {
                copiedNumber = number;
                btnDelete_Click(null, null);
            }
        }

        private void btnCopyNumber_Click(object sender, EventArgs e)
        {
            // копирование телефонного номера
            Phonebook.PhoneNumber number = GetSelectedObject() as Phonebook.PhoneNumber;
            if (number != null)
            {
                copiedNumber = number;
                btnPasteNumber.Enabled = true;
            }
        }

        private void btnPasteNumber_Click(object sender, EventArgs e)
        {
            // вставка телефонного номера
            Phonebook.PhoneGroup group;
            TreeNode groupNode;
            GetCurrentGroup(out group, out groupNode);

            if (group != null)
                InsertOrUpdateNumber(group, groupNode, copiedNumber);
        }


        private void tvPhonebook_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // установка доступности кнопок
            SetButtonsEnabled();
        }

        private void tvPhonebook_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // установить иконку, если группа была развёрнута
            TreeNode node = e.Node;
            if (node.Tag is Phonebook.PhoneGroup)
                node.ImageKey = node.SelectedImageKey = "folder_open.png";
        }

        private void tvPhonebook_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // установить иконку, если группа была свёрнута
            TreeNode node = e.Node;
            if (node.Tag is Phonebook.PhoneGroup)
                node.ImageKey = node.SelectedImageKey = "folder_closed.png";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение телефонного справочника
            string errMsg;
            if (phonebook.Save(appDirs.ConfigDir + Phonebook.DefFileName, out errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
