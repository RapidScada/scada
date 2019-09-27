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
 * Summary  : Control for editing a subscription
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
    /// Control for editing a subscription.
    /// <para>Элемент управления для редактирования подписки.</para>
    /// </summary>
    public partial class CtrlSubscription : UserControl
    {
        private SubscriptionConfig subscriptionConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlSubscription()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the edited subscription.
        /// </summary>
        public SubscriptionConfig SubscriptionConfig
        {
            get
            {
                return subscriptionConfig;
            }
            set
            {
                subscriptionConfig = null;
                ShowSubscriptionProps(value);
                subscriptionConfig = value;
            }
        }


        /// <summary>
        /// Shows the subscription properties.
        /// </summary>
        private void ShowSubscriptionProps(SubscriptionConfig subscriptionConfig)
        {
            if (subscriptionConfig != null)
            {
                chkSubscrActive.Checked = subscriptionConfig.Active;
                txtDisplayName.Text = subscriptionConfig.DisplayName;
                numPublishingInterval.SetValue(subscriptionConfig.PublishingInterval);
            }
        }

        /// <summary>
        /// Raises the ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(subscriptionConfig, changeArgument));
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            txtDisplayName.Select();
        }


        /// <summary>
        /// Occurs when a property of the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler ObjectChanged;


        private void chkSubscrActive_CheckedChanged(object sender, EventArgs e)
        {
            if (subscriptionConfig != null)
            {
                subscriptionConfig.Active = chkSubscrActive.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (subscriptionConfig != null)
            {
                subscriptionConfig.DisplayName = txtDisplayName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void numPublishingInterval_ValueChanged(object sender, EventArgs e)
        {
            if (subscriptionConfig != null)
            {
                subscriptionConfig.PublishingInterval = Convert.ToInt32(numPublishingInterval.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
