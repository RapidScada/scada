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
 * Module   : Communicator Shell
 * Summary  : Represents an environment of the Communicator shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Agent.Connector;
using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using Utils;

namespace Scada.Comm.Shell.Code
{
    /// <summary>
    /// Represents an environment of the Communicator shell.
    /// <para>Представляет среду оболочки Коммуникатора.</para>
    /// </summary>
    public class CommEnvironment
    {
        /// <summary>
        /// Gets or sets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; set; }

        /// <summary>
        /// Gets or sets the user interface of the drivers accessed by full file name.
        /// </summary>
        public Dictionary<string, KPView> KPViews { get; set; }

        /// <summary>
        /// Gets or sets the client of the Agent service.
        /// </summary>
        /// <remarks>Null allowed.</remarks>
        public IAgentClient AgentClient { get; set; }

        /// <summary>
        /// Gets the application error log.
        /// </summary>
        public Log ErrLog { get; set; }


        /// <summary>
        /// Gets information about available driver files.
        /// </summary>
        internal FileInfo[] GetDrivers()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AppDirs.KPDir);

            return dirInfo.Exists ?
                dirInfo.GetFiles("Kp*.dll", SearchOption.TopDirectoryOnly) :
                new FileInfo[0];
        }

        /// <summary>
        /// Validates the environment and throws an exception if it is incorrect.
        /// </summary>
        public void Validate()
        {
            if (AppDirs == null)
                throw new InvalidOperationException("AppDirs must not be null.");

            if (KPViews == null)
                throw new InvalidOperationException("KPViews must not be null.");

            if (ErrLog == null)
                throw new InvalidOperationException("ErrLog must not be null.");
        }

        /// <summary>
        /// Gets the user interface of the driver.
        /// </summary>
        public KPView GetKPView(string dllPath)
        {
            if (!KPViews.TryGetValue(dllPath, out KPView kpView))
            {
                kpView = KPFactory.GetKPView(dllPath);
                kpView.AppDirs = AppDirs;
                KPViews[dllPath] = kpView;
            }

            return kpView;
        }

        /// <summary>
        /// Gets the user interface of the particular device.
        /// </summary>
        public KPView GetKPView(string dllPath, int kpNum, KPView.KPProperties kpProps)
        {
            KPView commonKpView = GetKPView(dllPath);
            KPView kpView = KPFactory.GetKPView(commonKpView.GetType(), kpNum);
            kpView.KPProps = kpProps;
            kpView.AppDirs = AppDirs;
            return kpView;
        }
    }
}
