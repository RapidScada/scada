/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : ScadaDataTest
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2014
 */

using System;
using System.Data;
using System.Windows.Forms;
using Scada.Data;

namespace ScadaDataTest
{
    /// <summary>
    /// Main form of the application
    /// </summary>
    public partial class FrmMain : Form
    {
        private DataTable dataTable;

        public FrmMain()
        {
            InitializeComponent();
            dataTable = null;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (rbSrez.Checked)
            {
                dataTable = new DataTable("SrezTable");
                SrezAdapter sa = new SrezAdapter();
                sa.FileName = txtFileName.Text;
                sa.Fill(dataTable);
            }
            else if (rbEvent.Checked)
            {
                dataTable = new DataTable("EventTable");
                EventAdapter ea = new EventAdapter();
                ea.FileName = txtFileName.Text;
                ea.Fill(dataTable);
            }
            else // rbBase.Checked
            {
                dataTable = new DataTable("BaseTable");
                BaseAdapter ba = new BaseAdapter();
                ba.FileName = txtFileName.Text;
                ba.Fill(dataTable, true);
            }

            dataGridView.DataSource = dataTable;
        }

        private void btnFileName_Click(object sender, EventArgs e)
        {
            string fileName = txtFileName.Text.Trim();
            if (fileName != "")
                openFileDialog.FileName = fileName;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtFileName.Text = openFileDialog.FileName;
            txtFileName.Focus();
            txtFileName.DeselectAll();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (dataTable != null)
                dataTable.DefaultView.RowFilter = txtFilter.Text;
        }
    }
}
