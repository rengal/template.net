using System;
using Resto.Framework.Common.XmlSerialization;

namespace Resto.Framework.Data
{
    public class PersistedEntity : CachedEntity, IRootEntity, IDeletable
    {
        private bool deleted;

        private DateTime created;
        private DateTime modified;
        private DateTime deletedDate;

        private long localId = -1;

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        public long LocalId
        {
            get { return localId; }
            set { localId = value; }
        }

        public PersistedEntity()
        {
            var now = DateTime.Now;
            created = now;
            modified = now;
        }

        public PersistedEntity(Guid id)
            : base(id)
        {
            var now = DateTime.Now;
            created = now;
            modified = now;
        }

        public override string ToString()
        {
            return Serializer.ObjectToString(this);
        }

        public bool IsSavedEntity
        {
            get { return EntityManager.INSTANCE.Contains(Id); }
        }

        public PersistedEntity CreateCopy()
        {
            return EntityManager.INSTANCE.CreateDetachedCopy(this);
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public DateTime Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        public DateTime DeletedDate
        {
            get { return deletedDate; }
            set { deletedDate = value; }
        }

        public virtual void AfterDeserialization()
        {}
    }
}
