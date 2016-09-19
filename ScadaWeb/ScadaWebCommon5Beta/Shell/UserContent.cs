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
 * Summary  : Content accessible to the web application user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using System;
using System.Collections.Generic;
using Utils;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Content accessible to the web application user
    /// <para>Контент, доступный пользователю веб-приложения</para>
    /// </summary>
    public class UserContent
    {
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected UserContent()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserContent(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this.log = log;
            ReportItems = new List<ReportItem>();
            DataWndItems = new List<DataWndItem>();
        }


        /// <summary>
        /// Получить элементы отчётов, доступные пользователю
        /// </summary>
        public List<ReportItem> ReportItems { get; protected set; }

        /// <summary>
        /// Получить элементы окон данных, доступные пользователю
        /// </summary>
        public List<DataWndItem> DataWndItems { get; protected set; }

        
        /// <summary>
        /// Инициализировать доступный контент пользователя
        /// </summary>
        public void Init(UserData userData, DataAccess dataAccess)
        {
            if (userData == null)
                throw new ArgumentNullException("userData");

            try
            {
                ReportItems.Clear();
                DataWndItems.Clear();

                /*if (userData.PluginSpecs != null)
                {
                    foreach (PluginSpec pluginSpec in userData.PluginSpecs)
                    {
                        if (pluginSpec.ReportSpecs != null)
                            Reports.AddRange(pluginSpec.ReportSpecs);
                        if (pluginSpec.DataWndSpecs != null)
                            DataWindows.AddRange(pluginSpec.DataWndSpecs);
                    }
                }*/

                ReportItems.Sort();
                DataWndItems.Sort();
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при инициализации доступного контента пользователя" :
                    "Error initializing accessible user content");
            }
        }
    }
}
