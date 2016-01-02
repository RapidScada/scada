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
 * Module   : KpSnmp
 * Summary  : Device properties form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpSnmp
{
    /// <summary>
    /// Device properties form
    /// <para>Форма настройки свойств КП</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private AppDirs appDirs;   // директории приложения
        private int kpNum;         // номер настраиваемого КП
        private Config config;     // конфигурация КП
        private bool modified;     // признак изменения конфигурации
        private TreeNode rootNode; // корневой узел дерева

        
        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();

            appDirs = null;
            kpNum = 0;
            config = new Config();
            modified = false;
            rootNode = treeView.Nodes["nodeDevice"];
        }


        /// <summary>
        /// Получить или установить признак изменения конфигурации КП
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
        /// Построить дерево конфигурации
        /// </summary>
        private void BuildTree()
        {
            try
            {
                treeView.BeginUpdate();
                rootNode.Nodes.Clear();

                foreach (Config.VarGroup group in config.VarGroups)
                    rootNode.Nodes.Add(CreateGroupNode(group));

                rootNode.Expand();
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Создать узел дерева для группы переменных
        /// </summary>
        private TreeNode CreateGroupNode(Config.VarGroup group)
        {
            string imageKey = group.Variables.Count > 0 ? "folder_open.png" : "folder_closed.png";
            TreeNode groupNode = TreeViewUtils.CreateNode(group, imageKey, true);

            foreach (Config.Variable variable in group.Variables)
                groupNode.Nodes.Add(CreateVariableNode(variable));

            return groupNode;
        }
        
        /// <summary>
        /// Создать узел дерева для переменной
        /// </summary>
        private TreeNode CreateVariableNode(Config.Variable variable)
        {
            return TreeViewUtils.CreateNode(variable, "variable.png");
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(AppDirs appDirs, int kpNum)
        {
            if (appDirs == null)
                throw new ArgumentNullException("appDirs");

            FrmConfig frmConfig = new FrmConfig();
            frmConfig.appDirs = appDirs;
            frmConfig.kpNum = kpNum;
            frmConfig.ShowDialog();
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            // локализация модуля
            string errMsg;
            if (!Localization.UseRussian)
            {
                if (Localization.LoadDictionaries(appDirs.LangDir, "KpSnmp", out errMsg))
                {
                    Translator.TranslateForm(this, "Scada.Comm.Devices.KpSnmp.FrmConfig");
                    //KpPhrases.Init();
                    //TranslateKpTree();
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            // вывод заголовка
            Text = string.Format(Text, kpNum);

            // загрузка конфигурации КП
            string fileName = Config.GetFileName(appDirs.ConfigDir, kpNum);
            if (File.Exists(fileName) && !config.Load(fileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // вывод дерева конфигурации
            BuildTree();
        }

        private void btnAddVarGroup_Click(object sender, EventArgs e)
        {
            Config.VarGroup newVarGroup = FrmVarGroup.CreateVarGroup();
            if (newVarGroup != null)
            {
                /*if (phonebook.PhoneGroups.ContainsKey(newGroup.Name))
                {
                    ScadaUiUtils.ShowWarning(KpPhrases.PhoneGroupExists);
                }
                else
                {
                    InsertGroup(newGroup);
                    Modified = true;
                }*/
            }
        }

        private void btnAddVariable_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
