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
 * Module   : PlgChart
 * Summary  : The phrases used by the minute data report
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// The phrases used by the minute data report
    /// <para>Фразы, используемые отчётом по минутным данным</para>
    /// </summary>
    public static class MinDataRepPhrases
    {
        static MinDataRepPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Web.Plugins.Chart.MinDataRepBuilder
        public static string MinDataWorksheet { get; private set; }
        public static string MinDataGen { get; private set; }
        public static string CnlsCaption { get; private set; }
        public static string TimeColTitle { get; private set; }
        public static string ValColTitle { get; private set; }

        private static void SetToDefault()
        {
            MinDataWorksheet = "MinDataWorksheet"; // GetEmptyPhrase() не удовлетворяет ограничениям Excel
            MinDataGen = Localization.Dict.GetEmptyPhrase("MinDataGen");
            CnlsCaption = Localization.Dict.GetEmptyPhrase("CnlsCaption");
            TimeColTitle = Localization.Dict.GetEmptyPhrase("TimeColTitle");
            ValColTitle = Localization.Dict.GetEmptyPhrase("ValColTitle");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Chart.MinDataRepBuilder", out dict))
            {
                MinDataWorksheet = dict.GetPhrase("MinDataWorksheet", MinDataWorksheet);
                MinDataGen = dict.GetPhrase("MinDataGen", MinDataGen);
                CnlsCaption = dict.GetPhrase("CnlsCaption", CnlsCaption);
                TimeColTitle = dict.GetPhrase("TimeColTitle", TimeColTitle);
                ValColTitle = dict.GetPhrase("ValColTitle", ValColTitle);
            }
        }
    }
}