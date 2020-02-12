/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaServerCommon
 * Summary  : The base class for server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2020
 */

using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using Utils;

namespace Scada.Server.Modules
{
    /// <summary>
    /// The base class for server module logic.
    /// <para>Родительский класс логики работы серверного модуля.</para>
    /// </summary>
    public abstract class ModLogic
    {
        /// <summary>
        /// Время ожидания остановки работы модуля, мс.
        /// </summary>
        public const int WaitForStop = 7000;

        private AppDirs appDirs; // директории приложения


        /// <summary>
        /// Конструктор.
        /// </summary>
        public ModLogic()
        {
            appDirs = new AppDirs();
            Settings = null;
            WriteToLog = null;
            ServerData = null;
            ServerCommands = null;
        }


        /// <summary>
        /// Получить имя модуля.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Получить или установить директории приложения.
        /// </summary>
        public AppDirs AppDirs
        {
            get
            {
                return appDirs;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                appDirs = value;
            }
        }

        /// <summary>
        /// Получить или установить настройки SCADA-Сервера.
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        /// Получить или установить метод записи в журнал приложения.
        /// </summary>
        public Log.WriteActionDelegate WriteToLog { get; set; }

        /// <summary>
        /// Получить или установить объект для доступа к данным сервера.
        /// </summary>
        public IServerData ServerData { get; set; }

        /// <summary>
        /// Получить или установить объект для отправки команд.
        /// </summary>
        public IServerCommands ServerCommands { get; set; }


        /// <summary>
        /// Выполнить действия при запуске работы сервера.
        /// </summary>
        public virtual void OnServerStart()
        {
            if (WriteToLog != null)
                WriteToLog(string.Format(Localization.UseRussian ? "Запуск работы модуля {0}" : 
                    "Start {0} module", Name), Log.ActTypes.Action);
        }

        /// <summary>
        /// Выполнить действия при остановке работы сервера.
        /// </summary>
        public virtual void OnServerStop()
        {
            if (WriteToLog != null)
                WriteToLog(string.Format(Localization.UseRussian ? "Завершение работы модуля {0}" :
                    "Stop {0} module", Name), Log.ActTypes.Action);
        }
        
        /// <summary>
        /// Performs actions after receiving and before processing new current data.
        /// </summary>
        public virtual void OnCurDataProcessing(SrezTableLight.Srez receivedSrez)
        {
        }

        /// <summary>
        /// Выполнить действия после обработки новых текущих данных.
        /// </summary>
        /// <remarks>Номера каналов упорядочены по возрастанию.
        /// Вычисление дорасчётных каналов текущего среза в момент вызова метода не выполнено.</remarks>
        public virtual void OnCurDataProcessed(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
        }

        /// <summary>
        /// Выполнить действия после вычисления дорасчётных каналов текущего среза.
        /// </summary>
        /// <remarks>Номера каналов упорядочены по возрастанию.</remarks>
        public virtual void OnCurDataCalculated(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
        }

        /// <summary>
        /// Performs actions after receiving and before processing new archive data.
        /// </summary>
        public virtual void OnArcDataProcessing(SrezTableLight.Srez receivedSrez)
        {
        }

        /// <summary>
        /// Выполнить действия после обработки новых архивных данных.
        /// </summary>
        /// <remarks>
        /// Номера каналов упорядочены по возрастанию.
        /// Вычисление дорасчётных каналов архивного среза в момент вызова метода завершено.
        /// Параметр arcSrez равен null, если запись архивных срезов отключена.
        /// </remarks>
        public virtual void OnArcDataProcessed(int[] cnlNums, SrezTableLight.Srez arcSrez)
        {
        }

        /// <summary>
        /// Выполнить действия при создании события.
        /// </summary>
        /// <remarks>Метод вызывается до записи события на диск, поэтому свойства события можно изменить.</remarks>
        public virtual void OnEventCreating(EventTableLight.Event ev)
        {
        }

        /// <summary>
        /// Выполнить действия после создания события и записи на диск.
        /// </summary>
        /// <remarks>Метод вызывается после записи на события диск.</remarks>
        public virtual void OnEventCreated(EventTableLight.Event ev)
        {
        }

        /// <summary>
        /// Выполнить действия после квитирования события.
        /// </summary>
        public virtual void OnEventChecked(DateTime date, int evNum, int userID)
        {
        }

        /// <summary>
        /// Выполнить действия после приёма команды ТУ.
        /// </summary>
        /// <remarks>Метод вызывается после приёма команды ТУ от подключенных клиентов и 
        /// не вызывается после передачи команды ТУ серверными модулями.</remarks>
        public virtual void OnCommandReceived(int ctrlCnlNum, Command cmd, int userID, ref bool passToClients)
        {
        }

        /// <summary>
        /// Проверить имя и пароль пользователя, получить его роль.
        /// </summary>
        /// <remarks>Если пароль пустой, то он не проверяется.</remarks>
        public virtual bool ValidateUser(string username, string password, out int roleID, out bool handled)
        {
            roleID = BaseValues.Roles.Err;
            handled = false;
            return false;
        }
    }
}
