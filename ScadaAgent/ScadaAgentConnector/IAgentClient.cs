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

using Scada.Client;
using System;
using System.Collections.Generic;

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


        /// <summary>
        /// Tests the connection with Agent.
        /// </summary>
        bool TestConnection(out string errMsg);

        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        bool ControlService(ServiceApp serviceApp, ServiceCommand command);

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        bool GetServiceStatus(ServiceApp serviceApp, out ServiceStatus serviceStatus);

        /// <summary>
        /// Gets available parts of the configuration.
        /// </summary>
        bool GetAvailableConfig(out ConfigParts configParts);

        /// <summary>
        /// Downloads the configuration to the file.
        /// </summary>
        void DownloadConfig(string destFileName, ConfigOptions configOptions);

        /// <summary>
        /// Uploads the configuration from the file.
        /// </summary>
        void UploadConfig(string srcFileName, ConfigOptions configOptions);

        /// <summary>
        /// Reads the log file.
        /// </summary>
        bool ReadLog(RelPath relPath, ref DateTime fileAge, out ICollection<string> lines);

        /// <summary>
        /// Reads the rest of the log file.
        /// </summary>
        bool ReadLog(RelPath relPath, long offsetFromEnd, ref DateTime fileAge, out ICollection<string> lines);

        /// <summary>
        /// Creates new settings for connecting to Server based on the connection settings of the Agent.
        /// </summary>
        CommSettings CreateCommSettings();
    }
}
