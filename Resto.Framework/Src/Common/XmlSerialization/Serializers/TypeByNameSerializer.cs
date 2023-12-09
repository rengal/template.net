using System;
using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class TypeByNameSerializer : BaseNullableAtomSerializer<Type>
    {
        [NotNull]
        [Pure]
        protected override string ToString([NotNull] Type value, bool _)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return EntitiesRegistryBase.GetClassName(value);
        }

        [NotNull]
        [Pure]
        protected override Type DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return EntitiesRegistryBase.GetType(text);
        }
    }
}