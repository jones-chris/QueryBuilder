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
        public static readonly string connString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=budgeto;";
        public static readonly IDatabaseConnection dbConnection = new DatabaseConnectionFactory().CreateDbConnection(DatabaseType.PostgreSQL, connString);
        public static readonly BaseSqlGenerator sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(DatabaseType.PostgreSQL);
        public static Query basicQuery;
        public static string table = "county_spending_detail";
        public static List<string> columns = new List<string>() { "service", "fund" };
        public static List<Criteria> multipleCriteria = new List<Criteria>() { };
        public static Criteria criteria1;
        public static Criteria criteria2;
        public static Criteria criteriaWithSubQuery;
        public static Criteria criteriaWithIsNotNull;
        public static long limit = 100;
        public static long offset = 50;
        public static DataTable tableSchema = TestUtil.MultiColumnDataTableBuilder();

        [TestInitialize]
        public void RunBeforeEachTest()
        {
            basicQuery = new Query("main")
                        .SetTableSchema(tableSchema)
                        .SetDistinct(false)
                        .SetColumns(columns)
                        .SetTable(table);

            // set up criteria1
            criteria1 = new Criteria();
            criteria1.AndOr = Conjunction.And;
            criteria1.Column = "fund";
            criteria1.Operator = Operator.EqualTo;
            criteria1.Filter = "Permitting";

            // set up criteria2
            criteria2 = new Criteria();
            criteria2.AndOr = Conjunction.And;
            criteria2.Column = "service";
            criteria2.Operator = Operator.In;
            criteria2.Filter = "Housing and Community Development";

            // set up criteriaWithSubQuery
            criteriaWithSubQuery = new Criteria();
            criteriaWithSubQuery.AndOr = Conjunction.And;
            criteriaWithSubQuery.Column = "fund";
            criteriaWithSubQuery.Operator = Operator.In;
            criteriaWithSubQuery.Filter = "select distinct fund from county_spending_detail limit 10";

            // set up criteriaWithIsNotNull
            criteriaWithIsNotNull = new Criteria();
            criteriaWithIsNotNull.AndOr = Conjunction.And;
            criteriaWithIsNotNull.Column = "fund";
            criteriaWithIsNotNull.Operator = Operator.IsNotNull;

            // set up multipleCriteria
            multipleCriteria = new List<Criteria>();
            multipleCriteria.Add(criteria1);
            multipleCriteria.Add(criteria2);
        }
        

        [TestMethod]
        public void CreateSql_SelectFromLimitSuppressNullsTrue()
        {
            basicQuery.SetLimit(100);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count == 100);
        }

        [TestMethod]
        public void CreateSql_SelectFromLimitSuppressNullsFalse()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetSuppressNulls(false);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count == 100);
        }

        [TestMethod]
        public void CreateSql_SelectFromLimitDistinctTrue()
        {
            basicQuery.SetDistinct(true);
            basicQuery.SetLimit(100);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // offset
        [TestMethod]
        public void CreateSql_SelectFromLimitOffset()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetOffset(50);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        [TestMethod]
        public void CreateSql_SelectFromLimitGroupBy()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetGroupBy(true);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        [TestMethod]
        public void CreateSql_SelectFromLimitOrderBy()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetOrderBy(true);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        [TestMethod]
        public void CreateSql_SelectFromLimitGroupByOrderBy()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetGroupBy(true);
            basicQuery.SetOrderBy(true);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        [TestMethod]
        public void CreateSql_SelectFromLimitSingleCriteria()
        {
            basicQuery.SetSuppressNulls(false);
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteria1 });

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        [TestMethod]
        public void CreateSql_SelectFromLimitSingleCriteriaSuppressNulls()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteria1 });

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        [TestMethod]
        public void CreateSql_SelectFromLimitMultipleCriteria()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteria1 });

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // multiple criteria, suppress nulls
        [TestMethod]
        public void CreateSql_SelectFromLimitMultipleCriteriaSuppressNulls()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(multipleCriteria);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // subquery
        [TestMethod]
        public void CreateSql_SelectFromLimitSingleCriteriaSubQuery()
        {
            basicQuery.SetSuppressNulls(false);
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteriaWithSubQuery });

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // subquery, suppress nulls
        [TestMethod]
        public void CreateSql_SelectFromLimitSingleCriteriaSubQuerySuppressNulls()
        {
            basicQuery.SetSuppressNulls(true);
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteriaWithSubQuery });

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // multiple criteria, subquery
        [TestMethod]
        public void CreateSql_SelectFromLimitMultipleCriteriaSubQuery()
        {
            basicQuery.SetSuppressNulls(false);
            basicQuery.SetLimit(100);
            multipleCriteria.Add(criteriaWithSubQuery);
            basicQuery.SetCriteria(multipleCriteria);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // subquery, suppress nulls
        [TestMethod]
        public void CreateSql_SelectFromLimitMultipleCriteriaSubQuerySuppressNulls()
        {
            basicQuery.SetSuppressNulls(true);
            basicQuery.SetLimit(100);
            multipleCriteria.Add(criteriaWithSubQuery);
            basicQuery.SetCriteria(multipleCriteria);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // is not null
        [TestMethod]
        public void CreateSql_SelectFromLimitSingleCriteriaIsNotNull()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteriaWithIsNotNull });

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // is not null, suppress nulls
        [TestMethod]
        public void CreateSql_SelectFromLimitSingleCriteriaIsNotNullSuppressNulls()
        {
            basicQuery.SetSuppressNulls(true);
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteriaWithIsNotNull });

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

        // multiple criteria, is not null
        [TestMethod]
        public void CreateSql_SelectFromLimitMultipleCriteriaIsNotNulls()
        {
            basicQuery.SetSuppressNulls(false);
            basicQuery.SetLimit(100);
            multipleCriteria.Add(criteriaWithIsNotNull);
            basicQuery.SetCriteria(multipleCriteria);

            var sql = sqlGenerator.CreateSql(basicQuery);
            var results = dbConnection.query(sql);

            Assert.IsTrue(results.Rows.Count > 0);
        }

    }
}