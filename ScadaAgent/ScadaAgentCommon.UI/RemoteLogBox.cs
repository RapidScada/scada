/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : ScadaAgentCommon.UI
 * Summary  : Provides displaying and updating both local and remote log
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Agent.Connector;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Agent.UI
{
    /// <summary>
    /// Provides displaying and updating both local and remote log.
    /// <para>Обеспечивает отображение и обновление как локального, так и удалённого журнала.</para>
    /// </summary>
    public class RemoteLogBox : LogBox
    {
        /// <summary>
        /// The remote path of the log.
        /// </summary>
        protected RelPath logPath;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RemoteLogBox(ListBox listBox, bool colorize = false)
            : base(listBox, colorize)
        {
            logPath = new RelPath();
            AgentClient = null;
        }


        /// <summary>
        /// Gets or sets the client of the Agent service.
        /// </summary>
        public IAgentClient AgentClient { get; set; }

        /// <summary>
        /// Gets or sets the remote path of the log.
        /// </summary>
        public RelPath LogPath
        {
            get
            {
                return logPath;
            }
            set
            {
                logPath = value;
                logFileAge = DateTime.MinValue;
            }
        }


        /// <summary>
        /// Refresh log content using Agent.
        /// </summary>
        protected void RefreshUsingAgent()
        {
            try
            {
                if (FullLogView && AgentClient.ReadLog(logPath, ref logFileAge, out ICollection<string> lines) ||
                    !FullLogView && AgentClient.ReadLog(logPath, LogViewSize, ref logFileAge, out lines))
                {
                    if (!listBox.IsDisposed)
                        SetLines(lines);
                }
            }
            catch (Exception ex)
            {
                if (!listBox.IsDisposed)
                    SetFirstLine(ex.Message);
            }
        }

        /// <summary>
        /// Refresh log content.
        /// </summary>
        public void Refresh()
        {
            if (AgentClient != null)
            {
                if (AgentClient.IsLocal)
                    RefreshFromFile();
                else
                    RefreshUsingAgent();
            }
        }
    }
}
