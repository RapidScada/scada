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
 * Module   : ScadaWebCommon
 * Summary  : Cache of views
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using System;
using Utils;

namespace Scada.Web
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
        public ViewCache(ServerComm serverComm, Log log)
        {
            if (serverComm == null)
                throw new ArgumentNullException("serverComm");
            if (log == null)
                throw new ArgumentNullException("log");

            this.serverComm = serverComm;
            this.log = log;
        }
    }
}