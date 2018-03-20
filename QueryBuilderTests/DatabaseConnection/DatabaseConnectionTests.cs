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
        public static DatabaseType dbType = DatabaseType.PostgreSQL;
        public static string connString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=budgeto;";
        public static IDatabaseConnection dbConnection = new DatabaseConnectionFactory().CreateDbConnection(dbType, connString);

        [TestMethod]
        public void query_PostgreSQL()
        {
            var sql = "select * from county_spending_detail limit 10;";

            var dt = dbConnection.Query(sql);

            Assert.IsTrue(dt.Rows.Count == 10);
        }

        [TestMethod]
        public void userSignIn_PostgreSQL()
        {
            dbConnection.UserSignIn();

            Assert.IsTrue(true); // This will pass the test if no exception is thrown before this line.
        }

        [TestMethod]
        public void getSchema_PostgreSQL()
        {
            var table = "county_spending_detail";

            var dt = dbConnection.GetColumns(table);

            Assert.IsTrue(dt.Rows.Count > 0);
        }
    }
}