using System.Collections.Generic;
using System.Linq;

namespace Resto.Framework.Common.Helpers
{
    public static class BooleanHelper
    {
        public static bool And(this IEnumerable<bool> vars)
        {
            return vars.All(var => var);
        }

        public static bool Or(this IEnumerable<bool> vars)
        {
            if (!vars.Any())
            {
                return true;
            }
            return vars.Any(var => var);
        }
    }
}