using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers.Wrappers
{
    internal static class SerializerWrapper
    {
        [Pure, NotNull]
        public static ISerializerWrapper<T> GetFor<T>()
        {
            return SerializerWrapperSingleton<T>.Instance;
        }

        #region Inner Types
        private static class SerializerWrapperSingleton<T>
        {
            [NotNull]
            public static ISerializerWrapper<T> Instance { get; }

            static SerializerWrapperSingleton()
            {
                Instance = Create();
            }

            private static ISerializerWrapper<T> Create()
            {
                var type = typeof(T);
                var sealedType = type.IsSealed;

                if (sealedType)
                    return new SerializerWrapperForSealedType<T>();

                if (type.IsGenericType)
                {
                    var genericTypeDefinition = type.GetGenericTypeDefinition();
                    // для этих двоих никогда не интересуемся конкретным типом коллекции, считая, что все они совместимы
                    // соответственно, при сериализации не запоминаем, какая именно коллекция там была (массив, список али ещё что),
                    // а при десериализации создаём любой удобный нам тип (например, List<T>)
                    if (genericTypeDefinition == typeof(IReadOnlyCollection<>) || genericTypeDefinition == typeof(IReadOnlyList<>))
                        return new SerializerWrapperForSelfReliantBaseType<T>();
                }

                return (ISerializerWrapper<T>)Activator.CreateInstance(typeof(SerializerWrapperForCanBeNullNonSealedType<>).MakeGenericType(typeof(T)));
            }
        }

        private sealed class SerializerWrapperForSealedType<T> : ISerializerWrapper<T>
        {
            [NotNull] private readonly ISerializer<T> serializer;

            public SerializerWrapperForSealedType()
            {
                Debug.Assert(typeof(T).IsSealed);
                serializer = SerializersManager.GetSerializer<T>();
            }

            public T DeepClone(T value, bool byValue, [NotNull] IDeserializationContext context)
            {
                return serializer.DeepClone(value, byValue, context);
            }

            public void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, T value, bool byValue,
                [NotNull] ISerializationContext context, SerializationMetadata metadata)
            {
                Debug.Assert(name != null);
                Debug.Assert(writer != null);
                Debug.Assert(context != null);

                serializer.WriteElement(name, writer, typeof(T), value, byValue, context, metadata);
            }

            public T ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
            {
                Debug.Assert(reader != null);
                Debug.Assert(context != null);

                return serializer.ReadElement(reader, byValue, context, metadata);
            }

            public string ToDebugString(T value, int depth)
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                return value == null ? StringExtensions.NullRepresentation : serializer.ToDebugString(value, depth);
                // ReSharper restore CompareNonConstrainedGenericWithNull
            }

            public void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, [CanBeNull] T value, bool byValue,
                [NotNull] ISerializationContext context, SerializationMetadata metadata)
            {
                // вызываем упрощённую реализацию, так как defaultType не имеет смысла для sealed-типа
                WriteElement(name, writer, value, byValue, context, metadata);
            }

            public T ReadElement([NotNull] XmlReader reader, [NotNull] Type defaultType, bool byValue,
                [NotNull] IDeserializationContext context, SerializationMetadata metadata)
            {
                // вызываем упрощённую реализацию, так как defaultType не имеет смысла для sealed-типа
                return ReadElement(reader, byValue, context, metadata);
            }
        }

        private sealed class SerializerWrapperForCanBeNullNonSealedType<T> : ISerializerWrapper<T>
            where T : class
        {
            [NotNull] private readonly Lazy<ISerializer<T>> serializer = new Lazy<ISerializer<T>>(SerializersManager.GetSerializer<T>);

            private readonly bool isTypeGenericAndInterface;
            [CanBeNull] private readonly Type[] genericArguments;

            public SerializerWrapperForCanBeNullNonSealedType()
            {
                var type = typeof(T);
                isTypeGenericAndInterface = type.IsInterface && type.IsGenericType;
                if (isTypeGenericAndInterface)
                    genericArguments = type.GetGenericArguments();
            }

            public T DeepClone(T value, bool byValue, [NotNull] IDeserializationContext context)
            {
                if (value == null)
                    return default;

                var typeOfValue = value.GetType();
                return typeOfValue == typeof(T)
                    ? serializer.Value.DeepClone(value, byValue, context)
                    : (T)SerializersManager.GetSerializer(typeOfValue).DeepClone(value, byValue, context);
            }

            public void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [CanBeNull] T value, bool byValue,
                [NotNull] ISerializationContext context, SerializationMetadata metadata)
            {
                WriteElement(name, writer, typeof(T), value, byValue, context, metadata);
            }

            public void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, [CanBeNull] T value, bool byValue,
                [NotNull] ISerializationContext context, SerializationMetadata metadata)
            {
                Debug.Assert(name != null);
                Debug.Assert(writer != null);
                Debug.Assert(defaultType != null);
                Debug.Assert(context != null);


                if (value == null)
                {
                    writer.WriteStartElement(name);
                    Serializer.WriteNullValue(writer);
                    writer.WriteEndElement();
                    return;
                }

                // defaultType — тип по умолчанию, либо выводится из типа поля (typeof(T)), либо задаётся явно атрибутом
                var typeOfValue = value.GetType();
                var serializerType = typeof(T);
                if (typeOfValue == serializerType)
                    serializer.Value.WriteElement(name, writer, serializerType, value, byValue, context, metadata);
                else
                    SerializersManager.GetSerializer(typeOfValue).WriteObjectToElement(name, writer, typeOfValue == defaultType ? defaultType : serializerType, value, byValue, context, metadata);
            }

            public T ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
            {
                return ReadElement(reader, typeof(T), byValue, context, metadata);
            }

            public T ReadElement([NotNull] XmlReader reader, [NotNull] Type defaultType, bool byValue,
                [NotNull] IDeserializationContext context, SerializationMetadata metadata)
            {
                Debug.Assert(reader != null);
                Debug.Assert(defaultType != null);
                Debug.Assert(context != null);

                if (Serializer.IsNullValue(reader))
                    return default;

                var typeName = Serializer.ReadTypeAttribute(reader);
                if (typeName == null)
                {
                    // Если в xml не указан тип, значит, тип значения можно получить из кода.
                    // При наличии атрибута с явным указанием типа берём тип из этого атрибута, иначе смотрим тип поля.
                    return typeof(T) == defaultType
                        ? serializer.Value.ReadElementContent(reader, byValue, context, metadata)
                        : (T)SerializersManager.GetSerializer(defaultType).ReadObjectContentFromElement(reader, byValue, context, metadata);
                }

                var actualType = EntitiesRegistryBase.GetType(typeName);
                if (actualType.IsGenericTypeDefinition && isTypeGenericAndInterface)
                {
                    Debug.Assert(genericArguments != null);
                    actualType = actualType.MakeGenericType(genericArguments);
                }
                else if (!typeof(T).IsAssignableFrom(actualType))
                {
                    throw new SerializerException("Type " + actualType + " is not a subtype of " + typeof(T));
                }

                return (T)SerializersManager.GetSerializer(actualType).ReadObjectContentFromElement(reader, byValue, context, metadata);
            }

            public string ToDebugString(T value, int depth)
            {
                if (value == null)
                    return StringExtensions.NullRepresentation;

                var typeOfValue = value.GetType();
                return typeOfValue == typeof(T)
                    ? serializer.Value.ToDebugString(value, depth)
                    : SerializersManager.GetSerializer(typeOfValue).ObjectToDebugString(value, depth);
            }
        }

        /// <summary>
        /// Обёртка для сериализаторов таких типов, для которых достаточно знать базовый тип и никогда не нужно запоминать конкретный тип.
        /// </summary>
        /// <remarks>
        /// Например, если поле типа <see cref="IReadOnlyCollection{T}"/>, то фактически в нём может быть любой класс, реализующий этот интерфейс,
        /// и конкретный тип коллекции запоминать в xml избыточно, считаем, что имеем право десериализовать, скажем, в <see cref="List{T}"/>.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        private sealed class SerializerWrapperForSelfReliantBaseType<T> : ISerializerWrapper<T>
        {
            [NotNull] private readonly Lazy<ISerializer<T>> serializer = new Lazy<ISerializer<T>>(SerializersManager.GetSerializer<T>);

            public T DeepClone(T value, bool byValue, [NotNull] IDeserializationContext context)
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                if (value == null)
                    // ReSharper restore CompareNonConstrainedGenericWithNull
                    return default;

                return serializer.Value.DeepClone(value, byValue, context);
            }

            public void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [CanBeNull] T value, bool byValue,
                [NotNull] ISerializationContext context, SerializationMetadata metadata)
            {
                WriteElement(name, writer, typeof(T), value, byValue, context, metadata);
            }

            public void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, [CanBeNull] T value, bool byValue,
                [NotNull] ISerializationContext context, SerializationMetadata metadata)
            {
                Debug.Assert(name != null);
                Debug.Assert(writer != null);
                Debug.Assert(defaultType != null);
                Debug.Assert(context != null);


                // ReSharper disable CompareNonConstrainedGenericWithNull
                if (value == null)
                // ReSharper restore CompareNonConstrainedGenericWithNull
                {
                    writer.WriteStartElement(name);
                    Serializer.WriteNullValue(writer);
                    writer.WriteEndElement();
                    return;
                }

                serializer.Value.WriteElement(name, writer, typeof(T), value, byValue, context, metadata);
            }

            public T ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
            {
                return ReadElement(reader, typeof(T), byValue, context, metadata);
            }

            public T ReadElement([NotNull] XmlReader reader, [NotNull] Type defaultType, bool byValue,
                [NotNull] IDeserializationContext context, SerializationMetadata metadata)
            {
                Debug.Assert(reader != null);
                Debug.Assert(defaultType != null);
                Debug.Assert(context != null);
                Debug.Assert(typeof(T).IsAssignableFrom(defaultType));

                if (Serializer.IsNullValue(reader))
                    return default;

                return serializer.Value.ReadElementContent(reader, byValue, context, metadata);
            }

            public string ToDebugString(T value, int depth)
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                if (value == null)
                    return StringExtensions.NullRepresentation;
                // ReSharper restore CompareNonConstrainedGenericWithNull

                var typeOfValue = value.GetType();
                return typeOfValue == typeof(T)
                    ? serializer.Value.ToDebugString(value, depth)
                    : SerializersManager.GetSerializer(typeOfValue).ObjectToDebugString(value, depth);
            }
        }
        #endregion
    }
}
