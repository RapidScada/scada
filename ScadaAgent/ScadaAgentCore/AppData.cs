/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : ScadaAgentCore
 * Summary  : Common data of the agent
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Scada.Agent
{
    /// <summary>
    /// Common data of the agent
    /// <para>Общие данные агента</para>
    /// </summary>
    public sealed class AppData
    {
        private static readonly AppData appDataInstance; // экземпляр объекта AppData


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static AppData()
        {
            appDataInstance = new AppData();
        }

        /// <summary>
        /// Конструктор, ограничивающий создание объекта из других классов
        /// </summary>
        private AppData()
        {
            AppDirs = new AppDirs();
            Log = new Log(Log.Formats.Full);
        }


        /// <summary>
        /// Получить директории приложения
        /// </summary>
        public AppDirs AppDirs { get; private set; }

        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public Log Log { get; private set; }


        /// <summary>
        /// Получить общие данные агента
        /// </summary>
        public static AppData GetAppData()
        {
            return appDataInstance;
        }
    }
}
