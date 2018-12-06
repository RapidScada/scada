/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Group of Modbus elements.
    /// <para>Группа элементов Modbus.</para>
    /// </summary>
    public class ElemGroup : DataUnit
    {
        private string reqDescr; // the request description


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private ElemGroup()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ElemGroup(TableType tableType)
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

            // расчёт длины ответа
            if (TableType == TableType.DiscreteInputs || TableType == TableType.Coils)
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
                byte[] elemVal = new byte[elem.ElemType == ElemType.Bool ? 1 : elem.Length * 2];
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

                    if (TableType == TableType.DiscreteInputs || TableType == TableType.Coils)
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
        /// Loads the group from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement groupElem)
        {
            if (groupElem == null)
                throw new ArgumentNullException("groupElem");

            Name = groupElem.GetAttribute("name");
            Address = (ushort)groupElem.GetAttrAsInt("address");
            Active = groupElem.GetAttrAsBool("active", true);

            XmlNodeList elemNodes = groupElem.SelectNodes("Elem");
            int maxElemCnt = MaxElemCnt;
            ElemType defElemType = DefElemType;

            foreach (XmlElement elemElem in elemNodes)
            {
                if (Elems.Count >= maxElemCnt)
                    break;

                Elem elem = CreateElem();
                elem.Name = elemElem.GetAttribute("name");
                elem.ElemType = elemElem.GetAttrAsEnum("type", defElemType);

                if (ByteOrderEnabled)
                {
                    elem.ByteOrderStr = elemElem.GetAttribute("byteOrder");
                    elem.ByteOrder = ModbusUtils.ParseByteOrder(elem.ByteOrderStr);
                }

                Elems.Add(elem);
            }
        }

        /// <summary>
        /// Saves the group into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement groupElem)
        {
            if (groupElem == null)
                throw new ArgumentNullException("groupElem");

            groupElem.SetAttribute("active", Active);
            groupElem.SetAttribute("tableType", TableType);
            groupElem.SetAttribute("address", Address);
            groupElem.SetAttribute("name", Name);

            foreach (Elem elem in Elems)
            {
                XmlElement elemElem = groupElem.AppendElem("Elem");
                elemElem.SetAttribute("name", elem.Name);

                if (ElemTypeEnabled)
                    elemElem.SetAttribute("type", elem.ElemType.ToString().ToLowerInvariant());

                if (ByteOrderEnabled)
                    elemElem.SetAttribute("byteOrder", elem.ByteOrderStr);
            }
        }

        /// <summary>
        /// Copies the group properties from the source group.
        /// </summary>
        public virtual void CopyFrom(ElemGroup srcGroup)
        {
            if (srcGroup == null)
                throw new ArgumentNullException("srcGroup");

            Name = srcGroup.Name;
            Address = srcGroup.Address;
            Active = srcGroup.Active;
            StartKPTagInd = srcGroup.StartKPTagInd;
            StartSignal = srcGroup.StartSignal;
            Elems.Clear();

            foreach (Elem srcElem in srcGroup.Elems)
            {
                Elem destElem = CreateElem();
                destElem.Name = srcElem.Name;
                destElem.ElemType = srcElem.ElemType;
                destElem.ByteOrder = srcElem.ByteOrder; // copy the array reference
                destElem.ByteOrderStr = srcElem.ByteOrderStr;
                Elems.Add(destElem);
            }
        }

        /// <summary>
        /// Creates a new Modbus element.
        /// </summary>
        public virtual Elem CreateElem()
        {
            return new Elem();
        }

        /// <summary>
        /// Обновить код функции в соответствии с типом таблицы данных
        /// </summary>
        public void UpdateFuncCode()
        {
            switch (TableType)
            {
                case TableType.DiscreteInputs:
                    FuncCode = FuncCodes.ReadDiscreteInputs;
                    break;
                case TableType.Coils:
                    FuncCode = FuncCodes.ReadCoils;
                    break;
                case TableType.InputRegisters:
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
                case ElemType.UShort:
                    return BitConverter.ToUInt16(buf, 0);
                case ElemType.Short:
                    return BitConverter.ToInt16(buf, 0);
                case ElemType.UInt:
                    return BitConverter.ToUInt32(buf, 0);
                case ElemType.Int:
                    return BitConverter.ToInt32(buf, 0);
                case ElemType.ULong:
                    return BitConverter.ToUInt64(buf, 0);
                case ElemType.Long:
                    return BitConverter.ToInt64(buf, 0);
                case ElemType.Float:
                    return BitConverter.ToSingle(buf, 0);
                case ElemType.Double:
                    return BitConverter.ToDouble(buf, 0);
                case ElemType.Bool:
                    return buf[0] > 0 ? 1.0 : 0.0;
                default:
                    return 0.0;
            }
        }
    }
}
