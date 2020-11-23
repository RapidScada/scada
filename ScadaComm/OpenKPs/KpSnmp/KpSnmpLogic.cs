/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : KpSnmp
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 * 
 * Description
 * Interacting with controllers via SNMP protocol.
 */

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Scada.Comm.Devices.KpSnmp;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic
    /// <para>Логика работы КП</para>
    /// </summary>
    public class KpSnmpLogic : KPLogic
    {
        /// <summary>
        /// Группа запрашиваемых переменных
        /// </summary>
        private class VarGroup
        {
            /// <summary>
            /// Конструктор, ограничивающий создание объекта без параметров
            /// </summary>
            private VarGroup()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public VarGroup(string name, int varCnt, int startSignal)
            {
                Name = name;
                Variables = new Variable[varCnt];
                StartSignal = startSignal;
                ReqDescr = string.Format(Localization.UseRussian ?
                    "Получение значений переменных группы \"{0}\"" :
                    "Get variables of the group \"{0}\"", Name);
            }

            /// <summary>
            /// Получить имя группы
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// Получить переменные
            /// </summary>
            public Variable[] Variables { get; private set; }
            /// <summary>
            /// Получить начальный сигнал КП для переменных группы
            /// </summary>
            public int StartSignal { get; private set; }
            /// <summary>
            /// Получить описание запроса переменных группы
            /// </summary>
            public string ReqDescr { get; private set; }
        }


        /// <summary>
        /// Номер порта  по умолчанию
        /// </summary>
        private const int DefaultPort = 161;
        /// <summary>
        /// Макс. длина строкового значения тега
        /// </summary>
        private const int MaxTagStrLen = 50;

        private KpConfig config;            // конфигурация КП
        private bool fatalError;            // фатальная ошибка при инициализации КП
        private VarGroup[] varGroups;       // группы запрашиваемых переменных
        private string[] strVals;           // строковые значения тегов для отображения
        private bool[] isBitsArr;           // признаки, что теги имеют тип BITS
        private IPEndPoint endPoint;        // адрес и порт для соединения с устройством
        private OctetString readCommunity;  // пароль на чтение данных
        private OctetString writeCommunity; // пароль на запись данных
        private VersionCode snmpVersion;    // версия SNMP


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpSnmpLogic(int number)
            : base(number)
        {
            CanSendCmd = true;
            ConnRequired = false;

            config = new KpConfig();
            fatalError = false;
            varGroups = null;
            strVals = null;
            isBitsArr = null;
            endPoint = null;
            readCommunity = null;
            writeCommunity = null;
            snmpVersion = VersionCode.V2;
        }


        /// <summary>
        /// Создать переменную для работы по SNMP на основе переменной конфигурации
        /// </summary>
        private Variable CreateVariable(KpConfig.Variable configVar)
        {
            try
            {
                return new Variable(new ObjectIdentifier(configVar.OID));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Localization.UseRussian ?
                    "Ошибка при создании переменной \"{0}\" с идентификатором {1}: {2}" :
                    "Error creating variable \"{0}\" with identifier {1}: {2}",
                    configVar.Name, configVar.OID, ex.Message));
            }
        }

        /// <summary>
        /// Инициализировать теги КП и группы переменных на основе конфигурации
        /// </summary>
        private void InitKPTags()
        {
            int groupCnt = config.VarGroups.Count;
            List<TagGroup> tagGroups = new List<TagGroup>(groupCnt);
            List<bool> isBitsList = new List<bool>();
            varGroups = new VarGroup[groupCnt];
            int signal = 1;

            for (int i = 0; i < groupCnt; i++)
            {
                KpConfig.VarGroup configVarGroup = config.VarGroups[i];
                int varCnt = configVarGroup.Variables.Count;
                TagGroup tagGroup = new TagGroup(configVarGroup.Name);
                VarGroup varGroup = new VarGroup(configVarGroup.Name, varCnt, signal);

                for (int j = 0; j < varCnt; j++)
                {
                    KpConfig.Variable configVar = configVarGroup.Variables[j];
                    KPTag kpTag = new KPTag(signal++, configVar.Name);
                    tagGroup.KPTags.Add(kpTag);
                    varGroup.Variables[j] = CreateVariable(configVar);
                    isBitsList.Add(configVar.IsBits);
                }

                tagGroups.Add(tagGroup);
                varGroups[i] = varGroup;
            }

            InitKPTags(tagGroups);

            strVals = new string[KPTags.Length];
            Array.Clear(strVals, 0, strVals.Length);
            isBitsArr = isBitsList.ToArray();
        }

        /// <summary>
        /// Извлечь пароли на чтение и запись данных из конфигурации
        /// </summary>
        private void RetrieveCommunities()
        {
            try
            {
                readCommunity = new OctetString(config.ReadCommunity);
                writeCommunity = new OctetString(config.WriteCommunity);
            }
            catch (Exception ex)
            {
                throw new Exception((Localization.UseRussian ?
                    "Ошибка при извлечении паролей: " :
                    "Error retrieving communities: ") + ex.Message);
            }
        }

        /// <summary>
        /// Извлечь версию SNMP из конфигурации
        /// </summary>
        private void RetrieveSnmpVersion()
        {
            int ver = config.SnmpVersion;
            if (ver == 1)
                snmpVersion = VersionCode.V1;
            else if (ver == 2)
                snmpVersion = VersionCode.V2;
            else
                throw new Exception(string.Format(Localization.UseRussian ?
                    "SNMP v{0} не поддерживается." :
                    "SNMP v{0} is not supported.", ver));
        }

        /// <summary>
        /// Извлечь адрес и порт для соединения с устройством
        /// </summary>
        private void RetrieveEndPoint()
        {
            CommUtils.ExtractAddrAndPort(CallNum, DefaultPort, out IPAddress addr, out int port);
            endPoint = new IPEndPoint(addr, port);
        }

        /// <summary>
        /// Преобразовать данные переменной SNMP в строку для вывода в журнал
        /// </summary>
        private string ConvertVarDataToString(ISnmpData snmpData, bool isBits, int maxLen = -1, bool addType = true)
        {
            if (snmpData == null)
            {
                return "null";
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                if (snmpData is OctetString)
                {
                    if (isBits)
                    {
                        sb.Append(((OctetString)snmpData).ToHexString());
                    }
                    else
                    {
                        string s = snmpData.ToString();
                        if (maxLen > 0 && s.Length > maxLen)
                            sb.Append(s.Substring(0, maxLen)).Append("...");
                        else
                            sb.Append(s);
                    }
                }
                else
                {
                    sb.Append(snmpData.ToString());
                }

                if (addType)
                    sb.Append(" (").Append(snmpData.TypeCode.ToString()).Append(")");

                return sb.ToString();
            }
        }

        /// <summary>
        /// Декодировать данные переменной SNMP
        /// </summary>
        private bool DecodeVarData(ISnmpData snmpData, bool isBits, 
            out SrezTableLight.CnlData tagData, out bool isString)
        {
            tagData = SrezTableLight.CnlData.Empty;
            isString = false;

            if (snmpData == null)
            {
                return false;
            }
            else if (isBits)
            {
                if (snmpData.TypeCode == SnmpType.OctetString)
                {
                    // получение значения типа BITS: последний полученный байт строки - младший байт значения тега
                    byte[] snmpRaw = ((OctetString)snmpData).GetRaw();
                    byte[] tagRaw = new byte[8];

                    for (int i = 0, j = snmpRaw.Length - 1; i < 8; i++, j--)
                        tagRaw[i] = j >= 0 ? snmpRaw[j] : (byte)0;

                    tagData = new SrezTableLight.CnlData(BitConverter.ToUInt64(tagRaw, 0), 1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    switch (snmpData.TypeCode)
                    {
                        case SnmpType.Integer32:
                            tagData = new SrezTableLight.CnlData(((Integer32)snmpData).ToInt32(), 1);
                            return true;
                        case SnmpType.Counter32:
                            tagData = new SrezTableLight.CnlData(((Counter32)snmpData).ToUInt32(), 1);
                            return true;
                        case SnmpType.Counter64:
                            tagData = new SrezTableLight.CnlData(((Counter64)snmpData).ToUInt64(), 1);
                            return true;
                        case SnmpType.Gauge32:
                            tagData = new SrezTableLight.CnlData(((Gauge32)snmpData).ToUInt32(), 1);
                            return true;
                        case SnmpType.TimeTicks:
                            tagData = new SrezTableLight.CnlData(((TimeTicks)snmpData).ToUInt32(), 1);
                            return true;
                        case SnmpType.OctetString:
                            string s = snmpData.ToString().Trim();
                            double val;
                            if (s.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                tagData = new SrezTableLight.CnlData(1.0, 1);
                                return true;
                            }
                            else if (s.Equals("false", StringComparison.OrdinalIgnoreCase))
                            {
                                tagData = new SrezTableLight.CnlData(0.0, 1);
                                return true;
                            }
                            else if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
                            {
                                tagData = new SrezTableLight.CnlData(val, 1);
                                return true;
                            }
                            else
                            {
                                // преобразование 8 байт строки в число double
                                tagData = new SrezTableLight.CnlData(ScadaUtils.EncodeAscii(s), 1);
                                isString = true;
                                return true;
                            }
                        default:
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Ошибка при расшифровке данных \"{0}\" типа {1}: {2}" :
                        "Error decoding data \"{0}\" of type {1}: {2}",
                        snmpData.ToString(), snmpData.TypeCode.ToString(), ex.Message));
                    return false;
                }
            }
        }

        /// <summary>
        /// Закодировать данные переменной SNMP, используя данные команды КП
        /// </summary>
        private ISnmpData EncodeVarData(string cmdDataStr)
        {
            cmdDataStr = cmdDataStr.TrimStart();
            if (cmdDataStr.Length == 1 || cmdDataStr.Length >= 3 && cmdDataStr[1] == ' ')
            {
                char typeCode = cmdDataStr[0];
                string valStr = cmdDataStr.Length >= 3 ? cmdDataStr.Substring(2) : "";

                try
                {
                    switch (typeCode)
                    {
                        case 'i':
                            return new Integer32(int.Parse(valStr));
                        case 'u':
                            return new Gauge32(uint.Parse(valStr));
                        case 't':
                            return new TimeTicks(uint.Parse(valStr));
                        case 'a':
                            return new IP(valStr);
                        case 'o':
                            return new ObjectIdentifier(valStr);
                        case 's':
                            return new OctetString(valStr);
                        case 'x':
                            return new OctetString(ByteTool.Convert(valStr));
                        case 'd':
                            return new OctetString(ByteTool.ConvertDecimal(valStr));
                        case 'n':
                            return new Null();
                        default:
                            return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Найти переменную по сигналу
        /// </summary>
        private bool FindVariableBySignal(int signal, out ObjectIdentifier oid)
        {
            foreach (VarGroup varGroup in varGroups)
            {
                if (varGroup.StartSignal <= signal && signal < varGroup.StartSignal + varGroup.Variables.Length)
                {
                    int index = signal - varGroup.StartSignal;
                    oid = varGroup.Variables[index].Id;
                    return true;
                }
            }

            oid = null;
            return false;
        }

        /// <summary>
        /// Выполнить запрос переменных группы
        /// </summary>
        private void Request(VarGroup varGroup)
        {
            int varCnt = varGroup.Variables.Length;

            if (lastCommSucc)
            {
                lastCommSucc = false;
                int tryNum = 0;

                while (RequestNeeded(ref tryNum))
                {
                    WriteToLog(varGroup.ReqDescr);

                    try
                    {
                        if (varCnt > 0)
                        {
                            // получение значений переменных
                            IList<Variable> receivedVars = Messenger.Get(
                                snmpVersion, endPoint, readCommunity, varGroup.Variables, ReqParams.Timeout);

                            if (receivedVars == null || receivedVars.Count != varCnt)
                                throw new Exception(KpPhrases.VariablesMismatch);

                            for (int i = 0, tagInd = varGroup.StartSignal - 1; i < varCnt; i++, tagInd++)
                            {
                                Variable receivedVar = receivedVars[i];

                                if (receivedVar.Id != varGroup.Variables[i].Id)
                                    throw new Exception(KpPhrases.VariablesMismatch);

                                ISnmpData snmpData = receivedVar.Data;
                                bool isBits = isBitsArr[tagInd];
                                WriteToLog(KPTags[tagInd].Name + " = " + ConvertVarDataToString(snmpData, isBits));

                                // расшифровка данных переменной и установка данных тега КП
                                bool decodeOK = DecodeVarData(snmpData, isBits, 
                                    out SrezTableLight.CnlData tagData, out bool isString);
                                SetCurData(tagInd, tagData);
                                strVals[tagInd] = isString ?
                                    ConvertVarDataToString(snmpData, isBits, MaxTagStrLen, false) : null;
                            }
                        }
                        else
                        {
                            WriteToLog(KpPhrases.NoVariables);
                        }

                        lastCommSucc = true;
                    }
                    catch (Exception ex)
                    {
                        // ex.ToString() содержит более подробную информацию о причине ошибки по сравнению с ex.Message
                        WriteToLog((Localization.UseRussian ?
                            "Ошибка при получении переменных: " :
                            "Error getting variables: ") + ex.ToString());
                    }

                    // завершение запроса
                    FinishRequest();
                    tryNum++;
                }

                // установка недостоверности данных в случае ошибки опроса заданной группы
                if (!lastCommSucc && tryNum > 0)
                    InvalidateCurData(varGroup.StartSignal - 1, varCnt);
            }
            else
            {
                // установка недостоверности данных в случае ошибки опроса предыдущей группы
                InvalidateCurData(varGroup.StartSignal - 1, varCnt);
            }
        }

        /// <summary>
        /// Отправить команду установки переменной
        /// </summary>
        private void Command(string varName, bool isBits, Variable variable)
        {
            lastCommSucc = false;
            int tryNum = 0;

            while (RequestNeeded(ref tryNum))
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Установка значения переменной \"{0}\"" :
                    "Set variable \"{0}\"", varName));

                try
                {
                    IList<Variable> sentVars = Messenger.Set(snmpVersion, endPoint, writeCommunity,
                        new List<Variable>() { variable }, ReqParams.Timeout);

                    if (sentVars == null || sentVars.Count != 1 || sentVars[0].Id != variable.Id)
                        throw new Exception(KpPhrases.VariablesMismatch);

                    WriteToLog(varName + " = " + ConvertVarDataToString(sentVars[0].Data, isBits));
                    lastCommSucc = true;
                }
                catch (Exception ex)
                {
                    WriteToLog((Localization.UseRussian ?
                        "Ошибка при установке переменной: " :
                        "Error setting variable: ") + ex.Message);
                }

                // завершение запроса
                FinishRequest();
                tryNum++;
            }
        }

        /// <summary>
        /// Преобразовать данные тега КП в строку
        /// </summary>
        protected override string ConvertTagDataToStr(int signal, SrezTableLight.CnlData tagData)
        {
            if (strVals[signal - 1] != null)
            {
                return strVals[signal - 1];
            }
            else if (tagData.Stat > 0 && isBitsArr[signal - 1])
            {
                return "0x" + ((long)tagData.Val).ToString("X");
            }
            else
            {
                return base.ConvertTagDataToStr(signal, tagData);
            }
        }


        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        public override void Session()
        {
            base.Session();

            if (fatalError)
            {
                WriteToLog(KpPhrases.CommunicationImpossible);
                Thread.Sleep(ReqParams.Delay);
                lastCommSucc = false;
            }
            else if (varGroups.Length == 0)
            {
                WriteToLog(KpPhrases.NoVariables);
                Thread.Sleep(ReqParams.Delay);
            }
            else
            {
                // запрос переменных по группам
                foreach (VarGroup varGroup in varGroups)
                    Request(varGroup);
            }

            // расчёт статистики
            CalcSessStats();
        }

        /// <summary>
        /// Отправить команду ТУ
        /// </summary>
        public override void SendCmd(Command cmd)
        {
            base.SendCmd(cmd);
            lastCommSucc = false;

            if (fatalError)
            {
                WriteToLog(KpPhrases.CommunicationImpossible);
            }
            else
            {
                int signal = cmd.CmdNum;

                if ((cmd.CmdTypeID == BaseValues.CmdTypes.Standard || cmd.CmdTypeID == BaseValues.CmdTypes.Binary) &&
                    FindVariableBySignal(signal, out ObjectIdentifier varOid))
                {
                    ISnmpData data = cmd.CmdTypeID == BaseValues.CmdTypes.Standard ?
                        new Integer32((int)cmd.CmdVal) :
                        EncodeVarData(cmd.GetCmdDataStr());

                    if (data == null)
                    {
                        WriteToLog(CommPhrases.IncorrectCmdData);
                    }
                    else
                    {
                        int tagInd = signal - 1;
                        Command(KPTags[tagInd].Name, isBitsArr[tagInd], new Variable(varOid, data));
                    }
                }
                else
                {
                    WriteToLog(CommPhrases.IllegalCommand);
                }
            }

            CalcCmdStats();
        }

        /// <summary>
        /// Выполнить действия после добавления КП на линию связи
        /// </summary>
        public override void OnAddedToCommLine()
        {
            // загрузка конфигурации КП
            bool configLoaded = config.Load(KpConfig.GetFileName(AppDirs.ConfigDir, Number, ReqParams.CmdLine), 
                out string errMsg);

            if (configLoaded)
            {
                try
                {
                    // инициализация тегов КП
                    InitKPTags();
                    fatalError = false;
                }
                catch
                {
                    fatalError = true;
                    throw;
                }
            }
            else
            {
                fatalError = true;
                throw new Exception(errMsg);
            }
        }

        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            if (!fatalError)
            {
                try
                {
                    // инициализация данных КП
                    RetrieveCommunities();
                    RetrieveSnmpVersion();
                    RetrieveEndPoint();
                }
                catch
                {
                    fatalError = true;
                    throw;
                }
            }
        }
    }
}
