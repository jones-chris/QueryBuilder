using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QueryBuilder.SqlGenerators
{
    [Serializable, XmlRoot("Criteria"), XmlType("Criteria")]
    public class Criteria
    {
        private bool _orIsNull;
        private string _filter;
        [XmlElement] public string AndOr { get; set; }
        [XmlElement] public string FrontParenthesis { get; set; }
        [XmlElement] public string Column { get; set; }
        [XmlElement] public string Operator { get; set; }
        [XmlElement] public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
            }
        }
        [XmlElement] public string EndParenthesis { get; set; }
        [XmlElement] public bool OrIsNull
        {
            get { return _orIsNull; }
            set
            {
                FrontParenthesis = "(";
                _orIsNull = value;
                EndParenthesis = ")";
            }
        }

        public enum Operators
        {
            Equals,
            NotEquals,
            GreaterThanOrEquals,
            LessThanOrEquals,
            GreaterThan,
            LessThan,
            Like,
            NotLike,
            In,
            NotIn,
            IsNull,
            IsNotNull
        }

        public Criteria()
        {
            AndOr = "";
            FrontParenthesis = "";
            Column = "";
            Operator = "";
            Filter = "";
            EndParenthesis = "";
        }

        public Criteria(string AndOr = "", string FrontParenthesis = "", string Column = "", 
            string Operator = "", string Filter = "", string EndParenthesis = "", bool Locked = false)
        {
            this.AndOr = AndOr;
            this.FrontParenthesis = FrontParenthesis;
            this.Column = Column;
            this.Operator = Operator;
            this.Filter = Filter;
            this.EndParenthesis = EndParenthesis;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Criteria that = (Criteria)obj;
            if (!Column.Equals(that.Column)) return false;
            if (!Operator.Equals(that.Operator)) return false;
            if (!Filter.Equals(that.Filter)) return false;
            return true;
        }

        public override string ToString()
        {
            if (OrIsNull && Filter != null)
            {
                return $" {AndOr} {FrontParenthesis}{Column} {Operator} {Filter} OR {Column} IS NULL {EndParenthesis} ";
            }
            else if (OrIsNull && Filter == null)
            {
                return $" {AndOr} {Column} IS NULL ";
            }
            else
            {
                return $" {AndOr} {FrontParenthesis}{Column} {Operator} {Filter}{EndParenthesis} ";
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
