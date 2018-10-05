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
 * Module   : ScadaAgentEngine
 * Summary  : Common data of the agent
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System.Text;
using Utils;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Common data of the agent
    /// <para>Общие данные агента</para>
    /// </summary>
    public sealed class AppData
    {
        /// <summary>
        /// Имя файла журнала приложения без директории
        /// </summary>
        private const string LogFileName = "ScadaAgent.log";
        /// <summary>
        /// Имя файла информации о работе приложения
        /// </summary>
        public const string InfoFileName = "ScadaAgent.txt";

        private static readonly AppData appDataInstance; // экземпляр объекта AppData
        private int tempFileNameCntr; // счётчик временных файлов


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
            tempFileNameCntr = 0;

            AppDirs = new AppDirs();
            Settings = new Settings();
            Log = new Log(Log.Formats.Full) { Encoding = Encoding.UTF8 };
            SessionManager = new SessionManager(Log);
            InstanceManager = new InstanceManager(Settings, Log);
        }


        /// <summary>
        /// Получить директории приложения
        /// </summary>
        public AppDirs AppDirs { get; private set; }

        /// <summary>
        /// Получить настройки агента
        /// </summary>
        public Settings Settings { get; private set; }

        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public Log Log { get; private set; }

        /// <summary>
        /// Получить менеджер сессий
        /// </summary>
        public SessionManager SessionManager { get; private set; }

        /// <summary>
        /// Получить менеджер экземпляров систем
        /// </summary>
        public InstanceManager InstanceManager { get; private set; }


        /// <summary>
        /// Инициализировать общие данные агента
        /// </summary>
        public void Init(string exeDir)
        {
            AppDirs.Init(exeDir);
            Log.FileName = AppDirs.LogDir + LogFileName;
        }

        /// <summary>
        /// Получить имя временного файла
        /// </summary>
        public string GetTempFileName(string prefix = "", string extension = "")
        {
            return AppDirs.TempDir +
                (string.IsNullOrEmpty(prefix) ? "temp" : prefix) + 
                "-" + (++tempFileNameCntr) + 
                "." + (string.IsNullOrEmpty(extension) ? "tmp" : extension);
        }

        /// <summary>
        /// Получить общие данные агента
        /// </summary>
        public static AppData GetInstance()
        {
            return appDataInstance;
        }
    }
}
