using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.SqlGenerators;
using QueryBuilder.Exceptions;

namespace QueryBuilder.Utilities
{
    public class Interpolator
    {
        private string _formattable;
        private IList<Criteria> _criteria;

        public Interpolator() { }

        public Interpolator(string formattable, IList<Criteria> criteria)
        {
            _formattable = formattable;
            _criteria = criteria;
        }

        public Interpolator SetFormattable(string formattable)
        {
            _formattable = formattable;
            return this;
        }

        public Interpolator SetCriteria(IList<Criteria> criteria)
        {
            _criteria = criteria;
            return this;
        }

        public bool NeedsInterpolation()
        {
            return (_formattable.Contains('{') && _formattable.Contains('}')) ? true : false;
        }

        public string Interpolate()
        {
            var clonedFormattable = string.Copy(_formattable);

            // While the formattable string still contains placeholders
            while (clonedFormattable.Contains('{') && clonedFormattable.Contains('}'))
            {
                // make fragment variable of first interpolated fragment
                var startMark = clonedFormattable.IndexOf('{');
                var length = clonedFormattable.IndexOf('}') - startMark + 1;
                var fragment = clonedFormattable.Substring(startMark, length).ToLower();

                // loop through Criteria objects' Column property
                bool foundMatch = false;
                var i = 0;
                Criteria[] matchingCriteria = new Criteria[_criteria.Count];
                foreach (var criteria in _criteria)
                {
                    // if Property value = fragment, then add criteria object to list of matching criteria
                    if (criteria.Column.ToLower() == fragment)
                    {
                        foundMatch = true;
                        matchingCriteria[i] = criteria;
                        i++;
                    }
                }

                // if no Property matches, throw exception
                if (!foundMatch)
                {
                    throw new FragmentNotFoundException(fragment + " was not found");
                }
                else
                {
                    // Build new string to replace placeholder
                    var replacementString = "";
                    foreach (var criteria in matchingCriteria)
                    {
                        replacementString += criteria.ToString();
                    }

                    clonedFormattable = clonedFormattable.Replace(fragment, replacementString);
                }
            }

            if (clonedFormattable.Contains('{') && clonedFormattable.Contains('}'))
            {
                return null;
            }
            else
            {
                return clonedFormattable;
            }

        }
    }
}
