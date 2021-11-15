/*
 * Copyright 2021 Elena Shiryaeva
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModDbExport
 * Summary  : Represents a control for editing event triggers option
 * 
 * Author   : Elena Shiryaeva
 * Created  : 2021
 * Modified : 2021
 */
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Scada.Server.Modules.DbExport.Config;
using System.Collections.Generic;
using Scada.UI;
using Scada.Config;
using System.Drawing;

namespace Scada.Server.Modules.DbExport.UI
{
    /// <summary>
    /// Represents a control for editing trigger event options.
    /// <para>Представляет элемент управления для редактирования параметров триггеров событий.</para>
    /// </summary>
    public partial class CtrlEventTrigger : UserControl
    {
        private EventTriggerOptions eventTiggerOptions;
        private ICollection<int> resultRange;           // range after editing
        private bool cnlNumChanged;
        private bool deviceNumChanged;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlEventTrigger()
        {
            InitializeComponent();
            cnlNumChanged = false;
            deviceNumChanged = false;
        }


        /// <summary>
        /// Gets or sets an editable trigger options.
        /// </summary>
        internal EventTriggerOptions EventTriggerOptions
        {
            get
            {
                return eventTiggerOptions;
            }
            set
            {
                if (value != null)
                {
                    eventTiggerOptions = null;
                    chkActive.Checked = value.Active;
                    txtName.Text = value.Name;

                    txtSql.AppendText(value.Query.Trim());
                    txtCnlNum.Text = RangeUtils.RangeToStr(value.CnlNums);
                    txtDeviceNum.Text = RangeUtils.RangeToStr(value.DeviceNums);

                    List<string> names = value.GetParamNames();

                    string s;
                    if (DbmsType == KnownDBMS.Oracle)  // oralce db
                        s = ":";
                    else
                        s = "@";

                    foreach (string val in names)
                        lvParametrs.Items.Add(s + val);

                    txtTriggerType.Text = LibPhrases.EventType;
                }

                eventTiggerOptions = value;
            }
        }

        /// <summary>
        ///Gets editable target DbType.
        /// </summary>
        internal KnownDBMS DbmsType { get; set; }

        /// <summary>
        /// Triggers an event TriggerOptionsChanged.
        /// </summary>
        private void OnEventTriggerOptionsChanged()
        {
            TriggerEventOptionsChanged?.Invoke(this, new ObjectChangedEventArgs(eventTiggerOptions));
        }

        /// <summary>
        /// An event that occurs when the properties of an edited trigger event options.
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler TriggerEventOptionsChanged;

        /// <summary>
        /// Sets input focus.
        /// </summary>
        public void SetFocus()
        {
            txtName.Select();
        }

        /// <summary>
        /// Clears the fields.
        /// </summary>
        public void Clear()
        {
            eventTiggerOptions = null;
            chkActive.Checked = false;
            txtSql.Text = "";
            txtCnlNum.Text = "";
            txtDeviceNum.Text = "";
            txtName.Text = "";
            lvParametrs.Items.Clear();
        }

        private void txtSql_TextChanged(object sender, EventArgs e)
        {
            if (eventTiggerOptions != null)
            {
                eventTiggerOptions.Query = txtSql.Text;
                OnEventTriggerOptionsChanged();
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (eventTiggerOptions != null)
            {
                eventTiggerOptions.Name = txtName.Text;
                OnEventTriggerOptionsChanged();
            }
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (eventTiggerOptions != null)
            {
                eventTiggerOptions.Active = chkActive.Checked;
                OnEventTriggerOptionsChanged();
            }
        }

        private void btnEditCnlNum_Click(object sender, EventArgs e)
        {
            if (eventTiggerOptions != null)
            {
                if (FrmRangeEdit.EditRange(eventTiggerOptions.CnlNums))
                {
                    txtCnlNum.Text = RangeUtils.RangeToStr(eventTiggerOptions.CnlNums);
                    txtCnlNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    OnEventTriggerOptionsChanged();
                }
            }

            txtCnlNum.Select();
            txtCnlNum.DeselectAll();
        }

        private void txtCnlNum_Validating(object sender, CancelEventArgs e)
        {
            if (eventTiggerOptions != null && cnlNumChanged)
            {
                if (RangeUtils.StrToRange(txtCnlNum.Text, true, true, out resultRange))
                {
                    // update the list of trigger input channels
                    eventTiggerOptions.CnlNums.Clear();

                    foreach (int val in resultRange)
                    {
                        eventTiggerOptions.CnlNums.Add(val);
                    }

                    txtCnlNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    OnEventTriggerOptionsChanged();
                }
                else
                {
                    // show a message
                    txtCnlNum.ForeColor = Color.Red;
                    ScadaUiUtils.ShowError(LibPhrases.RangeNotValid);
                }
            }
        }

        private void txtCnlNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtCnlNum_Validating(null, null);
        }

        private void txtCnlNum_TextChanged(object sender, EventArgs e)
        {
            cnlNumChanged = true;
        }

        private void txtCnlNum_Enter(object sender, EventArgs e)
        {
            cnlNumChanged = false;
        }



        private void btnEditDeviceNum_Click(object sender, EventArgs e)
        {
            if (eventTiggerOptions != null)
            {
                if (FrmRangeEdit.EditRange(eventTiggerOptions.DeviceNums))
                {
                    txtDeviceNum.Text = RangeUtils.RangeToStr(eventTiggerOptions.DeviceNums);
                    txtDeviceNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    OnEventTriggerOptionsChanged();
                }
            }

            txtDeviceNum.Select();
            txtDeviceNum.DeselectAll();
        }


        private void txtDeviceNum_Validating(object sender, CancelEventArgs e)
        {
            if (eventTiggerOptions != null && deviceNumChanged)
            {
                if (RangeUtils.StrToRange(txtDeviceNum.Text, true, true, out resultRange))
                {
                    // update the list of trigger device nums
                    eventTiggerOptions.DeviceNums.Clear();

                    foreach (int val in resultRange)
                    {
                        eventTiggerOptions.DeviceNums.Add(val);
                    }

                    txtDeviceNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    OnEventTriggerOptionsChanged();
                }
                else
                {
                    // show a message
                    txtDeviceNum.ForeColor = Color.Red;
                    ScadaUiUtils.ShowError(LibPhrases.RangeNotValid);
                }
            }
        }

        private void txtDeviceNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtDeviceNum_Validating(null, null);
        }

        private void txtDeviceNum_TextChanged(object sender, EventArgs e)
        {
            deviceNumChanged = true;
        }

        private void txtDeviceNum_Enter(object sender, EventArgs e)
        {
            deviceNumChanged = false;
        }
    }
}
