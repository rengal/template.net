using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    internal sealed class CopyContext
    {
        #region Fields
        private readonly Dictionary<object, Entity> registeredObjects = new Dictionary<object, Entity>();
        #endregion

        #region Methods
        internal void Register([NotNull] object obj, [NotNull] Entity entity)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (registeredObjects.ContainsKey(obj))
                throw new ArgumentException("Object already was registered", nameof(obj));

            registeredObjects.Add(obj, entity);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal TTarget GetConverted<TSource, TTarget>([CanBeNull] TSource source, [NotNull] Func<CopyContext, TSource, TTarget> factory)
            where TSource : class
            where TTarget : Entity
        {
            if (source == null)
                return null;
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            Entity result;
            if (registeredObjects.TryGetValue(source, out result))
                return (TTarget)result;

            return factory(this, source);
        }
        #endregion
    }
}