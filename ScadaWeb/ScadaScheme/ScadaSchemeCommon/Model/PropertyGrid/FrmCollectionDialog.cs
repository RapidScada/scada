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
 * Module   : ScadaSchemeCommon
 * Summary  : Form for editing collection
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2019
 */

using Scada.UI;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Form for editing collection.
    /// <para>Форма редактирования коллекции.</para>
    /// </summary>
    internal partial class FrmCollectionDialog : Form
    {
        private IList collection; // the edited collection
        private Type itemType;    // the type of collection items
        private ISchemeViewAvailable schemeViewAvailable; // the scheme view reference


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCollectionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCollectionDialog(IList collection, Type itemType, ISchemeViewAvailable schemeViewAvailable)
            : this()
        {
            this.collection = collection ?? throw new ArgumentNullException("collection");
            this.itemType = itemType ?? throw new ArgumentNullException("itemType");
            this.schemeViewAvailable = schemeViewAvailable ?? throw new ArgumentNullException("schemeViewAvailable");
        }


        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        private void SetBtnsEnabled()
        {
            int selInd = lbItems.SelectedIndex;
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
                btnDown.Enabled = selInd < lbItems.Items.Count - 1;
            }
            
        }


        private void FrmConditionDialog_Load(object sender, EventArgs e)
        {
            // translate the form
            Translator.TranslateForm(this, GetType().FullName);
            string itemTypeDisplayName = Localization.GetDictionary(itemType.FullName)
                .GetPhrase("DisplayName", itemType.Name);
            Text = string.Format(Text, itemTypeDisplayName);

            // display the collection items
            lbItems.BeginUpdate();

            try
            {
                foreach (object item in collection)
                {
                    if (item is ICloneable cloneable)
                        lbItems.Items.Add(cloneable.Clone());
                    else
                        throw new ScadaException("Collection items must implement ICloneable.");
                }
            }
            finally
            {
                lbItems.EndUpdate();
            }

            if (lbItems.Items.Count > 0)
                lbItems.SelectedIndex = 0;
        }

        private void lbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            // вывод свойств выбранного элемента
            propGrid.SelectedObject = lbItems.SelectedItem;
            SetBtnsEnabled();
        }

        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // перерисовка элемента в списке
            int selInd = lbItems.SelectedIndex;
            if (selInd >= 0)
                lbItems.Items[selInd] = lbItems.Items[selInd];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // добавление элемента
            object item = Activator.CreateInstance(itemType);

            if (item is ISchemeViewAvailable schemeViewAvailable)
                schemeViewAvailable.SchemeView = this.schemeViewAvailable.SchemeView;

            lbItems.SelectedIndex = lbItems.Items.Add(item);
            propGrid.Select();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            // удаление элемента
            int selInd = lbItems.SelectedIndex;

            if (selInd >= 0)
            {
                lbItems.Items.RemoveAt(selInd);
                int itemCnt = lbItems.Items.Count;
                if (itemCnt > 0)
                    lbItems.SelectedIndex = selInd < itemCnt ? selInd : itemCnt - 1;
            }

            propGrid.Select();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            // перемещение элемента вверх
            int selInd = lbItems.SelectedIndex;

            if (selInd > 0)
            {
                lbItems.SelectedIndexChanged -= lbItems_SelectedIndexChanged;
                lbItems.BeginUpdate();

                object item = lbItems.SelectedItem;
                lbItems.Items.RemoveAt(selInd);
                lbItems.Items.Insert(selInd - 1, item);
                lbItems.SelectedIndex = selInd - 1;

                lbItems.EndUpdate();
                lbItems.SelectedIndexChanged += lbItems_SelectedIndexChanged;
                SetBtnsEnabled();
            }

            propGrid.Select();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            // перемещение элемента вниз
            int selInd = lbItems.SelectedIndex;
            int itemCnt = lbItems.Items.Count;

            if (selInd < itemCnt - 1)
            {
                lbItems.SelectedIndexChanged -= lbItems_SelectedIndexChanged;
                lbItems.BeginUpdate();

                object item = lbItems.SelectedItem;
                lbItems.Items.RemoveAt(selInd);
                lbItems.Items.Insert(selInd + 1, item);
                lbItems.SelectedIndex = selInd + 1;

                lbItems.EndUpdate();
                lbItems.SelectedIndexChanged += lbItems_SelectedIndexChanged;
                SetBtnsEnabled();
            }

            propGrid.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // заполнение коллекции после редактирования
            collection.Clear();

            foreach (object item in lbItems.Items)
            {
                collection.Add(item);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
