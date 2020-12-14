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
 * Module   : Server Shell
 * Summary  : Represents an environment of the Server shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using Scada.Agent.Connector;
using Scada.Client;
using Scada.Server.Modules;
using System;
using System.Collections.Generic;
using Utils;

namespace Scada.Server.Shell.Code
{
    /// <summary>
    /// Represents an environment of the Server shell.
    /// <para>Представляет среду оболочки Сервера.</para>
    /// </summary>
    public class ServerEnvironment
    {
        /// <summary>
        /// The user interface of the modules accessed by full file name.
        /// </summary>
        protected Dictionary<string, ModView> moduleViews;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerEnvironment(AppDirs appDirs, Log errLog)
        {
            moduleViews = new Dictionary<string, ModView>();

            AppDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            ErrLog = errLog ?? throw new ArgumentNullException("errLog");
            AgentClient = null;
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; protected set; }

        /// <summary>
        /// Gets the application error log.
        /// </summary>
        public Log ErrLog { get; protected set; }

        /// <summary>
        /// Gets or sets the client of the Agent service.
        /// </summary>
        /// <remarks>Null allowed.</remarks>
        public IAgentClient AgentClient { get; set; }


        /// <summary>
        /// Gets the user interface of the driver.
        /// </summary>
        public ModView GetModuleView(string dllPath)
        {
            if (!moduleViews.TryGetValue(dllPath, out ModView modView))
            {
                modView = ModFactory.GetModView(dllPath);
                modView.AppDirs = AppDirs;
                moduleViews[dllPath] = modView;
            }

            return modView;
        }

        /// <summary>
        /// Gets the object that communicates with the Server service.
        /// </summary>
        public ServerComm GetServerComm(Settings settings)
        {
            if (AgentClient == null || settings == null)
                return null;

            CommSettings commSettings = AgentClient.CreateCommSettings();
            commSettings.ServerPort = settings.TcpPort;
            return new ServerComm(commSettings, new LogStub());
        }
    }
}
