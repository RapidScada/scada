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
 * Summary  : Channel creation wizard. Step 1
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Comm;
using Scada.Comm.Devices;
using Scada.Data.Entities;
using Scada.Data.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.App.Controls.Tools
{
    /// <summary>
    /// Channel creation wizard. Step 1.
    /// <para>Мастер создания каналов. Шаг 1.</para>
    /// </summary>
    public partial class CtrlCnlCreate1 : UserControl
    {
        /// <summary>
        /// Item representing a device.
        /// <para>Элемент, представляющий КП.</para>
        /// </summary>
        private class DeviceItem
        {
            public DeviceItem()
            {
                KPEntity = null;
                KPSettings = null;
                CommLineSettings = null;
                Instance = null;
                KPView = null;
            }

            public KP KPEntity { get; set; }
            public Settings.KP KPSettings { get; set; }
            public Settings.CommLine CommLineSettings { get; set; }
            public Instance Instance { get; set; }
            public KPView KPView { get; set; }
        }


        private ScadaProject project; // the project under development
        private AppData appData;      // the common data of the application
        private Dictionary<int, DeviceItem> deviceItems; // the device items by device numbers


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCnlCreate1()
        {
            InitializeComponent();

            cbDevice.DisplayMember = "Name";
            cbDevice.ValueMember = "KPNum";
        }


        /// <summary>
        /// Gets the selected object.
        /// </summary>
        public KP SelectedDevice
        {
            get
            {
                return cbDevice.SelectedItem as KP;
            }
        }

        /// <summary>
        /// Gets the channel prototypes.
        /// </summary>
        public KPView.KPCnlPrototypes CnlPrototypes { get; private set; }

        /// <summary>
        /// Gets a value indicating whether channels can be created.
        /// </summary>
        public bool StatusOK
        {
            get
            {
                return CnlPrototypes != null && (CnlPrototypes.InCnls.Count > 0 || CnlPrototypes.CtrlCnls.Count > 0);
            }
        }


        /// <summary>
        /// Scans Communicator settings of all instances.
        /// </summary>
        private void ScanCommSettings()
        {
            deviceItems = new Dictionary<int, DeviceItem>(project.ConfigBase.KPTable.ItemCount);

            foreach (KP kpEntity in project.ConfigBase.KPTable.EnumerateItems())
            {
                deviceItems.Add(kpEntity.KPNum, new DeviceItem { KPEntity = kpEntity });
            }

            foreach (Instance instance in project.Instances)
            {
                if (instance.LoadAppSettings(out string errMsg) && instance.CommApp.Enabled)
                {
                    foreach (Settings.CommLine commLineSettings in instance.CommApp.Settings.CommLines)
                    {
                        foreach (Settings.KP kpSettings in commLineSettings.ReqSequence)
                        {
                            if (deviceItems.TryGetValue(kpSettings.Number, out DeviceItem deviceItem))
                            {
                                deviceItem.KPSettings = kpSettings;
                                deviceItem.CommLineSettings = commLineSettings;
                                deviceItem.Instance = instance;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fills the combo box with the communication lines.
        /// </summary>
        private void FillCommLineList()
        {
            DataTable commLineTable = project.ConfigBase.CommLineTable.ToDataTable();
            commLineTable.AddEmptyRow(0, AppPhrases.AllCommLines);
            commLineTable.DefaultView.Sort = "CommLineNum";

            cbCommLine.DisplayMember = "Name";
            cbCommLine.ValueMember = "CommLineNum";
            cbCommLine.DataSource = commLineTable;

            try
            {
                cbCommLine.SelectedValue = appData.AppState.RecentSelection.CommLineNum;
            }
            catch
            {
                cbCommLine.SelectedValue = 0;
            }
        }

        /// <summary>
        /// Raises a SelectedDeviceChanged event.
        /// </summary>
        private void OnSelectedDeviceChanged()
        {
            SelectedDeviceChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(ScadaProject project, AppData appData)
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.appData = appData ?? throw new ArgumentNullException("appData");

            CnlPrototypes = null;
            ScanCommSettings();
            FillCommLineList();
        }
        
        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            cbCommLine.Select();
        }


        /// <summary>
        /// Occurs when the selected device changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler SelectedDeviceChanged;


        private void cbCommLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            // filter device by the selected communication line
            int commLineNum = (int)cbCommLine.SelectedValue;
            IEnumerable kps = commLineNum > 0 ?
                project.ConfigBase.KPTable.SelectItems(new TableFilter("CommLineNum", commLineNum)) :
                project.ConfigBase.KPTable.EnumerateItems();

            cbDevice.DataSource = kps.Cast<KP>().ToList();

            try
            {
                cbDevice.SelectedValue = appData.AppState.RecentSelection.KPNum;
            }
            catch
            {
                cbDevice.SelectedValue = null;
            }
        }

        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            // create a user interface of the selected device
            CnlPrototypes = null;

            if (cbDevice.SelectedItem is KP kp)
            {
                if (deviceItems.TryGetValue(kp.KPNum, out DeviceItem deviceItem) && deviceItem.Instance != null)
                {
                    try
                    {
                        if (deviceItem.KPView == null)
                        {
                            CommDirs commDirs = new CommDirs(
                                appData.AppSettings.PathOptions.CommDir, deviceItem.Instance);
                            deviceItem.KPView = KPFactory.GetKPView(
                                Path.Combine(commDirs.KPDir, deviceItem.KPSettings.Dll), kp.KPNum);
                            deviceItem.KPView.KPProps = new KPView.KPProperties(
                                deviceItem.CommLineSettings.CustomParams, deviceItem.KPSettings.CmdLine);
                            deviceItem.KPView.AppDirs = commDirs;
                        }

                        CnlPrototypes = deviceItem.KPView.DefaultCnls;
                        int inCnlCnt = CnlPrototypes == null ? 0 : CnlPrototypes.InCnls.Count;
                        int ctrlCnlCnt = CnlPrototypes == null ? 0 : CnlPrototypes.CtrlCnls.Count;

                        txtInfo.Text = string.Format(AppPhrases.DeviceInfo, 
                            deviceItem.KPSettings.Dll, deviceItem.Instance.Name, inCnlCnt, ctrlCnlCnt);
                        pbStatus.Image = inCnlCnt > 0 || ctrlCnlCnt > 0 ? 
                            Properties.Resources.success : Properties.Resources.warning;
                    }
                    catch (Exception ex)
                    {
                        txtInfo.Text = ex.Message;
                        pbStatus.Image = Properties.Resources.error;
                    }
                }
                else
                {
                    txtInfo.Text = AppPhrases.DeviceNotFound;
                    pbStatus.Image = Properties.Resources.warning;
                }
            }
            else
            {
                txtInfo.Text = AppPhrases.NoDeviceSelected;
                pbStatus.Image = Properties.Resources.warning;
            }

            OnSelectedDeviceChanged();
        }
    }
}
