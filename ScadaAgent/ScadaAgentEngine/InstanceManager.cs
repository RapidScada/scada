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
 * Summary  : System instance manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Concurrent;
using Utils;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// System instance manager
    /// <para>Менеджер экземпляров систем</para>
    /// </summary>
    public class InstanceManager
    {
        private Settings settings; // настройки агента
        private ILog log;          // журнал приложения
        private ConcurrentDictionary<string, object> locks; // объекты для блокировки экземпларов систем


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private InstanceManager()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public InstanceManager(Settings settings, ILog log)
        {
            this.settings = settings ?? throw new ArgumentNullException("settings");
            this.log = log ?? throw new ArgumentNullException("log");
            locks = new ConcurrentDictionary<string, object>();
        }


        /// <summary>
        /// Получить экземпляр системы по наименованию
        /// </summary>
        public ScadaInstance GetScadaInstance(string name)
        {
            if (settings.Instances.TryGetValue(name, out ScadaInstanceSettings instanceSettings))
            {
                object syncRoot = locks.GetOrAdd(name, (key) => { return new object(); });
                ScadaInstance scadaInstance = new ScadaInstance(instanceSettings, syncRoot, log);
                return scadaInstance;
            }
            else
            {
                return null;
            }
        }
    }
}
