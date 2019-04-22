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
 * Summary  : Input channel properties form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.UI;
using System;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Input channel properties form.
    /// <para>Форма свойств входного канала.</para>
    /// </summary>
    public partial class FrmInCnlProps : Form
    {
        private readonly DataGridView dataGridView;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmInCnlProps()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInCnlProps(DataGridView dataGridView)
            : this()
        {
            this.dataGridView = dataGridView ?? throw new ArgumentNullException("dataGridView");
        }


        /// <summary>
        /// Shows the properties of the current item.
        /// </summary>
        private void ShowItemProps()
        {
            if (dataGridView.CurrentRow == null)
            {
                btnOK.Enabled = false;
            }
            else
            {
                DataGridViewCellCollection cells = dataGridView.CurrentRow.Cells;
                chkActive.SetChecked(cells["Active"]);
                txtCnlNum.SetText(cells["CnlNum"]);
                txtName.SetText(cells["Name"]);
                cbCnlType.SetValue(cells["CnlTypeID"]);
                cbObj.SetValue(cells["ObjNum"]);
                cbKP.SetValue(cells["KPNum"]);
                txtSignal.SetText(cells["Signal"]);
                chkFormulaUsed.SetChecked(cells["FormulaUsed"]);
                txtFormula.SetText(cells["Formula"]);
                chkAveraging.SetChecked(cells["Averaging"]);
                cbParam.SetValue(cells["ParamID"]);
                cbFormat.SetValue(cells["FormatID"]);
                cbUnit.SetValue(cells["UnitID"]);
                txtCtrlCnlNum.SetText(cells["CtrlCnlNum"]);
                chkEvEnabled.SetChecked(cells["EvEnabled"]);
                chkEvSound.SetChecked(cells["EvSound"]);
                chkEvOnChange.SetChecked(cells["EvOnChange"]);
                chkEvOnUndef.SetChecked(cells["EvOnUndef"]);
                txtLimLowCrash.SetText(cells["LimLowCrash"]);
                txtLimLow.SetText(cells["LimLow"]);
                txtLimHigh.SetText(cells["LimHigh"]);
                txtLimHighCrash.SetText(cells["LimHighCrash"]);
            }
        }

        /// <summary>
        /// Validates and applies the property changes.
        /// </summary>
        private bool ApplyChanges()
        {
            // validate changes
            StringBuilder sbError = new StringBuilder();

            if (!(int.TryParse(txtCnlNum.Text, out int cnlNum) && 1 <= cnlNum && cnlNum <= AdminUtils.MaxCnlNum))
                sbError.AppendError(lblCnlNum, string.Format(CommonPhrases.IntegerRangingRequired, 1, AdminUtils.MaxCnlNum));

            if (cbCnlType.SelectedValue == null)
                sbError.AppendError(lblCnlType, CommonPhrases.NonemptyRequired);

            int signal = -1;
            if (txtSignal.Text != "" && !int.TryParse(txtSignal.Text, out signal))
                sbError.AppendError(lblSignal, CommonPhrases.IntegerRequired);

            int ctrlCnlNum = -1;
            if (txtCtrlCnlNum.Text != "" && !int.TryParse(txtCtrlCnlNum.Text, out ctrlCnlNum))
                sbError.AppendError(lblCtrlCnlNum, CommonPhrases.IntegerRequired);

            double limLowCrash = double.NaN;
            if (txtLimLowCrash.Text != "" && !double.TryParse(txtLimLowCrash.Text, out limLowCrash))
                sbError.AppendError(lblLimLowCrash, CommonPhrases.RealRequired);

            double limLow = double.NaN;
            if (txtLimLow.Text != "" && !double.TryParse(txtLimLow.Text, out limLow))
                sbError.AppendError(lblLimLow, CommonPhrases.RealRequired);

            double limHigh = double.NaN;
            if (txtLimHigh.Text != "" && !double.TryParse(txtLimHigh.Text, out limHigh))
                sbError.AppendError(lblLimHigh, CommonPhrases.RealRequired);

            double limHighCrash = double.NaN;
            if (txtLimHighCrash.Text != "" && !double.TryParse(txtLimHighCrash.Text, out limHighCrash))
                sbError.AppendError(lblLimHighCrash, CommonPhrases.RealRequired);

            if (sbError.Length > 0)
            {
                sbError.Insert(0, AppPhrases.CorrectErrors + Environment.NewLine);
                ScadaUiUtils.ShowError(sbError.ToString());
            }
            else if (dataGridView.CurrentRow != null)
            {
                // apply changes
                DataGridViewCellCollection cells = dataGridView.CurrentRow.Cells;
                cells["Active"].Value = chkActive.Checked;
                cells["CnlNum"].Value = cnlNum;
                cells["Name"].Value = txtName.Text;
                cells["CnlTypeID"].Value = cbCnlType.SelectedValue ?? DBNull.Value;
                cells["ObjNum"].Value = cbObj.SelectedValue ?? DBNull.Value;
                cells["KPNum"].Value = cbKP.SelectedValue ?? DBNull.Value;
                cells["Signal"].Value = signal > 0 ? (object)signal : DBNull.Value;
                cells["FormulaUsed"].Value = chkFormulaUsed.Checked;
                cells["Formula"].Value = txtFormula.Text;
                cells["Averaging"].Value = chkAveraging.Checked;
                cells["ParamID"].Value = cbParam.SelectedValue ?? DBNull.Value;
                cells["FormatID"].Value = cbFormat.SelectedValue ?? DBNull.Value;
                cells["UnitID"].Value = cbUnit.SelectedValue ?? DBNull.Value;
                cells["CtrlCnlNum"].Value = ctrlCnlNum > 0 ? (object)ctrlCnlNum : DBNull.Value;
                cells["EvEnabled"].Value = chkEvEnabled.Checked;
                cells["EvSound"].Value = chkEvSound.Checked;
                cells["EvOnChange"].Value = chkEvOnChange.Checked;
                cells["EvOnUndef"].Value = chkEvOnUndef.Checked;
                cells["LimLowCrash"].Value = double.IsNaN(limLowCrash) ? DBNull.Value : (object)limLowCrash;
                cells["LimLow"].Value = double.IsNaN(limLow) ? DBNull.Value : (object)limLow;
                cells["LimHigh"].Value = double.IsNaN(limHigh) ? DBNull.Value : (object)limHigh;
                cells["LimHighCrash"].Value = double.IsNaN(limHighCrash) ? DBNull.Value : (object)limHighCrash;
                return true;
            }

            return false;
        }


        private void FrmInCnlProps_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            ShowItemProps();

            if (ScadaUtils.IsRunningOnMono)
                btnOK.Enabled = false; // because the combo boxes are not filled
        }

        private void cbObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtObjNum.Text = cbObj.SelectedValue is int intVal ? intVal.ToString() : "";
        }

        private void cbKP_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKPNum.Text = cbKP.SelectedValue is int intVal ? intVal.ToString() : "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ApplyChanges())
                DialogResult = DialogResult.OK;
        }
    }
}
