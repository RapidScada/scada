/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : The control for editing element group
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using Scada.Comm.Devices.Modbus.Protocol;
using Scada.UI;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Comm.Devices.Modbus.UI
{
    /// <summary>
    /// The control for editing element group
    /// <para>Элемент управления для редактирования группы элементов</para>
    /// </summary>
    public partial class CtrlElemGroup : UserControl
    {
        private ElemGroup elemGroup;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlElemGroup()
        {
            InitializeComponent();
            elemGroup = null;
        }


        /// <summary>
        /// Получить или установить редактируемую группу элементов
        /// </summary>
        public ElemGroup ElemGroup
        {
            get
            {
                return elemGroup;
            }
            set
            {
                if (value != null)
                    elemGroup = null; // чтобы не вызывалось событие ObjectChanged

                ShowElemGroupProps(value);
                elemGroup = value;
            }
        }
        

        /// <summary>
        /// Отобразить свойства группы элементов
        /// </summary>
        private void ShowElemGroupProps(ElemGroup elemGroup)
        {
            if (elemGroup == null)
            {
                chkGrActive.Checked = false;
                txtGrName.Text = "";
                cbGrTableType.SelectedIndex = 0;
                numGrAddress.Value = 1;
                numGrElemCnt.Value = 1;
                gbElemGroup.Enabled = false;
            }
            else
            {
                chkGrActive.Checked = elemGroup.Active;
                txtGrName.Text = elemGroup.Name;
                cbGrTableType.SelectedIndex = (int)elemGroup.TableType;
                numGrAddress.Value = elemGroup.Address + 1;
                numGrElemCnt.Value = 1;
                numGrElemCnt.Maximum = ElemGroup.GetMaxElemCnt(elemGroup.TableType);
                numGrElemCnt.Value = elemGroup.Elems.Count;
                gbElemGroup.Enabled = true;
            }
        }

        /// <summary>
        /// Вызвать событие ObjectChanged
        /// </summary>
        private void OnObjectChanged()
        {
            ObjectChanged?.Invoke(this, elemGroup);
        }

        /// <summary>
        /// Установить фокус ввода
        /// </summary>
        public void SetFocus()
        {
            txtGrName.Select();
        }


        /// <summary>
        /// Событие возникающее при изменении свойств редактируемого объекта
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler ObjectChanged;


        private void chkGrActive_CheckedChanged(object sender, EventArgs e)
        {
            // изменение активности группы элементов
            if (elemGroup != null)
            {
                elemGroup.Active = chkGrActive.Checked;
                OnObjectChanged();
            }
        }

        private void txtGrName_TextChanged(object sender, EventArgs e)
        {
            // изменение наименования группы элементов
            if (elemGroup != null)
            {
                elemGroup.Name = txtGrName.Text;
                OnObjectChanged();
            }
        }

        private void cbGrTableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // изменение типа таблицы данных группы элементов
            if (elemGroup != null)
            {
                TableTypes tableType = (TableTypes)cbGrTableType.SelectedIndex;
                int maxElemCnt = ElemGroup.GetMaxElemCnt(tableType);

                bool cancel = elemGroup.Elems.Count > maxElemCnt &&
                    MessageBox.Show(string.Format(KpPhrases.ElemRemoveWarning, maxElemCnt), 
                        CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, 
                        MessageBoxIcon.Question) != DialogResult.Yes;

                if (cancel)
                {
                    cbGrTableType.SelectedIndexChanged -= cbGrTableType_SelectedIndexChanged;
                    cbGrTableType.SelectedIndex = (int)elemGroup.TableType;
                    cbGrTableType.SelectedIndexChanged += cbGrTableType_SelectedIndexChanged;
                }
                else
                {
                    // ограничение макс. количества элементов в группе
                    if (numGrElemCnt.Value > maxElemCnt)
                        numGrElemCnt.Value = maxElemCnt;
                    numGrElemCnt.Maximum = maxElemCnt;

                    // установка типа таблицы данных
                    elemGroup.TableType = tableType;

                    // установка типа элементов группы по умолчанию
                    ElemTypes elemType = elemGroup.DefElemType;
                    foreach (Elem elem in elemGroup.Elems)
                        elem.ElemType = elemType;

                    // обновление узлов дерева
                    //UpdateElemGroupNode();
                    //UpdateElemNodes();

                    OnObjectChanged();
                }
            }
        }

        private void numGrAddress_ValueChanged(object sender, EventArgs e)
        {
            // изменение адреса начального элемента в группе
            if (elemGroup != null)
            {
                elemGroup.Address = (ushort)(numGrAddress.Value - 1);
                //UpdateElemNodes();
                OnObjectChanged();
            }
        }

        private void numGrElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // изменение количества элементов в группе
            /*if (elemGroup != null)
            {
                //treeView.BeginUpdate();
                int elemCnt = elemGroup.Elems.Count;
                int newElemCnt = (int)numGrElemCnt.Value;

                if (elemCnt < newElemCnt)
                {
                    // добавление новых элементов
                    ElemTypes elemType = elemGroup.DefElemType;
                    ushort elemLen = (ushort)Elem.GetElemLength(elemType);
                    ushort elemAddr = elemGroup.Address;

                    for (int elemInd = 0; elemInd < newElemCnt; elemInd++)
                    {
                        if (elemInd < elemCnt)
                        {
                            elemAddr += (ushort)elemGroup.Elems[elemInd].Length;
                        }
                        else
                        {
                            ElemInfo elemInfo = new ElemInfo();
                            elemInfo.Elem = new Elem() { ElemType = elemType };
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
                OnObjectChanged();
                //treeView.EndUpdate();
            }*/
        }
    }
}
