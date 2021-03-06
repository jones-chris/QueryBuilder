﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using System.Reflection;

namespace QueryBuilder.SqlGenerators.Tests
{
    /// <summary>
    /// This class only has one method, CreateSql.  Because the individual methods called in CreateSql have been tested
    /// in BaseSqlGeneratorTests, this test class' purpose is to test that all the individual methods combine to make
    /// SQL statements that are successfully run against a database.
    /// </summary>

    [TestClass]
    public class PgSqlGeneratorTests
    {
        private string connString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=budgeto;";
        private DatabaseType dbType = DatabaseType.PostgreSQL;

        [TestMethod]
        public void RunAllTests_PostgreSQL()
        {
            var sqlGeneratorTests = new SqlGeneratorTests(connString, dbType);

            var methods = sqlGeneratorTests.GetType().GetMethods();

            foreach (var method in methods)
            {
                if (method.ReturnType == typeof(DataTable) && method.IsPublic)
                {
                    var results = (DataTable)method.Invoke(new SqlGeneratorTests(connString, dbType), null);
                    Assert.IsTrue(results.Rows.Count > 0, $"{method.Name} failed.");
                }
            }

        }

    }
}