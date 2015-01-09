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
 * Module   : SCADA-Communicator Service
 * Summary  : ScadaCommSvc service implementation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2014
 */

using System;
using System.Diagnostics;
using System.ServiceProcess;
using Utils;

namespace Scada.Comm.Svc
{
    /// <summary>
    /// ScadaCommSvc service implementation
    /// <para>Реализация службы ScadaCommSvc</para>
    /// </summary>
    public partial class SvcMain : ServiceBase
    {
        private Manager mngr; // менеджер, управляющий работой приложения
        private Log appLog;   // журнал приложения

        public SvcMain()
        {
            InitializeComponent();
            
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            mngr = new Manager();
            appLog = mngr.Log;
        }

        protected override void OnStart(string[] args)
        {
            // инициализация необходимых директорий
            bool dirsExist;    // необходимые директории существуют
            bool logDirExists; // директория log-файлов существует
            mngr.InitAppDirs(out dirsExist, out logDirExists);

            if (logDirExists)
            {
                appLog.WriteBreak();
                appLog.WriteAction(Localization.UseRussian ? "Служба ScadaCommService запущена" :
                    "ScadaCommService is started", Log.ActTypes.Action);
            }

            if (dirsExist)
            {
                // локализация ScadaData.dll
                if (!Localization.UseRussian)
                {
                    string errMsg;
                    if (Localization.LoadDictionaries(Manager.LangDir, "ScadaData", out errMsg))
                        CommonPhrases.Init();
                    else
                        appLog.WriteAction(errMsg, Log.ActTypes.Error);
                }

                // считывание конфигурации и запуск потоков
                if (mngr.ParseConfig())
                    mngr.StartThreads();
                else
                    appLog.WriteAction(Localization.UseRussian ? "Нормальная работа программы невозможна." :
                        "Normal program execution is impossible.", Log.ActTypes.Error);
            }
            else
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Не существуют необходимые директории:\r\n{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n" +
                    "Нормальная работа программы невозможна." :
                    "Required directories are not exist:\r\n{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n" +
                    "Normal program execution is impossible.",
                    Manager.ConfigDir, Manager.LangDir, Manager.LogDir, Manager.KpDir, Manager.CmdDir);

                try
                {
                    if (EventLog.SourceExists("ScadaCommService"))
                        EventLog.WriteEvent("ScadaCommService", 
                            new EventInstance(0, 0, EventLogEntryType.Warning), errMsg);
                }
                catch { }

                if (logDirExists)
                    appLog.WriteAction(errMsg, Log.ActTypes.Error);
            }
        }

        protected override void OnStop()
        {
            mngr.StopThreads();
            appLog.WriteAction(Localization.UseRussian ? "Служба ScadaCommService остановлена" :
                "ScadaCommService is stopped", Log.ActTypes.Action);
            appLog.WriteBreak();
        }

        protected override void OnShutdown()
        {
            mngr.StopThreads();
            appLog.WriteAction(Localization.UseRussian ? "Служба ScadaCommService отключена" :
                "ScadaCommService is shutdown", Log.ActTypes.Action);
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
