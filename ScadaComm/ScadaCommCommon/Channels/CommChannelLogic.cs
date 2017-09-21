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
 * Module   : ScadaCommCommon
 * Summary  : The base class for communication channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2017
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// The base class for communication channel logic
    /// <para>Родительский класс логики работы канала связи</para>
    /// </summary>
    public abstract class CommChannelLogic
    {
        /// <summary>
        /// Режимы работы канала связи
        /// </summary>
        public enum OperatingBehaviors
        {
            /// <summary>
            /// Ведущий - циклический опрос
            /// </summary>
            Master,
            /// <summary>
            /// Ведомый - ожидание данных
            /// </summary>
            Slave
        }

        /// <summary>
        /// Задержка потока работы канала связи для экономии ресурсов в режиме ведущего, мс
        /// </summary>
        protected const int MasterThreadDelay = 100;
        /// <summary>
        /// Задержка потока работы канала связи для экономии ресурсов в режиме ведомого, мс
        /// </summary>
        protected const int SlaveThreadDelay = 10;
        /// <summary>
        /// Время ожидания остановки потока работы канала связи, мс
        /// </summary>
        protected const int WaitForStop = 10000;

        /// <summary>
        /// Сообщение об отсутствии требуемого параметра канала связи
        /// </summary>
        protected static readonly string CommCnlParamRequired = Localization.UseRussian ?
            "Требуется пользовательский параметр линии связи {0}." :
            "Custom communication line parameter {0} is required.";

        /// <summary>
        /// Метод записи в журнал линии связи
        /// </summary>
        private Log.WriteLineDelegate writeToLog;
        /// <summary>
        /// Список КП на линии связи
        /// </summary>
        protected List<KPLogic> kpList;
        /// <summary>
        /// Признак, что список КП не пустой
        /// </summary>
        protected bool kpListNotEmpty;
        /// <summary>
        /// Первый (нулевой) КП в списке
        /// </summary>
        protected KPLogic firstKP;
        /// <summary>
        /// Поток работы канала связи
        /// </summary>
        /// <remarks>Использование потока опционально</remarks>
        protected Thread thread;
        /// <summary>
        /// Работа потока канала связи прервана
        /// </summary>
        protected volatile bool terminated;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommChannelLogic()
        {
            writeToLog = text => { }; // заглушка
            kpList = new List<KPLogic>();
            kpListNotEmpty = false;
            firstKP = null;
            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Получить наименование типа канала связи
        /// </summary>
        /// <remarks>Используется для вывода в журнал и идентификации канала связи</remarks>
        public abstract string TypeName { get; }

        /// <summary>
        /// Получить режим работы
        /// </summary>
        public abstract OperatingBehaviors Behavior { get; }

        /// <summary>
        /// Получить или установить метод записи в журнал линии связи
        /// </summary>
        public Log.WriteLineDelegate WriteToLog
        {
            get
            {
                return writeToLog;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                writeToLog = value;
            }
        }


        /// <summary>
        /// Проверить, что все КП на линии связи имеют одинаковые библиотеки
        /// </summary>
        private bool AreDllsEqual(out string warnMsg)
        {
            HashSet<string> dllSet = new HashSet<string>();
            foreach (KPLogic kpLogic in kpList)
                dllSet.Add(kpLogic.Dll);

            bool dllsAreEqual = dllSet.Count <= 1;
            warnMsg = dllsAreEqual ? "" : (Localization.UseRussian ?
                "Предупреждение! Не рекомендуется использовать КП разных типов на одной линии связи." :
                "Warning! It is not recommended to use different device types on the same communication line.");
            return dllsAreEqual;
        }

        /// <summary>
        /// Проверить поддержку режима работы канала связи подключенными КП
        /// </summary>
        protected void CheckBehaviorSupport()
        {
            // проверка поддержки режима работы
            foreach (KPLogic kpLogic in kpList)
            {
                if (!kpLogic.CheckBehaviorSupport(Behavior))
                    throw new ScadaException(string.Format(Localization.UseRussian ? 
                        "Поведение {0} канала связи не поддерживается {1}." : 
                        "{0} behavior of the communication channel is not supprted by {1}", 
                        Behavior, kpLogic.Caption));
            }

            // проверка однотипности библиотек КП в режиме ведомого
            string warnMsg;
            if (Behavior == OperatingBehaviors.Slave && !AreDllsEqual(out warnMsg))
                WriteToLog(warnMsg);
        }
        
        /// <summary>
        /// Запустить поток работы канала связи
        /// </summary>
        protected void StartThread(ThreadStart start)
        {
            terminated = false;
            thread = new Thread(start);
            thread.Start();
        }

        /// <summary>
        /// Остановить поток работы канала связи
        /// </summary>
        protected void StopThread()
        {
            if (thread != null)
            {
                terminated = true;
                if (!thread.Join(WaitForStop))
                    thread.Abort();
                thread = null;
            }
        }

        /// <summary>
        /// Выполнить метод ProcIncomingReq для заданного КП с обработкой исключений
        /// </summary>
        protected bool ExecProcIncomingReq(KPLogic kpLogic, byte[] buffer, int offset, int count, ref KPLogic targetKP)
        {
            try
            {
                return kpLogic.ProcIncomingReq(buffer, offset, count, ref targetKP);
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ? 
                    "Ошибка при обработке считанного входящего запроса: " :
                    "Error processing just read incoming request: ") + ex.Message);
                targetKP = null;
                return false;
            }
        }

        /// <summary>
        /// Выполнить метод ProcUnreadIncomingReq для заданного КП с обработкой исключений
        /// </summary>
        protected bool ExecProcUnreadIncomingReq(KPLogic kpLogic, Connection conn, ref KPLogic targetKP)
        {
            try
            {
                return kpLogic.ProcUnreadIncomingReq(conn, ref targetKP);
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при обработке не считанного входящего запроса: " :
                    "Error processing unread incoming request: ") + ex.Message);
                targetKP = null;
                return false;
            }
        }


        /// <summary>
        /// Инициализировать канал связи
        /// </summary>
        /// <remarks>В случае исключения дальнейшая работа линии связи невозможна</remarks>
        public virtual void Init(SortedList<string, string> commCnlParams, List<KPLogic> kpList)
        {
            // проверка аргументов метода
            if (commCnlParams == null)
                throw new ArgumentNullException("commCnlParams");
            if (kpList == null)
                throw new ArgumentNullException("kpList");

            // копирование ссылок на КП линии связи
            foreach (KPLogic kpLogic in kpList)
            {
                if (kpLogic == null)
                    throw new ArgumentException("All the devices must not be null.");

                this.kpList.Add(kpLogic);
            }

            kpListNotEmpty = kpList.Count > 0;
            firstKP = kpListNotEmpty ? kpList[0] : null;
        }

        /// <summary>
        /// Запустить работу канала связи
        /// </summary>
        /// <remarks>В случае исключения линия связи попытается повторно запустить работу</remarks>
        public abstract void Start();

        /// <summary>
        /// Остановить работу канала связи
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Выполнить действия перед сеансом опроса КП или отправкой команды
        /// </summary>
        public virtual void BeforeSession(KPLogic kpLogic)
        {
        }

        /// <summary>
        /// Выполнить действия после сеанса опроса КП или отправки команды
        /// </summary>
        public virtual void AfterSession(KPLogic kpLogic)
        {
        }

        /// <summary>
        /// Получить информацию о работе канала связи
        /// </summary>
        public virtual string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder();

            if (Localization.UseRussian)
            {
                string title = "Канал связи";
                sbInfo.AppendLine(title)
                    .AppendLine(new string('-', title.Length))
                    .AppendLine("Тип: " + TypeName);
            }
            else
            {
                string title = "Connection Channel";
                sbInfo.AppendLine(title)
                    .AppendLine(new string('-', title.Length))
                    .AppendLine("Type: " + TypeName);
            }

            return sbInfo.ToString();
        }
    }
}