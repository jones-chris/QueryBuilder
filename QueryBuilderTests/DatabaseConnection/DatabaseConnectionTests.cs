using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.DatabaseConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Config;
using System.Data;

namespace QueryBuilder.DatabaseConnections.Tests
{
    [TestClass]
    public class DatabaseConnectionTests
    {
        public static DatabaseType dbType = DatabaseType.Access;
        public static string connString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=C:\\novena\\db_testing\\novena_dev.accdb;";
        public IDatabaseConnection dbAccess = new DatabaseConnectionFactory().CreateDbConnection(dbType, connString);

        [TestMethod]
        public void query_Access()
        {
            var sql = "select top 10 * from cash_contributions_detail;";

            var dt = dbAccess.query(sql);

            Assert.IsTrue(dt.Rows.Count == 10);
        }

        [TestMethod]
        public void userSignInTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void getSchemaTest()
        {
            Assert.Fail();
        }
    }
}