/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ModDbExport
 * Summary  : Exports data to a one target
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Db;
using Scada.Server.Modules.DbExport.Config;
using Scada.Server.Modules.DbExport.Triggers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Utils;

namespace Scada.Server.Modules.DbExport
{
    /// <summary>
    /// Exports data to a one target.
    /// <para>Экспортирует данные в одну базу данных.</para>
    /// </summary>
    internal class Exporter
    {
        /// <summary>
        /// Represents queue statistics.
        /// </summary>
        private class QueueStats
        {
            public bool ErrorState { get; set; }
            public int ExportedItems { get; set; }
            public int SkippedItems { get; set; }
        }

        /// <summary>
        /// Specifies the connection statuses.
        /// </summary>
        private enum ConnStatus { Undefined, Normal, Error }

        /// <summary>
        /// The prefix of log files and state file.
        /// </summary>
        private const string FilePrefix = "ModDbExport";
        /// <summary>
        /// The minimum queue size.
        /// </summary>
        private const int MinQueueSize = 100;
        /// <summary>
        /// The number of queue items transferred in a single loop iteration.
        /// </summary>
        private const int BundleSize = 100;
        /// <summary>
        /// The delay in case of a database error, in milliseconds.
        /// </summary>
        private const int ErrorDelay = 1000;
        /// <summary>
        /// The period of writing information.
        /// </summary>
        private static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromMilliseconds(1000);
        /// <summary>
        /// The period of checking the archive for changes.
        /// </summary>
        private static readonly TimeSpan CheckArcPeriod = TimeSpan.FromSeconds(10);
        /// <summary>
        /// The connection status names in English.
        /// </summary>
        private static readonly string[] ConnStatusNamesEn = { "Undefined", "Normal", "Error" };
        /// <summary>
        /// The connection status names in Russian.
        /// </summary>
        private static readonly string[] ConnStatusNamesRu = { "не определено", "норма", "ошибка" };

        private readonly ExportTargetConfig exporterConfig;   // the exporter configuration
        private readonly EntityMap entityMap;                 // the map of channels and devices
        private readonly IServerData serverData;              // provides access to server data
        private readonly string arcDir;                       // the DAT archive directory
        private readonly TimeSpan dataLifetime;               // the data lifetime in the queue
        private readonly Log log;                             // the exporter log
        private readonly string infoFileName;                 // the information file name
        private readonly string stateFileName;                // the state file name
        private readonly string exporterTitle;                // the exporter ID and name
        private readonly DataSource dataSource;               // provides access to the DB
        private readonly ClassifiedTriggers triggers;         // the triggers separated by classes

        private Thread thread;            // the exporter thread
        private volatile bool terminated; // it is required to stop the thread
        private ConnStatus connStatus;    // the connection status

        private int maxQueueSize;                                    // the maximum queue size
        private Queue<QueueItem<SrezTableLight.Srez>> curDataQueue;  // the queue of current data
        private Queue<QueueItem<SrezTableLight.Srez>> arcDataQueue;  // the queue of archive data
        private Queue<QueueItem<EventTableLight.Event>> eventQueue;  // the queue of events
        private QueueStats curDataStats;        // the statistics of current data queue
        private QueueStats arcDataStats;        // the statistics of archive data queue
        private QueueStats eventStats;          // the statistics of event queue

        private ArcUploadState arcUploadState;  // the state of uploading archives
        private ArcUploadState taskUploadState; // the state of uploading archives according to the task


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Exporter(ExportTargetConfig exporterConfig, EntityMap entityMap, 
            IServerData serverData, AppDirs appDirs, string arcDir)
        {
            this.exporterConfig = exporterConfig ?? throw new ArgumentNullException(nameof(exporterConfig));
            this.entityMap = entityMap ?? throw new ArgumentNullException(nameof(entityMap));
            this.serverData = serverData ?? throw new ArgumentNullException(nameof(serverData));
            this.arcDir = arcDir ?? throw new ArgumentNullException(nameof(arcDir));

            GeneralOptions generalOptions = exporterConfig.GeneralOptions;
            dataLifetime = TimeSpan.FromSeconds(generalOptions.DataLifetime);
            string prefix = FilePrefix + "_" + generalOptions.ID.ToString("D3");
            log = new Log(Log.Formats.Simple) { FileName = Path.Combine(appDirs.LogDir, prefix + ".log") };
            infoFileName = Path.Combine(appDirs.LogDir, prefix + ".txt");
            stateFileName = Path.Combine(appDirs.StorageDir, prefix + "_State.xml");
            exporterTitle = string.Format("[{0}] {1}", generalOptions.ID, generalOptions.Name);
            dataSource = DataSourceFactory.GetDataSource(exporterConfig.ConnectionOptions);
            triggers = new ClassifiedTriggers(exporterConfig.Triggers, dataSource);

            thread = null;
            terminated = false;
            connStatus = ConnStatus.Undefined;

            CreateQueues();
            InitArcUploading();
        }


        /// <summary>
        /// Creates the data queues.
        /// </summary>
        private void CreateQueues()
        {
            maxQueueSize = Math.Max(exporterConfig.GeneralOptions.MaxQueueSize, MinQueueSize);

            if (triggers.CurDataTriggers.Count > 0)
            {
                curDataQueue = new Queue<QueueItem<SrezTableLight.Srez>>(maxQueueSize);
                curDataStats = new QueueStats();
            }
            else
            {
                curDataQueue = null;
                curDataStats = null;
            }

            if (triggers.ArcDataTriggers.Count > 0)
            {
                arcDataQueue = new Queue<QueueItem<SrezTableLight.Srez>>(maxQueueSize);
                arcDataStats = new QueueStats();
            }
            else
            {
                arcDataQueue = null;
                arcDataStats = null;
            }

            if (triggers.EventTriggers.Count > 0)
            {
                eventQueue = new Queue<QueueItem<EventTableLight.Event>>(maxQueueSize);
                eventStats = new QueueStats();
            }
            else
            {
                eventQueue = null;
                eventStats = null;
            }
        }

        /// <summary>
        /// Initializes objects required for uploading archives.
        /// </summary>
        private void InitArcUploading()
        {
            arcUploadState = exporterConfig.ArcUploadOptions.Enabled ? new ArcUploadState() : null;
            taskUploadState = null;
        }

        /// <summary>
        /// Exporter cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                DateTime utcNowDT = DateTime.UtcNow;
                DateTime arcCheckDT = DateTime.MinValue;
                DateTime writeInfoDT = utcNowDT;
                WriteInfo();
                LoadState();

                while (!terminated)
                {
                    // export data
                    try
                    {
                        if (Connect())
                        {
                            ExportCurData();
                            ExportArcData();
                            ExportEvents();
                        }
                    }
                    finally
                    {
                        Disconnect();
                    }

                    // upload archive
                    if (exporterConfig.ArcUploadOptions.Enabled || taskUploadState != null)
                    {
                        utcNowDT = DateTime.UtcNow;
                        if (utcNowDT - arcCheckDT > CheckArcPeriod)
                        {
                            arcCheckDT = utcNowDT;
                            UploadArcData();
                        }
                    }

                    // write information
                    utcNowDT = DateTime.UtcNow;
                    if (utcNowDT - writeInfoDT > WriteInfoPeriod)
                    {
                        writeInfoDT = utcNowDT;
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
        /// Connects to the database.
        /// </summary>
        private bool Connect()
        {
            try
            {
                dataSource.Connect();
                connStatus = ConnStatus.Normal;
                return true;
            }
            catch (Exception ex)
            {
                connStatus = ConnStatus.Error;
                log.WriteException(ex, Localization.UseRussian ? 
                    "Ошибка при соединении с БД" :
                    "Error connecting to DB");
                Thread.Sleep(ErrorDelay);
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        private void Disconnect()
        {
            try
            {
                dataSource.Disconnect();
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ? 
                    "Ошибка при разъединении с БД" :
                    "Error disconnecting from DB");
            }
        }

        /// <summary>
        /// Exports current data.
        /// </summary>
        private void ExportCurData()
        {
            if (curDataQueue == null)
                return;

            DbTransaction trans = null;

            try
            {
                trans = dataSource.Connection.BeginTransaction();

                for (int i = 0; i < BundleSize; i++)
                {
                    // retrieve a snapshot from the queue
                    QueueItem<SrezTableLight.Srez> queueItem;
                    SrezTableLight.Srez snapshot;

                    lock (curDataQueue)
                    {
                        if (curDataQueue.Count > 0)
                        {
                            queueItem = curDataQueue.Dequeue();
                            snapshot = queueItem.Value;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // export the snapshot
                    if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                    {
                        curDataStats.SkippedItems++;
                        log.WriteError(string.Format(Localization.UseRussian ?
                            "Устаревший текущий срез за {0} не экспортирован" :
                            "The outdated current snapshot for {0} is not exported",
                            snapshot.DateTime.ToLocalizedString()));
                    }
                    else if (ExportSnapshot(snapshot, trans, triggers.CurDataTriggers))
                    {
                        curDataStats.ExportedItems++;
                        curDataStats.ErrorState = false;
                    }
                    else
                    {
                        // return the unsent snapshot to the queue
                        lock (curDataQueue)
                        {
                            curDataQueue.Enqueue(queueItem);
                        }

                        curDataStats.ErrorState = true;
                        Thread.Sleep(ErrorDelay);
                        break;
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                dataSource.SafeRollback(trans);
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при экспорте текущих данных" :
                    "Error export current data");
            }
        }

        /// <summary>
        /// Exports archive data.
        /// </summary>
        private void ExportArcData()
        {
            if (arcDataQueue == null)
                return;

            DbTransaction trans = null;

            try
            {
                trans = dataSource.Connection.BeginTransaction();

                for (int i = 0; i < BundleSize; i++)
                {
                    // retrieve a snapshot from the queue
                    QueueItem<SrezTableLight.Srez> queueItem;
                    SrezTableLight.Srez snapshot;

                    lock (arcDataQueue)
                    {
                        if (arcDataQueue.Count > 0)
                        {
                            queueItem = arcDataQueue.Dequeue();
                            snapshot = queueItem.Value;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // export the snapshot
                    if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                    {
                        arcDataStats.SkippedItems++;
                        log.WriteError(string.Format(Localization.UseRussian ?
                            "Устаревший архивный срез за {0} не экспортирован" :
                            "The outdated archive snapshot for {0} is not exported",
                            snapshot.DateTime.ToLocalizedString()));
                    }
                    else if (ExportSnapshot(snapshot, trans, triggers.ArcDataTriggers))
                    {
                        arcDataStats.ExportedItems++;
                        arcDataStats.ErrorState = false;

                    }
                    else
                    {
                        // return the unsent snapshot to the queue
                        lock (arcDataQueue)
                        {
                            arcDataQueue.Enqueue(queueItem);
                        }

                        arcDataStats.ErrorState = true;
                        Thread.Sleep(ErrorDelay);
                        break;
                    }

                    // confirm sending to allow new data to be queued
                    if (arcDataQueue.Count == 0)
                    {
                        if (arcUploadState != null && arcUploadState.ConfirmSending())
                            SaveState();

                        taskUploadState?.ConfirmSending();
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                dataSource.SafeRollback(trans);
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при экспорте архивных данных" :
                    "Error export archive data");
            }
        }

        /// <summary>
        /// Exports events.
        /// </summary>
        private void ExportEvents()
        {
            if (eventQueue == null)
                return;

            DbTransaction trans = null;

            try
            {
                trans = dataSource.Connection.BeginTransaction();

                for (int i = 0; i < BundleSize; i++)
                {
                    // retrieve an event from the queue
                    QueueItem<EventTableLight.Event> queueItem;
                    EventTableLight.Event ev;

                    lock (eventQueue)
                    {
                        if (eventQueue.Count > 0)
                        {
                            queueItem = eventQueue.Dequeue();
                            ev = queueItem.Value;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // export the event
                    if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                    {
                        eventStats.SkippedItems++;
                        log.WriteError(string.Format(Localization.UseRussian ?
                            "Устаревшее событие за {0} не экспортировано" :
                            "The outdated event for {0} is not exported",
                            ev.DateTime.ToLocalizedString()));
                    }
                    else if (ExportEvent(ev, trans))
                    {
                        eventStats.ExportedItems++;
                        eventStats.ErrorState = false;
                    }
                    else
                    {
                        // return the unsent event to the queue
                        lock (eventQueue)
                        {
                            eventQueue.Enqueue(queueItem);
                        }

                        eventStats.ErrorState = true;
                        Thread.Sleep(ErrorDelay);
                        break;
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                dataSource.SafeRollback(trans);
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при экспорте событий" :
                    "Error export events");
            }
        }

        /// <summary>
        /// Exports the specified snapshot.
        /// </summary>
        private bool ExportSnapshot(SrezTableLight.Srez snapshot, DbTransaction trans, 
            IEnumerable<DataTrigger> dataTriggers)
        {
            Trigger currentTrigger = null;

            try
            {
                int firstCnlNum = snapshot.CnlNums.Length > 0 ? snapshot.CnlNums[0] : 0;
                entityMap.DeviceByCnlNum.TryGetValue(firstCnlNum, out int deviceNum);

                foreach (DataTrigger trigger in dataTriggers)
                {
                    currentTrigger = trigger;

                    if ((trigger.CnlNums.Count == 0 || trigger.CnlNums.Overlaps(snapshot.CnlNums)) &&
                        (trigger.DeviceNums.Count == 0 || trigger.DeviceNums.Contains(deviceNum)))
                    {
                        DbCommand cmd = trigger.Command;
                        cmd.Transaction = trans;
                        trigger.DateTimeParam.Value = snapshot.DateTime;
                        trigger.KpNumParam.Value = deviceNum;

                        if (trigger.DataTriggerOptions.SingleQuery)
                        {
                            if (trigger.CnlNums.Count > 0)
                            {
                                foreach (int cnlNum in trigger.CnlNums)
                                {
                                    SrezTableLight.CnlData cnlData = snapshot.GetCnlData(cnlNum);
                                    trigger.SetValParam(cnlNum, cnlData.Val);
                                    trigger.SetStatParam(cnlNum, cnlData.Stat);
                                }

                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            void ExportDataPoint(int cnlNum)
                            {
                                SrezTableLight.CnlData cnlData = snapshot.GetCnlData(cnlNum);
                                trigger.CnlNumParam.Value = cnlNum;
                                trigger.ValParam.Value = cnlData.Val;
                                trigger.StatParam.Value = cnlData.Stat;
                                cmd.ExecuteNonQuery();
                            }

                            if (trigger.CnlNums.Count > 0)
                            {
                                foreach (int cnlNum in snapshot.CnlNums)
                                {
                                    if (trigger.CnlNums.Contains(cnlNum))
                                        ExportDataPoint(cnlNum);
                                }
                            }
                            else
                            {
                                foreach (int cnlNum in snapshot.CnlNums)
                                {
                                    ExportDataPoint(cnlNum);
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при экспорте среза по триггеру \"{0}\"" :
                    "Error export snapshot by the trigger \"{0}\"",
                    currentTrigger?.Options?.Name ?? "");
                return false;
            }
        }

        /// <summary>
        /// Exports the specified event.
        /// </summary>
        private bool ExportEvent(EventTableLight.Event ev, DbTransaction trans)
        {
            Trigger currentTrigger = null;

            try
            {
                foreach (EventTrigger trigger in triggers.EventTriggers)
                {
                    currentTrigger = trigger;

                    if ((trigger.CnlNums.Count == 0 || trigger.CnlNums.Contains(ev.CnlNum)) &&
                        (trigger.DeviceNums.Count == 0 || trigger.DeviceNums.Contains(ev.KPNum)))
                    {
                        DbCommand cmd = trigger.Command;
                        cmd.Transaction = trans;
                        dataSource.SetParam(cmd, "dateTime", ev.DateTime);
                        dataSource.SetParam(cmd, "objNum", ev.ObjNum);
                        dataSource.SetParam(cmd, "kpNum", ev.KPNum);
                        dataSource.SetParam(cmd, "paramID", ev.ParamID);
                        dataSource.SetParam(cmd, "cnlNum", ev.CnlNum);
                        dataSource.SetParam(cmd, "oldCnlVal", ev.OldCnlVal);
                        dataSource.SetParam(cmd, "oldCnlStat", ev.OldCnlStat);
                        dataSource.SetParam(cmd, "newCnlVal", ev.NewCnlVal);
                        dataSource.SetParam(cmd, "newCnlStat", ev.NewCnlStat);
                        dataSource.SetParam(cmd, "checked", ev.Checked);
                        dataSource.SetParam(cmd, "userID", ev.UserID);
                        dataSource.SetParam(cmd, "descr", ev.Descr);
                        dataSource.SetParam(cmd, "data", ev.Data);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при экспорте события по триггеру \"{0}\"" :
                    "Error export event by the trigger \"{0}\"", 
                    currentTrigger?.Options?.Name ?? "");
                return false;
            }
        }

        /// <summary>
        /// Makes and enqueues snapshots of archive data.
        /// </summary>
        private void MakeArcSnapshots(DateTime dateTime)
        {
            foreach (int[] cnlNums in entityMap.CnlNumGroups)
            {
                EnqueueArcData(serverData.GetSnapshot(dateTime, exporterConfig.ArcUploadOptions.SnapshotType, cnlNums));
            }
        }

        /// <summary>
        /// Uploads archive data if possible.
        /// </summary>
        private void UploadArcData()
        {
            ArcUploadState uploadState = taskUploadState ?? arcUploadState;
            bool maxTimeReached = false;

            if (uploadState.IsReady &&
                serverData.GetAvailableSnapshots(uploadState.SnapshotDate, exporterConfig.ArcUploadOptions.SnapshotType) is
                DateTime[] availableSnapshots)
            {
                int snapshotIndex = uploadState.SnapshotIndex;
                int snapshotCount = availableSnapshots.Length;

                if (snapshotCount > 0)
                {
                    if (uploadState.SentSnapshotDT < uploadState.SnapshotDate)
                    {
                        if (uploadState.MinSnapshotDT.Date == uploadState.SnapshotDate)
                        {
                            // find a snapshot to upload
                            int index = Array.BinarySearch(availableSnapshots, uploadState.MinSnapshotDT);
                            snapshotIndex = index >= 0 ? index : ~index;
                        }
                        else
                        {
                            // upload the first snapshot
                            snapshotIndex = 0;
                        }
                    }
                    else if (0 <= snapshotIndex && snapshotIndex < snapshotCount &&
                        availableSnapshots[snapshotIndex] == uploadState.SentSnapshotDT)
                    {
                        // upload the next snapshot
                        snapshotIndex++;
                    }
                    else
                    {
                        // correct the index
                        int index = Array.BinarySearch(availableSnapshots, uploadState.SentSnapshotDT);
                        snapshotIndex = index >= 0 ? index + 1 : ~index;
                    }
                }

                if (0 <= snapshotIndex && snapshotIndex < snapshotCount)
                {
                    DateTime snapshotDT = availableSnapshots[snapshotIndex];

                    if (snapshotDT <= uploadState.MaxSnapshotDT)
                    {
                        if (snapshotDT.AddMilliseconds(exporterConfig.ArcUploadOptions.Delay) <= DateTime.Now)
                        {
                            // upload the snapshot
                            uploadState.IsReady = false;
                            uploadState.SnapshotIndex = snapshotIndex;
                            uploadState.QueuedSnapshotDT = snapshotDT;
                            MakeArcSnapshots(snapshotDT);
                        }
                    }
                    else
                    {
                        maxTimeReached = true;
                    }
                }
                else if (uploadState.SnapshotDate < DateTime.Today)
                {
                    // go to the next day
                    uploadState.SnapshotIndex = -1;
                    uploadState.SnapshotDate = uploadState.SnapshotDate.AddDays(1.0);

                    if (uploadState.SnapshotDate > uploadState.MaxSnapshotDT)
                        maxTimeReached = true;
                }
                else
                {
                    // date is today and no snapshots to upload
                    maxTimeReached = true;
                }

                if (maxTimeReached && taskUploadState != null)
                {
                    // stop the task
                    taskUploadState = null;
                }
            }
        }

        /// <summary>
        /// Loads events from file and adds to the queue.
        /// </summary>
        private void ExportEventsFromFile(DateTime minDT, DateTime maxDT)
        {
            if (minDT.Date != maxDT.Date)
            {
                log.WriteError(Localization.UseRussian ?
                    "Временной диапазон событий должен быть в пределах одних суток" :
                    "The time range of events must be within one day");
                return;
            }

            string fileName = ServerUtils.BuildEvFileName(arcDir, minDT.Date);

            if (!File.Exists(fileName))
            {
                log.WriteError(string.Format(CommonPhrases.NamedFileNotFound, fileName));
                return;
            }

            try
            {
                EventTableLight eventTable = new EventTableLight();
                EventAdapter eventAdapter = new EventAdapter { FileName = fileName };
                eventAdapter.Fill(eventTable);

                foreach (EventTableLight.Event ev in eventTable.AllEvents)
                {
                    if (minDT <= ev.DateTime && ev.DateTime <= maxDT)
                        EnqueueEvent(ev);
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при загрузке событий из файла {0}" :
                    "Error loading events from file {0}", fileName);
            }
        }

        /// <summary>
        /// Writes the exporter information to the file.
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                // build text
                StringBuilder sbInfo = new StringBuilder();

                if (Localization.UseRussian)
                {
                    sbInfo
                        .AppendLine("Состояние экспортёра")
                        .AppendLine("--------------------")
                        .Append("Наименование   : ").AppendLine(exporterTitle)
                        .Append("Сервер БД      : ").AppendLine(exporterConfig.ConnectionOptions.Server)
                        .Append("Соединение     : ").AppendLine(ConnStatusNamesRu[(int)connStatus])
                        .AppendLine();

                    void AppendQueueStatsRu(QueueStats queueStats, int? queueSize)
                    {
                        if (queueStats == null)
                        {
                            sbInfo.AppendLine("Очередь не используется");
                        }
                        else
                        {
                            sbInfo
                                .Append("Состояние      : ").AppendLine(queueStats.ErrorState ? "ошибка" : "норма")
                                .Append("В очереди      : ").Append(queueSize).Append(" из ").Append(maxQueueSize).AppendLine()
                                .Append("Экспортировано : ").Append(queueStats.ExportedItems).AppendLine()
                                .Append("Пропущено      : ").Append(queueStats.SkippedItems).AppendLine();
                        }

                        sbInfo.AppendLine();
                    }

                    sbInfo.AppendLine("Текущие данные");
                    sbInfo.AppendLine("--------------");
                    AppendQueueStatsRu(curDataStats, curDataQueue?.Count);

                    sbInfo.AppendLine("Архивные данные");
                    sbInfo.AppendLine("---------------");
                    AppendQueueStatsRu(arcDataStats, arcDataQueue?.Count);

                    sbInfo.AppendLine("События");
                    sbInfo.AppendLine("-------");
                    AppendQueueStatsRu(eventStats, eventQueue?.Count);

                    sbInfo.AppendLine("Передача архивов");
                    sbInfo.AppendLine("----------------");
                    ArcUploadState.AppendInfo(arcUploadState, sbInfo);

                    if (taskUploadState != null)
                    {
                        sbInfo.AppendLine("Передача архивов по заданию");
                        sbInfo.AppendLine("---------------------------");
                        taskUploadState.AppendInfo(sbInfo);
                    }
                }
                else
                {
                    sbInfo
                        .AppendLine("Exporter State")
                        .AppendLine("--------------")
                        .Append("Name       : ").AppendLine(exporterTitle)
                        .Append("DB server  : ").AppendLine(exporterConfig.ConnectionOptions.Server)
                        .Append("Connection : ").AppendLine(ConnStatusNamesEn[(int)connStatus])
                        .AppendLine();

                    void AppendQueueStatsEn(QueueStats queueStats, int? queueSize)
                    {
                        if (queueStats == null)
                        {
                            sbInfo.AppendLine("Queue is not in use");
                        }
                        else
                        {
                            sbInfo
                                .Append("Status     : ").AppendLine(queueStats.ErrorState ? "Error" : "Normal")
                                .Append("In queue   : ").Append(queueSize).Append(" of ").Append(maxQueueSize).AppendLine()
                                .Append("Exported   : ").Append(queueStats.ExportedItems).AppendLine()
                                .Append("Skipped    : ").Append(queueStats.SkippedItems).AppendLine();
                        }

                        sbInfo.AppendLine();
                    }

                    sbInfo.AppendLine("Current Data");
                    sbInfo.AppendLine("------------");
                    AppendQueueStatsEn(curDataStats, curDataQueue?.Count);

                    sbInfo.AppendLine("Archive Data");
                    sbInfo.AppendLine("------------");
                    AppendQueueStatsEn(arcDataStats, arcDataQueue?.Count);

                    sbInfo.AppendLine("Events");
                    sbInfo.AppendLine("------");
                    AppendQueueStatsEn(eventStats, eventQueue?.Count);

                    sbInfo.AppendLine("Archive Uploading");
                    sbInfo.AppendLine("-----------------");
                    ArcUploadState.AppendInfo(arcUploadState, sbInfo);

                    if (taskUploadState != null)
                    {
                        sbInfo.AppendLine("Archive Uploading by Task");
                        sbInfo.AppendLine("-------------------------");
                        taskUploadState.AppendInfo(sbInfo);
                    }
                }

                // write to file
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
                log.WriteException(ex, ModPhrases.WriteInfoError);
            }
        }

        /// <summary>
        /// Loads the export state.
        /// </summary>
        private void LoadState()
        {
            try
            {
                if (File.Exists(stateFileName))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(stateFileName);

                    if (arcUploadState != null)
                        arcUploadState.SentSnapshotDT = xmlDoc.DocumentElement.GetChildAsDateTime("SentSnapshotDT");

                    log.WriteAction(Localization.UseRussian ?
                        "Состояние экспорта загружено из файла" :
                        "Export state loaded from file");
                }
                else
                {
                    log.WriteAction(Localization.UseRussian ?
                        "Файл состояния экспорта отсутствует" :
                        "Export state file is missing");
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при загрузке состояния экспорта" :
                    "Error loading export state");
            }
            finally
            {
                arcUploadState?.InitSnapshotDate(exporterConfig.ArcUploadOptions.MaxAge);
            }
        }

        /// <summary>
        /// Saves the exporter state.
        /// </summary>
        private void SaveState()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ModDbExport_State");
                xmlDoc.AppendChild(rootElem);

                if (arcUploadState != null)
                    rootElem.AppendElem("SentSnapshotDT", arcUploadState.SentSnapshotDT);

                xmlDoc.Save(stateFileName);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при сохранении состояния экспорта" :
                    "Error saving export state");
            }
        }


        /// <summary>
        /// Starts the exporter.
        /// </summary>
        public void Start()
        {
            try
            {
                if (thread == null)
                {
                    log.WriteBreak();
                    log.WriteAction(string.Format(Localization.UseRussian ?
                        "Запуск экспортёра \"{0}\"" :
                        "Start exporter \"{0}\"", exporterTitle));

                    terminated = false;
                    thread = new Thread(Execute);
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при запуске работы экспортёра" :
                    "Error starting exporter");
            }

        }

        /// <summary>
        /// Stops the exporter.
        /// </summary>
        public void Stop()
        {
            try
            {
                if (thread != null)
                {
                    terminated = true;

                    if (thread.Join(ModLogic.WaitForStop))
                    {
                        log.WriteAction(string.Format(Localization.UseRussian ?
                            "Экспортёр \"{0}\" остановлен" :
                            "Exporter \"{0}\" is stopped", exporterTitle));
                    }
                    else
                    {
                        thread.Abort();
                        log.WriteError(Localization.UseRussian ?
                            "Работа экспортёра прервана" :
                            "Exporter is aborted");
                    }

                    thread = null;
                    log.WriteBreak();
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при остановке работы экспортёра" :
                    "Error stop exporter");
            }
        }

        /// <summary>
        /// Enqueues the current data for export.
        /// </summary>
        public void EnqueueCurData(SrezTableLight.Srez snapshot)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));

            if (curDataQueue != null)
            {
                lock (curDataQueue)
                {
                    if (curDataQueue.Count < maxQueueSize)
                    {
                        curDataQueue.Enqueue(new QueueItem<SrezTableLight.Srez>(DateTime.UtcNow, snapshot));
                    }
                    else
                    {
                        curDataStats.SkippedItems++;
                        log.WriteError(string.Format(Localization.UseRussian ?
                            "Невозможно добавить текущие данные в очередь. Максимальный размер очереди {0} превышен" :
                            "Unable to enqueue current data. The maximum size of the queue {0} is exceeded",
                            maxQueueSize));
                    }
                }
            }
        }

        /// <summary>
        /// Enqueues the archive data for export.
        /// </summary>
        public void EnqueueArcData(SrezTableLight.Srez snapshot)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));

            if (arcDataQueue != null)
            {
                lock (arcDataQueue)
                {
                    if (arcDataQueue.Count < maxQueueSize)
                    {
                        arcDataQueue.Enqueue(new QueueItem<SrezTableLight.Srez>(DateTime.UtcNow, snapshot));
                    }
                    else
                    {
                        arcDataStats.SkippedItems++;
                        log.WriteError(string.Format(Localization.UseRussian ?
                            "Невозможно добавить архивные данные в очередь. Максимальный размер очереди {0} превышен" :
                            "Unable to enqueue archive data. The maximum size of the queue {0} is exceeded",
                            maxQueueSize));
                    }
                }
            }
        }

        /// <summary>
        /// Enqueues the event for export.
        /// </summary>
        public void EnqueueEvent(EventTableLight.Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            if (eventQueue != null)
            {
                lock (eventQueue)
                {
                    if (eventQueue.Count < maxQueueSize)
                    {
                        eventQueue.Enqueue(new QueueItem<EventTableLight.Event>(DateTime.UtcNow, ev));
                    }
                    else
                    {
                        eventStats.SkippedItems++;
                        log.WriteError(string.Format(Localization.UseRussian ?
                            "Невозможно добавить событие в очередь. Максимальный размер очереди {0} превышен" :
                            "Unable to enqueue an event. The maximum size of the queue {0} is exceeded",
                            maxQueueSize));
                    }
                }
            }
        }

        /// <summary>
        /// Enqueues the command for execution.
        /// </summary>
        public void EnqueueCmd(int outCnlNum, Command cmd, ref bool passToClients)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            if (outCnlNum == exporterConfig.GeneralOptions.OutCnlNum)
            {
                passToClients = false;

                if (CmdParams.Parse(cmd.GetCmdDataStr(), out CmdParams cmdParams, out string errMsg))
                {
                    switch (cmdParams.Action)
                    {
                        case CmdAction.ArcUpload:
                            if (arcDataQueue == null)
                            {
                                log.WriteError(Localization.UseRussian ?
                                    "Невозможно выполнить команду, потому что экспорт архивов отключен" :
                                    "Unable to execute a command because archive export is disabled");
                            }
                            else
                            {
                                log.WriteError(Localization.UseRussian ?
                                    "Получена команда экспорта архивов" :
                                    "Archive export command received");

                                taskUploadState = new ArcUploadState()
                                {
                                    SnapshotDate = cmdParams.MinDT.Date,
                                    MinSnapshotDT = cmdParams.MinDT,
                                    MaxSnapshotDT = cmdParams.MaxDT
                                };
                            }
                            break;

                        case CmdAction.EvUpload:
                            if (eventQueue == null)
                            {
                                log.WriteError(Localization.UseRussian ?
                                    "Невозможно выполнить команду, потому что экспорт событий отключен" :
                                    "Unable to execute a command because event export is disabled");
                            }
                            else
                            {
                                log.WriteError(Localization.UseRussian ?
                                    "Получена команда экспорта событий" :
                                    "Event export command received");
                                ExportEventsFromFile(cmdParams.MinDT, cmdParams.MaxDT);
                            }
                            break;

                        default:
                            log.WriteError(Localization.UseRussian ?
                                "Неизвестная команда" :
                                "Unknown command");
                            break;
                    }
                }
                else
                {
                    log.WriteError(errMsg);
                }
            }
        }
    }
}
