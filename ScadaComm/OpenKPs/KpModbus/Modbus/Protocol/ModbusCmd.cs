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
 * Summary  : Modbus command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2021
 */

using System;
using System.Xml;

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Modbus command.
    /// <para>Команда Modbus.</para>
    /// </summary>
    public class ModbusCmd : DataUnit
    {
        private string reqDescr; // the command description


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private ModbusCmd()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModbusCmd(TableType tableType, bool multiple)
            : base(tableType)
        {
            if (!(tableType == TableType.Coils || tableType == TableType.HoldingRegisters))
                throw new InvalidOperationException(ModbusPhrases.IllegalDataTable);

            reqDescr = "";
            Multiple = multiple;
            ElemType = DefElemType;
            ElemCnt = 1;
            ByteOrder = null;
            ByteOrderStr = "";
            CmdNum = 1;
            Value = 0;
            Data = null;

            // определение кодов функций
            UpdateFuncCode();
            ExcFuncCode = (byte)(FuncCode | 0x80);
        }


        /// <summary>
        /// Получить описание запроса для выполнения команды
        /// </summary>
        public override string ReqDescr
        {
            get
            {
                if (reqDescr == "")
                    reqDescr = string.Format(ModbusPhrases.Command,
                        string.IsNullOrEmpty(Name) ? "" : " \"" + Name + "\"");
                return reqDescr;
            }
        }

        /// <summary>
        /// Получить или установить признак множественной команды
        /// </summary>
        public bool Multiple { get; set; }

        /// <summary>
        /// Получить или установить тип элементов команды
        /// </summary>
        public ElemType ElemType { get; set; }

        /// <summary>
        /// Получить признак, что команды разрешено использование типов
        /// </summary>
        public override bool ElemTypeEnabled
        {
            get
            {
                return TableType == TableType.HoldingRegisters && Multiple;
            }
        }

        /// <summary>
        /// Получить или установить количество элементов, устанавливаемое командой
        /// </summary>
        public int ElemCnt { get; set; }

        /// <summary>
        /// Получить или установить массив, определяющий порядок байт
        /// </summary>
        public int[] ByteOrder { get; set; }

        /// <summary>
        /// Получить или установить строковую запись порядка байт
        /// </summary>
        public string ByteOrderStr { get; set; }

        /// <summary>
        /// Получить или установить номер команды КП
        /// </summary>
        public int CmdNum { get; set; }

        /// <summary>
        /// Получить или установить значение команды
        /// </summary>
        public ushort Value { get; set; }

        /// <summary>
        /// Получить или установить данные множественной команды
        /// </summary>
        public byte[] Data { get; set; }


        /// <summary>
        /// Инициализировать PDU запроса, рассчитать длину ответа
        /// </summary>
        public override void InitReqPDU()
        {
            if (Multiple)
            {
                // формирование PDU для команды WriteMultipleCoils или WriteMultipleRegisters
                int quantity;   // quantity of registers
                int dataLength; // data length in bytes

                if (TableType == TableType.Coils)
                {
                    quantity = ElemCnt;
                    dataLength = (ElemCnt % 8 == 0) ? ElemCnt / 8 : ElemCnt / 8 + 1;
                }
                else
                {
                    quantity = ElemCnt * ModbusUtils.GetQuantity(ElemType);
                    dataLength = quantity * 2;
                }

                ReqPDU = new byte[6 + dataLength];
                ReqPDU[0] = FuncCode;
                ReqPDU[1] = (byte)(Address / 256);
                ReqPDU[2] = (byte)(Address % 256);
                ReqPDU[3] = (byte)(quantity / 256);
                ReqPDU[4] = (byte)(quantity % 256);
                ReqPDU[5] = (byte)dataLength;

                ModbusUtils.ApplyByteOrder(Data, 0, ReqPDU, 6, dataLength, ByteOrder, false);

                // установка длины ответа
                RespPduLen = 5;
            }
            else
            {
                // формирование PDU для команды WriteSingleCoil или WriteSingleRegister
                int dataLength = TableType == TableType.Coils ? 2 : ModbusUtils.GetDataLength(ElemType);
                ReqPDU = new byte[3 + dataLength];
                ReqPDU[0] = FuncCode;
                ReqPDU[1] = (byte)(Address / 256);
                ReqPDU[2] = (byte)(Address % 256);

                if (TableType == TableType.Coils)
                {
                    ReqPDU[3] = Value > 0 ? (byte)0xFF : (byte)0x00;
                    ReqPDU[4] = 0x00;
                }
                else
                {

                    byte[] data = dataLength == 2 ?
                        new byte[] // standard Modbus
                        {
                            (byte)(Value / 256),
                            (byte)(Value % 256)
                        } :
                        Data;

                    ModbusUtils.ApplyByteOrder(data, 0, ReqPDU, 3, dataLength, ByteOrder, false);
                }

                // установка длины ответа
                RespPduLen = ReqPDU.Length; // echo
            }
        }

        /// <summary>
        /// Расшифровать PDU ответа
        /// </summary>
        public override bool DecodeRespPDU(byte[] buffer, int offset, int length, out string errMsg)
        {
            if (base.DecodeRespPDU(buffer, offset, length, out errMsg))
            {
                if (buffer[offset + 1] == ReqPDU[1] && buffer[offset + 2] == ReqPDU[2] &&
                    buffer[offset + 3] == ReqPDU[3] && buffer[offset + 4] == ReqPDU[4])
                {
                    return true;
                }
                else
                {
                    errMsg = ModbusPhrases.IncorrectPduData;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Loads the command from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement cmdElem)
        {
            if (cmdElem == null)
                throw new ArgumentNullException("cmdElem");

            Address = (ushort)cmdElem.GetAttrAsInt("address");
            ElemType = cmdElem.GetAttrAsEnum("elemType", DefElemType);
            ElemCnt = cmdElem.GetAttrAsInt("elemCnt", 1);
            Name = cmdElem.GetAttribute("name");
            CmdNum = cmdElem.GetAttrAsInt("cmdNum");

            if (ByteOrderEnabled)
            {
                ByteOrderStr = cmdElem.GetAttribute("byteOrder");
                ByteOrder = ModbusUtils.ParseByteOrder(ByteOrderStr);
            }
        }

        /// <summary>
        /// Saves the command into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement cmdElem)
        {
            if (cmdElem == null)
                throw new ArgumentNullException("cmdElem");

            cmdElem.SetAttribute("tableType", TableType);
            cmdElem.SetAttribute("multiple", Multiple);
            cmdElem.SetAttribute("address", Address);

            if (ElemTypeEnabled)
                cmdElem.SetAttribute("elemType", ElemType.ToString().ToLowerInvariant());

            if (Multiple)
                cmdElem.SetAttribute("elemCnt", ElemCnt);

            if (ByteOrderEnabled)
                cmdElem.SetAttribute("byteOrder", ByteOrderStr);

            cmdElem.SetAttribute("cmdNum", CmdNum);
            cmdElem.SetAttribute("name", Name);
        }

        /// <summary>
        /// Copies the command properties from the source command.
        /// </summary>
        public virtual void CopyFrom(ModbusCmd srcCmd)
        {
            if (srcCmd == null)
                throw new ArgumentNullException("srcCmd");

            ElemCnt = srcCmd.ElemCnt;
            Address = srcCmd.Address;
            Name = srcCmd.Name;
            CmdNum = srcCmd.CmdNum;
        }

        /// <summary>
        /// Обновить код функции в соответствии с типом таблицы данных
        /// </summary>
        public void UpdateFuncCode()
        {
            if (TableType == TableType.Coils)
                FuncCode = Multiple ? FuncCodes.WriteMultipleCoils : FuncCodes.WriteSingleCoil;
            else
                FuncCode = Multiple ? FuncCodes.WriteMultipleRegisters : FuncCodes.WriteSingleRegister;
        }

        /// <summary>
        /// Установить данные команды, преобразовав их в зависимости от типа элементов команды
        /// </summary>
        public void SetCmdData(double cmdVal)
        {
            bool reverse = true;

            switch (ElemType)
            {
                case ElemType.UShort:
                    Data = BitConverter.GetBytes((ushort)cmdVal);
                    break;
                case ElemType.Short:
                    Data = BitConverter.GetBytes((short)cmdVal);
                    break;
                case ElemType.UInt:
                    Data = BitConverter.GetBytes((uint)cmdVal);
                    break;
                case ElemType.Int:
                    Data = BitConverter.GetBytes((int)cmdVal);
                    break;
                case ElemType.ULong:
                    Data = BitConverter.GetBytes((ulong)cmdVal);
                    break;
                case ElemType.Long:
                    Data = BitConverter.GetBytes((long)cmdVal);
                    break;
                case ElemType.Float:
                    Data = BitConverter.GetBytes((float)cmdVal);
                    break;
                case ElemType.Double:
                    Data = BitConverter.GetBytes(cmdVal);
                    break;
                default:
                    Data = BitConverter.GetBytes(cmdVal);
                    reverse = false;
                    break;
            }

            if (reverse)
                Array.Reverse(Data);
        }
    }
}
