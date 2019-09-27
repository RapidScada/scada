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
 * Module   : KpOpcUa
 * Summary  : Control for editing a monitored item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Comm.Devices.OpcUa.Config;
using Scada.UI;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Comm.Devices.OpcUa.UI
{
    /// <summary>
    /// Control for editing a monitored item.
    /// <para>Элемент управления для редактирования отслеживаемого элемента.</para>
    /// </summary>
    public partial class CtrlItem : UserControl
    {
        private ItemConfig itemConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlItem()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the edited monitored item.
        /// </summary>
        public ItemConfig ItemConfig
        {
            get
            {
                return itemConfig;
            }
            set
            {
                itemConfig = null;
                ShowItemProps(value);
                itemConfig = value;
            }
        }


        /// <summary>
        /// Shows the monitored item properties.
        /// </summary>
        private void ShowItemProps(ItemConfig itemConfig)
        {
            if (itemConfig != null)
            {
                chkItemActive.Checked = itemConfig.Active;
                txtDisplayName.Text = itemConfig.DisplayName;
                txtNodeID.Text = itemConfig.NodeID;
                chkIsArray.Checked = itemConfig.IsArray;
                numArrayLen.Enabled = itemConfig.IsArray;
                numArrayLen.SetValue(itemConfig.ArrayLen);
                numCnlNum.SetValue(itemConfig.CnlNum);

                if (itemConfig.Tag is FrmConfig.ItemConfigTag tag)
                    txtSignal.Text = tag.GetSignalStr();
            }
        }

        /// <summary>
        /// Updates the information in the item tag.
        /// </summary>
        private void UpdateTag()
        {
            if (itemConfig.Tag is FrmConfig.ItemConfigTag tag)
            {
                tag.SetLength(itemConfig.IsArray, itemConfig.ArrayLen);
                txtSignal.Text = tag.GetSignalStr();
            }
        }

        /// <summary>
        /// Raises the ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(itemConfig, changeArgument));
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            txtDisplayName.Select();
        }

        /// <summary>
        /// Shows the signal value.
        /// </summary>
        public void ShowSignal()
        {
            if (itemConfig?.Tag is FrmConfig.ItemConfigTag tag)
                txtSignal.Text = tag.GetSignalStr();
        }


        /// <summary>
        /// Occurs when a property of the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler ObjectChanged;


        private void chkItemActive_CheckedChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.Active = chkItemActive.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.DisplayName = txtDisplayName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void chkIsArray_CheckedChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                if (chkIsArray.Checked)
                {
                    itemConfig.IsArray = true;
                    numArrayLen.Enabled = true;
                }
                else
                {
                    itemConfig.IsArray = false;
                    numArrayLen.SetValue(1);
                    numArrayLen.Enabled = false;
                }

                UpdateTag();
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numArrayLen_ValueChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.ArrayLen = Convert.ToInt32(numArrayLen.Value);
                UpdateTag();
                OnObjectChanged(TreeUpdateTypes.UpdateSignals);
            }
        }

        private void numCnlNum_ValueChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.CnlNum = Convert.ToInt32(numCnlNum.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
