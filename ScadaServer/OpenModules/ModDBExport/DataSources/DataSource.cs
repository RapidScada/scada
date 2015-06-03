using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Scada.Server.Module.DBExport
{
    internal abstract class DataSource
    {
        public DBType DBType { get; set; }
        
        public string Server { get; set; }
        
        public string Database { get; set; }
        
        public string User { get; set; }
        
        public string Password { get; set; }
        
        public string ConnectionString { get; set; }


        public DbConnection Connection { get; protected set; }

        public DbCommand ExportCurDataCmd { get; protected set; }

        public DbCommand ExportArcDataCmd { get; protected set; }

        public DbCommand ExportEventCmd { get; protected set; }


        protected abstract void ClearPool();

        protected abstract DbCommand CreateCommand(string cmdText);


        public abstract void InitConnection();

        public abstract void AddCmdParamWithValue(DbCommand cmd, string paramName, object value);

        public virtual void Connect()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection is not inited.");

            try
            {
                Connection.Open();
            }
            catch 
            {
                Connection.Close();
                ClearPool();
                throw;
            }
        }

        public virtual void Disconnect()
        {
            if (Connection != null)
                Connection.Close();
        }

        public virtual void InitCommands(string exportCurDataQuery, string exportArcDataQuery, string exportEventQuery)
        {
            ExportCurDataCmd = string.IsNullOrEmpty(exportCurDataQuery) ? null : CreateCommand(exportCurDataQuery);
            ExportArcDataCmd = string.IsNullOrEmpty(exportArcDataQuery) ? null : CreateCommand(exportArcDataQuery);
            ExportEventCmd = string.IsNullOrEmpty(exportArcDataQuery) ? null : CreateCommand(exportEventQuery);
        }
    }
}
