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
 * Summary  : Exporter for one export destination
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 * 
 * Description
 * Server module for real time data export from Rapid SCADA to DB.
 */

using Scada.Data;
using System;
using System.Collections.Generic;
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

        private Log log; // журнал работы модуля
        private Queue<SrezTableLight.Srez> curSrezQueue; // очередь экспортируемых текущих срезов
        private Queue<SrezTableLight.Srez> arcSrezQueue; // очередь экспортируемых архивных срезов
        private Queue<EventTableLight.Event> evQueue;    // очередь экспортируемых событий

        private Thread thread;            // поток работы экспортёра
        private volatile bool terminated; // необходимо завершить работу потока

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

            fatalError = false;
            exportError = false;
            expCurSrezCnt = 0;
            expArcSrezCnt = 0;
            expEvCnt = 0;
            skipCurSrezCnt = 0;
            skipArcSrezCnt = 0;
            skipEvCnt = 0;

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
        /// Получить признак, что работа экспортёра завершена
        /// </summary>
        public bool Terminated { get; private set; }


        /// <summary>
        /// Цикл работы менеждера (метод вызывается в отдельном потоке)
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }


        /// <summary>
        /// Запустить работу экспортёра
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// Начать остановку работы экспортёра
        /// </summary>
        public void Terminate()
        {

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
                    .Append("; Состояние: ").Append(stateStr)
                    .Append("; В очереди тек/арх/соб: ")
                    .Append(curSrezQueueCnt).Append("/").Append(arcSrezQueueCnt).Append("/").Append(evQueueCnt)
                    .Append("; Экспортировано тек/арх/соб: ")
                    .Append(expCurSrezCnt).Append("/").Append(expArcSrezCnt).Append("/").Append(expEvCnt)
                    .Append("; Пропущено тек/арх/соб: ")
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
                    .Append("; State: ").Append(stateStr)
                    .Append("; In queue cur/arc/ev: ")
                    .Append(curSrezQueueCnt).Append("/").Append(arcSrezQueueCnt).Append("/").Append(evQueueCnt)
                    .Append("; Exported cur/arc/ev: ")
                    .Append(expCurSrezCnt).Append("/").Append(expArcSrezCnt).Append("/").Append(expEvCnt)
                    .Append("; Skipped cur/arc/ev: ")
                    .Append(skipCurSrezCnt).Append("/").Append(skipArcSrezCnt).Append("/").Append(skipEvCnt);
            }

            return sbInfo.ToString();
        }
    }
}
