/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : SCADA-Communicator Service
 * Summary  : Communication with SCADA-Server adapted for SCADA-Communicator
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2015
 */

using Scada.Client;
using Scada.Comm.Devices;
using Scada.Data;
using System.Collections.Generic;
using Utils;

namespace Scada.Comm.Svc
{
    /// <summary>
    /// Communication with SCADA-Server adapted for SCADA-Communicator
    /// <para>Обмен данными со SCADA-Сервером, адаптированный для SCADA-Коммуникатора</para>
    /// </summary>
    sealed class ServerCommEx : ServerComm
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        private ServerCommEx()
        {
        }

        /// <summary>
        /// Конструктор с установкой общих параметров конфигурации и log-файла
        /// </summary>
        public ServerCommEx(Settings.CommonParams commonParams, Log log)
            : base()
        {
            this.commSettings = new CommSettings(commonParams.ServerHost, commonParams.ServerPort, 
                commonParams.ServerUser, commonParams.ServerPwd, commonParams.ServerTimeout);
            this.log = log;
        }


        /// <summary>
        /// Преобразовать среза параметров в срез входных каналов
        /// </summary>
        private SrezTableLight.Srez ConvertSrez(KPLogic.TagSrez tagSrez)
        {
            List<int> boundIndexes;
            int cnlCnt;

            if (tagSrez == null)
            {
                boundIndexes = null;
                cnlCnt = 0;
            }
            else
            {
                boundIndexes = tagSrez.GetBoundTagIndexes();
                cnlCnt = boundIndexes.Count;
            }

            if (cnlCnt == 0)
            {
                return null;
            }
            else
            {
                SrezTableLight.Srez srez = new SrezTableLight.Srez(tagSrez.DateTime, cnlCnt);

                for (int i = 0; i < cnlCnt; i++)
                {
                    int tagInd = boundIndexes[i];
                    srez.CnlNums[i] = tagSrez.KPTags[tagInd].CnlNum;
                    srez.CnlData[i] = tagSrez.TagData[tagInd];
                }

                return srez;
            }
        }


        /// <summary>
        /// Отправить текущий срез SCADA-Серверу
        /// </summary>
        public bool SendSrez(KPLogic.TagSrez curSrez)
        {
            bool result;
            SrezTableLight.Srez srez = ConvertSrez(curSrez);
            return srez == null || SendSrez(srez, out result) && result;
        }

        /// <summary>
        /// Отправить архивный срез SCADA-Серверу
        /// </summary>
        public bool SendArchive(KPLogic.TagSrez arcSrez)
        {
            bool result;
            SrezTableLight.Srez srez = ConvertSrez(arcSrez);
            return srez == null || SendArchive(srez, out result) && result;
        }

        /// <summary>
        /// Отправить событие SCADA-Серверу
        /// </summary>
        public bool SendEvent(KPLogic.KPEvent kpEvent)
        {
            if (kpEvent == null || kpEvent.KPTag == null || kpEvent.KPTag.CnlNum <= 0)
            {
                return true;
            }
            else
            {
                EventTableLight.Event ev = new EventTableLight.Event();
                ev.Number = kpEvent.KPNum;
                ev.DateTime = kpEvent.DateTime;
                ev.ObjNum = kpEvent.KPTag.ObjNum;
                ev.KPNum = kpEvent.KPNum;
                ev.ParamID = kpEvent.KPTag.ParamID;
                ev.CnlNum = kpEvent.KPTag.CnlNum;
                ev.OldCnlVal = kpEvent.OldData.Val;
                ev.OldCnlStat = kpEvent.OldData.Stat;
                ev.NewCnlVal = kpEvent.NewData.Val;
                ev.NewCnlStat = kpEvent.NewData.Stat;
                ev.Checked = kpEvent.Checked;
                ev.UserID = kpEvent.UserID;
                ev.Descr = kpEvent.Descr;
                ev.Data = kpEvent.Data;

                bool result;
                return SendEvent(ev, out result) && result;
            }
        }

        /// <summary>
        /// Принять команду от SCADA-Сервера
        /// </summary>
        public bool ReceiveCommand(out Command cmd)
        {
            int kpNum;
            int cmdNum;
            double cmdVal;
            byte[] cmdData;

            if (ReceiveCommand(out kpNum, out cmdNum, out cmdVal, out cmdData))
            {
                int cmdType = cmdData == null ? double.IsNaN(cmdVal) ? 
                    BaseValues.CmdTypes.Request : BaseValues.CmdTypes.Standard : BaseValues.CmdTypes.Binary;
                cmd = new Command(cmdType);
                cmd.KPNum = kpNum;
                cmd.CmdNum = cmdNum;
                if (cmdType == BaseValues.CmdTypes.Standard)
                    cmd.CmdVal = cmdVal;
                else if (cmdType == BaseValues.CmdTypes.Binary)
                    cmd.CmdData = cmdData;
                return true;
            }
            else
            {
                cmd = null;
                return false;
            }
        }
    }
}
