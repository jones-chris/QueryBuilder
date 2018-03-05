using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using QueryBuilder.Config;
using QueryBuilder.Utilities;

namespace QueryBuilder.SqlGenerators
{
    [Serializable, XmlRoot("Criteria"), XmlType("Criteria")]
    public class Criteria
    {
        private bool _orIsNull;
        [XmlElement]
        public Conjunction? AndOr { get; set; }
        [XmlElement]
        public Parenthesis? FrontParenthesis { get; set; }
        [XmlElement]
        public string Column { get; set; }
        [XmlElement]
        public Operator? Operator { get; set; }
        [XmlElement]
        public string Filter { get; set; }
        [XmlElement]
        public Parenthesis? EndParenthesis { get; set; }
        [XmlElement]
        public bool OrIsNull
        {
            get { return _orIsNull; }
            set
            {
                FrontParenthesis = Parenthesis.BeginningParenthesis;
                _orIsNull = value;
                EndParenthesis = Parenthesis.EndingParenthesis;
            }
        }

        public Criteria()
        {
            //AndOr = null;
            //FrontParenthesis = null;
            //Column = null;
            //Operator = null;
            //Filter = null;
            //EndParenthesis = null;
        }

        //public Criteria(Conjunctions? AndOr = null, string FrontParenthesis = "", string Column = "", 
        //    string Operator = "", string Filter = "", string EndParenthesis = "", bool Locked = false)
        //{
        //    this.AndOr = AndOr;
        //    this.FrontParenthesis = FrontParenthesis;
        //    this.Column = Column;
        //    this.Operator = Operator;
        //    this.Filter = Filter;
        //    this.EndParenthesis = EndParenthesis;
        //}

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
            var modifiedFilter = (FilterIsSubQuery()) ? $"({Filter})" : Filter;

            if (OrIsNull && Filter != null)
            {
                return $" {AndOr} {FrontParenthesis}{Column} {Utility.GetDisplayName(Operator)} {modifiedFilter} OR {Column} IS NULL {EndParenthesis} ";
            }
            else if (OrIsNull && Filter == null)
            {
                return $" {AndOr} {Column} IS NULL ";
            }
            else
            {
                return $" {AndOr} {FrontParenthesis}{Column} {Utility.GetDisplayName(Operator)} {modifiedFilter}{EndParenthesis} ";
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool FilterIsSubQuery()
        {
            if (Filter == null) return false;

            if (Filter.Length >= 6)
            {
                return (Filter.Substring(0, 6).ToLower() == "select") ? true : false;
            }
            return false;
        }

    }
}
