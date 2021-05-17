/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Modified : 2021
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
        public static string UserDisabled { get; private set; }
        public static string WrongPassword { get; private set; }
        public static string IllegalRole { get; private set; }

        // Словарь Scada.Web.Report
        public static string GenReport { get; private set; }
        public static string IncorrectRepTemplate { get; private set; }
        public static string IncorrectStartDate { get; private set; }
        public static string IncorrectEndDate { get; private set; }
        public static string IncorrectPeriod { get; private set; }
        public static string DayPeriodTooLong { get; private set; }
        public static string MonthPeriodTooLong { get; private set; }

        // Словарь Scada.Web.UserRights
        public static string CnlNumsEmptyError { get; private set; }
        public static string CnlsViewsCountError { get; private set; }

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

        // Словарь Scada.Web.Plugins
        public static string LoadPluginConfigError { get; private set; }
        public static string SavePluginConfigError { get; private set; }

        // Словарь Scada.Web.Shell.MenuItem
        public static string ReportsMenuItem { get; private set; }
        public static string AdminMenuItem { get; private set; }
        public static string ConfigMenuItem { get; private set; }
        public static string RegMenuItem { get; private set; }
        public static string PluginsMenuItem { get; private set; }
        public static string AboutMenuItem { get; private set; }

        // Словарь Scada.Web.Shell.RememberMe
        public static string SecurityViolation { get; private set; }

        // Словарь Scada.Web.Shell.UserContent
        public static string ReportNotFound { get; private set; }
        public static string UnexpectedReportSpec { get; private set; }
        public static string DataWndNotFound { get; private set; }
        public static string UnexpectedDataWndSpec { get; private set; }

        private static void SetToDefault()
        {
            NotLoggedOn = Localization.Dict.GetEmptyPhrase("NotLoggedOn");
            IncorrectDate = Localization.Dict.GetEmptyPhrase("IncorrectDate");

            ServerUnavailable = Localization.Dict.GetEmptyPhrase("ServerUnavailable");
            UserDisabled = Localization.Dict.GetEmptyPhrase("UserDisabled");
            WrongPassword = Localization.Dict.GetEmptyPhrase("WrongPassword");
            IllegalRole = Localization.Dict.GetEmptyPhrase("IllegalRole");

            GenReport = Localization.Dict.GetEmptyPhrase("GenReport");
            IncorrectRepTemplate = Localization.Dict.GetEmptyPhrase("IncorrectRepTemplate");
            IncorrectStartDate = Localization.Dict.GetEmptyPhrase("IncorrectStartDate");
            IncorrectEndDate = Localization.Dict.GetEmptyPhrase("IncorrectEndDate");
            IncorrectPeriod = Localization.Dict.GetEmptyPhrase("IncorrectPeriod");
            DayPeriodTooLong = Localization.Dict.GetEmptyPhrase("DayPeriodTooLong");
            MonthPeriodTooLong = Localization.Dict.GetEmptyPhrase("MonthPeriodTooLong");

            CnlNumsEmptyError = Localization.Dict.GetEmptyPhrase("CnlNumsEmptyError");
            CnlsViewsCountError = Localization.Dict.GetEmptyPhrase("CnlsViewsCountError");

            LoadViewSettingsError = Localization.Dict.GetEmptyPhrase("LoadViewSettingsError");
            SaveViewSettingsError = Localization.Dict.GetEmptyPhrase("SaveViewSettingsError");
            LoadViewSettingsBaseError = Localization.Dict.GetEmptyPhrase("LoadViewSettingsBaseError");

            LoadWebSettingsError = Localization.Dict.GetEmptyPhrase("LoadWebSettingsError");
            SaveWebSettingsError = Localization.Dict.GetEmptyPhrase("SaveWebSettingsError");

            RightYes = Localization.Dict.GetEmptyPhrase("RightYes");
            RightNo = Localization.Dict.GetEmptyPhrase("RightNo");

            LoadPluginConfigError = Localization.Dict.GetEmptyPhrase("LoadPluginConfigError");
            SavePluginConfigError = Localization.Dict.GetEmptyPhrase("SavePluginConfigError");

            ReportsMenuItem = Localization.Dict.GetEmptyPhrase("ReportsMenuItem");
            AdminMenuItem = Localization.Dict.GetEmptyPhrase("AdminMenuItem");
            ConfigMenuItem = Localization.Dict.GetEmptyPhrase("ConfigMenuItem");
            RegMenuItem = Localization.Dict.GetEmptyPhrase("RegMenuItem");
            PluginsMenuItem = Localization.Dict.GetEmptyPhrase("PluginsMenuItem");
            AboutMenuItem = Localization.Dict.GetEmptyPhrase("AboutMenuItem");

            SecurityViolation = Localization.Dict.GetEmptyPhrase("SecurityViolation");

            ReportNotFound = Localization.Dict.GetEmptyPhrase("ReportNotFound");
            UnexpectedReportSpec = Localization.Dict.GetEmptyPhrase("UnexpectedReportSpec");
            DataWndNotFound = Localization.Dict.GetEmptyPhrase("DataWndNotFound");
            UnexpectedDataWndSpec = Localization.Dict.GetEmptyPhrase("UnexpectedDataWndSpec");
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
                UserDisabled = dict.GetPhrase("UserDisabled", UserDisabled);
                WrongPassword = dict.GetPhrase("WrongPassword", WrongPassword);
                IllegalRole = dict.GetPhrase("IllegalRole", IllegalRole);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Report", out dict))
            {
                GenReport = dict.GetPhrase("GenReport", GenReport);
                IncorrectRepTemplate = dict.GetPhrase("IncorrectRepTemplate", IncorrectRepTemplate);
                IncorrectStartDate = dict.GetPhrase("IncorrectStartDate", IncorrectStartDate);
                IncorrectEndDate = dict.GetPhrase("IncorrectEndDate", IncorrectEndDate);
                IncorrectPeriod = dict.GetPhrase("IncorrectPeriod", IncorrectPeriod);
                DayPeriodTooLong = dict.GetPhrase("DayPeriodTooLong", DayPeriodTooLong);
                MonthPeriodTooLong = dict.GetPhrase("MonthPeriodTooLong", MonthPeriodTooLong);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.UserRights", out dict))
            {
                CnlNumsEmptyError = dict.GetPhrase("CnlNumsEmptyError", CnlNumsEmptyError);
                CnlsViewsCountError = dict.GetPhrase("CnlsViewsCountError", CnlsViewsCountError);
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

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins", out dict))
            {
                LoadPluginConfigError = dict.GetPhrase("LoadPluginConfigError", LoadPluginConfigError);
                SavePluginConfigError = dict.GetPhrase("SavePluginConfigError", SavePluginConfigError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Shell.MenuItem", out dict))
            {
                ReportsMenuItem = dict.GetPhrase("ReportsMenuItem", ReportsMenuItem);
                AdminMenuItem = dict.GetPhrase("AdminMenuItem", AdminMenuItem);
                ConfigMenuItem = dict.GetPhrase("ConfigMenuItem", ConfigMenuItem);
                RegMenuItem = dict.GetPhrase("RegMenuItem", RegMenuItem);
                PluginsMenuItem = dict.GetPhrase("PluginsMenuItem", PluginsMenuItem);
                AboutMenuItem = dict.GetPhrase("AboutMenuItem", AboutMenuItem);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Shell.RememberMe", out dict))
            {
                SecurityViolation = dict.GetPhrase("SecurityViolation", SecurityViolation);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Shell.UserContent", out dict))
            {
                ReportNotFound = dict.GetPhrase("ReportNotFound", ReportNotFound);
                UnexpectedReportSpec = dict.GetPhrase("UnexpectedReportSpec", UnexpectedReportSpec);
                DataWndNotFound = dict.GetPhrase("DataWndNotFound", DataWndNotFound);
                UnexpectedDataWndSpec = dict.GetPhrase("UnexpectedDataWndSpec", UnexpectedDataWndSpec);
            }
        }
    }
}
