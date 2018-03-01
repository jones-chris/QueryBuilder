using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderTests.TestUtilities
{
    public class TestUtil
    {
        public static DataTable MultiColumnDataTableBuilder()
        {
            var dt = new DataTable();
            dt.Columns.Add("COLUMN_NAME");
            dt.Columns.Add("col2");
            dt.Columns.Add("col3");
            dt.Columns.Add("col4");
            dt.Columns.Add("col5");
            dt.Columns.Add("col6");
            dt.Columns.Add("col7");
            dt.Columns.Add("col8");
            dt.Columns.Add("col9");
            dt.Columns.Add("col10");
            dt.Columns.Add("co11");
            dt.Columns.Add("DATA_TYPE");
            dt.Rows.Add(new object[] { "service", null, null, null, null, null, null, null, null, null, null, "text" });
            dt.Rows.Add(new object[] { "fund", null, null, null, null, null, null, null, null, null, null, "text" });
            dt.Rows.Add(new object[] { "account", null, null, null, null, null, null, null, null, null, null, "int2" });

            return dt;
        }
    }
}
