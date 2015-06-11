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
 * Module   : ModDBExport
 * Summary  : Query configuration control
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// Query configuration control
    /// <para>Элемент управления для конфигурации запроса</para>
    /// </summary>
    public partial class CtrlExportQuery : UserControl
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlExportQuery()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Получить или установить признак выполнения экспорта
        /// </summary>
        public bool Export
        {
            get
            {
                return chkExport.Checked;
            }
            set
            {
                chkExport.Checked = value;
            }
        }

        /// <summary>
        /// Получить или установить SQL-запрос
        /// </summary>
        public string Query
        {
            get
            {
                return txtQuery.Text;
            }
            set
            {
                txtQuery.Text = value;
            }
        }


        /// <summary>
        /// Вызвать событие TriggerChanged
        /// </summary>
        private void OnQueryChanged()
        {
            if (QueryChanged != null)
                QueryChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Событие возникающее при изменении свойств элемента управления
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler QueryChanged;


        private void chkExport_CheckedChanged(object sender, EventArgs e)
        {
            OnQueryChanged();
        }

        private void txtQuery_TextChanged(object sender, EventArgs e)
        {
            OnQueryChanged();
        }
    }
}
