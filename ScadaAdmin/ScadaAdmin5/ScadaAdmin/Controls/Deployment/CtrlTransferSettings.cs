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
 * Module   : Administrator
 * Summary  : Control for editing configuration transfer settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Admin.App.Controls.Deployment
{
    /// <summary>
    /// Control for editing configuration transfer settings.
    /// <para>Элемент управления для редактирования настроек передачи конфигурации.</para>
    /// </summary>
    public partial class CtrlTransferSettings : UserControl
    {
        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlTransferSettings()
        {
            InitializeComponent();
            changing = false;
        }


        /// <summary>
        /// Gets a value indicating whether none of the options are selected.
        /// </summary>
        private bool Empty
        {
            get
            {
                return !(chkIncludeBase.Checked || chkIncludeInterface.Checked ||
                    chkIncludeServer.Checked || chkIncludeComm.Checked || chkIncludeWeb.Checked);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the object filter is available for editing.
        /// </summary>
        public bool ObjFilterEnabled
        {
            get
            {
                return txtObjFilter.Enabled;
            }
            set
            {
                lblObjFilter.Enabled = txtObjFilter.Enabled = value;
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
        /// Validates the form fields.
        /// </summary>
        public bool ValidateFields()
        {
            if (Empty)
            {
                ScadaUiUtils.ShowError("Configuration is not selected. Конфигурация не выбрана.");
                return false;
            }

            if (!RangeUtils.StrToRange(txtObjFilter.Text, true, out ICollection<int> collection))
            {
                ScadaUiUtils.ShowError("Object filter is incorrect.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        public void SettingsToControls(TransferSettings transferSettings)
        {
            if (transferSettings == null)
                throw new ArgumentNullException("transferSettings");

            changing = true;
            gbOptions.Enabled = true;
            chkIncludeBase.Checked = transferSettings.IncludeBase;
            chkIncludeInterface.Checked = transferSettings.IncludeInterface;
            chkIncludeServer.Checked = transferSettings.IncludeServer;
            chkIncludeComm.Checked = transferSettings.IncludeComm;
            chkIncludeWeb.Checked = transferSettings.IncludeWeb;
            chkIgnoreRegKeys.Checked = transferSettings.IgnoreRegKeys;
            chkIgnoreWebStorage.Checked = transferSettings.IgnoreWebStorage;
            txtObjFilter.Text = ObjFilterEnabled ? RangeUtils.RangeToStr(transferSettings.ObjNums) : "";
            changing = false;
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        public void ControlsToSettings(TransferSettings transferSettings)
        {
            if (transferSettings == null)
                throw new ArgumentNullException("transferSettings");

            transferSettings.IncludeBase = chkIncludeBase.Checked;
            transferSettings.IncludeInterface = chkIncludeInterface.Checked;
            transferSettings.IncludeServer = chkIncludeServer.Checked;
            transferSettings.IncludeComm = chkIncludeComm.Checked;
            transferSettings.IncludeWeb = chkIncludeWeb.Checked;
            transferSettings.IgnoreRegKeys = chkIgnoreRegKeys.Checked;
            transferSettings.IgnoreWebStorage = chkIgnoreWebStorage.Checked;
            transferSettings.SetObjNums(ObjFilterEnabled ? RangeUtils.StrToRange(txtObjFilter.Text, true) : null);
        }

        /// <summary>
        /// Clears and disables the control.
        /// </summary>
        public void Disable()
        {
            changing = true;
            chkIncludeBase.Checked = false;
            chkIncludeInterface.Checked = false;
            chkIncludeServer.Checked = false;
            chkIncludeComm.Checked = false;
            chkIncludeWeb.Checked = false;
            chkIgnoreRegKeys.Checked = false;
            chkIgnoreWebStorage.Checked = false;
            gbOptions.Enabled = false;
            changing = false;
        }


        /// <summary>
        /// Occurs when the settings changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler SettingsChanged;


        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                OnSettingsChanged();
        }
    }
}
