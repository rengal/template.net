using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class EnumClassSerializer<T> : BaseNullableAtomSerializer<T> where T : class
    {
        private readonly Func<string, T> parse;
        private readonly Func<T, string> getValue;

        public EnumClassSerializer()
        {
            if (!typeof(T).IsDefined(typeof(EnumClassAttribute), false))
                throw new InvalidOperationException(string.Format("Enum class serializer support only generic types with EnumClass attribute. Actual type is '{0}'.", typeof(T)));

            parse = CreateParseDelegate();
            getValue = CreateGetValueDelegate();
        }

        private static Func<string, T> CreateParseDelegate()
        {
            var enumClassType = typeof(T);

            var methodParse = enumClassType.GetMethod("Parse", new[] { typeof(string) });
            if (methodParse == null)
                throw new SerializerException("Cannot find method Parse(string) in " + enumClassType);
            if (!methodParse.IsStatic)
                throw new SerializerException("Cannot find static method Parse(string) in " + enumClassType);


            var param = Expression.Parameter(typeof(string), "value");
            var parseCall = Expression.Call(methodParse, param);

            var lambda = Expression.Lambda(typeof(Func<string, T>), parseCall, param);
            var compiled = (Func<string, T>)lambda.Compile();
            return compiled;
        }

        private static Func<T, string> CreateGetValueDelegate()
        {
            var enumClassType = typeof(T);

            var propertyValue = enumClassType.GetProperty("_Value", typeof(string));
            if (propertyValue == null)
                throw new SerializerException("Cannot find property _Value in " + enumClassType);

            var param = Expression.Parameter(typeof(T), "obj");
            var prop = Expression.Property(param, propertyValue);

            var lambda = Expression.Lambda(typeof(Func<T, string>), prop, param);
            var compiled = (Func<T, string>)lambda.Compile();
            return compiled;
        }

        [NotNull]
        [Pure]
        protected override string ToString([NotNull] T value, bool _)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return getValue(value);
        }

        [NotNull]
        [Pure]
        protected override T DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return parse(text);
        }
    }
}