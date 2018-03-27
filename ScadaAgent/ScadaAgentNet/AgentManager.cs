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
 * Module   : ScadaAgentNet
 * Summary  : Agent manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using Utils;

namespace Scada.Agent.Net
{
    /// <summary>
    /// Agent manager
    /// <para>Менеджер агента</para>
    /// </summary>
    public class AgentManager
    {
        private ILog log;                 // журнал приложения
        private AgentLogic agentLogic;    // объект, реализующий основную логику агента
        private ServiceHost agentSvcHost; // хост WCF-службы для взаимодействия с агентом


        /// <summary>
        /// Конструктор
        /// </summary>
        public AgentManager()
        {
            log = new LogStub();
            agentLogic = null;
            agentSvcHost = null;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Вывести информацию о необработанном исключении в журнал
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            log.WriteException(ex, string.Format(Localization.UseRussian ? 
                "Необработанное исключение" :
                "Unhandled exception"));
        }

        /// <summary>
        /// Запустить WCF-службу для взаимодействия с агентом
        /// </summary>
        private bool StartWcfService()
        {
            try
            {
                agentSvcHost = new ServiceHost(typeof(AgentSvc));
                ServiceBehaviorAttribute behavior =
                    agentSvcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.UseSynchronizationContext = false;
                agentSvcHost.Open();
                string serviceUrl = agentSvcHost.BaseAddresses.Count > 0 ?
                    agentSvcHost.BaseAddresses[0].AbsoluteUri : "";

                log.WriteAction(string.Format(Localization.UseRussian ?
                    "WCF-служба запущена по адресу {0}" :
                    "WCF service is started at {0}", serviceUrl));

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при запуске WCF-службы" :
                    "Error starting WCF service");
                return false;
            }
        }

        /// <summary>
        /// Остановить WCF-службу, взаимодействующую с веб-интерфейсом
        /// </summary>
        private void StopWcfService()
        {
            if (agentSvcHost != null)
            {
                try
                {
                    agentSvcHost.Close();
                    log.WriteAction(Localization.UseRussian ?
                        "WCF-служба остановлена" :
                        "WCF service is stopped");
                }
                catch
                {
                    agentSvcHost.Abort();
                    log.WriteAction(Localization.UseRussian ?
                        "WCF-служба прервана" :
                        "WCF service is aborted");
                }

                agentSvcHost = null;
            }
        }
        
        
        /// <summary>
        /// Запустить агента
        /// </summary>
        public void StartAgent()
        {
            // инициализация общих данных
            AppData appData = AppData.GetInstance();
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            appData.Init(exeDir);

            log = appData.Log;
            log.WriteBreak();
            log.WriteAction(string.Format(Localization.UseRussian ?
                "Агент {0} запущен" :
                "Agent {0} started", AgentUtils.AppVersion));


            if (appData.AppDirs.Exist)
            {
                // локализация
                string errMsg;
                if (Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaData", out errMsg))
                    CommonPhrases.Init();
                else
                    log.WriteError(errMsg);

                // запуск
                agentLogic = new AgentLogic(appData.SessionManager, appData.Log);

                if (!(StartWcfService() && agentLogic.StartProcessing()))
                {
                    log.WriteError(Localization.UseRussian ?
                        "Нормальная работа программы невозможна" :
                        "Normal program execution is impossible");
                }
            }
            else
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Необходимые директории не существуют:{0}{1}{0}" +
                    "Нормальная работа программы невозможна" :
                    "The required directories do not exist:{0}{1}{0}" +
                    "Normal program execution is impossible",
                    Environment.NewLine, string.Join(Environment.NewLine, appData.AppDirs.GetRequiredDirs()));

                try
                {
                    if (EventLog.SourceExists("ScadaAgent"))
                        EventLog.WriteEvent("ScadaAgent", new EventInstance(0, 0, EventLogEntryType.Error), errMsg);
                }
                catch { }

                log.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Остановить агента
        /// </summary>
        public void StopAgent()
        {
            StopWcfService();
            agentLogic?.StopProcessing();

            log.WriteAction(Localization.UseRussian ?
                "Агент остановлен" :
                "Agent is stopped");
            log.WriteBreak();
        }
    }
}
