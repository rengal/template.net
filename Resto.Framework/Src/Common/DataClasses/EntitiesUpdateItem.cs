using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    [DataClass("EntitiesUpdateItem")]
    public class EntitiesUpdateItem
    {
        private readonly Guid id;
        private readonly string type;
        private readonly string xml;

        /// <summary>
        /// Сырой объект, как замена сериализованного значения, для уменьшения накладных расходов.
        /// Имя поля "r" такое, что бы использовать значения из XMLConverter.java
        /// </summary>
        private readonly ByValue<PersistedEntity> r = null;
        private readonly bool deleted;

        [UsedImplicitly] // используется десериализатором
        private EntitiesUpdateItem() { }

        public EntitiesUpdateItem(Guid id, string type, PersistedEntity entity, bool deleted)
        {
            this.id = id;
            this.type = type;
            r = new ByValue<PersistedEntity>(entity);
            this.deleted = deleted;
        }

        public EntitiesUpdateItem(Guid id, string type, string xml, bool deleted)
        {
            this.id = id;
            this.type = type;
            this.xml = xml;
            this.deleted = deleted;
        }

        public Guid Id
        {
            get { return id; }
        }

        public string Type
        {
            get { return type; }
        }

        public string Xml
        {
            get { return xml; }
        }

        public PersistedEntity Entity
        {
            get { return r != null ? r.Value : null; }
        }

        public bool Deleted
        {
            get { return deleted; }
        }
    }
}