using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.SqlGenerators;
using QueryBuilder.Exceptions;

namespace QueryBuilder.Config.Tests
{
    [TestClass]
    public class SqlGeneratorFactoryTests
    {
        [TestMethod]
        public void SqlGeneratorFactory_PostgreSQL()
        {
            var databaseType = DatabaseType.PostgreSQL;
            var expectedSqlGeneratorType = typeof(PgSqlGenerator);

            var sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(databaseType);

            Assert.IsTrue(expectedSqlGeneratorType.Equals(sqlGenerator.GetType()));
        }

        [TestMethod]
        public void SqlGeneratorFactory_MySQL()
        {
            var databaseType = DatabaseType.MySql;
            var expectedSqlGeneratorType = typeof(MySqlSqlGenerator);

            var sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(databaseType);

            Assert.IsTrue(expectedSqlGeneratorType.Equals(sqlGenerator.GetType()));
        }

        [TestMethod]
        public void SqlGeneratorFactory_Oracle()
        {
            var databaseType = DatabaseType.Oracle;
            var expectedSqlGeneratorType = typeof(OracleSqlGenerator);

            var sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(databaseType);

            Assert.IsTrue(expectedSqlGeneratorType.Equals(sqlGenerator.GetType()));
        }

        [TestMethod]
        public void SqlGeneratorFactory_SQLServer()
        {
            var databaseType = DatabaseType.SqlServer;
            var expectedSqlGeneratorType = typeof(SqlServerSqlGenerator);

            var sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(databaseType);

            Assert.IsTrue(expectedSqlGeneratorType.Equals(sqlGenerator.GetType()));
        }

        [TestMethod]
        public void SqlGeneratorFactory_SQLite()
        {
            var databaseType = DatabaseType.Sqlite;
            var expectedSqlGeneratorType = typeof(SqliteSqlGenerator);

            var sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(databaseType);

            Assert.IsTrue(expectedSqlGeneratorType.Equals(sqlGenerator.GetType()));
        }

        [TestMethod]
        public void SqlGeneratorFactory_Redshift()
        {
            var databaseType = DatabaseType.Redshift;
            var expectedSqlGeneratorType = typeof(RedshiftSqlGenerator);

            var sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(databaseType);

            Assert.IsTrue(expectedSqlGeneratorType.Equals(sqlGenerator.GetType()));
        }

        [TestMethod]
        public void SqlGeneratorFactory_Access()
        {
            var databaseType = DatabaseType.Access;
            var expectedSqlGeneratorType = typeof(AccessSqlGenerator);

            var sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(databaseType);

            Assert.IsTrue(expectedSqlGeneratorType.Equals(sqlGenerator.GetType()));
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseTypeNotRecognizedException))]
        public void CreateDbConnection_BadDatabaseType()
        {
            var databaseType = (DatabaseType)15;

            var databaseConnection = new SqlGeneratorFactory().CreateSqlGenerator(databaseType);
        }
    }
}