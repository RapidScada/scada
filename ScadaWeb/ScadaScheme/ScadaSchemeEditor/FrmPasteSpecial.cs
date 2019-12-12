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
 * Module   : Scheme Editor
 * Summary  : Special paste parameters form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Special paste parameters form.
    /// <para>Форма параметров специальной вставки.</para>
    /// </summary>
    public partial class FrmPasteSpecial : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPasteSpecial()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the special paste parameters.
        /// </summary>
        public PasteSpecialParams PasteSpecialParams { get; set; }


        private void FrmPasteSpecial_Load(object sender, EventArgs e)
        {
            if (PasteSpecialParams == null)
                throw new InvalidOperationException("PasteSpecialParams must not be null.");

            Translator.TranslateForm(this, GetType().FullName);
            numInCnlOffset.SetValue(PasteSpecialParams.InCnlOffset);
            numCtrlCnlOffset.SetValue(PasteSpecialParams.CtrlCnlOffset);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            PasteSpecialParams.InCnlOffset = Convert.ToInt32(numInCnlOffset.Value);
            PasteSpecialParams.CtrlCnlOffset = Convert.ToInt32(numCtrlCnlOffset.Value);
            DialogResult = DialogResult.OK;
        }
    }
}
