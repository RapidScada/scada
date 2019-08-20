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
 * Module   : Administrator
 * Summary  : Channel creation wizard. Step 2
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Config;
using Scada.Admin.Project;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Controls.Tools
{
    /// <summary>
    /// Channel creation wizard. Step 1.
    /// <para>Мастер создания каналов. Шаг 1.</para>
    /// </summary>
    public partial class CtrlCnlCreate3 : UserControl
    {
        private ScadaProject project;      // the project under development
        private AdminSettings appSettings; // the application settings

        private int lastStartInCnl;  // the last calculated start input channel number
        private int lastInCnlCnt;    // the last specified number of input channels
        private int lastStartOutCnl; // the last calculated start output channel number
        private int lastOutCnlCnt;   // the last specified number of output channels


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCnlCreate3()
        {
            InitializeComponent();

            numStartInCnl.Maximum = ushort.MaxValue;
            numEndInCnl.Maximum = int.MaxValue;
            numStartOutCnl.Maximum = ushort.MaxValue;
            numEndOutCnl.Maximum = int.MaxValue;

            lastStartInCnl = 1;
            lastInCnlCnt = 0;
            lastStartOutCnl = 1;
            lastOutCnlCnt = 0;
        }


        /// <summary>
        /// Gets or sets the selected device name.
        /// </summary>
        public string DeviceName
        {
            get
            {
                return txtDevice.Text;
            }
            set
            {
                txtDevice.Text = value ?? "";
            }
        }

        /// <summary>
        /// Gets the start input channel number.
        /// </summary>
        public int StartInCnl
        {
            get
            {
                return Convert.ToInt32(numStartInCnl.Value);
            }
        }

        /// <summary>
        /// Gets the start output channel number.
        /// </summary>
        public int StartOutCnl
        {
            get
            {
                return Convert.ToInt32(numStartOutCnl.Value);
            }
        }


        /// <summary>
        /// Calculates a start channel number.
        /// </summary>
        private bool CalcStartCnlNum(IBaseTable cnlTable, int cnlCnt, out int startCnlNum)
        {
            ChannelOptions channelOptions = appSettings.ChannelOptions;
            int cnlMult = channelOptions.CnlMult;
            int cnlGap = channelOptions.CnlGap;
            startCnlNum = cnlMult + channelOptions.CnlShift;
            int prevCnlNum = 0;

            foreach (int cnlNum in cnlTable.EnumerateKeys())
            {
                if (prevCnlNum < startCnlNum && startCnlNum <= cnlNum)
                {
                    if (startCnlNum + cnlCnt + cnlGap <= cnlNum)
                        return true;
                    else
                        startCnlNum += cnlMult;
                }

                prevCnlNum = cnlNum;
            }

            return startCnlNum <= ushort.MaxValue;
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(ScadaProject project, AdminSettings appSettings)
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.appSettings = appSettings ?? throw new ArgumentNullException("appSettings");
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            numStartInCnl.Select();
        }

        /// <summary>
        /// Sets the input channel numbers by default.
        /// </summary>
        public void SetInCnlNums(int inCnlCnt)
        {
            lastStartInCnl = 1;
            lastInCnlCnt = inCnlCnt;

            if (inCnlCnt > 0)
            {
                gbInCnls.Enabled = true;

                if (CalcStartCnlNum(project.ConfigBase.InCnlTable, inCnlCnt, out int startCnlNum))
                    lastStartInCnl = startCnlNum;
            }
            else
            {
                gbInCnls.Enabled = false;
            }

            numStartInCnl.SetValue(lastStartInCnl);
            numEndInCnl.SetValue(lastStartInCnl + lastInCnlCnt - 1);
        }

        /// <summary>
        /// Sets the output channel numbers by default.
        /// </summary>
        public void SetOutCnlNums(int ctrlCnlCnt)
        {
            lastStartOutCnl = 1;
            lastOutCnlCnt = ctrlCnlCnt;

            if (ctrlCnlCnt > 0)
            {
                gbOutCnls.Enabled = true;

                if (CalcStartCnlNum(project.ConfigBase.CtrlCnlTable, ctrlCnlCnt, out int startCnlNum))
                    lastStartOutCnl = startCnlNum;
            }
            else
            {
                gbOutCnls.Enabled = false;
            }

            numStartOutCnl.SetValue(lastStartOutCnl);
            numEndOutCnl.SetValue(lastStartOutCnl + lastOutCnlCnt - 1);
        }


        private void btnResetInCnls_Click(object sender, EventArgs e)
        {
            if (lastStartInCnl > 0)
                numStartInCnl.SetValue(lastStartInCnl);
        }

        private void btnResetOutCnls_Click(object sender, EventArgs e)
        {
            if (lastStartOutCnl > 0)
                numStartOutCnl.SetValue(lastStartOutCnl);
        }

        private void numStartInCnl_ValueChanged(object sender, EventArgs e)
        {
            int startInCnl = Convert.ToInt32(numStartInCnl.Value);
            numEndInCnl.SetValue(startInCnl + lastInCnlCnt - 1);
        }

        private void numStartOutCnl_ValueChanged(object sender, EventArgs e)
        {
            int startOutCnl = Convert.ToInt32(numStartOutCnl.Value);
            numEndOutCnl.SetValue(startOutCnl + lastOutCnlCnt - 1);
        }
    }
}
