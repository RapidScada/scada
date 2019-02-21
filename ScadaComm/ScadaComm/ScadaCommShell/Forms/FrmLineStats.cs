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
 * Module   : Communicator Shell
 * Summary  : Form for displaying communication line stats
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Agent;
using Scada.Agent.Connector;
using Scada.Agent.UI;
using Scada.Comm.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Comm.Shell.Forms
{
    /// <summary>
    /// Form for displaying communication line stats.
    /// <para>Форма для отображения статистики линии связи.</para>
    /// </summary>
    public partial class FrmLineStats : Form, IChildForm
    {
        private readonly Settings.CommLine commLine;  // the communication line settings to edit
        private readonly CommEnvironment environment; // the application environment
        private RemoteLogBox stateBox; // object to refresh state
        private RemoteLogBox logBox;   // object to refresh log
        private bool stateTabActive;   // the State tab is currently active


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
            stateBox = new RemoteLogBox(lbState) { FullLogView = true };
            logBox = new RemoteLogBox(lbLog, true) { AutoScroll = true };
            stateTabActive = true;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


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
                stateBox.SetFirstLine(CommShellPhrases.SetProfile);
                logBox.SetFirstLine(CommShellPhrases.SetProfile);
                tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
            }
            else
            {
                stateBox.SetFirstLine(CommShellPhrases.Loading);
                logBox.SetFirstLine(CommShellPhrases.Loading);
                string stateFileName = CommUtils.GetLineStateFileName(commLine.Number);
                string logFileName = CommUtils.GetLineLogFileName(commLine.Number);

                if (agentClient.IsLocal)
                {
                    tmrRefresh.Interval = ScadaUiUtils.LogLocalRefreshInterval;
                    stateBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, stateFileName);
                    logBox.LogFileName = Path.Combine(environment.AppDirs.LogDir, logFileName);
                }
                else
                {
                    tmrRefresh.Interval = ScadaUiUtils.LogRemoteRefreshInterval;
                    stateBox.LogPath = new RelPath(ConfigParts.Comm, AppFolder.Log, stateFileName);
                    logBox.LogPath = new RelPath(ConfigParts.Comm, AppFolder.Log, logFileName);
                }
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void FrmLineStats_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            Text = string.Format(CommShellPhrases.LineStatsTitle, commLine.Number);

            ChildFormTag.MainFormMessage += ChildFormTag_MainFormMessage;
            lbTabs.SelectedIndex = 0;
            InitRefresh();
            tmrRefresh.Start();
        }

        private void FrmLineStats_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmrRefresh.Stop();
        }

        private void FrmLineStats_VisibleChanged(object sender, EventArgs e)
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

        private void ChildFormTag_MainFormMessage(object sender, FormMessageEventArgs e)
        {
            // update log file names
            if (e.Message == CommMessage.UpdateFileName)
            {
                Text = string.Format(CommShellPhrases.LineStatsTitle, commLine.Number);
                InitRefresh();
            }
        }

        private void lbTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            lbTabs.DrawTabItem(e);
        }

        private void lbTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTabs.SelectedIndex == 0)
            {
                stateTabActive = true;
                chkPause.Enabled = false;
                chkPause.Checked = false;
                lbState.Visible = true;
                lbLog.Visible = false;
            }
            else
            {
                stateTabActive = false;
                chkPause.Enabled = true;
                lbState.Visible = false;
                lbLog.Visible = true;
            }
        }

        private async void tmrRefresh_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                tmrRefresh.Stop();

                if (stateBox.AgentClient == environment.AgentClient)
                {
                    if (stateTabActive)
                        await Task.Run(() => stateBox.Refresh());
                    else if (!chkPause.Checked)
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
