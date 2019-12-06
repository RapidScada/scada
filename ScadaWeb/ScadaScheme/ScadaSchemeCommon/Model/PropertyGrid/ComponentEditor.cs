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
 * Summary  : Component selection editor for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Component selection editor for PropertyGrid.
    /// <para>Редактор выбора компонента для PropertyGrid.</para>
    /// </summary>
    public class ComponentEditor : UITypeEditor
    {
        private const int ItemCount = 12;
        private IWindowsFormsEditorService editorSvc = null;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            editorSvc = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (context?.Instance is ISchemeViewAvailable schemeViewAvailable && editorSvc != null)
            {
                // создание и заполнение выпадающего списка
                ListBox listBox = new ListBox
                {
                    BorderStyle = BorderStyle.None,
                    IntegralHeight = false
                };

                int selectedID = value is int intVal ? intVal : 0;
                int selectedIndex = -1;

                foreach (BaseComponent component in schemeViewAvailable.SchemeView.Components.Values)
                {
                    int index = listBox.Items.Add(component);
                    if (component.ID == selectedID)
                        selectedIndex = index;
                }

                if (selectedIndex >= 0)
                    listBox.SelectedIndex = selectedIndex;

                listBox.Height = Math.Min(listBox.Items.Count, ItemCount) * listBox.ItemHeight;
                listBox.Click += listBox_Click;
                listBox.KeyDown += listBox_KeyDown;

                // отображение выпадающего списка
                editorSvc.DropDownControl(listBox);

                // установка выбранного значения
                value = (listBox.SelectedItem as BaseComponent)?.ID ?? 0;

                // очистка ресурсов выпадающего списка
                listBox.Dispose();
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private void listBox_Click(object sender, EventArgs e)
        {
            editorSvc?.CloseDropDown();
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
                editorSvc?.CloseDropDown();
        }
    }
}
