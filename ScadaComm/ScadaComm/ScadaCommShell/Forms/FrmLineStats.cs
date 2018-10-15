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
 * Summary  : Form for displaying communication line stats
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Agent;
using Scada.Agent.Connector;
using Scada.Comm.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Shell.Forms
{
    /// <summary>
    /// Form for displaying communication line stats.
    /// <para>Форма для отображения статистики линии связи.</para>
    /// </summary>
    public partial class FrmLineStats : Form
    {
        private readonly Settings.CommLine commLine;  // the communication line settings to edit
        private readonly CommEnvironment environment; // the application environment
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
        private FrmLineStats()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLineStats(Settings.CommLine commLine, CommEnvironment environment)
            : this()
        {
            this.commLine = commLine ?? throw new ArgumentNullException("commLine");
            this.environment = environment ?? throw new ArgumentNullException("environment");
            stateBox = new LogBox(lbLog) { FullLogView = true };
            logBox = new LogBox(lbLog, true) { AutoScroll = true };
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
                logBox.SetFirstLine("Connection undefined"); // TODO: phrase
                tmrRefresh.Interval = CommShellUtils.LogRemoteRefreshInterval;
            }
            else
            {
                logBox.SetFirstLine("Loading..."); // TODO: phrase
                string stateFileName = CommUtils.GetLineStateFileName(commLine.Number);
                string logFileName = CommUtils.GetLineLogFileName(commLine.Number);

                if (agentClient.IsLocal)
                {
                    localMode = true;
                    tmrRefresh.Interval = CommShellUtils.LogLocalRefreshInterval;
                    stateBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, stateFileName);
                    logBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, logFileName);
                }
                else
                {
                    localMode = false;
                    tmrRefresh.Interval = CommShellUtils.LogRemoteRefreshInterval;
                    statePath = new RelPath(ConfigParts.Comm, AppFolder.Log, stateFileName);
                    logPath = new RelPath(ConfigParts.Comm, AppFolder.Log, logFileName);
                    stateFileAge = DateTime.MinValue;
                    logFileAge = DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Refresh the communication line log asynchronously.
        /// </summary>
        private async Task RefreshLogAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (agentClient.ReadLog(logPath, logBox.LogViewSize, ref logFileAge, out ICollection<string> lines))
                    {
                        logBox.SetLines(lines);
                    }
                }
                catch (Exception ex)
                {
                    logBox.SetFirstLine(ex.Message);
                    logFileAge = DateTime.MinValue;
                }
            });
        }


        private void FrmLineStats_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Comm.Shell.Forms.FrmLineStats");
            lbTabs.SelectedIndex = 0;
            InitRefresh();
            tmrRefresh.Start();
        }

        private void lbTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            lbTabs.DrawTabItem(e);
        }

        private void lbTabs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmLineStats_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Interval = localMode ?
                    CommShellUtils.LogLocalRefreshInterval :
                    CommShellUtils.LogRemoteRefreshInterval;
            }
            else
            {
                tmrRefresh.Interval = CommShellUtils.LogInactiveTimerInterval;
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
                            //stateBox.RefreshFromFile();

                            if (!chkPause.Checked)
                                logBox.RefreshFromFile();
                        }
                        else
                        {
                            //await RefreshStateAsync();

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
