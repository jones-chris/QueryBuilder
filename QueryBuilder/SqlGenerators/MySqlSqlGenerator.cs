using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Config;
using System.Data;

namespace QueryBuilder.SqlGenerators
{
    public class MySqlSqlGenerator : BaseSqlGenerator
    {
        public MySqlSqlGenerator()
        {
            base.openingColumnMark = '`';
            base.closingColumnMark = '`';

            //Website for type mappings:
            //https://www.devart.com/dotconnect/mysql/docs/DataTypeMapping.html
            Dictionary<string, bool> mappings = new Dictionary<string, bool>();
            mappings.Add("bool", false);
            mappings.Add("boolean", false);
            mappings.Add("tinyint", false);
            mappings.Add("tinyint unsigned", false);
            mappings.Add("smallint", false);
            mappings.Add("year", false);
            mappings.Add("int", false);
            mappings.Add("smallint unsigned", false);
            mappings.Add("mediumint", false);
            mappings.Add("bigint", false);
            mappings.Add("int unsigned", false);
            mappings.Add("integer unsigned", false);
            mappings.Add("float", false);
            mappings.Add("double", false);
            mappings.Add("real", false);
            mappings.Add("decimal", false);
            mappings.Add("numeric", false);
            mappings.Add("dec", false);
            mappings.Add("fixed", false);
            mappings.Add("bigint unsigned", false);
            mappings.Add("float unsigned", false);
            mappings.Add("double unsigned", false);
            mappings.Add("serial", false);
            mappings.Add("date", true);
            mappings.Add("timestamp", true);
            mappings.Add("datetime", true);
            mappings.Add("datetimeoffset", true);
            mappings.Add("time", true);
            mappings.Add("char", true);
            mappings.Add("varchar", true);
            mappings.Add("tinytext", true);
            mappings.Add("text", true);
            mappings.Add("mediumtext", true);
            mappings.Add("longtext", true);
            mappings.Add("set", true);
            mappings.Add("enum", true);
            mappings.Add("nchar", true);
            mappings.Add("national char", true);
            mappings.Add("nvarchar", true);
            mappings.Add("national varchar", true);
            mappings.Add("character varying", true);

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