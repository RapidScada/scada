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
 * Summary  : Form to send a telecontrol command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Comm.Shell.Code;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Comm.Shell.Forms
{
    /// <summary>
    /// Form to send a telecontrol command.
    /// <para>Форма для отправки команды ТУ.</para>
    /// </summary>
    public partial class FrmDeviceCommand : Form
    {
        private readonly Settings.KP kp;              // the device that receives command
        private readonly CommEnvironment environment; // the application environment


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceCommand()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceCommand(Settings.KP kp, CommEnvironment environment)
            : this()
        {
            this.kp = kp ?? throw new ArgumentNullException("kp");
            this.environment = environment ?? throw new ArgumentNullException("environment");
        }


        /// <summary>
        /// Sets the properties of controls according to the command type.
        /// </summary>
        private void AdjustControls()
        {
            if (rbStandard.Checked)
            {
                numCmdNum.Enabled = true;
                pnlCmdVal.Visible = true;
                pnlCmdData.Visible = false;
            }
            else if (rbBinary.Checked)
            {
                numCmdNum.Enabled = true;
                pnlCmdVal.Visible = false;
                pnlCmdData.Visible = true;
            }
            else // rbRequest.Checked
            {
                numCmdNum.Enabled = false;
                pnlCmdVal.Visible = false;
                pnlCmdData.Visible = false;
            }
        }


        private void FrmDeviceCommand_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            Text = string.Format(Text, kp.Caption);
            AdjustControls();
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
            // initialize a command
            Command cmd = new Command() { KPNum = kp.Number };
            bool cmdOK = false;

            if (rbStandard.Checked)
            {
                cmd.CmdTypeID = BaseValues.CmdTypes.Standard;
                cmd.CmdNum = decimal.ToInt32(numCmdNum.Value);

                double cmdVal = ScadaUtils.StrToDouble(txtCmdVal.Text);
                if (double.IsNaN(cmdVal))
                {
                    ScadaUiUtils.ShowError(CommonPhrases.IncorrectCmdVal);
                }
                else
                {
                    cmd.CmdVal = cmdVal;
                    cmdOK = true;
                }
            }
            else if (rbBinary.Checked)
            {
                cmd.CmdTypeID = BaseValues.CmdTypes.Binary;
                cmd.CmdNum = decimal.ToInt32(numCmdNum.Value);

                if (rbString.Checked)
                {
                    cmd.CmdData = Command.StrToCmdData(txtCmdData.Text);
                    cmdOK = true;
                }
                else if (ScadaUtils.HexToBytes(txtCmdData.Text, out byte[] cmdData, true))
                {
                    cmd.CmdData = cmdData;
                    cmdOK = true;
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.IncorrectCmdData);
                }
            }
            else
            {
                cmd.CmdTypeID = BaseValues.CmdTypes.Request;
                cmdOK = true;
            }

            // save the command to file
            if (cmdOK)
            {
                if (CommUtils.SaveCmd(environment.AppDirs.CmdDir, CommShellUtils.CommandSender, cmd, out string msg))
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    environment.ErrLog.WriteError(msg);
                    ScadaUiUtils.ShowError(msg);
                }
            }
        }
    }
}
