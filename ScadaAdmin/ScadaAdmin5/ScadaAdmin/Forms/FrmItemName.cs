/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : Form for entering a name of a project item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.App.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for entering a name of a project item.
    /// <para>Форма ввода наименования элемента проекта.</para>
    /// </summary>
    public partial class FrmItemName : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmItemName()
        {
            InitializeComponent();

            ItemName = "";
            ExistingNames = null;
            Modified = false;
        }


        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        public string ItemName
        {
            get
            {
                return txtName.Text.Trim();
            }
            set
            {
                txtName.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the names of the existing items in lower case for uniqueness checking.
        /// </summary>
        public HashSet<string> ExistingNames { get; set; }

        /// <summary>
        /// Gets a value indicating whether the name was modified.
        /// </summary>
        public bool Modified { get; private set; }


        /// <summary>
        /// Validates the form field.
        /// </summary>
        private bool ValidateField()
        {
            string name = ItemName;

            if (name == "")
            {
                ScadaUiUtils.ShowError(AppPhrases.ItemNameEmpty);
                return false;
            }

            if (!AdminUtils.NameIsValid(name))
            {
                ScadaUiUtils.ShowError(AppPhrases.ItemNameInvalid);
                return false;
            }

            if (ExistingNames != null && ExistingNames.Contains(name.ToLowerInvariant()))
            {
                ScadaUiUtils.ShowError(AppPhrases.ItemNameDuplicated);
                return false;
            }

            return true;
        }


        private void FrmInstanceName_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            Modified = false;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateField())
                DialogResult = DialogResult.OK;
        }
    }
}
