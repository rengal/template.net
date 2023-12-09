using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization;
using log4net;
using Resto.Framework.Common;

namespace Resto.Framework.Data
{
    public interface IBaseEntityManager : IEntitiesProvider
    {
        /// <summary>
        /// Текущая версия PE
        /// </summary>
        int EntitiesUpdateRevision { get; }

        /// <summary>
        /// Очищает EM, выставляет ревизию в -1
        /// </summary>
        void Reset();

        /// <summary>
        /// Очищает EM, если нужно перезагрузить данные для другого сервера.
        /// </summary>
        /// <returns>Возвращает <c>true</c>, если ЕМ был очищен, иначе - <c>false</c></returns>
        bool ResetForReloadIfNeed();

        /// <summary>
        /// Сохраняте PE в кэш
        /// </summary>
        void SaveEntities();

        /// <summary>
        /// Загружает PE из кэша
        /// </summary>
        /// <returns>Весь кэш PE одним объектом</returns>
        [CanBeNull]
        ParsedEntitiesUpdate LoadEntites();

        /// <summary>
        /// Разбирает обновление, пришедшее с сервера
        /// </summary>
        /// <param name="update">Апдейт</param>
        /// <returns>Разобранные PE</returns>
        [CanBeNull]
        ParsedEntitiesUpdate ParseUpdate(EntitiesUpdate update);

        /// <summary>
        /// Обработка разробранного обновление пришедшего с сервера или загруженного из кэша
        /// </summary>
        /// <param name="update">Разобранный апдейт</param>
        /// <returns>Успешно ли был обработан апдейт</returns>
        bool OnDataUpdate(ParsedEntitiesUpdate update);

        /// <summary>
        /// Содержет ли EM PE c таким id
        /// </summary>
        /// <param name="id">GUID PE</param>
        /// <returns></returns>
        bool Contains(Guid id);

        /// <summary>
        /// Содержет ли EM PE c таким id типа T
        /// </summary>
        /// <typeparam name="T">Тип PE</typeparam>
        /// <param name="id">GUID PE</param>
        /// <returns></returns>
        bool Contains<T>(Guid id) where T : PersistedEntity;
        /// <summary>
        /// Содежит ли PE этот объект
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Contains(PersistedEntity entity);

        /// <summary>
        /// Получение нетипизированного объекта из EM
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="RestoException">Если был передан Id объекта, который отсутствует в кэше</exception>
        /// <returns></returns>
        [NotNull]
        PersistedEntity Get(Guid id);

        /// <summary>
        /// Получение объекта из EM по идентификатору и типу
        /// </summary>
        /// <exception cref="KeyNotFoundException">Если был передан Id объекта, который отсутствует в кэше</exception>
        /// <typeparam name="T">Тип PE</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get<T>(Guid id) where T : PersistedEntity;
        ReadOnlyCollection<TEntity> GetAll<TEntity>() where TEntity : PersistedEntity;
        ReadOnlyCollection<TEntity> GetAllNotDeleted<TEntity>() where TEntity : PersistedEntity;
        ReadOnlyCollection<TEntity> GetAll<TEntity>(Func<TEntity, bool> filter) where TEntity : PersistedEntity;
        ReadOnlyCollection<TEntity> GetAllNotDeleted<TEntity>(Func<TEntity, bool> filter) where TEntity : PersistedEntity;
        [CanBeNull]
        TEntity GetSingleton<TEntity>() where TEntity : PersistedEntity;
        [CanBeNull]
        TEntity GetSingleton<TEntity>(bool raiseExceptionOnIncorrectState) where TEntity : PersistedEntity;
        TEntity CreateDetachedCopy<TEntity>(TEntity mainEntity) where TEntity : PersistedEntity;
        void CallEntityEvent(PersistedEntity entity, EntityEventType eventType);

        /// <summary>
        /// Добавление подписки на события изменения всех объектов типа PersistedEntity
        /// </summary>
        /// <param name="type"></param>
        /// <param name="listener"></param>
        void AddListener(Type type, EntityEventListener listener);

        /// <summary>
        /// Удаление подписки на события изменения всех объектов типа PersistedEntity
        /// </summary>
        /// <param name="type"></param>
        /// <param name="listener"></param>
        void RemoveListener(Type type, EntityEventListener listener);

        /// <summary>
        /// Добавление подписки на события изменения всех объектов типа
        /// </summary>
        /// <typeparam name="T">Тип объектов</typeparam>
        /// <param name="listener"></param>
        void AddListener<T>(EntityEventListener<T> listener) where T : PersistedEntity;
        /// <summary>
        /// Удаление подписки на события изменения всех объектов типа
        /// </summary>
        /// <typeparam name="T">Тип объектов</typeparam>
        /// <param name="listener"></param>
        void RemoveListener<T>(EntityEventListener<T> listener) where T : PersistedEntity;
    }

    public abstract class BaseEntityManager : IBaseEntityManager, IRootEntitiesProvider<PersistedEntity>
    {
        protected static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(BaseEntityManager));

        #region Data Members
        protected readonly ReaderWriterLockSlim accessLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private int entitiesUpdateRevision = -1;
        protected Guid? serverInstanceId;

        protected readonly Dictionary<Guid, PersistedEntity> entities = new Dictionary<Guid, PersistedEntity>(GuidComparer.Default);
        private readonly Dictionary<Type, HashSet<IEntityEventListenersNoGen>> listenersByType = new Dictionary<Type, HashSet<IEntityEventListenersNoGen>>();
        protected readonly Dictionary<Type, EntityList<PersistedEntity>> entitiesByType = new Dictionary<Type, EntityList<PersistedEntity>>();

        protected static readonly Cache<Type, Type> rootTypesCache = new Cache<Type, Type>(CalculateRootType); 
        #endregion Data Members

        #region Methods
        public void Reset()
        {
            accessLock.EnterWriteLock();
            try
            {
                entities.Clear();
                entitiesByType.Clear();

                ResetInternal();
            }
            finally
            {
                accessLock.ExitWriteLock();
            }
            OnResetInternal();
        }

        public virtual bool ResetForReloadIfNeed()
        {
            return false;
        }

        public virtual void OnResetInternal()
        {
        }

        public int EntitiesUpdateRevision
        {
            get { return entitiesUpdateRevision; }
            protected set { entitiesUpdateRevision = value; }
        }

        public virtual void SaveEntities()
        {}

        public virtual ParsedEntitiesUpdate LoadEntites()
        {
            return null;
        }

        public virtual ParsedEntitiesUpdate ParseUpdate(EntitiesUpdate update)
        {
            return null;
        }

        public abstract bool OnDataUpdate(ParsedEntitiesUpdate update);
        #endregion Methods

        #region Implementation
        private void ResetInternal()
        {
            EntitiesUpdateRevision = -1;
            serverInstanceId = null;
        }
        #endregion Implementation

        #region Entities
        /// <summary>
        /// Приходит ли entity в апдейтах с сервера или создается локально
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>true - entity приходит в апдейтах с сервера / 
        /// false - entity не приходит в апдейтах с сервера (создается локально)</returns>
        protected virtual bool IsServerSideEntity(Entity entity)
        {
            return true;
        }

        public bool Contains(Guid id)
        {
            accessLock.EnterReadLock();
            try
            {
                return entities.ContainsKey(id);
            }
            finally
            {
                accessLock.ExitReadLock();
            }
        }

        public bool Contains<T>(Guid id) where T : PersistedEntity
        {
            accessLock.EnterReadLock();
            try
            {
                PersistedEntity entity;
                if (!entities.TryGetValue(id, out entity))
                {
                    return false;
                }
                return entity is T;
            }
            finally
            {
                accessLock.ExitReadLock();
            }
        }

        public bool Contains(PersistedEntity entity)
        {
            accessLock.EnterReadLock();
            try
            {
                PersistedEntity existingEntity;
                return entities.TryGetValue(entity.Id, out existingEntity) && ReferenceEquals(existingEntity, entity);
            }
            finally
            {
                accessLock.ExitReadLock();
            }
        }

        PersistedEntity IRootEntitiesProvider<PersistedEntity>.TryGet(Guid id)
        {
            accessLock.EnterReadLock();
            try
            {
                return entities.GetOrDefault(id);
            }
            finally
            {
                accessLock.ExitReadLock();
            }
        }

        PersistedEntity IRootEntitiesProvider<PersistedEntity>.Get(Guid id)
        {
            return Get(id);
        }

        [NotNull]
        public virtual PersistedEntity Get(Guid id)
        {
            accessLock.EnterReadLock();
            try
            {
                PersistedEntity entity;
                if (!entities.TryGetValue(id, out entity))
                    throw new RestoException("Entity with given id was not found: " + id);

                return entity;
            }
            finally
            {
                accessLock.ExitReadLock();
            }
        }

        public virtual T Get<T>(Guid id) where T : PersistedEntity
        {
            accessLock.EnterReadLock();
            try
            {
                return (T)entities[id];
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(string.Format("Can't find entity of type {0}, id: {1}", typeof(T), id), ex);
                throw;
            }
            finally
            {
                accessLock.ExitReadLock();
            }
        }

        [Pure]
        private static Type CalculateRootType(Type entityType)
        {
            var rootType = entityType;
            while (rootType != null)
            {
                if (rootType.IsDefined(typeof(RootEntityAttribute), false))
                    return rootType;

                rootType = rootType.BaseType;
            }
            throw new RestoException("Class and its superclasses are not annotated with [RootEntity]: " + entityType);
        }

        protected void Add([NotNull] PersistedEntity entity)
        {
            accessLock.EnterWriteLock();
            try
            {
                if (entities.ContainsKey(entity.Id))
                    throw new RestoException("Entity with same id already exists: " + entity.Id);

                var type = rootTypesCache[entity.GetType()];

                EntityList<PersistedEntity> list;
                if (!entitiesByType.TryGetValue(type, out list))
                {
                    list = new EntityList<PersistedEntity>();
                    entitiesByType[type] = list;
                }

                list.Add(entity);

                entities[entity.Id] = entity;
            }
            finally
            {
                accessLock.ExitWriteLock();
            }
        }

        public virtual ReadOnlyCollection<TEntity> GetAll<TEntity>() where TEntity : PersistedEntity
        {
            return GetAll<TEntity>(false, null);
        }

        public virtual ReadOnlyCollection<TEntity> GetAllNotDeleted<TEntity>() where TEntity : PersistedEntity
        {
            return GetAll<TEntity>(true, null);
        }

        public ReadOnlyCollection<TEntity> GetAll<TEntity>(Func<TEntity, bool> filter) where TEntity : PersistedEntity
        {
            return GetAll(false, filter);
        }

        public ReadOnlyCollection<TEntity> GetAllNotDeleted<TEntity>(Func<TEntity, bool> filter) where TEntity : PersistedEntity
        {
            return GetAll(true, filter);
        }

        [NotNull]
        private ReadOnlyCollection<TEntity> GetAll<TEntity>(bool notDeleted, [CanBeNull] Func<TEntity, bool> filter) where TEntity : PersistedEntity
        {
            accessLock.EnterReadLock();
            try
            {
                var entityType = typeof(TEntity);
                var rootType = rootTypesCache[entityType];

                if (!entitiesByType.TryGetValue(rootType, out var matchedRootEntities))
                    return new TEntity[0].AsReadOnly();

                var entitiesForSearch = entityType == rootType
                                        ? matchedRootEntities.Cast<TEntity>()
                                        : matchedRootEntities.OfType<TEntity>();

                if (notDeleted)
                    entitiesForSearch = entitiesForSearch.Where(entity => !entity.Deleted);
                if (filter != null)
                    entitiesForSearch = entitiesForSearch.Where(filter);

                return entitiesForSearch.ToList().AsReadOnly();
            }
            finally
            {
                accessLock.ExitReadLock();
            }
        }

        public TEntity GetSingleton<TEntity>() where TEntity : PersistedEntity
        {
            return GetSingleton<TEntity>(false);
        }

        public TEntity GetSingleton<TEntity>(bool raiseExceptionOnIncorrectState) where TEntity : PersistedEntity
        {
            accessLock.EnterReadLock();
            try
            {
                IList<TEntity> list = GetAll<TEntity>();
                if (list.Count == 0)
                    return null;

                if (raiseExceptionOnIncorrectState && list.Count > 1)
                {
                    var errorStr = string.Format("Too many instances of '{0}': {1}. Must be only one.", typeof(TEntity).Name, list.Count);
                    Log.Error(errorStr);
                    throw new RestoException(errorStr);
                }
                return list[0];
            }
            finally
            {
                accessLock.ExitReadLock();
            }
        }

        protected virtual void RemoveInternal(PersistedEntity entity)
        { }

        protected void Remove(PersistedEntity entity)
        {
            accessLock.EnterWriteLock();
            try
            {
                if (!entities.ContainsKey(entity.Id))
                    throw new RestoException("Entity with give id does not exists: " + entity.Id);

                var list = entitiesByType[rootTypesCache[entity.GetType()]];
                list.Remove(entity);
                entities.Remove(entity.Id);
                RemoveInternal(entity);
            }
            finally
            {
                accessLock.ExitWriteLock();
            }
        }

        public TEntity CreateDetachedCopy<TEntity>(TEntity mainEntity) where TEntity : PersistedEntity
        {
            return Serializer.DeepClone(mainEntity);
        }

        protected bool ProcessUpdateInternal(ParsedEntitiesUpdate update)
        {
            if (EntitiesUpdateRevision == -1 && !update.FullUpdate)
            {
                // При обнуленном репозитории добавляем сущности только по фуллапдейту
                return true;
            }

            if (update.Revision == EntitiesUpdateRevision && update.Items.Count == 0)
            {
                // Если не изменилась ни ревизия, ни один объект, то это пустой update, просто выходим
                return true;
            }

            if (update.Revision <= EntitiesUpdateRevision && update.ServerInstanceId == serverInstanceId)
            {
                Log.Info("Obsolete server update revision " + update.Revision + ". Local timestamp: " + EntitiesUpdateRevision);
                return false;
            }

            // помещать события об изменении объектов будем в список - добавление в него быстрей, чем в коллекцию
            List<KeyValuePair<PersistedEntity, EntityEventType>> entityEvents;

            accessLock.EnterWriteLock();
            try
            {
                entityEvents = new List<KeyValuePair<PersistedEntity, EntityEventType>>(update.Items.Count);

                var updateCounter = 0;
                var createCounter = 0;
                var deleteCounter = 0;

                foreach (var item in update.Items)
                {
                    if (Contains(item.Id))
                    {
                        var oldValue = Get(item.Id);
                        if (item.Deleted)
                        {
                            Remove(oldValue);
                            entityEvents.Add(new KeyValuePair<PersistedEntity, EntityEventType>(oldValue, EntityEventType.REMOVED));
                            deleteCounter++;
                        }
                        else
                        {
                            Serializer.ShallowCopy(item.Entity, oldValue);
                            entityEvents.Add(new KeyValuePair<PersistedEntity, EntityEventType>(oldValue, EntityEventType.UPDATED));
                            updateCounter++;
                        }
                    }
                    else if (!item.Deleted)
                    {
                        Add(item.Entity);
                        entityEvents.Add(new KeyValuePair<PersistedEntity, EntityEventType>(item.Entity, EntityEventType.CREATED));
                        createCounter++;
                    }
                }

                if (update.FullUpdate)
                {
                    // не создаем вспомогательных списков для вычисления разницы между существующими и пришедшими объектами,
                    // т.к. это чревато большим overhead'ом по памяти
                    var entitiesToRemove = entities.Keys
                        .Except(update.Items.Select(entity => entity.Id))
                        .Select(Get)
                        .Where(IsServerSideEntity) // удаляем только Entity, которые присылаются с сервера
                        .ToList();

                    foreach (var entity in entitiesToRemove)
                    {
                        Remove(entity);
                        entityEvents.Add(new KeyValuePair<PersistedEntity, EntityEventType>(entity, EntityEventType.REMOVED));
                        deleteCounter++;
                    }
                }

                EntitiesUpdateRevision = update.Revision;

                if (serverInstanceId != update.ServerInstanceId)
                {
                    /* TODO Кусок кода закоментирован, т.к. из-за него глючит номенклатура после перезагрузки сервера
                                        //Если изменился сервер ID, то сервер сменился и нужно на всякий случай запросить полный update
                                        if (serverInstanceId != null)
                                        {
                                            //Чтобы запросить полный update, ставим ревизию в -1
                                            entitiesUpdateRevision = -1;
                                            Log.Info("Server instance id was changed, resetting revision number");
                                        }
                    */
                    serverInstanceId = update.ServerInstanceId;
                    Log.Info("New server instance id: " + serverInstanceId);
                }

                Log.Info("Created: " + createCounter + " entities. Updated: " + updateCounter + " entities. Deleted: " + deleteCounter + " entities. Revision: " + EntitiesUpdateRevision);

            }
            catch (Exception e)
            {
                Log.Warn("Cannot load update from server", e);

                return false;
            }
            finally
            {
                accessLock.ExitWriteLock();
            }

            Log.Info("Processing listeners");

            foreach (var kvp in entityEvents)
            {
                CallEntityEvent(kvp.Key, kvp.Value);
            }

            return true;
        }

        public void CallEntityEvent(PersistedEntity entity, EntityEventType eventType)
        {
            var listeners = new List<HashSet<IEntityEventListenersNoGen>>();
            accessLock.EnterUpgradeableReadLock();
            try
            {
                for (var entityType = entity.GetType();
                     entityType != null && entityType != typeof(object);
                     entityType = entityType.BaseType)
                {
                    HashSet<IEntityEventListenersNoGen> typeListeners;
                    if (listenersByType.TryGetValue(entityType, out typeListeners))
                        listeners.Add(typeListeners);
                }
            }
            finally
            {
                accessLock.ExitUpgradeableReadLock();
            }

            foreach (var listener in listeners)
            {
                CallEntityListeners(listener, entity, eventType);
            }
        }
        #endregion

        #region Listeners
        public void AddListener(Type type, EntityEventListener listener)
        {
            if (!typeof(PersistedEntity).IsAssignableFrom(type))
                throw new ArgumentOutOfRangeException(nameof(type));

            accessLock.EnterWriteLock();
            try
            {
                if (!listenersByType.ContainsKey(type))
                    listenersByType[type] = new HashSet<IEntityEventListenersNoGen>();

                listenersByType[type].Add(new EntityEventListenersNoGenImpl(listener));
            }
            finally
            {
                accessLock.ExitWriteLock();
            }
        }

        public void RemoveListener(Type type, EntityEventListener listener)
        {
            if (!typeof(PersistedEntity).IsAssignableFrom(type))
                throw new ArgumentOutOfRangeException(nameof(type));

            accessLock.EnterWriteLock();
            try
            {
                if (!listenersByType.ContainsKey(type))
                    return;

                listenersByType[type].Remove(new EntityEventListenersNoGenImpl(listener));
            }
            finally
            {
                accessLock.ExitWriteLock();
            }
        }

        public void AddListener<T>(EntityEventListener<T> listener) where T : PersistedEntity
        {
            accessLock.EnterWriteLock();
            try
            {
                var type = typeof(T);
                if (!listenersByType.ContainsKey(type))
                    listenersByType[type] = new HashSet<IEntityEventListenersNoGen>();

                listenersByType[type].Add(new EntityEventListenersNoGenImpl<T>(listener));
            }
            finally
            {
                accessLock.ExitWriteLock();
            }
        }

        public void RemoveListener<T>(EntityEventListener<T> listener) where T : PersistedEntity
        {
            accessLock.EnterWriteLock();
            try
            {
                var type = typeof(T);
                HashSet<IEntityEventListenersNoGen> listeners;
                if (listenersByType.TryGetValue(type, out listeners))
                    listeners.Remove(new EntityEventListenersNoGenImpl<T>(listener));
            }
            finally
            {
                accessLock.ExitWriteLock();
            }
        }

        private static void CallEntityListeners([NotNull] IEnumerable<IEntityEventListenersNoGen> listeners, PersistedEntity entity, EntityEventType eventType)
        {
            foreach (var listener in listeners.ToList())
            {
                try
                {
                    listener.Invoke(entity, eventType);
                }
                catch (Exception e)
                {
                    Log.ErrorFormat("Exception in listener on entity [{0}], event type [{1}]. Exception: {2}",
                                              entity != null ? (Guid?)entity.Id : null, eventType, e);
                }
            }
        }
        #endregion Listeners

        #region IEntitiesProvider
        public IBaseEntityManager EntityManager
        {
            get { return this; }
        }

        public IRootEntitiesProvider<TRootEntityBaseType> GetEntities<TRootEntityBaseType>() where TRootEntityBaseType : Entity, IRootEntity
        {
            if (typeof(TRootEntityBaseType) == typeof(PersistedEntity))
                return (IRootEntitiesProvider<TRootEntityBaseType>)this;

            throw new NotSupportedException();
        }
        #endregion
    }

    public class MemoryEntityManager : BaseEntityManager
    {
        public override bool OnDataUpdate(ParsedEntitiesUpdate update)
        {
            return true;
        }
    }
}