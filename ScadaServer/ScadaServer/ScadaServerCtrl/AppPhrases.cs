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
 * Module   : SCADA-Server Control
 * Summary  : The phrases used in the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2019
 */

namespace Scada.Server.Ctrl
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

        // Словарь Scada.Server.Ctrl
        public static string IncorrectFilter { get; private set; }

        // Словарь Scada.Server.Ctrl.FrmBaseTableView
        public static string LoadBaseTableError { get; private set; }

        // Словарь Scada.Server.Ctrl.FrmEventTableEdit
        public static string EditEventTableTitle { get; private set; }
        public static string ViewEventTableTitle { get; private set; }
        public static string LoadEventTableError { get; private set; }
        public static string SaveEventTableError { get; private set; }
        public static string SaveEventTableConfirm { get; private set; }

        // Словарь Scada.Server.Ctrl.FrmMain
        public static string MainFormTitle { get; private set; }
        public static string CommonParamsNode { get; private set; }
        public static string SaveParamsNode { get; private set; }
        public static string FilesNode { get; private set; }
        public static string BaseNode { get; private set; }
        public static string CurSrezNode { get; private set; }
        public static string MinSrezNode { get; private set; }
        public static string HrSrezNode { get; private set; }
        public static string EventsNode { get; private set; }
        public static string ModulesNode { get; private set; }
        public static string GeneratorNode { get; private set; }
        public static string StatsNode { get; private set; }
        public static string ServiceState { get; private set; }
        public static string RestartNeeded { get; private set; }
        public static string GetFileListError { get; private set; }
        public static string LoadModuleError { get; private set; }
        public static string SecondInstanceClosed { get; private set; }
        public static string CheckSecondInstanceError { get; private set; }
        public static string ServiceNotInstalled { get; private set; }
        public static string SaveSettingsConfirm { get; private set; }
        public static string ServiceStartFailed { get; private set; }
        public static string ServiceStopFailed { get; private set; }
        public static string ServiceRestartFailed { get; private set; }
        public static string ChooseItfDir { get; private set; }
        public static string ChooseArcDir { get; private set; }
        public static string ChooseArcCopyDir { get; private set; }
        public static string ModuleFileFilter { get; private set; }
        public static string ModuleAlreadyAdded { get; private set; }
        public static string IncorrectCnlNum { get; private set; }
        public static string IncorrectCnlVal { get; private set; }
        public static string IncorrectOldCnlVal { get; private set; }
        public static string IncorrectNewCnlVal { get; private set; }
        public static string IncorrectCmdVal { get; private set; }
        public static string IncorrectHexCmdData { get; private set; }
        public static string CmdDataRequired { get; private set; }
        public static string DataSentSuccessfully { get; private set; }
        public static string EventSentSuccessfully { get; private set; }
        public static string EventCheckSentSuccessfully { get; private set; }
        public static string CmdSentSuccessfully { get; private set; }

        // Словарь Scada.Server.Ctrl.FrmSrezTableEdit
        public static string EditSrezTableTitle { get; private set; }
        public static string ViewSrezTableTitle { get; private set; }
        public static string LoadSrezTableError { get; private set; }
        public static string SaveSrezTableError { get; private set; }
        public static string SaveSrezTableConfirm { get; private set; }

        private static void SetToDefault()
        {
            IncorrectFilter = "Некорректный фильтр.";

            LoadBaseTableError = "Ошибка при загрузке таблицы базы конфигурации";

            EditEventTableTitle = "Редактирование таблицы событий";
            ViewEventTableTitle = "Просмотр таблицы событий";
            LoadEventTableError = "Ошибка при загрузке таблицы событий";
            SaveEventTableError = "Ошибка при сохранении таблицы событий";
            SaveEventTableConfirm = "Таблица событий была изменена.\r\nСохранить изменения?";

            MainFormTitle = "SCADA-Сервер";
            CommonParamsNode = "Общие параметры";
            SaveParamsNode = "Параметры записи";
            FilesNode = "Файлы";
            BaseNode = "База конфигурации";
            CurSrezNode = "Текущий срез";
            MinSrezNode = "Минутные срезы";
            HrSrezNode = "Часовые срезы";
            EventsNode = "События";
            ModulesNode = "Модули";
            GeneratorNode = "Генератор";
            StatsNode = "Статистика";
            ServiceState = "Состояние службы {0}: {1}";
            RestartNeeded = "Изменения настроек вступят в силу после перезапуска службы SCADA-Сервера.";
            GetFileListError = "Ошибка при получении списка файлов данных";
            LoadModuleError = "Ошибка при загрузке модуля";
            SecondInstanceClosed = "Программа для управления SCADA-Сервером уже запущена.\r\nВторая копия будет закрыта.";
            CheckSecondInstanceError = "Ошибка при проверке запуска второй копии программы";
            ServiceNotInstalled = "Служба SCADA-Сервера не установлена.";
            SaveSettingsConfirm = "Настройки SCADA-Сервера были изменены.\r\nСохранить изменения?";
            ServiceStartFailed = "Не удалось запустить службу SCADA-Сервера";
            ServiceStopFailed = "Не удалось остановить службу SCADA-Сервера";
            ServiceRestartFailed = "Не удалось перезапусить службу SCADA-Сервера";
            ChooseItfDir = "Выберите директорию интерфейса";
            ChooseArcDir = "Выберите директорию архива в формате DAT";
            ChooseArcCopyDir = "Выберите директорию копии архива в формате DAT";
            ModuleFileFilter = "Модули (*.dll)|*.dll|Все файлы (*.*)|*.*";
            ModuleAlreadyAdded = "Модуль уже добавлен.";
            IncorrectCnlNum = "Некорректный номер канала.";
            IncorrectCnlVal = "Некорректное значение канала.";
            IncorrectOldCnlVal = "Некорректное старое значение канала.";
            IncorrectNewCnlVal = "Некорректное новое значение канала.";
            IncorrectCmdVal = "Некорректное значение команды.";
            IncorrectHexCmdData = "Некорректные 16-ричные данные команды.";
            CmdDataRequired = "Необходимо ввести данные команды.";
            DataSentSuccessfully = "Данные отправлены успешно.";
            EventSentSuccessfully = "Событие отправлено успешно.";
            EventCheckSentSuccessfully = "Команда квитирования события отправлена успешно.";
            CmdSentSuccessfully = "Команда отправлена успешно.";

            EditSrezTableTitle = "Редактирование таблицы срезов";
            ViewSrezTableTitle = "Просмотр таблицы срезов";
            LoadSrezTableError = "Ошибка при загрузке таблицы срезов";
            SaveSrezTableError = "Ошибка при сохранении таблицы срезов";
            SaveSrezTableConfirm = "Таблица срезов была изменена.\r\nСохранить изменения?";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Server.Ctrl", out dict))
                IncorrectFilter = dict.GetPhrase("IncorrectFilter", IncorrectFilter);

            if (Localization.Dictionaries.TryGetValue("Scada.Server.Ctrl.FrmBaseTableView", out dict))
                LoadBaseTableError = dict.GetPhrase("LoadBaseTableError", LoadBaseTableError);

            if (Localization.Dictionaries.TryGetValue("Scada.Server.Ctrl.FrmEventTableEdit", out dict))
            {
                EditEventTableTitle = dict.GetPhrase("EditEventTableTitle", EditEventTableTitle);
                ViewEventTableTitle = dict.GetPhrase("ViewEventTableTitle", ViewEventTableTitle);
                LoadEventTableError = dict.GetPhrase("LoadEventTableError", LoadEventTableError);
                SaveEventTableError = dict.GetPhrase("SaveEventTableError", SaveEventTableError);
                SaveEventTableConfirm = dict.GetPhrase("SaveEventTableConfirm", SaveEventTableConfirm);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Server.Ctrl.FrmMain", out dict))
            {
                MainFormTitle = dict.GetPhrase("this", MainFormTitle);
                CommonParamsNode = dict.GetPhrase("CommonParamsNode", CommonParamsNode);
                SaveParamsNode = dict.GetPhrase("SaveParamsNode", SaveParamsNode);
                FilesNode = dict.GetPhrase("FilesNode", FilesNode);
                BaseNode = dict.GetPhrase("BaseNode", BaseNode);
                CurSrezNode = dict.GetPhrase("CurSrezNode", CurSrezNode);
                MinSrezNode = dict.GetPhrase("MinSrezNode", MinSrezNode);
                HrSrezNode = dict.GetPhrase("HrSrezNode", HrSrezNode);
                EventsNode = dict.GetPhrase("EventsNode", EventsNode);
                ModulesNode = dict.GetPhrase("ModulesNode", ModulesNode);
                GeneratorNode = dict.GetPhrase("GeneratorNode", GeneratorNode);
                StatsNode = dict.GetPhrase("StatsNode", StatsNode);
                ServiceState = dict.GetPhrase("ServiceState", ServiceState);
                RestartNeeded = dict.GetPhrase("RestartNeeded", RestartNeeded);
                GetFileListError = dict.GetPhrase("GetFileListError", GetFileListError);
                LoadModuleError = dict.GetPhrase("LoadModuleError", LoadModuleError);
                SecondInstanceClosed = dict.GetPhrase("SecondInstanceClosed", SecondInstanceClosed);
                CheckSecondInstanceError = dict.GetPhrase("CheckSecondInstanceError", CheckSecondInstanceError);
                ServiceNotInstalled = dict.GetPhrase("ServiceNotInstalled", ServiceNotInstalled);
                SaveSettingsConfirm = dict.GetPhrase("SaveSettingsConfirm", SaveSettingsConfirm);
                ServiceStartFailed = dict.GetPhrase("ServiceStartFailed", ServiceStartFailed);
                ServiceStopFailed = dict.GetPhrase("ServiceStopFailed", ServiceStopFailed);
                ServiceRestartFailed = dict.GetPhrase("ServiceRestartFailed", ServiceRestartFailed);
                ChooseItfDir = dict.GetPhrase("ChooseItfDir", ChooseItfDir);
                ChooseArcDir = dict.GetPhrase("ChooseArcDir", ChooseArcDir);
                ChooseArcCopyDir = dict.GetPhrase("ChooseArcCopyDir", ChooseArcCopyDir);
                ModuleFileFilter = dict.GetPhrase("ModuleFileFilter", ModuleFileFilter);
                ModuleAlreadyAdded = dict.GetPhrase("ModuleAlreadyAdded", ModuleAlreadyAdded);
                IncorrectCnlNum = dict.GetPhrase("IncorrectCnlNum", IncorrectCnlNum);
                IncorrectCnlVal = dict.GetPhrase("IncorrectCnlVal", IncorrectCnlVal);
                IncorrectOldCnlVal = dict.GetPhrase("IncorrectOldCnlVal", IncorrectOldCnlVal);
                IncorrectNewCnlVal = dict.GetPhrase("IncorrectNewCnlVal", IncorrectNewCnlVal);
                IncorrectCmdVal = dict.GetPhrase("IncorrectCmdVal", IncorrectCmdVal);
                IncorrectHexCmdData = dict.GetPhrase("IncorrectHexCmdData", IncorrectHexCmdData);
                CmdDataRequired = dict.GetPhrase("CmdDataRequired", CmdDataRequired);
                DataSentSuccessfully = dict.GetPhrase("DataSentSuccessfully", DataSentSuccessfully);
                EventSentSuccessfully = dict.GetPhrase("EventSentSuccessfully", EventSentSuccessfully);
                EventCheckSentSuccessfully = dict.GetPhrase("EventCheckSentSuccessfully", EventCheckSentSuccessfully);
                CmdSentSuccessfully = dict.GetPhrase("CmdSentSuccessfully", CmdSentSuccessfully);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Server.Ctrl.FrmSrezTableEdit", out dict))
            {
                EditSrezTableTitle = dict.GetPhrase("EditSrezTableTitle", EditSrezTableTitle);
                ViewSrezTableTitle = dict.GetPhrase("ViewSrezTableTitle", ViewSrezTableTitle);
                LoadSrezTableError = dict.GetPhrase("LoadSrezTableError", LoadSrezTableError);
                SaveSrezTableError = dict.GetPhrase("SaveSrezTableError", SaveSrezTableError);
                SaveSrezTableConfirm = dict.GetPhrase("SaveSrezTableConfirm", SaveSrezTableConfirm);
            }
        }
    }
}
