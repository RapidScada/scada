/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : ScadaSchemeCommon
 * Summary  : Editing channel filter form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Scheme
{
    /// <summary>
    /// Editing channel filter form
    /// <para>Форма редактирования фильтра по каналам</para>
    /// </summary>
    public partial class FrmCnlsFilterDialog : Form
    {
        private EditorData editorData; // ссылка на данные редактора схем
        private List<int> cnlsFilter;  // ссылка на фильтр по входным каналам

        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmCnlsFilterDialog()
        {
            InitializeComponent();
            cnlsFilter = null;
        }

        private void FrmCnlsFilterDialog_Load(object sender, EventArgs e)
        {
            // перевод формы
            Localization.TranslateForm(this, "Scada.Scheme.FrmCnlsFilterDialog");

            // вывод фильтра по входным каналам
            SchemeApp schemeApp = SchemeApp.GetSchemeApp();
            editorData = schemeApp.EditorData;
            if (editorData != null && editorData.SchemeView != null)
                cnlsFilter = schemeApp.EditorData.SchemeView.CnlsFilter;

            if (cnlsFilter != null)
                txtCnlNums.Text = string.Join<int>(" ", cnlsFilter);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // установка фильтра по входным каналам
            if (cnlsFilter != null)
            {
                cnlsFilter.Clear();
                editorData.Modified = true;
                editorData.SetFormTitle();
                string[] cnlNums = txtCnlNums.Text.Split(SchemeView.Separator, StringSplitOptions.RemoveEmptyEntries);

                foreach (string cnlNumStr in cnlNums)
                {
                    int cnlNum;
                    if (int.TryParse(cnlNumStr, out cnlNum))
                    {
                        int ind = cnlsFilter.BinarySearch(cnlNum);
                        if (ind < 0)
                            cnlsFilter.Insert(~ind, cnlNum);
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
