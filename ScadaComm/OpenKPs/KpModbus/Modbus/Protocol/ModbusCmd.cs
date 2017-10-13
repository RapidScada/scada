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
 * Summary  : Modbus command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using System;

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Modbus command
    /// <para>Команда Modbus</para>
    /// </summary>
    public class ModbusCmd : DataUnit
    {
        private string reqDescr; // описание команды


        /// <summary>
        /// Конструктор
        /// </summary>
        private ModbusCmd()
            : base()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ModbusCmd(TableTypes tableType, bool multiple = false, int elemCnt = 1)
            : base(tableType)
        {
            if (!(tableType == TableTypes.Coils || tableType == TableTypes.HoldingRegisters))
                throw new InvalidOperationException(ModbusPhrases.IllegalDataTable);

            reqDescr = "";
            Multiple = multiple;
            ElemType = DefElemType;
            ElemCnt = elemCnt;
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
        public ElemTypes ElemType { get; set; }

        /// <summary>
        /// Получить признак, что команды разрешено использование типов
        /// </summary>
        public override bool ElemTypeEnabled
        {
            get
            {
                return TableType == TableTypes.HoldingRegisters && Multiple;
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
                int byteCnt = TableType == TableTypes.Coils ?
                    ((ElemCnt % 8 == 0) ? ElemCnt / 8 : ElemCnt / 8 + 1) : 
                    ElemCnt * 2;

                ReqPDU = new byte[6 + byteCnt];
                ReqPDU[0] = FuncCode;
                ReqPDU[1] = (byte)(Address / 256);
                ReqPDU[2] = (byte)(Address % 256);
                ReqPDU[3] = (byte)(ElemCnt / 256);
                ReqPDU[4] = (byte)(ElemCnt % 256);
                ReqPDU[5] = (byte)byteCnt;

                ModbusUtils.ApplyByteOrder(Data, ReqPDU, 6, byteCnt, ByteOrder);
            }
            else
            {
                // формирование PDU для команды WriteSingleCoil или WriteSingleRegister
                ReqPDU = new byte[5];
                ReqPDU[0] = FuncCode;
                ReqPDU[1] = (byte)(Address / 256);
                ReqPDU[2] = (byte)(Address % 256);

                if (TableType == TableTypes.Coils)
                {
                    ReqPDU[3] = Value > 0 ? (byte)0xFF : (byte)0x00;
                    ReqPDU[4] = 0x00;
                }
                else
                {
                    byte[] data = new byte[] 
                    {
                        (byte)(Value / 256),
                        (byte)(Value % 256)
                    };
                    ModbusUtils.ApplyByteOrder(data, ReqPDU, 3, 2, ByteOrder);
                }
            }

            // установка длины ответа
            RespPduLen = 5;
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
        /// Обновить код функции в соответствии с типом таблицы данных
        /// </summary>
        public void UpdateFuncCode()
        {
            if (TableType == TableTypes.Coils)
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
                case ElemTypes.UShort:
                    Data = BitConverter.GetBytes((ushort)cmdVal);
                    break;
                case ElemTypes.Short:
                    Data = BitConverter.GetBytes((short)cmdVal);
                    break;
                case ElemTypes.UInt:
                    Data = BitConverter.GetBytes((uint)cmdVal);
                    break;
                case ElemTypes.Int:
                    Data = BitConverter.GetBytes((int)cmdVal);
                    break;
                case ElemTypes.ULong:
                    Data = BitConverter.GetBytes((ulong)cmdVal);
                    break;
                case ElemTypes.Long:
                    Data = BitConverter.GetBytes((long)cmdVal);
                    break;
                case ElemTypes.Float:
                    Data = BitConverter.GetBytes((float)cmdVal);
                    break;
                case ElemTypes.Double:
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
