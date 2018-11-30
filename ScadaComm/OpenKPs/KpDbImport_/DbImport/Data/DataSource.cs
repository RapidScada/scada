using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Comm.Devices.DbImport.Data
{
    internal class DataSource
    {
        /// <summary>
        /// Extracts host name and port from the specified server name.
        /// </summary>
        protected static void ExtractHostAndPort(string server, int defaultPort, out string host, out int port)
        {
            int ind = server.IndexOf(':');

            if (ind >= 0)
            {
                host = server.Substring(0, ind);
                if (!int.TryParse(server.Substring(ind + 1), out port))
                    port = defaultPort;
            }
            else
            {
                host = server;
                port = defaultPort;
            }
        }
    }
}
