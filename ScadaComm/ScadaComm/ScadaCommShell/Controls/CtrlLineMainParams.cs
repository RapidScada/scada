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
 * Summary  : Control for editing main parameters of a communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Comm.Channels;

namespace Scada.Comm.Shell.Controls
{
    /// <summary>
    /// Control for editing main parameters of a communication line.
    /// <para>Элемент управления для редактирования основных параметров линии связи.</para>
    /// </summary>
    public partial class CtrlLineMainParams : UserControl
    {
        /// <summary>
        /// List item representing a communication channel.
        /// <para>Элемент списка, представляющий канал связи.</para>
        /// </summary>
        private class CommCnlItem
        {
            public CommCnlItem(CommChannelView commChannelView)
            {
                CommChannelView = commChannelView;
                CommCnlParams = null;
            }

            public CommChannelView CommChannelView { get; private set; }
            public SortedList<string, string> CommCnlParams { get; set; }

            public bool Match(Settings.CommLine commLine)
            {
                return CommChannelView.TypeName == commLine.CommCnlType;
            }
            public override string ToString()
            {
                return CommChannelView.ToString();
            }
        }


        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineMainParams()
        {
            InitializeComponent();

            changing = false;
            CommLine = null;
        }


        /// <summary>
        /// Gets or sets the communication line settings to edit.
        /// </summary>
        public Settings.CommLine CommLine { get; set; }


        /// <summary>
        /// Fills the list of communication channel types.
        /// </summary>
        private void FillCommCnlTypes()
        {
            try
            {
                cbCommCnlType.BeginUpdate();

                cbCommCnlType.Items.AddRange(new object[]
                {
                    new CommCnlItem(new CommSerialView()),
                    new CommCnlItem(new CommTcpClientView()),
                    new CommCnlItem(new CommTcpServerView()),
                    new CommCnlItem(new CommUdpView())
                });
            }
            finally
            {
                cbCommCnlType.EndUpdate();
            }
        }

        /// <summary>
        /// Displays the communication channel parameters.
        /// </summary>
        private void DisplayCommCnlParams(CommCnlItem commCnlItem)
        {
            if (commCnlItem == null)
            {
                btnCommCnlProps.Enabled = false;
                txtCommCnlParams.Text = "";
            }
            else
            {
                try
                {
                    btnCommCnlProps.Enabled = commCnlItem.CommChannelView.CanShowProps;
                    txtCommCnlParams.Text = commCnlItem.CommChannelView.GetPropsInfo(commCnlItem.CommCnlParams);
                }
                catch (Exception ex)
                {
                    btnCommCnlProps.Enabled = false;
                    txtCommCnlParams.Text = ex.Message;
                }
            }
        }

        /// <summary>
        /// Raises a SettingsChanged event.
        /// </summary>
        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        public void SettingsToControls()
        {
            if (CommLine == null)
                throw new InvalidOperationException("CommLine must not be null.");

            changing = true;

            // communication line
            chkLineActive.Checked = CommLine.Active;
            chkLineBound.Checked = CommLine.Bind;
            numLineNumber.SetValue(CommLine.Number);
            txtLineName.Text = CommLine.Name;

            // communication channel
            int commCnlTypeIndex = 0;

            for (int i = 0, cnt = cbCommCnlType.Items.Count; i < cnt; i++)
            {
                if (cbCommCnlType.Items[i] is CommCnlItem commCnlItem && commCnlItem.Match(CommLine))
                {
                    commCnlTypeIndex = i;
                    break;
                }
            }

            cbCommCnlType.SelectedIndex = commCnlTypeIndex;

            // communication parameters
            numReqTriesCnt.SetValue(CommLine.ReqTriesCnt);
            numCycleDelay.SetValue(CommLine.CycleDelay);
            chkCmdEnabled.Checked = CommLine.CmdEnabled;
            chkReqAfterCmd.Checked = CommLine.ReqAfterCmd;
            chkDetailedLog.Checked = CommLine.DetailedLog;

            changing = false;
        }
        
        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        public void ControlsToSettings()
        {
            if (CommLine == null)
                throw new InvalidOperationException("CommLine must not be null.");

            // communication line
            CommLine.Active = chkLineActive.Checked;
            CommLine.Bind = chkLineBound.Checked;
            CommLine.Number = decimal.ToInt32(numLineNumber.Value);
            CommLine.Name = txtLineName.Text;

            // communication channel
            CommLine.CommCnlParams.Clear();

            if (cbCommCnlType.SelectedItem is CommCnlItem commCnlItem)
            {
                CommLine.CommCnlType = commCnlItem.CommChannelView.TypeName;

                foreach (KeyValuePair<string, string> commCnlParam in commCnlItem.CommCnlParams)
                {
                    CommLine.CommCnlParams.Add(commCnlParam.Key, commCnlParam.Value);
                }
            }
            else
            {
                CommLine.CommCnlType = "";
            }

            // communication parameters
            CommLine.ReqTriesCnt = decimal.ToInt32(numReqTriesCnt.Value);
            CommLine.CycleDelay = decimal.ToInt32(numCycleDelay.Value);
            CommLine.CmdEnabled = chkCmdEnabled.Checked;
            CommLine.ReqAfterCmd = chkReqAfterCmd.Checked;
            CommLine.DetailedLog = chkDetailedLog.Checked;
        }


        /// <summary>
        /// Occurs when the settings changes.
        /// </summary>
        public event EventHandler SettingsChanged;


        private void CtrlLineMainParams_Load(object sender, EventArgs e)
        {
            FillCommCnlTypes();
        }

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                OnSettingsChanged();
        }

        private void cbCommCnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCommCnlType.SelectedItem is CommCnlItem commCnlItem)
            {
                if (commCnlItem.CommCnlParams == null)
                {
                    // initialize communication channel parameters
                    commCnlItem.CommCnlParams = new SortedList<string, string>();
                    commCnlItem.CommChannelView.SetCommCnlParamsToDefault(commCnlItem.CommCnlParams);

                    foreach (KeyValuePair<string, string> commCnlParam in CommLine.CommCnlParams)
                    {
                        commCnlItem.CommCnlParams[commCnlParam.Key] = commCnlParam.Value;
                    }
                }

                DisplayCommCnlParams(commCnlItem);
            }
            else
            {
                DisplayCommCnlParams(null);
            }

            if (!changing)
                OnSettingsChanged();
        }

        private void btnCommCnlProps_Click(object sender, EventArgs e)
        {
            if (cbCommCnlType.SelectedItem is CommCnlItem commCnlItem && commCnlItem.CommChannelView.CanShowProps)
            {
                // show the communication channel properties
                commCnlItem.CommChannelView.ShowProps(commCnlItem.CommCnlParams, out bool modified);

                if (modified)
                {
                    DisplayCommCnlParams(commCnlItem);
                    OnSettingsChanged();
                }
            }
        }
    }
}
