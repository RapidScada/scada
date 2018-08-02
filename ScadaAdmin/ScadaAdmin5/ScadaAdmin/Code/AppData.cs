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
 * Module   : Administrator
 * Summary  : Common data of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Common data of the application
    /// <para>Общие данные приложения</para>
    /// </summary>
    internal sealed class AppData
    {
        private static readonly AppData appDataInstance; // the instance of the AppData class


        /// <summary>
        /// Initializes the class
        /// </summary>
        static AppData()
        {
            appDataInstance = new AppData();
        }

        /// <summary>
        /// Initializes a new instance of the class and prevents creating objects of the class from outside
        /// </summary>
        private AppData()
        {
            Log = new Log(Log.Formats.Full);
        }


        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public Log Log { get; private set; }


        /// <summary>
        /// Gets the instance of the common data
        /// </summary>
        public static AppData GetInstance()
        {
            return appDataInstance;
        }
    }
}
