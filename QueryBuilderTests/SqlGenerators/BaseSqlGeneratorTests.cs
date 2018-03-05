using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.SqlGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using QueryBuilder.Exceptions;
using QueryBuilder.Config;
using QueryBuilderTests.TestUtilities;

namespace QueryBuilder.SqlGenerators.Tests
{
    [TestClass]
    public class BaseSqlGeneratorTests : BaseSqlGenerator
    {
        public static IList<string> columns = new List<string>() { "fund", "service" };
        public static string table = "county_spending_detail";
        public static Criteria criteria1 = new Criteria();
        public static Criteria criteria2 = new Criteria();
        public static IList<Criteria> multipleCriteria = new List<Criteria>();
        public static BaseSqlGenerator sqlGenerator = MockRepository.GeneratePartialMock<BaseSqlGenerator>();
        
        static BaseSqlGeneratorTests()
        {
            //criteria1.AndOr = Conjunction.And;
            //criteria1.Column = "fund";
            //criteria1.Operator = Operator.EqualTo;
            //criteria1.Filter = "fund1";

            //criteria2.AndOr = Conjunction.And;
            //criteria2.Column = "service";
            //criteria2.Operator = Operator.In;
            //criteria2.Filter = "service1,service2";

            //multipleCriteria.Add(criteria1);
            //multipleCriteria.Add(criteria2);
        }

        public BaseSqlGeneratorTests()
        {
            tableSchema = TestUtil.MultiColumnDataTableBuilder();

            typeMappings.Add("text", true);
            typeMappings.Add("int2", false);
        }

        [TestInitialize]
        public void RunBeforeEachTest()
        {
            base.openingColumnMark = '`';
            base.closingColumnMark = '`';

            criteria1 = new Criteria();
            criteria1.AndOr = Conjunction.And;
            criteria1.Column = "fund";
            criteria1.Operator = Operator.EqualTo;
            criteria1.Filter = "fund1";

            criteria2 = new Criteria();
            criteria2.AndOr = Conjunction.And;
            criteria2.Column = "service";
            criteria2.Operator = Operator.In;
            criteria2.Filter = "service1,service2";

            multipleCriteria.Add(criteria1);
            multipleCriteria.Add(criteria2);
        }

        public override string CreateSql(Query query)
        {
            throw new NotImplementedException();
        }

        //==========================================================================================
        // CreateSELECTClause Tests
        //==========================================================================================

        [TestMethod]
        public void CreateSELECTClause_NotDistinctAndMulitpleColumns()
        {
            var expectedSQL = "SELECT `fund`, `service` ";

            var actualSQL = CreateSELECTClause(false, columns).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateSELECTClause_DistinctAndMulitpleColumns()
        {
            var expectedSQL = "SELECT DISTINCT `fund`, `service` ";

            var actualSQL = CreateSELECTClause(true, columns).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateSELECTClause_NotDistinctAndSingleColumn()
        {
            var column = new List<string>() { "service" };
            var expectedSQL = "SELECT `service` ";

            var actualSQL = CreateSELECTClause(false, column).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateSELECTClause_DistinctAndSingleColumn()
        {
            var column = new List<string>() { "service" };
            var expectedSQL = "SELECT DISTINCT `service` ";

            var actualSQL = CreateSELECTClause(true, column).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSELECTClause_DistinctAndNullColumns()
        {
            var actualSQL = CreateSELECTClause(true, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSELECTClause_NotDistinctAndNullColumns()
        {
            var actualSQL = CreateSELECTClause(false, null);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyCollectionException))]
        public void CreateSELECTClause_DistinctAndEmptyColumns()
        {
            var columns = new List<string>() { };

            var actualSQL = CreateSELECTClause(true, columns);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyCollectionException))]
        public void CreateSELECTClause_NotDistinctAndEmptyColumns()
        {
            var columns = new List<string>() { };

            var actualSQL = CreateSELECTClause(false, columns);
        }

        //==========================================================================================
        // CreateFROMClause Tests
        //==========================================================================================

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFROMClause_NullTable()
        {
            var actualSQL = CreateFROMClause(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFROMClause_EmptyString()
        {
            var table = "";

            var actualSQL = CreateFROMClause(table);
        }

        [TestMethod]
        public void CreateFROMClause_NonEmptyString()
        {
            var table = "county_spending_detail";
            var expectedSQL = " FROM `county_spending_detail` ";

            var actualSQL = CreateFROMClause(table).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        //==========================================================================================
        // CreateWHEREClause Tests
        //==========================================================================================

        [TestMethod]
        public void CreateWHEREClause_NullCriteriaAndColumnsAndDoNotSuppressNulls()
        {
            var actualSQL = CreateWHEREClause(null, null, false);

            Assert.IsNull(actualSQL);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWHEREClause_NullCriteriaAndNoColumnsAndSuppressNulls()
        {
            var actualSQL = CreateWHEREClause(null, null, true);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyCollectionException))]
        public void CreateWHEREClause_NullCriteriaAndEmptyColumnsCollectionAndSuppressNulls()
        {
            var actualSQL = CreateWHEREClause(null, new List<string>(), true);
        }

        [TestMethod]
        public void CreateWHEREClause_NullCriteriaAndOneColumnAndSuppressNulls()
        {
            var expectedSQL = " WHERE (`service` IS NOT NULL ) ";

            var actualSQL = CreateWHEREClause(null, new List<string>() { "service" }, true).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateWHEREClause_NullCriteriaAndMultipleColumnsAndSuppressNulls()
        {
            var expectedSQL = " WHERE (`service` IS NOT NULL OR `fund` IS NOT NULL ) ";

            var actualSQL = CreateWHEREClause(null, new List<string>() { "service", "fund" }, true).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyCollectionException))]
        public void CreateWHEREClause_EmptyCriteriaAndMultipleColumnsAndSuppressNulls()
        {
            var actualSQL = CreateWHEREClause(new List<Criteria>(), new List<string>() { "service", "fund" }, true);
        }

        [TestMethod]
        public void CreateWHEREClause_OneCriteriaAndSingleColumnAndNoSuppressNulls_MakesFirstCriteriaAndOrPropetyNull()
        {
            var actualSQL = CreateWHEREClause(new List<Criteria>() { criteria1 }, new List<string>() { "service" }, false).ToString();

            Assert.IsTrue(criteria1.AndOr == null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateWHEREClause_OneCriteriaAndSingleColumnAndNoSuppressNulls_CriteriaHasNullFilter()
        {
            var nullFilterCriteria = new Criteria();
            nullFilterCriteria.AndOr = Conjunction.And;
            nullFilterCriteria.Column = "service";
            nullFilterCriteria.Operator = Operator.EqualTo;
            nullFilterCriteria.Filter = null;

            var actualSQL = CreateWHEREClause(new List<Criteria>() { nullFilterCriteria }, new List<string>() { "service" }, false);
        }

        [TestMethod]
        public void CreateWHEREClause_OneCriteriaAndSingleColumnAndNoSuppressNulls_OperatorIsNotNull()
        {
            criteria2.Filter = null;
            criteria2.Operator = Operator.IsNotNull;
            var expectedSQL = " WHERE  service Is Not Null  ";

            var actualSQL = CreateWHEREClause(new List<Criteria>() { criteria2 }, new List<string>() { "service" }, false).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateWHEREClause_CriteriaIsSubQueryAndSingleColumnAndNoSuppressNulls()
        {
            criteria1.Filter = "select distinct fund from county_spending_detail";
            criteria1.Operator = Operator.In;
            var expectedSQL = $" WHERE  fund In ({criteria1.Filter}) ";

            var actualSQL = CreateWHEREClause(new List<Criteria>() { criteria1 }, new List<string>() { "service" }, false).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateWHEREClause_CannotFindColumnDataTypeThrowsException()
        {
            var criteria = new Criteria();
            criteria.AndOr = Conjunction.And;
            criteria.Column = "this_column_will_not_be_found";
            criteria.Operator = Operator.GreaterThan;
            criteria.Filter = "a column";
            
            var actualSQL = CreateWHEREClause(new List<Criteria>() { criteria }, null, false);
        }

        [TestMethod]
        public void CreateWHEREClause_ColumnShouldHaveQuotesAndHasInOperator()
        {
            var expectedSQL = " WHERE  service In ('service1','service2') ";

            var actualSQL = CreateWHEREClause(new List<Criteria>() { criteria2 }, null, false).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateWHEREClause_ColumnShouldNotHaveQuotesAndHasInOperator()
        {
            var criteria = new Criteria();
            criteria.AndOr = Conjunction.And;
            criteria.Column = "account";
            criteria.Operator = Operator.In;
            criteria.Filter = "account1,account2";
            var expectedSQL = " WHERE  account In (account1,account2) ";

            var actualSQL = CreateWHEREClause(new List<Criteria>() { criteria }, null, false).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateWHEREClause_ColumnShouldHaveQuotesAndHasEqualToOperator()
        {
            var expectedSQL = " WHERE  fund = 'fund1'  ";

            var actualSQL = CreateWHEREClause(new List<Criteria>() { criteria1 }, null, false).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateWHEREClause_ColumnShouldHaveNotQuotesAndHasEqualToOperator()
        {
            var criteria = new Criteria();
            criteria.AndOr = Conjunction.And;
            criteria.Column = "account";
            criteria.Operator = Operator.EqualTo;
            criteria.Filter = "123";
            var expectedSQL = " WHERE  account = 123  ";

            var actualSQL = CreateWHEREClause(new List<Criteria>() { criteria }, null, false).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        [ExpectedException(typeof(BadSQLException))]
        public void CreateWHEREClause_OneCriteriaAndSingleColumnAndNoSuppressNulls_CriteriaHasNullOperator()
        {
            var nullFilterCriteria = new Criteria();
            nullFilterCriteria.AndOr = Conjunction.And;
            nullFilterCriteria.Column = "service";
            nullFilterCriteria.Operator = null;
            nullFilterCriteria.Filter = "service1";

            var actualSQL = CreateWHEREClause(new List<Criteria>() { nullFilterCriteria }, null, false);
        }

        [TestMethod]
        [ExpectedException(typeof(BadSQLException))]
        public void CreateWHEREClause_OneCriteriaAndSingleColumnAndNoSuppressNulls_CriteriaHasNullColumn()
        {
            var nullFilterCriteria = new Criteria();
            nullFilterCriteria.AndOr = Conjunction.And;
            nullFilterCriteria.Column = null;
            nullFilterCriteria.Operator = Operator.EqualTo;
            nullFilterCriteria.Filter = "service1";

            var actualSQL = CreateWHEREClause(new List<Criteria>() { nullFilterCriteria }, null, false);
        }

        //==========================================================================================
        // CreateGROUPBYClause Tests
        //==========================================================================================

        [TestMethod]
        public void CreateGROUPBYClause_GroupByIsFalse()
        {
            var actualSQL = CreateGROUPBYCluase(false, null);

            Assert.IsNull(actualSQL);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateGROUPBYClause_NullColumns()
        {
            var actualSQL = CreateGROUPBYCluase(true, null);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyCollectionException))]
        public void CreateGROUPBYClause_EmptyColumnsCollection()
        {
            var actualSQL = CreateGROUPBYCluase(true, new List<string>());
        }

        [TestMethod]
        public void CreateGROUPBYClause_OneColumn()
        {
            var expectedSQL = " GROUP BY `service` ";

            var actualSQL = CreateGROUPBYCluase(true, new List<string>() { "service" }).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateGROUPBYClause_MulitpleColumns()
        {
            var expectedSQL = " GROUP BY `service`, `fund` ";

            var actualSQL = CreateGROUPBYCluase(true, new List<string>() { "service", "fund" }).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }


        //==========================================================================================
        // CreateORDERBYClause Tests
        //==========================================================================================

        [TestMethod]
        public void CreateORDERBYClause_OrderByIsFalse()
        {
            var actualSQL = CreateORDERBYCluase(false, null, false);

            Assert.IsNull(actualSQL);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateORDERBYClause_NullColumns()
        {
            var actualSQL = CreateORDERBYCluase(true, null, false);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyCollectionException))]
        public void CreateORDERBYClause_EmptyColumnsCollection()
        {
            var actualSQL = CreateORDERBYCluase(true, new List<string>(), false);
        }

        [TestMethod]
        public void CreateORDERBYClause_OneColumn()
        {
            var expectedSQL = " ORDER BY `service` DESC ";

            var actualSQL = CreateORDERBYCluase(true, new List<string>() { "service" }, false).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateORDERBYClause_MulitpleColumns()
        {
            var expectedSQL = " ORDER BY `service`, `fund` DESC ";

            var actualSQL = CreateORDERBYCluase(true, new List<string>() { "service", "fund" }, false).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateORDERBYClause_MulitpleColumnsWithASC()
        {
            var expectedSQL = " ORDER BY `service`, `fund` ASC ";

            var actualSQL = CreateORDERBYCluase(true, new List<string>() { "service", "fund" }, true).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        //==========================================================================================
        // CreateLIMITClause Tests
        //==========================================================================================

        [TestMethod]
        public void CreateLIMITClause_NullLimit()
        {
            Assert.IsNull(CreateLimitClause(null));
        }

        [TestMethod]
        public void CreateLIMITClause_NonNullLimit()
        {
            var expectedSQL = " LIMIT 100 ";

            var actualSQL = CreateLimitClause(100).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        //==========================================================================================
        // CreateOFFSETClause Tests
        //==========================================================================================

        [TestMethod]
        public void CreateOFFSETClause_NullOffset()
        {
            Assert.IsNull(CreateOffsetClause(null));
        }

        [TestMethod]
        public void CreateOFFSETClause_NonNullOffset()
        {
            var expectedSQL = " OFFSET 100 ";

            var actualSQL = CreateOffsetClause(100).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        //==========================================================================================
        // CreateSuppressNullsClause Tests
        //==========================================================================================

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSuppressNullsClause_NullColumns()
        {
            Assert.IsNull(CreateSuprressNullsClause(null));
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyCollectionException))]
        public void CreateSuppressNullsClause_EmptyColumnsCollection()
        {
            var actualSQL = CreateSuprressNullsClause(new List<string>());
        }

        [TestMethod]
        public void CreateSuppressNullsClause_OneColumn()
        {
            var expectedSQL = " (`service` IS NOT NULL ) ";

            var actualSQL = CreateSuprressNullsClause(new List<string>() { "service" }).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CreateSuppressNullsClause_MulitpleColumns()
        {
            var expectedSQL = " (`service` IS NOT NULL  OR `fund` IS NOT NULL ) ";

            var actualSQL = CreateSuprressNullsClause(new List<string>() { "service", "fund" }).ToString();

            Assert.IsTrue(sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(sqlStringsMatch(expectedSQL, actualSQL));
        }

        /// <summary>
        /// Check that the strings are the same length.
        /// </summary>
        /// <param name="expectedSQL"></param>
        /// <param name="actualSQL"></param>
        /// <returns>boolean</returns>
        private bool sqlStringsAreSameLength(string expectedSQL, string actualSQL)
        {
            return expectedSQL.Length == actualSQL.Length;
        }

        /// <summary>
        /// Check that each string has the same characters at each index.
        /// </summary>
        /// <param name="expectedSQL"></param>
        /// <param name="actualSQL"></param>
        /// <returns>boolean</returns>
        private bool sqlStringsMatch(string expectedSQL, string actualSQL)
        {
            for (int i = 0; i < actualSQL.Length; i++)
            {
                if (actualSQL[i] != expectedSQL[i]) return false;
            }
            return true;
        }
    }
}