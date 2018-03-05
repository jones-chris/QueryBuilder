using QueryBuilder.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.SqlGenerators
{
    public class SqliteSqlGenerator : BaseSqlGenerator
    {
        public SqliteSqlGenerator()
        {
            base.openingColumnMark = '"';
            base.closingColumnMark = '"';

            //Website for type mappings:
            //https://www.devart.com/dotconnect/sqlite/docs/DataTypeMapping.html
            Dictionary<string, bool> mappings = new Dictionary<string, bool>();
            mappings.Add("boolean", false);
            mappings.Add("smallint", false);
            mappings.Add("int16", false);
            mappings.Add("int", false);
            mappings.Add("int32", false);
            mappings.Add("integer", false);
            mappings.Add("int64", false);
            mappings.Add("real", false);
            mappings.Add("numeric", false);
            mappings.Add("decimal", false);
            mappings.Add("money", false);
            mappings.Add("currency", false);
            mappings.Add("date", true);
            mappings.Add("time", true);
            mappings.Add("datetime", true);
            mappings.Add("smalldate", true);
            mappings.Add("datetimeoffset", true);
            mappings.Add("text", true);
            mappings.Add("ntext", true);
            mappings.Add("char", true);
            mappings.Add("nchar", true);
            mappings.Add("varchar", true);
            mappings.Add("nvarchar", true);
            mappings.Add("string", true);

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
