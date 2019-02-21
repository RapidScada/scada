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
 * Summary  : Form for generating data.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Tables;
using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Windows.Forms;
using Utils;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for generating data.
    /// <para>Форма для генерации данных.</para>
    /// </summary>
    public partial class FrmGenData : Form
    {
        private readonly ServerComm serverComm; // the object to communicate with Server
        private readonly Log errLog;            // the application error log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmGenData()
        {
            InitializeComponent();

            dtpArcDate.CustomFormat = Localization.Culture.DateTimeFormat.ShortDatePattern;
            dtpArcTime.CustomFormat = Localization.Culture.DateTimeFormat.LongTimePattern;
            dtpArcDate.Value = dtpArcTime.Value = DateTime.Today;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmGenData(ServerComm serverComm, Log errLog)
            : this()
        {
            this.serverComm = serverComm ?? throw new ArgumentNullException("serverComm");
            this.errLog = errLog ?? throw new ArgumentNullException("errLog");
        }


        private void FrmGenData_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
        }

        private void rbArcData_CheckedChanged(object sender, EventArgs e)
        {
            dtpArcDate.Enabled = dtpArcTime.Enabled = rbArcData.Checked;
        }

        private void btnValOff_Click(object sender, EventArgs e)
        {
            txtCnlVal.Text = "0";
        }

        private void btnValOn_Click(object sender, EventArgs e)
        {
            txtCnlVal.Text = "1";
        }

        private void btnStatZero_Click(object sender, EventArgs e)
        {
            numCnlStat.Value = 0;
        }

        private void btnStatOne_Click(object sender, EventArgs e)
        {
            numCnlStat.Value = 1;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // retrieve data to send
            if (!int.TryParse(cbCnlNum.Text, out int cnlNum))
            {
                ScadaUiUtils.ShowError(ServerShellPhrases.IncorrectCnlNum);
                return;
            }

            double cnlVal = ScadaUtils.StrToDouble(txtCnlVal.Text);
            if (double.IsNaN(cnlVal))
            {
                ScadaUiUtils.ShowError(ServerShellPhrases.IncorrectCnlVal);
                return;
            }

            DateTime srezDT = rbCurData.Checked ? 
                DateTime.MinValue :
                dtpArcDate.Value.Date.Add(dtpArcTime.Value.TimeOfDay);
            SrezTableLight.Srez srez = new SrezTableLight.Srez(srezDT, 1);
            srez.CnlNums[0] = cnlNum;
            srez.CnlData[0] = new SrezTableLight.CnlData(cnlVal, decimal.ToInt32(numCnlStat.Value));

            // send data
            if (rbCurData.Checked ? 
                serverComm.SendSrez(srez, out bool result) :
                serverComm.SendArchive(srez, out result))
            {
                cbCnlNum.Items.Remove(cnlNum);
                cbCnlNum.Items.Insert(0, cnlNum);
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
