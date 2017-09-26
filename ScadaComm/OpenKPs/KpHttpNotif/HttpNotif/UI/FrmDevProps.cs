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
 * Module   : KpHttpNotif
 * Summary  : Device properties form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Comm.Devices.AB;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Comm.Devices.HttpNotif.UI
{
    /// <summary>
    /// Device properties form
    /// <para>Форма настройки свойств КП</para>
    /// </summary>
    public partial class FrmDevProps : Form
    {
        private int kpNum;                   // номер КП
        private KPView.KPProperties kpProps; // свойства КП, сохраняемые SCADA-Коммуникатором
        private AppDirs appDirs;             // директории приложения


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmDevProps()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(int kpNum, KPView.KPProperties kpProps, AppDirs appDirs)
        {
            if (appDirs == null)
                throw new ArgumentNullException("appDirs");

            FrmDevProps frmDevProps = new FrmDevProps();
            frmDevProps.kpNum = kpNum;
            frmDevProps.kpProps = kpProps;
            frmDevProps.appDirs = appDirs;
            frmDevProps.ShowDialog();
        }


        private void FrmDevProps_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Devices.HttpNotif.UI.FrmDevProps");

            // вывод заголовка
            Text = string.Format(Text, kpNum);

            // установка адреса запроса в соответствии с командной строкой КП
            txtReqUrl.Text = kpProps.CmdLine;
            kpProps.Modified = false;
        }

        private void txtReqUrl_TextChanged(object sender, EventArgs e)
        {
            kpProps.Modified = true;
        }

        private void btnAddressBook_Click(object sender, EventArgs e)
        {
            // отображение адресной книги
            FrmAddressBook.ShowDialog(appDirs);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // установка командной строки КП в соответствии с адресом запроса
            if (kpProps.Modified)
                kpProps.CmdLine = txtReqUrl.Text;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            kpProps.Modified = false;
        }
    }
}
