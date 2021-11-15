/*
 * Copyright 2021 Elena Shiryaeva
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModDbExport
 * Summary  : Represents a control for editing triggers option
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
    /// Represents a control for editing trigger options.
    /// <para>Представляет элемент управления для редактирования параметров триггеров.</para>
    /// </summary>
    public partial class CtrlTrigger : UserControl
    {
        private DataTriggerOptions triggerOptions;
        private ICollection<int> resultRange;   // range after editing
        private bool cnlNumChanged;
        private bool deviceNumChanged;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlTrigger()
        {
            InitializeComponent();
            triggerOptions = null;
            cnlNumChanged = false;
            deviceNumChanged = false;
        }


        /// <summary>
        /// Gets or sets an editable trigger options.
        /// </summary>
        internal DataTriggerOptions DataTriggerOptions
        {
            get
            {
                return triggerOptions;
            }
            set
            {
                if (value != null)
                {
                    triggerOptions = null;
                    chkActive.Checked = value.Active;
                    chkSingleQuery.Checked = value.SingleQuery;
                    chkSingleQuery.Enabled = true;
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
                        lvParametrs.Items.Add(s+val);

                    if (value.TriggerType == "CurDataTrigger")
                        txtTriggerType.Text = LibPhrases.CurDataType;
                    if (value.TriggerType == "ArcDataTrigger")
                        txtTriggerType.Text = LibPhrases.ArcDataType;

                    if (txtCnlNum.Text == "")
                    {
                        chkSingleQuery.Checked = false;
                        chkSingleQuery.Enabled = false;
                    }
                    else
                    {
                        chkSingleQuery.Enabled = true;
                    }
                }

                triggerOptions = value;
            }
        }

        /// <summary>
        ///Gets editable target DbType.
        /// </summary>
        internal KnownDBMS DbmsType { get; set; }
       

        /// <summary>
        /// Trigges an event TriggerOptionsChanged.
        /// </summary>
        private void OnTriggerOptionsChanged()
        {
            TriggerOptionsChanged?.Invoke(this, new ObjectChangedEventArgs(triggerOptions));
        }

        /// <summary>
        /// An event that occurs when the properties of an edited trigger options.
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler TriggerOptionsChanged;

        /// <summary>
        /// Set input focus.
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
            triggerOptions = null;
            chkActive.Checked = false;
            txtSql.Text = "";
            txtCnlNum.Text = "";
            txtName.Text = "";
            chkSingleQuery.Checked = false;
            lvParametrs.Items.Clear();
        }

        private void txtSql_TextChanged(object sender, EventArgs e)
        {
            if (triggerOptions != null)
            {
                triggerOptions.Query = txtSql.Text;
                OnTriggerOptionsChanged();
            }
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (triggerOptions != null)
            {
                triggerOptions.Active = chkActive.Checked;
                OnTriggerOptionsChanged();
            }
        }

        private void chkSingleQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (triggerOptions != null)
            {
                triggerOptions.SingleQuery = chkSingleQuery.Checked;
                OnTriggerOptionsChanged();

                lvParametrs.Items.Clear();
                List<string> names = triggerOptions.GetParamNames();

                string s;
                if (DbmsType == KnownDBMS.Oracle)  // oralce db
                    s = ":";
                else
                    s = "@";

                foreach (string val in names)
                    lvParametrs.Items.Add(s + val);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (triggerOptions != null)
            {
                triggerOptions.Name = txtName.Text;
                OnTriggerOptionsChanged();
            }
        }


        private void btnEditCnlNum_Click(object sender, EventArgs e)
        {
            if (triggerOptions != null)
            {
                if (FrmRangeEdit.EditRange(triggerOptions.CnlNums))
                {
                    txtCnlNum.Text = RangeUtils.RangeToStr(triggerOptions.CnlNums);
                    txtCnlNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    if (txtCnlNum.Text == "")
                    {
                        chkSingleQuery.Checked = false;
                        chkSingleQuery.Enabled = false;
                    }
                    else
                    {
                        chkSingleQuery.Enabled = true;
                    }

                    OnTriggerOptionsChanged();
                }
            }

            txtCnlNum.Select();
            txtCnlNum.DeselectAll();
        }

        private void txtCnlNum_Validating(object sender, CancelEventArgs e)
        {
            if (triggerOptions != null && cnlNumChanged)
            {
                if (RangeUtils.StrToRange(txtCnlNum.Text, true, true, out resultRange))
                {
                    // update the list of trigger input channels
                    triggerOptions.CnlNums.Clear();

                    foreach (int val in resultRange)
                    {
                        triggerOptions.CnlNums.Add(val);
                    }

                    txtCnlNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    if (txtCnlNum.Text == "")
                    {
                        chkSingleQuery.Checked = false;
                        chkSingleQuery.Enabled = false;
                    }
                    else
                    {
                        chkSingleQuery.Enabled = true;
                    }

                    OnTriggerOptionsChanged();
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
            if (triggerOptions != null)
            {
                if (FrmRangeEdit.EditRange(triggerOptions.DeviceNums))
                {
                    txtDeviceNum.Text = RangeUtils.RangeToStr(triggerOptions.DeviceNums);
                    txtDeviceNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
               
                    OnTriggerOptionsChanged();
                }
            }

            txtDeviceNum.Select();
            txtDeviceNum.DeselectAll();
        }


        private void txtDeviceNum_Validating(object sender, CancelEventArgs e)
        {
            if (triggerOptions != null && deviceNumChanged)
            {
                if (RangeUtils.StrToRange(txtDeviceNum.Text, true, true, out resultRange))
                {
                    // update the list of trigger device nums
                    triggerOptions.DeviceNums.Clear();

                    foreach (int val in resultRange)
                    {
                        triggerOptions.DeviceNums.Add(val);
                    }

                    txtDeviceNum.ForeColor = Color.FromKnownColor(KnownColor.WindowText);

                    OnTriggerOptionsChanged();
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
