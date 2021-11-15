/*
 * Copyright 2021 Elena Shiryaeva
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModDbExport
 * Summary  : Represents a control for editing connection options
 * 
 * Author   : Elena Shiryaeva
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Scada.Config;
using Scada.UI;
using Scada.Db;

namespace Scada.Server.Modules.DbExport.UI
{
    /// <summary>
    /// Represents a control for editing connection options.
    /// <para>Представляет элемент управления для редактирования настроек соединения.</para>
    /// </summary>
    public partial class CtrlConnectionOptions : UserControl
    {
        private DbConnectionOptions connectionOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlConnectionOptions()
        {
            InitializeComponent();
            connectionOptions = null;
        }


        /// <summary>
        /// Gets or sets an editable connection options.
        /// </summary>
        public DbConnectionOptions ConnectionOptions
        {
            get
            {
                return connectionOptions;
            }
            set
            {
                if (value != null)
                {
                    connectionOptions = null;
                    txtDBMS.Text = value.DBMS;
                    txtDataBase.Text = value.Database;
                    txtServer.Text = value.Server;
                    txtUser.Text = value.Username;
                    txtUserPwd.Text = value.Password;
                    txtConnectionString.Text = value.ConnectionString;

                    if (value.ConnectionString != "")
                    {
                        chkConnectionString.Checked = true;
                        txtDataBase.Enabled = false;
                        txtServer.Enabled = false;
                        txtUser.Enabled = false;
                        txtUserPwd.Enabled = false;
                        txtConnectionString.Enabled = true;
                    }
                    else
                    {
                        chkConnectionString.Checked = false;
                        txtDBMS.Enabled = true;
                        txtDataBase.Enabled = true;
                        txtServer.Enabled = true;
                        txtUser.Enabled = true;
                        txtUserPwd.Enabled = true;
                        txtConnectionString.Enabled = false;

                        SetConnectionString(value);
                    }
                }

                connectionOptions = value;
            }
        }

        /// <summary>
        /// Triggers an event ConnectChanged.
        /// </summary>
        private void OnConnectChanged()
        {
            ConnectChanged?.Invoke(this, new ObjectChangedEventArgs(connectionOptions));
        }

        /// <summary>
        /// An event that occurs when the properties of an edited connect options change.
        /// </summary>
        [Category("Property Changed")]
        public event ObjectChangedEventHandler ConnectChanged;

        /// <summary>
        /// Set input focus.
        /// </summary>
        public void SetFocus()
        {
            txtDataBase.Select();
        }

        /// <summary>
        /// Set and show an auto-generated connection string. 
        /// </summary>
        private void SetConnectionString(DbConnectionOptions dbConnectionOptions)
        {
            if (dbConnectionOptions != null)
            {
                if (dbConnectionOptions.KnownDBMS == KnownDBMS.Oracle)
                    txtConnectionString.Text = OraDataSource.BuildOraConnectionString(dbConnectionOptions, true);
                else if (dbConnectionOptions.KnownDBMS == KnownDBMS.PostgreSQL)
                    txtConnectionString.Text = PgSqlDataSource.BuildPgSqlConnectionString(dbConnectionOptions, true);
                else if (dbConnectionOptions.KnownDBMS == KnownDBMS.MySQL)
                    txtConnectionString.Text = MySqlDataSource.BuildMySqlConnectionString(dbConnectionOptions, true);
                else if (dbConnectionOptions.KnownDBMS == KnownDBMS.MSSQL)
                    txtConnectionString.Text = SqlDataSource.BuildSqlConnectionString(dbConnectionOptions, true);
                else
                    txtConnectionString.Text = "";
            }
        }


        private void txtDBMS_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.DBMS = txtDBMS.Text;
                OnConnectChanged();
            }
        }

        private void txtDataBase_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Database = txtDataBase.Text;

                if (txtDataBase.Text != "")
                {
                    SetConnectionString(connectionOptions);
                    connectionOptions.ConnectionString = "";
                }
                OnConnectChanged();
            }
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Server = txtServer.Text;

                if (txtServer.Text != "")
                {
                    SetConnectionString(connectionOptions);
                    connectionOptions.ConnectionString = "";
                }
                
                OnConnectChanged();
            }
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Username = txtUser.Text;

                if (txtUser.Text != "")
                {
                    SetConnectionString(connectionOptions);
                    connectionOptions.ConnectionString = "";
                }
                
                OnConnectChanged();
            }
        }

        private void txtUserPwd_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Password = txtUserPwd.Text;

                if (txtUserPwd.Text != "")
                {
                    SetConnectionString(connectionOptions);
                    connectionOptions.ConnectionString = "";
                }
                
                OnConnectChanged();
            }
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.ConnectionString = txtConnectionString.Text;
                OnConnectChanged();
            }
        }

        private void chkConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                txtDataBase.Enabled = !chkConnectionString.Checked;
                txtServer.Enabled = !chkConnectionString.Checked;
                txtUser.Enabled = !chkConnectionString.Checked;
                txtUserPwd.Enabled = !chkConnectionString.Checked;
                txtConnectionString.Enabled = chkConnectionString.Checked;

                if (chkConnectionString.Checked)
                {
                    SetConnectionString(connectionOptions);
                    connectionOptions.ConnectionString = txtConnectionString.Text;
                    txtDataBase.Text = "";
                    txtServer.Text = "";
                    txtUser.Text = "";
                    txtUserPwd.Text = "";
                    txtConnectionString.Select();
                }
                else
                {
                    connectionOptions.ConnectionString = "";
                }

                OnConnectChanged();
            }
        }
    }
}
