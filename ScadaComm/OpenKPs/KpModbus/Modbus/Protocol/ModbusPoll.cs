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
 * Module   : KpModbus
 * Summary  : Polls devices using Modbus protocol
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2019
 */

using Scada.Comm.Channels;
using System.Globalization;
using Utils;

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Polls devices using Modbus protocol.
    /// <para>Опрос устройств по протоколу Modbus.</para>
    /// </summary>
    public class ModbusPoll
    {
        /// <summary>
        /// Делегат выполнения запроса
        /// </summary>
        public delegate bool RequestDelegate(DataUnit dataUnit);

        /// <summary>
        /// Размер буфера входных данных по умолчанию, байт
        /// </summary>
        private const int DefInBufSize = 300;


        /// <summary>
        /// Конструктор
        /// </summary>
        public ModbusPoll()
            : this(DefInBufSize)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ModbusPoll(int inBufSize)
        {
            InBuf = new byte[inBufSize];
            TransactionID = 0;
            Timeout = 0;
            Connection = null;
            WriteToLog = null;
        }


        /// <summary>
        /// Получить буфер входных данных
        /// </summary>
        public byte[] InBuf { get; protected set; }

        /// <summary>
        /// Gets the current transaction ID in the TCP mode.
        /// </summary>
        public ushort TransactionID { get; protected set; }

        /// <summary>
        /// Получить или установить таймаут запросов через последовательный порт
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Получить или установить соединение с физическим КП
        /// </summary>
        public Connection Connection { get; set; }

        /// <summary>
        /// Получить или установить метод записи строки в журнал
        /// </summary>
        public Log.WriteLineDelegate WriteToLog { get; set; }


        /// <summary>
        /// Вызвать метод записи в журнал
        /// </summary>
        protected void ExecWriteToLog(string text)
        {
            WriteToLog?.Invoke(text);
        }

        /// <summary>
        /// Проверить, что соединение установлено
        /// </summary>
        protected bool CheckConnection()
        {
            if (Connection == null || !Connection.Connected)
            {
                ExecWriteToLog(ModbusPhrases.ConnectionRequired);
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Выполнить запрос в режиме RTU
        /// </summary>
        public bool RtuRequest(DataUnit dataUnit)
        {
            if (!CheckConnection())
                return false;

            bool result = false;

            // отправка запроса
            ExecWriteToLog(dataUnit.ReqDescr);
            Connection.Write(dataUnit.ReqADU, 0, dataUnit.ReqADU.Length,
                CommUtils.ProtocolLogFormats.Hex, out string logText);
            ExecWriteToLog(logText);

            // приём ответа
            // считывание начала ответа для определения длины PDU
            int readCnt = Connection.Read(InBuf, 0, 5, Timeout, CommUtils.ProtocolLogFormats.Hex, out logText);
            ExecWriteToLog(logText);

            if (readCnt == 5)
            {
                int pduLen;
                int count;

                if (InBuf[0] != dataUnit.ReqADU[0]) // проверка адреса устройства в ответе
                {
                    ExecWriteToLog(ModbusPhrases.IncorrectDevAddr);
                }
                else if (!(InBuf[1] == dataUnit.FuncCode || InBuf[1] == dataUnit.ExcFuncCode))
                {
                    ExecWriteToLog(ModbusPhrases.IncorrectPduFuncCode);
                }
                else
                {
                    if (InBuf[1] == dataUnit.FuncCode)
                    {
                        // считывание окончания ответа
                        pduLen = dataUnit.RespPduLen;
                        count = dataUnit.RespAduLen - 5;

                        readCnt = Connection.Read(InBuf, 5, count, Timeout,
                            CommUtils.ProtocolLogFormats.Hex, out logText);
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
                        if (InBuf[pduLen + 1] + InBuf[pduLen + 2] * 256 == ModbusUtils.CalcCRC16(InBuf, 0, pduLen + 1))
                        {
                            // расшифровка ответа
                            string errMsg;

                            if (dataUnit.DecodeRespPDU(InBuf, 1, pduLen, out errMsg))
                            {
                                ExecWriteToLog(ModbusPhrases.OK);
                                result = true;
                            }
                            else
                            {
                                ExecWriteToLog(errMsg + "!");
                            }
                        }
                        else
                        {
                            ExecWriteToLog(ModbusPhrases.CrcError);
                        }
                    }
                    else
                    {
                        ExecWriteToLog(ModbusPhrases.CommErrorWithExclamation);
                    }
                }
            }
            else
            {
                ExecWriteToLog(ModbusPhrases.CommErrorWithExclamation);
            }

            return result;
        }

        /// <summary>
        /// Выполнить запрос в режиме ASCII
        /// </summary>
        public bool AsciiRequest(DataUnit dataUnit)
        {
            if (!CheckConnection())
                return false;

            bool result = false;

            // отправка запроса
            ExecWriteToLog(dataUnit.ReqDescr);
            Connection.WriteLine(dataUnit.ReqStr, out string logText);
            ExecWriteToLog(logText);

            // приём ответа
            string line = Connection.ReadLine(Timeout, out logText);
            ExecWriteToLog(logText);
            int lineLen = line == null ? 0 : line.Length;

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
                            ExecWriteToLog(ModbusPhrases.IncorrectSymbol);
                            parseOK = false;
                        }
                    }

                    if (parseOK)
                    {
                        if (aduBuf[aduLen - 1] == ModbusUtils.CalcLRC(aduBuf, 0, aduLen - 1))
                        {
                            // расшифровка ответа
                            string errMsg;

                            if (dataUnit.DecodeRespPDU(aduBuf, 1, aduLen - 2, out errMsg))
                            {
                                ExecWriteToLog(ModbusPhrases.OK);
                                result = true;
                            }
                            else
                            {
                                ExecWriteToLog(errMsg + "!");
                            }
                        }
                        else
                        {
                            ExecWriteToLog(ModbusPhrases.LrcError);
                        }
                    }
                }
                else
                {
                    ExecWriteToLog(ModbusPhrases.IncorrectAduLength);
                }
            }
            else
            {
                ExecWriteToLog(ModbusPhrases.CommErrorWithExclamation);
            }

            return result;
        }

        /// <summary>
        /// Выполнить запрос в режиме TCP
        /// </summary>
        public bool TcpRequest(DataUnit dataUnit)
        {
            if (!CheckConnection())
                return false;

            bool result = false;

            // specify transaction ID
            if (++TransactionID == 0)
                TransactionID = 1;

            dataUnit.ReqADU[0] = (byte)(TransactionID / 256);
            dataUnit.ReqADU[1] = (byte)(TransactionID % 256);

            // отправка запроса
            WriteToLog(dataUnit.ReqDescr);
            Connection.Write(dataUnit.ReqADU, 0, dataUnit.ReqADU.Length,
                CommUtils.ProtocolLogFormats.Hex, out string logText);
            ExecWriteToLog(logText);

            // приём ответа
            // считывание MBAP Header
            int readCnt = Connection.Read(InBuf, 0, 7, Timeout, CommUtils.ProtocolLogFormats.Hex, out logText);
            ExecWriteToLog(logText);

            if (readCnt == 7)
            {
                int pduLen = InBuf[4] * 256 + InBuf[5] - 1;

                if (InBuf[0] == dataUnit.ReqADU[0] && InBuf[1] == dataUnit.ReqADU[1] && 
                    InBuf[2] == 0 && InBuf[3] == 0 && pduLen > 0 &&
                    InBuf[6] == dataUnit.ReqADU[6])
                {
                    // считывание PDU
                    readCnt = Connection.Read(InBuf, 7, pduLen, Timeout,
                        CommUtils.ProtocolLogFormats.Hex, out logText);
                    ExecWriteToLog(logText);

                    if (readCnt == pduLen)
                    {
                        // расшифровка ответа
                        string errMsg;

                        if (dataUnit.DecodeRespPDU(InBuf, 7, pduLen, out errMsg))
                        {
                            ExecWriteToLog(ModbusPhrases.OK);
                            result = true;
                        }
                        else
                        {
                            ExecWriteToLog(errMsg + "!");
                        }
                    }
                    else
                    {
                        WriteToLog(ModbusPhrases.CommErrorWithExclamation);
                    }
                }
                else
                {
                    WriteToLog(ModbusPhrases.IncorrectMbap);
                }
            }
            else
            {
                WriteToLog(ModbusPhrases.CommErrorWithExclamation);
            }

            return result;
        }

        /// <summary>
        /// Получить метод запроса, соответствующий режиму передачи данных.
        /// </summary>
        public RequestDelegate GetRequestMethod(TransMode transMode)
        {
            switch (transMode)
            {
                case TransMode.RTU:
                    return RtuRequest;
                case TransMode.ASCII:
                    return AsciiRequest;
                default: // TransMode.TCP
                    return TcpRequest;
            }
        }
    }
}
