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
 * Module   : SCADA-Communicator Control
 * Summary  : The phrases used in the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2016
 */

namespace Scada.Comm.Ctrl
{
    /// <summary>
    /// The phrases used in the application
    /// <para>Фразы, используемые приложением</para>
    /// </summary>
    internal static class AppPhrases
    {
        static AppPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Comm.Ctrl.FrmImport
        public static string ImportAlternateTitle { get; private set; }
        public static string NoImportData { get; private set; }
        public static string PrepareImportFormError1 { get; private set; }
        public static string PrepareImportFormError2 { get; private set; }

        // Словарь Scada.Comm.Ctrl.FrmMain
        public static string MainFormTitle { get; private set; }
        public static string CommonParamsNode { get; private set; }
        public static string KpDllsNode { get; private set; }
        public static string LinesNode { get; private set; }
        public static string LineStatsNode { get; private set; }
        public static string StatsNode { get; private set; }
        public static string ServiceState { get; private set; }
        public static string RestartNeeded { get; private set; }
        public static string Yes { get; private set; }
        public static string No { get; private set; }
        public static string ReceiveBaseTableError { get; private set; }
        public static string UpdateSettingsCompleted { get; private set; }
        public static string UpdateSettingsError { get; private set; }
        public static string GetKpTypeInfoError { get; private set; }
        public static string SecondInstanceClosed { get; private set; }
        public static string CheckSecondInstanceError { get; private set; }
        public static string ServiceNotInstalled { get; private set; }
        public static string SaveSettingsConfirm { get; private set; }
        public static string ServiceStartFailed { get; private set; }
        public static string ServiceStopFailed { get; private set; }
        public static string ServiceRestartFailed { get; private set; }
        public static string ImportLinesAndKpError { get; private set; }
        public static string ImportKpError { get; private set; }
        public static string ResetReqParamsError { get; private set; }
        public static string ShowKpPropsUnsupported { get; private set; }
        public static string ShowKpPropsError { get; private set; }
        public static string ShowCommCnlPropsError { get; private set; }
        public static string UnknownDLL { get; private set; }
        public static string IncorrectCmdVal { get; private set; }
        public static string IncorrectHexCmdData { get; private set; }
        public static string CmdDataRequired { get; private set; }

        private static void SetToDefault()
        {
            ImportAlternateTitle = "Импорт КП - Линия {0}";
            NoImportData = "Отсутствуют данные для импорта.";
            PrepareImportFormError1 = "Ошибка при подготовке формы импорта линий связи и КП";
            PrepareImportFormError2 = "Ошибка при подготовке формы импорта КП";

            MainFormTitle = "SCADA-Коммуникатор";
            CommonParamsNode = "Общие параметры";
            KpDllsNode = "Библиотеки КП";
            LinesNode = "Линии связи";
            LineStatsNode = "Статистика линии связи";
            StatsNode = "Статистика";
            ServiceState = "Состояние службы {0}: {1}";
            RestartNeeded = "Изменения настроек вступят в силу после перезапуска службы SCADA-Коммуникатора.";
            Yes = "Да";
            No = "Нет";
            ReceiveBaseTableError = "Ошибка при приёме таблиц базы конфигурации";
            UpdateSettingsCompleted = "Обновление настроек по базе конфигурации выполнено успешно.";
            UpdateSettingsError = "Ошибка при обновлении настроек по базе конфигурации";
            GetKpTypeInfoError = "Ошибка при получении информации о типах КП";
            SecondInstanceClosed = "Программа для управления SCADA-Коммуникатором уже запущена.\r\nВторая копия будет закрыта.";
            CheckSecondInstanceError = "Ошибка при проверке запуска второй копии программы";
            ServiceNotInstalled = "Служба SCADA-Коммуникатора не установлена.";
            SaveSettingsConfirm = "Настройки SCADA-Коммуникатора были изменены.\r\nСохранить изменения?";
            ServiceStartFailed = "Не удалось запустить службу SCADA-Коммуникатора";
            ServiceStopFailed = "Не удалось остановить службу SCADA-Коммуникатора";
            ServiceRestartFailed = "Не удалось перезапусить службу SCADA-Коммуникатора";
            ImportLinesAndKpError = "Ошибка при импорте линий связи и КП";
            ImportKpError = "Ошибка при импорте КП";
            ResetReqParamsError = "Ошибка при установке параметров опроса КП по умолчанию";
            ShowKpPropsUnsupported = "{0} не поддерживает отображение свойств КП.";
            ShowKpPropsError = "Ошибка при отображении свойств КП";
            ShowCommCnlPropsError = "Ошибка при отображении свойств канала связи";
            UnknownDLL = "Неизвестная DLL.";
            IncorrectCmdVal = "Некорректное значение команды.";
            IncorrectHexCmdData = "Некорректные 16-ричные данные команды.";
            CmdDataRequired = "Необходимо ввести данные команды.";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Comm.Ctrl.FrmImport", out dict))
            {
                ImportAlternateTitle = dict.GetPhrase("ImportAlternateTitle", ImportAlternateTitle);
                NoImportData = dict.GetPhrase("NoImportData", NoImportData);
                PrepareImportFormError1 = dict.GetPhrase("PrepareImportFormError1", PrepareImportFormError1);
                PrepareImportFormError2 = dict.GetPhrase("PrepareImportFormError2", PrepareImportFormError2);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Comm.Ctrl.FrmMain", out dict))
            {
                MainFormTitle = dict.GetPhrase("this", MainFormTitle);
                CommonParamsNode = dict.GetPhrase("CommonParamsNode", CommonParamsNode);
                KpDllsNode = dict.GetPhrase("KpDllsNode", KpDllsNode);
                LinesNode = dict.GetPhrase("LinesNode", LinesNode);
                LineStatsNode = dict.GetPhrase("LineStatsNode", LineStatsNode);
                StatsNode = dict.GetPhrase("StatsNode", StatsNode);
                ServiceState = dict.GetPhrase("ServiceState", ServiceState);
                RestartNeeded = dict.GetPhrase("RestartNeeded", RestartNeeded);
                Yes = dict.GetPhrase("Yes", Yes);
                No = dict.GetPhrase("No", No);
                ReceiveBaseTableError = dict.GetPhrase("ReceiveBaseTableError", ReceiveBaseTableError);
                UpdateSettingsCompleted = dict.GetPhrase("UpdateSettingsCompleted", UpdateSettingsCompleted);
                UpdateSettingsError = dict.GetPhrase("UpdateSettingsError", UpdateSettingsError);
                GetKpTypeInfoError = dict.GetPhrase("GetKpTypeInfoError", GetKpTypeInfoError);
                SecondInstanceClosed = dict.GetPhrase("SecondInstanceClosed", SecondInstanceClosed);
                CheckSecondInstanceError = dict.GetPhrase("CheckSecondInstanceError", CheckSecondInstanceError);
                ServiceNotInstalled = dict.GetPhrase("ServiceNotInstalled", ServiceNotInstalled);
                SaveSettingsConfirm = dict.GetPhrase("SaveSettingsConfirm", SaveSettingsConfirm);
                ServiceStartFailed = dict.GetPhrase("ServiceStartFailed", ServiceStartFailed);
                ServiceStopFailed = dict.GetPhrase("ServiceStopFailed", ServiceStopFailed);
                ServiceRestartFailed = dict.GetPhrase("ServiceRestartFailed", ServiceRestartFailed);
                ImportLinesAndKpError = dict.GetPhrase("ImportLinesAndKpError", ImportLinesAndKpError);
                ImportKpError = dict.GetPhrase("ImportKpError", ImportKpError);
                ResetReqParamsError = dict.GetPhrase("ResetReqParamsError", ResetReqParamsError);
                ShowKpPropsUnsupported = dict.GetPhrase("ShowKpPropsUnsupported", ShowKpPropsUnsupported);
                ShowKpPropsError = dict.GetPhrase("ShowKpPropsError", ShowKpPropsError);
                ShowCommCnlPropsError = dict.GetPhrase("ShowCommCnlPropsError", ShowCommCnlPropsError);
                UnknownDLL = dict.GetPhrase("UnknownDLL", UnknownDLL);
                IncorrectCmdVal = dict.GetPhrase("IncorrectCmdVal", IncorrectCmdVal);
                IncorrectHexCmdData = dict.GetPhrase("IncorrectHexCmdData", IncorrectHexCmdData);
                CmdDataRequired = dict.GetPhrase("CmdDataRequired", CmdDataRequired);
            }
        }
    }
}
