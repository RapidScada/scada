using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Data.Tables
{
    public class TableRelation
    {
        public TableRelation(IBaseTable parentTable, IBaseTable childTable, string childColumn, TableIndex childIndex)
        {
            ParentTable = parentTable ?? throw new ArgumentNullException("parentTable");
            ChildTable = childTable ?? throw new ArgumentNullException("childTable");
            ChildColumn = childColumn ?? throw new ArgumentNullException("childColumn");
            ChildIndex = childIndex ?? throw new ArgumentNullException("childIndex");
        }

        public IBaseTable ParentTable { get; protected set; }
        public IBaseTable ChildTable { get; protected set; }
        public string ChildColumn { get; protected set; }
        public TableIndex ChildIndex { get; protected set; }
    }
}
