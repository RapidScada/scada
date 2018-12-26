/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : Device properties form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Comm.Devices.DbImport.Configuration;
using Scada.Comm.Devices.DbImport.Data;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Comm.Devices.DbImport.UI
{
    /// <summary>
    /// Device properties form.
    /// <para>Форма настройки свойств КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private AppDirs appDirs;       // the application directories
        private int kpNum;             // the device number
        private string configFileName; // the configuration file name
        private Config config;         // the device configuration
        private bool connChanging;     // connection settings are changing



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
            configFileName = "";
            config = new Config();
            connChanging = false;
        }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            connChanging = true;

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

            connChanging = false;
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


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            // translate the form
            if (Localization.LoadDictionaries(appDirs.LangDir, "KpDbImport", out string errMsg))
                Translator.TranslateForm(this, "Scada.Comm.Devices.DbImport.UI.FrmConfig");
            else
                ScadaUiUtils.ShowError(errMsg);

            Text = string.Format(Text, kpNum);

            // load configuration
            configFileName = Config.GetFileName(appDirs.ConfigDir, kpNum);

            if (File.Exists(configFileName) && !config.Load(configFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // display the configuration
            ConfigToControls();
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
            }
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            if (!connChanging)
            {
                EnableConnString();
            }
        }

        private void chkAutoTagCount_CheckedChanged(object sender, EventArgs e)
        {
            numTagCount.Enabled = !chkAutoTagCount.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // retrieve the configuration
            ControlsToConfig();

            // save the configuration
            if (config.Save(configFileName, out string errMsg))
                DialogResult = DialogResult.OK;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
