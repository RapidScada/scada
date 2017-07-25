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
 * Summary  : Editor of image output conditions for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using Scada.Scheme.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Editor of image output conditions for PropertyGrid
    /// Редактор условий вывода изображений для PropertyGrid
    /// </summary>
    public class ConditionEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorSvc = provider == null ? null :
                (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (context != null && context.Instance is BaseComponent && editorSvc != null)
            {
                List<Condition> conditions = (List<Condition>)value;
                BaseComponent component = (BaseComponent)context.Instance;
                editorSvc.ShowDialog(new FrmConditionDialog(conditions, component));
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return context != null && context.Instance is BaseComponent ?
                UITypeEditorEditStyle.Modal : UITypeEditorEditStyle.None;
        }
    }
}
