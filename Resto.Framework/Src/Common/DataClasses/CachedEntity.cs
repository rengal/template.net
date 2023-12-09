using System;

namespace Resto.Framework.Data
{
    [DataClass("CachedEntity")]
    public class CachedEntity : Entity
    {
        [HasDefaultValue]
        private int revision;
        [HasDefaultValue]
        private Guid? lastModifyNode;

        public CachedEntity() { }

        public CachedEntity(Guid id)
            : base(id)
        {}

        public int Revision
        {
            get { return revision; }
            set { revision = value; }
        }

        public Guid? LastModifyNode
        {
            get { return lastModifyNode; }
            set { lastModifyNode = value; }
        }
    }
}