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
 * Summary  : Editor of images for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Editor of images for PropertyGrid
    /// <para>Редактор изображений для PropertyGrid</para>
    /// </summary>
    public class ImageEditor : UITypeEditor
    {
        /// <summary>
        /// Директория, из которой открывались изображения
        /// </summary>
        public static string ImageDir = "";


        private SchemeDocument GetSchemeDoc(object instance)
        {
            SchemeDocument schemeDoc = null;

            if (instance is ISchemeDocAvailable)
            {
                schemeDoc = ((ISchemeDocAvailable)instance).SchemeDoc;
            }
            else if (instance is ICollection)
            {
                foreach (object obj in (ICollection)instance)
                {
                    schemeDoc = GetSchemeDoc(obj);
                    if (schemeDoc == null)
                        break;
                }
            }

            return schemeDoc;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorSvc = provider == null ? null :
                (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (context != null && context.Instance != null && editorSvc != null)
            {
                SchemeDocument schemeDoc = GetSchemeDoc(context.Instance);

                if (schemeDoc != null)
                {
                    Type propType = context.PropertyDescriptor.PropertyType;

                    if (propType == typeof(Dictionary<string, Image>))
                    {
                        // редактирование словаря изображений
                        Dictionary<string, Image> images = (Dictionary<string, Image>)value;
                        FrmImageDialog frmImageDialog = new FrmImageDialog(images, schemeDoc) { ImageDir = ImageDir };
                        editorSvc.ShowDialog(frmImageDialog);
                        ImageDir = frmImageDialog.ImageDir;
                    }
                    else if (propType == typeof(string))
                    {
                        // выбор изображения
                        string imageName = (value ?? "").ToString();
                        FrmImageDialog frmImageDialog = 
                            new FrmImageDialog(imageName, schemeDoc.Images, schemeDoc) { ImageDir = ImageDir };

                        if (editorSvc.ShowDialog(frmImageDialog) == DialogResult.OK)
                        {
                            value = frmImageDialog.SelectedImageName;
                            ImageDir = frmImageDialog.ImageDir;
                        }
                    }
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
