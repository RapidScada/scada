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
 * Summary  : The common application data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2014
 */

using System;
using System.IO;
using System.Web;
using System.Windows.Forms;
using Scada.Client;
using Utils;
using System.Text;

namespace Scada.Web
{
	/// <summary>
    /// The common application data
    /// <para>Общие данные приложения</para>
	/// </summary>
	public static class AppData
	{
        /// <summary>
        /// Имя файла журнала приложения по умолчанию
        /// </summary>
        public const string DefLogFileName = "ScadaWeb.log";


        private static string dictWriteTimeStr; // время записи в файлы словарей

        
        /// <summary>
        /// Конструктор
        /// </summary>
        static AppData()
		{
            dictWriteTimeStr = "";
            Inited = false;
            Log = new Log(Log.Formats.Full);
            LogFileName = DefLogFileName;
            MainData = new MainData();
            WebSettings = new WebSettings();
        }


        /// <summary>
        /// Получить признак инициализации общих данных приложения
        /// </summary>
        public static bool Inited { get; private set; }
        
        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public static Log Log { get; private set; }

        /// <summary>
        /// Получить объект для работы с данными системы
        /// </summary>
        public static MainData MainData { get; private set; }

        /// <summary>
        /// Получить настройки веб-приложения
        /// </summary>
        public static WebSettings WebSettings { get; private set; }


        /// <summary>
        /// Получить директорию приложения
        /// </summary>
        public static string AppDir { get; private set; }

        /// <summary>
        /// Получить директорию исполняемых файлов
        /// </summary>
        public static string BinDir { get; private set; }

        /// <summary>
        /// Получить директорию конфигурации
        /// </summary>
        public static string ConfigDir { get; private set; }

        /// <summary>
        /// Получить директорию перевода на различные языки
        /// </summary>
        public static string LangDir { get; private set; }

        /// <summary>
        /// Получить директорию журналов
        /// </summary>
        public static string LogDir { get; private set; }

        /// <summary>
        /// Получить или установить имя файла журнала приложения (без директории)
        /// </summary>
        /// <remarks>Имя файла должно устанавливаться до инициализации общих данных приложения</remarks>
        public static string LogFileName { get; set; }

        /// <summary>
        /// Получить директорию веб-страниц отчётов
        /// </summary>
        public static string ReportDir { get; private set; }

        /// <summary>
        /// Получить директорию шаблонов отчётов
        /// </summary>
        public static string TemplateDir { get; private set; }


        /// <summary>
        /// Обновить настройки веб-приложения
        /// </summary>
        private static void RefreshWebSettings()
        {
            string errMsg;
            if (!WebSettings.LoadFromFile(AppData.ConfigDir + WebSettings.DefFileName, out errMsg))
                Log.WriteAction(errMsg, Log.ActTypes.Error);
        }

        /// <summary>
        /// Обновить словари, если файл словарей изменился
        /// </summary>
        private static void RefreshDictionaries()
        {
            if (!Localization.UseRussian)
            {
                DateTime fileWriteTime1 = GetFileWriteTime(Localization.GetDictionaryFileName(LangDir, "ScadaData"));
                DateTime fileWriteTime2 = GetFileWriteTime(Localization.GetDictionaryFileName(LangDir, "ScadaWeb"));
                string writeTimeStr = fileWriteTime1.ToString() + fileWriteTime2.ToString();

                if (dictWriteTimeStr != writeTimeStr)
                {
                    dictWriteTimeStr = writeTimeStr;
                    string errMsg;

                    if (Localization.LoadDictionaries(LangDir, "ScadaData", out errMsg))
                        CommonPhrases.Init();
                    else
                        Log.WriteAction(errMsg, Log.ActTypes.Error);

                    if (Localization.LoadDictionaries(LangDir, "ScadaWeb", out errMsg))
                        WebPhrases.Init();
                    else
                        Log.WriteAction(errMsg, Log.ActTypes.Error);
                }
            }
        }

        /// <summary>
        /// Получить время последней записи в файл
        /// </summary>
        private static DateTime GetFileWriteTime(string fileName)
        {
            try { return File.GetLastWriteTime(fileName); }
            catch { return DateTime.MinValue; }
        }


        /// <summary>
        /// Инициализировать общие данные приложения
        /// </summary>
        public static void InitAppData()
        {
            if (!Inited)
            {
                Inited = true;

                // определение директорий приложения
                if (HttpContext.Current == null)
                {
                    AppDir = ScadaUtils.NormalDir(Path.GetDirectoryName(Application.ExecutablePath));
                    BinDir = AppDir;
                }
                else
                {
                    AppDir = ScadaUtils.NormalDir(HttpContext.Current.Request.PhysicalApplicationPath);
                    BinDir = AppDir + "bin\\";
                }

                ConfigDir = AppDir + "config\\";
                LangDir = AppDir + "lang\\";
                LogDir = AppDir + "log\\";
                ReportDir = AppDir + "report\\";
                TemplateDir = AppDir + "templates\\";

                // настройка объекта для работы с данными системы
                MainData.SettingsFileName = ConfigDir + CommSettings.DefFileName;

                // настройка журнала приложения
                Log.FileName = LogDir + LogFileName;
                Log.Encoding = Encoding.UTF8;
                Log.WriteBreak();
                Log.WriteAction(Localization.UseRussian ? "Инициализация общих данных приложения" : 
                    "Initialize common application data", Log.ActTypes.Action);
            }

            // обновление словарей
            RefreshDictionaries();

            // обновление настроек веб-приложения
            RefreshWebSettings();
        }
    }
}