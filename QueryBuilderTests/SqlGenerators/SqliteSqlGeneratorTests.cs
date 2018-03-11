using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.SqlGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Config;
using System.Data;
using QueryBuilderTests.SqlGenerators;

namespace QueryBuilder.SqlGenerators.Tests
{
    [TestClass]
    public class SqliteSqlGeneratorTests
    {
        private string connString = @"Data Source=C:\novena\db_setup\novena-dev.db;Version=3;";
        private DatabaseType dbType = DatabaseType.Sqlite;

        [TestMethod]
        public void RunAllTests_SQLite()
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