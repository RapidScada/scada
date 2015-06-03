using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Scada.Server.Module.DBExport
{
    internal class SqlDataSource : DataSource
    {
        private SqlConnection sqlConn;


        protected override void ClearPool()
        {
            if (sqlConn != null)
                SqlConnection.ClearPool(sqlConn);
        }

        protected override DbCommand CreateCommand(string sql)
        {
            return new SqlCommand(sql, sqlConn);
        }

        public override void InitConnection()
        {
            Connection = sqlConn = new SqlConnection(ConnectionString);
        }

        public override void AddCmdParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd is SqlCommand)
            {
                SqlCommand sqlCmd = (SqlCommand)cmd;
                sqlCmd.Parameters.AddWithValue(paramName, value);
            }
            else
            {
                throw new InvalidCastException("SqlCommand is required.");
            }
        }
    }
}
