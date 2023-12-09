using System.Collections.Generic;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class ByValueListSerializer<T> : BaseCollectionSerializer<ByValueList<T>, T> where T : PersistedEntity
    {
        protected override bool ByValue
        {
            get { return true; }
        }
    }

    internal sealed class ArbitraryGenericCollectionSerializer<T, TItem> : BaseCollectionSerializer<T, TItem> where T : class, ICollection<TItem>, new()
    {
        protected override bool ByValue
        {
            get { return false; }
        }
    }
}