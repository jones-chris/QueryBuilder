using QueryBuilder.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.SqlGenerators
{
    public class RedshiftSqlGenerator : BaseSqlGenerator
    {
        public RedshiftSqlGenerator()
        {
            base.openingColumnMark = '"';
            base.closingColumnMark = '"';

            //Website for type mappings (Redshift uses System.Data.ODBC):
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/odbc-data-type-mappings
            Dictionary<string, bool> mappings = new Dictionary<string, bool>();
            mappings.Add("sql_bit", false);
            mappings.Add("sql_bigint", false);
            mappings.Add("sql_integer", false);
            mappings.Add("sql_decimal", false);
            mappings.Add("sql_double", false);
            mappings.Add("sql_numeric", false);
            mappings.Add("sql_real", false);
            mappings.Add("sql_smallint", false);
            mappings.Add("sql_char", true);
            mappings.Add("sql_long_varchar", true);
            mappings.Add("sql_wchar", true);
            mappings.Add("sql_wlongvarchar", true);
            mappings.Add("sql_wvarchar", true);
            mappings.Add("sql_type_times", true);
            mappings.Add("sql_type_timestamp", true);

            base.typeMappings = mappings;
        }

        //public override string CreateSql(DataTable tableSchema, bool distinct = false, List<string> columns = null, string table = null, List<Criteria> criteria = null,
        //    bool groupBy = false, bool orderBy = false, string limit = null, string offset = null, bool asc = false)
        //{
        //    base.tableSchema = tableSchema;

        //    StringBuilder sql = new StringBuilder("");
        //    sql.Append(CreateSELECTClause(distinct, columns));
        //    sql.Append(CreateFROMClause(table));
        //    sql.Append(CreateWHEREClause(criteria));
        //    sql.Append(CreateGROUPBYCluase(groupBy, columns));
        //    sql.Append(CreateORDERBYCluase(orderBy, columns, asc));
        //    sql.Append(CreateLimitClause(limit));
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
    }
}
