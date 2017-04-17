/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : Form for editing channel filter
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Form for editing channel filter
    /// <para>Форма редактирования фильтра по каналам</para>
    /// </summary>
    internal partial class FrmCnlFilterDialog : Form
    {
        private List<int> cnlFilter;    // ссылка на редактируемый фильтр по входным каналам
        IObservableItem observableItem; // элемент, изменения которого отслеживаются


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private FrmCnlFilterDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmCnlFilterDialog(List<int> cnlFilter, IObservableItem observableItem)
            : this()
        {
            if (cnlFilter == null)
                throw new ArgumentNullException("cnlFilter");
            if (observableItem == null)
                throw new ArgumentNullException("observableItem");

            this.cnlFilter = cnlFilter;
            this.observableItem = observableItem;
        }


        private void FrmCnlsFilterDialog_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Scheme.Model.PropertyGrid.FrmCnlFilterDialog");
            
            // вывод фильтра по входным каналам
            txtCnlNums.Text = string.Join(CnlFilterConverter.MainCnlSep, cnlFilter);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // разбор текста и заполнение фильтра по входным каналам
            cnlFilter.Clear();
            string[] cnlNums = txtCnlNums.Text.Split(CnlFilterConverter.CnlSep, 
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string cnlNumStr in cnlNums)
            {
                int cnlNum;
                if (int.TryParse(cnlNumStr, out cnlNum))
                {
                    int ind = cnlFilter.BinarySearch(cnlNum);
                    if (ind < 0)
                        cnlFilter.Insert(~ind, cnlNum);
                }
            }

            observableItem.OnItemChanged();
            DialogResult = DialogResult.OK;
        }
    }
}
