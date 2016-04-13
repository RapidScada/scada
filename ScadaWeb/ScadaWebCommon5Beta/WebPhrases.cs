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
 * Module   : ScadaWebCommon
 * Summary  : The phrases used by the web application and its plugins
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Web
{
    /// <summary>
    /// The phrases used by the web application and its plugins
    /// <para>Фразы, используемые веб-приложением и его плагинами</para>
    /// </summary>
    public static class WebPhrases
    {
        static WebPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Web.AppData
        public static string ServerUnavailable { get; private set; }
        public static string WrongPassword { get; private set; }
        public static string NoRights { get; private set; }
        public static string IllegalRole { get; private set; }

        // Словарь Scada.Web.Plugins
        public static string ViewsMenuItem { get; private set; }
        public static string ReportsMenuItem { get; private set; }
        public static string AdminMenuItem { get; private set; }
        public static string ConfigMenuItem { get; private set; }
        public static string AboutMenuItem { get; private set; }

        private static void SetToDefault()
        {
            ServerUnavailable = Localization.Dict.GetEmptyPhrase("ServerUnavailable");
            WrongPassword = Localization.Dict.GetEmptyPhrase("WrongPassword");
            NoRights = Localization.Dict.GetEmptyPhrase("NoRights");
            IllegalRole = Localization.Dict.GetEmptyPhrase("IllegalRole");

            ViewsMenuItem = Localization.Dict.GetEmptyPhrase("ViewsMenuItem");
            ReportsMenuItem = Localization.Dict.GetEmptyPhrase("ReportsMenuItem");
            AdminMenuItem = Localization.Dict.GetEmptyPhrase("AdminMenuItem");
            ConfigMenuItem = Localization.Dict.GetEmptyPhrase("ConfigMenuItem");
            AboutMenuItem = Localization.Dict.GetEmptyPhrase("AboutMenuItem");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Web.AppData", out dict))
            {
                ServerUnavailable = dict.GetPhrase("ServerUnavailable", ServerUnavailable);
                WrongPassword = dict.GetPhrase("WrongPassword", WrongPassword);
                NoRights = dict.GetPhrase("NoRightsL", NoRights);
                IllegalRole = dict.GetPhrase("IllegalRole", IllegalRole);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins", out dict))
            {
                ViewsMenuItem = dict.GetPhrase("ViewsMenuItem", ViewsMenuItem);
                ReportsMenuItem = dict.GetPhrase("ReportsMenuItem", ReportsMenuItem);
                AdminMenuItem = dict.GetPhrase("AdminMenuItem", AdminMenuItem);
                ConfigMenuItem = dict.GetPhrase("ConfigMenuItem", ConfigMenuItem);
                AboutMenuItem = dict.GetPhrase("AboutMenuItem", AboutMenuItem);
            }
        }
    }
}
