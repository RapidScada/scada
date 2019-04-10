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
 * Summary  : Control for displaying SVG images
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Windows.Forms;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Control for displaying SVG images.
    /// <para>Элемент управления для отображения изображений в формате SVG.</para>
    /// </summary>
    public partial class CtrlSvgViewer : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlSvgViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the specified image.
        /// </summary>
        public void ShowImage(byte[] imageData)
        {
            if (imageData == null)
                throw new ArgumentNullException("imageData");

            string base64 = Convert.ToBase64String(imageData);
            string html = string.Format(
                "<html><head><meta http-equiv='X-UA-Compatible' content='IE=Edge'/></head>" + 
                "<body style='margin:0; padding:0'>" +
                "<div style='text-align:center;'>" +
                "<img src='data:image/svg+xml;base64," + base64 +
                "' width={0} height={1} border=0 style='object-fit:cover;' /></div></html></body>", 
                Width, Height);

            webBrowser.DocumentText = html;
            webBrowser.Invalidate();
        }
    }
}
