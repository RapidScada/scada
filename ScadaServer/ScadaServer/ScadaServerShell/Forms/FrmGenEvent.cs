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
 * Summary  : Form to generate an event
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
    /// Form to generate an event.
    /// <para>Форма для генерации события.</para>
    /// </summary>
    public partial class FrmGenEvent : Form
    {
        private readonly ServerComm serverComm; // the object to communicate with Server
        private readonly Log errLog;            // the application error log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmGenEvent()
        {
            InitializeComponent();
            dtpDate.CustomFormat = Localization.Culture.DateTimeFormat.ShortDatePattern;
            dtpTime.CustomFormat = Localization.Culture.DateTimeFormat.LongTimePattern;
            dtpDate.Value = dtpTime.Value = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmGenEvent(ServerComm serverComm, Log errLog)
            : this()
        {
            this.serverComm = serverComm ?? throw new ArgumentNullException("serverComm");
            this.errLog = errLog ?? throw new ArgumentNullException("errLog");
        }


        private void FrmGenEvent_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
        }

        private void btnSetCurTime_Click(object sender, EventArgs e)
        {
            dtpDate.Value = dtpTime.Value = DateTime.Now;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // create event to send
            double oldCnlVal = ScadaUtils.StrToDouble(txtOldCnlVal.Text);
            if (double.IsNaN(oldCnlVal))
            {
                ScadaUiUtils.ShowError(ServerShellPhrases.IncorrectOldCnlVal);
                return;
            }

            double newCnlVal = ScadaUtils.StrToDouble(txtNewCnlVal.Text);
            if (double.IsNaN(newCnlVal))
            {
                ScadaUiUtils.ShowError(ServerShellPhrases.IncorrectNewCnlVal);
                return;
            }

            EventTableLight.Event ev = new EventTableLight.Event
            {
                DateTime = dtpDate.Value.Date.Add(dtpTime.Value.TimeOfDay),
                ObjNum = decimal.ToInt32(numObjNum.Value),
                KPNum = decimal.ToInt32(numKPNum.Value),
                ParamID = decimal.ToInt32(numParamID.Value),
                CnlNum = decimal.ToInt32(numCnlNum.Value),
                OldCnlVal = oldCnlVal,
                OldCnlStat = decimal.ToInt32(numOldCnlStat.Value),
                NewCnlVal = newCnlVal,
                NewCnlStat = decimal.ToInt32(numNewCnlStat.Value),
                Checked = numUserID.Value > 0,
                UserID = decimal.ToInt32(numUserID.Value),
                Descr = txtDescr.Text,
                Data = txtData.Text
            };

            // send event
            if (serverComm.SendEvent(ev, out bool result))
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
