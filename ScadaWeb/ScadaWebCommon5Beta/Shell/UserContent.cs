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

using Scada.Web.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Reports = new List<ContentSpec>();
            DataWindows = new List<DataWindowSpec>();
        }


        /// <summary>
        /// Получить отчёты, доступные пользователю
        /// </summary>
        public List<ContentSpec> Reports { get; protected set; }

        /// <summary>
        /// Получить окна данных, доступные пользователю
        /// </summary>
        public List<DataWindowSpec> DataWindows { get; protected set; }

        
        /// <summary>
        /// Инициализировать контент пользователя
        /// </summary>
        public void Init(UserData userData)
        {
            if (userData == null)
                throw new ArgumentNullException("userData");

            try
            {
                // получение спецификаций отчётов и окон данных из плагинов
                Reports.Clear();
                DataWindows.Clear();

                if (userData.PluginSpecs != null)
                {
                    foreach (PluginSpec pluginSpec in userData.PluginSpecs)
                    {
                        if (pluginSpec.ReportSpecs != null)
                            Reports.AddRange(pluginSpec.ReportSpecs);
                        if (pluginSpec.DataWindowSpecs != null)
                            DataWindows.AddRange(pluginSpec.DataWindowSpecs);
                    }
                }

                Reports.Sort();
                DataWindows.Sort();
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при инициализации контента пользователя" :
                    "Error initializing user content");
            }
        }
    }
}
