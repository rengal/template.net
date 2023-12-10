using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    public partial class FilterSingleValueCriteria
    {
        public override bool IsValueChecked(object theValue)
        {
            return Value == null ? theValue == null : Value.Equals(theValue);
        }
    }
}
