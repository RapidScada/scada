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
 * Summary  : Group of Modbus elements
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using System;
using System.Collections.Generic;

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Group of Modbus elements
    /// <para>Группа элементов Modbus</para>
    /// </summary>
    public class ElemGroup : DataUnit
    {
        private string reqDescr; // описание запроса


        /// <summary>
        /// Конструктор
        /// </summary>
        private ElemGroup()
            : base()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ElemGroup(TableTypes tableType)
            : base(tableType)
        {
            reqDescr = "";
            Active = true;
            Elems = new List<Elem>();
            ElemVals = null;
            TotalElemLength = -1;
            StartKPTagInd = -1;
            StartSignal = 0;

            // определение кодов функций
            UpdateFuncCode();
            ExcFuncCode = (byte)(FuncCode | 0x80);
        }


        /// <summary>
        /// Получить или установить признак активности
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Получить список элементов в группе
        /// </summary>
        public List<Elem> Elems { get; private set; }

        /// <summary>
        /// Получить значения элементов в группе
        /// </summary>
        public byte[][] ElemVals { get; private set; }

        /// <summary>
        /// Получить суммарную длину элементов (количество адресов) в группе
        /// </summary>
        public int TotalElemLength { get; private set; }

        /// <summary>
        /// Получить тип элементов группы по умолчанию
        /// </summary>
        public ElemTypes DefElemType
        {
            get
            {
                return GetDefElemType(TableType);
            }
        }

        /// <summary>
        /// Получить описание запроса
        /// </summary>
        public override string ReqDescr
        {
            get
            {
                if (reqDescr == "")
                    reqDescr = string.Format(ModbusPhrases.Request,
                        string.IsNullOrEmpty(Name) ? "" : " \"" + Name + "\"");
                return reqDescr;
            }
        }

        /// <summary>
        /// Получить или установить индекс тега КП, соответствующего начальному элементу
        /// </summary>
        public int StartKPTagInd { get; set; }

        /// <summary>
        /// Получить или установить сигнал КП, соответствующий начальному элементу
        /// </summary>
        public int StartSignal { get; set; }


        /// <summary>
        /// Инициализировать PDU запроса, рассчитать длину ответа
        /// </summary>
        public override void InitReqPDU()
        {
            // определение суммарной длины запрашиваемых элементов
            TotalElemLength = 0;
            foreach (Elem elem in Elems)
                TotalElemLength += elem.Length;

            // формирование PDU
            ReqPDU = new byte[5];
            ReqPDU[0] = FuncCode;
            ReqPDU[1] = (byte)(Address / 256);
            ReqPDU[2] = (byte)(Address % 256);
            ReqPDU[3] = (byte)(TotalElemLength / 256);
            ReqPDU[4] = (byte)(TotalElemLength % 256);

            // рассчёт длины ответа
            if (TableType == TableTypes.DiscreteInputs || TableType == TableTypes.Coils)
            {
                int n = TotalElemLength / 8;
                if ((TotalElemLength % 8) > 0)
                    n++;
                RespPduLen = 2 + n;
                RespByteCnt = (byte)n;
            }
            else
            {
                RespPduLen = 2 + TotalElemLength * 2;
                RespByteCnt = (byte)(TotalElemLength * 2);
            }

            // инициализация массива значений элементов
            int elemCnt = Elems.Count;
            ElemVals = new byte[elemCnt][];

            for (int i = 0; i < elemCnt; i++)
            {
                Elem elem = Elems[i];
                byte[] elemVal = new byte[elem.ElemType == ElemTypes.Bool ? 1 : elem.Length * 2];
                Array.Clear(elemVal, 0, elemVal.Length);
                ElemVals[i] = elemVal;
            }
        }

        /// <summary>
        /// Расшифровать PDU ответа
        /// </summary>
        public override bool DecodeRespPDU(byte[] buffer, int offset, int length, out string errMsg)
        {
            if (base.DecodeRespPDU(buffer, offset, length, out errMsg))
            {
                if (buffer[offset + 1] == RespByteCnt)
                {
                    int len = ElemVals.Length;
                    int byteNum = offset + 2;

                    if (TableType == TableTypes.DiscreteInputs || TableType == TableTypes.Coils)
                    {
                        int bitNum = 0;
                        for (int elemInd = 0; elemInd < len; elemInd++)
                        {
                            ElemVals[elemInd][0] = ((buffer[byteNum] >> bitNum) & 0x01) > 0 ? (byte)1 : (byte)0;

                            if (++bitNum == 8)
                            {
                                bitNum = 0;
                                byteNum++;
                            }
                        }
                    }
                    else
                    {
                        for (int elemInd = 0; elemInd < len; elemInd++)
                        {
                            byte[] elemVal = ElemVals[elemInd];
                            int elemLen = Elems[elemInd].Length;
                            int elemValLen = elemLen * 2;
                            // копирование считанных байт в обратном порядке
                            for (int i = elemValLen - 1, j = byteNum; i >= 0; i--, j++)
                                elemVal[i] = buffer[j];
                            byteNum += elemValLen;
                        }
                    }

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
            switch (TableType)
            {
                case TableTypes.DiscreteInputs:
                    FuncCode = FuncCodes.ReadDiscreteInputs;
                    break;
                case TableTypes.Coils:
                    FuncCode = FuncCodes.ReadCoils;
                    break;
                case TableTypes.InputRegisters:
                    FuncCode = FuncCodes.ReadInputRegisters;
                    break;
                default: // TableTypes.HoldingRegisters:
                    FuncCode = FuncCodes.ReadHoldingRegisters;
                    break;
            }
        }

        /// <summary>
        /// Получить значение элемента в соответствии с его типом, преобразованное в double
        /// </summary>
        public double GetElemVal(int elemInd)
        {
            Elem elem = Elems[elemInd];
            byte[] elemVal = ElemVals[elemInd];
            byte[] buf;

            // перестановка байт в случае необходимости
            if (elem.ByteOrder == null)
            {
                buf = elemVal;
            }
            else
            {
                buf = new byte[elemVal.Length];
                ModbusUtils.ApplyByteOrder(elemVal, buf, elem.ByteOrder);
            }

            // расчёт значения
            switch (elem.ElemType)
            {
                case ElemTypes.Bool:
                    return buf[0] > 0 ? 1.0 : 0.0;
                case ElemTypes.UShort:
                    return BitConverter.ToUInt16(buf, 0);
                case ElemTypes.Short:
                    return BitConverter.ToInt16(buf, 0);
                case ElemTypes.UInt:
                    return BitConverter.ToUInt32(buf, 0);
                case ElemTypes.Int:
                    return BitConverter.ToInt32(buf, 0);
                case ElemTypes.ULong:
                    return BitConverter.ToUInt64(buf, 0);
                case ElemTypes.Long:
                    return BitConverter.ToInt64(buf, 0);
                case ElemTypes.Float:
                    return BitConverter.ToSingle(buf, 0);
                case ElemTypes.Double:
                    return BitConverter.ToDouble(buf, 0);
                default:
                    return 0.0;
            }
        }
    }
}
