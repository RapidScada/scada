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
 * Module   : KpSnmp
 * Summary  : Device properties form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2019
 */

using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpSnmp
{
    /// <summary>
    /// Device properties form.
    /// <para>Форма настройки свойств КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private AppDirs appDirs;       // директории приложения
        private int kpNum;             // номер настраиваемого КП
        private string cmdLine;        // командная строка КП
        private KpConfig config;         // конфигурация КП
        private string configFileName; // имя файла конфигурации КП
        private bool modified;         // признак изменения конфигурации
        private TreeNode rootNode;     // корневой узел дерева

        
        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();

            appDirs = null;
            kpNum = 0;
            cmdLine = "";
            config = new KpConfig();
            configFileName = "";
            modified = false;
            rootNode = treeView.Nodes["nodeDevice"];
            rootNode.Tag = config;
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
        /// Определить, что выбрана группа или переменная
        /// </summary>
        private bool GroupOrVariableIsSelected
        {
            get
            {
                object selObj = treeView.GetSelectedObject();
                return selObj is KpConfig.VarGroup || selObj is KpConfig.Variable;
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

                foreach (KpConfig.VarGroup group in config.VarGroups)
                    rootNode.Nodes.Add(CreateGroupNode(group));

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
        /// Создать узел дерева для группы переменных
        /// </summary>
        private TreeNode CreateGroupNode(KpConfig.VarGroup group)
        {
            string imageKey = group.Variables.Count > 0 ? "folder_open.png" : "folder_closed.png";
            TreeNode groupNode = TreeViewUtils.CreateNode(group, imageKey, true);

            foreach (KpConfig.Variable variable in group.Variables)
                groupNode.Nodes.Add(CreateVariableNode(variable));

            return groupNode;
        }
        
        /// <summary>
        /// Создать узел дерева для переменной
        /// </summary>
        private TreeNode CreateVariableNode(KpConfig.Variable variable)
        {
            return TreeViewUtils.CreateNode(variable, "variable.png");
        }

        /// <summary>
        /// Вычислить сигнал выбранной переменной
        /// </summary>
        private int CalcSignal()
        {
            TreeNode selectedNode = treeView.SelectedNode;
            if (selectedNode != null && selectedNode.Tag is KpConfig.Variable)
            {
                int signal = 1;
                TreeNode selGroupNode = selectedNode.Parent;

                foreach (TreeNode groupNode in rootNode.Nodes)
                {
                    if (groupNode == selGroupNode)
                    {
                        signal += selectedNode.Index;
                        break;
                    }
                    else
                    {
                        signal += groupNode.Nodes.Count;
                    }
                }

                return signal;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Установить доступность кнопок
        /// </summary>
        private void SetButtonsEnabled()
        {
            btnAddVariable.Enabled = btnEdit.Enabled = btnDelete.Enabled = 
                GroupOrVariableIsSelected;
            btnMoveUp.Enabled = treeView.MoveUpSelectedNodeIsEnabled(TreeViewUtils.MoveBehavior.ThroughSimilarParents);
            btnMoveDown.Enabled = treeView.MoveDownSelectedNodeIsEnabled(TreeViewUtils.MoveBehavior.ThroughSimilarParents);
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(AppDirs appDirs, int kpNum, string cmdLine)
        {
            FrmConfig frmConfig = new FrmConfig
            {
                appDirs = appDirs ?? throw new ArgumentNullException("appDirs"),
                kpNum = kpNum,
                cmdLine = cmdLine
            };

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
                    KpPhrases.InitFromDictionaries();
                    rootNode.Text = KpPhrases.DeviceNode;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            // вывод заголовка
            Text = string.Format(Text, kpNum);

            // загрузка конфигурации КП
            configFileName = KpConfig.GetFileName(appDirs.ConfigDir, kpNum, cmdLine);
            if (File.Exists(configFileName) && !config.Load(configFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);
            Modified = false;

            // вывод дерева конфигурации
            BuildTree();

            // установка доступности кнопок
            SetButtonsEnabled();
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(CommPhrases.SaveKpSettingsConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        string errMsg;
                        if (!config.Save(configFileName, out errMsg))
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


        private void btnAddVarGroup_Click(object sender, EventArgs e)
        {
            // добавление группы переменных
            KpConfig.VarGroup newVarGroup = FrmVarGroup.CreateVarGroup();
            if (newVarGroup != null)
            {
                TreeNode groupNode = CreateGroupNode(newVarGroup);
                treeView.Insert(rootNode, groupNode);
                Modified = true;
            }
        }

        private void btnAddVariable_Click(object sender, EventArgs e)
        {
            // добавление переменной
            TreeNode closestGroupNode = treeView.SelectedNode?.FindClosest(typeof(KpConfig.VarGroup));
            if (closestGroupNode != null)
            {
                KpConfig.Variable newVariable = FrmVariable.CreateVariable();
                if (newVariable != null)
                {
                    TreeNode variableNode = CreateVariableNode(newVariable);
                    treeView.Insert(closestGroupNode, variableNode);
                    Modified = true;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // редактирование выбранного объекта
            object selObj = treeView.GetSelectedObject();
            bool edited = false;

            if (selObj is KpConfig.VarGroup)
                edited = FrmVarGroup.EditVarGroup((KpConfig.VarGroup)selObj);
            else if (selObj is KpConfig.Variable)
                edited = FrmVariable.EditVariable((KpConfig.Variable)selObj, CalcSignal());

            if (edited)
            {
                treeView.UpdateSelectedNodeText();
                Modified = true;
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            // перемещение выбранного объекта вверх
            if (GroupOrVariableIsSelected)
            {
                treeView.MoveUpSelectedNode(TreeViewUtils.MoveBehavior.ThroughSimilarParents);
                Modified = true;
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            // перемещение выбранного объекта вниз
            if (GroupOrVariableIsSelected)
            {
                treeView.MoveDownSelectedNode(TreeViewUtils.MoveBehavior.ThroughSimilarParents);
                Modified = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // удаление выбранного объекта
            if (GroupOrVariableIsSelected)
            {
                treeView.RemoveSelectedNode();
                Modified = true;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            // отображение формы дополнительных настроек
            if (FrmSettings.Show(config))
                Modified = true;
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // установка доступности кнопок
            SetButtonsEnabled();
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // установка изображения развёрнутой группы
            if (e.Node.Tag is KpConfig.VarGroup)
                e.Node.SetImageKey("folder_open.png");
        }

        private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // установка изображения свёрнутой группы
            if (e.Node.Tag is KpConfig.VarGroup)
                e.Node.SetImageKey("folder_closed.png");
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // выбор узла дерева по щелчку правой кнопкой мыши (как в SCADA-Коммуникатор)
            if (e.Button == MouseButtons.Right && e.Node != null)
                treeView.SelectedNode = e.Node;
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // вызов формы редактирования, если нет дочерних узлов
            if (e.Node.Nodes.Count == 0)
                btnEdit_Click(null, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение конфигурации КП
            string errMsg;
            if (config.Save(configFileName, out errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
