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
 * Module   : PlgChartCommon
 * Summary  : The phrases used by chart plugins
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// The phrases used by chart plugins
    /// <para>Фразы, используемые плагинами графиков</para>
    /// </summary>
    public static class ChartPhrases
    {
        static ChartPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Web.Plugins.Chart
        public static string PerfWarning { get; private set; }
        public static string AddCnlBtn { get; private set; }
        public static string RemoveCnlBtn { get; private set; }
        public static string CnlInfoBtn { get; private set; }
        public static string ObjectHint { get; private set; }
        public static string DeviceHint { get; private set; }
        public static string ViewHint { get; private set; }

        private static void SetToDefault()
        {
            PerfWarning = Localization.Dict.GetEmptyPhrase("PerfWarning");
            AddCnlBtn = Localization.Dict.GetEmptyPhrase("AddCnlBtn");
            RemoveCnlBtn = Localization.Dict.GetEmptyPhrase("RemoveCnlBtn");
            CnlInfoBtn = Localization.Dict.GetEmptyPhrase("CnlInfoBtn");
            ObjectHint = Localization.Dict.GetEmptyPhrase("ObjectHint");
            DeviceHint = Localization.Dict.GetEmptyPhrase("DeviceHint");
            ViewHint = Localization.Dict.GetEmptyPhrase("ViewHint");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Chart", out dict))
            {
                PerfWarning = dict.GetPhrase("PerfWarning", PerfWarning);
                AddCnlBtn = dict.GetPhrase("AddCnlBtn", AddCnlBtn);
                RemoveCnlBtn = dict.GetPhrase("RemoveCnlBtn", RemoveCnlBtn);
                CnlInfoBtn = dict.GetPhrase("CnlInfoBtn", CnlInfoBtn);
                ObjectHint = dict.GetPhrase("ObjectHint", ObjectHint);
                DeviceHint = dict.GetPhrase("DeviceHint", DeviceHint);
                ViewHint = dict.GetPhrase("ViewHint", ViewHint);
            }
        }
    }
}
