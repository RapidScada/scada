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
 * Module   : KpSms
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2015
 * 
 * Description
 * Device library for testing.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Scada.Data;
using Scada.Comm.Channels;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic
    /// <para>Логика работы КП</para>
    /// </summary>
    public sealed class KpTestLogic : KPLogic
    {
        private static readonly Connection.TextStopCondition ReadStopCondition = 
            new Connection.TextStopCondition("\n");

        private Random random;

        public KpTestLogic(int number)
            : base(number)
        {
            random = new Random();
            ConnRequired = true;

            List<TagGroup> tagGroups = new List<TagGroup>();
            TagGroup tagGroup = new TagGroup("Group 1");
            tagGroup.KPTags.Add(new KPTag(1, "Tag 1"));
            tagGroup.KPTags.Add(new KPTag(2, "Tag 2"));
            tagGroup.KPTags.Add(new KPTag(3, "Tag 3"));
            tagGroup.KPTags.Add(new KPTag(4, "Tag 4"));
            tagGroup.KPTags.Add(new KPTag(5, "Tag 5"));
            tagGroups.Add(tagGroup);

            tagGroup = new TagGroup("Group 2");
            tagGroup.KPTags.Add(new KPTag(6, "Tag 6"));
            tagGroup.KPTags.Add(new KPTag(7, "Tag 7"));
            tagGroup.KPTags.Add(new KPTag(8, "Tag 8"));
            tagGroup.KPTags.Add(new KPTag(9, "Tag 9"));
            tagGroup.KPTags.Add(new KPTag(10, "Tag 10"));
            tagGroups.Add(tagGroup);

            InitKPTags(tagGroups);
        }

        public override void Session()
        {
            base.Session();

            // write to the serial port
            // запись в последовательный порт
            string logText;
            Connection.WriteLine("test " + Address, out logText);
            WriteToLog(logText);

            // read from the serial port
            // чтение из последовательного порта
            bool stopReceived;
            Connection.ReadLines(ReqParams.Timeout, ReadStopCondition, out stopReceived, out logText);
            WriteToLog(logText);

            // finish request
            // завершение запроса
            FinishRequest();

            // generate current data
            // генерация текущих значений
            for (int i = 0; i < KPTags.Length; i++)
                SetCurData(i, (double)random.Next(1000) / 10.0, 1);

            // generate archive data
            // генерация архивного среза
            TagSrez srez = new TagSrez(1);
            srez.DateTime = DateTime.Now;
            srez.KPTags[0] = KPTags[0];
            srez.TagData[0] = curData[0];
            srez.Descr = "Test archive data " + random.Next(100);
            AddArcSrez(srez);
            CommLineSvc.FlushArcData(this);

            // generate event
            // генерация события
            KPEvent kpEvent = new KPEvent(DateTime.Now, Number, KPTags[0]);
            kpEvent.Descr = "Test event " + random.Next(100);
            AddEvent(kpEvent);
            CommLineSvc.FlushArcData(this);

            // calculate stats
            // расчёт статистики
            CalcSessStats();
        }

        public override void SendCmd(Command cmd)
        {
            base.SendCmd(cmd);
            CalcCmdStats();
        }
    }
}
