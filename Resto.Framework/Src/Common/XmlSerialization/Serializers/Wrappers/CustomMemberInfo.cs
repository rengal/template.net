using System;
using System.Reflection;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers.Wrappers
{
    internal sealed class CustomMemberInfo
    {
        private readonly Either<FieldInfo, PropertyInfo> info;

        public CustomMemberInfo([NotNull] FieldInfo fieldInfo)
        {
            info = Either<FieldInfo, PropertyInfo>.CreateLeft(fieldInfo);
        }

        public CustomMemberInfo([NotNull] PropertyInfo propertyInfo)
        {
            info = Either<FieldInfo, PropertyInfo>.CreateRight(propertyInfo);
        }

        [NotNull, Pure]
        public Func<T, TValue> CreateGetter<T, TValue>()
        {
            return info.Case(ExpressionHelper.GetFieldGetter<T, TValue>, ExpressionHelper.GetPropertyGetter<T, TValue>);
        }

        [NotNull, Pure]
        public Action<T, TValue> CreateSetter<T, TValue>()
        {
            return info.Case(ExpressionHelper.GetFieldSetter<T, TValue>, ExpressionHelper.GetPropertySetter<T, TValue>);
        }

        public T GetCustomAttribute<T>(bool inherit)
            where T : Attribute
        {
            return Member.GetCustomAttribute<T>(inherit);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return Member.IsDefined(attributeType, inherit);
        }

        public Type MemberType => info.Case(field => field.FieldType, property => property.PropertyType);

        public string Name => info.Case(field => field.Name, property => DecapitalizeFirstChar(property.Name));

        private MemberInfo Member => info.Case<MemberInfo>(field => field, property => property);

        [NotNull, Pure]
        private static string DecapitalizeFirstChar([NotNull] string source)
        {
            if (source.Length == 1)
                return source.ToLowerInvariant();

            return char.ToLowerInvariant(source[0]) + source.Substring(1);
        }
    }
}
