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
 * Summary  : Agent settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Agent
{
    /// <summary>
    /// Agent settings
    /// <para>Настройки агента</para>
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Agent settings for the system instance
        /// <para>Настройки агента для экземплара системы</para>
        /// </summary>
        public class ScadaInstance
        {
            public string Name { get; set; }

            public string Directory { get; set; }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        public Settings()
        {
            Culture = "";
            Instances = new List<ScadaInstance>();
        }


        /// <summary>
        /// Получить или установить культуру агента
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Получить настройки экземпляров систем
        /// </summary>
        public List<ScadaInstance> Instances { get; private set; }
    }
}
