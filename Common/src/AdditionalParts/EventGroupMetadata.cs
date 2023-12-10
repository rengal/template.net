using System.Collections.Generic;
using Resto.Framework.Data;

namespace Resto.Data
{
    public sealed partial class EventGroupMetadata
    {
        [Transient]
        private readonly List<EventAttributeMetadata> inheritedAttributes = new List<EventAttributeMetadata>();

        // атрибуты унаследованные от родителя
        public IList<EventAttributeMetadata> InheritedAttributes
        {
            get { return inheritedAttributes; }
        }
    }
}