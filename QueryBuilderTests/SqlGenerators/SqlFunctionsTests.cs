using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.SqlGenerators;
using System.Collections.Generic;
using QueryBuilderTests.TestUtilities;
using QueryBuilder.Config;

namespace QueryBuilder.SqlGenerators.Tests {
    [TestClass]
    public class SqlFunctionsTests {
        public static string column1 = "column1";
        public static string column2 = "column2";
        public static string alias = "my_column";
        public static IList<string> columns;

        [ClassInitialize]
        public static void RunOnceAtClassInitialization(TestContext context) {
            columns = new List<string>();
            columns.Add(column1);
            columns.Add(column2);
        }

        [TestMethod]
        public void SumTest() {
            var expectedSQL = $"SUM({column1}) {alias}";

            var actualSQL = SqlFunctions.Sum(column1, alias);

            Assert.IsTrue(TestUtil.sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(TestUtil.sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void AverageTest() {
            var expectedSQL = $"AVG({column1}) {alias}";

            var actualSQL = SqlFunctions.Average(column1, alias);

            Assert.IsTrue(TestUtil.sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(TestUtil.sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void MinimumTest() {
            var expectedSQL = $"MIN({column1}) {alias}";

            var actualSQL = SqlFunctions.Minimum(column1, alias);

            Assert.IsTrue(TestUtil.sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(TestUtil.sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void MaximumTest() {
            var expectedSQL = $"MAX({column1}) {alias}";

            var actualSQL = SqlFunctions.Maximum(column1, alias);

            Assert.IsTrue(TestUtil.sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(TestUtil.sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void Length_NotSQLServer() {
            var expectedSQL = $"LENGTH({column1}) {alias}";

            var actualSQL = SqlFunctions.Length(column1, alias, DatabaseType.PostgreSQL);

            Assert.IsTrue(TestUtil.sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(TestUtil.sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void Length_SQLServer() {
            var expectedSQL = $"LEN({column1}) {alias}";

            var actualSQL = SqlFunctions.Length(column1, alias, DatabaseType.SqlServer);

            Assert.IsTrue(TestUtil.sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(TestUtil.sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void CountTest() {
            var expectedSQL = $"COUNT({column1}) {alias}";

            var actualSQL = SqlFunctions.Count(column1, alias);

            Assert.IsTrue(TestUtil.sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(TestUtil.sqlStringsMatch(expectedSQL, actualSQL));
        }

        [TestMethod]
        public void ConcatenateTest() {
            var expectedSQL = $"CONCAT({columns[0]},{columns[1]}) {alias}";

            var actualSQL = SqlFunctions.Concatenate(columns, alias);

            Assert.IsTrue(TestUtil.sqlStringsAreSameLength(expectedSQL, actualSQL));
            Assert.IsTrue(TestUtil.sqlStringsMatch(expectedSQL, actualSQL));
        }
    }
}