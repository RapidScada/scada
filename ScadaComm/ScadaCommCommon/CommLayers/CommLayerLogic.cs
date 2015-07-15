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
 * Module   : ScadaCommCommon
 * Summary  : The base class for communication layer logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// The base class for communication layer logic
    /// <para>Родительский класс логики работы слоя связи</para>
    /// </summary>
    public abstract class CommLayerLogic
    {
        /// <summary>
        /// Режимы работы слоя связи
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
        /// Задержка потока работы слоя связи для экономии ресурсов в режиме ведущего, мс
        /// </summary>
        protected const int MasterThreadDelay = 100;
        /// <summary>
        /// Задержка потока работы слоя связи для экономии ресурсов в режиме ведомого, мс
        /// </summary>
        protected const int SlaveThreadDelay = 10;
        /// <summary>
        /// Время ожидания остановки потока работы слоя связи, мс
        /// </summary>
        protected const int WaitForStop = 10000;

        /// <summary>
        /// Сообщение об отсутствии требуемого параметра слоя связи
        /// </summary>
        protected static readonly string LayerParamRequired = Localization.UseRussian ?
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
        /// Словарь КП по адресам
        /// </summary>
        protected Dictionary<int, KPLogic> kpAddrDict;
        /// <summary>
        /// Словарь КП по позывным
        /// </summary>
        protected Dictionary<string, KPLogic> kpCallNumDict;
        /// <summary>
        /// Поток работы слоя связи
        /// </summary>
        /// <remarks>Использование потока опционально</remarks>
        protected Thread thread;
        /// <summary>
        /// Работа потока слоя связи прервана
        /// </summary>
        protected volatile bool terminated;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommLayerLogic()
        {
            writeToLog = text => { }; // заглушка
            kpList = new List<KPLogic>();
            kpAddrDict = new Dictionary<int, KPLogic>();
            kpCallNumDict = new Dictionary<string, KPLogic>();
            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Получить внутреннее наименование слоя связи
        /// </summary>
        /// <remarks>Используется для вывода в журнал и идентификации слоя связи</remarks>
        public abstract string InternalName { get; }

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
        protected bool AreDllsEqual(out string warnMsg)
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
        /// Получить строковый параметр слоя связи
        /// </summary>
        protected string GetStringLayerParam(Dictionary<string, string> layerParams,
            string name, bool required, string defaultValue)
        {
            string val;
            if (layerParams.TryGetValue(name, out val))
                return val;
            else if (required)
                throw new ArgumentException(string.Format(LayerParamRequired, name));
            else
                return defaultValue;
        }

        /// <summary>
        /// Получить логический параметр слоя связи
        /// </summary>
        protected bool GetBoolLayerParam(Dictionary<string, string> layerParams,
            string name, bool required, bool defaultValue)
        {
            string valStr;
            bool val;

            if (layerParams.TryGetValue(name, out valStr))
            {
                if (bool.TryParse(valStr, out val))
                {
                    return val;
                }
                else
                {
                    throw new ArgumentException(string.Format(Localization.UseRussian ?
                        "Пользовательский параметр линии связи {0} должен быть false или true." :
                        "Custom communication line parameter {0} must be false or true.", valStr));
                }
            }
            else if (required)
            {
                throw new ArgumentException(string.Format(LayerParamRequired, name));
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Получить целочисленный параметр слоя связи
        /// </summary>
        protected int GetIntLayerParam(Dictionary<string, string> layerParams, 
            string name, bool required, int defaultValue)
        {
            string valStr;
            int val;

            if (layerParams.TryGetValue(name, out valStr))
            {
                if (int.TryParse(valStr, out val))
                {
                    return val;
                }
                else
                {
                    throw new ArgumentException(string.Format(Localization.UseRussian ?
                        "Пользовательский параметр линии связи {0} должен быть целым числом." :
                        "Custom communication line parameter {0} must be an integer.", valStr));
                }
            }
            else if (required)
            {
                throw new ArgumentException(string.Format(LayerParamRequired, name));
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Получить параметр слоя связи перечислимого типа
        /// </summary>
        protected T GetEnumLayerParam<T>(Dictionary<string, string> layerParams,
            string name, bool required, T defaultValue) where T : struct 
        {
            string valStr;
            T val;

            if (layerParams.TryGetValue(name, out valStr))
            {
                if (Enum.TryParse<T>(valStr, true, out val))
                {
                    return val;
                }
                else
                {
                    throw new ArgumentException(string.Format(Localization.UseRussian ?
                        "Невозможно преобразовать пользовательский параметр линии связи {0}." :
                        "Unable to convert custom communication line parameter {0}.", valStr));
                }
            }
            else if (required)
            {
                throw new ArgumentException(string.Format(LayerParamRequired, name));
            }
            else
            {
                return defaultValue;
            }
        }
        
        /// <summary>
        /// Запустить поток работы слоя связи
        /// </summary>
        protected void StartThread(ThreadStart start)
        {
            terminated = false;
            thread = new Thread(start);
            thread.Start();
        }

        /// <summary>
        /// Остановить поток работы слоя связи
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
        /// Инициализировать слой связи
        /// </summary>
        /// <remarks>В случае исключения дальнейшая работа линии связи невозможна</remarks>
        public virtual void Init(Dictionary<string, string> layerParams, List<KPLogic> kpList)
        {
            // проверка аргументов метода
            if (layerParams == null)
                throw new ArgumentNullException("layerParams");
            if (kpList == null)
                throw new ArgumentNullException("kpList");

            // копирование ссылок на КП линии связи
            foreach (KPLogic kpLogic in kpList)
            {
                this.kpList.Add(kpLogic);

                int addr = kpLogic.Address;
                if (addr > 0 && !kpAddrDict.ContainsKey(addr))
                    kpAddrDict.Add(addr, kpLogic);

                string callNum = kpLogic.CallNum;
                if (!string.IsNullOrEmpty(callNum) && !kpCallNumDict.ContainsKey(callNum))
                    kpCallNumDict.Add(callNum, kpLogic);

                //kpLogic.FindKPLogic = FindKPLogic; !!!
            }
        }

        /// <summary>
        /// Запустить работу слоя связи
        /// </summary>
        /// <remarks>В случае исключения линия связи попытается повторно запустить работу</remarks>
        public abstract void Start();

        /// <summary>
        /// Остановить работу слоя связи
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
        /// Получить информацию о работе слоя связи
        /// </summary>
        public abstract string GetInfo();

        /// <summary>
        /// Найти КП на линии связи
        /// </summary>
        /// <remarks>Временно, необходимо перенести в класс линии связи</remarks>
        public KPLogic FindKPLogic(int address, string callNum)
        {
            bool addrEmpty = address <= 0;
            bool callNumEmpty = string.IsNullOrEmpty(callNum);

            if (addrEmpty && callNumEmpty)
            {
                return null;
            }
            else if (addrEmpty)
            {
                // поиск в словаре по позывному
                KPLogic foundKPLogic;
                return kpCallNumDict.TryGetValue(callNum, out foundKPLogic) ? foundKPLogic : null;
            }
            else if (callNumEmpty)
            {
                // поиск в словаре по адресу
                KPLogic foundKPLogic;
                return kpAddrDict.TryGetValue(address, out foundKPLogic) ? foundKPLogic : null;
            }
            else
            {
                // поиск в списке по адресу и позывному
                KPLogic foundKPLogic = null;
                foreach (KPLogic kpLogic in kpList)
                {
                    if (kpLogic.Address == address && kpLogic.CallNum == callNum)
                    {
                        foundKPLogic = kpLogic;
                        break;
                    }
                }
                return foundKPLogic;
            }
        }
    }
}