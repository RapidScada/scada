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
using Scada.Comm.Devices;
using Scada.Comm.Devices.KpSnmp;
using Scada.Data;
using System;
using System.Collections.Generic;
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
        private class VarGroup
        {
            private VarGroup()
            {
            }
            public VarGroup(string name, int varCnt, int startSignal)
            {
                Name = name;
                VarNames = new string[varCnt];
                Variables = new Variable[varCnt];
                StartSignal = startSignal;
            }

            public string Name { get; private set; }
            public string[] VarNames { get; private set; }
            public Variable[] Variables { get; private set; }
            public int StartSignal { get; private set; }
        }


        private Config config;        // конфигурация КП
        private VarGroup[] varGroups;
        private IPEndPoint endPoint;


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpSnmpLogic(int number)
            : base(number)
        {
            ConnRequired = false;

            config = new Config();
            varGroups = null;
            endPoint = null;
        }


        /// <summary>
        /// Создать переменную для работы по SNMP на основе переменной конфигурации
        /// </summary>
        private Variable CreateVariable(Config.Variable configVar)
        {
            return new Variable(new ObjectIdentifier(configVar.OID));
        }


        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        public override void Session()
        {
            base.Session();

            if (varGroups == null)
            {
                WriteToLog(Localization.UseRussian ?
                    "Нормальное взаимодействие с КП невозможно, т.к. конфигурация не загружена" :
                    "Normal device communication is impossible because configuration has not been loaded");
                Thread.Sleep(ReqParams.Delay);
                lastCommSucc = false;
            }
            else if (varGroups.Length == 0)
            {
                WriteToLog(Localization.UseRussian ?
                    "Отсутствуют переменные для запроса" :
                    "No variables for request");
                Thread.Sleep(ReqParams.Delay);
            }
            else
            {
                const string community = "public";
                const VersionCode version = VersionCode.V2;
                OctetString communityStr = new OctetString(community);

                foreach (VarGroup varGroup in varGroups)
                {
                    WriteToLog("Запрос " + varGroup.Name);
                    IList<Variable> receivedVariables =
                        Messenger.Get(version, endPoint, communityStr, varGroup.Variables, ReqParams.Timeout);

                    foreach (Variable variable in receivedVariables)
                    {
                        WriteToLog(variable.ToString());
                    }
                }
            }

            CalcSessStats();
        }
        
        /// <summary>
        /// Отправить команду ТУ
        /// </summary>
        public override void SendCmd(Command cmd)
        {
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
                // инициализация тегов КП и групп переменныхна основе конфигурации
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

                // !!! TODO:
                IPAddress ip = IPAddress.Parse(CallNum);
                endPoint = new IPEndPoint(ip, 161);
            }
            else
            {
                WriteToLog(errMsg);
            }
        }
   }
}
