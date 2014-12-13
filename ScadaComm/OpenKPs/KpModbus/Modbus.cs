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
 * Summary  : Modbus protocol implementation. The class version: 1.3
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace Scada.Comm.KP
{
    /// <summary>
    /// Modbus protocol implementation
    /// <para>Реализация протокола Modbus</para>
    /// </summary>
    public class Modbus
    {
        /// <summary>
        /// Режимы передачи данных
        /// </summary>
        public enum TransModes
        {
            /// <summary>
            /// Передача данных через последовательный порт в бинарном формате
            /// </summary>
            RTU,
            /// <summary>
            /// Передача данных через последовательный порт в символьном формате
            /// </summary>
            ASCII,
            /// <summary>
            /// Передача данных через локальную сеть по протоколу TCP/IP
            /// </summary>
            TCP
        }

        /// <summary>
        /// Типы таблиц данных
        /// </summary>
        public enum TableTypes
        {
            /// <summary>
            /// Дискретные входы (1 бит, только чтение, 1X-обращения)
            /// </summary>
            DiscreteInputs,
            /// <summary>
            /// Флаги (1 бит, чтение и запись, 0X-обращения)
            /// </summary>
            Coils,
            /// <summary>
            /// Входные регистры (16 бит, только чтение, 3X-обращения)
            /// </summary>
            InputRegisters,
            /// <summary>
            /// Регистры хранения (16 бит, чтение и запись, 4X-обращения)
            /// </summary>
            HoldingRegisters
        }

        /// <summary>
        /// Типы элементов
        /// </summary>
        public enum ElemTypes
        {
            /// <summary>
            /// Логическое значение
            /// </summary>
            Bool,
            /// <summary>
            /// 2-байтное целое без знака
            /// </summary>
            UShort,
            /// <summary>
            /// 2-байтное целое со знаком
            /// </summary>
            Short,
            /// <summary>
            /// 4-байтное целое без знака
            /// </summary>
            UInt,
            /// <summary>
            /// 4-байтное целое со знаком
            /// </summary>
            Int,
            /// <summary>
            /// 4-байтное вещественное с плавающей запятой
            /// </summary>
            Float
        }

        /// <summary>
        /// Используемые фразы
        /// </summary>
        public static class Phrases
        {
            /// <summary>
            /// Статический конструктор
            /// </summary>
            static Phrases()
            {
                if (Localization.UseRussian)
                {
                    IncorrectPduLength = "Некорректная длина PDU";
                    IncorrectPduFuncCode = "Некорректный код функции PDU";
                    IncorrectPduData = "Некорректные данные PDU";
                    Request = "Запрос значений группы элементов{0}";
                    Command = "Команда{0}";
                    DeviceError = "Ошибка устройства";
                    IllegalDataTable = "Недопустимая таблица данных для команды.";
                    LoadTemplateError = "Ошибка при загрузке шаблона устройства";
                    SaveTemplateError = "Ошибка при сохранении шаблона устройства";
                    ClearNetStreamError = "Ошибка при очистке сетевого потока";
                    OK = "OK!";
                    CrcError = "Ошибка CRC!";
                    LrcError = "Ошибка LRC!";
                    CommErrorWithExclamation = "Ошибка связи!";
                    CommError = "Ошибка связи";
                    IncorrectSymbol = "Некорректный символ!";
                    IncorrectAduLength = "Некорректная длина ADU!";
                    RequestImpossible = "Выполнение запроса невозможно, т.к. сетевой поток закрыт";
                    IncorrectMbap = "Некорректные данные MBAP Header!";
                }
                else
                {
                    IncorrectPduLength = "Incorrect PDU length";
                    IncorrectPduFuncCode = "Incorrect PDU function code";
                    IncorrectPduData = "Incorrect PDU data";
                    Request = "Request element group{0}";
                    Command = "Command{0}";
                    DeviceError = "Device error";
                    IllegalDataTable = "Illegal data table for the command.";
                    LoadTemplateError = "Error loading device template";
                    SaveTemplateError = "Error saving device template";
                    ClearNetStreamError = "Error clear network stream";
                    OK = "OK!";
                    CrcError = "CRC error!";
                    LrcError = "LRC error!";
                    CommErrorWithExclamation = "Communication error!";
                    CommError = "Communication error";
                    IncorrectSymbol = "Incorrect symbol!";
                    IncorrectAduLength = "Incorrect ADU length!";
                    RequestImpossible = "Request is impossible because network stream is closed";
                    IncorrectMbap = "Incorrect MBAP Header data!";
                }
            }

            public static string IncorrectPduLength { get; set; }
            public static string IncorrectPduFuncCode { get; set; }
            public static string IncorrectPduData { get; set; }
            public static string Request { get; set; }
            public static string Command { get; set; }
            public static string DeviceError { get; set; }
            public static string IllegalDataTable { get; set; }
            public static string LoadTemplateError { get; set; }
            public static string SaveTemplateError { get; set; }
            public static string ClearNetStreamError { get; set; }
            public static string OK { get; set; }
            public static string CrcError { get; set; }
            public static string LrcError { get; set; }
            public static string CommErrorWithExclamation { get; set; }
            public static string CommError { get; set; }
            public static string IncorrectSymbol { get; set; }
            public static string IncorrectAduLength { get; set; }
            public static string RequestImpossible { get; set; }
            public static string IncorrectMbap { get; set; }
        }

        /// <summary>
        /// Коды функций
        /// </summary>
        public static class FuncCodes
        {
            /// <summary>
            /// Считать дискретные входы
            /// </summary>
            public const byte ReadDiscreteInputs = 0x02;
            /// <summary>
            /// Считать флаги
            /// </summary>
            public const byte ReadCoils = 0x01;
            /// <summary>
            /// Считать входные регистры
            /// </summary>
            public const byte ReadInputRegisters = 0x04;
            /// <summary>
            /// Считать регистры хранения
            /// </summary>
            public const byte ReadHoldingRegisters = 0x03;

            /// <summary>
            /// Записать флаг
            /// </summary>
            public const byte WriteSingleCoil = 0x05;
            /// <summary>
            /// Записать регистр хранения
            /// </summary>
            public const byte WriteSingleRegister = 0x06;
            /// <summary>
            /// Записать множество флагов
            /// </summary>
            public const byte WriteMultipleCoils = 0x0F;
            /// <summary>
            /// Записать множество регистров хранения
            /// </summary>
            public const byte WriteMultipleRegisters = 0x10;
        }

        /// <summary>
        /// Блок данных
        /// </summary>
        public abstract class DataUnit
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            protected DataUnit()
                : this(TableTypes.DiscreteInputs)
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            public DataUnit(TableTypes tableType)
            {
                Name = "";
                TableType = tableType;
                Address = 0;

                FuncCode = 0;
                ExcFuncCode = 0;
                ReqPDU = null;
                RespPduLen = 0;
                ReqADU = null;
                ReqStr = "";
                RespByteCnt = 0;
            }


            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Получить или установить тип таблицы даных
            /// </summary>
            public TableTypes TableType { get; set; }

            /// <summary>
            /// Получить или установить адрес начального элемента
            /// </summary>
            public ushort Address { get; set; }

            /// <summary>
            /// Получить описание запроса для получения значений элементов
            /// </summary>
            public abstract string ReqDescr { get; }


            /// <summary>
            /// Код функции запроса
            /// </summary>
            public byte FuncCode { get; protected set; }

            /// <summary>
            /// Код функции, обозначающий исключение
            /// </summary>
            public byte ExcFuncCode { get; protected set; }

            /// <summary>
            /// Получить PDU запроса
            /// </summary>
            public byte[] ReqPDU { get; protected set; }

            /// <summary>
            /// Получить длину PDU ответа на запрос
            /// </summary>
            public int RespPduLen { get; protected set; }

            /// <summary>
            /// Получить ADU запроса
            /// </summary>
            public byte[] ReqADU { get; protected set; }

            /// <summary>
            /// Получить строку запроса в режиме ASCII
            /// </summary>
            public string ReqStr { get; protected set; }

            /// <summary>
            /// Получить длину ADU ответа на запрос
            /// </summary>
            public int RespAduLen { get; protected set; }

            /// <summary>
            /// Получить количество байт, которое указывается в ответе
            /// </summary>
            public byte RespByteCnt { get; protected set; }


            /// <summary>
            /// Инициализировать PDU запроса, рассчитать длину ответа
            /// </summary>
            public abstract void InitReqPDU();

            /// <summary>
            /// Инициализировать ADU запроса и рассчитать длину ответа
            /// </summary>
            public virtual void InitReqADU(byte devAddr, TransModes transMode)
            {
                if (ReqPDU != null)
                {
                    int pduLen = ReqPDU.Length;

                    switch (transMode)
                    {
                        case TransModes.RTU:
                            ReqADU = new byte[pduLen + 3];
                            ReqADU[0] = devAddr;
                            ReqPDU.CopyTo(ReqADU, 1);
                            ushort crc = CalcCRC16(ReqADU, 0, pduLen + 1);
                            ReqADU[pduLen + 1] = (byte)(crc % 256);
                            ReqADU[pduLen + 2] = (byte)(crc / 256);
                            RespAduLen = RespPduLen + 3;
                            break;
                        case TransModes.ASCII:
                            byte[] aduBuf = new byte[pduLen + 2];
                            aduBuf[0] = devAddr;
                            ReqPDU.CopyTo(aduBuf, 1);
                            aduBuf[pduLen + 1] = CalcLRC(aduBuf, 0, pduLen + 1);

                            StringBuilder sbADU = new StringBuilder();
                            foreach (byte b in aduBuf)
                                sbADU.Append(b.ToString("X2"));

                            ReqADU = Encoding.Default.GetBytes(sbADU.ToString());
                            ReqStr = Colon + sbADU;
                            RespAduLen = RespPduLen + 2;
                            break;
                        default: // TransModes.TCP
                            ReqADU = new byte[pduLen + 7];
                            ReqADU[0] = 0;
                            ReqADU[1] = 0;
                            ReqADU[2] = 0;
                            ReqADU[3] = 0;
                            ReqADU[4] = (byte)((pduLen + 1) / 256);
                            ReqADU[5] = (byte)((pduLen + 1) % 256);
                            ReqADU[6] = devAddr;
                            ReqPDU.CopyTo(ReqADU, 7);
                            RespAduLen = RespPduLen + 7;
                            break;
                    }
                }
            }

            /// <summary>
            /// Расшифровать PDU ответа
            /// </summary>
            public virtual bool DecodeRespPDU(byte[] buffer, int offset, int length, out string errMsg)
            {
                errMsg = "";
                bool result = false;
                byte respFuncCode = buffer[offset];

                if (respFuncCode == FuncCode)
                {
                    if (length == RespPduLen)
                        result = true;
                    else
                        errMsg = Phrases.IncorrectPduLength;
                }
                else if (respFuncCode == ExcFuncCode)
                {
                    errMsg = length == 2 ? Phrases.DeviceError + ": " + GetExcDescr(buffer[offset + 1]) : 
                        Phrases.IncorrectPduLength;
                }
                else
                {
                    errMsg = Phrases.IncorrectPduFuncCode;
                }

                return result;
            }
        }

        /// <summary>
        /// Элемент
        /// </summary>
        public class Elem
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Elem()
            {
                Name = "";
                ElemType = ElemTypes.Bool;
            }


            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Получить или установить тип
            /// </summary>
            public ElemTypes ElemType { get; set; }

            /// <summary>
            /// Получить длину элемента (количество адресов)
            /// </summary>
            public int Length
            {
                get
                {
                    return GetElemLength(ElemType);
                }
            }


            /// <summary>
            /// Получить длину элемента (количество адресов) в зависимости от типа элемента
            /// </summary>
            public static int GetElemLength(ElemTypes elemType)
            {
                return elemType == ElemTypes.UInt || elemType == ElemTypes.Int || elemType == ElemTypes.Float ?
                    2 : 1;
            }
        }

        /// <summary>
        /// Группа элементов
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
                Elems = new List<Elem>();
                ElemVals = null;
                TotalElemLength = -1;
                StartParamInd = -1;
                StartSignal = 0;

                // определение кода функции запроса
                switch (tableType)
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

                // определение кода функции, обозначающего исключение
                ExcFuncCode = (byte)(FuncCode | 0x80);
            }


            /// <summary>
            /// Получить список элементов в группе
            /// </summary>
            public List<Elem> Elems { get; private set; }

            /// <summary>
            /// Получить значения элементов в группе
            /// </summary>
            public uint[] ElemVals { get; private set; }

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
                        reqDescr = string.Format(Phrases.Request, 
                            string.IsNullOrEmpty(Name) ? "" : " \"" + Name + "\"");
                    return reqDescr;
                }
            }

            /// <summary>
            /// Получить или установить индекс параметра КП, соответствующего начальному элементу
            /// </summary>
            public int StartParamInd { get; set; }

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
                ElemVals = new uint[Elems.Count];
                Array.Clear(ElemVals, 0, Elems.Count);
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
                                ElemVals[elemInd] = ((buffer[byteNum] >> bitNum) & 0x01) > 0 ? (uint)1 : (uint)0;

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
                                if (Elems[elemInd].Length == 1)
                                {
                                    ElemVals[elemInd] = (uint)(buffer[byteNum] * 256 + buffer[byteNum + 1]);
                                    byteNum += 2;
                                }
                                else
                                {
                                    ElemVals[elemInd] = (uint)((buffer[byteNum] << 24) + (buffer[byteNum + 1] << 16) + 
                                        (buffer[byteNum + 2] << 8) + buffer[byteNum + 3]);
                                    byteNum += 4;
                                }                                
                            }
                        }

                        return true;
                    }
                    else
                    {
                        errMsg = Phrases.IncorrectPduData;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Получить значение элемента в соответствии с его типом, преобразованное в double
            /// </summary>
            public double GetElemVal(int elemInd)
            {
                ElemTypes elemType = Elems[elemInd].ElemType;
                uint elemVal = ElemVals[elemInd];

                if (elemType == ElemTypes.UShort || elemType == ElemTypes.UInt)
                {
                    return elemVal;
                }
                else
                {
                    byte[] buf = BitConverter.GetBytes(ElemVals[elemInd]);

                    if (elemType == ElemTypes.Short)
                        return BitConverter.ToInt16(buf, 0);
                    else if (elemType == ElemTypes.Int)
                        return BitConverter.ToInt32(buf, 0);
                    else if (elemType == ElemTypes.Float)
                        return BitConverter.ToSingle(buf, 0);
                    else // ElemTypes.Bool
                        return elemVal > 0 ? 1.0 : 0.0;
                }
            }

            /// <summary>
            /// Получить максимально допустимое количество элементов в группе
            /// </summary>
            public static int GetMaxElemCnt(TableTypes tableType)
            {
                return tableType == TableTypes.DiscreteInputs || tableType == TableTypes.Coils ? 2000 : 125;
            }

            /// <summary>
            /// Получить тип элементов группы по умолчанию в зависимости от типа таблицы данных
            /// </summary>
            public static ElemTypes GetDefElemType(TableTypes tableType)
            {
                return tableType == TableTypes.DiscreteInputs || tableType == TableTypes.Coils ? 
                    ElemTypes.Bool : ElemTypes.UShort;
            }
        }

        /// <summary>
        /// Команда
        /// </summary>
        public class Cmd : DataUnit
        {
            private string reqDescr; // описание команды


            /// <summary>
            /// Конструктор
            /// </summary>
            private Cmd()
                : base()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            public Cmd(TableTypes tableType, bool multiple = false, int elemCnt = 1)
                : base(tableType)
            {
                if (tableType == TableTypes.DiscreteInputs || tableType == TableTypes.InputRegisters)
                    throw new Exception(Phrases.IllegalDataTable);

                reqDescr = "";
                Multiple = multiple;
                ElemCnt = elemCnt;
                Value = 0;
                Data = null;
                CmdNum = 1;

                // определение кода функции запроса
                if (tableType == TableTypes.Coils)
                    FuncCode = multiple ? FuncCodes.WriteMultipleCoils : FuncCodes.WriteSingleCoil;
                else
                    FuncCode = multiple ? FuncCodes.WriteMultipleRegisters : FuncCodes.WriteSingleRegister;

                // определение кода функции, обозначающего исключение
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
                        reqDescr = string.Format(Phrases.Command, 
                            string.IsNullOrEmpty(Name) ? "" : " \"" + Name + "\"");
                    return reqDescr;
                }
            }

            /// <summary>
            /// Получить или установить признак множественной команды
            /// </summary>
            public bool Multiple { get; set; }

            /// <summary>
            /// Получить или установить количество элементов, устанавливаемое множественной командой
            /// </summary>
            public int ElemCnt { get; set; }

            /// <summary>
            /// Получить или установить значение команды
            /// </summary>
            public ushort Value { get; set; }

            /// <summary>
            /// Получить или установить данные множественной команды
            /// </summary>
            public byte[] Data { get; set; }

            /// <summary>
            /// Получить или установить номер команды КП
            /// </summary>
            public int CmdNum { get; set; }

            
            /// <summary>
            /// Инициализировать PDU запроса, рассчитать длину ответа
            /// </summary>
            public override void InitReqPDU()
            {
                if (Multiple)
                {
                    // формирование PDU для команды WriteMultipleCoils или WriteMultipleRegisters
                    int byteCnt = TableType == TableTypes.Coils ?
                        ((ElemCnt % 8 == 0) ? ElemCnt / 8 : ElemCnt / 8 + 1) : ElemCnt * 2;

                    ReqPDU = new byte[6 + byteCnt];
                    ReqPDU[0] = FuncCode;
                    ReqPDU[1] = (byte)(Address / 256);
                    ReqPDU[2] = (byte)(Address % 256);
                    ReqPDU[3] = (byte)(ElemCnt / 256);
                    ReqPDU[4] = (byte)(ElemCnt % 256);
                    ReqPDU[5] = (byte)byteCnt;

                    int dataLen = Data == null ? 0 : Data.Length;
                    int len = Math.Min(dataLen, byteCnt);
                    int ind = 6;

                    for (int i = 0; i < len; i++)
                        ReqPDU[ind++] = Data[i];

                    while (ind < byteCnt)
                        ReqPDU[ind++] = 0x00;
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
                        ReqPDU[3] = (byte)(Value / 256);
                        ReqPDU[4] = (byte)(Value % 256);
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
                        errMsg = Phrases.IncorrectPduData;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Модель устройства
        /// </summary>
        public class DeviceModel
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public DeviceModel()
            {
                ElemGroups = new List<ElemGroup>();
                Cmds = new List<Cmd>();
            }


            /// <summary>
            /// Получить список групп элементов
            /// </summary>
            public List<ElemGroup> ElemGroups { get; private set; }

            /// <summary>
            /// Получить список команд
            /// </summary>
            public List<Cmd> Cmds { get; private set; }


            /// <summary>
            /// Найти команду по номеру
            /// </summary>
            public Cmd FindCmd(int cmdNum)
            {
                foreach (Cmd cmd in Cmds)
                {
                    if (cmd.CmdNum == cmdNum)
                        return cmd;
                }

                return null;
            }

            /// <summary>
            /// Загрузить шаблон устройства
            /// </summary>
            public bool LoadTemplate(string fileName, out string errMsg)
            {
                try
                {
                    // очистка списков групп элементов и команд
                    ElemGroups.Clear();
                    Cmds.Clear();

                    // загрузка шаблона устройства
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);

                    // загрузка групп элементов
                    XmlNode elemGroupsNode = xmlDoc.DocumentElement.SelectSingleNode("ElemGroups");

                    if (elemGroupsNode != null)
                    {
                        int paramInd = 0;

                        foreach (XmlElement elemGroupElem in elemGroupsNode.ChildNodes)
                        {
                            TableTypes tableType = (TableTypes)(Enum.Parse(typeof(TableTypes), 
                                elemGroupElem.GetAttribute("tableType"), true));
                            ElemGroup elemGroup = new ElemGroup(tableType);
                            elemGroup.Address = ushort.Parse(elemGroupElem.GetAttribute("address"));
                            elemGroup.Name = elemGroupElem.GetAttribute("name");
                            elemGroup.StartParamInd = paramInd;
                            elemGroup.StartSignal = paramInd + 1;

                            XmlNodeList elemNodes = elemGroupElem.SelectNodes("Elem");
                            foreach (XmlElement elemElem in elemNodes)
                            {
                                Elem elem = new Elem();
                                elem.Name = elemElem.GetAttribute("name");
                                string elemTypeStr = elemElem.GetAttribute("type");
                                elem.ElemType = elemTypeStr == "" ? elemGroup.DefElemType : 
                                    (ElemTypes)(Enum.Parse(typeof(ElemTypes), elemTypeStr, true));
                                elemGroup.Elems.Add(elem);
                            }

                            if (0 < elemGroup.Elems.Count && elemGroup.Elems.Count <= ElemGroup.GetMaxElemCnt(tableType))
                            {
                                ElemGroups.Add(elemGroup);
                                paramInd += elemGroup.Elems.Count;
                            }
                        }
                    }

                    // загрузка команд
                    XmlNode cmdsNode = xmlDoc.DocumentElement.SelectSingleNode("Cmds");

                    if (cmdsNode != null)
                    {
                        foreach (XmlElement cmdElem in cmdsNode.ChildNodes)
                        {
                            TableTypes tableType = (TableTypes)(Enum.Parse(typeof(TableTypes),
                                cmdElem.GetAttribute("tableType"), true));
                            string multiple = cmdElem.GetAttribute("multiple");
                            string elemCnt = cmdElem.GetAttribute("elemCnt");
                            Cmd cmd = multiple == "" || elemCnt == "" ? 
                                new Cmd(tableType) : 
                                new Cmd(tableType, bool.Parse(multiple), int.Parse(elemCnt));
                            cmd.Address = ushort.Parse(cmdElem.GetAttribute("address"));
                            cmd.Name = cmdElem.GetAttribute("name");
                            cmd.CmdNum = int.Parse(cmdElem.GetAttribute("cmdNum"));

                            if (0 < cmd.CmdNum && cmd.CmdNum <= ushort.MaxValue)
                                Cmds.Add(cmd);
                        }
                    }

                    errMsg = "";
                    return true;
                }
                catch (Exception ex)
                {
                    errMsg = Phrases.LoadTemplateError + ":\r\n" + ex.Message;
                    return false;
                }
            }

            /// <summary>
            /// Сохранить шаблон устройства
            /// </summary>
            public bool SaveTemplate(string fileName, out string errMsg)
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    xmlDoc.AppendChild(xmlDecl);

                    XmlElement rootElem = xmlDoc.CreateElement("DevTemplate");
                    xmlDoc.AppendChild(rootElem);

                    // сохранение групп элементов
                    XmlElement elemGroupsElem = xmlDoc.CreateElement("ElemGroups");
                    rootElem.AppendChild(elemGroupsElem);

                    foreach (ElemGroup elemGroup in ElemGroups)
                    {
                        XmlElement elemGroupElem = xmlDoc.CreateElement("ElemGroup");
                        elemGroupElem.SetAttribute("name", elemGroup.Name);
                        elemGroupElem.SetAttribute("tableType", elemGroup.TableType.ToString());
                        elemGroupElem.SetAttribute("address", elemGroup.Address.ToString());
                        elemGroupsElem.AppendChild(elemGroupElem);

                        bool writeElemType = elemGroup.TableType == TableTypes.InputRegisters || 
                            elemGroup.TableType == TableTypes.HoldingRegisters;

                        foreach (Elem elem in elemGroup.Elems)
                        {
                            XmlElement elemElem = xmlDoc.CreateElement("Elem");
                            elemElem.SetAttribute("name", elem.Name);
                            if (writeElemType)
                                elemElem.SetAttribute("type", elem.ElemType.ToString().ToLower());
                            elemGroupElem.AppendChild(elemElem);
                        }
                    }

                    // сохранение команд
                    XmlElement cmdsElem = xmlDoc.CreateElement("Cmds");
                    rootElem.AppendChild(cmdsElem);

                    foreach (Cmd cmd in Cmds)
                    {
                        XmlElement cmdElem = xmlDoc.CreateElement("Cmd");
                        cmdsElem.AppendChild(cmdElem);

                        cmdElem.SetAttribute("name", cmd.Name);
                        cmdElem.SetAttribute("tableType", cmd.TableType.ToString());
                        cmdElem.SetAttribute("multiple", cmd.Multiple.ToString().ToLower());
                        cmdElem.SetAttribute("address", cmd.Address.ToString());
                        cmdElem.SetAttribute("elemCnt", cmd.ElemCnt.ToString());
                        cmdElem.SetAttribute("cmdNum", cmd.CmdNum.ToString());
                    }

                    xmlDoc.Save(fileName);
                    errMsg = "";
                    return true;
                }
                catch (Exception ex)
                {
                    errMsg = Phrases.SaveTemplateError + ":\r\n" + ex.Message;
                    return false;
                }
            }

            /// <summary>
            /// Копировать модель устройства из заданной
            /// </summary>
            public void CopyFrom(DeviceModel srcDeviceModel)
            {
                if (srcDeviceModel == null)
                    throw new ArgumentNullException("srcDeviceModel");

                // очистка списков групп элементов и команд
                ElemGroups.Clear();
                Cmds.Clear();

                // копирование групп элементов
                foreach (ElemGroup srcGroup in srcDeviceModel.ElemGroups)
                {
                    ElemGroup elemGroup = new ElemGroup(srcGroup.TableType)
                    {
                        Address = srcGroup.Address,
                        Name = srcGroup.Name,
                        StartParamInd = srcGroup.StartParamInd,
                        StartSignal = srcGroup.StartSignal,
                    };

                    foreach (Elem srcElem in srcGroup.Elems)
                    {
                        elemGroup.Elems.Add(new Elem()
                        {
                            Name = srcElem.Name,
                            ElemType = srcElem.ElemType
                        });
                    }

                    ElemGroups.Add(elemGroup);
                }

                // копирование команд
                foreach (Cmd srcCmd in srcDeviceModel.Cmds)
                {
                    Cmds.Add(new Cmd(srcCmd.TableType)
                    {
                        Multiple = srcCmd.Multiple,
                        ElemCnt = srcCmd.ElemCnt,
                        Address = srcCmd.Address,
                        Name = srcCmd.Name,
                        CmdNum = srcCmd.CmdNum
                    });
                }
            }
        }


        /// <summary>
        /// Символ начала сообщения в режиме ASCII
        /// </summary>
        public const string Colon = ":";

        /// <summary>
        /// Окончание сообщения в режиме ASCII
        /// </summary>
        public const string CRLF = "\x0D\x0A";

        /// <summary>
        /// Размер буфера входных данных, байт
        /// </summary>
        public const int InBufSize = 300;

        /// <summary>
        /// Номер порта, используемого в режиме TCP по умолчанию
        /// </summary>
        public const int DefTcpPort = 502;

        #region Таблицы для расчёта CRC-16
        /* Table of CRC values for high–order byte */
        private static readonly byte[] CRCHiTable = new byte[] {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40
        };

        /* Table of CRC values for low–order byte */
        private static readonly byte[] CRCLoTable = new byte[] {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4,
            0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD,
            0x1D, 0x1C, 0xDC, 0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32, 0x36, 0xF6, 0xF7,
            0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE,
            0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2,
            0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB,
            0x7B, 0x7A, 0xBA, 0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0, 0x50, 0x90, 0x91,
            0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88,
            0x48, 0x49, 0x89, 0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83, 0x41, 0x81, 0x80,
            0x40
        };
        #endregion


        /// <summary>
        /// Конструктор
        /// </summary>
        public Modbus()
        {
            InBuf = new byte[InBufSize];
            WriteToLog = null;
            SerialPort = null;
            Timeout = 0;
            NetStream = null;
        }


        /// <summary>
        /// Получить буфер входных данных
        /// </summary>
        public byte[] InBuf { get; private set; }

        /// <summary>
        /// Получить или установить ссылку на последовательный порт
        /// </summary>
        public SerialPort SerialPort { get; set; }

        /// <summary>
        /// Получить или установить таймаут запросов через последовательный порт
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Получить или установить поток данных TCP-клиента
        /// </summary>
        public NetworkStream NetStream { get; set; }

        /// <summary>
        /// Получить или установить метод записи в журнал
        /// </summary>
        public KPLogic.WriteToLogDelegate WriteToLog { get; set; }


        /// <summary>
        /// Записать данные в поток TCP-клиента
        /// </summary>
        private void WriteNetStream(byte[] buffer, int count)
        {
            NetStream.Write(buffer, 0, count);
            string logText = KPUtils.SendNotation + " (" + count + "): " + KPUtils.BytesToHex(buffer, 0, count);
            ExecWriteToLog(logText);
        }

        /// <summary>
        /// Считать данные из потока TCP-клиента
        /// </summary>
        private int ReadNetStream(int index, int count)
        {
            int readCnt = NetStream.Read(InBuf, index, count);
            string logText = KPUtils.ReceiveNotation + " (" + readCnt + "/" + count + "): " + 
                KPUtils.BytesToHex(InBuf, index, readCnt);
            ExecWriteToLog(logText);
            return readCnt;
        }

        /// <summary>
        /// Очистить поток данных TCP-клиента
        /// </summary>
        private void ClearNetStream()
        {
            try
            {
                if (NetStream.DataAvailable)
                    NetStream.Read(InBuf, 0, InBufSize);
            }
            catch (Exception ex)
            {
                ExecWriteToLog(Phrases.ClearNetStreamError + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Вызвать метод записи в журнал
        /// </summary>
        private void ExecWriteToLog(string text)
        {
            if (WriteToLog != null)
                WriteToLog(text);
        }


        /// <summary>
        /// Выполнить запрос в режиме RTU
        /// </summary>
        public bool RtuRequest(DataUnit dataUnit)
        {
            bool result = false;

            // отправка запроса
            ExecWriteToLog(dataUnit.ReqDescr);
            string logText;
            KPUtils.WriteToSerialPort(SerialPort, dataUnit.ReqADU, 0, dataUnit.ReqADU.Length, out logText);
            ExecWriteToLog(logText);

            // приём ответа
            // считывание начала ответа для определения длины PDU
            int readCnt = KPUtils.ReadFromSerialPort(SerialPort, InBuf, 0, 5, Timeout, false, out logText);
            ExecWriteToLog(logText);

            if (readCnt == 5)
            {
                int pduLen;
                int count;

                if (InBuf[1] == dataUnit.FuncCode)
                {
                    // считывание окончания ответа
                    pduLen = dataUnit.RespPduLen;
                    count = dataUnit.RespAduLen - 5;

                    readCnt = KPUtils.ReadFromSerialPort(SerialPort, InBuf, 5, count, Timeout, false, out logText);
                    ExecWriteToLog(logText);
                }
                else // устройство вернуло исключение
                {
                    pduLen = 2;
                    count = 0;
                    readCnt = 0;
                }

                if (readCnt == count)
                {
                    if (InBuf[pduLen + 1] + InBuf[pduLen + 2] * 256 == CalcCRC16(InBuf, 0, pduLen + 1))
                    {
                        // расшифровка ответа
                        string errMsg;

                        if (dataUnit.DecodeRespPDU(InBuf, 1, pduLen, out errMsg))
                        {
                            ExecWriteToLog(Phrases.OK);
                            result = true;
                        }
                        else
                        {
                            ExecWriteToLog(errMsg + "!");
                        }
                    }
                    else
                    {
                        ExecWriteToLog(Phrases.CrcError);
                    }
                }
                else
                {
                    ExecWriteToLog(Phrases.CommErrorWithExclamation);
                }
            }
            else
            {
                ExecWriteToLog(Phrases.CommErrorWithExclamation);
            }

            return result;
        }

        /// <summary>
        /// Выполнить запрос в режиме ASCII
        /// </summary>
        public bool AsciiRequest(DataUnit dataUnit)
        {
            bool result = false;

            // отправка запроса
            ExecWriteToLog(dataUnit.ReqDescr);
            string logText;
            KPUtils.WriteLineToSerialPort(SerialPort, dataUnit.ReqStr, out logText);
            ExecWriteToLog(logText);

            // приём ответа
            bool endFound;
            List<string> lines = KPUtils.ReadLinesFromSerialPort(SerialPort, Timeout, false, "", out endFound, out logText);
            ExecWriteToLog(logText);

            string line = lines.Count == 1 ? lines[0] : "";
            int lineLen = line.Length;

            if (lineLen >= 3)
            {
                int aduLen = (lineLen - 1) / 2;

                if (aduLen == dataUnit.RespAduLen && lineLen % 2 == 1)
                {
                    // получение ADU ответа
                    byte[] aduBuf = new byte[aduLen];
                    bool parseOK = true;

                    for (int i = 0, j = 1; i < aduLen && parseOK; i++, j += 2)
                    {
                        try
                        {
                            aduBuf[i] = byte.Parse(line.Substring(j, 2), NumberStyles.HexNumber);
                        }
                        catch
                        {
                            ExecWriteToLog(Phrases.IncorrectSymbol);
                            parseOK = false;
                        }
                    }

                    if (parseOK)
                    {
                        if (aduBuf[aduLen - 1] == Modbus.CalcLRC(aduBuf, 0, aduLen - 1))
                        {
                            // расшифровка ответа
                            string errMsg;

                            if (dataUnit.DecodeRespPDU(aduBuf, 1, aduLen - 2, out errMsg))
                            {
                                ExecWriteToLog(Phrases.OK);
                                result = true;
                            }
                            else
                            {
                                ExecWriteToLog(errMsg + "!");
                            }
                        }
                        else
                        {
                            ExecWriteToLog(Phrases.LrcError);
                        }
                    }
                }
                else
                {
                    ExecWriteToLog(Phrases.IncorrectAduLength);
                }
            }
            else
            {
                ExecWriteToLog(Phrases.CommErrorWithExclamation);
            }

            return result;
        }

        /// <summary>
        /// Выполнить запрос в режиме TCP
        /// </summary>
        public bool TcpRequest(DataUnit dataUnit)
        {
            if (NetStream == null)
            {
                ExecWriteToLog(Phrases.RequestImpossible);
                return false;
            }

            bool result = false;
            bool closeNetStream = false;

            try
            {
                // отправка запроса
                WriteToLog(dataUnit.ReqDescr);
                WriteNetStream(dataUnit.ReqADU, dataUnit.ReqADU.Length);

                // приём ответа
                if (ReadNetStream(0, 7) /*считывание MBAP Header*/ == 7)
                {
                    int pduLen = InBuf[4] * 256 + InBuf[5] - 1;

                    if (InBuf[0] == 0 && InBuf[1] == 0 && InBuf[2] == 0 && InBuf[3] == 0 && pduLen > 0 &&
                        InBuf[6] == dataUnit.ReqADU[6])
                    {
                        if (ReadNetStream(7, pduLen) /*считывание PDU*/ == pduLen)
                        {
                            // расшифровка ответа
                            string errMsg;

                            if (dataUnit.DecodeRespPDU(InBuf, 7, pduLen, out errMsg))
                            {
                                ExecWriteToLog(Phrases.OK);
                                result = true;
                            }
                            else
                            {
                                ExecWriteToLog(errMsg + "!");
                            }
                        }
                        else
                        {
                            WriteToLog(Phrases.CommErrorWithExclamation);
                        }
                    }
                    else
                    {
                        WriteToLog(Phrases.IncorrectMbap);
                    }
                }
                else
                {
                    WriteToLog(Phrases.CommErrorWithExclamation);
                }
            }
            catch (Exception ex)
            {
                WriteToLog(Phrases.CommError + ": " + ex.Message);
                closeNetStream = true;
            }

            // очистка потока данных в случае ошибки
            if (!result)
                ClearNetStream();

            if (closeNetStream)
            {
                NetStream.Close();
                NetStream = null;
            }

            return result;
        }


        /// <summary>
        /// Рассчитать CRC-16
        /// </summary>
        /// <remarks>Метод взят из официального описания протокола Modbus.</remarks>
        public static ushort CalcCRC16(byte[] buffer, int offset, int length)
        {
            byte crcHi = 0xFF;   // high byte of CRC initialized
            byte crcLo = 0xFF;   // low byte of CRC initialized
            int index;           // will index into CRC lookup table

            while (length-- > 0) // pass through message buffer
            {
                index = crcLo ^ buffer[offset++]; // calculate the CRC
                crcLo = (byte)(crcHi ^ CRCHiTable[index]);
                crcHi = CRCLoTable[index];
            }

            return (ushort)((crcHi << 8) | crcLo);
        }

        /// <summary>
        /// Рассчитать LRC
        /// </summary>
        /// <remarks>Метод взят из официального описания протокола Modbus.</remarks>
        public static byte CalcLRC(byte[] buffer, int offset, int length)
        {
            byte uchLRC = 0;                // LRC char initialized
            while (length-- > 0)            // pass through message buffer
                uchLRC += buffer[offset++]; // add buffer byte without carry
            return (byte)(-(sbyte)uchLRC);  // return twos complement
        }

        /// <summary>
        /// Получить наименование типа таблицы данных
        /// </summary>
        public static string GetTableTypeName(TableTypes tableType)
        {
            switch (tableType)
            {
                case TableTypes.DiscreteInputs:
                    return "Discrete Inputs";
                case TableTypes.Coils:
                    return "Coils";
                case TableTypes.InputRegisters:
                    return "Input Registers";
                default: // TableTypes.HoldingRegisters
                    return "Holding Registers";
            }
        }

        /// <summary>
        /// Получить описание исключения, полученного от устройства
        /// </summary>
        public static string GetExcDescr(byte excCode)
        {
            string descr = "[" + excCode.ToString("X2") + "] ";

            switch (excCode)
            {
                case 0x01:
                    descr += "ILLEGAL FUNCTION";
                    break;
                case 0x02:
                    descr += "ILLEGAL DATA ADDRESS";
                    break;
                case 0x03:
                    descr += "ILLEGAL DATA VALUE";
                    break;
                case 0x04:
                    descr += "SLAVE DEVICE FAILURE";
                    break;
                case 0x05:
                    descr += "ACKNOWLEDGE";
                    break;
                case 0x06:
                    descr += "SLAVE DEVICE BUSY";
                    break;
                case 0x08:
                    descr += "MEMORY PARITY ERROR";
                    break;
                case 0x0A:
                    descr += "GATEWAY PATH UNAVAILABLE";
                    break;
                case 0x0B:
                    descr += "GATEWAY TARGET DEVICE FAILED TO RESPOND";
                    break;
                default:
                    descr += "Unknown";
                    break;
            }

            return descr;
        }
    }
}
