/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : Cache of the data received from SCADA-Server for clients usage
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Data;
using System;
using System.Data;
using System.Threading;
using Utils;

namespace Scada.Client
{
    /// <summary>
    /// Cache of the data received from SCADA-Server for clients usage
    /// <para>Кэш данных, полученных от SCADA-Сервера, для использования клиентами</para>
    /// </summary>
    /// <remarks>All the returned data are not thread safe
    /// <para>Все возвращаемые данные не являются потокобезопасными</para></remarks>
    public class DataCache
    {
        /// <summary>
        /// Время актуальности данных в кэше, с
        /// </summary>
        protected const int DataValidTime = 1;
        /// <summary>
        /// Время ожидания снятия блокировки базы конфигурации, с
        /// </summary>
        protected const int WaitBaseLock = 5;


        /// <summary>
        /// Объект для обмена данными со SCADA-Сервером
        /// </summary>
        protected readonly ServerComm serverComm;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;

        /// <summary>
        /// Время последего успешного заполнения таблиц базы конфигурации
        /// </summary>
        protected DateTime baseFillDT;
        /// <summary>
        /// Время последнего изменения успешно считанной базы конфигурации
        /// </summary>
        protected DateTime baseAge;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected DataCache()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DataCache(ServerComm serverComm, Log log)
        {
            if (serverComm == null)
                throw new ArgumentNullException("serverComm");
            if (log == null)
                throw new ArgumentNullException("log");

            this.serverComm = serverComm;
            this.log = log;

            baseFillDT = DateTime.MinValue;
            baseAge = DateTime.MinValue;

            BaseTables = new BaseTables();
            CnlProps = new InCnlProps[0];
            CtrlCnlProps = new CtrlCnlProps[0];
        }


        /// <summary>
        /// Получить таблицы базы конфигурации
        /// </summary>
        /// <remarks>Таблицы после загрузки не изменяются экземпляром данного класса.
        /// При обновлении таблиц объект таблиц пересоздаётся, обеспечивая целостность</remarks>
        public BaseTables BaseTables { get; protected set; }

        /// <summary>
        /// Получить свойства входных каналов
        /// </summary>
        /// <remarks>Свойства каналов автоматически создаются после обновления таблиц базы конфигурации</remarks>
        public InCnlProps[] CnlProps { get; protected set; }

        /// <summary>
        /// Получить свойства входных каналов
        /// </summary>
        /// <remarks>Свойства каналов автоматически создаются после обновления таблиц базы конфигурации</remarks>
        public CtrlCnlProps[] CtrlCnlProps { get; protected set; }


        /// <summary>
        /// Заполнить свойства входных каналов
        /// </summary>
        protected void FillCnlProps()
        {
        }

        /// <summary>
        /// Заполнить свойства каналов управления
        /// </summary>
        protected void FillCtrlCnlProps()
        {
        }


        /// <summary>
        /// Обновить таблицы базы конфигурации и свойства каналов
        /// </summary>
        public void RefreshBaseTables()
        {
            lock (this)
            {
                try
                {
                    DateTime utcNowDT = DateTime.UtcNow;

                    if ((utcNowDT - baseFillDT).TotalSeconds > DataValidTime) // данные устарели
                    {
                        DateTime newBaseAge = serverComm.ReceiveFileAge(ServerComm.Dirs.BaseDAT,
                            BaseTables.GetFileName(BaseTables.InCnlTable));

                        if (newBaseAge == DateTime.MinValue)
                        {
                            throw new ScadaException(Localization.UseRussian ?
                                "Не удалось принять время изменения базы конфигурации." :
                                "Unable to receive the configuration database modification time.");
                        }
                        else if (baseAge != newBaseAge /*база конфигурации изменена*/)
                        {
                            baseFillDT = utcNowDT;
                            baseAge = newBaseAge;

                            log.WriteAction(Localization.UseRussian ? 
                                "Обновление таблиц базы конфигурации" :
                                "Refresh the tables of the configuration database");

                            // ожидание снятия возможной блокировки базы конфигурации
                            DateTime t0 = utcNowDT;
                            while (serverComm.ReceiveFileAge(ServerComm.Dirs.BaseDAT, "baselock") > DateTime.MinValue &&
                                (DateTime.UtcNow - t0).TotalSeconds <= WaitBaseLock)
                            {
                                Thread.Sleep(ScadaUtils.ThreadDelay);
                            }

                            // получение данных таблиц
                            foreach (DataTable dataTable in BaseTables.AllTables)
                            {
                                string tableName = BaseTables.GetFileName(dataTable);

                                if (!serverComm.ReceiveBaseTable(tableName, dataTable))
                                {
                                    log.WriteError(string.Format(Localization.UseRussian ?
                                        "Не удалось принять таблицу {0}" :
                                        "Unable to receive the table {0}", tableName));

                                    baseFillDT = DateTime.MinValue;
                                    baseAge = DateTime.MinValue;
                                }
                            }

                            // заполнение свойств каналов
                            FillCnlProps();
                            FillCtrlCnlProps();
                        }
                    }
                }
                catch (Exception ex)
                {
                    baseFillDT = DateTime.MinValue;
                    baseAge = DateTime.MinValue;

                    log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при обновлении таблиц базы конфигурации" :
                        "Error refreshing the tables of the configuration database");
                }
            }
        }

        /// <summary>
        /// Обновить данные текущего среза
        /// </summary>
        public void RefreshCurData()
        {
        }


        /// <summary>
        /// Получить текущий срез из кеша или от сервера
        /// </summary>
        /// <remarks>Возвращаемый срез после загрузки не изменяется экземпляром данного класса. 
        /// Метод всегда возвращает объект, не равный null</remarks>
        public SrezTableLight.Srez GetCurSnapshot()
        {
            return new SrezTableLight.Srez(DateTime.MinValue, 1);
        }

        /// <summary>
        /// Получить тренд минутных данных заданного канала за сутки
        /// </summary>
        /// <remarks>Возвращаемый тренд после загрузки не изменяется экземпляром данного класса.
        /// Метод всегда возвращает объект, не равный null</remarks>
        public Trend GetMinTrend(int cnlNum, DateTime date)
        {
            return new Trend(cnlNum);
        }

        /// <summary>
        /// Получить таблицу часового среза за сутки из кеша или от сервера
        /// </summary>
        /// <remarks>Возвращаемая таблица после загрузки не изменяется экземпляром данного класса. 
        /// Метод всегда возвращает объект, не равный null</remarks>
        public SrezTableLight GetHourTable(DateTime date)
        {
            return new SrezTableLight();
        }

        /// <summary>
        /// Получить таблицу событий за сутки из кеша или от сервера
        /// </summary>
        /// <remarks>Возвращаемая таблица после загрузки не изменяется экземпляром данного класса.
        /// Метод всегда возвращает объект, не равный null</remarks>
        public EventTableLight GetEventTable(DateTime date)
        {
            return new EventTableLight();
        }
    }
}