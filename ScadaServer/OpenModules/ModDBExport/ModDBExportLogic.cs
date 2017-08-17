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
 * Module   : ModDBExport
 * Summary  : Server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 * 
 * Description
 * Server module for real time data export from Rapid SCADA to DB.
 */

using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Server.Modules.DBExport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Server module logic
    /// <para>Логика работы серверного модуля</para>
    /// </summary>
    public class ModDBExportLogic : ModLogic
    {
        /// <summary>
        /// Имя файла журнала работы модуля
        /// </summary>
        internal const string LogFileName = "ModDBExport.log";
        /// <summary>
        /// Имя файла информации о работе модуля
        /// </summary>
        private const string InfoFileName = "ModDBExport.txt";
        /// <summary>
        /// Задержка потока обновления файла информации, мс
        /// </summary>
        private const int InfoThreadDelay = 500;

        private bool normalWork;          // признак нормальной работы модуля
        private string workState;         // строковая запись состояния работы
        private Log log;                  // журнал работы модуля
        private string infoFileName;      // полное имя файла информации
        private Thread infoThread;        // поток для обновления файла информации
        private Config config;            // конфигурация модуля
        private List<Exporter> exporters; // экспортёры


        /// <summary>
        /// Конструктор
        /// </summary>
        public ModDBExportLogic()
        {
            normalWork = true;
            workState = Localization.UseRussian ? "норма" : "normal";
            log = null;
            infoFileName = "";
            infoThread = null;
            config = null;
            exporters = null;
        }


        /// <summary>
        /// Получить имя модуля
        /// </summary>
        public override string Name
        {
            get
            {
                return "ModDBExport";
            }
        }


        /// <summary>
        /// Получить параметры команды
        /// </summary>
        private void GetCmdParams(Command cmd, out string dataSourceName, out DateTime dateTime)
        {
            string cmdDataStr = cmd.GetCmdDataStr();
            string[] parts = cmdDataStr.Split('\n');

            dataSourceName = parts[0];
            try { dateTime = ScadaUtils.XmlParseDateTime(parts[1]); }
            catch { dateTime = DateTime.MinValue; }
        }

        /// <summary>
        /// Найти экспортёр по наименованию источника данных
        /// </summary>
        private Exporter FindExporter(string dataSourceName)
        {
            foreach (Exporter exporter in exporters)
                if (exporter.DataSource.Name == dataSourceName)
                    return exporter;
            return null;
        }

        /// <summary>
        /// Экспортировать текущие данные, загрузив их из файла
        /// </summary>
        private void ExportCurDataFromFile(Exporter exporter)
        {
            // загрузка текущего среза из файла
            SrezTableLight srezTable = new SrezTableLight();
            SrezAdapter srezAdapter = new SrezAdapter();
            srezAdapter.FileName = ServerUtils.BuildCurFileName(Settings.ArcDir);

            try
            {
                srezAdapter.Fill(srezTable);
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при загрузке текущего среза из файла {0}: {1}" :
                    "Error loading current data from file {0}: {1}", srezAdapter.FileName, ex.Message));
            }

            // добавление среза в очередь экспорта
            if (srezTable.SrezList.Count > 0)
            {
                SrezTableLight.Srez sourceSrez = srezTable.SrezList.Values[0];
                SrezTableLight.Srez srez = new SrezTableLight.Srez(DateTime.Now, sourceSrez.CnlNums, sourceSrez);
                exporter.EnqueueCurData(srez);
                log.WriteAction(Localization.UseRussian ? "Текущие данные добавлены в очередь экспорта" :
                    "Current data added to export queue");
            }
            else
            {
                log.WriteAction(Localization.UseRussian ? "Отсутствуют текущие данные для экспорта" :
                    "No current data to export");
            }
        }

        /// <summary>
        /// Экспортировать архивные данные, загрузив их из файла
        /// </summary>
        private void ExportArcDataFromFile(Exporter exporter, DateTime dateTime)
        {
            // загрузка таблицы минутных срезов из файла
            SrezTableLight srezTable = new SrezTableLight();
            SrezAdapter srezAdapter = new SrezAdapter();
            srezAdapter.FileName = ServerUtils.BuildMinFileName(Settings.ArcDir, dateTime);

            try
            {
                srezAdapter.Fill(srezTable);
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при загрузке таблицы минутных срезов из файла {0}: {1}" :
                    "Error loading minute data table from file {0}: {1}", srezAdapter.FileName, ex.Message));
            }

            // поиск среза на заданное время
            SrezTableLight.Srez srez = srezTable.GetSrez(dateTime);

            // добавление среза в очередь экспорта
            if (srez == null)
            {
                log.WriteAction(Localization.UseRussian ? "Отсутствуют архивные данные для экспорта" :
                    "No archive data to export");
            }
            else
            {
                exporter.EnqueueArcData(srez);
                log.WriteAction(Localization.UseRussian ? "Архивные данные добавлены в очередь экспорта" :
                    "Archive data added to export queue");
            }
        }

        /// <summary>
        /// Экспортировать события, загрузив их из файла
        /// </summary>
        private void ExportEventsFromFile(Exporter exporter, DateTime date)
        {
            // загрузка таблицы событий из файла
            EventTableLight eventTable = new EventTableLight();
            EventAdapter eventAdapter = new EventAdapter();
            eventAdapter.FileName = ServerUtils.BuildEvFileName(Settings.ArcDir, date);

            try
            {
                eventAdapter.Fill(eventTable);
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при загрузке таблицы событий из файла {0}: {1}" :
                    "Error loading event table from file {0}: {1}", eventAdapter.FileName, ex.Message));
            }

            // добавление событий в очередь экспорта
            if (eventTable.AllEvents.Count > 0)
            {
                foreach (EventTableLight.Event ev in eventTable.AllEvents)
                    exporter.EnqueueEvent(ev);
                log.WriteAction(Localization.UseRussian ? "События добавлены в очередь экспорта" :
                    "Events added to export queue");
            }
            else
            {
                log.WriteAction(Localization.UseRussian ? "Отсутствуют события для экспорта" :
                    "No events to export");
            }
        }

        /// <summary>
        /// Записать в файл информацию о работе модуля
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                // формирование текста
                StringBuilder sbInfo = new StringBuilder();

                if (Localization.UseRussian)
                {
                    sbInfo
                        .AppendLine("Модуль экспорта данных")
                        .AppendLine("----------------------")
                        .Append("Состояние: ").AppendLine(workState).AppendLine()
                        .AppendLine("Источники данных")
                        .AppendLine("----------------");
                }
                else
                {
                    sbInfo
                        .AppendLine("Export Data Module")
                        .AppendLine("------------------")
                        .Append("State: ").AppendLine(workState).AppendLine()
                        .AppendLine("Data Sources")
                        .AppendLine("------------");
                }

                int cnt = exporters.Count;
                if (cnt > 0)
                {
                    for (int i = 0; i < cnt; i++)
                        sbInfo.Append((i + 1).ToString()).Append(". ").
                            AppendLine(exporters[i].GetInfo());
                }
                else
                {
                    sbInfo.AppendLine(Localization.UseRussian ? "Нет" : "No");
                }

                // вывод в файл
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                    writer.Write(sbInfo.ToString());
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.WriteAction(ModPhrases.WriteInfoError + ": " + ex.Message, Log.ActTypes.Exception);
            }
        }


        /// <summary>
        /// Выполнить действия при запуске работы сервера
        /// </summary>
        public override void OnServerStart()
        {
            // вывод в журнал
            log = new Log(Log.Formats.Simple);
            log.Encoding = Encoding.UTF8;
            log.FileName = AppDirs.LogDir + LogFileName;
            log.WriteBreak();
            log.WriteAction(string.Format(ModPhrases.StartModule, Name));

            // определение полного имени файла информации
            infoFileName = AppDirs.LogDir + InfoFileName;

            // загрука конфигурации
            config = new Config(AppDirs.ConfigDir);
            string errMsg;

            if (config.Load(out errMsg))
            {
                // создание и запуск экспортёров
                exporters = new List<Exporter>();
                foreach (Config.ExportDestination expDest in config.ExportDestinations)
                {
                    Exporter exporter = new Exporter(expDest, log);
                    exporters.Add(exporter);
                    exporter.Start();
                }

                // создание и запуск потока для обновления файла информации
                infoThread = new Thread(() => { while (true) { WriteInfo(); Thread.Sleep(InfoThreadDelay); } });
                infoThread.Start();
            }
            else
            {
                normalWork = false;
                workState = Localization.UseRussian ? "ошибка" : "error";
                WriteInfo();
                log.WriteAction(errMsg);
                log.WriteAction(ModPhrases.NormalModExecImpossible);
            }
        }

        /// <summary>
        /// Выполнить действия при остановке работы сервера
        /// </summary>
        public override void OnServerStop()
        {
            // остановка экспортёров
            foreach (Exporter exporter in exporters)
                exporter.Terminate();

            // ожидание завершения работы экспортёров
            DateTime nowDT = DateTime.Now;
            DateTime begDT = nowDT;
            DateTime endDT = nowDT.AddMilliseconds(WaitForStop);
            bool running;

            do
            {
                running = false;
                foreach (Exporter exporter in exporters)
                {
                    if (exporter.Running)
                    {
                        running = true;
                        break;
                    }
                }
                if (running)
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                nowDT = DateTime.Now;
            }
            while (begDT <= nowDT && nowDT <= endDT && running);

            // прерывание работы экспортёров
            if (running)
            {
                foreach (Exporter exporter in exporters)
                    if (exporter.Running)
                        exporter.Abort();
            }

            // прерывание потока для обновления файла информации
            if (infoThread != null)
            {
                infoThread.Abort();
                infoThread = null;
            }

            // вывод информации
            workState = Localization.UseRussian ? "остановлен" : "stopped";
            WriteInfo();
            log.WriteAction(string.Format(ModPhrases.StopModule, Name));
            log.WriteBreak();
        }

        /// <summary>
        /// Выполнить действия после обработки новых текущих данных
        /// </summary>
        public override void OnCurDataProcessed(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
            // экспорт текущих данных в БД
            if (normalWork)
            {
                // создание экпортируемого среза
                SrezTableLight.Srez srez = new SrezTableLight.Srez(DateTime.Now, cnlNums, curSrez);

                // добавление среза в очередь экспорта
                foreach (Exporter exporter in exporters)
                    exporter.EnqueueCurData(srez);
            }
        }

        /// <summary>
        /// Выполнить действия после обработки новых архивных данных
        /// </summary>
        public override void OnArcDataProcessed(int[] cnlNums, SrezTableLight.Srez arcSrez)
        {
            // экспорт архивных данных в БД
            if (normalWork)
            {
                // создание экпортируемого среза
                SrezTableLight.Srez srez = new SrezTableLight.Srez(arcSrez.DateTime, cnlNums, arcSrez);

                // добавление среза в очередь экспорта
                foreach (Exporter exporter in exporters)
                    exporter.EnqueueArcData(srez);
            }
        }

        /// <summary>
        /// Выполнить действия после создания события и записи на диск
        /// </summary>
        public override void OnEventCreated(EventTableLight.Event ev)
        {
            // экспорт события в БД
            if (normalWork)
            {
                // добавление события в очередь экспорта
                foreach (Exporter exporter in exporters)
                    exporter.EnqueueEvent(ev);
            }
        }

        /// <summary>
        /// Выполнить действия после приёма команды ТУ
        /// </summary>
        public override void OnCommandReceived(int ctrlCnlNum, Command cmd, int userID, ref bool passToClients)
        {
            // экспорт в ручном режиме
            if (normalWork)
            {
                bool exportCurData = ctrlCnlNum == config.CurDataCtrlCnlNum;
                bool exportArcData = ctrlCnlNum == config.ArcDataCtrlCnlNum;
                bool exportEvents = ctrlCnlNum == config.EventsCtrlCnlNum;
                bool procCmd = true;

                if (exportCurData)
                    log.WriteAction(Localization.UseRussian ? 
                        "Получена команда экспорта текущих данных" :
                        "Export current data command received");
                else if (exportArcData)
                    log.WriteAction(Localization.UseRussian ? 
                        "Получена команда экспорта архивных данных" :
                        "Export archive data command received");
                else if (exportEvents)
                    log.WriteAction(Localization.UseRussian ? 
                        "Получена команда экспорта событий" :
                        "Export events command received");
                else
                    procCmd = false;

                if (procCmd)
                {
                    passToClients = false;

                    if (cmd.CmdTypeID == BaseValues.CmdTypes.Binary)
                    {
                        string dataSourceName;
                        DateTime dateTime;
                        GetCmdParams(cmd, out dataSourceName, out dateTime);

                        if (dataSourceName == "")
                        {
                            log.WriteLine(string.Format(Localization.UseRussian ?
                                "Источник данных не задан" : "Data source is not specified"));
                        }
                        else
                        {
                            Exporter exporter = FindExporter(dataSourceName);

                            if (exporter == null)
                            {
                                log.WriteLine(string.Format(Localization.UseRussian ?
                                    "Неизвестный источник данных {0}" : "Unknown data source {0}", dataSourceName));
                            }
                            else
                            {
                                log.WriteLine(string.Format(Localization.UseRussian ?
                                    "Источник данных: {0}" : "Data source: {0}", dataSourceName));

                                if (exportCurData)
                                {
                                    ExportCurDataFromFile(exporter);
                                }
                                else if (exportArcData)
                                {
                                    if (dateTime == DateTime.MinValue)
                                    {
                                        log.WriteLine(string.Format(Localization.UseRussian ?
                                            "Некорректная дата и время" : "Incorrect date and time"));
                                    }
                                    else
                                    {
                                        log.WriteLine(string.Format(Localization.UseRussian ?
                                            "Дата и время: {0:G}" : "Date and time: {0:G}", dateTime));
                                        ExportArcDataFromFile(exporter, dateTime);
                                    }
                                }
                                else // exportEvents
                                {
                                    if (dateTime == DateTime.MinValue)
                                    {
                                        log.WriteLine(string.Format(Localization.UseRussian ?
                                            "Некорректная дата" : "Incorrect date"));
                                    }
                                    else
                                    {
                                        log.WriteLine(string.Format(Localization.UseRussian ?
                                            "Дата: {0:d}" : "Date: {0:d}", dateTime));
                                        ExportEventsFromFile(exporter, dateTime);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        log.WriteAction(ModPhrases.IllegalCommand);
                    }
                }
            }
        }
    }
}
