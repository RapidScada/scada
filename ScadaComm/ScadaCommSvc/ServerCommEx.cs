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
 * Module   : SCADA-Communicator Service
 * Summary  : Communication with SCADA-Server adapted for SCADA-Communicator
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using Scada.Client;
using Scada.Comm.KP;
using Scada.Data;
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
        public ServerCommEx(Manager.CommonParams commonParams, Log log)
            : base()
        {
            this.commSettings = new CommSettings(commonParams.ServerHost, commonParams.ServerPort, 
                commonParams.ServerUser, commonParams.ServerPwd, commonParams.ServerTimeout);
            this.log = log;
        }


        /// <summary>
        /// Преобразовать среза параметров в срез входных каналов
        /// </summary>
        private SrezTableLight.Srez ConvertSrez(KPLogic.ParamSrez paramSrez)
        {
            List<int> bindedIndexes;
            int cnlCnt;

            if (paramSrez == null)
            {
                bindedIndexes = null;
                cnlCnt = 0;
            }
            else
            {
                bindedIndexes = paramSrez.GetBindedParamIndexes();
                cnlCnt = bindedIndexes.Count;
            }

            if (cnlCnt == 0)
            {
                return null;
            }
            else
            {
                SrezTableLight.Srez srez = new SrezTableLight.Srez(paramSrez.DateTime, cnlCnt);

                for (int i = 0; i < cnlCnt; i++)
                {
                    int paramInd = bindedIndexes[i];
                    srez.CnlNums[i] = paramSrez.KPParams[paramInd].CnlNum;
                    KPLogic.ParamData paramData = paramSrez.Data[paramInd];
                    SrezTableLight.CnlData cnlData = new SrezTableLight.CnlData(paramData.Val, paramData.Stat);
                    srez.CnlData[i] = cnlData;
                }

                return srez;
            }
        }


        /// <summary>
        /// Отправить текущий срез SCADA-Серверу
        /// </summary>
        public bool SendSrez(KPLogic.ParamSrez curSrez)
        {
            bool result;
            SrezTableLight.Srez srez = ConvertSrez(curSrez);
            return srez == null || SendSrez(srez, out result) && result;
        }

        /// <summary>
        /// Отправить архивный срез SCADA-Серверу
        /// </summary>
        public bool SendArchive(KPLogic.ParamSrez arcSrez)
        {
            bool result;
            SrezTableLight.Srez srez = ConvertSrez(arcSrez);
            return srez == null || SendArchive(srez, out result) && result;
        }

        /// <summary>
        /// Отправить событие SCADA-Серверу
        /// </summary>
        public bool SendEvent(KPLogic.Event aEvent)
        {
            if (aEvent == null || aEvent.KPParam == null || aEvent.KPParam.CnlNum <= 0)
            {
                return true;
            }
            else
            {
                EventTableLight.Event ev = new EventTableLight.Event();
                ev.Number = aEvent.KPNum;
                ev.DateTime = aEvent.DateTime;
                ev.ObjNum = aEvent.KPParam.ObjNum;
                ev.KPNum = aEvent.KPNum;
                ev.ParamID = aEvent.KPParam.ParamID;
                ev.CnlNum = aEvent.KPParam.CnlNum;
                ev.OldCnlVal = aEvent.OldData.Val;
                ev.OldCnlStat = aEvent.OldData.Stat;
                ev.NewCnlVal = aEvent.NewData.Val;
                ev.NewCnlStat = aEvent.NewData.Stat;
                ev.Checked = aEvent.Checked;
                ev.UserID = aEvent.UserID;
                ev.Descr = aEvent.Descr;
                ev.Data = aEvent.Data;

                bool result;
                return SendEvent(ev, out result) && result;
            }
        }

        /// <summary>
        /// Принять команду от SCADA-Сервера
        /// </summary>
        public bool ReceiveCommand(out KPLogic.Command cmd)
        {
            int kpNum;
            int cmdNum;
            double cmdVal;
            byte[] cmdData;

            if (ReceiveCommand(out kpNum, out cmdNum, out cmdVal, out cmdData))
            {
                cmd = new KPLogic.Command();
                cmd.CmdType = cmdData == null ? 
                    (double.IsNaN(cmdVal) ? KPLogic.CmdType.Request : KPLogic.CmdType.Standard) :
                    KPLogic.CmdType.Binary;
                cmd.KPNum = kpNum;
                cmd.CmdNum = cmdNum;
                if (cmd.CmdType == KPLogic.CmdType.Standard)
                    cmd.CmdVal = cmdVal;
                else if (cmd.CmdType == KPLogic.CmdType.Binary)
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
