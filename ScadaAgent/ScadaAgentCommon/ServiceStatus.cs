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
 * Module   : ScadaAgentCommon
 * Summary  : Service statuses
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

namespace Scada.Agent
{
    /// <summary>
    /// Service statuses
    /// <para>Статусы службы</para>
    /// </summary>
    public enum ServiceStatus
    {
        /// <summary>
        /// Не определён
        /// </summary>
        Undefined,

        /// <summary>
        /// Норма
        /// </summary>
        Normal,

        /// <summary>
        /// Остановлен
        /// </summary>
        Stopped,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error
    }
}
