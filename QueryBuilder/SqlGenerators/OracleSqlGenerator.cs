using QueryBuilder.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.SqlGenerators
{
    public class OracleSqlGenerator : BaseSqlGenerator
    {
        public OracleSqlGenerator()
        {
            base.openingColumnMark = '"';
            base.closingColumnMark = '"';

            //Website for type mappings:
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/oracle-data-type-mappings
            Dictionary<string, bool> mappings = new Dictionary<string, bool>();
            mappings.Add("integer", false);
            mappings.Add("float", false);
            mappings.Add("unsigned integer", false);
            mappings.Add("number", false);
            mappings.Add("char", true);
            mappings.Add("long", false);
            mappings.Add("nchar", true);
            mappings.Add("nvarchar2", true);
            mappings.Add("varchar2", true);
            mappings.Add("date", true);
            mappings.Add("timestamp", true);
            mappings.Add("timestamp with local time zone", true);
            mappings.Add("timestamp with time zone", true);
            mappings.Add("interval year to month", true);
            mappings.Add("interval day to second", true);

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
                sql.Append(CreateWHEREClause(query.Criteria, query.Columns, query.SuppressNulls));

                // If SQL already contains a WHERE clause, then add " AND " followed by a LIMIT clause. 
                if (sql.ToString().Contains(" WHERE "))
                {
                    sql.Append(" AND " + CreateLimitClause(query.Limit));
                }
                // If SQL does not already contain a WHERE clause, then add " WHERE " followed by the LIMIT clause.
                else
                {
                    sql.Append(" WHERE " + CreateLimitClause(query.Limit));
                }

                sql.Append(CreateGROUPBYCluase(query.GroupBy, query.Columns));
                sql.Append(CreateORDERBYCluase(query.OrderBy, query.Columns, query.Ascending));
                sql.Append(CreateOffsetClause(query.Offset));
                return sql.ToString().Replace("  ", " ");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override StringBuilder CreateLimitClause(int? limit)
        {
            if (limit == null) return null;

            //var cleansedLimit = SQLCleanser.EscapeAndRemoveWords(limit);
            return new StringBuilder(" ROWNUM < " + limit).Replace("  ", " ");
        }

        protected override StringBuilder CreateOffsetClause(string offset)
        {
            if (offset == null) return null;

            var cleansedOffset = SQLCleanser.EscapeAndRemoveWords(offset);
            return new StringBuilder(" OFFSET " + cleansedOffset + " ROWS").Replace("  ", " ");
        }
    }
}
