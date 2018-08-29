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
 * Summary  : Form for editing parameters of a communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Comm.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WinControl;

namespace Scada.Comm.Shell.Forms
{
    /// <summary>
    /// Form for editing parameters of a communication line.
    /// <para>Форма редактирования параметров линии связи.</para>
    /// </summary>
    public partial class FrmLineParams : Form, IChildForm
    {
        private readonly Settings.CommLine commLine;  // the communication line settings to edit
        private readonly CommEnvironment environment; // the application environment


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLineParams()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLineParams(Settings.CommLine commLine, CommEnvironment environment)
            : this()
        {
            this.commLine = commLine ?? throw new ArgumentNullException("commLine");
            this.environment = environment ?? throw new ArgumentNullException("environment");
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Displays a control corresponding to the selected tab.
        /// </summary>
        private void DisplayControl(int tabIndex)
        {
            ctrlLineMainParams.Visible = tabIndex == 0;
            ctrlLineCustomParams.Visible = tabIndex == 1;
            ctrlLineReqSequence.Visible = tabIndex == 2;
        }

        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        private void SettingsToControls()
        {
            ctrlLineMainParams.SettingsToControls();
            ctrlLineCustomParams.SettingsToControls();
            ctrlLineReqSequence.SettingsToControls();
        }

        /// <summary>
        /// Set the settings according to the controls.
        /// </summary>
        private void ControlsToSettings()
        {
            ctrlLineMainParams.ControlsToSettings();
            ctrlLineCustomParams.ControlsToSettings();
            ctrlLineReqSequence.ControlsToSettings();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            ControlsToSettings();
            ChildFormTag.Modified = false;
        }


        private void FrmLineParams_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Comm.Shell.Forms.FrmLineParams", ctrlLineReqSequence.toolTip);
            lbTabs.SelectedIndex = 0;
            ctrlLineMainParams.CommLine = commLine;
            ctrlLineCustomParams.CommLine = commLine;
            ctrlLineReqSequence.CommLine = commLine;
            ctrlLineReqSequence.Environment = environment;
            SettingsToControls();
        }

        private void lbTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            // draw the list box item
            const int PaddingLeft = 5;
            string text = (string)lbTabs.Items[e.Index];
            SizeF textSize = e.Graphics.MeasureString(text, lbTabs.Font);
            Brush brush = e.State.HasFlag(DrawItemState.Selected) ? 
                SystemBrushes.HighlightText : SystemBrushes.WindowText;

            e.DrawBackground();
            e.Graphics.DrawString(text, lbTabs.Font, brush, 
                e.Bounds.Left + PaddingLeft, e.Bounds.Top + (lbTabs.ItemHeight - textSize.Height) / 2);
            e.DrawFocusRectangle();
        }

        private void lbTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayControl(lbTabs.SelectedIndex);
        }

        private void control_SettingsChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void ctrlLineReqSequence_CustomParamsChanged(object sender, SortedList<string, string> customParams)
        {

        }
    }
}
