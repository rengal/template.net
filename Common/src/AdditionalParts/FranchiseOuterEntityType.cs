using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Common;

namespace Resto.Data
{
    partial class FranchiseOuterEntityType
    {
        public static FranchiseOuterEntityType[] GetDerivedTypes<T>(bool withNull) where T : class
        {
            var values = VALUES.Where(v => typeof(T).IsAssignableFrom(v.EntityClass));
            // if (withNull)
            // {
            //     values = values.Union(EnumerableExtensions.AsSequence<FranchiseOuterEntityType>(null));
            // } //todo debugnow
            return values.ToArray();
        }
    }
}
