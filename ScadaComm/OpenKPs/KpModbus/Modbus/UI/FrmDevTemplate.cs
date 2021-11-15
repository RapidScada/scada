/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Modified : 2021
 */

using Scada.Comm.Devices.Modbus.Protocol;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.Modbus.UI
{
    /// <summary>
    /// Editing device template form.
    /// <para>Форма редактирования шаблона устройства.</para>
    /// </summary>
    public partial class FrmDevTemplate : Form
    {
        /// <summary>
        /// Имя файла нового шаблона устройства
        /// </summary>
        private const string NewFileName = "KpModbus_NewTemplate.xml";

        private AppDirs appDirs;                 // директории приложения
        private UiCustomization uiCustomization; // the customization object
        private string initialFileName;          // имя файла шаблона для открытия при запуске формы
        private string fileName;                 // имя файла шаблона устройства
        private bool saveOnly;                   // разрешена только команда сохранения при работе с файлами

        private DeviceTemplate template; // редактируемый шаблон устройства
        private bool modified;           // признак изменения шаблона устройства
        private ElemGroup selElemGroup;  // выбранная группа элементов
        private ElemInfo selElemInfo;    // информация о выбранном элементе
        private ModbusCmd selCmd;        // выбранная команда
        private TreeNode selNode;        // выбранный узел дерева
        private TreeNode grsNode;        // узел дерева "Группы элементов"
        private TreeNode cmdsNode;       // узел дерева "Команды"


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmDevTemplate()
        {
            InitializeComponent();

            appDirs = null;
            initialFileName = "";
            fileName = "";
            saveOnly = false;

            template = null;
            modified = false;
            selElemGroup = null;
            selElemInfo = null;
            selCmd = null;
            selNode = null;
            grsNode = treeView.Nodes["grsNode"];
            cmdsNode = treeView.Nodes["cmdsNode"];
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
                SetFormTitle();
                btnSave.Enabled = modified;
            }
        }


        /// <summary>
        /// Установить заголовок формы
        /// </summary>
        private void SetFormTitle()
        {
            Text = KpPhrases.TemplFormTitle + " - " + 
                (fileName == "" ? NewFileName : Path.GetFileName(fileName)) +
                (Modified ? "*" : "");
        }

        /// <summary>
        /// Загрузить шаблон устройства из файла
        /// </summary>
        private void LoadTemplate(string fname)
        {
            template = uiCustomization.TemplateFactory.CreateDeviceTemplate();
            fileName = fname;
            SetFormTitle();
            string errMsg;

            if (!template.Load(fname, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            FillTree();
        }

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
            selElemInfo = null;
            selCmd = null;
            selNode = null;
            ShowElemGroupProps(null);
            Modified = false;

            // приостановка отрисовки дерева
            treeView.BeginUpdate();

            // очистка дерева
            grsNode.Nodes.Clear();
            cmdsNode.Nodes.Clear();
            treeView.SelectedNode = grsNode;

            // заполнение узла групп элементов
            foreach (ElemGroup elemGroup in template.ElemGroups)
                grsNode.Nodes.Add(NewElemGroupNode(elemGroup));

            // заполнение узла команд
            foreach (ModbusCmd modbusCmd in template.Cmds)
                cmdsNode.Nodes.Add(NewCmdNode(modbusCmd));

            // раскрытие основных узлов дерева
            grsNode.Expand();
            cmdsNode.Expand();

            // возобновление отрисовки дерева
            treeView.EndUpdate();
        }

        /// <summary>
        /// Создать узел группы элементов
        /// </summary>
        private TreeNode NewElemGroupNode(ElemGroup elemGroup)
        {
            string name = elemGroup.Name == "" ? KpPhrases.DefGrName : elemGroup.Name;
            TreeNode grNode = new TreeNode(name + " (" + ModbusUtils.GetTableTypeName(elemGroup.TableType) + ")");
            grNode.ImageKey = grNode.SelectedImageKey = elemGroup.Active ? "group.png" : "group_inactive.png";
            grNode.Tag = elemGroup;

            ushort elemAddr = elemGroup.Address;
            int elemSig = elemGroup.StartKPTagInd + 1;

            foreach (Elem elem in elemGroup.Elems)
            {
                ElemInfo elemInfo = new ElemInfo()
                {
                    Elem = elem,
                    ElemGroup = elemGroup,
                    Settings = template.Sett,
                    Address = elemAddr,
                    Signal = elemSig++
                };

                grNode.Nodes.Add(NewElemNode(elemInfo));
                elemAddr += (ushort)elem.Quantity;
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
        private TreeNode NewCmdNode(ModbusCmd modbusCmd)
        {
            TreeNode cmdNode = new TreeNode(GetCmdCaption(modbusCmd));
            cmdNode.ImageKey = cmdNode.SelectedImageKey = "cmd.png";
            cmdNode.Tag = modbusCmd;
            return cmdNode;
        }

        /// <summary>
        /// Получить обозначение команды в дереве
        /// </summary>
        private string GetCmdCaption(ModbusCmd modbusCmd)
        {
            return (string.IsNullOrEmpty(modbusCmd.Name) ? KpPhrases.DefCmdName : modbusCmd.Name) +
                " (" + ModbusUtils.GetTableTypeName(modbusCmd.TableType) + ", " +
                ModbusUtils.GetAddressRange(modbusCmd.Address, 
                    modbusCmd.ElemCnt * ModbusUtils.GetQuantity(modbusCmd.ElemType),
                    template.Sett.ZeroAddr, template.Sett.DecAddr) + ")";
        }

        /// <summary>
        /// Обновить узел выбранной группы элементов
        /// </summary>
        private void UpdateElemGroupNode()
        {
            if (selElemGroup != null)
            {
                selNode.ImageKey = selNode.SelectedImageKey = selElemGroup.Active ? "group.png" : "group_inactive.png";
                selNode.Text = (selElemGroup.Name == "" ? KpPhrases.DefGrName : selElemGroup.Name) + 
                    " (" + ModbusUtils.GetTableTypeName(selElemGroup.TableType) + ")";
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

            if (grNode.Tag is ElemGroup elemGroup)
            {
                ushort elemAddr = elemGroup.Address;
                int elemSig = elemGroup.StartKPTagInd + 1;

                foreach (TreeNode elemNode in grNode.Nodes)
                {
                    ElemInfo elemInfo = elemNode.Tag as ElemInfo;
                    elemInfo.Address = elemAddr;
                    elemInfo.Signal = elemSig++;
                    elemNode.Text = elemInfo.Caption;
                    elemAddr += (ushort)elemInfo.Elem.Quantity;
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
            if (!(startGrNode.Tag is ElemGroup))
                return;

            // определение начального индекса тегов КП
            TreeNode prevGrNode = startGrNode.PrevNode;
            ElemGroup prevElemGroup = prevGrNode == null ? null : prevGrNode.Tag as ElemGroup;
            int tagInd = prevElemGroup == null ? 0 : prevElemGroup.StartKPTagInd + prevElemGroup.Elems.Count;

            // обновление групп и их элементов
            int grNodeCnt = grsNode.Nodes.Count;

            for (int i = startGrNode.Index; i < grNodeCnt; i++)
            {
                TreeNode grNode = grsNode.Nodes[i];
                ElemGroup elemGroup = grNode.Tag as ElemGroup;
                int elemSig = tagInd + 1;
                elemGroup.StartKPTagInd = tagInd;
                tagInd += elemGroup.Elems.Count;

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
                selNode.Text = GetCmdCaption(selCmd);
        }


        /// <summary>
        /// Отобразить свойства группы элементов
        /// </summary>
        private void ShowElemGroupProps(ElemGroup elemGroup)
        {
            ctrlElemGroup.Visible = true;
            ctrlElemGroup.Settings = template.Sett;
            ctrlElemGroup.ElemGroup = elemGroup;
            ctrlElem.Visible = false;
            ctrlCmd.Visible = false;
        }

        /// <summary>
        /// Отобразить свойства элемента
        /// </summary>
        private void ShowElemProps(ElemInfo elemInfo)
        {
            ctrlElemGroup.Visible = false;
            ctrlElem.Visible = true;
            ctrlElem.ElemInfo = elemInfo;
            ctrlCmd.Visible = false;
        }

        /// <summary>
        /// Отобразить свойства команды
        /// </summary>
        private void ShowCmdProps(ModbusCmd modbusCmd)
        {
            ctrlElemGroup.Visible = false;
            ctrlElem.Visible = false;
            ctrlCmd.Visible = true;
            ctrlCmd.Settings = template.Sett;
            ctrlCmd.ModbusCmd = modbusCmd;
        }

        /// <summary>
        /// Заблокировать редактирование свойств
        /// </summary>
        private void DisableProps()
        {
            ctrlElemGroup.ElemGroup = null;
            ctrlElem.ElemInfo = null;
            ctrlCmd.ModbusCmd = null;
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
                if (template.Save(newFileName, out errMsg))
                {
                    fileName = newFileName;
                    Modified = false;
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
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


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(AppDirs appDirs, UiCustomization uiCustomization)
        {
            string fileName = "";
            ShowDialog(appDirs, uiCustomization, false, ref fileName);
        }

        /// <summary>
        /// Отобразить форму модально, открыв заданный файл
        /// </summary>
        public static void ShowDialog(AppDirs appDirs, UiCustomization uiCustomization, 
            bool saveOnly, ref string fileName)
        {
            FrmDevTemplate frmDevTemplate = new FrmDevTemplate
            {
                appDirs = appDirs ?? throw new ArgumentNullException("appDirs"),
                uiCustomization = uiCustomization ?? throw new ArgumentNullException("uiCustomization"),
                initialFileName = fileName,
                saveOnly = saveOnly
            };

            frmDevTemplate.ShowDialog();
            fileName = frmDevTemplate.fileName;
        }


        private void FrmDevTemplate_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Devices.Modbus.UI.FrmDevTemplate");
            openFileDialog.SetFilter(KpPhrases.TemplateFileFilter);
            saveFileDialog.SetFilter(KpPhrases.TemplateFileFilter);
            TranslateTree();

            // настройка элементов управления
            openFileDialog.InitialDirectory = appDirs.ConfigDir;
            saveFileDialog.InitialDirectory = appDirs.ConfigDir;
            btnEditSettingsExt.Visible = uiCustomization.ExtendedSettingsAvailable;
            ctrlElem.Top = ctrlCmd.Top = ctrlElemGroup.Top;

            if (saveOnly)
            {
                btnNew.Visible = false;
                btnOpen.Visible = false;
            }

            if (string.IsNullOrEmpty(initialFileName))
            {
                saveFileDialog.FileName = NewFileName;
                template = uiCustomization.TemplateFactory.CreateDeviceTemplate();
                FillTree();
            }
            else
            {
                saveFileDialog.FileName = initialFileName;
                LoadTemplate(initialFileName);
            }
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
                saveFileDialog.FileName = NewFileName;
                template = uiCustomization.TemplateFactory.CreateDeviceTemplate();
                fileName = "";
                SetFormTitle();
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
                    LoadTemplate(openFileDialog.FileName);
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
            ElemGroup elemGroup = template.CreateElemGroup(TableType.DiscreteInputs);
            elemGroup.Elems.Add(elemGroup.CreateElem());
            int ind = selNode != null && selNode.Tag is ElemGroup ? selNode.Index + 1 : template.ElemGroups.Count;
            template.ElemGroups.Insert(ind, elemGroup);

            // создание узла дерева группы элементов
            TreeNode grNode = NewElemGroupNode(elemGroup);
            grsNode.Nodes.Insert(ind, grNode);
            UpdateSignals(grNode);
            grNode.Expand();
            treeView.SelectedNode = grNode;
            ctrlElemGroup.SetFocus();

            // установка признака изменения
            Modified = true;
        }

        private void btnAddElem_Click(object sender, EventArgs e)
        {
            // создание элемента и добавление в шаблон устройства
            ElemGroup elemGroup = selElemGroup == null ? selElemInfo.ElemGroup : selElemGroup;
            int maxElemCnt = elemGroup.MaxElemCnt;

            if (elemGroup.Elems.Count >= maxElemCnt)
            {
                MessageBox.Show(string.Format(KpPhrases.ElemCntExceeded, maxElemCnt), 
                    CommonPhrases.WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ElemInfo elemInfo = new ElemInfo();
            elemInfo.Elem = elemGroup.CreateElem();
            elemInfo.Elem.ElemType = elemGroup.DefElemType;
            elemInfo.ElemGroup = elemGroup;
            elemInfo.Settings = template.Sett;
            int ind = selNode.Tag is ElemInfo ? selNode.Index + 1 : elemGroup.Elems.Count;
            elemGroup.Elems.Insert(ind, elemInfo.Elem);

            // создание узла дерева элемента
            TreeNode elemNode = NewElemNode(elemInfo);
            TreeNode grNode = selNode.Tag is ElemInfo ? selNode.Parent : selNode;
            grNode.Nodes.Insert(ind, elemNode);
            UpdateElemNodes(grNode);
            UpdateSignals(grNode);
            treeView.SelectedNode = elemNode;
            ctrlElem.SetFocus();

            // установка признака изменения
            Modified = true;
        }

        private void btnAddCmd_Click(object sender, EventArgs e)
        {
            // создание команды и добавление в шаблон устройства
            ModbusCmd modbusCmd = template.CreateModbusCmd(TableType.Coils, false);
            int ind = selNode != null && selNode.Tag is ModbusCmd ? selNode.Index + 1 : template.Cmds.Count;
            template.Cmds.Insert(ind, modbusCmd);

            // создание узла дерева команды
            TreeNode cmdNode = NewCmdNode(modbusCmd);
            cmdsNode.Nodes.Insert(ind, cmdNode);
            treeView.SelectedNode = cmdNode;
            ctrlCmd.SetFocus();

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
                ElemGroup prevElemGroup = prevNode.Tag as ElemGroup;

                template.ElemGroups.RemoveAt(prevInd);
                template.ElemGroups.Insert(prevInd + 1, prevElemGroup);

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
                ModbusCmd prevCmd = prevNode.Tag as ModbusCmd;

                template.Cmds.RemoveAt(prevInd);
                template.Cmds.Insert(prevInd + 1, prevCmd);

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
                ElemGroup nextElemGroup = nextNode.Tag as ElemGroup;

                template.ElemGroups.RemoveAt(nextInd);
                template.ElemGroups.Insert(nextInd - 1, nextElemGroup);

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
                ModbusCmd nextCmd = nextNode.Tag as ModbusCmd;

                template.Cmds.RemoveAt(nextInd);
                template.Cmds.Insert(nextInd - 1, nextCmd);

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
                template.ElemGroups.Remove(selElemGroup);
                grsNode.Nodes.Remove(selNode);
            }
            else if (selElemInfo != null)
            {
                // удаление элемента
                ElemGroup elemGroup = selElemInfo.ElemGroup;
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
                template.Cmds.Remove(selCmd);
                cmdsNode.Nodes.Remove(selNode);
            }

            // установка признака изменения
            Modified = true;
        }

        private void btnEditSettings_Click(object sender, EventArgs e)
        {
            // редактирование настроек шаблона
            if (FrmTemplateSettings.ShowDialog(template.Sett))
            {
                // полное обновление дерева
                FillTree();
                // установка признака изменения
                Modified = true;
            }
        }

        private void btnEditSettingsExt_Click(object sender, EventArgs e)
        {
            // редактирование расширенных настроек
            if (uiCustomization.ShowExtendedSettings(template))
            {
                FillTree();
                Modified = true;
            }
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // отображение выбранного объекта и его свойств
            selNode = e.Node;
            object tag = selNode.Tag;
            selElemGroup = tag as ElemGroup;
            selElemInfo = tag as ElemInfo;
            selCmd = tag as ModbusCmd;

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

        private void ctrlElemGroup_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            // установка признака изменения конфигурации
            Modified = true;

            // отображение изменений группы элементов в дереве
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                UpdateElemGroupNode();

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.ChildNodes))
                UpdateElemNodes();

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.UpdateSignals))
            {
                treeView.BeginUpdate();
                int oldElemCnt = selNode.Nodes.Count;
                int newElemCnt = selElemGroup.Elems.Count;

                if (oldElemCnt < newElemCnt)
                {
                    // добавление узлов дерева для новых элементов
                    ushort elemAddr = selElemGroup.Address;

                    for (int elemInd = 0; elemInd < newElemCnt; elemInd++)
                    {
                        Elem elem = selElemGroup.Elems[elemInd];

                        if (elemInd >= oldElemCnt)
                        {
                            ElemInfo elemInfo = new ElemInfo()
                            {
                                Elem = elem,
                                ElemGroup = selElemGroup,
                                Settings = template.Sett,
                                Address = elemAddr
                            };

                            selNode.Nodes.Add(NewElemNode(elemInfo));
                        }

                        elemAddr += (ushort)elem.Quantity;
                    }
                }
                else if (oldElemCnt > newElemCnt)
                {
                    // удаление лишних узлов дерева
                    for (int i = newElemCnt; i < oldElemCnt; i++)
                    {
                        selNode.Nodes.RemoveAt(newElemCnt);
                    }
                }

                UpdateSignals(selNode);
                treeView.EndUpdate();
            }
        }

        private void ctrlElem_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            // установка признака изменения конфигурации
            Modified = true;

            // отображение изменений элемента в дереве
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                selNode.Text = selElemInfo.Caption;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.NextSiblings))
                UpdateElemNodes(selNode.Parent);
        }

        private void ctrlCmd_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            // установка признака изменения конфигурации
            Modified = true;

            // отображение изменений команды в дереве
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                UpdateCmdNode();
        }
    }
}
