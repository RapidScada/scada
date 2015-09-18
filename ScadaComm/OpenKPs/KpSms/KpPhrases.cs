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
 * Summary  : The phrases used by the device library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

namespace Scada.Comm.Devices.KpSms
{
    /// <summary>
    /// The phrases used by the device library
    /// <para>Фразы, используемые библиотекой КП</para>
    /// </summary>
    internal static class KpPhrases
    {
        static KpPhrases()
        {
            SetToDefault();
        }

        public static string PhonebookNode { get; private set; }
        public static string PhoneGroupExists { get; private set; }
        public static string UpdatePhoneNumberConfirm { get; private set; }
        public static string SavePhonebookConfirm { get; private set; }

        private static void SetToDefault()
        {
            PhonebookNode = "Справочник";
            PhoneGroupExists = "Группа уже существует.";
            UpdatePhoneNumberConfirm = "Номер уже существует в группе. Обновить?";
            SavePhonebookConfirm = "Справочник был изменён. Сохранить изменения?";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Comm.Devices.KpSms.FrmPhonebook", out dict))
            {
                PhonebookNode = dict.GetPhrase("PhonebookNode", PhonebookNode);
                PhoneGroupExists = dict.GetPhrase("PhoneGroupExists", PhoneGroupExists);
                UpdatePhoneNumberConfirm = dict.GetPhrase("UpdatePhoneNumberConfirm", UpdatePhoneNumberConfirm);
                SavePhonebookConfirm = dict.GetPhrase("SavePhonebookConfirm", SavePhonebookConfirm);
            }
        }
    }
}
