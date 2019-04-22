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
 * Summary  : Form for editing saving parameters of Server settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Windows.Forms;
using WinControl;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for editing saving parameters of Server settings.
    /// <para>Форма редактирования параметров записи Сервера.</para>
    /// </summary>
    public partial class FrmSaveParams : Form, IChildForm
    {
        /// <summary>
        /// The possible values of a current data writing period.
        /// </summary>
        private static readonly int[] WriteCurPerVals = { 0, 1, 2, 3, 4, 5, 10, 20, 30, 60 };
        /// <summary>
        /// The possible values of an unreliable on inactivity.
        /// </summary>
        private static readonly int[] InactUnrelTimeVals = { 0, 1, 2, 3, 4, 5, 10, 20, 30, 60 };
        /// <summary>
        /// The possible values of a minute data writing period.
        /// </summary>
        private static readonly int[] WriteMinPerVals = { 30, 60, 120, 180, 240, 300, 600 };

        private readonly Settings settings; // the application settings
        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmSaveParams()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSaveParams(Settings settings)
            : this()
        {
            this.settings = settings ?? throw new ArgumentNullException("settings");
            changing = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        private void SettingsToControls()
        {
            changing = true;

            // current data
            int ind = Array.IndexOf(WriteCurPerVals, settings.WriteCurPer);
            cbWriteCurPer.SelectedIndex = ind >= 0 ? ind : 0;
            ind = Array.IndexOf(InactUnrelTimeVals, settings.InactUnrelTime);
            cbInactUnrelTime.SelectedIndex = ind >= 0 ? ind : 0;
            chkWriteCur.Checked = settings.WriteCur;
            chkWriteCurCopy.Checked = settings.WriteCurCopy;

            // minute data
            ind = Array.IndexOf(WriteMinPerVals, settings.WriteMinPer);
            cbWriteMinPer.SelectedIndex = ind >= 0 ? ind : 0;
            numStoreMinPer.SetValue(settings.StoreMinPer);
            chkWriteMin.Checked = settings.WriteMin;
            chkWriteMinCopy.Checked = settings.WriteMinCopy;

            // hourly data
            cbWriteHrPer.SelectedIndex = settings.WriteHrPer == 1800 /*30 minutes*/ ? 0 : 1;
            numStoreHrPer.SetValue(settings.StoreHrPer);
            chkWriteHr.Checked = settings.WriteHr;
            chkWriteHrCopy.Checked = settings.WriteHrCopy;

            // events
            numStoreEvPer.SetValue(settings.StoreEvPer);
            cbWriteEvPer.SelectedIndex = 0;
            chkWriteEv.Checked = settings.WriteEv;
            chkWriteEvCopy.Checked = settings.WriteEvCopy;

            changing = false;
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        private void ControlsToSettings()
        {
            // current data
            settings.WriteCurPer = WriteCurPerVals[cbWriteCurPer.SelectedIndex];
            settings.InactUnrelTime = InactUnrelTimeVals[cbInactUnrelTime.SelectedIndex];
            settings.WriteCur = chkWriteCur.Checked;
            settings.WriteCurCopy = chkWriteCurCopy.Checked;

            // minute data
            settings.WriteMinPer = WriteMinPerVals[cbWriteMinPer.SelectedIndex];
            settings.StoreMinPer = decimal.ToInt32(numStoreMinPer.Value);
            settings.WriteMin = chkWriteMin.Checked;
            settings.WriteMinCopy = chkWriteMinCopy.Checked;

            // hourly data
            settings.WriteHrPer = cbWriteHrPer.SelectedIndex > 0 ? 3600 /*1 hour*/ : 1800 /*30 minutes*/;
            settings.StoreHrPer = decimal.ToInt32(numStoreHrPer.Value);
            settings.WriteHr = chkWriteHr.Checked;
            settings.WriteHrCopy = chkWriteHrCopy.Checked;

            // events
            settings.StoreEvPer = decimal.ToInt32(numStoreEvPer.Value);
            settings.WriteEv = chkWriteEv.Checked;
            settings.WriteEvCopy = chkWriteEvCopy.Checked;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            ControlsToSettings();

            if (ChildFormTag.SendMessage(this, ServerMessage.SaveSettings))
                ChildFormTag.Modified = false;
        }


        private void FrmSaveParams_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            SettingsToControls();
        }

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                ChildFormTag.Modified = true;
        }
    }
}
