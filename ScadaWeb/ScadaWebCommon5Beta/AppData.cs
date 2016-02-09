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
 * Summary  : Common data of the web application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2016
 */

using System.Text;
using System.Web;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Common data of the application
    /// <para>Общие данные приложения</para>
    /// </summary>
    public static class AppData
	{
        /// <summary>
        /// Имя файла журнала приложения без директории
        /// </summary>
        public const string LogFileName = "ScadaWeb.log";

        private static readonly object appDataLock;  // объект для синхронизации доступа к данным приложения
        

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static AppData()
		{
            appDataLock = new object();

            Inited = false;
            AppDirs = new AppDirs();
            Log = new Log(Log.Formats.Full);
        }


        /// <summary>
        /// Получить признак инициализации общих данных приложения
        /// </summary>
        public static bool Inited { get; private set; }

        /// <summary>
        /// Получить директории приложения
        /// </summary>
        public static AppDirs AppDirs { get; private set; }
        
        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public static Log Log { get; private set; }


        /// <summary>
        /// Инициализировать общие данные приложения
        /// </summary>
        public static void InitAppData()
        {
            ScadaWebUtils.CheckSessionExists();

            lock (appDataLock)
            {
                if (!Inited)
                {
                    Inited = true;

                    // инициализация директорий приложения
                    AppDirs.Init(HttpContext.Current.Request.PhysicalApplicationPath);

                    // настройка журнала приложения
                    Log.FileName = AppDirs.LogDir + LogFileName;
                    Log.Encoding = Encoding.UTF8;
                    Log.WriteBreak();
                    Log.WriteAction(Localization.UseRussian ? 
                        "Инициализация общих данных приложения" :
                        "Initialize common application data", Log.ActTypes.Action);
                }
            }
        }
    }
}