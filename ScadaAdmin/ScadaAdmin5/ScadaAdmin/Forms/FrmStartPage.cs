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
 * Summary  : Represents a start page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Represents a start page.
    /// <para>Представляет стартовую страницу.</para>
    /// </summary>
    public partial class FrmStartPage : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmStartPage()
        {
            InitializeComponent();
        }

        private void FrmStartPage_Load(object sender, EventArgs e)
        {

        }

        private void FrmStartPage_Resize(object sender, EventArgs e)
        {
            pnlContent.Height = Height;
            pnlContent.Left = Math.Max(0, (Width - pnlContent.Width) / 2);
        }
    }
}
