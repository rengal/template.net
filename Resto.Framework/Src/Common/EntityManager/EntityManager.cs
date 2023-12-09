using System;
using System.Diagnostics;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    public static class EntityManager
    {
        [CanBeNull]
        public static IEntitiesProviderFactory EntitiesProviderFactory
        {
            get; set;
        }

        [CanBeNull]
        private static IEntitiesProvider entitiesProvider;

        public static void ResetInstance()
        {
            if (EntitiesProviderFactory == null)
                throw new InvalidOperationException("Entity provider factory has not been set.");

            entitiesProvider = EntitiesProviderFactory.Create();
        }

        [NotNull]
        public static IEntitiesProvider EntitiesProvider
        {
            get
            {
                if (entitiesProvider == null)
                    ResetInstance();

                Debug.Assert(entitiesProvider != null);

                return entitiesProvider;
            }
        }

        [NotNull]
        public static IBaseEntityManager INSTANCE
        {
            get
            {
                if (entitiesProvider == null)
                    ResetInstance();

                Debug.Assert(entitiesProvider != null);

                return entitiesProvider.EntityManager;
            }
        }
    }
}