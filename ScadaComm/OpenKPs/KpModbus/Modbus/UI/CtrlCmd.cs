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
 * Summary  : The control for editing command
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
    /// The control for editing command
    /// <para>Элемент управления для редактирования команды</para>
    /// </summary>
    public partial class CtrlCmd : UserControl
    {
        private ModbusCmd modbusCmd;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlCmd()
        {
            InitializeComponent();
            modbusCmd = null;
            Settings = null;
        }


        /// <summary>
        /// Получить признак отображения адресов, начиная с 0
        /// </summary>
        private bool ZeroAddr
        {
            get
            {
                return Settings == null ? false : Settings.ZeroAddr;
            }
        }

        /// <summary>
        /// Получить смещение адреса
        /// </summary>
        private int AddrShift
        {
            get
            {
                return ZeroAddr ? 0 : 1;
            }
        }

        /// <summary>
        /// Получить признак отображения адресов в 10-тичной системе
        /// </summary>
        private bool DecAddr
        {
            get
            {
                return Settings == null ? false : Settings.DecAddr;
            }
        }

        /// <summary>
        /// Получить обозначение системы счисления адресов
        /// </summary>
        private string AddrNotation
        {
            get
            {
                return DecAddr ? "DEC" : "HEX";
            }
        }

        /// <summary>
        /// Получить или установить ссылку настройки шаблона
        /// </summary>
        public DeviceTemplate.Settings Settings { get; set; }

        /// <summary>
        /// Получить или установить редактируемую команду
        /// </summary>
        public ModbusCmd ModbusCmd
        {
            get
            {
                return modbusCmd;
            }
            set
            {
                modbusCmd = null; // чтобы не вызывалось событие ObjectChanged
                ShowCmdProps(value);
                modbusCmd = value;
            }
        }


        /// <summary>
        /// Отобразить свойства команды
        /// </summary>
        private void ShowCmdProps(ModbusCmd modbusCmd)
        {
            numCmdAddress.Value = 1;
            numCmdAddress.Minimum = AddrShift;
            numCmdAddress.Maximum = ushort.MaxValue + AddrShift;
            numCmdAddress.Hexadecimal = !DecAddr;
            ShowFuncCode(modbusCmd);

            if (modbusCmd == null)
            {
                cbCmdTableType.SelectedIndex = 0;
                numCmdAddress.Value = AddrShift;
                lblCmdAddressHint.Text = "";
                numCmdElemCnt.Value = 1;
                numCmdNum.Value = 1;
                txtCmdName.Text = "";
                txtCmdByteOrder.Text = "";
                gbCmd.Enabled = false;
            }
            else
            {
                cbCmdTableType.SelectedIndex = modbusCmd.TableType == TableTypes.Coils ? 0 : 1;
                chkCmdMultiple.Checked = modbusCmd.Multiple;
                numCmdAddress.Value = modbusCmd.Address + AddrShift;
                lblCmdAddressHint.Text = string.Format(KpPhrases.AddressHint, AddrNotation, AddrShift);
                numCmdElemCnt.Value = 1;
                numCmdElemCnt.Maximum = DataUnit.GetMaxElemCnt(modbusCmd.TableType);
                numCmdElemCnt.Value = modbusCmd.ElemCnt;
                numCmdElemCnt.Enabled = modbusCmd.Multiple;
                numCmdNum.Value = modbusCmd.CmdNum;
                txtCmdName.Text = modbusCmd.Name;
                gbCmd.Enabled = true;
            }
        }

        /// <summary>
        /// Отобразить код функции команды
        /// </summary>
        private void ShowFuncCode(ModbusCmd modbusCmd)
        {
            txtCmdFuncCode.Text = modbusCmd == null ? "" :
                string.Format("{0} ({1}H)", modbusCmd.FuncCode, modbusCmd.FuncCode.ToString("X2"));
        }

        /// <summary>
        /// Вызвать событие ObjectChanged
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(modbusCmd, changeArgument));
        }

        /// <summary>
        /// Установить фокус ввода
        /// </summary>
        public void SetFocus()
        {
            txtCmdName.Select();
        }


        /// <summary>
        /// Событие возникающее при изменении свойств редактируемого объекта
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler ObjectChanged;


        private void txtCmdName_TextChanged(object sender, EventArgs e)
        {
            // изменение наименования команды
            if (modbusCmd != null)
            {
                modbusCmd.Name = txtCmdName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void cbCmdTableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // изменение типа таблицы данных команды
            if (modbusCmd != null)
            {
                modbusCmd.TableType = cbCmdTableType.SelectedIndex == 0 ?
                    TableTypes.Coils :
                    TableTypes.HoldingRegisters;
                modbusCmd.UpdateFuncCode();
                ShowFuncCode(modbusCmd);

                // ограничение макс. количества элементов в группе
                int maxElemCnt = DataUnit.GetMaxElemCnt(modbusCmd.TableType);
                if (numCmdElemCnt.Value > maxElemCnt)
                    numCmdElemCnt.Value = maxElemCnt;
                numCmdElemCnt.Maximum = maxElemCnt;

                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void chkCmdMultiple_CheckedChanged(object sender, EventArgs e)
        {
            // изменение множественности команды
            if (modbusCmd != null)
            {
                modbusCmd.Multiple = chkCmdMultiple.Checked;
                modbusCmd.UpdateFuncCode();
                ShowFuncCode(modbusCmd);

                numCmdElemCnt.Enabled = modbusCmd.Multiple;
                if (!modbusCmd.Multiple)
                    numCmdElemCnt.Value = 1;

                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numCmdAddress_ValueChanged(object sender, EventArgs e)
        {
            // изменение адреса команды
            if (modbusCmd != null)
            {
                modbusCmd.Address = (ushort)(numCmdAddress.Value - 1);
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void numCmdElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // изменение количества элементов команды
            if (modbusCmd != null)
            {
                modbusCmd.ElemCnt = (int)numCmdElemCnt.Value;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void numCmdNum_ValueChanged(object sender, EventArgs e)
        {
            // изменение номера команды КП
            if (modbusCmd != null)
            {
                modbusCmd.CmdNum = (int)numCmdNum.Value;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
