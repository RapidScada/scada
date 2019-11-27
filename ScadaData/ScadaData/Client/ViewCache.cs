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

using Scada.Data.Models;
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
        /// Вместимость кэша неограниченная по количеству элементов
        /// </summary>
        protected const int Capacity = int.MaxValue;
        /// <summary>
        /// Период хранения в кэше с момента последнего доступа
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
        /// Объект для потокобезопасного доступа к данным кэша клиентов
        /// </summary>
        protected readonly DataAccess dataAccess;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;


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

            Cache = new Cache<int, BaseView>(StorePeriod, Capacity);
        }


        /// <summary>
        /// Получить объект кэша представлений
        /// </summary>
        /// <remarks>Использовать вне данного класса только для получения состояния кэша</remarks>
        public Cache<int, BaseView> Cache { get; protected set; }


        /// <summary>
        /// Получить свойства представления, вызвав исключение в случае неудачи
        /// </summary>
        protected UiObjProps GetViewProps(int viewID)
        {
            UiObjProps viewProps = dataAccess.GetUiObjProps(viewID);

            if (viewProps == null)
            {
                throw new ScadaException(Localization.UseRussian ?
                    "Отсутствуют свойства представления." :
                    "View properties are missing.");
            }

            return viewProps;
        }

        /// <summary>
        /// Загрузить представление от сервера
        /// </summary>
        protected bool LoadView(Type viewType, int viewID, DateTime viewAge, 
            ref BaseView view, out DateTime newViewAge)
        {
            UiObjProps viewProps = GetViewProps(viewID);
            newViewAge = serverComm.ReceiveFileAge(ServerComm.Dirs.Itf, viewProps.Path);

            if (newViewAge == DateTime.MinValue)
            {
                throw new ScadaException(Localization.UseRussian ?
                    "Не удалось принять время изменения файла представления." :
                    "Unable to receive view file modification time.");
            }
            else if (newViewAge != viewAge) // файл представления изменён
            {
                // создание и загрузка нового представления
                if (view == null)
                    view = (BaseView)Activator.CreateInstance(viewType);

                view.SetArgs(viewProps.Args);

                if (serverComm.ReceiveView(viewProps.Path, view))
                {
                    view.UpdateTitle(viewProps.ShortTitle);
                    return true;
                }
                else
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Не удалось принять представление." :
                        "Unable to receive view.");
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Получить представление из кэша или от сервера
        /// </summary>
        /// <remarks>Метод используется, если тип предсталения неизвестен на момент компиляции</remarks>
        public BaseView GetView(Type viewType, int viewID, bool throwOnError = false)
        {
            try
            {
                if (viewType == null)
                    throw new ArgumentNullException("viewType");

                // получение представления из кэша
                DateTime utcNowDT = DateTime.UtcNow;
                Cache<int, BaseView>.CacheItem cacheItem = Cache.GetOrCreateItem(viewID, utcNowDT);

                // блокировка доступа только к одному представлению
                lock (cacheItem)
                {
                    BaseView view = null;                     // представление, которое необходимо получить
                    BaseView viewFromCache = cacheItem.Value; // представление из кэша
                    DateTime viewAge = cacheItem.ValueAge;    // время изменения файла представления
                    DateTime newViewAge;                      // новое время изменения файла представления

                    if (viewFromCache == null)
                    {
                        // создание нового представления
                        view = (BaseView)Activator.CreateInstance(viewType);

                        if (view.StoredOnServer)
                        {
                            if (LoadView(viewType, viewID, viewAge, ref view, out newViewAge))
                                Cache.UpdateItem(cacheItem, view, newViewAge, utcNowDT);
                        }
                        else
                        {
                            UiObjProps viewProps = GetViewProps(viewID);
                            view.Path = viewProps.Path;
                            view.SetArgs(viewProps.Args);
                            view.UpdateTitle(viewProps.ShortTitle);
                            Cache.UpdateItem(cacheItem, view, DateTime.Now, utcNowDT);
                        }
                    }
                    else if (viewFromCache.StoredOnServer)
                    {
                        // представление могло устареть
                        bool viewIsNotValid = utcNowDT - cacheItem.ValueRefrDT > ViewValidSpan;

                        if (viewIsNotValid && LoadView(viewType, viewID, viewAge, ref view, out newViewAge))
                            Cache.UpdateItem(cacheItem, view, newViewAge, utcNowDT);
                    }

                    // использование представления из кэша
                    if (view == null && viewFromCache != null)
                    {
                        if (viewFromCache.GetType().Equals(viewType))
                            view = viewFromCache;
                        else
                            throw new ScadaException(Localization.UseRussian ?
                                "Несоответствие типа представления." :
                                "View type mismatch.");
                    }

                    // привязка свойств каналов или обновление существующей привязки
                    if (view != null)
                        dataAccess.BindCnlProps(view);

                    return view;
                }
            }
            catch (Exception ex)
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Ошибка при получении представления с ид.={0} из кэша или от сервера: {1}" :
                    "Error getting view with ID={0} from the cache or from the server: {1}", viewID, ex.Message);
                log.WriteException(ex, errMsg);

                if (throwOnError)
                    throw new ScadaException(errMsg);
                else
                    return null;
            }
        }

        /// <summary>
        /// Получить представление из кэша или от сервера
        /// </summary>
        public T GetView<T>(int viewID, bool throwOnError = false) where T : BaseView
        {
            return GetView(typeof(T), viewID, throwOnError) as T;
        }

        /// <summary>
        /// Получить уже загруженное представление только из кэша
        /// </summary>
        public BaseView GetViewFromCache(int viewID, bool throwOnFail = false)
        {
            try
            {
                Cache<int, BaseView>.CacheItem cacheItem = Cache.GetItem(viewID, DateTime.UtcNow);
                BaseView view = cacheItem == null ? null : cacheItem.Value;

                if (view == null && throwOnFail)
                    throw new ScadaException(string.Format(Localization.UseRussian ?
                        "Представление не найдено в кэше" :
                        "The view is not found in the cache", viewID));

                return view;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Ошибка при получении представления с ид.={0} из кэша: {1}" :
                    "Error getting view with ID={0} from the cache: {1}", viewID, ex.Message);
                log.WriteException(ex, errMsg);

                if (throwOnFail)
                    throw new ScadaException(errMsg);
                else
                    return null;
            }
        }
    }
}