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
 * Summary  : Form for creating a new file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Form for creating a new file.
    /// <para>Форма создания нового файла.</para>
    /// </summary>
    public partial class FrmFileNew : Form
    {
        /// <summary>
        /// The default file name without extension.
        /// </summary>
        private const string DefaultFileName = "NewFile";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFileNew()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets the short file name.
        /// </summary>
        public string FileName
        {
            get
            {
                return txtFileName.Text.Trim();
            }
        }

        /// <summary>
        /// Gets the file type.
        /// </summary>
        public KnownFileType FileType
        {
            get
            {
                return GetSelectedFileType();
            }
        }


        /// <summary>
        /// Gets the selected file type.
        /// </summary>
        private KnownFileType GetSelectedFileType()
        {
            switch (lbFileType.SelectedIndex)
            {
                case 1:
                    return KnownFileType.TableView;
                case 2:
                    return KnownFileType.TextFile;
                case 3:
                    return KnownFileType.XmlFile;
                default:
                    return KnownFileType.SchemeView;
            }
        }

        /// <summary>
        /// Corrects the file extension according to the file type.
        /// </summary>
        private void FixFileExtenstion()
        {
            if (!string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                string ext = FileCreator.GetExtension(GetSelectedFileType());
                txtFileName.Text = Path.ChangeExtension(txtFileName.Text, ext);
            }
        }

        /// <summary>
        /// Validates the form field.
        /// </summary>
        private bool ValidateFields()
        {
            string fileName = FileName;

            if (fileName == "")
            {
                ScadaUiUtils.ShowError(AppPhrases.FileNameEmpty);
                return false;
            }

            if (!AdminUtils.NameIsValid(fileName))
            {
                ScadaUiUtils.ShowError(AppPhrases.FileNameInvalid);
                return false;
            }

            return true;
        }


        private void FrmFileNew_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            txtFileName.Text = DefaultFileName;
            lbFileType.SelectedIndex = 0;
        }

        private void lbFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FixFileExtenstion();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
                DialogResult = DialogResult.OK;
        }
    }
}
