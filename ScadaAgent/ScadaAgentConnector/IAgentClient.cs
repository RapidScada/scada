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
 * Module   : ScadaAgentConnector
 * Summary  : Interface that represents a client of the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

namespace Scada.Agent.Connector
{
    /// <summary>
    /// Interface that represents a client of the Agent service.
    /// <para>Интерфейс, который представляет клиента службы Агента.</para>
    /// </summary>
    public interface IAgentClient
    {
        /// <summary>
        /// Gets a value indicating whether the connection is local.
        /// </summary>
        bool IsLocal { get; }
    }
}
