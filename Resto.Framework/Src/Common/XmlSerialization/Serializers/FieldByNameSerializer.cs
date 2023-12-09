using System;
using System.Diagnostics;
using System.Reflection;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class FieldByNameSerializer : BaseNullableAtomSerializer<FieldInfo>
    {
        [NotNull]
        [Pure]
        protected override string ToString([NotNull] FieldInfo value, bool _)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return EntitiesRegistryBase.GetClassName(value.DeclaringType) + "." + value.Name;
        }

        [NotNull]
        [Pure]
        protected override FieldInfo DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            var lastDotPos = text.LastIndexOf('.');
            if (lastDotPos <= 0)
                throw new ArgumentException($"Illegal field name: {text}");

            var typeName = text.Substring(0, lastDotPos);
            var fieldName = text.Substring(lastDotPos + 1);
            var type = EntitiesRegistryBase.GetType(typeName);
            var fieldInfo = type.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (fieldInfo == null)
                throw new ArgumentException($"Field {text} not exists");

            return fieldInfo;
        }
    }
}