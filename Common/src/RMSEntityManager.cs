using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Resto.Common;
using Resto.Common.Properties;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.XmlSerialization;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Data;
using Resto.Framework.Xml;

namespace Resto.Data
{
    public interface IRMSEntityManager : IBaseEntityManager
    {
        event EventHandler EntitiesLoaded;
        event EventHandler OnReset;
        ParsedEntitiesUpdate LoadEntites(DeserializationContext context);
        ParsedEntitiesUpdate ParseUpdate(EntitiesUpdate update, DeserializationContext context);
    }

    public class RMSEntityManager : BaseEntityManager, IRMSEntityManager
    {
        private string serverUrl;
        private ServerFingerPrintsInfo serverFingerPrints;
        public event EventHandler EntitiesLoaded;
        public event EventHandler OnReset = delegate { };

        #region Constructors

        public RMSEntityManager()
            : this(true)
        {
        }

        public RMSEntityManager(bool suppressPersistentEntityCheck)
        {
            // если проверка не подавляется и в настроках конфига CheckObjectInCacheForConverter = true
            // устанавливается делегат, который обрабатывает ситуацию, когда объекта нет в кэше
            if (!suppressPersistentEntityCheck && CommonConfig.Instance.CheckObjectInCacheForConverter)
            {
                Serializer.OverridePersistedEntityCheck(RaiseExceptionIfEntityNotInCache);
            }

            if (CommonConfig.Instance.CanArchiveCache)
            {
                EntitiesCacheArchiver.Instance.ProcessArchiveCache(true);
            }
        }

        #endregion Constructors

        /// <summary>
        /// Имя файла кэша.
        /// </summary>
        public static string CacheFileName => "entities_cache.bn";

        /// <summary>
        /// Папка с кэшем.
        /// </summary>
        public static string CacheFolderName => "EntitiesCache";

        /// <summary>
        /// Путь к файлу кэша.
        /// </summary> 
        /// <remarks>
        /// При очистке кэша  выполняется приведение и в этом случае кэш
        /// используется не в виде инстанса,
        /// поэтому есть это свойство и метод <see cref="GetCacheFilePath"/>
        /// В интеграции наследуются от <see cref="RMSEntityManager"/>,
        /// поэтому свойство virtual.
        /// </remarks>
        public virtual string CacheFilePath => GetCacheFilePath();

        /// <summary>
        /// Путь к файлу кэша.
        /// </summary>
        public static string GetCacheFilePath()
        {
            var dbId = ServerFingerPrintsContainer.Instance.ServerFingerPrintsInfo?.DbId.ToString() ?? string.Empty;
            return Path.Combine(CommonConfig.Instance.HomePath,
                CacheFolderName,
                dbId,
                CacheFileName);
        }

        public override bool ResetForReloadIfNeed()
        {
            var isCacheForCurrentServer = Equals(ServerFingerPrintsContainer.Instance.ServerFingerPrintsInfo, serverFingerPrints) &&
                                          CommonConfig.Instance.ServerUrl == serverUrl;
            if (isCacheForCurrentServer)
            {
                return false;
            }

            Reset();
            return true;
        }

        public override void SaveEntities()
        {
            FileStream fileStream = null;
            try
            {
                if (!ServerFingerPrintsContainer.Instance.HasFingerPrintsInfo)
                {
                    Log.Info("There are no server fingerprints. Cache won't be saved");
                    return;
                }

                if (entities.Count > 0 && 
                    EntitiesUpdateRevision >= 0 && 
                    serverInstanceId != null)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    Log.Info($"Saving entities to cache: {CacheFilePath}");
                    var update = new EntitiesUpdate(serverInstanceId, EntitiesUpdateRevision, true) { Version = VersionInfo.INTERNAL_VERSION };
                    foreach (var pair in entities)
                    {
                        if (IsServerSideEntity(pair.Value))
                        {
                            update.Items.Add(
                                new EntitiesUpdateItem(
                                    pair.Key,
                                    EntitiesRegistryBase.GetClassName(pair.Value.GetType()),
                                    pair.Value,
                                    false));
                        }
                    }

                    var cacheFolder = Path.GetDirectoryName(CacheFilePath);
                    if (cacheFolder != null && !Directory.Exists(cacheFolder))
                    {
                        Directory.CreateDirectory(cacheFolder);
                    }

                    fileStream = new FileStream(CacheFilePath, FileMode.OpenOrCreate);
                    Serializer.SerializeToDocument(fileStream, update, "root");
                    sw.Stop();
                    Log.Info("Elapsed: " + sw.ElapsedMilliseconds + "ms.");
                }
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
            finally
            {
                // Объявление нового потока в using не закроет его в случае исключения, закрываем явно
                fileStream?.Dispose();
            }
        }

        public override ParsedEntitiesUpdate LoadEntites()
        {
            return LoadEntites(null);
        }

        [CanBeNull]
        public virtual ParsedEntitiesUpdate LoadEntites(DeserializationContext context)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                Log.Info($"Trying to read entities from cache: {CacheFilePath}");
                if (!File.Exists(CacheFilePath))
                {
                    Log.Info("File doesn't exists");
                    return null;
                }

                if (!ServerFingerPrintsContainer.Instance.HasFingerPrintsInfo)
                {
                    Log.Info("There are no server fingerprints. Cache won't be loaded");
                    return null;
                }

                using (var stream = new FileStream(CacheFilePath, FileMode.Open))
                {
                    var xml = Serializer.Deserialize<EntitiesUpdate>(stream, true);
                    if (xml.Version != VersionInfo.INTERNAL_VERSION)
                    {
                        Log.Warn($"Cache xml version {xml.Version} differs from internal version {VersionInfo.INTERNAL_VERSION}." +
                                 "No entities loaded.");
                        return null;
                    }
                    var update = ParseUpdate(xml, context);
                    serverFingerPrints = ServerFingerPrintsContainer.Instance.ServerFingerPrintsInfo;
                    serverUrl = CommonConfig.Instance.ServerUrl;
                    sw.Stop();
                    Log.Info(update.Items.Count + " were loaded from cache file. Elapsed " + sw.ElapsedMilliseconds + "ms.");
                    return update;
                }
            }
            catch (Exception e)
            {
                Log.Info("Cache is invalid", e);
            }

            return null;
        }

        protected virtual DeserializationContext GetUpdateEntitiesCache()
        {
            return new DeserializationContext(this, true);
        }

        public ParsedEntitiesUpdate ParseUpdate(EntitiesUpdate update, DeserializationContext context)
        {
            if (context == null)
                context = GetUpdateEntitiesCache();

            var parsedUpdate = new ParsedEntitiesUpdate(update.NullableServerInstanceId, update.Revision, update.FullUpdate);

            foreach (EntitiesUpdateItem item in update.Items)
            {
                Type entityType = EntitiesRegistryBase.GetType(item.Type);
                try
                {
                    if (item.Deleted)
                    {
                        if (!entities.ContainsKey(item.Id))
                        {
                            Log.Error("Deleted entity not found, skipping: " + item.Type + "@" + item.Id);
                            continue;
                        }
                        parsedUpdate.Items.Add(new ParsedEntitiesUpdateItem(item.Id, item.Type, entities[item.Id], true));
                        continue;
                    }

                    if (item.Entity != null)
                    {
                        parsedUpdate.Items.Add(new ParsedEntitiesUpdateItem(item.Id, item.Type, item.Entity, false));
                        continue;
                    }

                    if (item.Xml == null)
                    {
                        throw new RestoException("Entity is not deleted, but both entity and xml fields are null: " + item.Type + "@" + item.Id);
                    }

                    PersistedEntity entity;
                    using (var reader = new XmlTextReader(new StringReader(item.Xml)))
                    {
                        entity = (PersistedEntity)Serializer.Deserialize(entityType, reader, context, true);
                    }
                    parsedUpdate.Items.Add(new ParsedEntitiesUpdateItem(item.Id, item.Type, entity, false));
                }
                catch (Exception ex)
                {
                    Log.Warn("Can't decode entity of type: " + entityType.FullName + " id: " + item.Id, ex);
                    throw;
                }
            }

            foreach (var entity in context.UninitializedEntities.Cast<PersistedEntity>())
            {
                //TODO Заполнять отсутствующие entity значениями по умолчанию
                entity.Deleted = true;
                Log.Warn("Entity was missed in update: " + entity.Id + " " + entity.GetType());
            }

            return parsedUpdate;
        }

        public override ParsedEntitiesUpdate ParseUpdate(EntitiesUpdate update)
        {
            return ParseUpdate(update, null);
        }

        public override bool OnDataUpdate(ParsedEntitiesUpdate update)
        {
            try
            {
                return ProcessUpdateInternal(update);
            }
            finally
            {
                if (update.FullUpdate)
                {
                    FireEntitesLoaded();
                }
            }
        }

        public override void OnResetInternal()
        {
            base.OnResetInternal();
            OnReset(this, EventArgs.Empty);
        }

        private void FireEntitesLoaded()
        {
            EntitiesLoaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Кидает исключение если объект не содержится кэше.
        /// </summary>
        /// <param name="entity"></param>
        private static void RaiseExceptionIfEntityNotInCache(PersistedEntity entity)
        {
            if (!Framework.Data.EntityManager.INSTANCE.Contains(entity.Id))
            {
                string name = entity is LocalizableNamePersistedEntity
                                  ? Convert.ToString(entity.GetType().GetProperty("Name").GetValue(entity, null))
                                  : Convert.ToString(entity);
                throw new NotInCacheException(string.Format(Resources.ObjectNotInCacheErrorCaption, name, entity.Id));
            }
        }
    }
}