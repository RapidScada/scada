/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : SCADA-Administrator
 * Summary  : The phrases used in the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2018
 */

using Scada;

namespace ScadaAdmin
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

        // Словарь ScadaAdmin
        public static string BaseSDFFileNotFound { get; private set; }
        public static string RefreshRequired { get; private set; }

        // Словарь ScadaAdmin.DownloadUpload
        public static string DownloadTitle { get; private set; }
        public static string ConnectionName { get; private set; }
        public static string SessionCreated { get; private set; }
        public static string UnableCreateSession { get; private set; }
        public static string LoggedOn { get; private set; }
        public static string UnableLogin { get; private set; }
        public static string ConnectAgentError { get; private set; }
        public static string DownloadDataEmpty { get; private set; }
        public static string DownloadSuccessful { get; private set; }
        public static string DownloadError { get; private set; }
        public static string UploadTitle { get; private set; }
        public static string NoConfigInSrc { get; private set; }
        public static string ConfigUploaded { get; private set; }
        public static string ServerRestarted { get; private set; }
        public static string UnableRestartServer { get; private set; }
        public static string CommRestarted { get; private set; }
        public static string UnableRestartComm { get; private set; }
        public static string UploadSuccessful { get; private set; }
        public static string UploadError { get; private set; }

        // Словарь ScadaAdmin.ImportExport
        public static string ChooseBaseTableFile { get; private set; }
        public static string ChooseArchiveFile { get; private set; }
        public static string BaseTableFileFilter { get; private set; }
        public static string ArchiveFileFilter { get; private set; }
        public static string ImportFileNotExist { get; private set; }
        public static string ImportDirNotExist { get; private set; }
        public static string ImportTitle { get; private set; }
        public static string ImportTableTitle { get; private set; }
        public static string ImportSource { get; private set; }
        public static string LoadTableError { get; private set; }
        public static string SrcTableColumns { get; private set; }
        public static string DestTableColumns { get; private set; }
        public static string NoColumns { get; private set; }
        public static string WriteDBError { get; private set; }
        public static string ImportCompleted { get; private set; }
        public static string ImportCompletedWithErr { get; private set; }
        public static string ImportTableCompleted { get; private set; }
        public static string ImportTableCompletedWithErr { get; private set; }
        public static string ImportResult { get; private set; }
        public static string ImportTableResult { get; private set; }
        public static string ImportTableErrors { get; private set; }
        public static string ImportTableError { get; private set; }
        public static string ImportAllTablesError { get; private set; }
        public static string ImportArchiveError { get; private set; }
        public static string ExportFileUndefied { get; private set; }
        public static string ExportDirUndefied { get; private set; }
        public static string ExportDirNotExists { get; private set; }
        public static string ExportCompleted { get; private set; }
        public static string ExportError { get; private set; }
        public static string DbPassCompleted { get; private set; }
        public static string DbPassError { get; private set; }

        // Словарь ScadaAdmin.FrmCloneCnls
        public static string NotReplace { get; private set; }
        public static string Undefined { get; private set; }
        public static string FillObjListError { get; private set; }
        public static string FillKPListError { get; private set; }
        public static string CloneInCnlsCompleted { get; private set; }
        public static string CloneCtrlCnlsCompleted { get; private set; }
        public static string AddedCnlsCount { get; private set; }
        public static string CloneInCnlsError { get; private set; }
        public static string CloneCtrlCnlsError { get; private set; }
        public static string CloneCnlError { get; private set; }

        // Словарь ScadaAdmin.FrmCnlMap
        public static string NoChannels { get; private set; }
        public static string InCnlsByObjTitle { get; private set; }
        public static string InCnlsByKPTitle { get; private set; }
        public static string CtrlCnlsByObjTitle { get; private set; }
        public static string CtrlCnlsByKPTitle { get; private set; }
        public static string ObjectCaptionFormat { get; private set; }
        public static string KPCaptionFormat { get; private set; }
        public static string UndefinedObject { get; private set; }
        public static string UndefinedKP { get; private set; }
        public static string CreateCnlMapError { get; private set; }

        // Словарь ScadaAdmin.FrmCreateCnls
        public static string LoadKPDllError { get; private set; }
        public static string DevCalcError { get; private set; }
        public static string DevHasNoCnls { get; private set; }
        public static string CalcCnlNumsErrors { get; private set; }
        public static string CreatedCnlsMissing { get; private set; }
        public static string CalcCnlNumsError { get; private set; }
        public static string ErrorsCount { get; private set; }
        public static string CnlError { get; private set; }
        public static string CreateCnlsTitle { get; private set; }
        public static string CheckDicts { get; private set; }
        public static string ParamNotFound { get; private set; }
        public static string UnitNotFound { get; private set; }
        public static string CmdValsNotFound { get; private set; }
        public static string CreateCnlsImpossible { get; private set; }
        public static string CreateCnlsStart { get; private set; }
        public static string InCnlNameTrancated { get; private set; }
        public static string CtrlCnlNameTrancated { get; private set; }
        public static string NumFormatNotFound { get; private set; }
        public static string TextFormatNotFound { get; private set; }
        public static string AddedInCnlsCount { get; private set; }
        public static string AddedCtrlCnlsCount { get; private set; }
        public static string CreateCnlsComplSucc { get; private set; }
        public static string CreateCnlsComplWithErr { get; private set; }
        public static string CreateCnlsError { get; private set; }
        public static string UndefinedItem { get; private set; }
        public static string DllError { get; private set; }
        public static string DllLoaded { get; private set; }
        public static string DllNotFound { get; private set; }
        public static string FillKPFilterError { get; private set; }
        public static string FillKPGridError { get; private set; }

        // Словарь ScadaAdmin.FrmImport
        public static string AllTablesItem { get; private set; }
        public static string ArchiveItem { get; private set; }

        // Словарь ScadaAdmin.FrmInCnlProps
        public static string ShowInCnlPropsError { get; private set; }
        public static string IncorrectInCnlNum { get; private set; }
        public static string IncorrectInCnlName { get; private set; }
        public static string IncorrectCnlType { get; private set; }
        public static string IncorrectSignal { get; private set; }
        public static string IncorrectCtrlCnlNum { get; private set; }
        public static string CtrlCnlNotExists { get; private set; }
        public static string IncorrectLimLowCrash { get; private set; }
        public static string IncorrectLimLow { get; private set; }
        public static string IncorrectLimHigh { get; private set; }
        public static string IncorrectLimHighCrash { get; private set; }
        public static string WriteInCnlPropsError { get; private set; }

        // Словарь ScadaAdmin.FrmLanguage
        public static string IncorrectLanguage { get; private set; }

        // Словарь ScadaAdmin.FrmMain
        public static string SelectTable { get; private set; }
        public static string SaveReqCaption { get; private set; }
        public static string SaveReqQuestion { get; private set; }
        public static string SaveReqYes { get; private set; }
        public static string SaveReqNo { get; private set; }
        public static string SaveReqCancel { get; private set; }
        public static string DbNode { get; private set; }
        public static string SystemNode { get; private set; }
        public static string DictNode { get; private set; }
        public static string ConnectError { get; private set; }
        public static string DisconnectError { get; private set; }
        public static string UndefObj { get; private set; }
        public static string UndefKP { get; private set; }
        public static string CnlGroupError { get; private set; }
        public static string BackupCompleted { get; private set; }
        public static string BackupError { get; private set; }
        public static string CompactCompleted { get; private set; }
        public static string CompactError { get; private set; }
        public static string ConnectionUndefined { get; private set; }
        public static string ServiceRestartError { get; private set; }
        public static string LanguageChanged { get; private set; }

        // Словарь ScadaAdmin.FrmReplace
        public static string ValueNotFound { get; private set; }
        public static string FindCompleted { get; private set; }
        public static string ReplaceCount { get; private set; }

        // Словарь ScadaAdmin.FrmSettings
        public static string ChooseBaseSDFFile { get; private set; }
        public static string BaseSDFFileFilter { get; private set; }
        public static string ChooseBackupDir { get; private set; }
        public static string ChooseCommDir { get; private set; }
        public static string BaseSDFFileNotExists { get; private set; }
        public static string BackupDirNotExists { get; private set; }
        public static string CommDirNotExists { get; private set; }

        // Словарь ScadaAdmin.FrmTable
        public static string RefreshDataError { get; private set; }
        public static string DeleteRowConfirm { get; private set; }
        public static string DeleteRowsConfirm { get; private set; }
        public static string ClearTableConfirm { get; private set; }

        // Словарь ScadaAdmin.ServersSettings
        public static string LoadServersSettingsError { get; private set; }
        public static string SaveServersSettingsError { get; private set; }

        // Словарь ScadaAdmin.Tables
        public static string UpdateDataError { get; private set; }
        public static string FillSchemaError { get; private set; }
        public static string DataRequired { get; private set; }
        public static string UniqueRequired { get; private set; }
        public static string UnableDeleteRow { get; private set; }
        public static string UnableAddRow { get; private set; }
        public static string TranslateError { get; private set; }
        public static string GetTableError { get; private set; }
        public static string GetTableByObjError { get; private set; }
        public static string GetTableByKPError { get; private set; }
        public static string GetCtrlCnlNameError { get; private set; }
        public static string GetInCnlNumsError { get; private set; }
        public static string GetCtrlCnlNumsError { get; private set; }

        // Словарь ScadaAdmin.Remote
        public static string ChooseConfigDir { get; private set; }
        public static string ConfigDirRequired { get; private set; }
        public static string ConfigArcRequired { get; private set; }

        // Словарь ScadaAdmin.Remote.CtrlServerConn
        public static string DeleteConnConfirm { get; private set; }

        // Словарь ScadaAdmin.Remote.FrmConnSettings
        public static string EmptyFieldsNotAllowed { get; private set; }
        public static string ConnNameDuplicated { get; private set; }
        public static string IncorrectSecretKey { get; private set; }

        // Словарь ScadaAdmin.Remote.FrmServerStatus
        public static string UndefinedSvcStatus { get; private set; }
        public static string NormalSvcStatus { get; private set; }
        public static string StoppedSvcStatus { get; private set; }
        public static string ErrorSvcStatus { get; private set; }

        private static void SetToDefault()
        {
            BaseSDFFileNotFound = "Файл базы конфигурации в формате SDF {0} не найден.";
            RefreshRequired = "\r\nОбновите открытые таблицы, чтобы отобразить изменения.";

            DownloadTitle = "{0} Скачивание конфигурации";
            ConnectionName = "Соединение : {0}";
            SessionCreated = "Создана сессия {0}";
            UnableCreateSession = "Не удалось создать сессию";
            LoggedOn = "Вход в систему выполнен";
            UnableLogin = "Не удалось войти в систему - {0}";
            ConnectAgentError = "Ошибка при соединении с Агентом";
            DownloadDataEmpty = "Отсутствуют данные для скачивания";
            DownloadSuccessful = "Скачивание завершено успешно за {0} с.";
            DownloadError = "Ошибка при скачивании конфигурации";
            UploadTitle = "{0} Передача конфигурации";
            NoConfigInSrc = "Конфигурация отсутствует в заданном источнике.";
            ConfigUploaded = "Конфигурация передана";
            ServerRestarted = "Служба Сервера перезапущена";
            UnableRestartServer = "Не удалось перезапустить службу Сервера";
            CommRestarted = "Служба Коммуникатора перезапущена";
            UnableRestartComm = "Не удалось перезапустить службу Коммуникатора";
            UploadSuccessful = "Передача завершена успешно за {0} с.\r\n" +
                "Проверьте работоспособность удалённого сервера.";
            UploadError = "Ошибка при передаче конфигурации";

            ChooseBaseTableFile = "Выберите файл таблицы базы конфигурации";
            ChooseArchiveFile = "Выберите файл архива конфигурации";
            BaseTableFileFilter = "Таблицы базы конфигурации (*.dat)|*.dat|Все файлы (*.*)|*.*";
            ArchiveFileFilter = "Архив конфигурации (*.zip)|*.zip|Все файлы (*.*)|*.*";
            ImportFileNotExist = "Импортируемый файл не существует.";
            ImportDirNotExist = "Импортируемая директория не существует.";
            ImportTitle = "Импорт базы конфигурации";
            ImportTableTitle = "Импорт таблицы базы конфигурации \"{0}\"";
            ImportSource = "Исходный файл или директория : ";
            LoadTableError = "Ошибка при загрузке импортируемой таблицы";
            SrcTableColumns = "Поля исходной таблицы";
            DestTableColumns = "Поля таблицы, в которую производится импорт";
            NoColumns = "Отсутствуют";
            WriteDBError = "Ошибка при записи информации в БД";
            ImportCompleted = "Импорт завершён успешно.\r\nДобавлено/обновлено записей: {0}.";
            ImportCompletedWithErr = "Импорт завершён с ошибками.\r\nДобавлено/обновлено записей: {0}. Ошибок: {1}.";
            ImportTableCompleted = "Импорт таблицы базы конфигурации завершён успешно.\r\nДобавлено/обновлено записей: {0}.";
            ImportTableCompletedWithErr = "Импорт таблицы базы конфигурации завершён с ошибками.\r\n" +
                "Добавлено/обновлено записей: {0}. Ошибок: {1}.";
            ImportResult = "Результат импорта";
            ImportTableResult = "Результат импорта таблицы";
            ImportTableErrors = "Ошибки импорта таблицы";
            ImportTableError = "Ошибка при импорте таблицы базы конфигурации";
            ImportAllTablesError = "Ошибка при импорте всех таблиц базы конфигурации";
            ImportArchiveError = "Ошибка при импорте базы конфигурации из архива";
            ExportFileUndefied = "Файл экспорта не определён.";
            ExportDirUndefied = "Директория экспорта не определена.";
            ExportDirNotExists = "Директория экспорта не существует.";
            ExportCompleted = "Экспорт таблицы базы конфигурации завершён успешно.";
            ExportError = "Ошибка при экспорте таблицы базы конфигурации";
            DbPassCompleted = "Передача базы конфигурации SCADA-Серверу завершена успешно.\r\n" +
                "Изменения вступят в силу после перезапуска службы SCADA-Сервера.";
            DbPassError = "Ошибка при передаче базы конфигурации SCADA-Серверу";

            NotReplace = "<Не заменять>";
            Undefined = "<Не определён>";
            FillObjListError = "Ошибка при заполнении списка объектов";
            FillKPListError = "Ошибка при заполнении списка КП";
            CloneInCnlsCompleted = "Клонирование входных каналов завершёно успешно.";
            CloneCtrlCnlsCompleted = "Клонирование каналов управления завершёно успешно.";
            AddedCnlsCount = "Количество добавленных каналов: {0}.";
            CloneInCnlsError = "Ошибка при клонировании входных каналов";
            CloneCtrlCnlsError = "Ошибка при клонировании каналов управления";
            CloneCnlError = "Ошибка при клонировании канала {0}";

            NoChannels = "Каналы отсутствуют";
            InCnlsByObjTitle = "Входные каналы по объектам";
            InCnlsByKPTitle = "Входные каналы по КП";
            CtrlCnlsByObjTitle = "Каналы управления по объектам";
            CtrlCnlsByKPTitle = "Каналы управления по КП";
            ObjectCaptionFormat = "Объект {0} \"{1}\"";
            KPCaptionFormat = "КП {0} \"{1}\"";
            UndefinedObject = "<Объект не определён>";
            UndefinedKP = "<КП не определён>";
            CreateCnlMapError = "Ошибка при создании карты каналов";

            LoadKPDllError = "Ошибка при загрузке библиотек КП";
            DevCalcError = "Ошибка";
            DevHasNoCnls = "Нет";
            CalcCnlNumsErrors = "При расчёте номеров каналов возникли ошибки.\r\nОшибки показаны в таблице КП.";
            CreatedCnlsMissing = "Создаваемые каналы отсутствуют.";
            CalcCnlNumsError = "Ошибка при расчёте номеров каналов";
            ErrorsCount = "Количество ошибок: {0}.";
            CnlError = "Канал {0}: {1}";
            CreateCnlsTitle = "Создание каналов";
            CheckDicts = "Проверка справочников.";
            ParamNotFound = "Не найдена величина \"{0}\".";
            UnitNotFound = "Не найдена размерность \"{0}\".";
            CmdValsNotFound = "Не найдены значения команды \"{0}\".";
            CreateCnlsImpossible = "Создание каналов невозможно.";
            CreateCnlsStart = "Создание каналов.";
            InCnlNameTrancated = "Наименование входного канала {0} было обрезано.";
            CtrlCnlNameTrancated = "Наименование канала управления {0} было обрезано.";
            NumFormatNotFound = "Не найден формат входного канала {0}. Описание формата: числовой, количество знаков дробной части равно {1}.";
            TextFormatNotFound = "Не найден формат входного канала {0}. Описание формата: текстовый.";
            AddedInCnlsCount = "Добавлено входных каналов: {0}.";
            AddedCtrlCnlsCount = "Добавлено каналов управления: {0}.";
            CreateCnlsComplSucc = "Создание каналов завершено успешно.";
            CreateCnlsComplWithErr = "Создание каналов завершено с ошибками.";
            CreateCnlsError = "Ошибка при создании каналов";
            UndefinedItem = "<Не определён>";
            DllError = "Ошибка";
            DllLoaded = "Загружена";
            DllNotFound = "Не найдена";
            FillKPFilterError = "Ошибка при заполнении фильтра КП";
            FillKPGridError = "Ошибка при заполнении таблицы выбора КП";

            AllTablesItem = "Все таблицы";
            ArchiveItem = "Таблицы из архива";

            ShowInCnlPropsError = "Ошибка при отображении свойств входного канала";
            IncorrectInCnlNum = "Некорректное значение номера входного канала:";
            IncorrectInCnlName = "Некорректное значение наименования входного канала:";
            IncorrectCnlType = "Некорректное значение типа канала:";
            IncorrectSignal = "Некорректное значение сигнала:";
            IncorrectCtrlCnlNum = "Некорректное значение номера канала управления:";
            CtrlCnlNotExists = "Канал управления {0} не существует.";
            IncorrectLimLowCrash = "Некорректное значение нижней аварийной границы:";
            IncorrectLimLow = "Некорректное значение нижней границы:";
            IncorrectLimHigh = "Некорректное значение верхней границы:";
            IncorrectLimHighCrash = "Некорректное значение верхней аварийной границы:";
            WriteInCnlPropsError = "Ошибка при записи свойств входного канала";

            IncorrectLanguage = "Некорректный язык.";

            SelectTable = "Выберите в проводнике\r\nтаблицу для редактирования";
            SaveReqCaption = "Сохранение изменений";
            SaveReqQuestion = "Сохранить изменения?";
            SaveReqYes = "&Да";
            SaveReqNo = "&Нет";
            SaveReqCancel = "Отмена";
            DbNode = "База конфигурации";
            SystemNode = "Система";
            DictNode = "Справочники";
            ConnectError = "Ошибка при соединении с БД";
            DisconnectError = "Ошибка при разъединении с БД";
            UndefObj = "<Объект не опр.>";
            UndefKP = "<КП не опр.>";
            CnlGroupError = "Ошибка при группировке каналов";
            BackupCompleted = "Резервирование базы конфигурации завершено успешно.\r\nДанные сохранены в файле\r\n{0}";
            BackupError = "Ошибка при резервировании базы конфигурации";
            CompactCompleted = "Упаковка базы конфигурации завершена успешно.";
            CompactError = "Ошибка при упаковке базы конфигурации";
            ConnectionUndefined = "База конфигурации не определена.";
            ServiceRestartError = "Ошибка при перезапуске службы";
            LanguageChanged = "Изменение языка вступит в силу после перезапуска программы.";

            ValueNotFound = "Значение не найдено.";
            FindCompleted = "Поиск завершён.";
            ReplaceCount = "Произведено замен: {0}.";

            ChooseBaseSDFFile = "Выберите файл базы конфигурации в формате SDF";
            BaseSDFFileFilter = "Базы конфигурации|*.sdf|Все файлы|*.*";
            ChooseBackupDir = "Выберите директорию резервного копирования базы конфигурации";
            ChooseCommDir = "Выберите директорию SCADA-Коммуникатора";
            BaseSDFFileNotExists = "Файл базы конфигурации в формате SDF не существует.";
            BackupDirNotExists = "Директория резервного копирования базы конфигурации не существует.";
            CommDirNotExists = "Директория SCADA-Коммуникатора не существует.";

            RefreshDataError = "Ошибка при обновлении данных таблицы";
            DeleteRowConfirm = "Вы уверены, что хотите удалить строку?";
            DeleteRowsConfirm = "Вы уверены, что хотите удалить строки?";
            ClearTableConfirm = "Вы уверены, что хотите очистить таблицу?";

            LoadServersSettingsError = "Ошибка при загрузке настроек взаимодействия с удалёнными серверами";
            SaveServersSettingsError = "Ошибка при сохранении настроек взаимодействия с удалёнными серверами";

            UpdateDataError = "Ошибка при сохранении изменений таблицы в БД";
            FillSchemaError = "Ошибка при получении схемы данных таблицы";
            DataRequired = "Столбец \"{0}\" не может содержать пустых значений.";
            UniqueRequired = "Для столбца \"{0}\" не допускаются дублирующиеся значения.";
            UnableDeleteRow = "Удаление или изменение строки невозможно, т.к. на неё ссылаются данные из таблицы \"{0}\".";
            UnableAddRow = "Добавление или изменение строки невозможно, т.к. не существует данных для столбца \"{0}\".";
            TranslateError = "Ошибка при переводе сообщения";
            GetTableError = "Ошибка при получении таблицы \"{0}\"";
            GetTableByObjError = "Ошибка при получении таблицы \"{0}\" по номеру объекта";
            GetTableByKPError = "Ошибка при получении таблицы \"{0}\" по номеру КП";
            GetCtrlCnlNameError = "Ошибка при получении наименования канала управления";
            GetInCnlNumsError = "Ошибка при получении номеров входных каналов";
            GetCtrlCnlNumsError = "Ошибка при получении номеров каналов управления";

            ChooseConfigDir = "Выберите директорию конфигурации";
            ConfigDirRequired = "Укажите директорию конфигурации.";
            ConfigArcRequired = "Укажите имя файла архива конфигурации.";

            DeleteConnConfirm = "Вы уверены, что хотите удалить подключение?";

            EmptyFieldsNotAllowed = "Пустые значения полей не допускаются.";
            ConnNameDuplicated = "Соединение с таким наименованием уже существует.";
            IncorrectSecretKey = "Некорректный секретный ключ.";

            UndefinedSvcStatus = "Не определён";
            NormalSvcStatus = "Норма";
            StoppedSvcStatus = "Остановлен";
            ErrorSvcStatus = "Ошибка";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("ScadaAdmin", out dict))
            {
                BaseSDFFileNotFound = dict.GetPhrase("BaseSDFFileNotFound", BaseSDFFileNotFound);
                RefreshRequired = dict.GetPhrase("RefreshRequired", RefreshRequired);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.DownloadUpload", out dict))
            {
                DownloadTitle = dict.GetPhrase("DownloadTitle", DownloadTitle);
                ConnectionName = dict.GetPhrase("ConnectionName", ConnectionName);
                SessionCreated = dict.GetPhrase("SessionCreated", SessionCreated);
                UnableCreateSession = dict.GetPhrase("UnableCreateSession", UnableCreateSession);
                LoggedOn = dict.GetPhrase("LoggedOn", LoggedOn);
                UnableLogin = dict.GetPhrase("UnableLogin", UnableLogin);
                ConnectAgentError = dict.GetPhrase("ConnectAgentError", ConnectAgentError);
                DownloadDataEmpty = dict.GetPhrase("DownloadDataEmpty", DownloadDataEmpty);
                DownloadSuccessful = dict.GetPhrase("DownloadSuccessful", DownloadSuccessful);
                DownloadError = dict.GetPhrase("DownloadError", DownloadError);
                UploadTitle = dict.GetPhrase("UploadTitle", UploadTitle);
                NoConfigInSrc = dict.GetPhrase("NoConfigInSrc", NoConfigInSrc);
                ConfigUploaded = dict.GetPhrase("ConfigUploaded", ConfigUploaded);
                ServerRestarted = dict.GetPhrase("ServerRestarted", ServerRestarted);
                UnableRestartServer = dict.GetPhrase("UnableRestartServer", UnableRestartServer);
                CommRestarted = dict.GetPhrase("CommRestarted", CommRestarted);
                UnableRestartComm = dict.GetPhrase("UnableRestartComm", UnableRestartComm);
                UploadSuccessful = dict.GetPhrase("UploadSuccessful", UploadSuccessful);
                UploadError = dict.GetPhrase("UploadError", UploadError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.ImportExport", out dict))
            {
                ChooseBaseTableFile = dict.GetPhrase("ChooseBaseTableFile", ChooseBaseTableFile);
                ChooseArchiveFile = dict.GetPhrase("ChooseArchiveFile", ChooseArchiveFile);
                BaseTableFileFilter = dict.GetPhrase("BaseTableFileFilter", BaseTableFileFilter);
                ArchiveFileFilter = dict.GetPhrase("ArchiveFileFilter", ArchiveFileFilter);
                ImportFileNotExist = dict.GetPhrase("ImportFileNotExist", ImportFileNotExist);
                ImportDirNotExist = dict.GetPhrase("ImportDirNotExist", ImportDirNotExist);
                ImportTitle = dict.GetPhrase("ImportTitle", ImportTitle);
                ImportTableTitle = dict.GetPhrase("ImportTableTitle", ImportTableTitle);
                ImportSource = dict.GetPhrase("ImportSource", ImportSource);
                LoadTableError = dict.GetPhrase("LoadTableError", LoadTableError);
                SrcTableColumns = dict.GetPhrase("SrcTableColumns", SrcTableColumns);
                DestTableColumns = dict.GetPhrase("DestTableColumns", DestTableColumns);
                NoColumns = dict.GetPhrase("NoColumns", NoColumns);
                WriteDBError = dict.GetPhrase("WriteDBError", WriteDBError);
                ImportCompleted = dict.GetPhrase("ImportCompleted", ImportCompleted);
                ImportCompletedWithErr = dict.GetPhrase("ImportCompletedWithErr", ImportCompletedWithErr);
                ImportTableCompleted = dict.GetPhrase("ImportTableCompleted", ImportTableCompleted);
                ImportTableCompletedWithErr = dict.GetPhrase("ImportTableCompletedWithErr", ImportTableCompletedWithErr);
                ImportResult = dict.GetPhrase("ImportResult", ImportResult);
                ImportTableResult = dict.GetPhrase("ImportTableResult", ImportTableResult);
                ImportTableErrors = dict.GetPhrase("ImportTableErrors", ImportTableErrors);
                ImportTableError = dict.GetPhrase("ImportError", ImportTableError);
                ImportAllTablesError = dict.GetPhrase("ImportAllTablesError", ImportAllTablesError);
                ImportArchiveError = dict.GetPhrase("ImportArchiveError", ImportArchiveError);
                ExportFileUndefied = dict.GetPhrase("ExportFileUndefied", ExportFileUndefied);
                ExportDirUndefied = dict.GetPhrase("ExportDirUndefied", ExportDirUndefied);
                ExportDirNotExists = dict.GetPhrase("ExportDirNotExists", ExportDirNotExists);
                ExportCompleted = dict.GetPhrase("ExportCompleted", ExportCompleted);
                ExportError = dict.GetPhrase("ExportError", ExportError);
                DbPassCompleted = dict.GetPhrase("DbPassCompleted", DbPassCompleted);
                DbPassError = dict.GetPhrase("DbPassError", DbPassError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmCloneCnls", out dict))
            {
                NotReplace = dict.GetPhrase("NotReplace", NotReplace);
                Undefined = dict.GetPhrase("Undefined", Undefined);
                FillObjListError = dict.GetPhrase("FillObjListError", FillObjListError);
                FillKPListError = dict.GetPhrase("FillKPListError", FillKPListError);
                CloneInCnlsCompleted = dict.GetPhrase("CloneInCnlsCompleted", CloneInCnlsCompleted);
                CloneCtrlCnlsCompleted = dict.GetPhrase("CloneCtrlCnlsCompleted", CloneCtrlCnlsCompleted);
                AddedCnlsCount = dict.GetPhrase("AddedCnlsCount", AddedCnlsCount);
                CloneInCnlsError = dict.GetPhrase("CloneInCnlsError", CloneInCnlsError);
                CloneCtrlCnlsError = dict.GetPhrase("CloneCtrlCnlsError", CloneCtrlCnlsError);
                CloneCnlError = dict.GetPhrase("CloneCnlError", CloneCnlError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmCnlMap", out dict))
            {
                NoChannels = dict.GetPhrase("NoChannels", NoChannels);
                InCnlsByObjTitle = dict.GetPhrase("InCnlsByObjTitle", InCnlsByObjTitle);
                InCnlsByKPTitle = dict.GetPhrase("InCnlsByKPTitle", InCnlsByKPTitle);
                CtrlCnlsByObjTitle = dict.GetPhrase("CtrlCnlsByObjTitle", CtrlCnlsByObjTitle);
                CtrlCnlsByKPTitle = dict.GetPhrase("CtrlCnlsByKPTitle", CtrlCnlsByKPTitle);
                ObjectCaptionFormat = dict.GetPhrase("ObjectCaptionFormat", ObjectCaptionFormat);
                KPCaptionFormat = dict.GetPhrase("KPCaptionFormat", KPCaptionFormat);
                UndefinedObject = dict.GetPhrase("UndefinedObject", UndefinedObject);
                UndefinedKP = dict.GetPhrase("UndefinedKP", UndefinedKP);
                CreateCnlMapError = dict.GetPhrase("CreateCnlMapError", CreateCnlMapError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmCreateCnls", out dict))
            {
                LoadKPDllError = dict.GetPhrase("LoadKPDllError", LoadKPDllError);
                DevCalcError = dict.GetPhrase("DevCalcError", DevCalcError);
                DevHasNoCnls = dict.GetPhrase("DevHasNoCnls", DevHasNoCnls);
                CalcCnlNumsErrors = dict.GetPhrase("CalcCnlNumsErrors", CalcCnlNumsErrors);
                CreatedCnlsMissing = dict.GetPhrase("CreatedCnlsMissing", CreatedCnlsMissing);
                CalcCnlNumsError = dict.GetPhrase("CalcCnlNumsError", CalcCnlNumsError);
                ErrorsCount = dict.GetPhrase("ErrorsCount", ErrorsCount);
                CnlError = dict.GetPhrase("CnlError", CnlError);
                CreateCnlsTitle = dict.GetPhrase("CreateCnlsTitle", CreateCnlsTitle);
                CheckDicts = dict.GetPhrase("CheckDicts", CheckDicts);
                ParamNotFound = dict.GetPhrase("ParamNotFound", ParamNotFound);
                UnitNotFound = dict.GetPhrase("UnitNotFound", UnitNotFound);
                CmdValsNotFound = dict.GetPhrase("CmdValsNotFound", CmdValsNotFound);
                CreateCnlsImpossible = dict.GetPhrase("CreateCnlsImpossible", CreateCnlsImpossible);
                CreateCnlsStart = dict.GetPhrase("CreateCnlsStart", CreateCnlsStart);
                InCnlNameTrancated = dict.GetPhrase("InCnlNameTrancated", InCnlNameTrancated);
                CtrlCnlNameTrancated = dict.GetPhrase("CtrlCnlNameTrancated", CtrlCnlNameTrancated);
                NumFormatNotFound = dict.GetPhrase("NumFormatNotFound", NumFormatNotFound);
                TextFormatNotFound = dict.GetPhrase("TextFormatNotFound", TextFormatNotFound);
                AddedInCnlsCount = dict.GetPhrase("AddedInCnlsCount", AddedInCnlsCount);
                AddedCtrlCnlsCount = dict.GetPhrase("AddedCtrlCnlsCount", AddedCtrlCnlsCount);
                CreateCnlsComplSucc = dict.GetPhrase("CreateCnlsComplSucc", CreateCnlsComplSucc);
                CreateCnlsComplWithErr = dict.GetPhrase("CreateCnlsComplWithErr", CreateCnlsComplWithErr);
                CreateCnlsError = dict.GetPhrase("CreateCnlsError", CreateCnlsError);
                UndefinedItem = dict.GetPhrase("UndefinedItem", UndefinedItem);
                DllError = dict.GetPhrase("DllError", DllError);
                DllLoaded = dict.GetPhrase("DllLoaded", DllLoaded);
                DllNotFound = dict.GetPhrase("DllNotFound", DllNotFound);
                FillKPFilterError = dict.GetPhrase("FillKPFilterError", FillKPFilterError);
                FillKPGridError = dict.GetPhrase("FillKPGridError", FillKPGridError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmImport", out dict))
            {
                AllTablesItem = dict.GetPhrase("AllTablesItem", AllTablesItem);
                ArchiveItem = dict.GetPhrase("ArchiveItem", ArchiveItem);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmInCnlProps", out dict))
            {
                ShowInCnlPropsError = dict.GetPhrase("ShowInCnlPropsError", ShowInCnlPropsError);
                IncorrectInCnlNum = dict.GetPhrase("IncorrectInCnlNum", IncorrectInCnlNum);
                IncorrectInCnlName = dict.GetPhrase("IncorrectInCnlName", IncorrectInCnlName);
                IncorrectCnlType = dict.GetPhrase("IncorrectCnlType", IncorrectCnlType);
                IncorrectSignal = dict.GetPhrase("IncorrectSignal", IncorrectSignal);
                IncorrectCtrlCnlNum = dict.GetPhrase("IncorrectCtrlCnlNum", IncorrectCtrlCnlNum);
                CtrlCnlNotExists = dict.GetPhrase("CtrlCnlNotExists", CtrlCnlNotExists);
                IncorrectLimLowCrash = dict.GetPhrase("IncorrectLimLowCrash", IncorrectLimLowCrash);
                IncorrectLimLow = dict.GetPhrase("IncorrectLimLow", IncorrectLimLow);
                IncorrectLimHigh = dict.GetPhrase("IncorrectLimHigh", IncorrectLimHigh);
                IncorrectLimHighCrash = dict.GetPhrase("IncorrectLimHighCrash", IncorrectLimHighCrash);
                WriteInCnlPropsError = dict.GetPhrase("WriteInCnlPropsError", WriteInCnlPropsError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmLanguage", out dict))
            {
                IncorrectLanguage = dict.GetPhrase("IncorrectLanguage", IncorrectLanguage);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmMain", out dict))
            {
                SelectTable = dict.GetPhrase("SelectTable", SelectTable);
                SaveReqCaption = dict.GetPhrase("SaveReqCaption", SaveReqCaption);
                SaveReqQuestion = dict.GetPhrase("SaveReqQuestion", SaveReqQuestion);
                SaveReqYes = dict.GetPhrase("SaveReqYes", SaveReqYes);
                SaveReqNo = dict.GetPhrase("SaveReqNo", SaveReqNo);
                SaveReqCancel = dict.GetPhrase("SaveReqCancel", SaveReqCancel);
                DbNode = dict.GetPhrase("DbNode", DbNode);
                SystemNode = dict.GetPhrase("SystemNode", SystemNode);
                DictNode = dict.GetPhrase("DictNode", DictNode);
                ConnectError = dict.GetPhrase("ConnectError", ConnectError);
                DisconnectError = dict.GetPhrase("DisconnectError", DisconnectError);
                UndefObj = dict.GetPhrase("UndefObj", UndefObj);
                UndefKP = dict.GetPhrase("UndefKP", UndefKP);
                CnlGroupError = dict.GetPhrase("CnlGroupError", CnlGroupError);
                BackupCompleted = dict.GetPhrase("BackupCompleted", BackupCompleted);
                BackupError = dict.GetPhrase("BackupError", BackupError);
                CompactCompleted = dict.GetPhrase("CompactCompleted", CompactCompleted);
                CompactError = dict.GetPhrase("CompactError", CompactError);
                ConnectionUndefined = dict.GetPhrase("ConnectionUndefined", ConnectionUndefined);
                ServiceRestartError = dict.GetPhrase("ServiceRestartError", ServiceRestartError);
                LanguageChanged = dict.GetPhrase("LanguageChanged", LanguageChanged);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmReplace", out dict))
            {
                ValueNotFound = dict.GetPhrase("ValueNotFound", ValueNotFound);
                FindCompleted = dict.GetPhrase("FindCompleted", FindCompleted);
                ReplaceCount = dict.GetPhrase("ReplaceCount", ReplaceCount);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmSettings", out dict))
            {
                ChooseBaseSDFFile = dict.GetPhrase("ChooseBaseSDFFile", ChooseBaseSDFFile);
                BaseSDFFileFilter = dict.GetPhrase("BaseSDFFileFilter", BaseSDFFileFilter);
                ChooseBackupDir = dict.GetPhrase("ChooseBackupDir", ChooseBackupDir);
                ChooseCommDir = dict.GetPhrase("ChooseCommDir", ChooseCommDir);
                BaseSDFFileNotExists = dict.GetPhrase("BaseSDFFileNotExists", BaseSDFFileNotExists);
                BackupDirNotExists = dict.GetPhrase("BackupDirNotExists", BackupDirNotExists);
                CommDirNotExists = dict.GetPhrase("CommDirNotExists", CommDirNotExists);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.FrmTable", out dict))
            {
                RefreshDataError = dict.GetPhrase("RefreshDataError", RefreshDataError);
                DeleteRowConfirm = dict.GetPhrase("DeleteRowConfirm", DeleteRowConfirm);
                DeleteRowsConfirm = dict.GetPhrase("DeleteRowsConfirm", DeleteRowsConfirm);
                ClearTableConfirm = dict.GetPhrase("ClearTableConfirm", ClearTableConfirm);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.ServersSettings", out dict))
            {
                LoadServersSettingsError = dict.GetPhrase("LoadServersSettingsError", LoadServersSettingsError);
                SaveServersSettingsError = dict.GetPhrase("SaveServersSettingsError", SaveServersSettingsError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.Tables", out dict))
            {
                UpdateDataError = dict.GetPhrase("UpdateDataError", UpdateDataError);
                FillSchemaError = dict.GetPhrase("FillSchemaError", FillSchemaError);
                DataRequired = dict.GetPhrase("DataRequired", DataRequired);
                UniqueRequired = dict.GetPhrase("UniqueRequired", UniqueRequired);
                UnableDeleteRow = dict.GetPhrase("UnableDeleteRow", UnableDeleteRow);
                UnableAddRow = dict.GetPhrase("UnableAddRow", UnableAddRow);
                TranslateError = dict.GetPhrase("TranslateError", TranslateError);
                GetTableError = dict.GetPhrase("GetTableError", GetTableError);
                GetTableByObjError = dict.GetPhrase("GetTableByObjError", GetTableByObjError);
                GetTableByKPError = dict.GetPhrase("GetTableByKPError", GetTableByKPError);
                GetCtrlCnlNameError = dict.GetPhrase("GetCtrlCnlNameError", GetCtrlCnlNameError);
                GetInCnlNumsError = dict.GetPhrase("GetInCnlNumsError", GetInCnlNumsError);
                GetCtrlCnlNumsError = dict.GetPhrase("GetCtrlCnlNumsError", GetCtrlCnlNumsError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.Remote", out dict))
            {
                ChooseConfigDir = dict.GetPhrase("ChooseConfigDir", ChooseConfigDir);
                ConfigDirRequired = dict.GetPhrase("ConfigDirRequired", ConfigDirRequired);
                ConfigArcRequired = dict.GetPhrase("ConfigArcRequired", ConfigArcRequired);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.Remote.CtrlServerConn", out dict))
            {
                DeleteConnConfirm = dict.GetPhrase("DeleteConnConfirm", DeleteConnConfirm);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.Remote.FrmConnSettings", out dict))
            {
                EmptyFieldsNotAllowed = dict.GetPhrase("EmptyFieldsNotAllowed", EmptyFieldsNotAllowed);
                ConnNameDuplicated = dict.GetPhrase("ConnNameDuplicated", ConnNameDuplicated);
                IncorrectSecretKey = dict.GetPhrase("IncorrectSecretKey", IncorrectSecretKey);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaAdmin.Remote.FrmServerStatus", out dict))
            {
                UndefinedSvcStatus = dict.GetPhrase("UndefinedSvcStatus", UndefinedSvcStatus);
                NormalSvcStatus = dict.GetPhrase("NormalSvcStatus", NormalSvcStatus);
                StoppedSvcStatus = dict.GetPhrase("StoppedSvcStatus", StoppedSvcStatus);
                ErrorSvcStatus = dict.GetPhrase("ErrorSvcStatus", ErrorSvcStatus);
            }
        }
    }
}
