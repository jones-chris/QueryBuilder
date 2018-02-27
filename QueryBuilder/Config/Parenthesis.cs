using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Config
{
    public enum Parenthesis
    {
        [Display(Name = "(")]
        BeginningParenthesis,
        [Display(Name = ")")]
        EndingParenthesis
    }
}
