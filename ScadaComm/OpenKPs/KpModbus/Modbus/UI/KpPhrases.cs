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
 * Module   : KpModbus
 * Summary  : The phrases used by the library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2019
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm.Devices.Modbus.UI
{
    /// <summary>
    /// The phrases used by the library.
    /// <para>Фразы, используемые библиотекой.</para>
    /// </summary>
    public static class KpPhrases
    {
        // Scada.Comm.Devices.Modbus.UI.FrmDevTemplate
        public static string TemplFormTitle { get; private set; }
        public static string GrsNode { get; private set; }
        public static string CmdsNode { get; private set; }
        public static string DefGrName { get; private set; }
        public static string DefElemName { get; private set; }
        public static string DefCmdName { get; private set; }
        public static string AddressHint { get; private set; }
        public static string SaveTemplateConfirm { get; private set; }
        public static string ElemCntExceeded { get; private set; }
        public static string ElemRemoveWarning { get; private set; }
        public static string TemplateFileFilter { get; private set; }

        // Scada.Comm.Devices.Modbus.UI.FrmDevProps
        public static string TemplNotExists { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Comm.Devices.Modbus.UI.FrmDevTemplate");
            TemplFormTitle = dict.GetPhrase("this");
            GrsNode = dict.GetPhrase("GrsNode");
            CmdsNode = dict.GetPhrase("CmdsNode");
            DefGrName = dict.GetPhrase("DefGrName");
            DefElemName = dict.GetPhrase("DefElemName");
            DefCmdName = dict.GetPhrase("DefCmdName");
            AddressHint = dict.GetPhrase("AddressHint");
            SaveTemplateConfirm = dict.GetPhrase("SaveTemplateConfirm");
            ElemCntExceeded = dict.GetPhrase("ElemCntExceeded");
            ElemRemoveWarning = dict.GetPhrase("ElemRemoveWarning");
            TemplateFileFilter = dict.GetPhrase("TemplateFileFilter");

            dict = Localization.GetDictionary("Scada.Comm.Devices.Modbus.UI.FrmDevProps");
            TemplNotExists = dict.GetPhrase("TemplNotExists");
        }
    }
}
