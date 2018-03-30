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
 * Summary  : Object for manipulating a system instance
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;

namespace Scada.Agent
{
    /// <summary>
    /// Object for manipulating a system instance
    /// <para>Объект для манипуляций с экземпляром системы</para>
    /// </summary>
    public class ScadaInstance
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private ScadaInstance()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ScadaInstance(string name, object syncRoot)
        {
            Name = name ?? throw new ArgumentNullException("name");
            SyncRoot = syncRoot ?? throw new ArgumentNullException("syncRoot");
        }


        /// <summary>
        /// Получить наименование
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Получить или установить объект для синхронизации доступа к экземпляру системы
        /// </summary>
        public object SyncRoot { get; private set; }


        /// <summary>
        /// Проверить пароль и права пользователя
        /// </summary>
        public bool ValidateUser(string username, string encryptedPassword, out string errMsg)
        {
            // ограничить кол-во попыток
            errMsg = "";
            return true;
        }

        /// <summary>
        /// Упаковать конфигурацию в архив
        /// </summary>
        public bool PackConfig(string destFileName, ConfigOptions configOptions)
        {
            return false;
        }

        /// <summary>
        /// Распаковать архив конфигурации
        /// </summary>
        public bool UnpackConfig(string srcFileName, ConfigOptions configOptions)
        {
            return false;
        }
    }
}
