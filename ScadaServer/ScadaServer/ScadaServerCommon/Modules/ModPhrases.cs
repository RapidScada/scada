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
 * Module   : ScadaServerCommon
 * Summary  : The phrases used in server modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2018
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Server.Modules
{
    /// <summary>
    /// The phrases used in server modules
    /// <para>Фразы, используемые серверными модулями</para>
    /// </summary>
    public static class ModPhrases
    {
        static ModPhrases()
        {
            SetToDefault();
            InitOnLocalization();
        }

        // Словарь Scada.Server.Modules
        public static string SaveModSettingsConfirm { get; private set; }
        public static string LoadModSettingsError { get; private set; }
        public static string SaveModSettingsError { get; private set; }
        public static string ConfigureModule { get; private set; }
        public static string CmdSentSuccessfully { get; private set; }

        // Словарь Scada.Server.Modules.ModFactory
        public static string GetViewTypeError { get; private set; }
        public static string CreateViewError { get; private set; }

        // Фразы, устанавливаемые в зависимости от локализации, не загружая из словаря
        public static string StartModule { get; private set; }
        public static string StopModule { get; private set; }
        public static string NormalModExecImpossible { get; private set; }
        public static string WriteInfoError { get; private set; }
        public static string IllegalCommand { get; private set; }
        public static string IncorrectCmdData { get; private set; }

        private static void SetToDefault()
        {
            SaveModSettingsConfirm = Localization.Dict.GetEmptyPhrase("SaveModSettingsConfirm");
            LoadModSettingsError = Localization.Dict.GetEmptyPhrase("LoadModSettingsError");
            SaveModSettingsError = Localization.Dict.GetEmptyPhrase("SaveModSettingsError");
            ConfigureModule = Localization.Dict.GetEmptyPhrase("ConfigureModule");

            GetViewTypeError = Localization.Dict.GetEmptyPhrase("GetViewTypeError");
            CreateViewError = Localization.Dict.GetEmptyPhrase("CreateViewError");
        }

        private static void InitOnLocalization()
        {
            if (Localization.UseRussian)
            {
                StartModule = "Запуск работы модуля {0}";
                StopModule = "Работа модуля {0} завершена";
                NormalModExecImpossible = "Нормальная работа модуля невозможна";
                WriteInfoError = "Ошибка при записи в файл информации о работе модуля";
                IllegalCommand = "Недопустимая команда";
                IncorrectCmdData = "Некорректные данные команды";
            }
            else
            {
                StartModule = "Start {0} module";
                StopModule = "Module {0} is stopped";
                NormalModExecImpossible = "Normal module execution is impossible";
                WriteInfoError = "Error writing module information to the file";
                IllegalCommand = "Illegal command";
                IncorrectCmdData = "Incorrect command data";
            }
        }

        public static void InitFromDictionaries()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Server.Modules", out dict))
            {
                SaveModSettingsConfirm = dict.GetPhrase("SaveModSettingsConfirm", SaveModSettingsConfirm);
                LoadModSettingsError = dict.GetPhrase("LoadModSettingsError", LoadModSettingsError);
                SaveModSettingsError = dict.GetPhrase("SaveModSettingsError", SaveModSettingsError);
                ConfigureModule = dict.GetPhrase("ConfigureModule", ConfigureModule);
                CmdSentSuccessfully = dict.GetPhrase("CmdSentSuccessfully", CmdSentSuccessfully);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Server.Modules.ModFactory", out dict))
            {
                GetViewTypeError = dict.GetPhrase("GetViewTypeError", GetViewTypeError);
                CreateViewError = dict.GetPhrase("CreateViewError", CreateViewError);
            }
        }
    }
}
