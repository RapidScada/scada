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

using Scada.Data;
using Scada.Server.Modules.DBExport;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
        /// Создать срез с заданными номерами каналов, используя данные из исходного среза
        /// </summary>
        private SrezTableLight.Srez CreateSrez(DateTime srezDT, int[] cnlNums, SrezTableLight.Srez sourceSrez)
        {
            int cnlCnt = cnlNums.Length;
            SrezTableLight.Srez srez = new SrezTableLight.Srez(srezDT, cnlCnt);

            for (int i = 0; i < cnlCnt; i++)
            {
                int cnlNum = cnlNums[i];
                SrezTableLight.CnlData cnlData;
                sourceSrez.GetCnlData(cnlNum, out cnlData);

                srez.CnlNums[i] = cnlNum;
                srez.CnlData[i] = cnlData;
            }

            return srez;
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
            log.FileName = LogDir + LogFileName;
            log.WriteBreak();
            log.WriteAction(string.Format(ModPhrases.StartModule, Name));

            // определение полного имени файла информации
            infoFileName = LogDir + InfoFileName;

            // загрука конфигурации
            config = new Config(ConfigDir);
            string errMsg;

            if (config.Load(out errMsg))
            {
                // инициализация источников данных
                int i = 0;
                while (i < config.ExportDestinations.Count)
                {
                    Config.ExportDestination expDest = config.ExportDestinations[i];
                    DataSource dataSource = expDest.DataSource;
                    Config.ExportParams expParams = expDest.ExportParams;

                    try
                    {
                        dataSource.InitConnection();
                        dataSource.InitCommands(
                            expParams.ExportCurData ? expParams.ExportCurDataQuery : "",
                            expParams.ExportArcData ? expParams.ExportArcDataQuery : "", 
                            expParams.ExportEvents ? expParams.ExportEventQuery : "");
                        i++;
                    }
                    catch (Exception ex)
                    {
                        log.WriteAction(string.Format(Localization.UseRussian ? 
                            "Ошибка при инициализации источника данных {0}: {1}" : 
                            "Error initializing data source {0}: {1}", dataSource.Name, ex.Message));
                        // исключение из работы назначения, источник данных которого не был успешно инициализирован
                        config.ExportDestinations.RemoveAt(i);
                    }
                }

                // создание и запуск экспортёров
                exporters = new List<Exporter>();
                foreach (Config.ExportDestination expDest in config.ExportDestinations)
                {
                    Exporter exporter = new Exporter(expDest, log);
                    exporter.Start();
                    exporters.Add(exporter);
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
                    if (!exporter.Terminated)
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
                    if (!exporter.Terminated)
                        exporter.Abort();
            }

            // прерывание потока для обновления файла информации
            if (infoThread != null)
            {
                infoThread.Abort();
                infoThread = null;
            }

            // вывод информации
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
                SrezTableLight.Srez srez = CreateSrez(DateTime.Now, cnlNums, curSrez);

                // добавление среза в очереди экспорта
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
                SrezTableLight.Srez srez = CreateSrez(arcSrez.DateTime, cnlNums, arcSrez);

                // добавление среза в очереди экспорта
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
                // добавление события в очереди экспорта
                foreach (Exporter exporter in exporters)
                    exporter.EnqueueEvent(ev);
            }
        }
    }
}
