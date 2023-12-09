using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.XmlSerialization;

namespace Resto.Framework.Data
{
    public class Entity : IEntity
    {
        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public Entity()
        {
            id = Guid.NewGuid();
        }

        public Entity(Guid id)
        {
            this.id = id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var entity = obj as Entity;
            if (entity == null)
                return false;
            return id.Equals(entity.id);
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType().Name, Id);
        }

        public Entity DeepClone(Guid newId)
        {
            var newEntity = Serializer.DeepClone(this);
            newEntity.id = newId;
            return newEntity;
        }
    }

    public static class EntityExtensions
    {
        public static Guid IdOrDefault([CanBeNull] this IEntity entity)
        {
            return entity != null ? entity.Id : Guid.Empty;
        }
    }

    public sealed class EntityEqualityComparer<TEntity> : IEqualityComparer<TEntity>
        where TEntity : IEntity
    {
        public static readonly IEqualityComparer<TEntity> Default = new EntityEqualityComparer<TEntity>();

        private EntityEqualityComparer()
        { }

        public bool Equals(TEntity x, TEntity y)
        {
            if (x == null)
                return y == null;
            if (y == null)
                return false;

            return GuidComparer.Default.Equals(x.Id, y.Id);
        }

        public int GetHashCode(TEntity entity)
        {
            return entity == null ? 0 : GuidComparer.Default.GetHashCode(entity.Id);
        }
    }

    public static class EntityEqualityComparer
    {
        public static readonly IEqualityComparer<IEntity> Default = EntityEqualityComparer<IEntity>.Default;
    }
}