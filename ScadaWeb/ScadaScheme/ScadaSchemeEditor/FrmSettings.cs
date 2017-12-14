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
 * Module   : Scheme Editor
 * Summary  : Application settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;
using System.Windows.Forms;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Application settings form
    /// <para>Форма настройки приложения</para>
    /// </summary>
    internal partial class FrmSettings : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmSettings()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static bool ShowDialog(Settings settings)
        {
            return true;
        }


        private void FrmSettings_Load(object sender, EventArgs e)
        {

        }

        private void FrmSettings_Shown(object sender, EventArgs e)
        {

        }

        private void btnWebDir_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
