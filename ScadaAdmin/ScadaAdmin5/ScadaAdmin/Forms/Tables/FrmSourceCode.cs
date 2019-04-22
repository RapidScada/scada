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
 * Summary  : Form for editing source code of a formula
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2019
 */

using System;
using System.Text;
using System.Windows.Forms;
using Scada.UI;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Form for editing source code of a formula.
    /// <para>Форма редактирования исходного кода формулы.</para>
    /// </summary>
    public partial class FrmSourceCode : Form
    {
        /// <summary>
        /// The maximum length of the source code by default.
        /// </summary>
        private const int DefaultMaxLength = 1000;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSourceCode()
        {
            InitializeComponent();

            MaxLength = DefaultMaxLength;
            SourceCode = "";
        }


        /// <summary>
        /// Gets or sets the maximum length of the source code.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the source code.
        /// </summary>
        public string SourceCode { get; set; }


        /// <summary>
        /// Normalizes line endings of the specified string.
        /// </summary>
        private string Normalize(string s)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (s != null)
            {
                foreach (char c in s)
                {
                    switch (c)
                    {
                        case '\r':
                            break;
                        case '\n':
                            stringBuilder.AppendLine();
                            break;
                        default:
                            stringBuilder.Append(c);
                            break;
                    }
                }
            }

            return stringBuilder.ToString();
        }


        private void FrmEditSource_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            txtSourceCode.MaxLength = MaxLength;
            string sourceCode = Normalize(SourceCode);
            txtSourceCode.Text = sourceCode.Length <= MaxLength ? sourceCode : sourceCode.Substring(0, MaxLength); 
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            lblTextLength.Text = txtSourceCode.Text.Length + " / " + txtSourceCode.MaxLength;
            btnOK.Enabled = txtSourceCode.Text != "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SourceCode = txtSourceCode.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
