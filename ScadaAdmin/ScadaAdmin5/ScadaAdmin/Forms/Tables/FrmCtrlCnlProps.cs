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
 * Summary  : Output channel properties form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
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
    /// Output channel properties form.
    /// <para>Форма свойств канала управления.</para>
    /// </summary>
    public partial class FrmCtrlCnlProps : Form
    {
        private readonly DataGridView dataGridView;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCtrlCnlProps()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCtrlCnlProps(DataGridView dataGridView)
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
                txtCtrlCnlNum.SetText(cells["CtrlCnlNum"]);
                txtName.SetText(cells["Name"]);
                cbCmdType.SetValue(cells["CmdTypeID"]);
                cbObj.SetValue(cells["ObjNum"]);
                cbKP.SetValue(cells["KPNum"]);
                txtCmdNum.SetText(cells["CmdNum"]);
                cbCmdVal.SetValue(cells["CmdValID"]);
                chkFormulaUsed.SetChecked(cells["FormulaUsed"]);
                txtFormula.SetText(cells["Formula"]);
                chkEvEnabled.SetChecked(cells["EvEnabled"]);
            }
        }

        /// <summary>
        /// Validates and applies the property changes.
        /// </summary>
        private bool ApplyChanges()
        {
            // validate changes
            StringBuilder sbError = new StringBuilder();

            if (!(int.TryParse(txtCtrlCnlNum.Text, out int cnlNum) && 1 <= cnlNum && cnlNum <= AdminUtils.MaxCnlNum))
                sbError.AppendError(lblCtrlCnlNum, string.Format(CommonPhrases.IntegerRangingRequired, 1, AdminUtils.MaxCnlNum));

            if (cbCmdType.SelectedValue == null)
                sbError.AppendError(lblCmdType, CommonPhrases.NonemptyRequired);

            int cmdNum = -1;
            if (txtCmdNum.Text != "" && !int.TryParse(txtCmdNum.Text, out cmdNum))
                sbError.AppendError(lblCmdNum, CommonPhrases.IntegerRequired);

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
                cells["CtrlCnlNum"].Value = cnlNum;
                cells["Name"].Value = txtName.Text;
                cells["CmdTypeID"].Value = cbCmdType.SelectedValue ?? DBNull.Value;
                cells["ObjNum"].Value = cbObj.SelectedValue ?? DBNull.Value;
                cells["KPNum"].Value = cbKP.SelectedValue ?? DBNull.Value;
                cells["CmdNum"].Value = cmdNum > 0 ? (object)cmdNum : DBNull.Value;
                cells["CmdValID"].Value = cbCmdVal.SelectedValue ?? DBNull.Value;
                cells["FormulaUsed"].Value = chkFormulaUsed.Checked;
                cells["Formula"].Value = txtFormula.Text;
                cells["EvEnabled"].Value = chkEvEnabled.Checked;
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
