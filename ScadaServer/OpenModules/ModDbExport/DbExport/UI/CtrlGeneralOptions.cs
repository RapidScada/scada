/*
 * Copyright 2021 Elena Shiryaeva
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModDbExport
 * Summary  : Represents a control for editing general options
 * 
 * Author   : Elena Shiryaeva
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Scada.Server.Modules.DbExport.Config;
using Scada.UI;

namespace Scada.Server.Modules.DbExport.UI
{
    /// <summary>
    /// Represents a control for editing general options.
    /// <para>Представляет элемент управления для редактирования общих настроек.</para>
    /// </summary>
    public partial class CtrlGeneralOptions : UserControl
    {
        private GeneralOptions generalOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlGeneralOptions()
        {
            InitializeComponent();
            generalOptions = null;
        }


        /// <summary>
        /// Gets or sest an editable general options.
        /// </summary>
        internal GeneralOptions GeneralOptions
        {
            get
            {
                return generalOptions;
            }
            set
            {
                if (value != null)
                {
                    generalOptions = null;
                    txtTargetID.Text = value.ID.ToString();
                    txtName.Text = value.Name;
                    chkActive.Checked = value.Active;
                    numMaxQueueSize.Value = value.MaxQueueSize;
                    numDataLifetime.Value = value.DataLifetime;
                    numOutCnlNum.Value = value.OutCnlNum;
                }

                generalOptions = value;
            }
        }

        /// <summary>
        /// Trigger an event GeneralOptionsChanged.
        /// </summary>
        private void OnGeneralOptionsChanged()
        {
            GeneralOptionsChanged?.Invoke(this, new ObjectChangedEventArgs(generalOptions));
        }

        /// <summary>
        /// An event that occurs when the properties of an edited gerenal options change.
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler GeneralOptionsChanged;

        /// <summary>
        /// Set input focus.
        /// </summary>
        public void SetFocus()
        {
            txtName.Select();
        }


        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.Name = txtName.Text;
                OnGeneralOptionsChanged();
            }
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.Active = chkActive.Checked;            
                OnGeneralOptionsChanged();
            }
        }

        private void numMaxQueueSize_ValueChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);
                OnGeneralOptionsChanged();
            }
        }

        private void numDataLifetime_ValueChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.DataLifetime = Convert.ToInt32(numDataLifetime.Value);
                OnGeneralOptionsChanged();
            }
        }

        private void numOutCnlNum_ValueChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.OutCnlNum = Convert.ToInt32(numOutCnlNum.Value);
                OnGeneralOptionsChanged();
            }
        }
    }
}
