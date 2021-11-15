/*
 * Copyright 2021 Elena Shiryaeva
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModDbExport
 * Summary  : Range edit or add form
 * 
 * Author   : Elena Shiryaeva
 * Created  : 2020
 * Modified : 2021
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Server.Modules.DbExport.UI
{
    /// <summary>
    /// Range edit or add form
    /// <para>Форма редактирования или добавления диапазонов</para>
    /// </summary>
    public partial class FrmRangeEdit : Form
    {
        private ICollection<int> resultRange; // range after editing

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmRangeEdit()
        {
            InitializeComponent();
            resultRange = null;
        }

        /// <summary>
        /// Edits Range or Adds Range.
        /// </summary>
        public static bool EditRange(ICollection<int> range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            FrmRangeEdit frmRange = new FrmRangeEdit();
            frmRange.txtRange.Text = RangeUtils.RangeToStr(range);

            if (frmRange.ShowDialog() == DialogResult.OK)
            {
                range.Clear();

                foreach (int val in frmRange.resultRange)
                {
                    range.Add(val);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void FrmRangeEdit_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            LibPhrases.Init();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (RangeUtils.StrToRange(txtRange.Text, true, true, out resultRange))
                DialogResult = DialogResult.OK;
            else
                ScadaUiUtils.ShowError(LibPhrases.RangeNotValid);
        }
    }
}
