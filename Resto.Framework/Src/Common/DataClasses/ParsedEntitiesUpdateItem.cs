using System;

namespace Resto.Framework.Data
{
    public sealed class ParsedEntitiesUpdateItem
    {
        private Guid id;
        private string type;
        private PersistedEntity entity;
        private bool deleted;

        public ParsedEntitiesUpdateItem() { }

        public ParsedEntitiesUpdateItem(Guid id, string type, PersistedEntity entity, bool deleted)
        {
            this.id = id;
            this.type = type;
            this.entity = entity;
            this.deleted = deleted;
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public PersistedEntity Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        /// <summary>
        /// Сущность физически удалена на сервере.
        /// </summary>
        /// <remarks>
        /// Не путать с логическим удалением, когда свойство <see cref="Deleted"/> обёртки равно <c>false</c>,
        /// но свойство <see cref="PersistedEntity.Deleted"/> сущности равно <c>true</c>.
        /// </remarks>
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
    }
}