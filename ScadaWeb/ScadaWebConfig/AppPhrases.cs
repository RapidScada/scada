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
 * Module   : SCADA-Web Configurator
 * Summary  : The phrases used in the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2014
 */

using Scada;

namespace ScadaWebConfig
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

        public static string ChooseConfigDir { get; private set; }
        public static string ChooseViewFile { get; private set; }
        public static string FileFilter { get; private set; }
        public static string ChooseSetToAddView { get; private set; }
        public static string ChooseViewToDelete { get; private set; }
        public static string ChooseViewToMove { get; private set; }
        public static string NewViewSetName { get; private set; }
        public static string NewViewTitle { get; private set; }
        public static string LoadConfigDirError { get; private set; }
        public static string SaveConfigDirError { get; private set; }
        public static string ParseViewTitleError { get; private set; }        

        private static void SetToDefault()
        {
            ChooseConfigDir = "Выберите директорию конфигурации";
            ChooseViewFile = "Выберите файл представления";
            FileFilter = "Представления (*.tbl;*.ofm;*.sch;*.fcs)|*.tbl;*.ofm;*.sch;*.fcs|" + 
                "Веб-страницы (*.aspx;*.htm;*.html)|*.aspx;*.htm;*.html|Все файлы (*.*)|*.*";
            ChooseSetToAddView = "Выберите набор для добавления представления.";
            ChooseViewToDelete = "Выберите удаляемый набор или представление.";
            ChooseViewToMove = "Выберите перемещаемый набор или представление.";
            NewViewSetName = "Новый набор представлений";
            NewViewTitle = "Новое представление";
            LoadConfigDirError = "Ошибка при загрузке директории конфигурации из файла";
            SaveConfigDirError = "Ошибка при сохранении директории конфигурации в файле";
            ParseViewTitleError = "Ошибка при распознавании заголовка представления";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("ScadaWebConfig.FrmMain", out dict))
            {
                ChooseConfigDir = dict.GetPhrase("ChooseConfigDir", ChooseConfigDir);
                ChooseViewFile = dict.GetPhrase("ChooseViewFile", ChooseViewFile);
                FileFilter = dict.GetPhrase("FileFilter", FileFilter);
                ChooseSetToAddView = dict.GetPhrase("ChooseSetToAddView", ChooseSetToAddView);
                ChooseViewToDelete = dict.GetPhrase("ChooseViewToDelete", ChooseViewToDelete);
                ChooseViewToMove = dict.GetPhrase("ChooseViewToMove", ChooseViewToMove);
                NewViewSetName = dict.GetPhrase("NewViewSetName", NewViewSetName);
                NewViewTitle = dict.GetPhrase("NewViewTitle", NewViewTitle);
                LoadConfigDirError = dict.GetPhrase("LoadConfigDirError", LoadConfigDirError);
                SaveConfigDirError = dict.GetPhrase("SaveConfigDirError", SaveConfigDirError);
                ParseViewTitleError = dict.GetPhrase("ParseViewTitleError", ParseViewTitleError);
            }
        }
    }
}
