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
        }


        /// <summary>
        /// Получить представление из кэша или от сервера
        /// </summary>
        public T GetView<T>(int viewID, bool throwOnError = false) where T : BaseView
        {
            // TODO: нужно записать загруженное представление в кэш
            try
            {
                T view;
                BaseView baseView = GetViewFromCache(viewID);

                if (baseView == null)
                {
                    ViewProps viewProps = dataAccess.GetViewProps(viewID);

                    if (viewProps == null)
                    {
                        view = null;
                        if (throwOnError)
                            throw new ScadaException(Localization.UseRussian ?
                                "Отсутствуют свойства представления." : 
                                "View properties are missing.");
                    }
                    else 
                    {
                        // создание и загрузка нового представления
                        view = (T)Activator.CreateInstance(typeof(T));
                        if (!serverComm.ReceiveView(viewProps.FileName, view))
                        {
                            view = null;
                            if (throwOnError)
                                throw new ScadaException(Localization.UseRussian ?
                                    "Представление не принято от сервера." : 
                                    "View is not received from the server.");
                        }
                    }
                }
                else
                {
                    view = baseView as T;
                    if (view == null && throwOnError)
                        throw new ScadaException(Localization.UseRussian ?
                            "Некорректный тип представления." : 
                            "Incorrect view type.");
                }


                return view;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Ошибка при получении представления с id={0} из кэша или от сервера" :
                    "Error getting view with id={0} from cache or from server", viewID);
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
            return null;
        }
    }
}