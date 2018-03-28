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
 * Module   : ScadaAgentCore
 * Summary  : Implementation of the agent main logic 
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Agent
{
    /// <summary>
    /// Implementation of the agent main logic 
    /// <para>Реализация основной логики агента</para>
    /// </summary>
    public sealed class AgentLogic
    {
        /// <summary>
        /// Время ожидания остановки потока, мс
        /// </summary>
        private const int WaitForStop = 10000;
        /// <summary>
        /// Период обработки сессий
        /// </summary>
        private static readonly TimeSpan SessProcPeriod = TimeSpan.FromSeconds(5);
        /// <summary>
        /// Период записи в файл информации о работе приложения
        /// </summary>
        private static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromSeconds(1);

        private SessionManager sessionManager; // ссылка на менджер сессий
        private ILog log;                      // журнал приложения
        private Thread thread;                 // поток работы сервера
        private volatile bool terminated;      // необходимо завершить работу потока


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private AgentLogic()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public AgentLogic(SessionManager sessionManager, ILog log)
        {
            this.sessionManager = sessionManager ?? throw new ArgumentNullException("sessionManager");
            this.log = log ?? throw new ArgumentNullException("log");
            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Цикл работы агента (метод вызывается в отдельном потоке)
        /// </summary>
        private void Execute()
        {
            try
            {
                DateTime sessProcDT = DateTime.MinValue; // время обработки сессий
                DateTime writeInfoDT = DateTime.MinValue; // время записи информации о работе приложения

                while (!terminated)
                {
                    DateTime utcNow = DateTime.UtcNow;

                    // удаление неактивных сессий
                    if (utcNow - sessProcDT >= SessProcPeriod)
                    {
                        sessProcDT = utcNow;
                        sessionManager.RemoveInactiveSessions();
                    }

                    // запись информации о работе приложения
                    if (utcNow - writeInfoDT >= WriteInfoPeriod)
                    {
                        writeInfoDT = utcNow;
                        WriteInfo();
                    }

                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
            finally
            {
                WriteInfo();
            }
        }

        /// <summary>
        /// Записать в файл информацию о работе приложения
        /// </summary>
        private void WriteInfo()
        {
        }


        /// <summary>
        /// Запустить обработку логики
        /// </summary>
        public bool StartProcessing()
        {
            try
            {
                if (thread == null)
                {
                    log.WriteAction(Localization.UseRussian ?
                        "Запуск обработки логики" :
                        "Start logic processing");
                    terminated = false;
                    thread = new Thread(new ThreadStart(Execute));
                    thread.Start();
                }
                else
                {
                    log.WriteAction(Localization.UseRussian ?
                        "Обработка логики уже запущена" :
                        "Logic processing is already started");
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при запуске обработки логики" :
                    "Error starting logic processing");
            }
            finally
            {
                if (thread == null)
                {
                    //workState = WorkStateNames.Error;
                    //WriteInfo();
                }
            }

            return true;
        }

        /// <summary>
        /// Остановить обработку логики
        /// </summary>
        public void StopProcessing()
        {
            try
            {
                if (thread != null)
                {
                    terminated = true;

                    if (thread.Join(WaitForStop))
                    {
                        log.WriteAction(Localization.UseRussian ?
                            "Обработка логики остановлена" :
                            "Logic processing is stopped");
                    }
                    else
                    {
                        thread.Abort();
                        log.WriteAction(Localization.UseRussian ?
                            "Обработка логики прервана" :
                            "Logic processing is aborted");
                    }

                    thread = null;
                }
            }
            catch (Exception ex)
            {
                //workState = WorkStateNames.Error;
                //WriteInfo();
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при остановке обработки логики" :
                    "Error stopping logic processing");
            }
        }
    }
}
