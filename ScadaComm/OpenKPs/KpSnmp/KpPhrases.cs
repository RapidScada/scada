/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : KpSnmp
 * Summary  : The phrases used by the device library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

namespace Scada.Comm.Devices.KpSnmp
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
            InitOnLocalization();
        }

        // Словарь Scada.Comm.Devices.KpSnmp.FrmConfig
        public static string Aaa { get; private set; }

        // Фразы, устанавливаемые в зависимости от локализации, не загружая из словаря
        public static string NoVariables { get; private set; }
        public static string VariablesMismatch { get; private set; }


        private static void SetToDefault()
        {
            Aaa = "Ааа";
        }

        private static void InitOnLocalization()
        {
            if (Localization.UseRussian)
            {
                NoVariables = "Отсутствуют переменные для запроса";
                VariablesMismatch = "Несоответствие запрошенных и принятых переменных";
            }
            else
            {
                NoVariables = "No variables for request";
                VariablesMismatch = "Mismatch of the requested and received variables";
            }
        }

        public static void InitFromDictionaries()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Comm.Devices.KpSnmp.FrmConfig", out dict))
            {
                Aaa = dict.GetPhrase("Aaa", Aaa);
            }
        }
    }
}
