using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.SqlGenerators;
using QueryBuilder.Config;
using QueryBuilder.Exceptions;

namespace QueryBuilder.Config
{
    public class SqlGeneratorFactory
    {
        public SqlGeneratorFactory() { }

        public BaseSqlGenerator CreateSqlGenerator(DatabaseType databaseType)
        {
            try
            {
                if (databaseType.Equals(DatabaseType.PostgreSQL))
                {
                    return new PgSqlGenerator();
                }
                else if (databaseType.Equals(DatabaseType.MySql))
                {
                    return new MySqlSqlGenerator();
                }
                else if (databaseType.Equals(DatabaseType.Oracle))
                {
                    return new OracleSqlGenerator();
                }
                else if (databaseType.Equals(DatabaseType.SqlServer))
                {
                    return new SqlServerSqlGenerator();
                } 
                else if (databaseType.Equals(DatabaseType.Sqlite)) 
                {
                    return new SqliteSqlGenerator();
                } 
                else if (databaseType.Equals(DatabaseType.Redshift))
                {
                    return new RedshiftSqlGenerator();
                }
                else if (databaseType.Equals(DatabaseType.Access))
                {
                    return new AccessSqlGenerator();
                }
                else
                {
                    //Thrown if database type setting is not null, but is not an integer that does not have a corresponding DatabaseType enum value.
                    //For example, a database type setting of 20 does not return an enum value in DatabaseType, so this exception would be thrown.
                    throw new DatabaseTypeNotRecognizedException($"Database type, {databaseType}, is not recognized");
                }
            }
            catch (Exception)
            {
                //Thrown if database type setting is null.
                //For example, a database type setting of null results in the databaseType field not being assigned, so this exception would be thrown.
                throw;
            }
        }
    }
}
