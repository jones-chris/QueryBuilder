using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QueryBuilder.SqlGenerators
{
    public static class SQLCleanser
    {
        // Only single and double quotes should be escaped because they COULD be included in criteria
        private static readonly string[] WordsNeedingEscaping = new string[] { "'", @""""}; 
        private static readonly string[] WordsNeedingRemoval = new string[] { "-", "=", ">", "<", "!=", "<>", ">=", "<=", ";", " DROP ", " CREATE ", " DELETE ", " INSERT ", " UPDATE ", "`" };


        public static string EscapeWords(string sql)
        {
            foreach (string word in WordsNeedingEscaping)
            {
                sql = Regex.Replace(sql, word, word + word, RegexOptions.IgnoreCase);
            }

            return sql;
        }

        public static string RemoveWords(string sql)
        {
            foreach (string word in WordsNeedingRemoval)
            {
                sql = Regex.Replace(sql, word, string.Empty, RegexOptions.IgnoreCase);
            }

            return sql;
        }

        public static string EscapeAndRemoveWords(string sql)
        {
            sql = EscapeWords(sql);
            sql = RemoveWords(sql);
            return sql;
        }
    }
}
