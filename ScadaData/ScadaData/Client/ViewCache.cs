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
 * Summary  : Cache of views
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
    /// Cache of views
    /// <para>Кэш представлений</para>
    /// </summary>
    public class ViewCache
    {
        /// <summary>
        /// Вместимость кеша неограниченная по количеству элементов
        /// </summary>
        protected const int Capacity = int.MaxValue;
        /// <summary>
        /// Период хранения в кеше с момента последнего доступа
        /// </summary>
        protected static readonly TimeSpan StorePeriod = TimeSpan.FromMinutes(10);
        /// <summary>
        /// Время актуальности представления в кэше
        /// </summary>
        protected static readonly TimeSpan ViewValidSpan = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Объект для обмена данными со SCADA-Сервером
        /// </summary>
        protected readonly ServerComm serverComm;
        /// <summary>
        /// Объект для потокобезопасного доступа к данным кеша клиентов
        /// </summary>
        protected readonly DataAccess dataAccess;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;

        /// <summary>
        /// Объект кеша представлений
        /// </summary>
        protected Cache<int, BaseView> cache;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected ViewCache()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewCache(ServerComm serverComm, DataAccess dataAccess, Log log)
        {
            if (serverComm == null)
                throw new ArgumentNullException("serverComm");
            if (dataAccess == null)
                throw new ArgumentNullException("dataAccess");
            if (log == null)
                throw new ArgumentNullException("log");

            this.serverComm = serverComm;
            this.dataAccess = dataAccess;
            this.log = log;

            cache = new Cache<int, BaseView>(StorePeriod, Capacity);
        }


        /// <summary>
        /// Получить представление из кэша или от сервера
        /// </summary>
        public T GetView<T>(int viewID, bool throwOnError = false) where T : BaseView
        {
            try
            {
                T view = null;

                // получение представления из кеша
                DateTime utcNowDT = DateTime.UtcNow;
                Cache<int, BaseView>.CacheItem cacheItem = cache.GetItem(viewID, utcNowDT);
                BaseView viewFromCache;
                DateTime viewAge;    // время изменения файла представления
                bool viewIsNotValid; // представление могло устареть

                if (cacheItem == null)
                {
                    viewFromCache = null;
                    viewAge = DateTime.MinValue;
                    viewIsNotValid = true;
                }
                else
                {
                    viewFromCache = cacheItem.Value;
                    viewAge = cacheItem.ValueAge;
                    viewIsNotValid = utcNowDT - cacheItem.ValueRefrDT > ViewValidSpan;
                }

                // получение представления от сервера
                if (viewFromCache == null || viewIsNotValid)
                {
                    ViewProps viewProps = dataAccess.GetViewProps(viewID);

                    if (viewProps == null)
                    {
                        if (throwOnError)
                            throw new ScadaException(Localization.UseRussian ?
                                "Отсутствуют свойства представления." : 
                                "View properties are missing.");
                    }
                    else 
                    {
                        DateTime newViewAge = serverComm.ReceiveFileAge(ServerComm.Dirs.Itf, viewProps.FileName);

                        if (newViewAge == DateTime.MinValue)
                        {
                            if (throwOnError)
                                throw new ScadaException(Localization.UseRussian ?
                                    "Не удалось принять время изменения файла представления." :
                                    "Unable to receive view file modification time.");
                        }
                        else if (newViewAge != viewAge) // файл представления изменён
                        {
                            // создание и загрузка нового представления
                            view = (T)Activator.CreateInstance(typeof(T));
                            if (serverComm.ReceiveView(viewProps.FileName, view))
                            {
                                if (cacheItem == null)
                                    // добавление представления в кеш
                                    cache.AddValue(viewID, view, newViewAge, utcNowDT);
                                else
                                    // обновление представления в кеше
                                    cache.UpdateItem(cacheItem, view, newViewAge, utcNowDT);
                            }
                            else
                            {
                                if (throwOnError)
                                    throw new ScadaException(Localization.UseRussian ?
                                        "Не удалось принять представление." :
                                        "Unable to receive view.");
                            }
                        }
                    }
                }
                
                // использование представления из кеша
                if (view == null && viewFromCache != null)
                {
                    view = viewFromCache as T;
                    if (view == null && throwOnError)
                        throw new ScadaException(Localization.UseRussian ?
                            "Несоответствие типа представления." :
                            "View type mismatch.");
                }

                // привязка свойств каналов после получения представления или при изменении базы конфигурации
                if (view != null)
                    dataAccess.BindCnlProps(view);

                return view;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Ошибка при получении представления с ид.={0} из кэша или от сервера" :
                    "Error getting view with ID={0} from the cache or from the server", viewID);
                log.WriteException(ex, errMsg);

                if (throwOnError)
                    throw new ScadaException(errMsg);
                else
                    return null;
            }
        }

        /// <summary>
        /// Получить уже загруженное представление только из кэша
        /// </summary>
        public BaseView GetViewFromCache(int viewID)
        {
            try
            {
                Cache<int, BaseView>.CacheItem cacheItem = cache.GetItem(viewID, DateTime.UtcNow);
                return cacheItem == null ? null : cacheItem.Value;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении представления с ид.={0} из кэша" :
                    "Error getting view with ID={0} from the cache", viewID);
                return null;
            }
        }
    }
}