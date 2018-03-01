using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.SqlGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using QueryBuilder.Exceptions;

namespace QueryBuilder.SqlGenerators.Tests
{
    [TestClass]
    public class BaseSqlGeneratorTests : BaseSqlGenerator
    {
        public static IList<string> columns = new List<string>() { "fund", "service" };
        public static string table = "county_spending_detail";
        public static BaseSqlGenerator sqlGenerator = MockRepository.GeneratePartialMock<BaseSqlGenerator>();
        
        [TestInitialize]
        public void RunBeforeEachTest()
        {
            base.openingColumnMark = '`';
            base.closingColumnMark = '`';
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



        //==========================================================================================
        // CreateGROUPBYClause Tests
        //==========================================================================================


        //==========================================================================================
        // CreateORDERBYClause Tests
        //==========================================================================================


        //==========================================================================================
        // CreateLIMITClause Tests
        //==========================================================================================

        //==========================================================================================
        // CreateOFFSETClause Tests
        //==========================================================================================

        //==========================================================================================
        // CreateSuppressNullsClause Tests
        //==========================================================================================

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