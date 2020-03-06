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
 * Module   : Communicator Shell
 * Summary  : Represents an environment of the Communicator shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
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
        /// The user interface of the drivers accessed by full file name.
        /// </summary>
        protected Dictionary<string, KPView> kpViews;

        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommEnvironment(AppDirs appDirs, Log errLog)
        {
            kpViews = new Dictionary<string, KPView>();

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
        /// Gets the user interface of the driver.
        /// </summary>
        public KPView GetKPView(string dllPath)
        {
            if (!kpViews.TryGetValue(dllPath, out KPView kpView))
            {
                kpView = KPFactory.GetKPView(dllPath);
                kpView.AppDirs = AppDirs;
                kpViews[dllPath] = kpView;
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
            kpView.KPProps = kpProps ?? throw new ArgumentNullException("kpProps");
            kpView.AppDirs = AppDirs;
            return kpView;
        }

        /// <summary>
        /// Gets a user interface object for the device.
        /// </summary>
        public bool TryGetKPView(Settings.KP kp, bool common, SortedList<string, string> customParams, 
            out KPView kpView, out string errMsg)
        {
            try
            {
                string dllPath = Path.Combine(AppDirs.KPDir, kp.Dll);
                kpView = common ?
                    GetKPView(dllPath) :
                    GetKPView(dllPath, kp.Number, new KPView.KPProperties(customParams, kp.CmdLine));
                errMsg = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrLog.WriteException(ex);
                kpView = null;
                errMsg = ex.Message;
                return false;
            }
        }
    }
}
