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
 * Summary  : Control for editing main request sequence of a communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.UI;
using Scada.Comm.Devices;
using Scada.Comm.Shell.Code;
using System.IO;

namespace Scada.Comm.Shell.Controls
{
    /// <summary>
    /// Control for editing main request sequence of a communication line.
    /// <para>Элемент управления для редактирования последовательности опроса линии связи.</para>
    /// </summary>
    public partial class CtrlLineReqSequence : UserControl
    {
        private bool changing; // controls are being changed programmatically
        private Settings.KP deviceBuf; // buffer to copy device


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineReqSequence()
        {
            InitializeComponent();
            numDeviceAddress.Maximum = int.MaxValue;

            SetColumnNames();
            changing = false;
            deviceBuf = null;
            CommLine = null;
            Environment = null;
            CustomParams = null;
        }


        /// <summary>
        /// Gets or sets the communication line settings to edit.
        /// </summary>
        public Settings.CommLine CommLine { get; set; }

        /// <summary>
        /// Gets or sets the application environment.
        /// </summary>
        public CommEnvironment Environment { get; set; }

        /// <summary>
        /// Gets or sets the working copy of the custom parameters.
        /// </summary>
        public SortedList<string, string> CustomParams { get; set; }


        /// <summary>
        /// Validates the required control properties.
        /// </summary>
        private void ValidateProps()
        {
            if (CommLine == null)
                throw new InvalidOperationException("CommLine must not be null.");

            if (Environment == null)
                throw new InvalidOperationException("Environment must not be null.");

            if (CustomParams == null)
                throw new InvalidOperationException("CustomParams must not be null.");
        }

        /// <summary>
        /// Sets the column names for the translation.
        /// </summary>
        private void SetColumnNames()
        {
            colDeviceOrder.Name = "colDeviceOrder";
            colDeviceActive.Name = "colDeviceActive";
            colDeviceBound.Name = "colDeviceBound";
            colDeviceNumber.Name = "colDeviceNumber";
            colDeviceName.Name = "colDeviceName";
            colDeviceDll.Name = "colDeviceDll";
            colDeviceAddress.Name = "colDeviceAddress";
            colDeviceCallNum.Name = "colDeviceCallNum";
            colDeviceTimeout.Name = "colDeviceTimeout";
            colDeviceDelay.Name = "colDeviceDelay";
            colDeviceTime.Name = "colDeviceTime";
            colDevicePeriod.Name = "colDevicePeriod";
            colDeviceCmdLine.Name = "colDeviceCmdLine";
        }

        /// <summary>
        /// Enables or disables the controls.
        /// </summary>
        private void SetControlsEnabled()
        {
            if (lvReqSequence.SelectedItems.Count > 0)
            {
                int index = lvReqSequence.SelectedIndices[0];
                btnMoveUpDevice.Enabled = index > 0;
                btnMoveDownDevice.Enabled = index < lvReqSequence.Items.Count - 1;
                btnDeleteDevice.Enabled = true;
                btnCutDevice.Enabled = true;
                btnCopyDevice.Enabled = true;
                gbSelectedDevice.Enabled = true;
            }
            else
            {
                btnMoveUpDevice.Enabled = false;
                btnMoveDownDevice.Enabled = false;
                btnDeleteDevice.Enabled = false;
                btnCutDevice.Enabled = false;
                btnCopyDevice.Enabled = false;
                gbSelectedDevice.Enabled = false;
            }
        }

        /// <summary>
        /// Fills the combo box that specifies a device driver.
        /// </summary>
        private void FillDriverComboBox()
        {
            try
            {
                cbDeviceDll.BeginUpdate();
                cbDeviceDll.Items.Clear();

                foreach (FileInfo fileInfo in Environment.GetDrivers())
                {
                    cbDeviceDll.Items.Add(fileInfo.Name);
                }
            }
            finally
            {
                cbDeviceDll.EndUpdate();
            }
        }

        /// <summary>
        /// Creates a new list view item that represents a device.
        /// </summary>
        private ListViewItem CreateDeviceItem(Settings.KP kp, ref int index)
        {
            return new ListViewItem(new string[] 
            {
                (++index ).ToString(),
                kp.Active ? "V" : " ", kp.Bind ? "V" : " ",
                kp.Number.ToString(), kp.Name, kp.Dll,
                kp.Address.ToString(), kp.CallNum,
                kp.Timeout.ToString(), kp.Delay.ToString(),
                kp.Time.ToString("T", Localization.Culture),
                kp.Period.ToString(),
                kp.CmdLine
            })
            {
                Tag = kp
            };
        }

        /// <summary>
        /// Displays the device properties.
        /// </summary>
        private void DisplayDevice(Settings.KP kp)
        {
            if (kp == null)
            {
                chkDeviceActive.Checked = false;
                chkDeviceBound.Checked = false;
                numDeviceNumber.Value = 0;
                txtDeviceName.Text = "";
                cbDeviceDll.Text = "";
                numDeviceAddress.Value = 0;
                txtDeviceCallNum.Text = "";
                numDeviceTimeout.Value = 0;
                numDeviceDelay.Value = 0;
                dtpDeviceTime.Value = dtpDeviceTime.MinDate;
                dtpDevicePeriod.Value = dtpDevicePeriod.MinDate;
                txtDeviceCmdLine.Text = "";
            }
            else
            {
                chkDeviceActive.Checked = kp.Active;
                chkDeviceBound.Checked = kp.Bind;
                numDeviceNumber.SetValue(kp.Number);
                txtDeviceName.Text = kp.Name;
                cbDeviceDll.Text = kp.Dll;
                numDeviceAddress.SetValue(kp.Address);
                txtDeviceCallNum.Text = kp.CallNum;
                numDeviceTimeout.SetValue(kp.Timeout);
                numDeviceDelay.SetValue(kp.Delay);
                dtpDeviceTime.SetTime(kp.Time);
                dtpDevicePeriod.SetTime(kp.Period);
                txtDeviceCmdLine.Text = kp.CmdLine;
            }
        }

        /// <summary>
        /// Adds the device to the request sequence.
        /// </summary>
        private void AddDevice(Settings.KP kp)
        {
            try
            {
                // add the device
                lvReqSequence.BeginUpdate();
                int index = lvReqSequence.SelectedIndices.Count > 0 ?
                    lvReqSequence.SelectedIndices[0] + 1 : lvReqSequence.Items.Count;
                lvReqSequence.Items.Insert(index, CreateDeviceItem(kp, ref index)).Selected = true;

                // update item numbers
                for (int i = index, cnt = lvReqSequence.Items.Count; i < cnt; i++)
                {
                    lvReqSequence.Items[i].Text = (i + 1).ToString();
                }
            }
            finally
            {
                lvReqSequence.EndUpdate();
                numDeviceNumber.Focus();
                OnSettingsChanged();
            }
        }

        /// <summary>
        /// Gets the selected list view item and the corresponding device.
        /// </summary>
        private bool GetSelectedItem(out ListViewItem item, out Settings.KP kp)
        {
            if (lvReqSequence.SelectedItems.Count > 0)
            {
                item = lvReqSequence.SelectedItems[0];
                kp = (Settings.KP)item.Tag;
                return true;
            }
            else
            {
                item = null;
                kp = null;
                return false;
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
        /// Raises a CustomParamsChanged event.
        /// </summary>
        private void OnCustomParamsChanged()
        {
            CustomParamsChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        public void SettingsToControls()
        {
            ValidateProps();
            FillDriverComboBox();

            try
            {
                lvReqSequence.BeginUpdate();
                lvReqSequence.Items.Clear();
                int index = 0;

                foreach (Settings.KP kp in CommLine.ReqSequence)
                {
                    lvReqSequence.Items.Add(CreateDeviceItem(kp.Clone(), ref index));
                }

                if (lvReqSequence.Items.Count > 0)
                    lvReqSequence.Items[0].Selected = true;
            }
            finally
            {
                lvReqSequence.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        public void ControlsToSettings()
        {
            ValidateProps();
            CommLine.ReqSequence.Clear();

            foreach (ListViewItem item in lvReqSequence.Items)
            {
                CommLine.ReqSequence.Add((Settings.KP)item.Tag);
            }
        }


        /// <summary>
        /// Occurs when the settings changes.
        /// </summary>
        public event EventHandler SettingsChanged;

        /// <summary>
        /// Occurs when the custom line parameters changes.
        /// </summary>
        public event EventHandler CustomParamsChanged;


        private void CtrlLineReqSequence_Load(object sender, EventArgs e)
        {
            SetControlsEnabled();
            btnPasteDevice.Enabled = false;
        }

        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            // add a new device
            AddDevice(new Settings.KP());
        }

        private void btnMoveUpDevice_Click(object sender, EventArgs e)
        {
            // move up the selected item
            if (lvReqSequence.SelectedIndices.Count > 0)
            {
                int index = lvReqSequence.SelectedIndices[0];

                if (index > 0)
                {
                    try
                    {
                        lvReqSequence.BeginUpdate();
                        ListViewItem item = lvReqSequence.Items[index];
                        ListViewItem prevItem = lvReqSequence.Items[index - 1];

                        lvReqSequence.Items.RemoveAt(index);
                        lvReqSequence.Items.Insert(index - 1, item);

                        item.Text = index.ToString();
                        prevItem.Text = (index + 1).ToString();
                    }
                    finally
                    {
                        lvReqSequence.EndUpdate();
                        lvReqSequence.Focus();
                    }
                }
            }
        }

        private void btnMoveDownDevice_Click(object sender, EventArgs e)
        {
            // move down the selected item
            if (lvReqSequence.SelectedIndices.Count > 0)
            {
                int index = lvReqSequence.SelectedIndices[0];

                if (index < lvReqSequence.Items.Count - 1)
                {
                    try
                    {
                        lvReqSequence.BeginUpdate();
                        ListViewItem item = lvReqSequence.Items[index];
                        ListViewItem nextItem = lvReqSequence.Items[index + 1];

                        lvReqSequence.Items.RemoveAt(index);
                        lvReqSequence.Items.Insert(index + 1, item);

                        item.Text = (index + 2).ToString();
                        nextItem.Text = (index + 1).ToString();
                    }
                    finally
                    {
                        lvReqSequence.EndUpdate();
                        lvReqSequence.Focus();
                    }
                }
            }
        }

        private void btnDeleteDevice_Click(object sender, EventArgs e)
        {
            if (lvReqSequence.SelectedIndices.Count > 0)
            {
                try
                {
                    // delete the selected device
                    lvReqSequence.BeginUpdate();
                    int index = lvReqSequence.SelectedIndices[0];
                    lvReqSequence.Items.RemoveAt(index);

                    if (lvReqSequence.Items.Count > 0)
                    {
                        // select an item
                        if (index >= lvReqSequence.Items.Count)
                            index = lvReqSequence.Items.Count - 1;
                        lvReqSequence.Items[index].Selected = true;

                        // update item numbers
                        for (int i = index, cnt = lvReqSequence.Items.Count; i < cnt; i++)
                        {
                            lvReqSequence.Items[i].Text = (i + 1).ToString();
                        }
                    }
                }
                finally
                {
                    lvReqSequence.EndUpdate();
                    lvReqSequence.Focus();
                    OnSettingsChanged();
                }
            }
        }

        private void btnCutDevice_Click(object sender, EventArgs e)
        {
            // cut the selected device
            btnCopyDevice_Click(null, null);
            btnDeleteDevice_Click(null, null);
        }

        private void btnCopyDevice_Click(object sender, EventArgs e)
        {
            // copy the selected device
            if (GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                btnPasteDevice.Enabled = true;
                deviceBuf = kp.Clone();
            }

            lvReqSequence.Focus();
        }

        private void btnPasteDevice_Click(object sender, EventArgs e)
        {
            // paste the copied device
            if (deviceBuf == null)
                lvReqSequence.Focus();
            else
                AddDevice(deviceBuf.Clone());
        }

        private void lvReqSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display the selected item properties
            changing = true;

            Settings.KP kp = lvReqSequence.SelectedItems.Count > 0 ?
                (Settings.KP)lvReqSequence.SelectedItems[0].Tag : null;

            DisplayDevice(kp);
            SetControlsEnabled();
            changing = false;
        }

        private void chkDeviceActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Active = chkDeviceActive.Checked;
                item.SubItems[1].Text = chkDeviceActive.Checked ? "V" : " ";
                OnSettingsChanged();
            }
        }

        private void chkDeviceBound_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Bind = chkDeviceBound.Checked;
                item.SubItems[2].Text = chkDeviceBound.Checked ? "V" : " ";
                OnSettingsChanged();
            }
        }

        private void numDeviceNumber_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Number = decimal.ToInt32(numDeviceNumber.Value);
                item.SubItems[3].Text = numDeviceNumber.Value.ToString();
                OnSettingsChanged();
            }
        }

        private void txtDeviceName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Name = txtDeviceName.Text;
                item.SubItems[4].Text = txtDeviceName.Text;
                OnSettingsChanged();
            }
        }

        private void cbDeviceDll_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Dll = cbDeviceDll.Text;
                item.SubItems[5].Text = cbDeviceDll.Text;
                OnSettingsChanged();
            }
        }

        private void numDeviceAddress_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Address = decimal.ToInt32(numDeviceAddress.Value);
                item.SubItems[6].Text = numDeviceAddress.Value.ToString();
                OnSettingsChanged();
            }
        }

        private void txtDeviceCallNum_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.CallNum = txtDeviceCallNum.Text;
                item.SubItems[7].Text = txtDeviceCallNum.Text;
                OnSettingsChanged();
            }
        }

        private void numDeviceTimeout_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Timeout = decimal.ToInt32(numDeviceTimeout.Value);
                item.SubItems[8].Text = numDeviceTimeout.Value.ToString();
                OnSettingsChanged();
            }
        }

        private void numDeviceDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Delay = decimal.ToInt32(numDeviceDelay.Value);
                item.SubItems[9].Text = numDeviceDelay.Value.ToString();
                OnSettingsChanged();
            }
        }

        private void dtpDeviceTime_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Time = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
                    dtpDeviceTime.Value.Hour, dtpDeviceTime.Value.Minute, dtpDeviceTime.Value.Second);
                item.SubItems[10].Text = kp.Time.ToString("T", Localization.Culture);
                OnSettingsChanged();
            }
        }

        private void dtpDevicePeriod_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Period = new TimeSpan(dtpDevicePeriod.Value.Hour, dtpDevicePeriod.Value.Minute, 
                    dtpDevicePeriod.Value.Second);
                item.SubItems[11].Text = kp.Period.ToString();
                OnSettingsChanged();
            }
        }

        private void txtDeviceCmdLine_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.CmdLine = txtDeviceCmdLine.Text;
                item.SubItems[12].Text = txtDeviceCmdLine.Text;
                OnSettingsChanged();
            }
        }

        private void btnResetReqParams_Click(object sender, EventArgs e)
        {
            // set the request parameters of the selected device by default
            if (GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                if (Environment.TryGetKPView(kp, true, null, out KPView kpView, out string errMsg))
                {
                    KPReqParams reqParams = kpView.DefaultReqParams;
                    numDeviceTimeout.SetValue(reqParams.Timeout);
                    numDeviceDelay.SetValue(reqParams.Delay);
                    dtpDeviceTime.SetTime(reqParams.Time);
                    dtpDevicePeriod.SetTime(reqParams.Period);
                    txtDeviceCmdLine.Text = reqParams.CmdLine;
                    OnSettingsChanged();
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }

        private void btnDeviceProps_Click(object sender, EventArgs e)
        {
            // show the properties of the selected device
            if (GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                if (Environment.TryGetKPView(kp, false, CustomParams, out KPView kpView, out string errMsg))
                {
                    if (kpView.CanShowProps)
                    {
                        kpView.ShowProps();

                        if (kpView.KPProps.Modified)
                        {
                            txtDeviceCmdLine.Text = kpView.KPProps.CmdLine;
                            OnCustomParamsChanged();
                            OnSettingsChanged();
                        }
                    }
                    else
                    {
                        ScadaUiUtils.ShowWarning(CommShellPhrases.NoDeviceProps);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }
    }
}
