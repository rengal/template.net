using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.Extensions
{
    public static class ConceptionExtensions
    {
        /// <summary>
        /// Упорядочивает концепции по имени, помещая "Без концепции" в начало последовательности 
        /// </summary>
        [NotNull]
        [Pure]
        public static IEnumerable<Conception> OrderByNameLocalNoConceptionAware([NotNull] this IEnumerable<Conception> items)
        {
            return items.OrderByDescending(item => item.IsNoConception)
                .ThenBy(item => item.NameLocal);
        }
    }
}
