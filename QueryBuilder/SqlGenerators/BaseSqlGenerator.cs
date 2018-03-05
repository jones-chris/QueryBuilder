using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Config;
using System.Data;
using QueryBuilder.Exceptions;

namespace QueryBuilder.SqlGenerators
{
public abstract class BaseSqlGenerator
{
    protected Dictionary<string, bool> typeMappings = new Dictionary<string, bool>();
    protected char openingColumnMark;
    protected char closingColumnMark;
    protected DataTable tableSchema;


    public BaseSqlGenerator()
    {
    }

    public abstract string CreateSql(Query query);

    protected virtual StringBuilder CreateSELECTClause(bool distinct, IList<string> columns)
    {
        if (columns == null) throw new ArgumentNullException();

        if (columns.Count == 0) throw new EmptyCollectionException();

        string startSql = (distinct == true) ? "SELECT DISTINCT " : "SELECT ";
        StringBuilder sql = new StringBuilder(startSql);
        foreach (string column in columns)
        {
            sql.Append(string.Format("{0}{1}{2}, ", openingColumnMark, SQLCleanser.EscapeAndRemoveWords(column), closingColumnMark));
        }
        sql = sql.Remove(sql.Length - 2, 2).Append(" ");
        return sql.Replace("  ", " ");
    }

    protected virtual StringBuilder CreateFROMClause(string table)
    {
        if (table == null) throw new ArgumentNullException();

        if (table == string.Empty) throw new ArgumentException("The table argument is an empty string");

        string s = string.Format(" FROM {0}{1}{2} ", openingColumnMark, SQLCleanser.EscapeAndRemoveWords(table), closingColumnMark);
        StringBuilder sql = new StringBuilder(s);
        return sql.Replace("  ", " ");
    }

    protected virtual StringBuilder CreateWHEREClause(IList<Criteria> criteria)
    {
        if (criteria == null)
        {
            return null;
        }

        if (criteria.Count == 0)
        {
            return null;
        }
        else
        {
            // First criteria's AndOr property is not needed since it's coming directly after the WHERE keyword in the SQL statement.
            criteria.First().AndOr = null;

            StringBuilder sql = new StringBuilder(" WHERE ");

            foreach (var theCriteria in criteria)
            {
                if (! CriteriaHasNullColumnOrOperator(theCriteria))
                {
                    // If Operator is "Is Null" or "Is Not Null".
                    if (theCriteria.Operator == Operator.IsNull || theCriteria.Operator == Operator.IsNotNull)
                    {
                        sql.Append($" {theCriteria.ToString()} ");
                        continue;
                    }

                    // Now that we know that the operator is something other than "Is Null or "Is Not Null", we need to check that the Filter is not null or an empty string.
                    if (theCriteria.Filter == null || theCriteria.Filter == string.Empty)
                    {
                        throw new Exception("A criteria has a null or empty filter, but the operator is not 'IsNull' or 'IsNotNull'");
                    }
                        
                    // Now that we know that the Filter is not null or an empty string, test if the filter is a subquery.
                    if (theCriteria.FilterIsSubQuery())
                    {
                        theCriteria.Filter = SQLCleanser.EscapeAndRemoveWords(theCriteria.Filter);
                        sql.Append($" {theCriteria.ToString()} ");
                        continue;
                    }

                    // If the filter is not a subquery, then determine if the filter needs quotes or not (quotes if it's text based, no quotes if it's number based).
                    var columnDataType = GetColumnDataType(theCriteria.Column);
                    if (columnDataType == null)
                    {
                        // If the column name cannot be found in the table schema datatable, then throw Exception
                        throw new Exception(string.Format($"Could not find column name, {theCriteria.Column}, in table schema for {tableSchema.TableName}"));
                    }
                    else
                    {
                        var shouldHaveQuotes = IsColumnQuoted(columnDataType);

                        // If the operator is "In" or "Not In".
                        if (theCriteria.Operator == Operator.In || theCriteria.Operator == Operator.NotIn)
                        {
                            var originalFilters = theCriteria.Filter.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            var newFilters = new string[originalFilters.Count()];

                            // If the filter should have quotes.
                            if (shouldHaveQuotes)
                            {
                                for (var i = 0; i < originalFilters.Count(); i++)
                                {
                                    newFilters[i] = $"'{SQLCleanser.EscapeAndRemoveWords(originalFilters[i])}'";
                                }
                                theCriteria.Filter = "(" + string.Join(",", newFilters) + ")";
                                sql.Append($" {theCriteria.ToString()} ");
                            }
                            //If the filter should NOT have quotes.
                            else
                            {
                                theCriteria.Filter = "(" + SQLCleanser.EscapeAndRemoveWords(theCriteria.Filter) + ")";
                                sql.Append($" {theCriteria.ToString()} ");
                            }
                        }
                        // If the operator is anything other than "In" or "Not In" (and not "Is Null" or "Is Not Null" because that test is done earlier in the method).
                        else
                        {
                            var cleansedValue = SQLCleanser.EscapeAndRemoveWords(theCriteria.Filter);
                            theCriteria.Filter = (shouldHaveQuotes) ? $" '{cleansedValue}' " : $" {cleansedValue} ";
                            sql.Append($" {theCriteria.ToString()} ");
                        }
                    }
                }
                else
                {
                    throw new BadSQLException("One or more of the criteria in the Criteria list has a " +
                                              "null value for it's Column and/or operator.  Please make sure each criteria has " +
                                              "has a non-null value for each of these properties");
                }
            }

            return sql.Replace("  ", " ");
        }
    }

    protected virtual StringBuilder CreateGROUPBYCluase(bool groupBy, IList<string> columns)
    {
        if (groupBy == false) return null;

        if (columns == null) throw new ArgumentNullException();

        if (columns.Count == 0)
        {
            throw new EmptyCollectionException();
        }
        else
        {
            StringBuilder sql = new StringBuilder(" GROUP BY ");
            foreach (string column in columns)
            {
                sql.Append(string.Format("{0}{1}{2}, ", openingColumnMark, SQLCleanser.EscapeAndRemoveWords(column), closingColumnMark));
            }
            sql.Remove(sql.Length - 2, 2).Append(" ");
            return sql.Replace("  ", " ");
        }
    }

    protected virtual StringBuilder CreateORDERBYCluase(bool orderBy, IList<string> columns, bool asc)
    {
        if (orderBy == false) return null;

        if (columns == null) throw new ArgumentNullException();

        if (columns.Count == 0)
        {
            throw new EmptyCollectionException();
        }
        else 
        {
            StringBuilder sql = new StringBuilder(" ORDER BY ");
            foreach (string column in columns)
            {
                sql.Append(string.Format("{0}{1}{2}, ", openingColumnMark, SQLCleanser.EscapeAndRemoveWords(column), closingColumnMark));
            }
            sql.Remove(sql.Length - 2, 2).Append(" ");
            return (asc == true) ? sql.Append(" ASC ").Replace("  ", " ") : sql.Append(" DESC ").Replace("  ", " ");
        }
    }

    protected virtual StringBuilder CreateLimitClause(long? limit)
    {
        return (limit == null) ? null : new StringBuilder($" LIMIT {limit} ").Replace("  ", " ");
    }

    protected virtual StringBuilder CreateOffsetClause(long? offset)
    {
        return (offset == null) ? null : new StringBuilder($" OFFSET {offset} ").Replace("  ", " ");
    }

    public virtual StringBuilder CreateSuprressNullsClause(IList<string> columns)
    {
        if (columns == null) throw new ArgumentNullException();

        if (columns.Count == 0)
        {
            throw new EmptyCollectionException();
        } 
        else
        {
            StringBuilder sql = new StringBuilder();
            for (var i = 0; i < columns.Count; i++)
            {
                if (i == 0)
                {
                    sql.Append($" ({openingColumnMark}{columns[i]}{closingColumnMark} IS NOT NULL ");
                }
                else
                {
                    sql.Append($" OR {openingColumnMark}{columns[i]}{closingColumnMark} IS NOT NULL ");
                }
            }
            return sql.Append(") ");
        }
    }

    private string GetColumnDataType(string columnName)
    {
        EnumerableRowCollection<DataRow> columnInfo = tableSchema.AsEnumerable()
                                                                    .Where(row => row.Field<string>("COLUMN_NAME")
                                                                    .Equals(columnName));

        if (columnInfo.Count() == 1)
        {
            return (string)columnInfo.ElementAt(0)["data_type"]; 
        }
        else
        {
            return null;
        }
    }

    private bool IsColumnQuoted(string columnDataType)
    {
        try
        {
            return typeMappings[columnDataType.ToLower()];
        }
        catch (Exception)
        {
            // If column does not exist in typeMappings list.
            // Best guess if can't find column type - have 50/50 chance and worst that happens is SQL query fails.
            // It's more conservative to use quotes, so that SQL injection has less chance of happening because criteria
            // is wrapped in single quotes.
            return true;
        }
    }

    private bool CriteriaHasNullColumnOrOperator(Criteria criteria)
    {
        // Test each criteria's Column and Operator properties.  If any criteria in list is null, then return true.
        if (criteria.Column == null) return true;
        if (criteria.Operator == null) return true;
        //if (criteria.Filter == null) return true;
           
        return false;
    }
}
}
