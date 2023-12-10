using System.Collections.Generic;
using Resto.Framework.Data;

namespace Resto.Data
{
    public sealed partial class EventTypeMetadata
    {
        [Transient]
        private EventTypeMetadata parent;
        [Transient]
        private bool isAbstract;
        [Transient]
        private readonly List<EventAttributeMetadata> inheritedOptionalAttributes = new List<EventAttributeMetadata>();
        [Transient]
        private readonly List<EventAttributeMetadata> inheritedMandatoryAttributes = new List<EventAttributeMetadata>();
        [Transient]
        private readonly List<EventAttributeMetadata> implementedAttributes = new List<EventAttributeMetadata>();

        public EventTypeMetadata Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public bool IsAbstract
        {
            get { return isAbstract; }
            set { isAbstract = value; }
        }

        // опциональные атрибуты унаследованные от родителя
        public IList<EventAttributeMetadata> InheritedOptionalAttributes
        {
            get { return inheritedOptionalAttributes; }
        }

        // обязательные атрибуты унаследованные от родителя
        public IList<EventAttributeMetadata> InheritedMandatoryAttributes
        {
            get { return inheritedMandatoryAttributes; }
        }

        // атрибуты реализованные в типе и его родителях
        public IList<EventAttributeMetadata> ImplementedAttributes
        {
            get { return implementedAttributes; }
        }
    }
}