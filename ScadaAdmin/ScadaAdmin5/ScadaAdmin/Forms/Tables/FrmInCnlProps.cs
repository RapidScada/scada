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
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Input channel properties form.
    /// <para>Форма свойств входного канала.</para>
    /// </summary>
    public partial class FrmInCnlProps : Form
    {
        private readonly FrmBaseTable frmBaseTable;
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
        public FrmInCnlProps(FrmBaseTable frmBaseTable)
            : this()
        {
            this.frmBaseTable = frmBaseTable ?? throw new ArgumentNullException("frmBaseTable");
            dataGridView = frmBaseTable.DataGridView;
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
            return true;
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
            txtObjNum.Text = cbObj.SelectedValue?.ToString() ?? "";
        }

        private void cbKP_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKPNum.Text = cbKP.SelectedValue?.ToString() ?? "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ApplyChanges())
                DialogResult = DialogResult.OK;

            // проверка введённых данных
            /*StringBuilder errors = new StringBuilder();
            string errMsg;

            if (!AppUtils.ValidateInt(txtCnlNum.Text, 1, ushort.MaxValue, out errMsg))
                errors.AppendLine(AppPhrases.IncorrectInCnlNum).AppendLine(errMsg);
            if (txtName.Text == "")
                errors.AppendLine(AppPhrases.IncorrectInCnlName).AppendLine(CommonPhrases.NonemptyRequired);
            if (cbCnlType.SelectedValue == null)
                errors.AppendLine(AppPhrases.IncorrectCnlType).AppendLine(CommonPhrases.NonemptyRequired);
            if (txtSignal.Text != "" && !AppUtils.ValidateInt(txtSignal.Text, 1, int.MaxValue, out errMsg))
                errors.AppendLine(AppPhrases.IncorrectSignal).AppendLine(errMsg);
            string ctrlCnlNum = txtCtrlCnlNum.Text;
            if (ctrlCnlNum != "")
            {
                if (AppUtils.ValidateInt(ctrlCnlNum, 1, ushort.MaxValue, out errMsg))
                {
                    if (Tables.GetCtrlCnlName(int.Parse(ctrlCnlNum)) == "")
                        errors.AppendLine(AppPhrases.IncorrectCtrlCnlNum).
                            AppendLine(string.Format(AppPhrases.CtrlCnlNotExists, ctrlCnlNum));
                }
                else
                {
                    errors.AppendLine(AppPhrases.IncorrectCtrlCnlNum).AppendLine(errMsg);
                }
            }
            if (txtLimLowCrash.Text != "" && !AppUtils.ValidateDouble(txtLimLowCrash.Text, out errMsg))
                errors.AppendLine(AppPhrases.IncorrectLimLowCrash).AppendLine(errMsg);
            if (txtLimLow.Text != "" && !AppUtils.ValidateDouble(txtLimLow.Text, out errMsg))
                errors.AppendLine(AppPhrases.IncorrectLimLow).AppendLine(errMsg);
            if (txtLimHigh.Text != "" && !AppUtils.ValidateDouble(txtLimHigh.Text, out errMsg))
                errors.AppendLine(AppPhrases.IncorrectLimHigh).AppendLine(errMsg);
            if (txtLimHighCrash.Text != "" && !AppUtils.ValidateDouble(txtLimHighCrash.Text, out errMsg))
                errors.AppendLine(AppPhrases.IncorrectLimHighCrash).AppendLine(errMsg);
            
            errMsg = errors.ToString().TrimEnd();

            if (errMsg == "")
            {
                // передача свойств входного канала в редактируемую таблицу
                try
                {
                    DataRowView dataRow = frmTable.Table.DefaultView[row.Index];
                    dataRow["Active"] = chkActive.Checked;
                    dataRow["CnlNum"] = txtCnlNum.Text;
                    dataRow["Name"] = txtName.Text;
                    dataRow["CnlTypeID"] = cbCnlType.SelectedValue;
                    dataRow["ModifiedDT"] = DateTime.Now;
                    dataRow["ObjNum"] = cbObj.SelectedValue;
                    dataRow["KPNum"] = cbKP.SelectedValue;
                    dataRow["Signal"] = txtSignal.Text == "" ? DBNull.Value : (object)txtSignal.Text;
                    dataRow["FormulaUsed"] = chkFormulaUsed.Checked;
                    dataRow["Formula"] = txtFormula.Text == "" ? DBNull.Value : (object)txtFormula.Text;
                    dataRow["Averaging"] = chkAveraging.Checked;
                    dataRow["ParamID"] = cbParam.SelectedValue;
                    dataRow["FormatID"] = cbFormat.SelectedValue;
                    dataRow["UnitID"] = cbUnit.SelectedValue;
                    dataRow["CtrlCnlNum"] = txtCtrlCnlNum.Text == "" ? DBNull.Value : (object)txtCtrlCnlNum.Text;
                    dataRow["EvEnabled"] = chkEvEnabled.Checked;
                    dataRow["EvSound"] = chkEvSound.Checked;
                    dataRow["EvOnChange"] = chkEvOnChange.Checked;
                    dataRow["EvOnUndef"] = chkEvOnUndef.Checked;
                    dataRow["LimLowCrash"] = txtLimLowCrash.Text == "" ? DBNull.Value : (object)txtLimLowCrash.Text;
                    dataRow["LimLow"] = txtLimLow.Text == "" ? DBNull.Value : (object)txtLimLow.Text;
                    dataRow["LimHigh"] = txtLimHigh.Text == "" ? DBNull.Value : (object)txtLimHigh.Text;
                    dataRow["LimHighCrash"] = txtLimHighCrash.Text == "" ? DBNull.Value : (object)txtLimHighCrash.Text;
                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    AppUtils.ProcError(AppPhrases.WriteInCnlPropsError + ":\r\n" + ex.Message);
                    DialogResult = DialogResult.Cancel;
                }
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
            }*/
        }
    }
}
