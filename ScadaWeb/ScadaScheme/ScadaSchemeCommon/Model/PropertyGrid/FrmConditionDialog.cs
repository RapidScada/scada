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
 * Summary  : Form for editing image output conditions
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using Scada.Scheme.Model.DataTypes;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Form for editing image output conditions
    /// <para>Форма редактирования условий вывода изображений</para>
    /// </summary>
    internal partial class FrmConditionDialog : Form
    {
        private List<Condition> conditions; // ссылка на редактируемые условия
        IObservableItem observableItem;     // элемент, изменения которого отслеживаются


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private FrmConditionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmConditionDialog(List<Condition> conditions, IObservableItem observableItem)
            : this()
        {
            if (conditions == null)
                throw new ArgumentNullException("conditions");
            if (observableItem == null)
                throw new ArgumentNullException("observableItem");

            this.conditions = conditions;
            this.observableItem = observableItem;
        }


        /// <summary>
        /// Получить или установить выбранные условия
        /// </summary>
        public List<Condition> SelectedConditions { get; set; }


        /// <summary>
        /// Установить доступность кнопок
        /// </summary>
        private void SetBtnsEnabled()
        {
            int selInd = lbCond.SelectedIndex;
            if (selInd < 0)
            {
                btnDel.Enabled = false;
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
            else
            {
                btnDel.Enabled = true;
                btnUp.Enabled = selInd > 0;
                btnDown.Enabled = selInd < lbCond.Items.Count - 1;
            }
            
        }


        private void FrmConditionDialog_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Scheme.Model.PropertyGrid.FrmConditionDialog");

            // вывод условий
            lbCond.BeginUpdate();
            try
            {
                foreach (Condition condition in conditions)
                    lbCond.Items.Add(condition.Clone());
            }
            finally
            {
                lbCond.EndUpdate();
            }

            if (lbCond.Items.Count > 0)
                lbCond.SelectedIndex = 0;
        }

        private void lbCond_SelectedIndexChanged(object sender, EventArgs e)
        {
            // вывод свойств выбранного условия
            propGrid.SelectedObject = lbCond.SelectedItem;
            SetBtnsEnabled();
        }

        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // перерисовка условия в списке
            int selInd = lbCond.SelectedIndex;
            if (selInd >= 0)
            {
                lbCond.SelectedIndexChanged -= lbCond_SelectedIndexChanged;
                lbCond.BeginUpdate();

                object item = lbCond.SelectedItem;
                lbCond.Items.RemoveAt(selInd);
                lbCond.Items.Insert(selInd, item);
                lbCond.SelectedIndex = selInd;

                lbCond.EndUpdate();
                lbCond.SelectedIndexChanged += lbCond_SelectedIndexChanged;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // добавление условия
            Condition cond = new Condition();
            lbCond.SelectedIndex = lbCond.Items.Add(cond);
            propGrid.Select();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            // удаление условия
            int selInd = lbCond.SelectedIndex;

            if (selInd >= 0)
            {
                lbCond.Items.RemoveAt(selInd);
                int itemCnt = lbCond.Items.Count;
                if (itemCnt > 0)
                    lbCond.SelectedIndex = selInd < itemCnt ? selInd : itemCnt - 1;
            }

            propGrid.Select();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            // перемещение условия вверх
            int selInd = lbCond.SelectedIndex;

            if (selInd > 0)
            {
                lbCond.SelectedIndexChanged -= lbCond_SelectedIndexChanged;
                lbCond.BeginUpdate();

                object item = lbCond.SelectedItem;
                lbCond.Items.RemoveAt(selInd);
                lbCond.Items.Insert(selInd - 1, item);
                lbCond.SelectedIndex = selInd - 1;

                lbCond.EndUpdate();
                lbCond.SelectedIndexChanged += lbCond_SelectedIndexChanged;
                SetBtnsEnabled();
            }

            propGrid.Select();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            // перемещение условия вниз
            int selInd = lbCond.SelectedIndex;
            int itemCnt = lbCond.Items.Count;

            if (selInd < itemCnt - 1)
            {
                lbCond.SelectedIndexChanged -= lbCond_SelectedIndexChanged;
                lbCond.BeginUpdate();

                object item = lbCond.SelectedItem;
                lbCond.Items.RemoveAt(selInd);
                lbCond.Items.Insert(selInd + 1, item);
                lbCond.SelectedIndex = selInd + 1;

                lbCond.EndUpdate();
                lbCond.SelectedIndexChanged += lbCond_SelectedIndexChanged;
                SetBtnsEnabled();
            }

            propGrid.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // заполнение списка условий после редактирования
            conditions.Clear();

            foreach (object item in lbCond.Items)
                conditions.Add((Condition)item);

            DialogResult = DialogResult.OK;
        }
    }
}
