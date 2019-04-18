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
 * Module   : KpSimulator
 * Summary  : Device driver communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver communication logic.
    /// <para>Логика работы драйвера КП.</para>
    /// </summary>
    public class KpSimulatorLogic : KPLogic
    {
        /// <summary>
        /// The period of sine waves in minutes.
        /// </summary>
        private const int SinePeriod = 60;
        /// <summary>
        /// The period of square waves in minutes.
        /// </summary>
        private const int SquarePeriod = 15;
        /// <summary>
        /// The period of triangular waves in minutes.
        /// </summary>
        private const int TrianglePeriod = 30;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public KpSimulatorLogic(int number)
            : base(number)
        {
            CanSendCmd = true;
            ConnRequired = false;
            InitDeviceTags();
        }


        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        private void InitDeviceTags()
        {
            List<TagGroup> tagGroups = new List<TagGroup>();

            TagGroup tagGroup = new TagGroup("Inputs");
            tagGroup.KPTags.Add(new KPTag(1, "Sine"));
            tagGroup.KPTags.Add(new KPTag(2, "Square"));
            tagGroup.KPTags.Add(new KPTag(3, "Triangle"));
            tagGroups.Add(tagGroup);

            tagGroup = new TagGroup("Outputs");
            tagGroup.KPTags.Add(new KPTag(4, "Relay State"));
            tagGroup.KPTags.Add(new KPTag(5, "Analog Output"));
            tagGroups.Add(tagGroup);

            InitKPTags(tagGroups);
        }

        /// <summary>
        /// Simulates reading input values.
        /// </summary>
        private void SimulateInputs()
        {
            double x = DateTime.Now.TimeOfDay.TotalMinutes;
            double y1 = Math.Sin(2 * Math.PI * x / SinePeriod);
            double y2 = Frac(x / SquarePeriod) <= 0.5 ? 1 : 0;
            double y3 = Frac(x / TrianglePeriod) <= 0.5 ? x % TrianglePeriod : TrianglePeriod - x % TrianglePeriod;

            WriteToLog(KPTags[0].Name + " = " + y1);
            WriteToLog(KPTags[1].Name + " = " + y2);
            WriteToLog(KPTags[2].Name + " = " + y3);

            SetCurData(0, y1, BaseValues.CnlStatuses.Defined);
            SetCurData(1, y2, BaseValues.CnlStatuses.Defined);
            SetCurData(2, y3, BaseValues.CnlStatuses.Defined);
        }

        /// <summary>
        /// Gets the fractional part of double.
        /// </summary>
        private double Frac(double d)
        {
            return d - Math.Truncate(d);
        }

        /// <summary>
        /// Converts the tag data to string.
        /// </summary>
        protected override string ConvertTagDataToStr(KPTag kpTag, SrezTableLight.CnlData tagData)
        {
            return tagData.Stat > 0 && (kpTag.Signal == 2 || kpTag.Signal == 4) ?
                tagData.Val > 0 ? CommPhrases.On : CommPhrases.Off :
                base.ConvertTagDataToStr(kpTag, tagData);
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();
            SimulateInputs();
            Thread.Sleep(ReqParams.Delay);
            CalcSessStats();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCmd(Command cmd)
        {
            base.SendCmd(cmd);
            int cmdNum = cmd.CmdNum;

            if ((cmdNum == 4 || cmdNum == 5) && cmd.CmdTypeID == BaseValues.CmdTypes.Standard)
            {
                if (cmdNum == 4)
                {
                    // set relay state
                    double relayVal = cmd.CmdVal > 0 ? 1 : 0;
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Установить состояние реле в {0}" :
                        "Set the relay state to {0}", relayVal));
                    SetCurData(3, relayVal, BaseValues.CnlStatuses.Defined);
                }
                else
                {
                    // set analog output
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Установить аналоговый выход в {0}" :
                        "Set the analog output to {0}", cmd.CmdVal));
                    SetCurData(4, cmd.CmdVal, BaseValues.CnlStatuses.Defined);
                }
            }
            else
            {
                lastCommSucc = false;
                WriteToLog(CommPhrases.IllegalCommand);
            }

            CalcCmdStats();
        }
    }
}
