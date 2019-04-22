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
 * Summary  : Form to send a telecontrol command.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.UI;
using System;
using System.Windows.Forms;
using Utils;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form to send a telecontrol command.
    /// <para>Форма для отправки команды ТУ.</para>
    /// </summary>
    public partial class FrmGenCommand : Form
    {
        private readonly ServerComm serverComm; // the object to communicate with Server
        private readonly Log errLog;            // the application error log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmGenCommand()
        {
            InitializeComponent();
            pnlCmdVal.Top = pnlCmdDevice.Top = pnlCmdData.Top;
            AdjustControls();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmGenCommand(ServerComm serverComm, Log errLog)
            : this()
        {
            this.serverComm = serverComm ?? throw new ArgumentNullException("serverComm");
            this.errLog = errLog ?? throw new ArgumentNullException("errLog");
        }


        /// <summary>
        /// Sets the properties of controls according to the command type.
        /// </summary>
        private void AdjustControls()
        {
            if (rbStandard.Checked)
            {
                pnlCmdVal.Visible = true;
                pnlCmdData.Visible = false;
                pnlCmdDevice.Visible = false;
            }
            else if (rbBinary.Checked)
            {
                pnlCmdVal.Visible = false;
                pnlCmdData.Visible = true;
                pnlCmdDevice.Visible = false;
            }
            else // rbRequest.Checked
            {
                pnlCmdVal.Visible = false;
                pnlCmdData.Visible = false;
                pnlCmdDevice.Visible = true;
            }
        }


        private void FrmDeviceCommand_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
        }

        private void rbCmdType_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                AdjustControls();
        }

        private void btnCmdVal_Click(object sender, EventArgs e)
        {
            txtCmdVal.Text = sender == btnOn ? "1" : "0";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // send a command to Server
            int userID = decimal.ToInt32(numUserID.Value);
            int ctrlCnlNum = decimal.ToInt32(numCtrlCnlNum.Value);
            bool cmdSent = false;
            bool sendOK = false;

            if (rbStandard.Checked)
            {
                double cmdVal = ScadaUtils.StrToDouble(txtCmdVal.Text);
                if (double.IsNaN(cmdVal))
                {
                    ScadaUiUtils.ShowError(CommonPhrases.IncorrectCmdVal);
                }
                else
                {
                    sendOK = serverComm.SendStandardCommand(userID, ctrlCnlNum, cmdVal, out bool result);
                    cmdSent = true;
                }
            }
            else if (rbBinary.Checked)
            {
                if (rbString.Checked)
                {
                    byte[] cmdData = Command.StrToCmdData(txtCmdData.Text);
                    sendOK = serverComm.SendBinaryCommand(userID, ctrlCnlNum, cmdData, out bool result);
                    cmdSent = true;
                }
                else if (ScadaUtils.HexToBytes(txtCmdData.Text, out byte[] cmdData, true))
                {
                    sendOK = serverComm.SendBinaryCommand(userID, ctrlCnlNum, cmdData, out bool result);
                    cmdSent = true;
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.IncorrectCmdData);
                }
            }
            else
            {
                int kpNum = decimal.ToInt32(numCmdKPNum.Value);
                sendOK = serverComm.SendRequestCommand(userID, ctrlCnlNum, kpNum, out bool result);
                cmdSent = true;
            }

            if (cmdSent)
            {
                if (sendOK)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    errLog.WriteError(serverComm.ErrMsg);
                    ScadaUiUtils.ShowError(serverComm.ErrMsg);
                }
            }
        }
    }
}
