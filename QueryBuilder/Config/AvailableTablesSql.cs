using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Config {
    public static class AvailableTablesSql {
        public static readonly Dictionary<DatabaseType, string> availableTablesSql = new Dictionary<DatabaseType, string>
        {
            {DatabaseType.PostgreSQL,   "SELECT table_name FROM information_schema.table_privileges WHERE grantee = '{0}' AND privilege_type = 'SELECT';"},
            {DatabaseType.Oracle,       "SELECT table_name FROM dba_tab_privs WHERE grantee ='{0}' AND privilege = 'SELECT';"},
            {DatabaseType.SqlServer,    "SELECT table_name FROM sp_table_privileges WHERE grantee = '{0}' AND privilege = 'SELECT';"},
            {DatabaseType.MySql,        "SELECT table_name FROM information_schema.table_privileges WHERE grantee = '{0}' AND privilege_type = 'SELECT';"},
            {DatabaseType.Redshift,     "SELECT table_name FROM information_schema.table_privileges WHERE grantee = '{0}' AND privilege_type = 'SELECT';"},
            {DatabaseType.Sqlite,       "SELECT tbl_name FROM sqlite_master where type ='table' OR type ='view';"},
            {DatabaseType.Access,       "SELECT name FROM MSysObjects WHERE Type = 1 AND Flags = 0"}
        };
    }
}
