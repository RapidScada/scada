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
 * Module   : ScadaSchemeCommon
 * Summary  : The phrases used by the the SCADA-Scheme modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2014
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Scheme
{
    /// <summary>
    /// The phrases used by the the SCADA-Scheme modules
    /// <para>Общие фразы, используемые модулями SCADA-Схема</para>
    /// </summary>
    public class SchemePhrases
    {
        static SchemePhrases()
        {
            SetToDefaultStatic();
        }

        public SchemePhrases()
        {
            SetToDefault();
        }

        // Фразы, используемые Silverlight-приложением, передающиеся с помощью WCF
        public string Loading { get; set; }
        public string SchemeNotLoaded { get; set; }
        public string ErrorDrawingScheme { get; set; }
        public string WcfError { get; set; }
        public string UnableGetSettings { get; set; }
        public string UnableLoadScheme { get; set; }
        public string UnableLoadCnlData { get; set; }
        public string ErrorExecutingAction { get; set; }
        public string WcfAddrUndefined { get; set; }

        // Общие фразы, не передающиеся с помощью WCF
        // Словарь Scada.Scheme.Editor.FrmMain
        public static string EditorFormTitle { get; private set; }
        public static string FileFilter { get; private set; }
        public static string SaveConfirm { get; private set; }

        // Словарь Scada.Scheme.EditorData
        public static string LoadSchemeError { get; private set; }
        public static string SaveSchemeError { get; private set; }
        public static string FileNameUndefined { get; private set; }

        // Словарь Scada.Scheme.FrmFontDialog
        public static string SizeInteger { get; private set; }

        // Словарь FrmImageDialog
        public static string LoadImageError { get; private set; }
        public static string SaveImageError { get; private set; }

        // Словарь Scada.Scheme.SchemeView
        public static string StringConvertError { get; private set; }
        public static string NameUniqueError { get; private set; }
        
        private void SetToDefault()
        {
            Loading = "Загрузка...";
            SchemeNotLoaded = "Схема не загружена";
            ErrorDrawingScheme = "Ошибка при отображении схемы";
            WcfError = "Ошибка при работе WCF-службы";
            UnableGetSettings = "Не удалось получить настройки приложения";
            UnableLoadScheme = "Не удалось загрузить схему";
            UnableLoadCnlData = "Не удалось загрузить данные входных каналов";
            ErrorExecutingAction = "Ошибка при вызове действия";
            WcfAddrUndefined = "Не задан адрес WCF-службы";
        }

        private static void SetToDefaultStatic()
        {
            EditorFormTitle = "SCADA-Редактор схем";
            FileFilter = "Схемы (*.sch)|*.sch|Все файлы (*.*)|*.*";
            SaveConfirm = "Схема была изменена. Сохранить изменения?";

            LoadSchemeError = "Ошибка при загрузке схемы из файла";
            SaveSchemeError = "Ошибка при сохранении схемы в файле";
            FileNameUndefined = "Имя файла не определено.";

            SizeInteger = "Размер должен быть целым числом.";

            LoadImageError = "Ошибка при загрузке изображения";
            SaveImageError = "Ошибка при сохранении изображения";

            StringConvertError = "Невозможно преобразовать строку";
            NameUniqueError = "Наименование должно быть уникальным.";
        }
        
        public void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.MainPage", out dict))
            {
                Loading = dict.GetPhrase("Loading", Loading);
                SchemeNotLoaded = dict.GetPhrase("SchemeNotLoaded", SchemeNotLoaded);
                ErrorDrawingScheme = dict.GetPhrase("ErrorDrawingScheme", ErrorDrawingScheme);
                WcfError = dict.GetPhrase("WcfError", WcfError);
                UnableGetSettings = dict.GetPhrase("UnableGetSettings", UnableGetSettings);
                UnableLoadScheme = dict.GetPhrase("UnableLoadScheme", UnableLoadScheme);
                UnableLoadCnlData = dict.GetPhrase("UnableLoadCnlData", UnableLoadCnlData);
                ErrorExecutingAction = dict.GetPhrase("ErrorExecutingAction", ErrorExecutingAction);
                WcfAddrUndefined = dict.GetPhrase("WcfAddrUndefined", WcfAddrUndefined);
            }
        }

        public static void InitStatic()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.Editor.FrmMain", out dict))
            {
                EditorFormTitle = dict.GetPhrase("this", EditorFormTitle);
                FileFilter = dict.GetPhrase("FileFilter", FileFilter);
                SaveConfirm = dict.GetPhrase("SaveConfirm", SaveConfirm);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.EditorData", out dict))
            {
                LoadSchemeError = dict.GetPhrase("LoadSchemeError", LoadSchemeError);
                SaveSchemeError = dict.GetPhrase("SaveSchemeError", SaveSchemeError);
                FileNameUndefined = dict.GetPhrase("FileNameUndefined", FileNameUndefined);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.FrmFontDialog", out dict))
                SizeInteger = dict.GetPhrase("SizeInteger", SizeInteger);

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.FrmImageDialog", out dict))
            {
                LoadImageError = dict.GetPhrase("LoadImageError", LoadImageError);
                SaveImageError = dict.GetPhrase("SaveImageError", SaveImageError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.SchemeView", out dict))
            {
                StringConvertError = dict.GetPhrase("StringConvertError", StringConvertError);
                NameUniqueError = dict.GetPhrase("NameUniqueError", NameUniqueError);
            }
        }
    }
}
