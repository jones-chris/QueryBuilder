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
    public class OracleSqlGeneratorTests
    {
        private string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));User Id=SYSTEM;Password=budgeto;";
        private DatabaseType dbType = DatabaseType.Oracle;

        [TestMethod]
        public void RunAllTests_Oracle()
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