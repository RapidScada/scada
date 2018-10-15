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
 * Module   : Server Shell
 * Summary  : Form for displaying Server stats
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Agent;
using Scada.Agent.Connector;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
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
        private LogBox stateBox;          // object to refresh state
        private LogBox logBox;            // object to refresh log
        private IAgentClient agentClient; // the cuurent Agent client
        private bool localMode;           // read logs from files directly
        private RelPath statePath;        // path of the remote state file
        private RelPath logPath;          // path of the remote log file
        private DateTime stateFileAge;    // last write time of the remote state file
        private DateTime logFileAge;      // last write time of the remote log file


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
            stateBox = new LogBox(lbState) { FullLogView = true };
            logBox = new LogBox(lbLog) { AutoScroll = true };
            agentClient = null;
            localMode = false;
            statePath = null;
            logPath = null;
            stateFileAge = DateTime.MinValue;
            logFileAge = DateTime.MinValue;
        }


        /// <summary>
        /// Initializes the log refresh process.
        /// </summary>
        private void InitRefresh()
        {
            agentClient = environment.AgentClient;

            if (agentClient == null)
            {
                stateBox.SetFirstLine(ServerShellPhrases.ConnectionUndefined);
                logBox.SetFirstLine(ServerShellPhrases.ConnectionUndefined);
                tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
            }
            else
            {
                stateBox.SetFirstLine("Loading..."); // TODO: phrase
                logBox.SetFirstLine("Loading...");

                if (agentClient.IsLocal)
                {
                    localMode = true;
                    tmrRefresh.Interval = ScadaUiUtils.LogLocalRefreshInterval;
                    stateBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, ServerUtils.AppStateFileName);
                    logBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, ServerUtils.AppLogFileName);
                }
                else
                {
                    localMode = false;
                    tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
                    statePath = new RelPath(ConfigParts.Server, AppFolder.Log, ServerUtils.AppStateFileName);
                    logPath = new RelPath(ConfigParts.Server, AppFolder.Log, ServerUtils.AppLogFileName);
                    stateFileAge = DateTime.MinValue;
                    logFileAge = DateTime.MinValue;
                }
            }
        }
        
        /// <summary>
        /// Refresh the server state asynchronously.
        /// </summary>
        private async Task RefreshStateAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (agentClient.ReadLog(statePath, ref stateFileAge, out ICollection<string> lines))
                        stateBox.SetLines(lines);
                }
                catch (Exception ex)
                {
                    stateBox.SetFirstLine(ex.Message);
                    stateFileAge = DateTime.MinValue;
                }
            });
        }

        /// <summary>
        /// Refresh the server log asynchronously.
        /// </summary>
        private async Task RefreshLogAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (agentClient.ReadLog(logPath, logBox.LogViewSize, ref logFileAge, out ICollection<string> lines))
                        logBox.SetLines(lines);
                }
                catch (Exception ex)
                {
                    logBox.SetFirstLine(ex.Message);
                    logFileAge = DateTime.MinValue;
                }
            });
        }


        private void FrmStats_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Server.Shell.Forms.FrmStats");
            InitRefresh();
            tmrRefresh.Start();
        }

        private void FrmStats_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Interval = localMode ?
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

                if (agentClient == environment.AgentClient)
                {
                    if (agentClient != null)
                    {
                        // refresh logs
                        if (localMode)
                        {
                            stateBox.RefreshFromFile();

                            if (!chkPause.Checked)
                                logBox.RefreshFromFile();
                        }
                        else
                        {
                            await RefreshStateAsync();

                            if (!chkPause.Checked)
                                await RefreshLogAsync();
                        }
                    }
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
