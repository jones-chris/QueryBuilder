using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using QueryBuilder.Exceptions;

namespace QueryBuilder.Config.Tests
{
    [TestClass]
    public class DatabaseConnectionFactoryTests
    {
        [TestMethod]
        public void DatabaseConnectionFactory_GoodDatabaseTypeAndConnectionString()
        {
            var databaseType = DatabaseType.PostgreSQL;
            var connString = "User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;Pooling = true;";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);

            Assert.IsTrue(databaseConnection != null);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseTypeNotRecognizedException))]
        public void CreateDbConnection_BadDatabaseType()
        {
            var databaseType = (DatabaseType)15;
            var connString = "User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;Pooling = true;";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDbConnection_BadConnectionString()
        {
            var databaseType = DatabaseType.PostgreSQL;
            var connString = "this connection string will fail!";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);
        }
    }
}