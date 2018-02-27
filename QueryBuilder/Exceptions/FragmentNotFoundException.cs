using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Exceptions
{
    public class FragmentNotFoundException : Exception
    {
        public FragmentNotFoundException(string message) : base(message) { }
    }
}
