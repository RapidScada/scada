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
 * Module   : Server Shell
 * Summary  : Form for displaying Server stats
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Agent;
using Scada.Agent.Connector;
using Scada.Agent.UI;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for displaying Server stats.
    /// <para>Форма для отображения статистики Сервера.</para>
    /// </summary>
    public partial class FrmStats : Form
    {
        private readonly ServerEnvironment environment; // the application environment
        private RemoteLogBox stateBox; // object to refresh state
        private RemoteLogBox logBox;   // object to refresh log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmStats()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmStats(ServerEnvironment environment)
            : this()
        {
            this.environment = environment ?? throw new ArgumentNullException("environment");
            stateBox = new RemoteLogBox(lbState) { FullLogView = true };
            logBox = new RemoteLogBox(lbLog) { AutoScroll = true };
        }


        /// <summary>
        /// Initializes the log refresh process.
        /// </summary>
        private void InitRefresh()
        {
            IAgentClient agentClient = environment.AgentClient;
            stateBox.AgentClient = agentClient;
            logBox.AgentClient = agentClient;

            if (agentClient == null)
            {
                stateBox.SetFirstLine(ServerShellPhrases.SetProfile);
                logBox.SetFirstLine(ServerShellPhrases.SetProfile);
                tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
            }
            else
            {
                stateBox.SetFirstLine(ServerShellPhrases.Loading);
                logBox.SetFirstLine(ServerShellPhrases.Loading);

                if (agentClient.IsLocal)
                {
                    tmrRefresh.Interval = ScadaUiUtils.LogLocalRefreshInterval;
                    stateBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, ServerUtils.AppStateFileName);
                    logBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, ServerUtils.AppLogFileName);
                }
                else
                {
                    tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
                    stateBox.LogPath = new RelPath(ConfigParts.Server, AppFolder.Log, ServerUtils.AppStateFileName);
                    logBox.LogPath = new RelPath(ConfigParts.Server, AppFolder.Log, ServerUtils.AppLogFileName);
                }
            }
        }


        private void FrmStats_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            InitRefresh();
            tmrRefresh.Start();
        }

        private void FrmStats_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmrRefresh.Stop();
        }

        private void FrmStats_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Interval = stateBox.AgentClient != null && stateBox.AgentClient.IsLocal ?
                    ScadaUiUtils.LogLocalRefreshInterval :
                    ScadaUiUtils.LogRemoteRefreshInterval;
            }
            else
            {
                tmrRefresh.Interval = ScadaUiUtils.LogInactiveTimerInterval;
            }
        }

        private async void tmrRefresh_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Stop();

                if (stateBox.AgentClient == environment.AgentClient)
                {
                    await Task.Run(() => stateBox.Refresh());

                    if (!chkPause.Checked)
                        await Task.Run(() => logBox.Refresh());
                }
                else
                {
                    InitRefresh();
                }

                tmrRefresh.Start();
            }
        }
    }
}
