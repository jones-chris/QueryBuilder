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

            try
            {
                StringBuilder sql = new StringBuilder("");
                sql.Append(CreateSELECTClause(query.Distinct, query.Columns));
                sql.Append(CreateFROMClause(query.Table));
                sql.Append(CreateWHEREClause(query.Criteria));

                if (sql.ToString().Contains(" WHERE "))
                {
                    if (query.SuppressNulls)
                    {
                        sql.Append(" AND " + CreateSuprressNullsClause(query.Columns));
                    }
                }
                else
                {
                    if (query.SuppressNulls)
                    {
                        sql.Append(" WHERE " + CreateSuprressNullsClause(query.Columns));
                    }
                }

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
