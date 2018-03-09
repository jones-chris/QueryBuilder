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

namespace QueryBuilderTests.SqlGenerators
{
    public class SqlGeneratorTests
    {

        protected string connString;
        protected IDatabaseConnection dbConnection;
        protected BaseSqlGenerator sqlGenerator;
        protected Query basicQuery;
        protected string table = "county_spending_detail";
        protected List<string> columns = new List<string>() { "service", "fund" };
        protected List<Criteria> multipleCriteria = new List<Criteria>() { };
        protected Criteria criteria1;
        protected Criteria criteria2;
        protected Criteria criteriaWithSubQuery;
        protected Criteria criteriaWithIsNotNull;
        protected long limit = 100;
        protected long offset = 50;
        protected DataTable tableSchema = TestUtil.MultiColumnDataTableBuilder();


        public SqlGeneratorTests(string dbConnString, DatabaseType dbType)
        {
            connString = dbConnString;
            dbConnection = new DatabaseConnectionFactory().CreateDbConnection(dbType, connString);
            sqlGenerator = new SqlGeneratorFactory().CreateSqlGenerator(dbType);
            RunBeforeEachTest();
        }


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


         
        public DataTable CreateSql_SelectFromLimitSuppressNullsTrue()
        {
            basicQuery.SetLimit(100);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count == 100);
        }

        
        public DataTable CreateSql_SelectFromLimitSuppressNullsFalse()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetSuppressNulls(false);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count == 100);
        }

         
        public DataTable CreateSql_SelectFromLimitDistinctTrue()
        {
            basicQuery.SetDistinct(true);
            basicQuery.SetLimit(100);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // offset
         
        public DataTable CreateSql_SelectFromLimitOffset()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetOffset(50);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

         
        public DataTable CreateSql_SelectFromLimitGroupBy()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetGroupBy(true);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

         
        public DataTable CreateSql_SelectFromLimitOrderBy()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetOrderBy(true);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

         
        public DataTable CreateSql_SelectFromLimitGroupByOrderBy()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetGroupBy(true);
            basicQuery.SetOrderBy(true);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

         
        public DataTable CreateSql_SelectFromLimitSingleCriteria()
        {
            basicQuery.SetSuppressNulls(false);
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteria1 });

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

         
        public DataTable CreateSql_SelectFromLimitSingleCriteriaSuppressNulls()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteria1 });

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

         
        public DataTable CreateSql_SelectFromLimitMultipleCriteria()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteria1 });

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // multiple criteria, suppress nulls
         
        public DataTable CreateSql_SelectFromLimitMultipleCriteriaSuppressNulls()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(multipleCriteria);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // subquery
         
        public DataTable CreateSql_SelectFromLimitSingleCriteriaSubQuery()
        {
            basicQuery.SetSuppressNulls(false);
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteriaWithSubQuery });

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // subquery, suppress nulls
         
        public DataTable CreateSql_SelectFromLimitSingleCriteriaSubQuerySuppressNulls()
        {
            basicQuery.SetSuppressNulls(true);
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteriaWithSubQuery });

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // multiple criteria, subquery
         
        public DataTable CreateSql_SelectFromLimitMultipleCriteriaSubQuery()
        {
            basicQuery.SetSuppressNulls(false);
            basicQuery.SetLimit(100);
            multipleCriteria.Add(criteriaWithSubQuery);
            basicQuery.SetCriteria(multipleCriteria);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // subquery, suppress nulls
         
        public DataTable CreateSql_SelectFromLimitMultipleCriteriaSubQuerySuppressNulls()
        {
            basicQuery.SetSuppressNulls(true);
            basicQuery.SetLimit(100);
            multipleCriteria.Add(criteriaWithSubQuery);
            basicQuery.SetCriteria(multipleCriteria);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // is not null
         
        public DataTable CreateSql_SelectFromLimitSingleCriteriaIsNotNull()
        {
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteriaWithIsNotNull });

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // is not null, suppress nulls
         
        public DataTable CreateSql_SelectFromLimitSingleCriteriaIsNotNullSuppressNulls()
        {
            basicQuery.SetSuppressNulls(true);
            basicQuery.SetLimit(100);
            basicQuery.SetCriteria(new List<Criteria>() { criteriaWithIsNotNull });

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

        // multiple criteria, is not null
         
        public DataTable CreateSql_SelectFromLimitMultipleCriteriaIsNotNulls()
        {
            basicQuery.SetSuppressNulls(false);
            basicQuery.SetLimit(100);
            multipleCriteria.Add(criteriaWithIsNotNull);
            basicQuery.SetCriteria(multipleCriteria);

            var sql = sqlGenerator.CreateSql(basicQuery);
            return dbConnection.query(sql);

            //Assert.IsTrue(results.Rows.Count > 0);
        }

    }
}
