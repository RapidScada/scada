using Scada.Comm.Devices.DbImport.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Comm.Devices.DbImport.Data
{
    internal class OraDataSource : DataSource
    {
        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public static string BuildConnectionString(DbConnSettings connSettings)
        {
            if (connSettings == null)
                throw new ArgumentNullException("connSettings");

            return string.Format("Server={0}{1};User ID={2};Password={3}", connSettings.Server, 
                string.IsNullOrEmpty(connSettings.Database) ? "" : "/" + connSettings.Database, 
                connSettings.User, connSettings.Password);
        }
    }
}
