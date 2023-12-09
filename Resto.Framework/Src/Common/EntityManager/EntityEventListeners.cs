using System;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    public enum EntityEventType
    {
        /// <summary>
        /// Тип события "объект создан".
        /// </summary>
        CREATED,
        /// <summary>
        /// Тип события "объект изменен".
        /// </summary>
        UPDATED,
        /// <summary>
        /// Тип события "объект удален".
        /// </summary>
        REMOVED,
        /// <summary>
        /// Тип события "объект откатили".
        /// </summary>
        ROLLBACK,
        /// <summary>
        /// Тип события "объект освобожден".
        /// </summary>
        RELEASED
    }

    /// <summary>
    /// Вызывается после обновления объектов с сервера
    /// </summary>
    public delegate void EntitiesUpdateFinished();

    /// <summary>
    /// Вызывается при создании, обновлении или удалении объекта.
    /// </summary>
    /// <param name="entity">Созданный, обновленный, или измененный объект</param>
    /// <param name="eventType">Тип события</param>
    public delegate void EntityEventListener(IEntity entity, EntityEventType eventType);

    /// <summary>
    /// Вызывается при создании, обновлении или удалении объекта.
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <param name="entity">Созданный, обновленный, или измененный объект</param>
    /// <param name="eventType">Тип события</param>
    public delegate void EntityEventListener<T>(T entity, EntityEventType eventType) where T : PersistedEntity;

    /// <summary>
    /// Нетипизированная завертка над EntityEventListener (для хранения в Dictionary)
    /// </summary>
    public interface IEntityEventListenersNoGen
    {
        void Invoke(PersistedEntity entity, EntityEventType eventType);
    }

    /// <summary>
    /// Реализация нетипизированной завертки над EntityEventListener
    /// </summary>
    public class EntityEventListenersNoGenImpl : IEntityEventListenersNoGen
    {
        private readonly EntityEventListener listener;

        public EntityEventListenersNoGenImpl([NotNull] EntityEventListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            this.listener = listener;
        }

        public void Invoke(PersistedEntity entity, EntityEventType eventType)
        {
            listener(entity, eventType);
        }

        public override bool Equals(object obj)
        {
            var other = obj as EntityEventListenersNoGenImpl;
            return other != null && listener.Equals(other.listener);
        }

        public override int GetHashCode()
        {
            return listener.GetHashCode();
        }
    }  
    
    /// <summary>
    /// Реализация типизированной завертки над EntityEventListener
    /// </summary>
    public class EntityEventListenersNoGenImpl<T> : IEntityEventListenersNoGen where T : PersistedEntity
    {
        private readonly EntityEventListener<T> listener;

        public EntityEventListenersNoGenImpl([NotNull] EntityEventListener<T> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            this.listener = listener;
        }

        public void Invoke(PersistedEntity entity, EntityEventType eventType)
        {
            listener((T)entity, eventType);
        }

        public override bool Equals(object obj)
        {
            var other = obj as EntityEventListenersNoGenImpl<T>;
            return other != null && listener.Equals(other.listener);
        }

        public override int GetHashCode()
        {
            return listener.GetHashCode();
        }
    }
}