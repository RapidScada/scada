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

        // Словарь Scada.Web
        public static string NotLoggedOn { get; private set; }
        public static string IncorrectDate { get; private set; }

        // Словарь Scada.Web.AppData
        public static string ServerUnavailable { get; private set; }
        public static string WrongPassword { get; private set; }
        public static string IllegalRole { get; private set; }

        // Словарь Scada.Web.Report
        public static string GenReport { get; private set; }
        public static string IncorrectRepTemplate { get; private set; }

        // Словарь Scada.Web.ViewSettings
        public static string LoadViewSettingsError { get; private set; }
        public static string SaveViewSettingsError { get; private set; }
        public static string LoadViewSettingsBaseError { get; private set; }

        // Словарь Scada.Web.WebSettings
        public static string LoadWebSettingsError { get; private set; }
        public static string SaveWebSettingsError { get; private set; }

        // Словарь Scada.Web.WFrmUser
        public static string RightYes { get; private set; }
        public static string RightNo { get; private set; }

        // Словарь Scada.Web.Shell.MenuItem
        public static string ReportsMenuItem { get; private set; }
        public static string AdminMenuItem { get; private set; }
        public static string ConfigMenuItem { get; private set; }
        public static string AboutMenuItem { get; private set; }

        // Словарь Scada.Web.Shell.RememberMe
        public static string SecurityViolation { get; private set; }

        private static void SetToDefault()
        {
            NotLoggedOn = Localization.Dict.GetEmptyPhrase("NotLoggedOn");
            IncorrectDate = Localization.Dict.GetEmptyPhrase("IncorrectDate");

            ServerUnavailable = Localization.Dict.GetEmptyPhrase("ServerUnavailable");
            WrongPassword = Localization.Dict.GetEmptyPhrase("WrongPassword");
            IllegalRole = Localization.Dict.GetEmptyPhrase("IllegalRole");

            GenReport = Localization.Dict.GetEmptyPhrase("GenReport");
            IncorrectRepTemplate = Localization.Dict.GetEmptyPhrase("IncorrectRepTemplate");

            LoadViewSettingsError = Localization.Dict.GetEmptyPhrase("LoadViewSettingsError");
            SaveViewSettingsError = Localization.Dict.GetEmptyPhrase("SaveViewSettingsError");
            LoadViewSettingsBaseError = Localization.Dict.GetEmptyPhrase("LoadViewSettingsBaseError");

            LoadWebSettingsError = Localization.Dict.GetEmptyPhrase("LoadWebSettingsError");
            SaveWebSettingsError = Localization.Dict.GetEmptyPhrase("SaveWebSettingsError");

            RightYes = Localization.Dict.GetEmptyPhrase("RightYes");
            RightNo = Localization.Dict.GetEmptyPhrase("RightNo");

            ReportsMenuItem = Localization.Dict.GetEmptyPhrase("ReportsMenuItem");
            AdminMenuItem = Localization.Dict.GetEmptyPhrase("AdminMenuItem");
            ConfigMenuItem = Localization.Dict.GetEmptyPhrase("ConfigMenuItem");
            AboutMenuItem = Localization.Dict.GetEmptyPhrase("AboutMenuItem");

            SecurityViolation = Localization.Dict.GetEmptyPhrase("SecurityViolation");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Web", out dict))
            {
                NotLoggedOn = dict.GetPhrase("NotLoggedOn", NotLoggedOn);
                IncorrectDate = dict.GetPhrase("IncorrectDate", IncorrectDate);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.AppData", out dict))
            {
                ServerUnavailable = dict.GetPhrase("ServerUnavailable", ServerUnavailable);
                WrongPassword = dict.GetPhrase("WrongPassword", WrongPassword);
                IllegalRole = dict.GetPhrase("IllegalRole", IllegalRole);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Report", out dict))
            {
                GenReport = dict.GetPhrase("GenReport", GenReport);
                IncorrectRepTemplate = dict.GetPhrase("IncorrectRepTemplate", IncorrectRepTemplate);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.ViewSettings", out dict))
            {
                LoadWebSettingsError = dict.GetPhrase("LoadViewSettingsError", LoadViewSettingsError);
                SaveWebSettingsError = dict.GetPhrase("SaveViewSettingsError", SaveViewSettingsError);
                LoadViewSettingsBaseError = dict.GetPhrase("LoadViewSettingsBaseError", LoadViewSettingsBaseError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WebSettings", out dict))
            {
                LoadWebSettingsError = dict.GetPhrase("LoadWebSettingsError", LoadWebSettingsError);
                SaveWebSettingsError = dict.GetPhrase("SaveWebSettingsError", SaveWebSettingsError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WFrmUser", out dict))
            {
                RightYes = dict.GetPhrase("RightYes", RightYes);
                RightNo = dict.GetPhrase("RightNo", RightNo);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Shell.MenuItem", out dict))
            {
                ReportsMenuItem = dict.GetPhrase("ReportsMenuItem", ReportsMenuItem);
                AdminMenuItem = dict.GetPhrase("AdminMenuItem", AdminMenuItem);
                ConfigMenuItem = dict.GetPhrase("ConfigMenuItem", ConfigMenuItem);
                AboutMenuItem = dict.GetPhrase("AboutMenuItem", AboutMenuItem);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Shell.RememberMe", out dict))
            {
                SecurityViolation = dict.GetPhrase("SecurityViolation", SecurityViolation);
            }
        }
    }
}
