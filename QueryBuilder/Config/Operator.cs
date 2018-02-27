using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder.Config
{
    public enum Operator
    {
        [Display(Name = "=")]
        Equals,
        [Display(Name = "<>")]
        NotEquals,
        [Display(Name = ">=")]
        GreaterThanOrEquals,
        [Display(Name = "<=")]
        LessThanOrEquals,
        [Display(Name = ">")]
        GreaterThan,
        [Display(Name = "<")]
        LessThan,
        [Display(Name = "Like")]
        Like,
        [Display(Name = "Not Like")]
        NotLike,
        [Display(Name = "In List")]
        In,
        [Display(Name = "Not In List")]
        NotIn,
        [Display(Name = "Is Null")]
        IsNull,
        [Display(Name = "Is Not Null")]
        IsNotNull
    }
}
