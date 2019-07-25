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
 * Module   : ScadaAgentEngine
 * Summary  : Implementation of the agent main logic 
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.IO;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Implementation of the agent main logic 
    /// <para>Реализация основной логики агента</para>
    /// </summary>
    public sealed class AgentLogic
    {
        /// <summary>
        /// Состояния работы агента
        /// </summary>
        private enum WorkState
        {
            Undefined = 0,
            Normal = 1,
            Error = 2,
            Terminated = 3
        }

        /// <summary>
        /// Наименования состояний работы на английском
        /// </summary>
        private static readonly string[] WorkStateNamesEn = { "undefined", "normal", "error", "terminated" };
        /// <summary>
        /// Наименования состояний работы на русском
        /// </summary>
        private static readonly string[] WorkStateNamesRu = { "не определено", "норма", "ошибка", "завершён" };

        /// <summary>
        /// Время ожидания остановки потока, мс
        /// </summary>
        private const int WaitForStop = 10000;
        /// <summary>
        /// Период обработки сессий
        /// </summary>
        private static readonly TimeSpan SessProcPeriod = TimeSpan.FromSeconds(5);
        /// <summary>
        /// Период удаления временных файлов
        /// </summary>
        private static readonly TimeSpan DelTempFilePeriod = TimeSpan.FromMinutes(1);
        /// <summary>
        /// Время жизни временных файлов
        /// </summary>
        private static readonly TimeSpan TempFileLifetime = TimeSpan.FromMinutes(10);
        /// <summary>
        /// Период записи в файл информации о работе приложения
        /// </summary>
        private static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromSeconds(1);

        private SessionManager sessionManager; // ссылка на менджер сессий
        private AppDirs appDirs;               // директории приложения
        private ILog log;                      // журнал приложения
        private Thread thread;                 // поток работы агента
        private volatile bool terminated;      // необходимо завершить работу потока
        private string infoFileName;           // полное имя файла информации
        private DateTime utcStartDT;           // дата и время запуска (UTC)
        private DateTime startDT;              // дата и время запуска
        private WorkState workState;           // состояние работы


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private AgentLogic()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public AgentLogic(SessionManager sessionManager, AppDirs appDirs, ILog log)
        {
            this.sessionManager = sessionManager ?? throw new ArgumentNullException("sessionManager");
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.log = log ?? throw new ArgumentNullException("log");

            thread = null;
            terminated = false;
            infoFileName = appDirs.LogDir + AppData.InfoFileName;
            utcStartDT = startDT = DateTime.MinValue;
            workState = WorkState.Undefined;
        }


        /// <summary>
        /// Подготовить обработку логики
        /// </summary>
        private void PrepareProcessing()
        {
            terminated = false;
            utcStartDT = DateTime.UtcNow;
            startDT = utcStartDT.ToLocalTime();
            workState = WorkState.Normal;
            WriteInfo();
        }

        /// <summary>
        /// Удалить устаревшие временные файлы
        /// </summary>
        private void DeleteOutdatedTempFiles()
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                DirectoryInfo dirInfo = new DirectoryInfo(appDirs.TempDir);

                foreach (FileInfo fileInfo in dirInfo.EnumerateFiles())
                {
                    if (utcNow - fileInfo.CreationTimeUtc >= TempFileLifetime)
                    {
                        fileInfo.Delete();
                        log.WriteAction(string.Format(Localization.UseRussian ?
                            "Удалён временный файл {0}" :
                            "Temporary file {0} deleted", fileInfo.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при удалении устаревших временных файлов" :
                    "Error deleting outdated temporary files");
            }
        }

        /// <summary>
        /// Удалить все временные файлы
        /// </summary>
        private void DeleteAllTempFiles()
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(appDirs.TempDir);

                foreach (FileInfo fileInfo in dirInfo.EnumerateFiles())
                {
                    fileInfo.Delete();
                }

                log.WriteAction(Localization.UseRussian ?
                    "Удалены все временные файлы" :
                    "All temporary files deleted");
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при удалении всех временных файлов" :
                    "Error deleting all temporary files");
            }
        }

        /// <summary>
        /// Цикл работы агента (метод вызывается в отдельном потоке)
        /// </summary>
        private void Execute()
        {
            try
            {
                DateTime sessProcDT = DateTime.MinValue;    // время обработки сессий
                DateTime delTempFileDT = DateTime.MinValue; // время удаления временных файлов
                DateTime writeInfoDT = DateTime.MinValue;   // время записи информации о работе приложения

                while (!terminated)
                {
                    try
                    {
                        DateTime utcNow = DateTime.UtcNow;

                        // удаление неактивных сессий
                        if (utcNow - sessProcDT >= SessProcPeriod)
                        {
                            sessProcDT = utcNow;
                            sessionManager.RemoveInactiveSessions();
                        }

                        // удаление устаревших временных файлов
                        if (utcNow - delTempFileDT >= DelTempFilePeriod)
                        {
                            delTempFileDT = utcNow;
                            DeleteOutdatedTempFiles();
                        }

                        // запись информации о работе приложения
                        if (utcNow - writeInfoDT >= WriteInfoPeriod)
                        {
                            writeInfoDT = utcNow;
                            WriteInfo();
                        }

                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, Localization.UseRussian ?
                            "Ошибка в цикле работы агента" :
                            "Error in the agent work cycle");
                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                }
            }
            finally
            {
                sessionManager.RemoveAllSessions();
                DeleteAllTempFiles();

                workState = WorkState.Terminated;
                WriteInfo();
            }
        }

        /// <summary>
        /// Записать в файл информацию о работе приложения
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                // формирование информации
                StringBuilder sbInfo = new StringBuilder();
                TimeSpan workSpan = DateTime.UtcNow - utcStartDT;
                string workSpanStr = workSpan.Days > 0 ? 
                    workSpan.ToString(@"d\.hh\:mm\:ss") :
                    workSpan.ToString(@"hh\:mm\:ss");

                if (Localization.UseRussian)
                {
                    sbInfo
                        .AppendLine("Агент")
                        .AppendLine("-----")
                        .Append("Запуск       : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Время работы : ").AppendLine(workSpanStr)
                        .Append("Состояние    : ").AppendLine(WorkStateNamesRu[(int)workState])
                        .Append("Версия       : ").AppendLine(AgentUtils.AppVersion)
                        .AppendLine()
                        .AppendLine("Активные сессии")
                        .AppendLine("---------------");
                }
                else
                {
                    sbInfo
                        .AppendLine("Agent")
                        .AppendLine("-----")
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workSpanStr)
                        .Append("State          : ").AppendLine(WorkStateNamesEn[(int)workState])
                        .Append("Version        : ").AppendLine(AgentUtils.AppVersion)
                        .AppendLine()
                        .AppendLine("Active Sessions")
                        .AppendLine("---------------");
                }

                sbInfo.Append(sessionManager.GetInfo());

                // запись в файл
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                {
                    writer.Write(sbInfo.ToString());
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при записи в файл информации о работе приложения" :
                    "Error writing application information to the file");
            }
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
                    PrepareProcessing();
                    thread = new Thread(new ThreadStart(Execute));
                    thread.Start();
                }
                else
                {
                    log.WriteAction(Localization.UseRussian ?
                        "Обработка логики уже запущена" :
                        "Logic processing is already started");
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при запуске обработки логики" :
                    "Error starting logic processing");
                return false;
            }
            finally
            {
                if (thread == null)
                {
                    workState = WorkState.Error;
                    WriteInfo();
                }
            }
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
                workState = WorkState.Error;
                WriteInfo();
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при остановке обработки логики" :
                    "Error stopping logic processing");
            }
        }
    }
}
