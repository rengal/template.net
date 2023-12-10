using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class FilterIncludeValuesCriteria
    {
        public FilterIncludeValuesCriteria()
        {
        }

        public FilterIncludeValuesCriteria([NotNull] IEnumerable<object> values)
            : base(values)
        {
        }

        public FilterIncludeValuesCriteria(object singleValue)
            : base(singleValue)
        {
        }
    }
}