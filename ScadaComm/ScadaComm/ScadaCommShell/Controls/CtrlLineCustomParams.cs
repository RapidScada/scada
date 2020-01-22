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
 * Summary  : Control for editing custom parameters of a communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
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

namespace Scada.Comm.Shell.Controls
{
    /// <summary>
    /// Control for editing custom parameters of a communication line.
    /// <para>Элемент управления для редактирования пользовательских параметров линии связи.</para>
    /// </summary>
    public partial class CtrlLineCustomParams : UserControl
    {
        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineCustomParams()
        {
            InitializeComponent();

            SetColumnNames();
            changing = false;
            CommLine = null;
        }


        /// <summary>
        /// Gets or sets the communication line settings to edit.
        /// </summary>
        public Settings.CommLine CommLine { get; set; }


        /// <summary>
        /// Sets the column names for the translation.
        /// </summary>
        private void SetColumnNames()
        {
            colParamName.Name = "colParamName";
            colParamValue.Name = "colParamValue";
        }
        
        /// <summary>
        /// Enables or disables the controls.
        /// </summary>
        private void SetControlsEnabled()
        {
            if (lvCustomParams.SelectedItems.Count > 0)
            {
                btnDeleteCustomParam.Enabled = true;
                gbSelectedParam.Enabled = true;
            }
            else
            {
                btnDeleteCustomParam.Enabled = false;
                gbSelectedParam.Enabled = false;
            }
        }

        /// <summary>
        /// Creates a new list view item that represents a custom parameter.
        /// </summary>
        private ListViewItem CreateCustomParamItem(string name, string value)
        {
            return new ListViewItem(new string[] { name, value });
        }

        /// <summary>
        /// Raises a SettingsChanged event.
        /// </summary>
        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Setup the controls according to the settings.
        /// </summary>
        public void SettingsToControls()
        {
            if (CommLine == null)
                throw new InvalidOperationException("CommLine must not be null.");

            ShowCustomParams(CommLine.CustomParams);
        }

        /// <summary>
        /// Sets the settings according to the controls.
        /// </summary>
        public void ControlsToSettings()
        {
            if (CommLine == null)
                throw new InvalidOperationException("CommLine must not be null.");

            RetrieveCustomParams(CommLine.CustomParams);
        }

        /// <summary>
        /// Fills the displayed list with the specified custom parameters.
        /// </summary>
        public void ShowCustomParams(SortedList<string, string> customParams)
        {
            try
            {
                lvCustomParams.BeginUpdate();
                lvCustomParams.Items.Clear();

                foreach (KeyValuePair<string, string> pair in customParams)
                {
                    lvCustomParams.Items.Add(CreateCustomParamItem(pair.Key, pair.Value));
                }

                if (lvCustomParams.Items.Count > 0)
                    lvCustomParams.Items[0].Selected = true;
            }
            finally
            {
                lvCustomParams.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the specified list with actual custom parameters.
        /// </summary>
        public void RetrieveCustomParams(SortedList<string, string> customParams)
        {
            customParams.Clear();

            foreach (ListViewItem item in lvCustomParams.Items)
            {
                string name = item.SubItems[0].Text.Trim();
                if (name != "")
                    customParams[name] = item.SubItems[1].Text.Trim();
            }
        }


        /// <summary>
        /// Occurs when the settings changes.
        /// </summary>
        public event EventHandler SettingsChanged;


        private void CtrlLineCustomParams_Load(object sender, EventArgs e)
        {
            SetControlsEnabled();
        }

        private void btnAddCustomParam_Click(object sender, EventArgs e)
        {
            ListViewItem item = CreateCustomParamItem("", "");
            lvCustomParams.Items.Add(item);
            item.Selected = true;
            txtParamName.Focus();
            OnSettingsChanged();
        }

        private void btnDeleteCustomParam_Click(object sender, EventArgs e)
        {
            // delete the selected item
            if (lvCustomParams.SelectedItems.Count > 0)
            {
                int index = lvCustomParams.SelectedIndices[0];
                lvCustomParams.Items.RemoveAt(index);

                if (lvCustomParams.Items.Count > 0)
                {
                    if (index >= lvCustomParams.Items.Count)
                        index = lvCustomParams.Items.Count - 1;
                    lvCustomParams.Items[index].Selected = true;
                }

                OnSettingsChanged();
            }

            lvCustomParams.Focus();
        }

        private void lvCustomParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display the selected item properties
            changing = true;

            if (lvCustomParams.SelectedItems.Count > 0)
            {
                ListViewItem item = lvCustomParams.SelectedItems[0];
                txtParamName.Text = item.SubItems[0].Text;
                txtParamValue.Text = item.SubItems[1].Text;
            }
            else
            {
                txtParamName.Text = "";
                txtParamValue.Text = "";
            }

            SetControlsEnabled();
            changing = false;
        }

        private void txtParamName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lvCustomParams.SelectedItems.Count > 0)
            {
                ListViewItem item = lvCustomParams.SelectedItems[0];
                item.SubItems[0].Text = txtParamName.Text;
                OnSettingsChanged();
            }
        }

        private void txtParamValue_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lvCustomParams.SelectedItems.Count > 0)
            {
                ListViewItem item = lvCustomParams.SelectedItems[0];
                item.SubItems[1].Text = txtParamValue.Text;
                OnSettingsChanged();
            }
        }
    }
}
