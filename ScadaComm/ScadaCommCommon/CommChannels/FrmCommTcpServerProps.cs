/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ScadaCommCommon
 * Summary  : TCP server communication channel properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// TCP server communication channel properties
    /// <para>Свойства канала связи TCP-сервер</para>
    /// </summary>
    internal partial class FrmCommTcpServerProps : Form
    {
        private SortedList<string, string> commCnlParams; // параметры канала связи
        private bool modified;                            // признак изменения параметров
        private CommTcpServerLogic.Settings settings;     // настройки канала связи

        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmCommTcpServerProps()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(SortedList<string, string> commCnlParams, out bool modified)
        {
            if (commCnlParams == null)
                throw new ArgumentNullException("commCnlParams");

            FrmCommTcpServerProps form = new FrmCommTcpServerProps();
            form.commCnlParams = commCnlParams;
            form.modified = false;
            form.ShowDialog();
            modified = form.modified;
        }


        private void FrmCommTcpServerProps_Load(object sender, EventArgs e)
        {
            // инициализация настроек канала связи
            settings = new CommTcpServerLogic.Settings();
            settings.Init(commCnlParams, false);

            // установка элементов управления в соответствии с параметрами канала связи
            cbBehavior.Text = settings.Behavior.ToString();

            modified = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
