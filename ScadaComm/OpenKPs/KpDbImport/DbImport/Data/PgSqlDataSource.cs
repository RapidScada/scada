using Scada.Comm.Devices.DbImport.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Comm.Devices.DbImport.Data
{
    internal class PgSqlDataSource : DataSource
    {
        /// <summary>
        /// The default port of the database server.
        /// </summary>
        private const int DefaultPort = 5432;

        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public static string BuildConnectionString(DbConnSettings connSettings)
        {
            if (connSettings == null)
                throw new ArgumentNullException("connSettings");

            ExtractHostAndPort(connSettings.Server, DefaultPort, out string host, out int port);
            return string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4}",
                host, port, connSettings.Database, connSettings.User, connSettings.Password);
        }
    }
}
