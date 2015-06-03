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


        protected abstract void InitConnection();

        protected abstract void ClearPool();

        public abstract DbCommand CreateCommand(string cmdText);

        public abstract void AddCmdParamWithValue(DbCommand cmd, string paramName, object value);


        public virtual void Connect()
        {
            InitConnection();

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
            Connection.Close();
        }
    }
}
