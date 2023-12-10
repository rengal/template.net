using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    public static class PersistedEntityCollectionExtention
    {
        public static ReadOnlyCollection<T> ContainsInCache<T>(this ICollection<T> collection) where T : PersistedEntity
        {
            return collection
                .Where(entity => entity != null && EntityManager.INSTANCE.Contains(entity.Id))
                .ToList()
                .AsReadOnly();
        }

        [NotNull]
        [Pure]
        public static List<T> OrderByNameLocal<T>([NotNull] this IEnumerable<T> items)
            where T : PersistedEntity, INamed
        {
            return items.OrderBy(item => item.NameLocal).ToList();
        }
    }
}