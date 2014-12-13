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
 * Module   : ScadaData
 * Summary  : The common phrases used in the system
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2014
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada
{
    /// <summary>
    /// The common phrases used in the system
    /// <para>Общие фразы, используемые в системе</para>
    /// </summary>
    public static class CommonPhrases
    {
        static CommonPhrases()
        {
            SetToDefault();
        }

        public static string InfoCaption { get; private set; }
        public static string QuestionCaption { get; private set; }
        public static string ErrorCaption { get; private set; }
        public static string WarningCaption { get; private set; }
        public static string ErrorWithColon { get; private set; }
        public static string SaveSettingsConfirm { get; private set; }
        public static string SaveModSettingsConfirm { get; private set; }
        public static string SaveKpSettingsConfirm { get; private set; }
        public static string FileNotFound { get; private set; }
        public static string DirNotExists { get; private set; }
        public static string NamedFileNotFound { get; private set; }
        public static string NamedDirNotExists { get; private set; }
        public static string BaseDATDir { get; private set; }
        public static string BaseDATDirNotExists { get; private set; }
        public static string ChooseBaseDATDir { get; private set; }
        public static string LoadAppSettingsError { get; private set; }
        public static string SaveAppSettingsError { get; private set; }
        public static string LoadCommSettingsError { get; private set; }
        public static string SaveCommSettingsError { get; private set; }
        public static string LoadModSettingsError { get; private set; }
        public static string SaveModSettingsError { get; private set; }
        public static string LoadKpSettingsError { get; private set; }
        public static string SaveKpSettingsError { get; private set; }
        public static string GridDataError { get; private set; }
        public static string IntegerRequired { get; private set; }
        public static string IntegerRangingRequired { get; private set; }
        public static string RealRequired { get; private set; }
        public static string NonemptyRequired { get; private set; }
        public static string DateTimeRequired { get; private set; }
        public static string LineLengthLimit { get; private set; }
        public static string NotNumber { get; private set; }
        public static string LoadImageError { get; private set; }
        public static string LoadHyperlinkError { get; private set; }
        public static string IncorrectFileFormat { get; private set; }
        public static string NoData { get; private set; }
        public static string NoRights { get; private set; }
        public static string IncorrectXmlNodeVal { get; private set; }
        public static string IncorrectXmlAttrVal { get; private set; }
        public static string IncorrectXmlParamVal { get; private set; }
        public static string XmlNodeNotFound { get; private set; }
        public static string CmdTypeTable { get; private set; }
        public static string CmdValTable { get; private set; }
        public static string CnlTypeTable { get; private set; }
        public static string CommLineTable { get; private set; }
        public static string CtrlCnlTable { get; private set; }
        public static string EvTypeTable { get; private set; }
        public static string FormatTable { get; private set; }
        public static string FormulaTable { get; private set; }
        public static string InCnlTable { get; private set; }
        public static string InterfaceTable { get; private set; }
        public static string KPTable { get; private set; }
        public static string KPTypeTable { get; private set; }
        public static string ObjTable { get; private set; }
        public static string ParamTable { get; private set; }
        public static string RightTable { get; private set; }
        public static string RoleTable { get; private set; }
        public static string UnitTable { get; private set; }
        public static string UserTable { get; private set; }
        public static string ContinuePendingSvcState { get; private set; }
        public static string PausedSvcState { get; private set; }
        public static string PausePendingSvcState { get; private set; }
        public static string RunningSvcState { get; private set; }
        public static string StartPendingSvcState { get; private set; }
        public static string StoppedSvcState { get; private set; }
        public static string StopPendingSvcState { get; private set; }
        public static string NotInstalledSvcState { get; private set; }

        private static void SetToDefault()
        {
            InfoCaption = "Информация";
            QuestionCaption = "Вопрос";
            ErrorCaption = "Ошибка";
            WarningCaption = "Предупреждение";
            ErrorWithColon = "Ошибка:";
            SaveSettingsConfirm = "Настройки были изменены. Сохранить изменения?";
            SaveModSettingsConfirm = "Настройки модуля были изменены. Сохранить изменения?";
            SaveKpSettingsConfirm = "Настройки КП были изменены. Сохранить изменения?";
            FileNotFound = "Не найден файл.";
            DirNotExists = "Директория не существует.";
            NamedFileNotFound = "Файл {0} не найден.";
            NamedDirNotExists = "Директория {0} не существует.";
            BaseDATDir = "Директория базы конфигурации в формате DAT";
            BaseDATDirNotExists = "Директория базы конфигурации в формате DAT не существует.";
            ChooseBaseDATDir = "Выберите директорию базы конфигурации в формате DAT";
            LoadAppSettingsError = "Ошибка при загрузке настроек приложения";
            SaveAppSettingsError = "Ошибка при сохранении настроек приложения";
            LoadCommSettingsError = "Ошибка при загрузке настроек соединения с сервером";
            SaveCommSettingsError = "Ошибка при сохранении настроек соединения с сервером";
            LoadModSettingsError = "Ошибка при загрузке настроек модуля";
            SaveModSettingsError = "Ошибка при сохранении настроек модуля";
            LoadKpSettingsError = "Ошибка при загрузке настроек КП";
            SaveKpSettingsError = "Ошибка при сохранении настроек КП";
            GridDataError = "Ошибка при работе с данными";
            IntegerRequired = "Требуется целое число.";
            IntegerRangingRequired = "Требуется целое число в диапазоне от {0} до {1}.";
            RealRequired = "Требуется вещественное число.";
            NonemptyRequired = "Требуется непустое значение.";
            DateTimeRequired = "Требуется дата и время.";
            LineLengthLimit = "Длина строки должна быть не более {0} символов.";
            NotNumber = "\"{0}\" не является числом";
            LoadImageError = "Ошибка при загрузке изображения из файла:\n{0}";
            LoadHyperlinkError = "Ошибка при загрузке гиперссылки из файла:\n{0}";
            IncorrectFileFormat = "Некорректный формат файла.";
            NoData = "Нет данных";
            NoRights = "Недостаточно прав.";
            IncorrectXmlNodeVal = "Некорректное значение XML-узла \"{0}\".";
            IncorrectXmlAttrVal = "Некорректное значение XML-атрибута \"{0}\".";
            IncorrectXmlParamVal = "Некорректное значение параметра \"{0}\".";
            XmlNodeNotFound = "XML-узел \"{0}\" не найден внутри узла \"{1}\".";
            CmdTypeTable = "Типы команд";
            CmdValTable = "Значения команд";
            CnlTypeTable = "Типы каналов";
            CommLineTable = "Линии связи";
            CtrlCnlTable = "Каналы управления";
            EvTypeTable = "Типы событий";
            FormatTable = "Форматы чисел";
            FormulaTable = "Формулы";
            InCnlTable = "Входные каналы";
            InterfaceTable = "Интерфейс";
            KPTable = "КП";
            KPTypeTable = "Типы КП";
            ObjTable = "Объекты";
            ParamTable = "Величины";
            RightTable = "Права";
            RoleTable = "Роли";
            UnitTable = "Размерности";
            UserTable = "Пользователи";
            ContinuePendingSvcState = "ожидание продолжения";
            PausedSvcState = "пауза";
            PausePendingSvcState = "ожидание паузы";
            RunningSvcState = "работает";
            StartPendingSvcState = "ожидание запуска";
            StoppedSvcState = "остановлена";
            StopPendingSvcState = "ожидание остановки";
            NotInstalledSvcState = "не установлена";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Common", out dict))
            {
                InfoCaption = dict.GetPhrase("InfoCaption", InfoCaption);
                QuestionCaption = dict.GetPhrase("QuestionCaption", QuestionCaption);
                ErrorCaption = dict.GetPhrase("ErrorCaption", ErrorCaption);
                WarningCaption = dict.GetPhrase("WarningCaption", WarningCaption);
                ErrorWithColon = dict.GetPhrase("ErrorWithColon", ErrorWithColon);
                SaveSettingsConfirm = dict.GetPhrase("SaveSettingsConfirm", SaveSettingsConfirm);
                SaveModSettingsConfirm = dict.GetPhrase("SaveModSettingsConfirm", SaveModSettingsConfirm);
                SaveKpSettingsConfirm = dict.GetPhrase("SaveKpSettingsConfirm", SaveKpSettingsConfirm);
                FileNotFound = dict.GetPhrase("FileNotFound", FileNotFound);
                DirNotExists = dict.GetPhrase("DirNotExists", DirNotExists);
                NamedFileNotFound = dict.GetPhrase("NamedFileNotFound", NamedFileNotFound);
                NamedDirNotExists = dict.GetPhrase("NamedDirNotExists", NamedDirNotExists);
                BaseDATDir = dict.GetPhrase("BaseDATDir", BaseDATDir);
                BaseDATDirNotExists = dict.GetPhrase("BaseDATDirNotExists", BaseDATDirNotExists);
                ChooseBaseDATDir = dict.GetPhrase("ChooseBaseDATDir", ChooseBaseDATDir);
                LoadAppSettingsError = dict.GetPhrase("LoadAppSettingsError", LoadAppSettingsError);
                SaveAppSettingsError = dict.GetPhrase("SaveAppSettingsError", SaveAppSettingsError);
                LoadCommSettingsError = dict.GetPhrase("LoadCommSettingsError", LoadCommSettingsError);
                SaveCommSettingsError = dict.GetPhrase("SaveCommSettingsError", SaveCommSettingsError);
                LoadModSettingsError = dict.GetPhrase("LoadModSettingsError", LoadModSettingsError);
                SaveModSettingsError = dict.GetPhrase("SaveModSettingsError", SaveModSettingsError);
                LoadKpSettingsError = dict.GetPhrase("LoadKpSettingsError", LoadKpSettingsError);
                SaveKpSettingsError = dict.GetPhrase("SaveKpSettingsError", SaveKpSettingsError);
                GridDataError = dict.GetPhrase("GridDataError", GridDataError);
                IntegerRequired = dict.GetPhrase("IntegerRequired", IntegerRequired);
                IntegerRangingRequired = dict.GetPhrase("IntegerRangingRequired", IntegerRangingRequired);
                RealRequired = dict.GetPhrase("RealRequired", RealRequired);
                NonemptyRequired = dict.GetPhrase("NonemptyRequired", NonemptyRequired);
                DateTimeRequired = dict.GetPhrase("DateTimeRequired", DateTimeRequired);
                LineLengthLimit = dict.GetPhrase("LineLengthLimit", LineLengthLimit);
                NotNumber = dict.GetPhrase("NotNumber", NotNumber);
                LoadImageError = dict.GetPhrase("LoadImageError", LoadImageError);
                LoadHyperlinkError = dict.GetPhrase("LoadHyperlinkError", LoadHyperlinkError);
                NoData = dict.GetPhrase("NoData", NoData);
                NoRights = dict.GetPhrase("NoRights", NoRights);
                IncorrectXmlNodeVal = dict.GetPhrase("IncorrectXmlNodeVal", IncorrectXmlNodeVal);
                IncorrectXmlAttrVal = dict.GetPhrase("IncorrectXmlAttrVal", IncorrectXmlAttrVal);
                IncorrectXmlParamVal = dict.GetPhrase("IncorrectXmlParamVal", IncorrectXmlParamVal);
                XmlNodeNotFound = dict.GetPhrase("XmlNodeNotFound", XmlNodeNotFound);
                CmdTypeTable = dict.GetPhrase("CmdTypeTable", CmdTypeTable);
                CmdValTable = dict.GetPhrase("CmdValTable", CmdValTable);
                CnlTypeTable = dict.GetPhrase("CnlTypeTable", CnlTypeTable);
                CommLineTable = dict.GetPhrase("CommLineTable", CommLineTable);
                CtrlCnlTable = dict.GetPhrase("CtrlCnlTable", CtrlCnlTable);
                EvTypeTable = dict.GetPhrase("EvTypeTable", EvTypeTable);
                FormatTable = dict.GetPhrase("FormatTable", FormatTable);
                FormulaTable = dict.GetPhrase("FormulaTable", FormulaTable);
                InCnlTable = dict.GetPhrase("InCnlTable", InCnlTable);
                InterfaceTable = dict.GetPhrase("InterfaceTable", InterfaceTable);
                KPTable = dict.GetPhrase("KPTable", KPTable);
                KPTypeTable = dict.GetPhrase("KPTypeTable", KPTypeTable);
                ObjTable = dict.GetPhrase("ObjTable", ObjTable);
                ParamTable = dict.GetPhrase("ParamTable", ParamTable);
                RightTable = dict.GetPhrase("RightTable", RightTable);
                RoleTable = dict.GetPhrase("RoleTable", RoleTable);
                UnitTable = dict.GetPhrase("UnitTable", UnitTable);
                UserTable = dict.GetPhrase("UserTable", UserTable);
                ContinuePendingSvcState = dict.GetPhrase("ContinuePendingSvcState", ContinuePendingSvcState);
                PausedSvcState = dict.GetPhrase("PausedSvcState", PausedSvcState);
                PausePendingSvcState = dict.GetPhrase("PausePendingSvcState", PausePendingSvcState);
                RunningSvcState = dict.GetPhrase("RunningSvcState", RunningSvcState);
                StartPendingSvcState = dict.GetPhrase("StartPendingSvcState", StartPendingSvcState);
                StoppedSvcState = dict.GetPhrase("StoppedSvcState", StoppedSvcState);
                StopPendingSvcState = dict.GetPhrase("StopPendingSvcState", StopPendingSvcState);
                NotInstalledSvcState = dict.GetPhrase("NotInstalledSvcState", NotInstalledSvcState);
            }
        }
    }
}
