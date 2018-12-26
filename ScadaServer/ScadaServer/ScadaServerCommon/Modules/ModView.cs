/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : The base class for server module user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2017
 */

using Scada.Client;
using System;

namespace Scada.Server.Modules
{
    /// <summary>
    /// The base class for server module user interface
    /// <para>Родительский класс пользовательского интерфейса серверного модуля</para>
    /// </summary>
    public abstract class ModView
    {
        private AppDirs appDirs; // директории приложения


        /// <summary>
        /// Конструктор
        /// </summary>
        public ModView()
        {
            appDirs = new AppDirs();
            ServerComm = null;
            CanShowProps = false;
        }


        /// <summary>
        /// Получить описание модуля
        /// </summary>
        public abstract string Descr { get; }

        /// <summary>
        /// Получить версию модуля
        /// </summary>
        /// <remarks>В будущем сделать данное свойство abstract</remarks>
        public virtual string Version
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Получить или установить директории приложения
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
        /// Получить или установить объект для обмена данными со SCADA-Сервером
        /// </summary>
        public ServerComm ServerComm { get; set; }

        /// <summary>
        /// Получить возможность отображения свойств модуля
        /// </summary>
        public bool CanShowProps { get; protected set; }


        /// <summary>
        /// Отобразить свойства модуля
        /// </summary>
        public virtual void ShowProps()
        {
        }
    }
}
