using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class FilterExcludeValuesCriteria
    {
        public FilterExcludeValuesCriteria()
        {
        }

        public FilterExcludeValuesCriteria([NotNull] IEnumerable<object> values)
            : base(values)
        {
        }

        public FilterExcludeValuesCriteria(object singleValue)
            : base(singleValue)
        {
        }
    }
}