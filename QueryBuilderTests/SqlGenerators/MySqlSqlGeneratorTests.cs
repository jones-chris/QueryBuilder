using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.SqlGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.DatabaseConnections;
using QueryBuilder.Config;
using System.Data;
using QueryBuilderTests.TestUtilities;
using QueryBuilderTests.SqlGenerators;

namespace QueryBuilder.SqlGenerators.Tests
{
    [TestClass]
    public class MySqlSqlGeneratorTests
    {
        private string connString = "Server=localhost;Database=sys;UID=root;Password=budgeto";
        private DatabaseType dbType = DatabaseType.MySql;

        [TestMethod]
        public void RunAllTests_MySQL()
        {
            var sqlGeneratorTests = new SqlGeneratorTests(connString, dbType);

            var methods = sqlGeneratorTests.GetType().GetMethods();

            DataTable results = null;
            foreach (var method in methods)
            {
                results = null;
                if (method.ReturnType == typeof(DataTable) && method.IsPublic)
                {
                    try
                    {
                        results = (DataTable)method.Invoke(new SqlGeneratorTests(connString, dbType), null);
                        Assert.IsTrue(results.Rows.Count > 0, $"{method.Name} failed.");
                    }
                    catch (Exception ex)
                    {
                        Assert.IsTrue(false, $"{method.Name} failed.  {ex.Message}.  {ex.InnerException}");
                    }

                }
            }
        }
    }
}