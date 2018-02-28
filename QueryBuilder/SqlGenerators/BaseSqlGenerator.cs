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
        if (columns == null) return null;

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
        if (table == null) return null;

        string s = string.Format(" FROM {0}{1}{2} ", openingColumnMark, SQLCleanser.EscapeAndRemoveWords(table), closingColumnMark);
        StringBuilder sql = new StringBuilder(s);
        return sql.Replace("  ", " ");
    }

    protected virtual StringBuilder CreateWHEREClause(IList<Criteria> criteria, IList<string> columns, bool suppressNulls)
    {
        StringBuilder sql = new StringBuilder(" WHERE ");
        if (suppressNulls)
        {
            sql.Append(CreateSuprressNullsClause(columns));
        }

        if (criteria == null) return sql.Replace("  ", " ");

        if (criteria.Count > 0)
        {
            // If nulls are not being suppressed, then the first Criteria's AndOr property is not needed since it's coming directly after
            // the WHERE keyword in the generated SQL statement.
            if (!suppressNulls)
            {
                criteria.First().AndOr = null;
            }

            //for each row
            foreach (var theCriteria in criteria)
            {
                if (!CriteriaHasNullValues(theCriteria))
                {
                    //sql.Append(theCriteria.AndOr == null ? null : $" {theCriteria.AndOr} ");
                    //sql.Append(theCriteria.FrontParenthesis == null ? null : $" {theCriteria.FrontParenthesis} ");
                    //sql.Append(theCriteria.Column == null ? null : $" {openingColumnMark}{theCriteria.Column}{closingColumnMark} ");
                    //sql.Append(theCriteria.Operator == null ? null : $" {theCriteria.Operator} ");
                    //sql.Append(theCriteria.EndParenthesis == null ? null : $" {theCriteria.EndParenthesis} ");

                    // determine if Criteria's filter property is a subquery
                    if (IsSubQuery(theCriteria.Filter))
                    {
                        theCriteria.Filter = SQLCleanser.EscapeAndRemoveWords(theCriteria.Filter);
                        sql.Append($" ({theCriteria.ToString()}) ");
                    }

                    // if not subquery, then determine if column needs quotes or not
                    var columnDataType = GetColumnDataType(theCriteria.Column);
                    if (columnDataType == null)
                    {
                        // if the column name cannot be found in the table schema datatable, then throw BadSQLException
                        throw new Exception(string.Format($"Could not find column name, {theCriteria.Column}, in table schema for {tableSchema.TableName}"));
                    }
                    else
                    {
                        var shouldHaveQuotes = IsColumnQuoted(columnDataType);

                        if (theCriteria.Operator == Operator.In || theCriteria.Operator == Operator.NotIn)
                        {
                            var originalFilters = theCriteria.Filter.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            var newFilters = new string[originalFilters.Count()];
                            if (shouldHaveQuotes)
                            {
                                for (var i = 0; i < originalFilters.Count(); i++)
                                {
                                    newFilters[i] = $"'{SQLCleanser.EscapeAndRemoveWords(originalFilters[i])}'";
                                }
                                theCriteria.Filter = "(" + string.Join(",", newFilters) + ")";
                                sql.Append($" {theCriteria.ToString()} ");
                            }
                            else
                            {
                                theCriteria.Filter = "(" + SQLCleanser.EscapeAndRemoveWords(theCriteria.Filter) + ")";
                                sql.Append($" {theCriteria.ToString()} ");
                            }
                        }
                        //else if (theCriteria.Operator.ToLower() == "is null")
                        //{
                        //    sql.Append($" {theCriteria.ToString()} ");
                        //}
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
                    throw new BadSQLException($"The criteria at index {criteria.IndexOf(theCriteria)} in the Criteria list has a " +
                                               "null value for it's Column, Operator, or Filter.  Please make sure each Criteria has " +
                                               "has a non-null value for each of these properties");
                }
            }

            return sql.Replace("  ", " ");
        }
        return sql.Replace("  ", " ");
    }

    protected virtual StringBuilder CreateGROUPBYCluase(bool groupBy, IList<string> columns)
    {
        if (groupBy == false) return null;

        if (columns.Count > 0)
        {
            StringBuilder sql = new StringBuilder(" GROUP BY ");
            foreach (string column in columns)
            {
                sql.Append(string.Format("{0}{1}{2}, ", openingColumnMark, SQLCleanser.EscapeAndRemoveWords(column), closingColumnMark));
            }
            sql.Remove(sql.Length - 2, 2).Append(" ");
            return sql.Replace("  ", " ");
        }
        return null;
    }

    protected virtual StringBuilder CreateORDERBYCluase(bool orderBy, IList<string> columns, bool asc)
    {
        if (orderBy == false) return null;

        if (columns.Count > 0)
        {
            StringBuilder sql = new StringBuilder(" ORDER BY ");
            foreach (string column in columns)
            {
                sql.Append(string.Format("{0}{1}{2}, ", openingColumnMark, SQLCleanser.EscapeAndRemoveWords(column), closingColumnMark));
            }
            sql.Remove(sql.Length - 2, 2).Append(" ");
            return (asc == true) ? sql.Append(" ASC ").Replace("  ", " ") : sql.Append(" DESC ").Replace("  ", " ");
        }
        return null;
    }

    protected virtual StringBuilder CreateLimitClause(int? limit)
    {
        if (limit == null) return null;

        //var cleansedLimit = SQLCleanser.EscapeAndRemoveWords(limit);
        return (limit == null) ? null : new StringBuilder(" LIMIT " + limit).Replace("  ", " ");
    }

    protected virtual StringBuilder CreateOffsetClause(string offset)
    {
        if (offset == null) return null;

        var cleansedOffset = SQLCleanser.EscapeAndRemoveWords(offset);
        return (offset == null) ? null : new StringBuilder(" OFFSET " + cleansedOffset).Replace("  ", " ");
    }

    public virtual StringBuilder CreateSuprressNullsClause(IList<string> columns)
    {
        if (columns == null) return null;

        StringBuilder sql = new StringBuilder();
        if (columns.Count > 0)
        {
            for (var i = 0; i < columns.Count; i++)
            {
                if (i == 0)
                {
                    sql.Append($" ({columns[i]} IS NOT NULL ");
                }
                else
                {
                    sql.Append($" OR {columns[i]} IS NOT NULL ");
                }
            }
        }

        return sql.Append(")");
    }

    private string GetColumnDataType(string columnName)
    {
        EnumerableRowCollection<DataRow> columnInfo = tableSchema.AsEnumerable()
                                                                    .Where(row => row.Field<string>("COLUMN_NAME")
                                                                    .Equals(columnName));

        if (columnInfo.Count() == 1)
        {
            return (string)columnInfo.ElementAt(0)["data_type"]; //11
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

    private bool IsSubQuery(string filter)
    {
        if (filter.Length >= 6)
        {
            return (filter.Substring(0, 6).ToLower() == "select") ? true : false;
        }
        return false;
    }

    //private IList<Criteria> AddParenthesisToCriteria(IList<Criteria> criteria)
    //{
    //    // If there is only one or zero items in criteria list, then just return criteria unaltered.
    //    if (criteria.Count <= 1)
    //    {
    //        return criteria;
    //    }

    //    for (var i = 0; i < criteria.Count; i++)
    //    {
    //        var currentColumn = criteria[i].Column;
    //        string priorColumn;
    //        string nextColumn;

    //        // If first criteria in list
    //        if (i == 0)
    //        {
    //            nextColumn = criteria[i + 1].Column;
    //            if (currentColumn == nextColumn) { criteria[i].FrontParenthesis = "("; }
    //        }
    //        // If last criteria in list
    //        else if (i == criteria.Count - 1)
    //        {
    //            priorColumn = criteria[i - 1].Column;
    //            if (currentColumn == priorColumn) { criteria[i].EndParenthesis = ")"; }
    //        }
    //        // If criteria is neither the first or last
    //        else
    //        {
    //            priorColumn = criteria[i - 1].Column;
    //            nextColumn = criteria[i + 1].Column;
    //            if (currentColumn != priorColumn && currentColumn == nextColumn)
    //            {
    //                criteria[i].FrontParenthesis = "(";
    //            }
    //            else if (currentColumn == priorColumn && currentColumn != nextColumn)
    //            {
    //                criteria[i].EndParenthesis = ")";
    //            }
    //        }
    //    }

    //    return criteria;
    //}

    private bool CriteriaHasNullValues(Criteria criteria)
    {
        // Test each criteria's Column, Operator, and Filter properties.  If any criteria in list is null, then return true.
        if (criteria.Column == null) return true;
        //if (criteria.Operator == null) return true;
        //if (criteria.Filter == null) return true;
           
        return false;
    }
}
}
