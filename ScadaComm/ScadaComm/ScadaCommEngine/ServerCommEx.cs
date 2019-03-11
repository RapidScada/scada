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
 * Module   : ScadaCommEngine
 * Summary  : Implements communication with Server within the Communicator application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2019
 */

using Scada.Client;
using Scada.Comm.Devices;
using Scada.Data.Models;
using Scada.Data.Tables;
using System.Collections.Generic;
using Utils;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Implements communication with Server within the Communicator application.
    /// <para>Реализует обмен данными с Сервером в приложении Коммуникатор.</para>
    /// </summary>
    internal sealed class ServerCommEx : ServerComm
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ServerCommEx(Settings.CommonParams commonParams, Log log)
            : base()
        {
            commSettings = new CommSettings(commonParams.ServerHost, commonParams.ServerPort, 
                commonParams.ServerUser, commonParams.ServerPwd, commonParams.ServerTimeout);
            this.log = log;
        }


        /// <summary>
        /// Преобразовать срез параметров в срез входных каналов
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
            SrezTableLight.Srez srez = ConvertSrez(curSrez);
            return srez == null || SendSrez(srez, out bool result) && result;
        }

        /// <summary>
        /// Отправить архивный срез SCADA-Серверу
        /// </summary>
        public bool SendArchive(KPLogic.TagSrez arcSrez)
        {
            SrezTableLight.Srez srez = ConvertSrez(arcSrez);
            return srez == null || SendArchive(srez, out bool result) && result;
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
                EventTableLight.Event ev = new EventTableLight.Event
                {
                    Number = kpEvent.KPNum,
                    DateTime = kpEvent.DateTime,
                    ObjNum = kpEvent.KPTag.ObjNum,
                    KPNum = kpEvent.KPNum,
                    ParamID = kpEvent.KPTag.ParamID,
                    CnlNum = kpEvent.KPTag.CnlNum,
                    OldCnlVal = kpEvent.OldData.Val,
                    OldCnlStat = kpEvent.OldData.Stat,
                    NewCnlVal = kpEvent.NewData.Val,
                    NewCnlStat = kpEvent.NewData.Stat,
                    Checked = kpEvent.Checked,
                    UserID = kpEvent.UserID,
                    Descr = kpEvent.Descr,
                    Data = kpEvent.Data
                };

                return SendEvent(ev, out bool result) && result;
            }
        }
    }
}
