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
 * Module   : KpSms
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2014
 * 
 * Description
 * Device library for testing.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace Scada.Comm.KP
{
    /// <summary>
    /// Device communication logic
    /// <para>Логика работы КП</para>
    /// </summary>
    public sealed class KpTestLogic : KPLogic
    {
        private Random random;

        public KpTestLogic(int number)
            : base(number)
        {
            random = new Random();
            InitArrays(10, 2);

            ParamGroup group;
            group = new ParamGroup("Group 1", 5);
            group.KPParams[0] = new Param(1, "Tag 1");
            group.KPParams[1] = new Param(2, "Tag 2");
            group.KPParams[2] = new Param(3, "Tag 3");
            group.KPParams[3] = new Param(4, "Tag 4");
            group.KPParams[4] = new Param(5, "Tag 5");
            ParamGroups[0] = group;

            group = new ParamGroup("Group 2", 5);
            group.KPParams[0] = new Param(6, "Tag 6");
            group.KPParams[1] = new Param(7, "Tag 7");
            group.KPParams[2] = new Param(8, "Tag 8");
            group.KPParams[3] = new Param(9, "Tag 9");
            group.KPParams[4] = new Param(10, "Tag 10");
            ParamGroups[1] = group;

            CopyParamsFromGroups();
        }

        public override void Session()
        {
            base.Session();

            if (SerialPort == null)
            {
                WriteToLog(KPUtils.WriteDataImpossible);
                Thread.Sleep(KPReqParams.Delay);
                lastCommSucc = false;
            }
            else
            {
                // write to the serial port
                // запись в последовательный порт
                string outStr = "test " + Address;
                SerialPort.WriteLine(outStr);
                WriteToLog("Send: " + outStr);

                // read from the serial port
                // чтение из последовательного порта
                string buf = SerialPort.ReadExisting();
                if (buf == null || buf == "")
                {
                    WriteToLog("Receive: no data");
                    lastCommSucc = false;
                }
                else
                {
                    WriteToLog("Receive: " + buf);
                }

                // finish request
                // завершение запроса
                FinishRequest();
            }

            // generate current data
            // генерация текущих значений
            for (int i = 0; i < KPParams.Length; i++)
                SetParamData(i, (double)random.Next(1000) / 10.0, 1);

            // generate archive data
            // генерация архивного среза
            /*ParamSrez srez = new ParamSrez(1);
            srez.DateTime = DateTime.Now;
            srez.KPParams[0] = KPParams[0];
            srez.Data[0] = CurData[0];
            srez.Descr = "Test archive data " + random.Next(100);
            SrezList.Add(srez);
            FlushKPArc();*/

            // generate event
            // генерация события
            /*Event ev = new Event(DateTime.Now, Number, KPParams[0]);
            ev.Descr = "Test event " + random.Next(100);
            EventList.Add(ev);
            FlushKPArc();*/

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
