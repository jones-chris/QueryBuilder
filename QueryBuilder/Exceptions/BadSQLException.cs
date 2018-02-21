using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Exceptions
{
    public class BadSQLException : Exception
    {
        public BadSQLException(string message) : base(message) { }
    }
}
