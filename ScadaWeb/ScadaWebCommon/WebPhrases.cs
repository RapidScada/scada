/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Summary  : The phrases used in SCADA-Web
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2014
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Web
{
    /// <summary>
    /// The phrases used in SCADA-Web
    /// <para>Фразы, используемые SCADA-Web</para>
    /// </summary>
    public static class WebPhrases
    {
        static WebPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Report
        public static string GenReport { get; private set; }
        public static string GenReportError { get; private set; }

        // Словарь Scada.Web
        public static string NotLoggedOn { get; private set; }
        public static string UnableLoadView { get; private set; }
        public static string IncorrectDate { get; private set; }
        public static string ErrorFormat { get; private set; }
        public static string ServerUnavailable { get; private set; }

        // Словарь Scada.Web.MainData
        public static string EventChecked { get; private set; }
        public static string EventUnchecked { get; private set; }
        public static string CommSettingsNotLoaded { get; private set; }
        public static string WrongPassword { get; private set; }
        public static string NoRightsL { get; private set; }
        public static string IllegalRole { get; private set; }

        // Словарь Scada.Web.RepHrEvTable
        public static string NoReportData { get; private set; }
        public static string HourDataPage { get; private set; }
        public static string EventsPage { get; private set; }
        public static string HourDataTitle { get; private set; }
        public static string AllEventsTitle { get; private set; }
        public static string EventsByViewTitle { get; private set; }

        // Словарь Scada.Web.TableView
        public static string LoadTableViewError { get; private set; }
        public static string SaveTableViewError { get; private set; }

        // Словарь Scada.Web.ViewSettings
        public static string LoadViewSettingsError { get; private set; }
        public static string SaveViewSettingsError { get; private set; }

        // Словарь Scada.Web.WebSettings
        public static string LoadWebSettingsError { get; private set; }
        public static string SaveWebSettingsError { get; private set; }
        
        // Словарь Scada.Web.WFrmCmdSend
        public static string OutCnlNotFound { get; private set; }
        public static string CmdNotSelected { get; private set; }
        public static string IncorrectCmdVal { get; private set; }
        public static string IncorrectCmdData { get; private set; }
        public static string EmptyCmdData { get; private set; }

        // Словарь Scada.Web.WFrmDiag
        public static string DiagCnlsUndefined { get; private set; }
        public static string DiagCnlNotFound { get; private set; }
        public static string CnlCntExceeded { get; private set; }
        public static string CnlNotSelected { get; private set; }
        public static string DiagPageClosed { get; private set; }

        // Словарь Scada.Web.WFrmDiagCnls
        public static string DiagSelColumn { get; private set; }
        public static string DiagCnlColumn { get; private set; }
        public static string DiagObjColumn { get; private set; }
        public static string DiagKPColumn { get; private set; }

        // Словарь Scada.Web.WFrmEvCheck
        public static string IncorrectEvDate { get; private set; }
        public static string IncorrectEvNum { get; private set; }
        public static string EventNotFound { get; private set; }

        // Словарь Scada.Web.WFrmEvTable
        public static string NumColumn { get; private set; }
        public static string DateColumn { get; private set; }
        public static string TimeColumn { get; private set; }
        public static string ObjColumn { get; private set; }
        public static string KPColumn { get; private set; }
        public static string CnlColumn { get; private set; }
        public static string EventColumn { get; private set; }
        public static string CheckColumn { get; private set; }

        // Словарь Scada.Web.WFrmLogin
        public static string UnableLogin { get; private set; }
        public static string NoViewSetRights { get; private set; }

        // Словарь Scada.Web.WFrmTableView
        public static string ItemColumn { get; private set; }
        public static string CurColumn { get; private set; }
        public static string InCnlNumHint { get; private set; }
        public static string OutCnlNumHint { get; private set; }
        public static string ObjectHint { get; private set; }
        public static string KPHint { get; private set; }
        public static string ParamHint { get; private set; }
        public static string UnitHint { get; private set; }

        private static void SetToDefault()
        {
            GenReport = "Генерация отчёта \"{0}\" пользователем {1}";
            GenReportError = "Ошибка при генерации отчёта \"{0}\": {1}";

            NotLoggedOn = "Пользователь не вошёл в систему.";
            UnableLoadView = "Не удалось загрузить представление.";
            IncorrectDate = "Некорректная дата запрашиваемых данных.";
            ErrorFormat = "Ошибка: {0}";
            ServerUnavailable = "нет связи с сервером";

            EventChecked = "Да";
            EventUnchecked = "Нет";
            CommSettingsNotLoaded = "настройки соединения с сервером не загружены";
            WrongPassword = "неверное имя пользователя или пароль";
            NoRightsL = "недостаточно прав";
            IllegalRole = "недопустимая роль пользователя";

            NoReportData = "Нет данных для генерации отчёта.";
            HourDataPage = "Часовые данные";
            EventsPage = "События";
            HourDataTitle = "{0} - Часовые данные по представлению \"{1}\". Получено {2}";
            AllEventsTitle = "{0} - Все события. Получено {1}";
            EventsByViewTitle = "{0} - События по представлению \"{1}\". Получено {2}";

            LoadTableViewError = "Ошибка при загрузке табличного представления из файла";
            SaveTableViewError = "Ошибка при сохранении табличного представления в файле";

            LoadViewSettingsError = "Ошибка при загрузке настроек представлений из файла";
            SaveViewSettingsError = "Ошибка при сохранении настроек представлений в файле";

            LoadWebSettingsError = "Ошибка при загрузке настроек веб-приложения из файла";
            SaveWebSettingsError = "Ошибка при сохранении настроек веб-приложения в файле";

            OutCnlNotFound = "Канал управления {0} не найден.";
            CmdNotSelected = "команда не выбрана";
            IncorrectCmdVal = "некорректное значение команды";
            IncorrectCmdData = "некорректная запись 16-ричных данных команды";
            EmptyCmdData = "не введены данные команды";

            DiagCnlsUndefined = "Не заданы номера каналов графиков.";
            DiagCnlNotFound = "Входной канал {0} не найден в базе конфигурации.";
            CnlCntExceeded = "Возможно выбрать не более {0} каналов.";
            CnlNotSelected = "Каналы не выбраны.";
            DiagPageClosed = "Страница графика была закрыта.";

            DiagSelColumn = "Выбрать";
            DiagCnlColumn = "Вх. канал";
            DiagObjColumn = "Объект";
            DiagKPColumn = "КП";

            IncorrectEvDate = "Некорректная дата события.";
            IncorrectEvNum = "Некорректный номер события.";
            EventNotFound = "событие не найдено";

            NumColumn = "№";
            DateColumn = "Дата";
            TimeColumn = "Время";
            ObjColumn = "Объект";
            KPColumn = "КП";
            CnlColumn = "Канал";
            EventColumn = "Событие";
            CheckColumn = "Квит.";

            UnableLogin = "Вход в систему невозможен: {0}.";
            NoViewSetRights = "отсутствуют права на выбранный набор представлений";

            ItemColumn = "Элемент";
            CurColumn = "ТЕК";
            InCnlNumHint = "№ вх. канала: ";
            OutCnlNumHint = "№ канала упр.: ";
            ObjectHint = "Объект: ";
            KPHint = "КП: ";
            ParamHint = "Величина: ";
            UnitHint = "Размерность: ";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Report", out dict))
            {
                GenReport = dict.GetPhrase("GenReport", GenReport);
                GenReportError = dict.GetPhrase("GenReportError", GenReportError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web", out dict))
            {
                NotLoggedOn = dict.GetPhrase("NotLoggedOn", NotLoggedOn);
                UnableLoadView = dict.GetPhrase("UnableLoadView", UnableLoadView);
                IncorrectDate = dict.GetPhrase("IncorrectDate", IncorrectDate);
                ErrorFormat = dict.GetPhrase("ErrorFormat", ErrorFormat);
                ServerUnavailable = dict.GetPhrase("ServerUnavailable", ServerUnavailable);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.MainData", out dict))
            {
                EventChecked = dict.GetPhrase("EventChecked", EventChecked);
                EventUnchecked = dict.GetPhrase("EventUnchecked", EventUnchecked);
                CommSettingsNotLoaded = dict.GetPhrase("CommSettingsNotLoaded", CommSettingsNotLoaded);
                WrongPassword = dict.GetPhrase("WrongPassword", WrongPassword);
                NoRightsL = dict.GetPhrase("NoRightsL", NoRightsL);
                IllegalRole = dict.GetPhrase("IllegalRole", IllegalRole);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.RepHrEvTable", out dict))
            {
                NoReportData = dict.GetPhrase("NoReportData", NoReportData);
                HourDataPage = dict.GetPhrase("HourDataPage", HourDataPage);
                EventsPage = dict.GetPhrase("EventsPage", EventsPage);
                HourDataTitle = dict.GetPhrase("HourDataTitle", HourDataTitle);
                AllEventsTitle = dict.GetPhrase("AllEventsTitle", AllEventsTitle);
                EventsByViewTitle = dict.GetPhrase("EventsByViewTitle", EventsByViewTitle);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.TableView", out dict))
            {
                LoadTableViewError = dict.GetPhrase("LoadTableViewError", LoadTableViewError);
                SaveTableViewError = dict.GetPhrase("SaveTableViewError", SaveTableViewError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.ViewSettings", out dict))
            {
                LoadViewSettingsError = dict.GetPhrase("LoadViewSettingsError", LoadViewSettingsError);
                SaveViewSettingsError = dict.GetPhrase("SaveViewSettingsError", SaveViewSettingsError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WebSettings", out dict))
            {
                LoadWebSettingsError = dict.GetPhrase("LoadWebSettingsError", LoadWebSettingsError);
                SaveWebSettingsError = dict.GetPhrase("SaveWebSettingsError", SaveWebSettingsError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WFrmCmdSend", out dict))
            {
                OutCnlNotFound = dict.GetPhrase("OutCnlNotFound", OutCnlNotFound);
                CmdNotSelected = dict.GetPhrase("CmdNotSelected", CmdNotSelected);
                IncorrectCmdVal = dict.GetPhrase("IncorrectCmdVal", IncorrectCmdVal);
                IncorrectCmdData = dict.GetPhrase("IncorrectCmdData", IncorrectCmdData);
                EmptyCmdData = dict.GetPhrase("EmptyCmdData", EmptyCmdData);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WFrmDiag", out dict))
            {
                DiagCnlsUndefined = dict.GetPhrase("DiagCnlsUndefined", DiagCnlsUndefined);
                DiagCnlNotFound = dict.GetPhrase("DiagCnlNotFound", DiagCnlNotFound);
                CnlCntExceeded = dict.GetPhrase("CnlCntExceeded", CnlCntExceeded);
                CnlNotSelected = dict.GetPhrase("CnlNotSelected", CnlNotSelected);
                DiagPageClosed = dict.GetPhrase("DiagPageClosed", DiagPageClosed);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WFrmDiagCnls", out dict))
            {
                DiagSelColumn = dict.GetPhrase("DiagSelColumn", DiagSelColumn);
                DiagCnlColumn = dict.GetPhrase("DiagCnlColumn", DiagCnlColumn);
                DiagObjColumn = dict.GetPhrase("DiagObjColumn", DiagObjColumn);
                DiagKPColumn = dict.GetPhrase("DiagKPColumn", DiagKPColumn);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WFrmEvCheck", out dict))
            {
                IncorrectEvDate = dict.GetPhrase("IncorrectEvDate", IncorrectEvDate);
                IncorrectEvNum = dict.GetPhrase("IncorrectEvNum", IncorrectEvNum);
                EventNotFound = dict.GetPhrase("EventNotFound", EventNotFound);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WFrmEvTable", out dict))
            {
                NumColumn = dict.GetPhrase("lblNumColumn", NumColumn);
                DateColumn = dict.GetPhrase("lblDateColumn", DateColumn);
                TimeColumn = dict.GetPhrase("lblTimeColumn", TimeColumn);
                ObjColumn = dict.GetPhrase("lblObjColumn", ObjColumn);
                KPColumn = dict.GetPhrase("lblKPColumn", KPColumn);
                CnlColumn = dict.GetPhrase("lblCnlColumn", CnlColumn);
                EventColumn = dict.GetPhrase("lblEventColumn", EventColumn);
                CheckColumn = dict.GetPhrase("lblCheckColumn", CheckColumn);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WFrmLogin", out dict))
            {
                UnableLogin = dict.GetPhrase("UnableLogin", UnableLogin);
                NoViewSetRights = dict.GetPhrase("NoViewSetRights", NoViewSetRights);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.WFrmTableView", out dict))
            {
                ItemColumn = dict.GetPhrase("ItemColumn", ItemColumn);
                CurColumn = dict.GetPhrase("CurColumn", CurColumn);
                InCnlNumHint = dict.GetPhrase("InCnlNumHint", InCnlNumHint);
                OutCnlNumHint = dict.GetPhrase("OutCnlNumHint", OutCnlNumHint);
                ObjectHint = dict.GetPhrase("ObjectHint", ObjectHint);
                KPHint = dict.GetPhrase("KPHint", KPHint);
                ParamHint = dict.GetPhrase("ParamHint", ParamHint);
                UnitHint = dict.GetPhrase("UnitHint", UnitHint);
            }
        }
    }
}
