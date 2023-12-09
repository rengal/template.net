using System;
using System.Collections;
using System.Collections.Generic;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    public class EntityList<TEntity> : ICollection<TEntity> where TEntity : Entity
    {
        private readonly HashSet<TEntity> entities = new HashSet<TEntity>(EntityEqualityComparer.Default);

        public void Add([NotNull] TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (!entities.Add(entity))
                throw new ArgumentException("Entity " + entity.GetType().Name + " with this id already exists: " + entity.Id);
        }

        public bool Remove(TEntity entity)
        {
            return entities.Remove(entity);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        public void Clear()
        {
            entities.Clear();
        }

        public bool Contains(TEntity item)
        {
            return entities.Contains(item);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            entities.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return entities.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}