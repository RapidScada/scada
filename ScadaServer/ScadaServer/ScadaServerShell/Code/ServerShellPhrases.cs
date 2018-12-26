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
 * Module   : Server Shell
 * Summary  : The phrases used in the Server shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Server.Shell.Code
{
    /// <summary>
    /// The phrases used in the Server shell.
    /// <para>Фразы, используемые оболочкой Сервера.</para>
    /// </summary>
    public class ServerShellPhrases
    {
        // Scada.Server.Shell.Code.ServerShell
        public static string CommonParamsNode { get; private set; }
        public static string SaveParamsNode { get; private set; }
        public static string ArchiveNode { get; private set; }
        public static string CurDataNode { get; private set; }
        public static string MinDataNode { get; private set; }
        public static string HourDataNode { get; private set; }
        public static string EventsNode { get; private set; }
        public static string ModulesNode { get; private set; }
        public static string GeneratorNode { get; private set; }
        public static string StatsNode { get; private set; }

        // Scada.Server.Shell.Forms
        public static string ConnectionUndefined { get; private set; }
        public static string Loading { get; private set; }

        // Scada.Server.Shell.Forms.FrmCommonParams
        public static string ChooseItfDir { get; private set; }
        public static string ChooseArcDir { get; private set; }
        public static string ChooseArcCopyDir { get; private set; }

        // Scada.Server.Shell.Forms.FrmModules
        public static string ModuleNotFound { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Server.Shell.Code.ServerShell");
            CommonParamsNode = dict.GetPhrase("CommonParamsNode");
            SaveParamsNode = dict.GetPhrase("SaveParamsNode");
            ArchiveNode = dict.GetPhrase("ArchiveNode");
            CurDataNode = dict.GetPhrase("CurDataNode");
            MinDataNode = dict.GetPhrase("MinDataNode");
            HourDataNode = dict.GetPhrase("HourDataNode");
            EventsNode = dict.GetPhrase("EventsNode");
            ModulesNode = dict.GetPhrase("ModulesNode");
            GeneratorNode = dict.GetPhrase("GeneratorNode");
            StatsNode = dict.GetPhrase("StatsNode");

            dict = Localization.GetDictionary("Scada.Server.Shell.Forms");
            ConnectionUndefined = dict.GetPhrase("ConnectionUndefined");
            Loading = dict.GetPhrase("Loading");

            dict = Localization.GetDictionary("Scada.Server.Shell.Forms.FrmCommonParams");
            ChooseItfDir = dict.GetPhrase("ChooseItfDir");
            ChooseArcDir = dict.GetPhrase("ChooseArcDir");
            ChooseArcCopyDir = dict.GetPhrase("ChooseArcCopyDir");

            dict = Localization.GetDictionary("Scada.Server.Shell.Forms.FrmModules");
            ModuleNotFound = dict.GetPhrase("ModuleNotFound");
        }
    }
}
