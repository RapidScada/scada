/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : KpDBImport
 * Summary  : Device configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using Scada.Comm.Devices.DbImport.Configuration;
using Scada.Comm.Devices.DbImport.Data;
using Scada.UI;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.DbImport.UI
{
    /// <summary>
    /// Device configuration form.
    /// <para>Форма настройки конфигурации КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private readonly AppDirs appDirs;  // the application directories
        private readonly int kpNum;        // the device number
        private readonly KpConfig config; // the device configuration
        private string configFileName;     // the configuration file name
        private bool modified;             // the configuration was modified
        private bool connChanging;         // connection settings are changing
        private bool cmdSelecting;         // a command is selecting



        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConfig(AppDirs appDirs, int kpNum)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.kpNum = kpNum;
            config = new KpConfig();
            configFileName = "";
            modified = false;
            connChanging = false;
            cmdSelecting = false;
        }

        
        /// <summary>
        /// Gets or sets a value indicating whether the configuration was modified.
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
            }
        }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            connChanging = true;
            cmdSelecting = true;

            // set the control values
            cbDataSourceType.SelectedIndex = (int)config.DataSourceType;
            txtServer.Text = config.DbConnSettings.Server;
            txtDatabase.Text = config.DbConnSettings.Database;
            txtUser.Text = config.DbConnSettings.User;
            txtPassword.Text = config.DbConnSettings.Password;
            txtSelectQuery.Text = config.SelectQuery;
            
            if (config.AutoTagCount)
            {
                numTagCount.Value = 0;
                numTagCount.Enabled = false;
                chkAutoTagCount.Checked = true;
            }
            else
            {
                numTagCount.SetValue(config.TagCount);
                numTagCount.Enabled = true;
                chkAutoTagCount.Checked = false;
            }

            // tune the controls represent the connection properties
            if (config.DataSourceType == DataSourceType.Undefined)
            {
                gbConnection.Enabled = false;
                txtConnectionString.Text = "";
            }
            else
            {
                gbConnection.Enabled = true;
                string connStr = BuildConnectionsString();

                if (string.IsNullOrEmpty(connStr) || !string.IsNullOrEmpty(config.DbConnSettings.ConnectionString))
                {
                    txtConnectionString.Text = config.DbConnSettings.ConnectionString;
                    EnableConnString();
                }
                else
                {
                    txtConnectionString.Text = connStr;
                    EnableConnProps();
                }
            }

            // fill the command list
            cbCommand.Items.AddRange(config.ExportCmds.ToArray());

            if (cbCommand.Items.Count > 0)
                cbCommand.SelectedIndex = 0;

            ShowCommandParams(cbCommand.SelectedItem as ExportCmd);

            connChanging = false;
            cmdSelecting = false;
        }

        /// <summary>
        /// Sets the configuration parameters according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            config.DataSourceType = (DataSourceType)cbDataSourceType.SelectedIndex;
            config.DbConnSettings.Server = txtServer.Text;
            config.DbConnSettings.Database = txtDatabase.Text;
            config.DbConnSettings.User = txtUser.Text;
            config.DbConnSettings.Password = txtPassword.Text;
            config.DbConnSettings.ConnectionString = 
                txtConnectionString.Text == BuildConnectionsString() ? "" : txtConnectionString.Text;
            config.SelectQuery = txtSelectQuery.Text;

            if (chkAutoTagCount.Checked)
            {
                config.AutoTagCount = true;
                config.TagCount = 0;
            }
            else
            {
                config.AutoTagCount = false;
                config.TagCount = Convert.ToInt32(numTagCount.Value);
            }
        }

        /// <summary>
        /// Builds a connection string based on the connection settings.
        /// </summary>
        private string BuildConnectionsString()
        {
            DataSourceType dataSourceType = (DataSourceType)cbDataSourceType.SelectedIndex;

            DbConnSettings connSettings = new DbConnSettings()
            {
                Server = txtServer.Text,
                Database = txtDatabase.Text,
                User = txtUser.Text,
                Password = txtPassword.Text
            };
            
            switch (dataSourceType)
            {
                case DataSourceType.MSSQL:
                    return SqlDataSource.BuildSqlConnectionString(connSettings);
                case DataSourceType.Oracle:
                    return OraDataSource.BuildOraConnectionString(connSettings);
                case DataSourceType.PostgreSQL:
                    return PgSqlDataSource.BuildPgSqlConnectionString(connSettings);
                case DataSourceType.MySQL:
                    return MySqlDataSource.BuildMySqlConnectionString(connSettings);
                case DataSourceType.OLEDB:
                    return OleDbDataSource.BuildOleDbConnectionString(connSettings);
                default:
                    return "";
            }
        }

        /// <summary>
        /// Display the connection controls like enabled.
        /// </summary>
        private void EnableConnProps()
        {
            txtServer.BackColor = txtDatabase.BackColor = txtUser.BackColor = txtPassword.BackColor =
                Color.FromKnownColor(KnownColor.Window);
            txtConnectionString.BackColor = Color.FromKnownColor(KnownColor.Control);
        }

        /// <summary>
        /// Display the connection string like enabled.
        /// </summary>
        private void EnableConnString()
        {
            txtServer.BackColor = txtDatabase.BackColor = txtUser.BackColor = txtPassword.BackColor =
                Color.FromKnownColor(KnownColor.Control);
            txtConnectionString.BackColor = Color.FromKnownColor(KnownColor.Window);
        }

        /// <summary>
        /// Shows the command parameters.
        /// </summary>
        private void ShowCommandParams(ExportCmd exportCmd)
        {
            if (exportCmd == null)
            {
                gbCommandParams.Enabled = false;
                numCmdNum.Value = numCmdNum.Minimum;
                txtName.Text = "";
                txtCmdQuery.Text = "";
            }
            else
            {
                gbCommandParams.Enabled = true;
                numCmdNum.SetValue(exportCmd.CmdNum);
                txtName.Text = exportCmd.Name;
                txtCmdQuery.Text = exportCmd.Query;
            }
        }

        /// <summary>
        /// Sort and update the command list.
        /// </summary>
        private void UpdateCommands()
        {
            try
            {
                cbCommand.BeginUpdate();
                config.ExportCmds.Sort();
                ExportCmd selectedCmd = cbCommand.SelectedItem as ExportCmd;

                cbCommand.Items.Clear();
                cbCommand.Items.AddRange(config.ExportCmds.ToArray());
                cbCommand.SelectedIndex = config.ExportCmds.IndexOf(selectedCmd);
            }
            finally
            {
                cbCommand.EndUpdate();
            }
        }

        /// <summary>
        /// Update text of the selected item.
        /// </summary>
        private void UpdateCommandItem()
        {
            if (cbCommand.SelectedIndex >= 0)
                cbCommand.Items[cbCommand.SelectedIndex] = cbCommand.SelectedItem;
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            // translate the form
            if (Localization.LoadDictionaries(appDirs.LangDir, "KpDbImport", out string errMsg))
                Translator.TranslateForm(this, "Scada.Comm.Devices.DbImport.UI.FrmConfig");
            else
                ScadaUiUtils.ShowError(errMsg);

            Text = string.Format(Text, kpNum);

            // load a configuration
            configFileName = KpConfig.GetFileName(appDirs.ConfigDir, kpNum);

            if (File.Exists(configFileName) && !config.Load(configFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // display the configuration
            ConfigToControls();
            Modified = false;
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(CommPhrases.SaveKpSettingsConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!config.Save(configFileName, out string errMsg))
                        {
                            ScadaUiUtils.ShowError(errMsg);
                            e.Cancel = true;
                        }
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }


        private void control_Changed(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void cbDataSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!connChanging)
            {
                connChanging = true;

                if (cbDataSourceType.SelectedIndex == 0)
                {
                    gbConnection.Enabled = false;
                    txtConnectionString.Text = "";
                }
                else
                {
                    gbConnection.Enabled = true;
                    string connStr = BuildConnectionsString();
                    txtConnectionString.Text = connStr;

                    if (string.IsNullOrEmpty(connStr))
                        EnableConnString();
                    else
                        EnableConnProps();
                }

                Modified = true;
                connChanging = false;
            }
        }

        private void txtConnProp_TextChanged(object sender, EventArgs e)
        {
            if (!connChanging)
            {
                string connStr = BuildConnectionsString();

                if (!string.IsNullOrEmpty(connStr))
                {
                    connChanging = true;
                    txtConnectionString.Text = connStr;
                    EnableConnProps();
                    connChanging = false;
                }

                Modified = true;
            }
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            if (!connChanging)
            {
                EnableConnString();
                Modified = true;
            }
        }

        private void chkAutoTagCount_CheckedChanged(object sender, EventArgs e)
        {
            numTagCount.Enabled = !chkAutoTagCount.Checked;
            Modified = true;
        }

        private void cbCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmdSelecting)
            {
                cmdSelecting = true;
                ShowCommandParams(cbCommand.SelectedItem as ExportCmd);
                cmdSelecting = false;
            }
        }

        private void btnCreateCommand_Click(object sender, EventArgs e)
        {
            // add a new command
            ExportCmd exportCmd = new ExportCmd();

            if (config.ExportCmds.Count > 0)
                exportCmd.CmdNum = config.ExportCmds[config.ExportCmds.Count - 1].CmdNum + 1;

            config.ExportCmds.Add(exportCmd);
            cbCommand.SelectedIndex = cbCommand.Items.Add(exportCmd);
            Modified = true;
        }

        private void btnDeleteCommand_Click(object sender, EventArgs e)
        {
            // delete the selected command
            int selectedIndex = cbCommand.SelectedIndex;

            if (selectedIndex >= 0)
            {
                config.ExportCmds.RemoveAt(selectedIndex);
                cbCommand.Items.RemoveAt(selectedIndex);

                if (cbCommand.Items.Count > 0)
                {
                    cbCommand.SelectedIndex = selectedIndex >= cbCommand.Items.Count ?
                        cbCommand.Items.Count - 1 : selectedIndex;
                }
                else
                {
                    ShowCommandParams(null);
                }

                Modified = true;
            }
        }

        private void numCmdNum_ValueChanged(object sender, EventArgs e)
        {
            if (!cmdSelecting && cbCommand.SelectedItem is ExportCmd exportCmd)
            {
                exportCmd.CmdNum = Convert.ToInt32(numCmdNum.Value);
                UpdateCommands();
                Modified = true;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!cmdSelecting && cbCommand.SelectedItem is ExportCmd exportCmd)
            {
                exportCmd.Name = txtName.Text;
                UpdateCommandItem();
                Modified = true;
            }
        }

        private void txtCmdQuery_TextChanged(object sender, EventArgs e)
        {
            if (!cmdSelecting && cbCommand.SelectedItem is ExportCmd exportCmd)
            {
                exportCmd.Query = txtCmdQuery.Text;
                Modified = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // retrieve the configuration
            ControlsToConfig();

            // save the configuration
            if (config.Save(configFileName, out string errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
