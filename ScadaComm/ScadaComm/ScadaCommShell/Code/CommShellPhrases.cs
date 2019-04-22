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
 * Module   : Communicator Shell
 * Summary  : The phrases used in the Communicator shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm.Shell.Code
{
    /// <summary>
    /// The phrases used in the Communicator shell.
    /// <para>Фразы, используемые оболочкой Коммуникатора.</para>
    /// </summary>
    public class CommShellPhrases
    {
        // Scada.Comm.Shell.Code.CommShell
        public static string CommonParamsNode { get; private set; }
        public static string DriversNode { get; private set; }
        public static string CommLinesNode { get; private set; }
        public static string CommLineNode { get; private set; }
        public static string LineParamsNode { get; private set; }
        public static string LineStatsNode { get; private set; }
        public static string DeviceNode { get; private set; }
        public static string StatsNode { get; private set; }

        // Scada.Comm.Shell.Forms
        public static string SetProfile { get; private set; }
        public static string Loading { get; private set; }
        public static string NoDeviceProps { get; private set; }

        // Scada.Comm.Shell.Forms.FrmLineParams
        public static string LineParamsTitle { get; private set; }

        // Scada.Comm.Shell.Forms.FrmLineStats
        public static string LineStatsTitle { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Comm.Shell.Code.CommShell");
            CommonParamsNode = dict.GetPhrase("CommonParamsNode");
            DriversNode = dict.GetPhrase("DriversNode");
            CommLinesNode = dict.GetPhrase("CommLinesNode");
            CommLineNode = dict.GetPhrase("CommLineNode");
            LineParamsNode = dict.GetPhrase("LineParamsNode");
            LineStatsNode = dict.GetPhrase("LineStatsNode");
            DeviceNode = dict.GetPhrase("DeviceNode");
            StatsNode = dict.GetPhrase("StatsNode");

            dict = Localization.GetDictionary("Scada.Comm.Shell.Forms");
            SetProfile = dict.GetPhrase("SetProfile");
            Loading = dict.GetPhrase("Loading");
            NoDeviceProps = dict.GetPhrase("NoDeviceProps");

            dict = Localization.GetDictionary("Scada.Comm.Shell.Forms.FrmLineParams");
            LineParamsTitle = dict.GetPhrase("LineParamsTitle");

            dict = Localization.GetDictionary("Scada.Comm.Shell.Forms.FrmLineStats");
            LineStatsTitle = dict.GetPhrase("LineStatsTitle");
        }
    }
}
