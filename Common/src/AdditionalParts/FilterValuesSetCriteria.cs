using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class FilterValuesSetCriteria
    {
        protected FilterValuesSetCriteria()
        {
        }

        protected FilterValuesSetCriteria([NotNull] IEnumerable<object> values)
        {
            Values = new HashSet<object>(values);
        }

        protected FilterValuesSetCriteria(object singleValue)
            : this(singleValue.AsSequence())
        {
        }

        public override bool IsValueChecked(object theValue)
        {
            return Values.Contains(theValue);
        }
    }
}
