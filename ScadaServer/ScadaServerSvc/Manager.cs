/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : SCADA-Server Service
 * Summary  : Program execution management
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Diagnostics;
using Utils;

namespace Scada.Server.Svc
{
    /// <summary>
    /// Program execution management
    /// <para>Управление работой программы</para>
    /// </summary>
    sealed class Manager
    {
        private MainLogic mainLogic; // объект, реализующий логику сервера
        private Log appLog;          // журнал приложения


        /// <summary>
        /// Конструктор
        /// </summary>
        public Manager()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            mainLogic = new MainLogic();
            appLog = mainLogic.AppLog;
        }


        /// <summary>
        /// Вывести информацию о необработанном исключении в журнал
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            appLog.WriteAction(string.Format(Localization.UseRussian ? "Необработанное исключение{0}" :
                "Unhandled exception{0}", ex == null ? "" : ": " + ex.ToString()), Log.ActTypes.Exception);
            appLog.WriteBreak();
        }
        
        
        /// <summary>
        /// Запустить службу
        /// </summary>
        public void StartService()
        {
            // инициализация необходимых директорий
            bool dirsExist;    // необходимые директории существуют
            bool logDirExists; // директория log-файлов существует
            mainLogic.InitAppDirs(out dirsExist, out logDirExists);

            if (logDirExists)
            {
                appLog.WriteBreak();
                appLog.WriteAction(Localization.UseRussian ? "Служба ScadaServerService запущена" :
                    "ScadaServerService is started", Log.ActTypes.Action);
            }

            if (dirsExist)
            {
                // локализация ScadaData.dll
                if (!Localization.UseRussian)
                {
                    string errMsg;
                    if (Localization.LoadDictionaries(mainLogic.LangDir, "ScadaData", out errMsg))
                        CommonPhrases.Init();
                    else
                        appLog.WriteAction(errMsg, Log.ActTypes.Error);
                }

                // запуск работы SCADA-Сервера
                if (!mainLogic.Start())
                    appLog.WriteAction(Localization.UseRussian ? "Нормальная работа программы невозможна." :
                        "Normal program execution is impossible.", Log.ActTypes.Error);
            }
            else
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Не существуют необходимые директории:{0}{1}{0}{2}{0}{3}{0}{4}{0}" +
                    "Нормальная работа программы невозможна." :
                    "Required directories are not exist:{0}{1}{0}{2}{0}{3}{0}{4}{0}" +
                    "Normal program execution is impossible.",
                    Environment.NewLine, mainLogic.ConfigDir, mainLogic.LangDir, mainLogic.LogDir, mainLogic.ModDir);

                try
                {
                    if (EventLog.SourceExists("ScadaServerService"))
                        EventLog.WriteEvent("ScadaServerService",
                            new EventInstance(0, 0, EventLogEntryType.Warning), errMsg);
                }
                catch { }

                if (logDirExists)
                    appLog.WriteAction(errMsg, Log.ActTypes.Error);
            }
        }

        /// <summary>
        /// Остановить службу
        /// </summary>
        public void StopService()
        {
            mainLogic.Stop();
            appLog.WriteAction(Localization.UseRussian ? "Служба ScadaServerService остановлена" :
                "ScadaServerService is stopped", Log.ActTypes.Action);
            appLog.WriteBreak();
        }

        /// <summary>
        /// Отключить службу немедленно при выключении компьютера
        /// </summary>
        public void ShutdownService()
        {
            mainLogic.Stop();
            appLog.WriteAction(Localization.UseRussian ? "Служба ScadaServerService отключена" :
                "ScadaServerService is shutdown", Log.ActTypes.Action);
            appLog.WriteBreak();
        }
    }
}