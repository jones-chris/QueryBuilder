using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QueryBuilder.DatabaseConnections
{
    public interface IDatabaseConnection
    {
        DataTable Query(string sql);
        void UserSignIn();
        DataTable GetDatabaseSchemas();
        DataTable GetSchemaTables(string schemaName);
        DataTable GetSchemaViews(string schemaName);
        DataTable GetSchemaTablesAndViews(string schemaName);
        DataTable GetUserTablesAndViews(string username);
        DataTable GetColumns(string tableName);
        
    }
}
