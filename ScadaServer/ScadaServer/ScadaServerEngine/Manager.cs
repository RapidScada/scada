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
 * Module   : SCADA-Server Service
 * Summary  : Program execution management
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2018
 */

using Scada.Server.Modules;
using System;
using System.IO;
using System.Reflection;
using Utils;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Program execution management
    /// <para>Управление работой программы</para>
    /// </summary>
    public sealed class Manager
    {
        private MainLogic mainLogic; // объект, реализующий логику сервера
        private Log appLog;          // журнал приложения


        /// <summary>
        /// Конструктор
        /// </summary>
        public Manager()
        {
            mainLogic = new MainLogic();
            appLog = mainLogic.AppLog;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Вывести информацию о необработанном исключении в журнал
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            appLog.WriteException(ex, string.Format(Localization.UseRussian ?
                "Необработанное исключение" :
                "Unhandled exception"));
        }
        
        
        /// <summary>
        /// Запустить службу
        /// </summary>
        public void StartService()
        {
            // инициализация объекта логики
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            mainLogic.Init(exeDir);

            appLog.WriteBreak();
            appLog.WriteAction(string.Format(Localization.UseRussian ? 
                "Служба ScadaServerService {0} запущена" :
                "ScadaServerService {0} is started", ServerUtils.AppVersion));

            if (mainLogic.AppDirs.Exist)
            {
                // локализация
                if (Localization.LoadDictionaries(mainLogic.AppDirs.LangDir, "ScadaData", out string errMsg))
                    CommonPhrases.Init();
                else
                    appLog.WriteError(errMsg);

                if (Localization.LoadDictionaries(mainLogic.AppDirs.LangDir, "ScadaServer", out errMsg))
                    ModPhrases.InitFromDictionaries();
                else
                    appLog.WriteError(errMsg);

                // запуск
                if (mainLogic.Start())
                    return;
            }
            else
            {
                appLog.WriteError(string.Format(Localization.UseRussian ?
                    "Необходимые директории не существуют:{0}{1}" :
                    "The required directories do not exist:{0}{1}",
                    Environment.NewLine, string.Join(Environment.NewLine, mainLogic.AppDirs.GetRequiredDirs())));
            }

            appLog.WriteError(Localization.UseRussian ?
                "Нормальная работа программы невозможна" :
                "Normal program execution is impossible");
        }

        /// <summary>
        /// Остановить службу
        /// </summary>
        public void StopService()
        {
            mainLogic.Stop();
            appLog.WriteAction(Localization.UseRussian ? 
                "Служба ScadaServerService остановлена" :
                "ScadaServerService is stopped");
            appLog.WriteBreak();
        }

        /// <summary>
        /// Отключить службу немедленно при выключении компьютера
        /// </summary>
        public void ShutdownService()
        {
            mainLogic.Stop();
            appLog.WriteAction(Localization.UseRussian ? 
                "Служба ScadaServerService отключена" :
                "ScadaServerService is shutdown");
            appLog.WriteBreak();
        }
    }
}
