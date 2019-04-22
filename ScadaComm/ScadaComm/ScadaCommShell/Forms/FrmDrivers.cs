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
 * Module   : Communicator Shell
 * Summary  : Form for viewing a list of Communicator drivers
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Comm.Devices;
using Scada.Comm.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Comm.Shell.Forms
{
    /// <summary>
    /// Form for viewing a list of Communicator drivers.
    /// <para>Форма просмотра списка драйверов Коммуникатора.</para>
    /// </summary>
    public partial class FrmDrivers : Form, IChildForm
    {
        /// <summary>
        /// List item representing a driver.
        /// <para>Элемент списка, представляющий драйвер.</para>
        /// </summary>
        private class DriverItem
        {
            public bool IsInitialized { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string Descr { get; set; }
            public KPView KPView { get; set; }

            public override string ToString()
            {
                return FileName;
            }
        }


        private readonly CommEnvironment environment; // the application environment


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDrivers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDrivers(CommEnvironment environment)
            : this()
        {
            this.environment = environment ?? throw new ArgumentNullException("environment");
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Fills the list of drivers.
        /// </summary>
        private void FillDriverList()
        {
            try
            {
                lbDrivers.BeginUpdate();
                lbDrivers.Items.Clear();

                foreach (FileInfo fileInfo in environment.GetDrivers())
                {
                    lbDrivers.Items.Add(new DriverItem()
                    {
                        IsInitialized = false,
                        FileName = fileInfo.Name,
                        FilePath = fileInfo.FullName
                    });
                }

                if (lbDrivers.Items.Count > 0)
                    lbDrivers.SelectedIndex = 0;
            }
            finally
            {
                lbDrivers.EndUpdate();
            }
        }

        /// <summary>
        /// Initializes the driver item if needed.
        /// </summary>
        private void InitDriverItem(DriverItem driverItem)
        {
            if (!driverItem.IsInitialized)
            {
                driverItem.IsInitialized = true;

                try
                {
                    KPView kpView = environment.GetKPView(driverItem.FilePath);
                    driverItem.Descr = kpView.KPDescr?.Replace("\n", Environment.NewLine);
                    driverItem.KPView = kpView;
                }
                catch (Exception ex)
                {
                    driverItem.Descr = ex.Message;
                    driverItem.KPView = null;
                }
            }
        }

        /// <summary>
        /// Enables or disables the Properties button.
        /// </summary>
        private void SetButtonEnabled()
        {
            btnProperties.Enabled = lbDrivers.SelectedItem is DriverItem driverItem &&
                driverItem.KPView != null && driverItem.KPView.CanShowProps;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void FrmDrivers_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            FillDriverList();
            SetButtonEnabled();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            if (lbDrivers.SelectedItem is DriverItem driverItem && 
                driverItem.KPView != null && driverItem.KPView.CanShowProps)
            {
                lbDrivers.Focus();
                driverItem.KPView.ShowProps();
            }
        }

        private void lbDrivers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display a driver description
            if (lbDrivers.SelectedItem is DriverItem driverItem)
            {
                InitDriverItem(driverItem);
                SetButtonEnabled();
                txtDescr.Text = driverItem.Descr;
            }
        }

        private void lbDrivers_DoubleClick(object sender, EventArgs e)
        {
            btnProperties_Click(null, null);
        }
    }
}
