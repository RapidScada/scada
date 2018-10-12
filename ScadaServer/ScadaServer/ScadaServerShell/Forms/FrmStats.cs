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
        /// <summary>
        /// Log refresh interval on local connection, ms.
        /// </summary>
        private const int LocalRefreshInterval = 500;
        /// <summary>
        /// Log refresh interval on remote connection, ms.
        /// </summary>
        private const int RemoteRefreshInterval = 1000;
        /// <summary>
        /// The timer interval when the form is hidden, ms.
        /// </summary>
        private const int InactiveTimerInterval = 10000;

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
            stateBox = new LogBox(rtbState) { FullLogView = true };
            logBox = new LogBox(rtbLog) { AutoScroll = true };
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
                rtbState.Text = rtbLog.Text = ServerShellPhrases.ConnectionUndefined;
                tmrRefresh.Interval = RemoteRefreshInterval;
            }
            else
            {
                rtbState.Text = "";
                rtbLog.Text = "";

                if (agentClient.IsLocal)
                {
                    localMode = true;
                    tmrRefresh.Interval = LocalRefreshInterval;
                    stateBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, ServerUtils.AppStateFileName);
                    logBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, ServerUtils.AppLogFileName);
                }
                else
                {
                    localMode = false;
                    tmrRefresh.Interval = RemoteRefreshInterval;
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
                    rtbState.Text = ex.Message;
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
                    rtbLog.Text = ex.Message;
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
                tmrRefresh.Interval = localMode ? LocalRefreshInterval : RemoteRefreshInterval;
            else
                tmrRefresh.Interval = InactiveTimerInterval;
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
