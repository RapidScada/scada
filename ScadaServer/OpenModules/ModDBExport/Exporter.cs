/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : ModDBExport
 * Summary  : Exporter for one export destination
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2017
 * 
 * Description
 * Server module for real time data export from Rapid SCADA to DB.
 */

using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// Exporter for one export destination
    /// <para>Экспортёр для одного назначения экспорта</para>
    /// </summary>
    internal class Exporter
    {
        private const int MaxQueueSize = 100; // максимальный размер очередей экспортируемых данных
        private const int BundleSize = 10;    // количество объектов очереди, экспортируемых за один проход цикла
        private const int ErrorDelay = 1000;  // задержка в случае ошибки экспорта, мс

        private Log log; // журнал работы модуля
        private Queue<SrezTableLight.Srez> curSrezQueue; // очередь экспортируемых текущих срезов
        private Queue<SrezTableLight.Srez> arcSrezQueue; // очередь экспортируемых архивных срезов
        private Queue<EventTableLight.Event> evQueue;    // очередь экспортируемых событий
        private Thread thread;                           // поток работы экспортёра
        private volatile bool terminated;                // необходимо завершить работу потока
        private volatile bool running;                   // поток работает

        // Состояние и статистика
        private bool fatalError;    // фатальная ошибка экспортёра
        private bool exportError;   // ошибка экспорта последнего объекта
        private int expCurSrezCnt;  // количество экспортированных текущих срезов
        private int expArcSrezCnt;  // количество экспортированных архивных срезов
        private int expEvCnt;       // количество экспортированных событий
        private int skipCurSrezCnt; // количество пропущенных текущих срезов
        private int skipArcSrezCnt; // количество пропущенных архивных срезов
        private int skipEvCnt;      // количество пропущенных событий


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private Exporter()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Exporter(Config.ExportDestination expDest, Log log)
        {
            if (expDest == null)
                throw new ArgumentNullException("expDest");

            this.log = log;
            curSrezQueue = new Queue<SrezTableLight.Srez>(MaxQueueSize);
            arcSrezQueue = new Queue<SrezTableLight.Srez>(MaxQueueSize);
            evQueue = new Queue<EventTableLight.Event>(MaxQueueSize);
            thread = null;
            terminated = false;
            running = false;

            ResetStats();

            DataSource = expDest.DataSource;
            ExportParams = expDest.ExportParams;
        }


        /// <summary>
        /// Получить источник данных
        /// </summary>
        public DataSource DataSource { get; private set; }

        /// <summary>
        /// Получить параметры экспорта
        /// </summary>
        public Config.ExportParams ExportParams { get; private set; }

        /// <summary>
        /// Получить признак, что экспортёр работает
        /// </summary>
        public bool Running
        {
            get
            {
                return running;
            }
        }


        /// <summary>
        /// Инициализировать источник данных
        /// </summary>
        private bool InitDataSource()
        {
            try
            {
                DataSource.InitConnection();
                DataSource.InitCommands(
                    ExportParams.ExportCurData ? ExportParams.ExportCurDataQuery : "",
                    ExportParams.ExportArcData ? ExportParams.ExportArcDataQuery : "",
                    ExportParams.ExportEvents ? ExportParams.ExportEventQuery : "");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при инициализации источника данных {0}: {1}" :
                    "Error initializing data source {0}: {1}", DataSource.Name, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Сбросить статистику
        /// </summary>
        private void ResetStats()
        {
            fatalError = false;
            exportError = false;
            expCurSrezCnt = 0;
            expArcSrezCnt = 0;
            expEvCnt = 0;
            skipCurSrezCnt = 0;
            skipArcSrezCnt = 0;
            skipEvCnt = 0;
        }

        /// <summary>
        /// Цикл работы менеждера (метод вызывается в отдельном потоке)
        /// </summary>
        private void Execute()
        {
            try
            {
                while (!terminated)
                {
                    try
                    {
                        // экспорт данных
                        if (Connect())
                        {
                            ExportCurData();
                            ExportArcData();
                            ExportEvents();
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        log.WriteAction(Localization.UseRussian ?
                            "Экспорт прерван. Не все данные экспортированы" :
                            "Export is aborted. Not all data is exported");
                    }
                    finally
                    {
                        Disconnect();
                    }

                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
            finally
            {
                running = false;
            }
        }

        /// <summary>
        /// Соединиться с БД с выводом возможной ошибки в журнал
        /// </summary>
        private bool Connect()
        {
            try
            {
                DataSource.Connect();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ? "Ошибка при соединении с БД {0}: {1}" :
                    "Error connecting to DB {0}: {1}", DataSource.Name, ex.Message));
                exportError = true;
                Thread.Sleep(ErrorDelay);
                return false;
            }
        }

        /// <summary>
        /// Разъединиться с БД с выводом возможной ошибки в журнал
        /// </summary>
        private void Disconnect()
        {
            try
            {
                DataSource.Disconnect();
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ? "Ошибка при разъединении с БД {0}: {1}" :
                    "Error disconnecting from DB {0}: {1}", DataSource.Name, ex.Message));
            }
        }

        /// <summary>
        /// Экспортировать текущие данные
        /// </summary>
        private void ExportCurData()
        {
            if (ExportParams.ExportCurData)
            {
                DbTransaction trans = null;
                SrezTableLight.Srez srez = null;

                try
                {
                    trans = DataSource.Connection.BeginTransaction();
                    DataSource.ExportCurDataCmd.Transaction = trans;

                    for (int i = 0; i < BundleSize; i++)
                    {
                        // извлечение среза из очереди
                        lock (curSrezQueue)
                        {
                            if (curSrezQueue.Count > 0)
                                srez = curSrezQueue.Dequeue();
                            else
                                break;
                        }

                        // экспорт
                        ExportSrez(DataSource.ExportCurDataCmd, srez);

                        expCurSrezCnt++;
                        exportError = false;
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    // возврат среза в очередь
                    if (srez != null)
                    {
                        lock (curSrezQueue)
                            curSrezQueue.Enqueue(srez);
                    }

                    log.WriteAction(string.Format(Localization.UseRussian ?
                        "Ошибка при экспорте текущих данных в БД {0}: {1}" :
                        "Error export current data to DB {0}: {1}", DataSource.Name, ex.Message));
                    exportError = true;
                    Thread.Sleep(ErrorDelay);
                }
            }
        }

        /// <summary>
        /// Экспортировать архивные данные
        /// </summary>
        private void ExportArcData()
        {
            if (ExportParams.ExportArcData)
            {
                DbTransaction trans = null;
                SrezTableLight.Srez srez = null;

                try
                {
                    trans = DataSource.Connection.BeginTransaction();
                    DataSource.ExportArcDataCmd.Transaction = trans;

                    for (int i = 0; i < BundleSize; i++)
                    {
                        // извлечение среза из очереди
                        lock (arcSrezQueue)
                        {
                            if (arcSrezQueue.Count > 0)
                                srez = arcSrezQueue.Dequeue();
                            else
                                break;
                        }

                        // экспорт
                        ExportSrez(DataSource.ExportArcDataCmd, srez);

                        expArcSrezCnt++;
                        exportError = false;
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    // возврат среза в очередь
                    if (srez != null)
                    {
                        lock (arcSrezQueue)
                            arcSrezQueue.Enqueue(srez);
                    }

                    log.WriteAction(string.Format(Localization.UseRussian ?
                        "Ошибка при экспорте архивных данных в БД {0}: {1}" :
                        "Error export archive data to DB {0}: {1}", DataSource.Name, ex.Message));
                    exportError = true;
                    Thread.Sleep(ErrorDelay);
                }
            }
        }

        /// <summary>
        /// Экспортировать события
        /// </summary>
        private void ExportEvents()
        {
            if (ExportParams.ExportEvents)
            {
                DbTransaction trans = null;
                EventTableLight.Event ev = null;

                try
                {
                    trans = DataSource.Connection.BeginTransaction();
                    DataSource.ExportEventCmd.Transaction = trans;

                    for (int i = 0; i < BundleSize; i++)
                    {
                        // извлечение события из очереди
                        lock (evQueue)
                        {
                            if (evQueue.Count > 0)
                                ev = evQueue.Dequeue();
                            else
                                break;
                        }

                        // экспорт
                        ExportEvent(DataSource.ExportEventCmd, ev);

                        expEvCnt++;
                        exportError = false;
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    // возврат события в очередь
                    if (ev != null)
                    {
                        lock (evQueue)
                            evQueue.Enqueue(ev);
                    }

                    log.WriteAction(string.Format(Localization.UseRussian ?
                        "Ошибка при экспорте событий в БД {0}: {1}" :
                        "Error export events to DB {0}: {1}", DataSource.Name, ex.Message));
                    exportError = true;
                    Thread.Sleep(ErrorDelay);
                }
            }
        }
        
        /// <summary>
        /// Экспортировать срез
        /// </summary>
        private void ExportSrez(DbCommand cmd, SrezTableLight.Srez srez)
        {
            DataSource.SetCmdParam(cmd, "dateTime", srez.DateTime);

            foreach (int cnlNum in srez.CnlNums)
            {
                SrezTableLight.CnlData cnlData;
                if (srez.GetCnlData(cnlNum, out cnlData))
                {
                    DataSource.SetCmdParam(cmd, "cnlNum", cnlNum);
                    DataSource.SetCmdParam(cmd, "val", cnlData.Val);
                    DataSource.SetCmdParam(cmd, "stat", cnlData.Stat);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Экспортировать событие
        /// </summary>
        private void ExportEvent(DbCommand cmd, EventTableLight.Event ev)
        {
            DataSource.SetCmdParam(cmd, "dateTime", ev.DateTime);
            DataSource.SetCmdParam(cmd, "objNum", ev.ObjNum);
            DataSource.SetCmdParam(cmd, "kpNum", ev.KPNum);
            DataSource.SetCmdParam(cmd, "paramID", ev.ParamID);
            DataSource.SetCmdParam(cmd, "cnlNum", ev.CnlNum);
            DataSource.SetCmdParam(cmd, "oldCnlVal", ev.OldCnlVal);
            DataSource.SetCmdParam(cmd, "oldCnlStat", ev.OldCnlStat);
            DataSource.SetCmdParam(cmd, "newCnlVal", ev.NewCnlVal);
            DataSource.SetCmdParam(cmd, "newCnlStat", ev.NewCnlStat);
            DataSource.SetCmdParam(cmd, "checked", ev.Checked);
            DataSource.SetCmdParam(cmd, "userID", ev.UserID);
            DataSource.SetCmdParam(cmd, "descr", ev.Descr);
            DataSource.SetCmdParam(cmd, "data", ev.Data);
            cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// Запустить работу экспортёра
        /// </summary>
        public void Start()
        {
            if (InitDataSource())
            {
                ResetStats();
                terminated = false;
                running = true;
                thread = new Thread(new ThreadStart(Execute));
                thread.Start();
            }
            else
            {
                fatalError = true;
            }
        }

        /// <summary>
        /// Начать остановку работы экспортёра
        /// </summary>
        public void Terminate()
        {
            terminated = true;
        }

        /// <summary>
        /// Прервать работу экспортёра
        /// </summary>
        public void Abort()
        {
            if (thread != null)
                thread.Abort();
        }

        /// <summary>
        /// Добавить текущие данные в очередь экспорта
        /// </summary>
        public void EnqueueCurData(SrezTableLight.Srez curSrez)
        {
            lock (curSrezQueue)
            {
                if (curSrezQueue.Count < MaxQueueSize)
                {
                    curSrezQueue.Enqueue(curSrez);
                }
                else
                {
                    skipCurSrezCnt++;
                    log.WriteAction(string.Format(Localization.UseRussian ?
                        "Невозможно добавить в очередь текущие данные. Максимальный размер очереди {0} превышен" :
                        "Unable to enqueue current data. The maximum size of the queue {0} is exceeded",
                        MaxQueueSize));
                }
            }
        }

        /// <summary>
        /// Добавить архивные данные в очередь экспорта
        /// </summary>
        public void EnqueueArcData(SrezTableLight.Srez arcSrez)
        {
            lock (arcSrezQueue)
            {
                if (arcSrezQueue.Count < MaxQueueSize)
                {
                    arcSrezQueue.Enqueue(arcSrez);
                }
                else
                {
                    skipArcSrezCnt++;
                    log.WriteAction(string.Format(Localization.UseRussian ?
                        "Невозможно добавить в очередь архивные данные. Максимальный размер очереди {0} превышен" :
                        "Unable to enqueue archive data. The maximum size of the queue {0} is exceeded",
                        MaxQueueSize));
                }
            }
        }

        /// <summary>
        /// Добавить событие в очередь экспорта
        /// </summary>
        public void EnqueueEvent(EventTableLight.Event ev)
        {
            lock (evQueue)
            {
                if (evQueue.Count < MaxQueueSize)
                {
                    evQueue.Enqueue(ev);
                }
                else
                {
                    skipEvCnt++;
                    log.WriteAction(string.Format(Localization.UseRussian ?
                        "Невозможно добавить в очередь событие. Максимальный размер очереди {0} превышен" :
                        "Unable to enqueue an event. The maximum size of the queue {0} is exceeded",
                        MaxQueueSize));
                }
            }
        }

        /// <summary>
        /// Получить информацию о работе экспортёра
        /// </summary>
        public string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder(DataSource.Name);
            string stateStr;

            // получение длин очередей
            int curSrezQueueCnt;
            lock (curSrezQueue)
                curSrezQueueCnt = curSrezQueue.Count;

            int arcSrezQueueCnt;
            lock (arcSrezQueue)
                arcSrezQueueCnt = arcSrezQueue.Count;

            int evQueueCnt;
            lock (evQueue)
                evQueueCnt = evQueue.Count;

            // формирование текста
            if (Localization.UseRussian)
            {
                if (fatalError)
                    stateStr = "фатальная ошибка";
                else if (exportError)
                    stateStr = "ошибка экспорта";
                else
                    stateStr = "норма";

                sbInfo
                    .Append("; состояние: ").Append(stateStr)
                    .Append("; в очереди тек/арх/соб: ")
                    .Append(curSrezQueueCnt).Append("/").Append(arcSrezQueueCnt).Append("/").Append(evQueueCnt)
                    .Append("; экспортировано тек/арх/соб: ")
                    .Append(expCurSrezCnt).Append("/").Append(expArcSrezCnt).Append("/").Append(expEvCnt)
                    .Append("; пропущено тек/арх/соб: ")
                    .Append(skipCurSrezCnt).Append("/").Append(skipArcSrezCnt).Append("/").Append(skipEvCnt);
            }
            else
            {
                if (fatalError)
                    stateStr = "fatal error";
                else if (exportError)
                    stateStr = "export error";
                else
                    stateStr = "normal";

                sbInfo
                    .Append("; state: ").Append(stateStr)
                    .Append("; in queue cur/arc/ev: ")
                    .Append(curSrezQueueCnt).Append("/").Append(arcSrezQueueCnt).Append("/").Append(evQueueCnt)
                    .Append("; exported cur/arc/ev: ")
                    .Append(expCurSrezCnt).Append("/").Append(expArcSrezCnt).Append("/").Append(expEvCnt)
                    .Append("; skipped cur/arc/ev: ")
                    .Append(skipCurSrezCnt).Append("/").Append(skipArcSrezCnt).Append("/").Append(skipEvCnt);
            }

            return sbInfo.ToString();
        }
    }
}
