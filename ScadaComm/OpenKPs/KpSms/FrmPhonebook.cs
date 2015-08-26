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
 * Summary  : Phonebook form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

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
        private AppDirs appDirs;        // директории приложения
        private Phonebook phonebook;    // телефонный справочник        
        private TreeNode nodePhonebook; // узел справочника


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmPhonebook()
        {
            InitializeComponent();

            appDirs = null;
            phonebook = new Phonebook();

            nodePhonebook = tvPhonebook.Nodes["nodePhonebook"];
        }


        /// <summary>
        /// Построить дерево телефонного српавочника
        /// </summary>
        private void BuildTree()
        {
            try
            {
                tvPhonebook.BeginUpdate();
                nodePhonebook.Nodes.Clear();

                foreach (Phonebook.PhoneGroup phoneGroup in phonebook.PhoneGroups.Values)
                    nodePhonebook.Nodes.Add(CreatePhoneGroupNode(phoneGroup));

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
        private TreeNode CreatePhoneGroupNode(Phonebook.PhoneGroup phoneGroup)
        {
            string imageKey = phoneGroup.PhoneNumbers.Count > 0 ? "folder_open.png" : "folder_closed.png";
            TreeNode nodeGroup = new TreeNode(phoneGroup.Name)
            {                
                ImageKey = imageKey,
                SelectedImageKey = imageKey,
                Tag = phoneGroup
            };

            foreach (Phonebook.PhoneNumber phoneNumber in phoneGroup.PhoneNumbers.Values)
                nodeGroup.Nodes.Add(CreatePhoneNumberNode(phoneNumber));

            nodeGroup.Expand();
            return nodeGroup;
        }

        /// <summary>
        /// Создать узел дерева для группы телефонных номеров
        /// </summary>
        private TreeNode CreatePhoneNumberNode(Phonebook.PhoneNumber phoneNumber)
        {
            return new TreeNode()
            {
                Text = phoneNumber.Number + (phoneNumber.Name == "" ? "" : " (" + phoneNumber.Name + ")"),
                ImageKey = "phone.png",
                SelectedImageKey = "phone.png",
                Tag = phoneNumber
            };
        }

        /// <summary>
        /// Вставить группу телефонных номеров в справочник и в дерево
        /// </summary>
        private void InsertPhoneGroup(Phonebook.PhoneGroup phoneGroup)
        {
            phonebook.PhoneGroups.Add(phoneGroup.Name, phoneGroup);
            int ind = phonebook.PhoneGroups.IndexOfKey(phoneGroup.Name);
            TreeNode node = CreatePhoneGroupNode(phoneGroup);
            nodePhonebook.Nodes.Insert(ind, node);
            tvPhonebook.SelectedNode = node;
        }

        /// <summary>
        /// Удалить группу телефонных номеров из справочника и из дерева
        /// </summary>
        private void RemovePhoneGroup(Phonebook.PhoneGroup phoneGroup)
        {
            int ind = phonebook.PhoneGroups.IndexOfKey(phoneGroup.Name);
            phonebook.PhoneGroups.RemoveAt(ind);
            nodePhonebook.Nodes.RemoveAt(ind);
        }

        /// <summary>
        /// Вставить телефонный номер в справочник и в дерево
        /// </summary>
        private void InsertPhoneNumber(Phonebook.PhoneGroup phoneGroup, TreeNode phoneGroupNode, 
            Phonebook.PhoneNumber phoneNumber)
        {
            phoneGroup.PhoneNumbers.Add(phoneNumber.Name, phoneNumber);
            int ind = phoneGroup.PhoneNumbers.IndexOfKey(phoneNumber.Name);
            TreeNode node = CreatePhoneNumberNode(phoneNumber);
            phoneGroupNode.Nodes.Insert(ind, node);
            tvPhonebook.SelectedNode = node;
        }

        /// <summary>
        /// Удалить телефонный номер из справочника и из дерева
        /// </summary>
        private void RemovePhoneNumber(Phonebook.PhoneGroup phoneGroup, TreeNode phoneGroupNode, 
            Phonebook.PhoneNumber phoneNumber)
        {
            int ind = phoneGroup.PhoneNumbers.IndexOfKey(phoneNumber.Name);
            phoneGroup.PhoneNumbers.RemoveAt(ind);
            phoneGroupNode.Nodes.RemoveAt(ind);
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
        private Phonebook.PhoneGroup GetCurrentPhoneGroup(out TreeNode phoneGroupNode)
        {
            TreeNode node = tvPhonebook.SelectedNode;
            while (node != null && !(node.Tag is Phonebook.PhoneGroup))
                node = node.Parent;

            if (node != null && node.Tag is Phonebook.PhoneGroup)
            {
                phoneGroupNode = node;
                return (Phonebook.PhoneGroup)phoneGroupNode.Tag;
            }
            else if (nodePhonebook.Nodes.Count > 0)
            {
                phoneGroupNode = nodePhonebook.Nodes[0];
                return (Phonebook.PhoneGroup)phoneGroupNode.Tag;
            }
            else
            {
                phoneGroupNode = null;
                return null;
            }
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
                if (Localization.LoadDictionaries(appDirs.LangDir, "KpOpc", out errMsg))
                {
                    Localization.TranslateForm(this, "Scada.Comm.Devices.KpSms.FrmConfig");
                    KpPhrases.Init();
                    nodePhonebook.Text = KpPhrases.PhonebookNode;
                }
                else
                {
                    ScadaUtils.ShowError(errMsg);
                }
            }

            // загрузка телефонного справочника
            string fileName = appDirs.ConfigDir + Phonebook.DefFileName;
            if (File.Exists(fileName) && !phonebook.Load(fileName, out errMsg))
                ScadaUtils.ShowError(errMsg);
        }

        private void FrmPhonebook_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void btnCreateGroup_Click(object sender, EventArgs e)
        {
            // создание группы телефонных номеров
            Phonebook.PhoneGroup newGroup = FrmPhoneGroup.CreatePhoneGroup();
            if (newGroup != null)
            {
                if (phonebook.PhoneGroups.ContainsKey(newGroup.Name))
                    ScadaUtils.ShowWarning("exists!!!");
                else
                    InsertPhoneGroup(newGroup);
            }
        }

        private void btnCreateNumber_Click(object sender, EventArgs e)
        {
            // создание телефонного номера
            TreeNode phoneGroupNode;
            Phonebook.PhoneGroup phoneGroup = GetCurrentPhoneGroup(out phoneGroupNode);

            if (phoneGroup != null)
            {
                Phonebook.PhoneNumber newNumber = FrmPhoneNumber.CreatePhoneNumber();
                if (newNumber != null)
                {
                    Phonebook.PhoneNumber oldNumber;
                    if (phoneGroup.PhoneNumbers.TryGetValue(newNumber.Name, out oldNumber))
                    {
                        ScadaUtils.ShowWarning("exists!!!");
                        RemovePhoneNumber(phoneGroup, phoneGroupNode, oldNumber);
                    }

                    InsertPhoneNumber(phoneGroup, phoneGroupNode, newNumber);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // редактирование выбранного объекта
            object selObj = GetSelectedObject();
            Phonebook.PhoneGroup phoneGroup = selObj as Phonebook.PhoneGroup;
            Phonebook.PhoneNumber phoneNumber = selObj as Phonebook.PhoneNumber;

            if (phoneGroup != null)
            {
                // редактирование группы телефонных номеров
                Phonebook.PhoneGroup newGroup = FrmPhoneGroup.EditPhoneGroup(phoneGroup);
                if (newGroup != null)
                {
                    RemovePhoneGroup(phoneGroup);
                    InsertPhoneGroup(newGroup);
                }
            }
            else if (phoneNumber != null)
            {
                // редактирование телефонного номера
                TreeNode phoneGroupNode;
                phoneGroup = GetCurrentPhoneGroup(out phoneGroupNode);

                if (phoneGroup != null)
                {
                    Phonebook.PhoneNumber newNumber = FrmPhoneNumber.EditPhoneNumber(phoneNumber);
                    if (newNumber != null)
                    {
                        RemovePhoneNumber(phoneGroup, phoneGroupNode, phoneNumber);
                        InsertPhoneNumber(phoneGroup, phoneGroupNode, newNumber);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // удаление выбранного объекта
            object selObj = GetSelectedObject();
            Phonebook.PhoneGroup phoneGroup = selObj as Phonebook.PhoneGroup;
            Phonebook.PhoneNumber phoneNumber = selObj as Phonebook.PhoneNumber;

            if (phoneGroup != null)
            {
                // удаление группы телефонных номеров
                RemovePhoneGroup(phoneGroup);
            }
            else if (phoneNumber != null)
            {
                // удаление телефонного номера
                TreeNode phoneGroupNode;
                phoneGroup = GetCurrentPhoneGroup(out phoneGroupNode);
                if (phoneGroup != null)
                    RemovePhoneNumber(phoneGroup, phoneGroupNode, phoneNumber);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
