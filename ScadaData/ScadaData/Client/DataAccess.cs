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
 * Summary  : Thread safe access to the client cache data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Data;
using System;
using Utils;

namespace Scada.Client
{
    /// <summary>
    /// Thread safe access to the client cache data
    /// <para>Потокобезопасный доступ к данным кеша клиентов</para>
    /// </summary>
    /// <remarks>The class replaces Scada.Web.MainData
    /// <para>Класс заменяет Scada.Web.MainData</para></remarks>
    public class DataAccess
    {
        /// <summary>
        /// Кеш данных
        /// </summary>
        protected readonly ClientCache clientCache;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;
        /// <summary>
        /// Объект для синхронизации достапа к свойствам входных каналов
        /// </summary>
        protected readonly object cnlPropsLock;
        /// <summary>
        /// Объект для синхронизации достапа к свойствам каналов управления
        /// </summary>
        protected readonly object ctrlCnlPropsLock;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected DataAccess()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DataAccess(ClientCache clientCache, Log log)
        {
            if (clientCache == null)
                throw new ArgumentNullException("clientCache");
            if (log == null)
                throw new ArgumentNullException("log");

            this.clientCache = clientCache;
            this.log = log;
            cnlPropsLock = new object();
            ctrlCnlPropsLock = new object();
        }
        
        
        /// <summary>
        /// Получить свойства входного канала по его номеру
        /// </summary>
        public InCnlProps GetCnlProps(int cnlNum)
        {
            lock (cnlPropsLock)
            {
                try
                {
                    // сохранение ссылки на свойства каналов,
                    // т.к. свойство CnlProps может быть изменено из другого потока
                    InCnlProps[] cnlProps = clientCache.CnlProps;

                    // поиск свойств заданного канала
                    int ind = Array.BinarySearch(cnlProps, cnlNum, InCnlProps.IntComp);
                    return ind >= 0 ? cnlProps[ind] : null;
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при получении свойств входного канала {0}" :
                        "Error getting input channel {0} properties", cnlNum);
                    return null;
                }
            }
        }

        /// <summary>
        /// Получить свойства канала управления по его номеру
        /// </summary>
        public CtrlCnlProps GetCtrlCnlProps(int ctrlCnlNum)
        {
            lock (ctrlCnlPropsLock)
            {
                try
                {
                    // сохранение ссылки на свойства каналов,
                    // т.к. свойство CtrlCnlProps может быть изменено из другого потока
                    CtrlCnlProps[] ctrlCnlProps = clientCache.CtrlCnlProps;

                    // поиск свойств заданного канала
                    int ind = Array.BinarySearch(ctrlCnlProps, ctrlCnlNum, CtrlCnlProps.IntComp);
                    return ind >= 0 ? ctrlCnlProps[ind] : null;
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при получении свойств канала управления {0}" :
                        "Error getting output channel {0} properties", ctrlCnlNum);
                    return null;
                }
            }
        }
    }
}
