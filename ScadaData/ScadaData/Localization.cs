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
 * Module   : ScadaData
 * Summary  : Localization mechanism
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Scada
{
    /// <summary>
    /// Localization mechanism
    /// <para>Механизм локализации</para>
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// Словарь
        /// </summary>
        public class Dict
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            private Dict()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Dict(string key)
            {
                Key = key;
                Phrases = new Dictionary<string, string>();
            }

            /// <summary>
            /// Получить ключ словаря
            /// </summary>
            public string Key { get; private set; }
            /// <summary>
            /// Получить фразы, содержащиеся в словаре, по их ключам
            /// </summary>
            public Dictionary<string, string> Phrases { get; private set; }

            /// <summary>
            /// Получить имя файла словаря для заданной культуры
            /// </summary>
            public static string GetFileName(string directory, string fileNamePrefix, string cultureName)
            {
                return ScadaUtils.NormalDir(directory) + 
                    fileNamePrefix + (string.IsNullOrEmpty(cultureName) ? "" : "." + cultureName) +  ".xml";
            }
            /// <summary>
            /// Получить фразу из словаря по ключу или пустую фразу при её отсутствии
            /// </summary>
            public string GetPhrase(string key)
            {
                return Phrases.ContainsKey(key) ? Phrases[key] : GetEmptyPhrase(key);
            }
            /// <summary>
            /// Получить фразу из словаря по ключу или значение по умолчанию при её отсутствии
            /// </summary>
            public string GetPhrase(string key, string defaultVal)
            {
                return Phrases.ContainsKey(key) ? Phrases[key] : defaultVal;
            }
            /// <summary>
            /// Получить пустую фразу для заданного ключа
            /// </summary>
            public static string GetEmptyPhrase(string key)
            {
                return "[" + key + "]";
            }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        static Localization()
        {
            InitDefaultCulture();
            SetCulture(ReadCulture());
            Dictionaries = new Dictionary<string, Dict>();
        }


        /// <summary>
        /// Получить наименование культуры по умолчанию
        /// </summary>
        public static string DefaultCultureName { get; private set; }

        /// <summary>
        /// Получить информацию о культуре по умолчанию
        /// </summary>
        public static CultureInfo DefaultCulture { get; private set; }

        /// <summary>
        /// Получить информацию о культуре всех приложений SCADA
        /// </summary>
        public static CultureInfo Culture { get; private set; }

        /// <summary>
        /// Получить признак использования русской локализации
        /// </summary>
        public static bool UseRussian { get; private set; }

        /// <summary>
        /// Получить загруженные словари для локализации
        /// </summary>
        public static Dictionary<string, Dict> Dictionaries { get; private set; }

        /// <summary>
        /// Получить признак, что запись дня должна располагаться после записи месяца
        /// </summary>
        public static bool DayAfterMonth
        {
            get
            {
                string pattern = Localization.Culture.DateTimeFormat.ShortDatePattern.ToLowerInvariant();
                return pattern.IndexOf('m') < pattern.IndexOf('d');
            }
        }


        /// <summary>
        /// Инициализировать наименование и культуру по умолчанию
        /// </summary>
        private static void InitDefaultCulture()
        {
            try
            {
                DefaultCultureName = CultureIsRussian(CultureInfo.CurrentCulture) ? "ru-RU" : "en-GB";
                DefaultCulture = CultureInfo.GetCultureInfo(DefaultCultureName);
            }
            catch
            {
                DefaultCultureName = "";
                DefaultCulture = CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Считать наименование культуры из реестра
        /// </summary>
        private static string ReadCulture()
        {
            try
            {
#if NETSTANDARD2_0
                return "";
#else
                using (Microsoft.Win32.RegistryKey key = 
                    Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, 
                    Microsoft.Win32.RegistryView.Registry64)
                    .OpenSubKey("Software\\SCADA", false))
                {
                    return key.GetValue("Culture").ToString();
                }
#endif
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Установить культуру
        /// </summary>
        public static void SetCulture(string cultureName)
        {
            try
            {
                Culture = string.IsNullOrEmpty(cultureName) ?
                   DefaultCulture : CultureInfo.GetCultureInfo(cultureName);
            }
            catch
            {
                Culture = DefaultCulture;
            }
            finally
            {
                UseRussian = CultureIsRussian(Culture);
            }
        }

        /// <summary>
        /// Проверить, что наименование культуры соответствует русской культуре
        /// </summary>
        private static bool CultureIsRussian(CultureInfo cultureInfo)
        {
            return cultureInfo.Name == "ru" || cultureInfo.Name.StartsWith("ru-", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Изменить культуру
        /// </summary>
        public static void ChangeCulture(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
                cultureName = ReadCulture();
            SetCulture(cultureName);
        }

        /// <summary>
        /// Записать наименование культуры в реестр
        /// </summary>
        public static bool WriteCulture(string cultureName, out string errMsg)
        {
            try
            {
#if !NETSTANDARD2_0
                using (Microsoft.Win32.RegistryKey key =
                    Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,
                    Microsoft.Win32.RegistryView.Registry64).
                    CreateSubKey("Software\\SCADA"))
                {
                    key.SetValue("Culture", cultureName);
                }
#endif
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = (UseRussian ? "Ошибка при записи информации о культуре в реестр: " : 
                    "Error writing culture info to the registry: ") + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Получить имя файла словаря в зависимости от культуры SCADA
        /// </summary>
        public static string GetDictionaryFileName(string directory, string fileNamePrefix)
        {
            return Dict.GetFileName(directory, fileNamePrefix, Culture.Name);
        }

        /// <summary>
        /// Загрузить словари для культуры SCADA
        /// </summary>
        public static bool LoadDictionaries(string directory, string fileNamePrefix, out string errMsg)
        {
            string fileName = GetDictionaryFileName(directory, fileNamePrefix);
            return LoadDictionaries(fileName, out errMsg);
        }

        /// <summary>
        /// Загрузить словари для культуры SCADA с возможностью загрузки словарей по умолчанию в случае ошибки
        /// </summary>
        public static bool LoadDictionaries(string directory, string fileNamePrefix, bool defaultOnError, out string errMsg)
        {
            string fileName = GetDictionaryFileName(directory, fileNamePrefix);

            if (LoadDictionaries(fileName, out errMsg))
            {
                return true;
            }
            else if (defaultOnError)
            {
                fileName = Dict.GetFileName(directory, fileNamePrefix, DefaultCultureName);
                string errMsg2;
                LoadDictionaries(fileName, out errMsg2);
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Загрузить словари для культуры SCADA
        /// </summary>
        /// <remarks>Если ключ загружаемого словаря совпадает с ключом уже загруженного, то словари сливаются.
        /// Если совпадают ключи фраз, то новое значение фразы записывается поверх старого</remarks>
        public static bool LoadDictionaries(string fileName, out string errMsg)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);

                    XmlNodeList dictNodeList = xmlDoc.DocumentElement.SelectNodes("Dictionary");
                    foreach (XmlElement dictElem in dictNodeList)
                    {
                        Dict dict;
                        string dictKey = dictElem.GetAttribute("key");

                        if (!Dictionaries.TryGetValue(dictKey, out dict))
                        {
                            dict = new Dict(dictKey);
                            Dictionaries.Add(dictKey, dict);
                        }

                        XmlNodeList phraseNodeList = dictElem.SelectNodes("Phrase");
                        foreach (XmlElement phraseElem in phraseNodeList)
                        {
                            string phraseKey = phraseElem.GetAttribute("key");
                            dict.Phrases[phraseKey] = phraseElem.InnerText;
                        }
                    }

                    errMsg = "";
                    return true;
                }
                catch (Exception ex)
                {
                    errMsg = string.Format(UseRussian ? 
                        "Ошибка при загрузке словарей из файла {0}: {1}" : 
                        "Error loading dictionaries from file {0}: {1}", fileName, ex.Message);
                    return false;
                }
            }
            else
            {
                errMsg = (UseRussian ? 
                    "Не найден файл словарей: " : 
                    "Dictionary file not found: ") + fileName;
                return false;
            }
        }

        /// <summary>
        /// Получить словарь по ключу или пустой словарь при его отсутствии
        /// </summary>
        public static Dict GetDictionary(string key)
        {
            return Dictionaries.TryGetValue(key, out Dict dict) ? dict : new Dict(key);
        }


        /// <summary>
        /// Преобразовать дату и время в строку в соответствии с культурой SCADA
        /// </summary>
        public static string ToLocalizedString(this DateTime dateTime)
        {
            return dateTime.ToString("d", Culture) + " " + dateTime.ToString("T", Culture);
        }

        /// <summary>
        /// Преобразовать дату в строку в соответствии с культурой SCADA
        /// </summary>
        public static string ToLocalizedDateString(this DateTime dateTime)
        {
            return dateTime.ToString("d", Culture);
        }

        /// <summary>
        /// Преобразовать время в строку в соответствии с культурой SCADA
        /// </summary>
        public static string ToLocalizedTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("T", Culture);
        }
    }
}