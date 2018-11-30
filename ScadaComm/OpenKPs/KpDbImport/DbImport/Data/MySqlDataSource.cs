using MySql.Data.MySqlClient;
using Scada.Comm.Devices.DbImport.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Comm.Devices.DbImport.Data
{
    internal class MySqlDataSource : DataSource
    {
        /// <summary>
        /// The default port of the database server.
        /// </summary>
        private const int DefaultPort = 3306;

        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public static string BuildConnectionString(DbConnSettings connSettings)
        {
            if (connSettings == null)
                throw new ArgumentNullException("connSettings");

            ExtractHostAndPort(connSettings.Server, DefaultPort, out string host, out int port);

            return new MySqlConnectionStringBuilder()
            {
                Server = host,
                Port = (uint)port,
                Database = connSettings.Database,
                UserID = connSettings.User,
                Password = connSettings.Password
            }
            .ToString();
        }
    }
}
