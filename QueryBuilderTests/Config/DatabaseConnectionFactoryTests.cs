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
    [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    public class DatabaseConnectionFactoryTests
    {
        [TestMethod]
        public void DatabaseConnectionFactory_PostgreSQL()
        {
            var databaseType = DatabaseType.PostgreSQL;
            var connString = "User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;Pooling = true;";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);

            Assert.IsTrue(databaseConnection != null);
        }

        [TestMethod]
        public void DatabaseConnectionFactory_Access()
        {
            var databaseType = DatabaseType.Access;
            var connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\mydatabase.accdb;User Id=admin;";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);

            Assert.IsTrue(databaseConnection != null);
        }

        [TestMethod]
        public void DatabaseConnectionFactory_MySQL()
        {
            var databaseType = DatabaseType.MySql;
            var connString = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);

            Assert.IsTrue(databaseConnection != null);
        }

        [TestMethod]
        public void DatabaseConnectionFactory_Oracle()
        {
            var databaseType = DatabaseType.Oracle;
            var connString = "Data Source=urOracle;User Id=urUsername;Password=urPassword;DBA Privilege=SYSDBA;";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);

            Assert.IsTrue(databaseConnection != null);
        }

        [TestMethod]
        public void DatabaseConnectionFactory_Redshift()
        {
            var databaseType = DatabaseType.Redshift;
            var connString = "Server=server; Database=database;UID=user;PWD=password;Port=port;SSL=true;Sslmode=Require";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);

            Assert.IsTrue(databaseConnection != null);
        }

        [TestMethod]
        public void DatabaseConnectionFactory_SQLite()
        {
            var databaseType = DatabaseType.Sqlite;
            var connString = "Data Source=c:\\mydb.db;Version=3;";

            var databaseConnection = new DatabaseConnectionFactory().CreateDbConnection(databaseType, connString);

            Assert.IsTrue(databaseConnection != null);
        }

        [TestMethod]
        public void DatabaseConnectionFactory_SQLServer()
        {
            var databaseType = DatabaseType.SqlServer;
            var connString = "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;";

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