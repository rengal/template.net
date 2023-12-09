using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.Serializers;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization
{
    public static class SerializersManager
    {
        [NotNull]
        private static readonly object Gate = new object();
        [NotNull]
        private static readonly LazyObject<Dictionary<Type, ISerializer>> SerializersCache = new LazyObject<Dictionary<Type, ISerializer>>(CreateDefaultSerializers);
        [NotNull]
        private static readonly HashSet<Type> RootEntityTypes = new HashSet<Type>();

        static SerializersManager()
        {
            RegisterRootEntityType<PersistedEntity>();
        }

        private static Dictionary<Type, ISerializer> CreateDefaultSerializers()
        {
            return new Dictionary<Type, ISerializer>
            {
                { typeof(string), new StringSerializer() },
                { typeof(XElement), new XElementSerializer() },
                { typeof(Type), new TypeByNameSerializer() },
                { typeof(FieldInfo), new FieldByNameSerializer() },
            }
                .AddSerializer(new BooleanSerializer())
                .AddSerializer(new ByteSerializer())
                .AddSerializer(new DateTimeSerializer())
                .AddSerializer(new DecimalSerializer())
                .AddSerializer(new DoubleSerializer())
                .AddSerializer(new FloatSerializer())
                .AddSerializer(new GuidSerializer())
                .AddSerializer(new IntSerializer())
                .AddSerializer(new LongSerializer())
                .AddSerializer(new RMSColorSerializer())
                .AddSerializer(new TimeSpanSerializer());
        }

        [NotNull]
        private static Dictionary<Type, ISerializer> AddSerializer<T>([NotNull] this Dictionary<Type, ISerializer> serializers, [NotNull] BasePrimitiveValueTypeSerializer<T> serializer)
            where T : struct
        {
            serializers.Add(typeof(T), serializer);
            serializers.Add(typeof(T?), new NullableValueTypeSerializer<T>(serializer));

            return serializers;
        }

        public static void AddSerializer<T>([NotNull] ISerializer<T> serializer)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            lock (Gate)
            {
                SerializersCache.Value[typeof(T)] = serializer;
            }
        }

        public static void RegisterRootEntityType<TRootEntityBaseType>() where TRootEntityBaseType : Entity, IRootEntity
        {
            lock (Gate)
            {
                RootEntityTypes.Add(typeof(TRootEntityBaseType));
            }
        }

        [NotNull]
        public static ISerializer<T> GetSerializer<T>()
        {
            return (ISerializer<T>)GetSerializer(typeof(T));
        }

        [NotNull]
        public static ISerializer GetSerializer([NotNull] Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            lock (Gate)
            {
                var serializers = SerializersCache.Value;
                if (serializers.TryGetValue(type, out var result))
                    return result;

                if (typeof(Type).IsAssignableFrom(type))
                    return serializers[typeof(Type)];

                if (typeof(FieldInfo).IsAssignableFrom(type))
                    return serializers[typeof(FieldInfo)];

                result = CreateSerializer(type);
                serializers[type] = result;
                return result;
            }
        }

        [NotNull]
        private static ISerializer CreateSerializer([NotNull] Type type)
        {
            Debug.Assert(type != null);
            if (type.Namespace == null)
                throw new RestoException($"Please, don't use classes without namespace. Type - {type}");
            if (type.IsPrimitive)
                throw new ArgumentOutOfRangeException(nameof(type), type, "Converters for primitive types should be created manually");


            if (type.IsEnum)
                return CreateGenericTypeSerializer(typeof(EnumSerializer<>), type);

            if (type.IsDefined(typeof(EnumClassAttribute), false))
                return CreateGenericTypeSerializer(typeof(EnumClassSerializer<>), type);

            if (typeof(IDictionary).IsAssignableFrom(type))
                return CreateDictionarySerializer(type);

            if (typeof(Entity).IsAssignableFrom(type))
            {
                if (typeof(IRootEntity).IsAssignableFrom(type))
                {
                    foreach (var rootEntityType in RootEntityTypes)
                    {
                        if (rootEntityType.IsAssignableFrom(type))
                            return CreateGenericTypeSerializer(typeof(RootEntitySerializer<,>), type, rootEntityType);
                    }

                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown root entity type");
                }

                return CreateGenericTypeSerializer(typeof(ChildEntitySerializer<>), type);
            }

            if (type.IsArray)
                return CreateGenericTypeSerializer(typeof(ArraySerializer<>), type.GetElementType());

            var isGenericType = type.IsGenericType;
            var genericArguments = type.GetGenericArguments();

            if (isGenericType)
            {
                var genericDefinition = type.GetGenericTypeDefinition();

                if (genericDefinition == typeof(Nullable<>))
                {
                    var valueSerializer = GetSerializer(genericArguments[0]);
                    return (ISerializer)Activator.CreateInstance(typeof(NullableValueTypeSerializer<>).MakeGenericType(genericArguments[0]), valueSerializer);
                }

                if (genericDefinition == typeof(ByValueList<>))
                    return CreateGenericTypeSerializer(typeof(ByValueListSerializer<>), genericArguments);

                if (genericDefinition == typeof(ICollection<>))
                    return CreateGenericTypeSerializer(typeof(GenericICollectionSerializer<>), genericArguments);

                if (genericDefinition == typeof(ReadOnlyCollection<>))
                    return CreateGenericTypeSerializer(typeof(ReadOnlyCollectionSerializer<>), genericArguments);

                if (genericDefinition == typeof(IReadOnlyList<>))
                    return CreateGenericTypeSerializer(typeof(GenericIReadOnlyListSerializer<>), genericArguments);

                if (genericDefinition == typeof(IReadOnlyCollection<>))
                    return CreateGenericTypeSerializer(typeof(GenericIReadOnlyCollectionSerializer<>), genericArguments);

                if (genericDefinition == typeof(ByValue<>))
                    return CreateGenericTypeSerializer(typeof(ByValueSerializer<>), genericArguments);
            }

            if (IsCollection(type))
                return CreateCollectionSerializer(type);

            if (!type.Namespace.StartsWith("System"))
            {
                if (type.IsValueType)
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Serialization of custom value types not supported");

                return CreateGenericTypeSerializer(typeof(NonEntitySerializer<>), type);
            }

            throw new ArgumentException("Cannot create converter for " + type, nameof(type));
        }

        private static ISerializer CreateDictionarySerializer([NotNull] Type type)
        {
            Debug.Assert(type != null);
            Debug.Assert(typeof(IDictionary).IsAssignableFrom(type));

            var genericType = type;
            while (!genericType.IsGenericType && genericType.BaseType != null)
                genericType = genericType.BaseType;

            if (!genericType.IsGenericType)
                throw new ArgumentException($"Dictionary must be generic. Actual type: {type}", nameof(type));

            var genericArgs = genericType.GetGenericArguments();
            if (genericArgs.Length != 2)
                throw new ArgumentException($"Wrong number of type arguments in dictionary type {type}", nameof(type));

            return CreateGenericTypeSerializer(typeof(DictionarySerializer<,,>), type, genericArgs[0], genericArgs[1]);
        }

        private static bool IsCollection([NotNull] Type type)
        {
            Debug.Assert(type != null);
            Debug.Assert(!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(ICollection<>));
            Debug.Assert(!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(ByValueList<>));

            return
                typeof(ICollection).IsAssignableFrom(type) ||
                type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        [NotNull]
        private static ISerializer CreateCollectionSerializer([NotNull] Type type)
        {
            Debug.Assert(type != null);
            Debug.Assert(!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(ICollection<>));
            Debug.Assert(!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(ByValueList<>));


            var genericType = type;
            while (!genericType.IsGenericType && genericType.BaseType != null)
                genericType = genericType.BaseType;

            if (!genericType.IsGenericType)
                throw new ArgumentException($"Collection must be generic. Actual type: {type}", nameof(type));

            var genericArgs = genericType.GetGenericArguments();
            if (genericArgs.Length != 1)
                throw new ArgumentException($"Wrong number of type arguments in collection type {type}", nameof(type));

            return CreateGenericTypeSerializer(typeof(ArbitraryGenericCollectionSerializer<,>), type, genericArgs[0]);
        }

        [NotNull]
        private static ISerializer CreateGenericTypeSerializer(Type serializerGenericType, params Type[] genericArguments)
        {
            Debug.Assert(serializerGenericType != null);
            Debug.Assert(serializerGenericType.IsGenericTypeDefinition);

            return (ISerializer)Activator.CreateInstance(serializerGenericType.MakeGenericType(genericArguments));
        }
    }
}