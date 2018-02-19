using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Config;
using System.Data;

namespace QueryBuilder.SqlGenerators
{
    public class AccessSqlGenerator : BaseSqlGenerator
    {
        public AccessSqlGenerator()
        {
            base.openingColumnMark = '[';
            base.closingColumnMark = ']';

            //Website for type mappings:
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ole-db-data-type-mappings
            Dictionary<string, bool> mappings = new Dictionary<string, bool>();
            mappings.Add("adboolean", false); //adBoolean
            mappings.Add("adbigint", false); // adBigInt
            mappings.Add("adcurrency", false); // adCurrency
            mappings.Add("addecimal", false); // adDecimal
            mappings.Add("addouble", false); // adDouble
            mappings.Add("adinteger", false); // adInteger
            mappings.Add("adnumeric", false); // adNumeric
            mappings.Add("adsingle", false); // adSingle
            mappings.Add("adsmallint", false); // adSmallInt
            mappings.Add("adtinyint", false); // adTinyInt
            mappings.Add("adunsignedbigint", false); // adUnsignedBigInt
            mappings.Add("adunsignedint", false); // adUnsignedInt
            mappings.Add("adunsignedsmallint", false); // adUnsignedSmallInt
            mappings.Add("adbstr", true); // adBSTR
            mappings.Add("adchar", true); // adChar
            mappings.Add("adwchar", true); // adWChar
            mappings.Add("addate", true); // adDate
            mappings.Add("addbdate", true); // adDBDate
            mappings.Add("addbTime", true); // adDBTime
            mappings.Add("addbtimestamp", true); // adDBTimeStamp
            mappings.Add("adfiletime", true); // adFileTime

            base.typeMappings = mappings;
        }

        //public override string CreateSql(DataTable tableSchema, bool distinct = false, List<string> columns = null, string table = null, List<Criteria> criteria = null,
        //    bool groupBy = false, bool orderBy = false, string limit = null, string offset = null, bool asc = false)
        //{
        //    base.tableSchema = tableSchema;

        //    StringBuilder sql = new StringBuilder("");
        //    sql.Append(CreateSELECTClause(distinct, columns, limit));
        //    sql.Append(CreateFROMClause(table));
        //    sql.Append(CreateWHEREClause(criteria));
        //    sql.Append(CreateGROUPBYCluase(groupBy, columns));
        //    sql.Append(CreateORDERBYCluase(orderBy, columns, asc));
        //    sql.Append(CreateOffsetClause(offset));
        //    return sql.ToString().Replace("  ", " ");
        //}

        public override string CreateSql(Query query)
        {
            base.tableSchema = query.TableSchema;

            try
            {
                StringBuilder sql = new StringBuilder("");
                sql.Append(CreateSELECTClause(query.Distinct, query.Columns));
                sql.Append(CreateFROMClause(query.Table));
                sql.Append(CreateWHEREClause(query.Criteria, query.Columns, query.SuppressNulls));
                sql.Append(CreateGROUPBYCluase(query.GroupBy, query.Columns));
                sql.Append(CreateORDERBYCluase(query.OrderBy, query.Columns, query.Ascending));
                sql.Append(CreateLimitClause(query.Limit));
                sql.Append(CreateOffsetClause(query.Offset));
                return sql.ToString().Replace("  ", " ");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private StringBuilder CreateSELECTClause(bool distinct, List<string> columns, string limit)
        {
            if (columns == null) return null;

            string startSql = (distinct == true) ? "SELECT DISTINCT " : "SELECT ";
            StringBuilder sql = new StringBuilder(startSql);

            if (limit != null)
            {
                sql.Append(" TOP " + SQLCleanser.EscapeAndRemoveWords(limit));
            }

            foreach (string column in columns)
            {
                sql.Append(string.Format("{0}{1}{2}, ", openingColumnMark, SQLCleanser.EscapeAndRemoveWords(column), closingColumnMark));
            }
            sql = sql.Remove(sql.Length - 2, 2).Append(" ");
            return sql.Replace("  ", " ");
        }
    }
}
