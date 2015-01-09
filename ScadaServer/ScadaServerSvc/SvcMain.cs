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
 * Module   : SCADA-Server Service
 * Summary  : ScadaServerSvc service implementation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2014
 */

using System;
using System.Diagnostics;
using System.ServiceProcess;
using Utils;

namespace Scada.Server.Svc
{
    /// <summary>
    /// ScadaServerSvc service implementation
    /// <para>Реализация службы ScadaServerSvc</para>
    /// </summary>
    public partial class SvcMain : ServiceBase
    {
        private MainLogic mainLogic; // объект, реализующий логику сервера
        private Log appLog;          // журнал приложения

        public SvcMain()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            mainLogic = new MainLogic();
            appLog = mainLogic.AppLog;
        }

        private void StopWork()
        {
            mainLogic.Stop();
        }

        protected override void OnStart(string[] args)
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
                    "Не существуют необходимые директории:\r\n{0}\r\n{1}\r\n{2}\r\n{3}\r\n" + 
                    "Нормальная работа программы невозможна." :
                    "Required directories are not exist:\r\n{0}\r\n{1}\r\n{2}\r\n{3}\r\n" + 
                    "Normal program execution is impossible.",
                    mainLogic.ConfigDir, mainLogic.LangDir, mainLogic.LogDir, mainLogic.ModDir);

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

        protected override void OnStop()
        {
            StopWork();
            appLog.WriteAction(Localization.UseRussian ? "Служба ScadaServerService остановлена" :
                "ScadaServerService is stopped", Log.ActTypes.Action);
            appLog.WriteBreak();
        }

        protected override void OnShutdown()
        {
            StopWork();
            appLog.WriteAction(Localization.UseRussian ? "Служба ScadaServerService отключена" :
                "ScadaServerService is shutdown", Log.ActTypes.Action);
            appLog.WriteBreak();
        }

        protected void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            appLog.WriteAction(string.Format(Localization.UseRussian ? "Необработанное исключение{0}" : 
                "Unhandled exception{0}", ex == null ? "" : ": " + ex.ToString()), Log.ActTypes.Exception);
            appLog.WriteBreak();
        }
    }
}
