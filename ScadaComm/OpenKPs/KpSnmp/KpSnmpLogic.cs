/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Modified : 2016
 * 
 * Description
 * Interacting with controllers via SNMP v2c protocol.
 */

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Scada.Comm.Devices.KpSnmp;
using Scada.Data;
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
                VarNames = new string[varCnt];
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
            /// Получить имена переменных
            /// </summary>
            public string[] VarNames { get; private set; }
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
        /// Используемая версия SNMP
        /// </summary>
        private const VersionCode SnmpVersion = VersionCode.V2;

        private Config config;              // конфигурация КП
        private bool fatalError;            // фатальная ошибка при инициализации КП
        private VarGroup[] varGroups;       // группы запрашиваемых переменных
        private IPEndPoint endPoint;        // адрес и порт для соединения с устройством
        private OctetString readCommunity;  // пароль на чтение данных
        private OctetString writeCommunity; // пароль на запись данных


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpSnmpLogic(int number)
            : base(number)
        {
            CanSendCmd = true;
            ConnRequired = false;

            config = new Config();
            fatalError = false;
            varGroups = null;
            endPoint = null;
            readCommunity = null;
            writeCommunity = null;
        }


        /// <summary>
        /// Создать переменную для работы по SNMP на основе переменной конфигурации
        /// </summary>
        private Variable CreateVariable(Config.Variable configVar)
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
            varGroups = new VarGroup[groupCnt];
            int signal = 1;

            for (int i = 0; i < groupCnt; i++)
            {
                Config.VarGroup configVarGroup = config.VarGroups[i];
                int varCnt = configVarGroup.Variables.Count;
                TagGroup tagGroup = new TagGroup(configVarGroup.Name);
                VarGroup varGroup = new VarGroup(configVarGroup.Name, varCnt, signal);

                for (int j = 0; j < varCnt; j++)
                {
                    Config.Variable configVar = configVarGroup.Variables[j];
                    KPTag kpTag = new KPTag(signal++, configVar.Name);
                    tagGroup.KPTags.Add(kpTag);
                    varGroup.VarNames[j] = configVar.Name;
                    varGroup.Variables[j] = CreateVariable(configVar);
                }

                tagGroups.Add(tagGroup);
                varGroups[i] = varGroup;
            }

            InitKPTags(tagGroups);
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
        /// Извлечь адрес и порт для соединения с устройством
        /// </summary>
        private void RetrieveEndPoint()
        {
            IPAddress addr;
            int port;
            CommUtils.ExtractAddrAndPort(CallNum, DefaultPort, out addr, out port);
            endPoint = new IPEndPoint(addr, port);
        }

        /// <summary>
        /// Преобразовать данные переменной SNMP в строку для вывода в журнал
        /// </summary>
        private string ConvertVarDataToString(ISnmpData data)
        {
            if (data == null)
            {
                return "null";
            }
            else
            {
                return new StringBuilder(data.ToString())
                    .Append(" (")
                    .Append(data.TypeCode.ToString())
                    .Append(")").ToString();
            }
        }

        /// <summary>
        /// Расшифровать данные переменной SNMP
        /// </summary>
        private SrezTable.CnlData DecodeVarData(ISnmpData data)
        {
            if (data == null)
            {
                return SrezTable.CnlData.Empty;
            }
            else
            {
                try
                {
                    switch (data.TypeCode)
                    {
                        case SnmpType.Integer32:
                            return new SrezTable.CnlData(((Integer32)data).ToInt32(), 1);
                        case SnmpType.Counter32:
                            return new SrezTable.CnlData(((Counter32)data).ToUInt32(), 1);
                        case SnmpType.Counter64:
                            return new SrezTable.CnlData(((Counter64)data).ToUInt64(), 1);
                        case SnmpType.TimeTicks:
                            return new SrezTable.CnlData(((TimeTicks)data).ToUInt32(), 1);
                        case SnmpType.OctetString:
                            string s = data.ToString().Trim();
                            double val;
                            if (s.Equals("true", StringComparison.OrdinalIgnoreCase))
                                return new SrezTable.CnlData(1.0, 1);
                            else if (s.Equals("false", StringComparison.OrdinalIgnoreCase))
                                return new SrezTable.CnlData(0.0, 1);
                            else if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
                                return new SrezTable.CnlData(val, 1);
                            else
                                return SrezTable.CnlData.Empty;
                        default:
                            return SrezTable.CnlData.Empty;
                    }
                }
                catch (Exception ex)
                {
                    WriteToLog(string.Format(Localization.UseRussian ? 
                        "Ошибка при расшифровке данных \"{0}\" типа {1}: {2}" :
                        "Error decoding data \"{0}\" of type {1}: {2}", 
                        data.ToString(), data.TypeCode.ToString(), ex.Message));
                    return SrezTable.CnlData.Empty;
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
        private bool FindVariableBySignal(int signal, out string name, out ObjectIdentifier oid)
        {
            foreach (VarGroup varGroup in varGroups)
            {
                if (varGroup.StartSignal <= signal && signal < varGroup.StartSignal + varGroup.Variables.Length)
                {
                    int index = signal - varGroup.StartSignal;
                    name = varGroup.VarNames[index];
                    oid = varGroup.Variables[index].Id;
                    return true;
                }
            }

            name = "";
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
                                SnmpVersion, endPoint, readCommunity, varGroup.Variables, ReqParams.Timeout);

                            if (receivedVars == null || receivedVars.Count != varCnt)
                                throw new Exception(KpPhrases.VariablesMismatch);

                            for (int i = 0, tagInd = varGroup.StartSignal - 1; i < varCnt; i++, tagInd++)
                            {
                                Variable receivedVar = receivedVars[i];

                                if (receivedVar.Id != varGroup.Variables[i].Id)
                                    throw new Exception(KpPhrases.VariablesMismatch);

                                WriteToLog(varGroup.VarNames[i] + " = " + ConvertVarDataToString(receivedVar.Data));

                                // расшифровка данных переменной и установка данных тега КП
                                SetCurData(tagInd, DecodeVarData(receivedVar.Data));
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
                        WriteToLog((Localization.UseRussian ?
                            "Ошибка при получении переменных: " :
                            "Error getting variables: ") + ex.Message);
                    }

                    // завершение запроса
                    FinishRequest();
                    tryNum++;
                }
            }

            // установка недостоверности данных в случае ошибки
            if (!lastCommSucc)
                InvalidateCurData(varGroup.StartSignal - 1, varCnt);
        }

        /// <summary>
        /// Отправить команду установки переменной
        /// </summary>
        private void Command(string varName, Variable variable)
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
                    IList<Variable> sentVars = Messenger.Set(SnmpVersion, endPoint, writeCommunity,
                        new List<Variable>() { variable }, ReqParams.Timeout);

                    if (sentVars == null || sentVars.Count != 1 || sentVars[0].Id != variable.Id)
                        throw new Exception(KpPhrases.VariablesMismatch);
                        
                    WriteToLog(varName + " = " + ConvertVarDataToString(sentVars[0].Data));
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
                Thread.Sleep(ReqParams.Delay);
            }
            else
            {
                string varName;
                ObjectIdentifier varOid;

                if ((cmd.CmdTypeID == BaseValues.CmdTypes.Standard || cmd.CmdTypeID == BaseValues.CmdTypes.Binary) &&
                    FindVariableBySignal(cmd.CmdNum, out varName, out varOid))
                {
                    ISnmpData data = cmd.CmdTypeID == BaseValues.CmdTypes.Standard ? 
                        new Integer32((int)cmd.CmdVal) :
                        EncodeVarData(cmd.GetCmdDataStr());

                    if (data == null)
                        WriteToLog(CommPhrases.IncorrectCmdData);
                    else
                        Command(varName, new Variable(varOid, data));
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
            string errMsg;
            bool configLoaded = config.Load(Config.GetFileName(AppDirs.ConfigDir, Number), out errMsg);
            
            if (configLoaded)
            {
                // инициализация данных КП
                try
                {
                    InitKPTags();
                    RetrieveCommunities();
                    RetrieveEndPoint();
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
   }
}
