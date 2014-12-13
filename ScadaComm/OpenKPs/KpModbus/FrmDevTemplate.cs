/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : KpModbus
 * Summary  : Editing device template form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Comm.KP
{
    /// <summary>
    /// Editing device template form
    /// <para>Форма редактирования шаблона устройства</para>
    /// </summary>
    public partial class FrmDevTemplate : Form
    {
        /// <summary>
        /// Информация об элементе группы
        /// </summary>
        private class ElemInfo
        {
            /// <summary>
            /// Получить или установить элемент
            /// </summary>
            public Modbus.Elem Elem { get; set; }
            /// <summary>
            /// Получить или установить группу элементов, в которую входит элемент
            /// </summary>
            public Modbus.ElemGroup ElemGroup { get; set; }
            /// <summary>
            /// Получить или установить адрес
            /// </summary>
            public ushort Address { get; set; }
            /// <summary>
            /// Получить или установить сигнал КП
            /// </summary>
            public int Signal { get; set; }
            /// <summary>
            /// Получить строковую запись диапазона адресов элемента
            /// </summary>
            public string AddressRange
            {
                get
                {
                    int addr = Address + 1;
                    return addr + (Elem == null || Elem.Length <= 1 ? "" : " - " + (addr + 1));
                }
            }
            /// <summary>
            /// Получить обозначение элемента в дереве
            /// </summary>
            public string Caption
            {
                get
                {
                    int addr = Address + 1;
                    return (Elem == null || Elem.Name == "" ? KpPhrases.DefElemName : Elem.Name) + 
                        " (" + AddressRange + ")";
                }
            }
        }

        private Modbus.DeviceModel devTemplate; // редактируемый шаблон устройства
        private bool modified;                  // признак изменения шаблона устройства
        private string fileName;                // имя файла шаблона устройства
        private Modbus.ElemGroup selElemGroup;  // выбранная группа элементов
        private ElemInfo selElemInfo;           // информация о выбранном элементе
        private Modbus.Cmd selCmd;              // выбранная команда
        private TreeNode selNode;               // выбранный узел дерева
        private TreeNode grsNode;               // узел дерева "Группы элементов"
        private TreeNode cmdsNode;              // узел дерева "Команды"
        private bool procChangedEv;             // обрабатывать события на изменение данных


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmDevTemplate()
        {
            InitializeComponent();

            devTemplate = new Modbus.DeviceModel();
            modified = false;
            fileName = "";
            selElemGroup = null;
            selElemInfo = null;
            selCmd = null;
            selNode = null;
            grsNode = treeView.Nodes["grsNode"];
            cmdsNode = treeView.Nodes["cmdsNode"];
            procChangedEv = false;

            ConfigDir = "";
            LangDir = "";
        }


        /// <summary>
        /// Получить или установить признак изменения шаблона устройства
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
        /// Получить или установить директорию конфигурации
        /// </summary>
        public string ConfigDir { get; set; }

        /// <summary>
        /// Получить или установить директорию языковых файлов
        /// </summary>
        public string LangDir { get; set; }


        /// <summary>
        /// Перевести текст основных узлов дерева
        /// </summary>
        private void TranslateTree()
        {
            grsNode.Text = KpPhrases.GrsNode;
            cmdsNode.Text = KpPhrases.CmdsNode;
        }

        /// <summary>
        /// Заполнить дерево в соответствии с шаблоном устройства
        /// </summary>
        private void FillTree()
        {
            // обнуление выбранных объектов и снятие признака изменения
            selElemGroup = null;
            selCmd = null;
            selElemInfo = null;
            ShowElemGroupProps(null);
            Modified = false;

            // приостановка отрисовки дерева
            treeView.BeginUpdate();

            // очистка дерева
            grsNode.Nodes.Clear();
            cmdsNode.Nodes.Clear();
            treeView.SelectedNode = grsNode;

            // заполнение узла групп элементов
            foreach (Modbus.ElemGroup elemGroup in devTemplate.ElemGroups)
                grsNode.Nodes.Add(NewElemGroupNode(elemGroup));

            // заполнение узла команд
            foreach (Modbus.Cmd cmd in devTemplate.Cmds)
                cmdsNode.Nodes.Add(NewCmdNode(cmd));

            // раскрытие основных узлов дерева
            grsNode.Expand();
            cmdsNode.Expand();

            // возобновление отрисовки дерева
            treeView.EndUpdate();
        }

        /// <summary>
        /// Создать узел группы элементов
        /// </summary>
        private TreeNode NewElemGroupNode(Modbus.ElemGroup elemGroup)
        {
            string name = elemGroup.Name == "" ? KpPhrases.DefGrName : elemGroup.Name;
            TreeNode grNode = new TreeNode(name + " (" + Modbus.GetTableTypeName(elemGroup.TableType) + ")");
            grNode.ImageKey = grNode.SelectedImageKey = "group.png";
            grNode.Tag = elemGroup;

            ushort elemAddr = elemGroup.Address;
            int elemSig = elemGroup.StartParamInd + 1;

            foreach (Modbus.Elem elem in elemGroup.Elems)
            {
                ElemInfo elemInfo = new ElemInfo();
                elemInfo.Elem = elem;
                elemInfo.ElemGroup = elemGroup;
                elemInfo.Address = elemAddr;
                elemInfo.Signal = elemSig++;

                grNode.Nodes.Add(NewElemNode(elemInfo));
                elemAddr += (ushort)elem.Length;
            }

            return grNode;
        }
        
        /// <summary>
        /// Создать узел элемента группы
        /// </summary>
        private TreeNode NewElemNode(ElemInfo elemInfo)
        {
            TreeNode elemNode = new TreeNode(elemInfo.Caption);
            elemNode.ImageKey = elemNode.SelectedImageKey = "elem.png";
            elemNode.Tag = elemInfo;
            return elemNode;
        }

        /// <summary>
        /// Создать узел команды
        /// </summary>
        private TreeNode NewCmdNode(Modbus.Cmd cmd)
        {
            string name = cmd.Name == "" ? KpPhrases.DefCmdName : cmd.Name;
            TreeNode cmdNode = new TreeNode(name + 
                " (" + Modbus.GetTableTypeName(cmd.TableType) + ", " + (cmd.Address + 1) + ")");
            cmdNode.ImageKey = cmdNode.SelectedImageKey = "cmd.png";
            cmdNode.Tag = cmd;
            return cmdNode;
        }

        /// <summary>
        /// Обновить узел выбранной группы элементов
        /// </summary>
        private void UpdateElemGroupNode()
        {
            if (selElemGroup != null)
            {
                string name = selElemGroup.Name == "" ? KpPhrases.DefGrName : selElemGroup.Name;
                selNode.Text = name + " (" + Modbus.GetTableTypeName(selElemGroup.TableType) + ")";
            }
        }

        /// <summary>
        /// Обновить узлы элементов выбранной группы
        /// </summary>
        private void UpdateElemNodes(TreeNode grNode = null)
        {
            treeView.BeginUpdate();

            if (grNode == null)
                grNode = selNode;

            if (grNode.Tag is Modbus.ElemGroup)
            {
                Modbus.ElemGroup elemGroup = (Modbus.ElemGroup)grNode.Tag;
                ushort elemAddr = elemGroup.Address;
                int elemSig = elemGroup.StartParamInd + 1;

                foreach (TreeNode elemNode in grNode.Nodes)
                {
                    ElemInfo elemInfo = elemNode.Tag as ElemInfo;
                    elemInfo.Address = elemAddr;
                    elemInfo.Signal = elemSig++;
                    elemNode.Text = elemInfo.Caption;
                    elemAddr += (ushort)elemInfo.Elem.Length;
                }
            }

            treeView.EndUpdate();
        }

        /// <summary>
        /// Обновить сигналы КП элементов групп, начиная с заданного узла дерева
        /// </summary>
        private void UpdateSignals(TreeNode startGrNode)
        {
            // проверка корректности заданного узла дерева
            if (!(startGrNode.Tag is Modbus.ElemGroup))
                return;

            // определение начального индекса параметров КП
            TreeNode prevGrNode = startGrNode.PrevNode;
            Modbus.ElemGroup prevElemGroup = prevGrNode == null ? null : prevGrNode.Tag as Modbus.ElemGroup;
            int paramInd = prevElemGroup == null ? 0 : prevElemGroup.StartParamInd + prevElemGroup.Elems.Count;

            // обновление групп и их элементов
            int grNodeCnt = grsNode.Nodes.Count;

            for (int i = startGrNode.Index; i < grNodeCnt; i++)
            {
                TreeNode grNode = grsNode.Nodes[i];
                Modbus.ElemGroup elemGroup = grNode.Tag as Modbus.ElemGroup;
                int elemSig = paramInd + 1;
                elemGroup.StartParamInd = paramInd;
                paramInd += elemGroup.Elems.Count;

                foreach (TreeNode elemNode in grNode.Nodes)
                {
                    ElemInfo elem = elemNode.Tag as ElemInfo;
                    elem.Signal = elemSig++;
                }
            }
        }

        /// <summary>
        /// Обновить узел выбранной команды
        /// </summary>
        private void UpdateCmdNode()
        {
            if (selCmd != null)
            {
                string name = selCmd.Name == "" ? KpPhrases.DefCmdName : selCmd.Name;
                selNode.Text = name + " (" + Modbus.GetTableTypeName(selCmd.TableType) + ", " + 
                    (selCmd.Address + 1) + ")";
            }
        }


        /// <summary>
        /// Отобразить свойства группы элементов
        /// </summary>
        private void ShowElemGroupProps(Modbus.ElemGroup elemGroup)
        {
            procChangedEv = false;

            gbElemGroup.Visible = true;
            gbElem.Visible = false;
            gbCmd.Visible = false;

            if (elemGroup == null)
            {
                txtGrName.Text = "";
                cbGrTableType.SelectedIndex = 0;
                numGrAddress.Value = 1;
                numGrElemCnt.Value = 1;
                gbElemGroup.Enabled = false;
            }
            else
            {
                txtGrName.Text = elemGroup.Name;
                cbGrTableType.SelectedIndex = (int)elemGroup.TableType;
                numGrAddress.Value = elemGroup.Address + 1;
                numGrElemCnt.Value = 1;
                numGrElemCnt.Maximum = Modbus.ElemGroup.GetMaxElemCnt(elemGroup.TableType);
                numGrElemCnt.Value = elemGroup.Elems.Count;
                gbElemGroup.Enabled = true;
            }

            procChangedEv = true;
        }

        /// <summary>
        /// Отобразить свойства элемента
        /// </summary>
        private void ShowElemProps(ElemInfo elemInfo)
        {
            procChangedEv = false;

            gbElemGroup.Visible = false;
            gbElem.Visible = true;
            gbCmd.Visible = false;

            if (elemInfo == null)
            {
                txtElemName.Text = "";
                txtElemAddress.Text = "";
                txtElemSignal.Text = "";
                rbBool.Checked = true;
                gbElem.Enabled = false;
            }
            else
            {
                txtElemName.Text = elemInfo.Elem.Name;
                txtElemAddress.Text = elemInfo.AddressRange;
                txtElemSignal.Text = elemInfo.Signal.ToString();
                Modbus.ElemTypes elemType = elemInfo.Elem.ElemType;
                rbUShort.Enabled = rbShort.Enabled = rbUInt.Enabled = rbInt.Enabled = rbFloat.Enabled = 
                    elemType != Modbus.ElemTypes.Bool;
                rbBool.Enabled = elemType == Modbus.ElemTypes.Bool;

                switch (elemType)
                {
                    case Modbus.ElemTypes.UShort:
                        rbUShort.Checked = true;
                        break;
                    case Modbus.ElemTypes.Short:
                        rbShort.Checked = true;
                        break;
                    case Modbus.ElemTypes.UInt:
                        rbUInt.Checked = true;
                        break;
                    case Modbus.ElemTypes.Int:
                        rbInt.Checked = true;
                        break;
                    case Modbus.ElemTypes.Float:
                        rbFloat.Checked = true;
                        break;
                    default:
                        rbBool.Checked = true;
                        break;
                }

                gbElem.Enabled = true;
            }

            procChangedEv = true;
        }

        /// <summary>
        /// Отобразить свойства команды
        /// </summary>
        private void ShowCmdProps(Modbus.Cmd cmd)
        {
            procChangedEv = false;

            gbElemGroup.Visible = false;
            gbElem.Visible = false;
            gbCmd.Visible = true;

            if (cmd == null)
            {
                cbCmdTableType.SelectedIndex = 0;
                numCmdAddress.Value = 1;
                numCmdElemCnt.Value = 1;
                numCmdNum.Value = 1;
                txtCmdName.Text = "";
                gbCmd.Enabled = false;
            }
            else
            {
                cbCmdTableType.SelectedIndex = cmd.TableType == Modbus.TableTypes.Coils ? 0 : 1;
                chkCmdMultiple.Checked = cmd.Multiple;
                numCmdAddress.Value = cmd.Address + 1;
                numCmdElemCnt.Value = cmd.ElemCnt;
                numCmdElemCnt.Enabled = cmd.Multiple;
                numCmdNum.Value = cmd.CmdNum;
                txtCmdName.Text = cmd.Name;
                gbCmd.Enabled = true;
            }

            procChangedEv = true;
        }

        /// <summary>
        /// Заблокировать редактирование свойств
        /// </summary>
        private void DisableProps()
        {
            if (gbElemGroup.Visible)
                ShowElemGroupProps(null);
            else if (gbElem.Visible)
                ShowElemProps(null);
            else if (gbCmd.Visible)
                ShowCmdProps(null);
        }

        /// <summary>
        /// Сохраненить изменения
        /// </summary>
        private bool SaveChanges(bool saveAs)
        {
            // определение имени файла
            string newFileName = "";

            if (saveAs || fileName == "")
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    newFileName = saveFileDialog.FileName;
            }
            else
            {
                newFileName = fileName;
            }

            if (newFileName == "")
            {
                return false;
            }
            else
            {
                // сохранение шаблона устройства
                string errMsg;
                if (devTemplate.SaveTemplate(newFileName, out errMsg))
                {
                    Modified = false;
                    fileName = newFileName;
                    return true;
                }
                else
                {
                    ScadaUtils.ShowError(errMsg);
                    return false;
                }
            }
        }

        /// <summary>
        /// Преверить необходимость сохранения изменений
        /// </summary>
        private bool CheckChanges()
        {
            if (modified)
            {
                DialogResult result = MessageBox.Show(KpPhrases.SaveTemplateConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        return SaveChanges(false);
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }


        private void FrmDevTemplate_Load(object sender, EventArgs e)
        {
            // локализация модуля
            string errMsg;
            if (!Localization.UseRussian)
            {
                if (Localization.LoadDictionaries(LangDir, "KpModbus", out errMsg))
                {
                    Localization.TranslateForm(this, "Scada.Comm.KP.FrmDevTemplate");
                    KpPhrases.Init();
                    TranslateTree();
                }
                else
                {
                    ScadaUtils.ShowError(errMsg);
                }
            }

            // настройка элементов управления
            openFileDialog.InitialDirectory = ConfigDir;
            saveFileDialog.InitialDirectory = ConfigDir;
            gbElem.Top = gbCmd.Top = gbElemGroup.Top;
            FillTree();
        }

        private void FrmDevTemplate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckChanges();
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            // создание шаблона устройства
            if (CheckChanges())
            {
                saveFileDialog.FileName = "";
                devTemplate = new Modbus.DeviceModel();
                fileName = "";
                FillTree();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // открытие шаблона устройства из файла
            if (CheckChanges())
            {
                openFileDialog.FileName = "";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    saveFileDialog.FileName = openFileDialog.FileName;
                    Modbus.DeviceModel templ = new Modbus.DeviceModel();
                    string errMsg;

                    if (templ.LoadTemplate(openFileDialog.FileName, out errMsg))
                    {
                        devTemplate = templ;
                        fileName = openFileDialog.FileName;
                        FillTree();
                    }
                    else
                    {
                        ScadaUtils.ShowError(errMsg);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение шаблона устройства в файл
            SaveChanges(sender == btnSaveAs);
        }


        private void btnAddElemGroup_Click(object sender, EventArgs e)
        {
            // создание группы элементов и добавление в шаблон устройства
            Modbus.ElemGroup elemGroup = new Modbus.ElemGroup(Modbus.TableTypes.DiscreteInputs);
            elemGroup.Elems.Add(new Modbus.Elem());
            int ind = selNode != null && selNode.Tag is Modbus.ElemGroup ? 
                selNode.Index + 1 : devTemplate.ElemGroups.Count;
            devTemplate.ElemGroups.Insert(ind, elemGroup);

            // создание узла дерева группы элементов
            TreeNode grNode = NewElemGroupNode(elemGroup);
            grsNode.Nodes.Insert(ind, grNode);
            UpdateSignals(grNode);
            grNode.Expand();
            treeView.SelectedNode = grNode;
            txtGrName.Select();

            // установка признака изменения
            Modified = true;
        }

        private void btnAddElem_Click(object sender, EventArgs e)
        {
            // создание элемента и добавление в шаблон устройства
            Modbus.ElemGroup elemGroup = selElemGroup == null ? selElemInfo.ElemGroup : selElemGroup;
            int maxElemCnt = Modbus.ElemGroup.GetMaxElemCnt(elemGroup.TableType);

            if (elemGroup.Elems.Count >= maxElemCnt)
            {
                MessageBox.Show(string.Format(KpPhrases.ElemCntExceeded, maxElemCnt), 
                    CommonPhrases.WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ElemInfo elemInfo = new ElemInfo();
            elemInfo.Elem = new Modbus.Elem() { ElemType = elemGroup.DefElemType };
            elemInfo.ElemGroup = elemGroup;
            int ind = selNode.Tag is ElemInfo ? selNode.Index + 1 : elemGroup.Elems.Count;
            elemGroup.Elems.Insert(ind, elemInfo.Elem);

            // создание узла дерева элемента
            TreeNode elemNode = NewElemNode(elemInfo);
            TreeNode grNode = selNode.Tag is ElemInfo ? selNode.Parent : selNode;
            grNode.Nodes.Insert(ind, elemNode);
            UpdateElemNodes(grNode);
            UpdateSignals(grNode);
            treeView.SelectedNode = elemNode;
            txtElemName.Select();

            // установка признака изменения
            Modified = true;
        }

        private void btnAddCmd_Click(object sender, EventArgs e)
        {
            // создание команды и добавление в шаблон устройства
            Modbus.Cmd cmd = new Modbus.Cmd(Modbus.TableTypes.Coils);
            int ind = selNode != null && selNode.Tag is Modbus.Cmd ? selNode.Index + 1 : devTemplate.Cmds.Count;
            devTemplate.Cmds.Insert(ind, cmd);

            // создание узла дерева команды
            TreeNode cmdNode = NewCmdNode(cmd);
            cmdsNode.Nodes.Insert(ind, cmdNode);
            treeView.SelectedNode = cmdNode;
            txtCmdName.Select();

            // установка признака изменения
            Modified = true;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            // перемещение объекта вверх
            TreeNode prevNode = selNode.PrevNode;
            int prevInd = prevNode.Index;

            if (selElemGroup != null)
            {
                // перемещение группы элементов вверх
                Modbus.ElemGroup prevElemGroup = prevNode.Tag as Modbus.ElemGroup;

                devTemplate.ElemGroups.RemoveAt(prevInd);
                devTemplate.ElemGroups.Insert(prevInd + 1, prevElemGroup);

                grsNode.Nodes.RemoveAt(prevInd);
                grsNode.Nodes.Insert(prevInd + 1, prevNode);

                UpdateSignals(selNode);
            }
            else if (selElemInfo != null)
            {
                // перемещение элемента вверх
                ElemInfo prevElemInfo = prevNode.Tag as ElemInfo;

                selElemInfo.ElemGroup.Elems.RemoveAt(prevInd);
                selElemInfo.ElemGroup.Elems.Insert(prevInd + 1, prevElemInfo.Elem);

                TreeNode grNode = selNode.Parent;
                grNode.Nodes.RemoveAt(prevInd);
                grNode.Nodes.Insert(prevInd + 1, prevNode);

                UpdateElemNodes(grNode);
                ShowElemProps(selElemInfo);
            }
            else if (selCmd != null)
            {
                // перемещение команды вверх
                Modbus.Cmd prevCmd = prevNode.Tag as Modbus.Cmd;

                devTemplate.Cmds.RemoveAt(prevInd);
                devTemplate.Cmds.Insert(prevInd + 1, prevCmd);

                cmdsNode.Nodes.RemoveAt(prevInd);
                cmdsNode.Nodes.Insert(prevInd + 1, prevNode);
            }

            // установка доступности кнопок
            btnMoveUp.Enabled = selNode.PrevNode != null;
            btnMoveDown.Enabled = selNode.NextNode != null;

            // установка признака изменения
            Modified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            // перемещение объекта вниз
            TreeNode nextNode = selNode.NextNode;
            int nextInd = nextNode.Index;

            if (selElemGroup != null)
            {
                // перемещение группы элементов вниз
                Modbus.ElemGroup nextElemGroup = nextNode.Tag as Modbus.ElemGroup;

                devTemplate.ElemGroups.RemoveAt(nextInd);
                devTemplate.ElemGroups.Insert(nextInd - 1, nextElemGroup);

                grsNode.Nodes.RemoveAt(nextInd);
                grsNode.Nodes.Insert(nextInd - 1, nextNode);

                UpdateSignals(nextNode);
            }
            else if (selElemInfo != null)
            {
                // перемещение элемента вниз
                ElemInfo nextElemInfo = nextNode.Tag as ElemInfo;

                selElemInfo.ElemGroup.Elems.RemoveAt(nextInd);
                selElemInfo.ElemGroup.Elems.Insert(nextInd - 1, nextElemInfo.Elem);

                TreeNode grNode = selNode.Parent;
                grNode.Nodes.RemoveAt(nextInd);
                grNode.Nodes.Insert(nextInd - 1, nextNode);

                UpdateElemNodes(grNode);
                ShowElemProps(selElemInfo);
            }
            else if (selCmd != null)
            {
                // перемещение команды вниз
                Modbus.Cmd nextCmd = nextNode.Tag as Modbus.Cmd;

                devTemplate.Cmds.RemoveAt(nextInd);
                devTemplate.Cmds.Insert(nextInd - 1, nextCmd);

                cmdsNode.Nodes.RemoveAt(nextInd);
                cmdsNode.Nodes.Insert(nextInd - 1, nextNode);
            }

            // установка доступности кнопок
            btnMoveUp.Enabled = selNode.PrevNode != null;
            btnMoveDown.Enabled = selNode.NextNode != null;

            // установка признака изменения
            Modified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selElemGroup != null)
            {
                // удаление группы элементов
                devTemplate.ElemGroups.Remove(selElemGroup);
                grsNode.Nodes.Remove(selNode);
            }
            else if (selElemInfo != null)
            {
                // удаление элемента
                Modbus.ElemGroup elemGroup = selElemInfo.ElemGroup;
                elemGroup.Elems.Remove(selElemInfo.Elem);
                TreeNode grNode = selNode.Parent;
                grsNode.Nodes.Remove(selNode);
                
                UpdateElemNodes(grNode);
                UpdateSignals(grNode);
                ShowElemProps(selElemInfo);
            }
            else if (selCmd != null)
            {
                // удаление команды
                devTemplate.Cmds.Remove(selCmd);
                cmdsNode.Nodes.Remove(selNode);
            }

            // установка признака изменения
            Modified = true;
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // отображение выбранного объекта и его свойств
            selNode = e.Node;
            object tag = selNode.Tag;
            selElemGroup = tag as Modbus.ElemGroup;
            selElemInfo = tag as ElemInfo;
            selCmd = tag as Modbus.Cmd;

            if (selElemGroup != null)
                ShowElemGroupProps(selElemGroup);
            else if (selElemInfo != null)
                ShowElemProps(selElemInfo);
            else if (selCmd != null)
                ShowCmdProps(selCmd);
            else if (selNode == grsNode)
                ShowElemGroupProps(null);
            else if (selNode == cmdsNode)
                ShowCmdProps(null);
            else // не выполняется
                DisableProps();

            // установка доступности кнопок
            btnAddElem.Enabled = selElemGroup != null || selElemInfo != null;
            bool nodeIsOk = selElemGroup != null || selCmd != null ||
                selElemInfo != null && selElemInfo.ElemGroup.Elems.Count > 1 /*последний не удалять*/;
            btnMoveUp.Enabled = nodeIsOk && selNode.PrevNode != null;
            btnMoveDown.Enabled = nodeIsOk && selNode.NextNode != null;
            btnDelete.Enabled = nodeIsOk;
        }


        private void cbGrTableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // изменение типа таблицы данных группы элементов
            if (procChangedEv && selElemGroup != null)
            {
                Modbus.TableTypes tableType = (Modbus.TableTypes)cbGrTableType.SelectedIndex;
                int maxElemCnt = Modbus.ElemGroup.GetMaxElemCnt(tableType);

                bool cancel = selElemGroup.Elems.Count > maxElemCnt && 
                    MessageBox.Show(string.Format(KpPhrases.ElemRemoveWarning, maxElemCnt), CommonPhrases.QuestionCaption,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes;

                if (cancel)
                {
                    procChangedEv = false;
                    cbGrTableType.SelectedIndex = (int)selElemGroup.TableType;
                    procChangedEv = true;
                }
                else
                {
                    // ограничение макс. количества элементов в группе
                    if (numGrElemCnt.Value > maxElemCnt)
                        numGrElemCnt.Value = maxElemCnt;
                    numGrElemCnt.Maximum = maxElemCnt;

                    // установка типа таблицы данных
                    selElemGroup.TableType = tableType;

                    // установка типа элементов группы по умолчанию
                    Modbus.ElemTypes elemType = selElemGroup.DefElemType;
                    foreach (Modbus.Elem elem in selElemGroup.Elems)
                        elem.ElemType = elemType;

                    // обновление узлов дерева
                    UpdateElemGroupNode();
                    UpdateElemNodes();

                    Modified = true;
                }
            }
        }

        private void numGrAddress_ValueChanged(object sender, EventArgs e)
        {
            // изменение адреса начального элемента в группе
            if (procChangedEv && selElemGroup != null)
            {
                selElemGroup.Address = (ushort)(numGrAddress.Value - 1);
                UpdateElemNodes();
                Modified = true;
            }
        }

        private void numGrElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // изменение количества элементов в группе
            if (procChangedEv && selElemGroup != null)
            {
                treeView.BeginUpdate();
                int elemCnt = selElemGroup.Elems.Count;
                int newElemCnt = (int)numGrElemCnt.Value;

                if (elemCnt < newElemCnt)
                {
                    // добавление новых элементов
                    Modbus.ElemTypes elemType = selElemGroup.DefElemType;
                    ushort elemLen = (ushort)Modbus.Elem.GetElemLength(elemType);
                    ushort elemAddr = selElemGroup.Address;

                    for (int elemInd = 0; elemInd < newElemCnt; elemInd++)
                    {
                        if (elemInd < elemCnt)
                        {
                            elemAddr += (ushort)selElemGroup.Elems[elemInd].Length;
                        }
                        else
                        {
                            ElemInfo elemInfo = new ElemInfo();
                            elemInfo.Elem = new Modbus.Elem() { ElemType = elemType };
                            elemInfo.Address = elemAddr;
                            elemInfo.ElemGroup = selElemGroup;

                            selElemGroup.Elems.Add(elemInfo.Elem);
                            selNode.Nodes.Add(NewElemNode(elemInfo));
                            elemAddr += elemLen;
                        }
                    }
                }
                else if (elemCnt > newElemCnt)
                {
                    // удаление лишних элементов
                    for (int i = newElemCnt; i < elemCnt; i++)
                    {
                        selElemGroup.Elems.RemoveAt(newElemCnt);
                        selNode.Nodes.RemoveAt(newElemCnt);
                    }
                }

                UpdateSignals(selNode);
                Modified = true;
                treeView.EndUpdate();
            }
        }

        private void txtGrName_TextChanged(object sender, EventArgs e)
        {
            // изменение наименования группы элементов
            if (procChangedEv && selElemGroup != null)
            {
                selElemGroup.Name = txtGrName.Text;
                UpdateElemGroupNode();
                Modified = true;
            }
        }

        private void txtElemName_TextChanged(object sender, EventArgs e)
        {
            // изменение наименования элемента
            if (procChangedEv && selElemInfo != null)
            {
                selElemInfo.Elem.Name = txtElemName.Text;
                selNode.Text = selElemInfo.Caption;
                Modified = true;
            }
        }

        private void rbType_CheckedChanged(object sender, EventArgs e)
        {
            // изменение типа элемента
            if (procChangedEv && selElemInfo != null)
            {
                if (rbUShort.Checked)
                    selElemInfo.Elem.ElemType = Modbus.ElemTypes.UShort;
                else if (rbShort.Checked)
                    selElemInfo.Elem.ElemType = Modbus.ElemTypes.Short;
                else if (rbUInt.Checked)
                    selElemInfo.Elem.ElemType = Modbus.ElemTypes.UInt;
                else if (rbInt.Checked)
                    selElemInfo.Elem.ElemType = Modbus.ElemTypes.Int;
                else if (rbFloat.Checked)
                    selElemInfo.Elem.ElemType = Modbus.ElemTypes.Float;
                else
                    selElemInfo.Elem.ElemType = Modbus.ElemTypes.Bool;

                txtElemAddress.Text = selElemInfo.AddressRange;
                selNode.Text = selElemInfo.Caption;
                UpdateElemNodes(selNode.Parent);
                Modified = true;
            }
        }


        private void cbCmdTableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // изменение типа таблицы данных команды
            if (procChangedEv && selCmd != null)
            {
                selCmd.TableType = cbCmdTableType.SelectedIndex == 0 ?
                    Modbus.TableTypes.Coils : Modbus.TableTypes.HoldingRegisters;

                // ограничение макс. количества элементов в группе
                int maxElemCnt = Modbus.ElemGroup.GetMaxElemCnt(selCmd.TableType);
                if (numGrElemCnt.Value > maxElemCnt)
                    numGrElemCnt.Value = maxElemCnt;
                numGrElemCnt.Maximum = maxElemCnt;

                // обновление узла дерева команды
                UpdateCmdNode();

                Modified = true;
            }
        }

        private void chkCmdMultiple_CheckedChanged(object sender, EventArgs e)
        {
            // изменение множественности команды
            if (procChangedEv && selCmd != null)
            {
                selCmd.Multiple = chkCmdMultiple.Checked;
                numCmdElemCnt.Enabled = selCmd.Multiple;
                if (!selCmd.Multiple)
                    numCmdElemCnt.Value = 1;
                Modified = true;
            }
        }

        private void numCmdAddress_ValueChanged(object sender, EventArgs e)
        {
            // изменение адреса команды
            if (procChangedEv && selCmd != null)
            {
                selCmd.Address = (ushort)(numCmdAddress.Value - 1);
                UpdateCmdNode();
                Modified = true;
            }
        }

        private void numCmdElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // изменение количества элементов команды
            if (procChangedEv && selCmd != null)
            {
                selCmd.ElemCnt = (int)numCmdElemCnt.Value;
                Modified = true;
            }
        }

        private void numCmdNum_ValueChanged(object sender, EventArgs e)
        {
            // изменение номера команды КП
            if (procChangedEv && selCmd != null)
            {
                selCmd.CmdNum = (int)numCmdNum.Value;
                Modified = true;
            }
        }

        private void txtCmdName_TextChanged(object sender, EventArgs e)
        {
            // изменение наименования команды
            if (procChangedEv && selCmd != null)
            {
                selCmd.Name = txtCmdName.Text;
                UpdateCmdNode();
                Modified = true;
            }
        }
    }
}
