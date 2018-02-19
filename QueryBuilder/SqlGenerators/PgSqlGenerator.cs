using QueryBuilder.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.SqlGenerators
{
    public class PgSqlGenerator : BaseSqlGenerator
    {
        public PgSqlGenerator()
        {
            base.openingColumnMark = '\"';
            base.closingColumnMark = '\"';

            //Website for type mappings:
            //http://www.npgsql.org/doc/types/basic.html
            Dictionary<string, bool> mappings = new Dictionary<string, bool>();
            mappings.Add("bool", false);
            mappings.Add("int2", false);
            mappings.Add("int4", false);
            mappings.Add("int8", false);
            mappings.Add("float4", false);
            mappings.Add("float8", false);
            mappings.Add("numeric", false);
            mappings.Add("money", false);
            mappings.Add("text", true);
            mappings.Add("varchar", true);
            mappings.Add("bpchar", true);
            mappings.Add("citext", true);
            mappings.Add("json", true);
            mappings.Add("jsonb", true);
            mappings.Add("date", true);
            mappings.Add("interval", true);
            mappings.Add("timestamptz", true);
            mappings.Add("time", true);
            mappings.Add("timetz", true);

            base.typeMappings = mappings;
        }

        public override string CreateSql(Query query)
        {
            base.tableSchema = query.TableSchema;

            //Query newQuery = new Query("main").SetDistinct(query.Distinct)
            //                                  .SetColumns(query.Columns)
            //                                  .SetTable(query.Table)
            //                                  .SetCriteria(query.Criteria)
            //                                  .SetSuppressNulls(query.SuppressNulls)
            //                                  .SetGroupBy(query.GroupBy)
            //                                  .SetOrderBy(query.OrderBy)
            //                                  .SetAscending(query.Ascending)
            //                                  .SetLimit(query.Limit)
            //                                  .SetOffset(query.Offset);


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
