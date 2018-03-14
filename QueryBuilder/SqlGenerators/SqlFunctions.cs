using System.Collections.Generic;
using QueryBuilder.Config;

namespace QueryBuilder.SqlGenerators {
    public static class SqlFunctions {

        /// <summary>
        /// Generates a SUM function string to be used in a SQL statement
        /// </summary>
        /// <param name="column">The name of the column to pass into the SQL SUM function</param>
        /// <param name="alias">The name of the result column</param>
        /// <returns>string</returns>
        public static string Sum(string column, string alias) {
            return $"SUM({column}) {alias}";
        }

        /// <summary>
        /// Generates a AVG function string to be used in a SQL statement
        /// </summary>
        /// <param name="column">The name of the column to pass into the SQL AVG function</param>
        /// <param name="alias">The name of the result column</param>
        /// <returns>string</returns>
        public static string Average(string column, string alias) {
            return $"AVG({column}) {alias}";
        }

        /// <summary>
        /// Generates a MIN function string to be used in a SQL statement
        /// </summary>
        /// <param name="column">The name of the column to pass into the SQL MIN function</param>
        /// <param name="alias">The name of the result column</param>
        /// <returns>string</returns>
        public static string Minimum(string column, string alias) {
            return $"MIN({column}) {alias}";
        }

        /// <summary>
        /// Generates a MAX function string to be used in a SQL statement
        /// </summary>
        /// <param name="column">The name of the column to pass into the SQL MAX function</param>
        /// <param name="alias">The name of the result column</param>
        /// <returns>string</returns>
        public static string Maximum(string column, string alias) {
            return $"MAX({column}) {alias}";
        }

        /// <summary>
        /// Generates a LENGTH/LEN function string to be used in a SQL statement
        /// </summary>
        /// <param name="column">The name of the column to pass into the SQL LENGTH/LEN function</param>
        /// <param name="alias">The name of the result column</param>
        /// <param name="dbType">The type of database to run the SQL statement against</param>
        /// <returns>string</returns>
        public static string Length(string column, string alias, DatabaseType dbType) {
            if (dbType == DatabaseType.SqlServer) {
                return $"LEN({column}) {alias}";
            } else {
                return $"LENGTH({column}) {alias}";
            }
        }

        /// <summary>
        /// Generates a COUNT function string to be used in a SQL statement
        /// </summary>
        /// <param name="column">The name of the column to pass into the SQL COUNT function</param>
        /// <param name="alias">The name of the result column</param>
        /// <returns>string</returns>
        public static string Count(string column, string alias) {
            return $"COUNT({column}) {alias}";
        }

        /// <summary>
        /// Generates a CONCAT function string to be used in a SQL statement
        /// </summary>
        /// <param name="columns">The name of the column to pass into the SQL CONCAT function</param>
        /// <param name="alias">The name of the result column</param>
        /// <returns>string</returns>
        public static string Concatenate(IList<string> columns, string alias) {
            var s = "CONCAT(";
            
            foreach (var column in columns) {
                s += column + ",";
            }

            s = s.Substring(0, s.Length - 1);
            s += ") " + alias;

            return s;
        }


    }
}
