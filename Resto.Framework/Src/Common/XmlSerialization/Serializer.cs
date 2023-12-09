using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization
{
    public class Serializer
    {
        #region Nested types
        [PublicAPI]
        public sealed class StringWriterWithEncoding : StringWriter
        {
            private readonly Encoding encoding;

            public StringWriterWithEncoding(Encoding encoding)
            {
                this.encoding = encoding;
            }

            public StringWriterWithEncoding(StringBuilder builder, Encoding encoding)
                : base(builder)
            {
                this.encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return encoding; }
            }
        }
        #endregion

        #region Проверка PersistedEntity
        // Делегат по умолчанию для проверки PersistedEntity.
        private static readonly Action<PersistedEntity> DefaultPersistedEntityCheck = entity => { };

        // Перегруженный делегат для проверки PersistedEntity.
        private static Action<PersistedEntity> overridenPersistedEntityCheck;

        /// <summary>
        /// Инициализация делегата для проверки PersistedEntity.
        /// </summary>
        /// <param name="check"></param>
        public static void OverridePersistedEntityCheck(Action<PersistedEntity> check)
        {
            overridenPersistedEntityCheck = check;
        }

        /// <summary>
        /// Делегат для проверки PersistedEntity.
        /// </summary>
        protected static Action<PersistedEntity> PersistedEntityCheck
        {
            get { return overridenPersistedEntityCheck ?? DefaultPersistedEntityCheck; }
        }
        #endregion

        #region Игнорируемые поля
        protected static readonly HashSetMultiDictionary<Type, FieldInfo> TypesWithIgnoredMembers = new HashSetMultiDictionary<Type, FieldInfo>();

        /// <summary>
        /// Указать поле, которое будет игнорироваться при сериализации/десериализации
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="field">Поле</param>
        public static void RegisterIgnoredMemberForType([NotNull] Type type, [NotNull] FieldInfo field)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            TypesWithIgnoredMembers.Add(type, field);
        }
        #endregion

        #region Consts
        private const string NullValueAttributeName = "null";
        private const string NullValueAttributeValue = "1";
        private const string TypeAttributeString = "cls";
        #endregion

        #region Static Fields & Static Ctor
        internal static readonly LogWrapper LogWrapper = new LogWrapper(typeof(Serializer));

        [NotNull]
        protected static readonly Lazy<ISerializer<string>> StringSerializer;

        static Serializer()
        {
            StringSerializer = new Lazy<ISerializer<string>>(SerializersManager.GetSerializer<string>, LazyThreadSafetyMode.PublicationOnly);
        }
        #endregion

        #region Serialization
        [NotNull]
        public static string GetXmlText<T>([NotNull] T value, [NotNull] string name) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return GetXmlText(value, typeof(T), name, SerializationFormatSettings.WithXmlDeclarationAndIndentation.XmlWriterSettings, new DefaultSerializationContext());
        }

        [NotNull]
        public static string GetXmlText([NotNull] object value)
        {
            return GetXmlText(value, SerializationFormatSettings.Default);
        }

        [NotNull]
        public static string GetXmlText([NotNull] object value, [NotNull] SerializationFormatSettings formatSettings)
        {
            return GetXmlText(value, new DefaultSerializationContext(), formatSettings);
        }

        [NotNull]
        public static string GetXmlText([NotNull] object value, [NotNull] ISerializationContext context, [NotNull] SerializationFormatSettings formatSettings)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (formatSettings == null)
                throw new ArgumentNullException(nameof(formatSettings));


            return GetXmlText(value, value.GetType(), "r", formatSettings.XmlWriterSettings, context);
        }

        [NotNull]
        private static string GetXmlText([NotNull] object value, [NotNull] Type type, [NotNull] string name, [NotNull] XmlWriterSettings xmlWriterSettings, [NotNull] ISerializationContext context)
        {
            Debug.Assert(value != null);
            Debug.Assert(name != null);
            Debug.Assert(xmlWriterSettings != null);
            Debug.Assert(context != null);


            using (var stringWriter = new StringWriterWithEncoding(xmlWriterSettings.Encoding))
            {
                using (var writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    SerializeToDocument(name, type, value, writer, context);
                    writer.Flush();
                }

                return stringWriter.ToString();
            }
        }

        public static void SerializeToDocument<T>([NotNull] Stream stream, T data, [NotNull] string tagName)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var writer = new XmlTextWriter(stream, Encoding.UTF8))
            {
                SerializeToDocument(data, writer, tagName);
            }
        }

        public static void SerializeToDocument<T>(T value, [NotNull] XmlWriter writer, [NotNull] string name)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            SerializeToDocument(name, typeof(T), value, writer, new DefaultSerializationContext());
        }

        public static void SerializeToDocument([NotNull] string name, [NotNull] object value, [NotNull] XmlWriter writer, [NotNull] ISerializationContext context)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            SerializeToDocument(name, value.GetType(), value, writer, context);
        }

        private static void SerializeToDocument([NotNull] string name, [NotNull] Type type, [CanBeNull] object value, [NotNull] XmlWriter writer, [NotNull] ISerializationContext context)
        {
            Debug.Assert(name != null);
            Debug.Assert(type != null);
            Debug.Assert(writer != null);
            Debug.Assert(context != null);

            writer.WriteStartDocument();
            SerializeToElement(name, type, value, writer, true, context);
            writer.WriteEndDocument();
        }

        private static void SerializeToElement([NotNull] string name, [NotNull] Type type, [CanBeNull] object value, [NotNull] XmlWriter writer, bool byValue, [NotNull] ISerializationContext context)
        {
            Debug.Assert(name != null);
            Debug.Assert(type != null);
            Debug.Assert(writer != null);
            Debug.Assert(context != null);


            if (value == null)
            {
                writer.WriteStartElement(name);
                writer.WriteAttributeString(NullValueAttributeName, NullValueAttributeValue);
                writer.WriteString("");
                writer.WriteEndElement();
            }
            else
            {
                SerializersManager
                    .GetSerializer(value.GetType())
                    .WriteObjectToElement(name, writer, type, value, byValue, context, SerializationMetadata.Empty);
            }
        }
        #endregion

        #region Deserialization
        public static T Deserialize<T>(string text, bool byValue)
        {
            return Deserialize<T>(new StringReader(text), byValue);
        }

        public static T Deserialize<T>(TextReader textReader, bool byValue)
        {
            return Deserialize<T>(new XmlTextReader(textReader), byValue);
        }

        public static T Deserialize<T>(Stream stream, bool byValue)
        {
            return Deserialize<T>(new XmlTextReader(stream), byValue);
        }

        public static T Deserialize<T>(XmlReader reader, bool byValue)
        {
            return Deserialize<T>(reader, new DeserializationContext(EntityManager.EntitiesProvider), byValue);
        }

        public static T Deserialize<T>(XmlReader reader, IDeserializationContext context, bool byValue)
        {
            return (T)Deserialize(typeof(T), reader, context, byValue);
        }

        [CanBeNull]
        public static object Deserialize([NotNull] Type type, [NotNull] XmlReader reader, [NotNull] IDeserializationContext context, bool byValue)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                        return DeserializeElement(type, reader, context, byValue);
                }
            }
            catch (Exception e) when (e is WebException || e is IOException)
            {
                // исключение может возникать, если чтение (XmlReader reader) производится непосредственно из сети
                // к десериализации это не имеет отношения, поэтому пробрасываем дальше as is
                throw;
            }
            catch (Exception e)
            {
                throw new SerializerException($"Exception on deserializing xml to {type.FullName}: {context.DeserializationStack.ToDebugString()}", e);
            }

            throw new SerializerException($"Exception on deserializing xml to {type.FullName}: Root element not found.");
        }

        [CanBeNull]
        private static object DeserializeElement([NotNull] Type type, [NotNull] XmlReader reader, [NotNull] IDeserializationContext context, bool byValue)
        {
            Debug.Assert(type != null);
            Debug.Assert(reader != null);
            Debug.Assert(context != null);

            if (IsNullValue(reader))
                return null;

            var typeName = ReadTypeAttribute(reader);
            if (typeName != null && !type.IsSealed)
            {
                var actualType = EntitiesRegistryBase.GetType(typeName);
                if (actualType.IsGenericTypeDefinition && type.IsInterface && type.IsGenericType)
                {
                    actualType = actualType.MakeGenericType(type.GetGenericArguments());
                }
                else if (!type.IsAssignableFrom(actualType))
                {
                    throw new SerializerException("Type " + actualType + " is not a subtype of " + type);
                }

                type = actualType;
            }

            var serializer = SerializersManager.GetSerializer(type);
            return serializer.ReadObjectContentFromElement(reader, byValue, context, SerializationMetadata.Empty);
        }
        #endregion

        #region Debug String
        [NotNull]
        public static string ObjectToString([CanBeNull] object value)
        {
            if (value == null)
                return StringExtensions.NullRepresentation;

            return SerializersManager.GetSerializer(value.GetType()).ObjectToDebugString(value, 5);
        }
        #endregion

        #region Helpers
        internal protected static void WriteNullValue([NotNull] XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            writer.WriteAttributeString(NullValueAttributeName, NullValueAttributeValue);
            writer.WriteString("");
        }

        internal protected static bool IsNullValue([NotNull] XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (NullValueAttributeValue != reader.GetAttribute(NullValueAttributeName))
                return false;

            if (!reader.IsEmptyElement)
            {
                while (reader.Read())
                {
                    var nodeType = reader.NodeType;
                    if (nodeType == XmlNodeType.EndElement)
                        break;

                    if (nodeType == XmlNodeType.Element)
                        throw new SerializerException("Null values cannot have child elements");
                }
            }

            return true;
        }

        protected static void WriteTypeAttribute([NotNull] XmlWriter writer, [NotNull] Type type)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            writer.WriteAttributeString(TypeAttributeString, EntitiesRegistryBase.GetClassName(type));
        }

        internal static string ReadTypeAttribute([NotNull] XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            return reader.GetAttribute(TypeAttributeString);
        }
        #endregion

        #region Copy
        public static void ShallowCopy<T>([NotNull] T from, [NotNull] T to) where T : class
        {
            if (from == null)
                throw new ArgumentNullException(nameof(@from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));
            if (from.GetType() != to.GetType())
                throw new SerializerException(string.Format("Cannot copy objects of different types: {0} and {1}", from.GetType(), to.GetType()));

            if (ReferenceEquals(@from, to))
                return;

            FieldsCopy(from, to);
        }

        /// <summary>
        /// Копирует значение полей.
        /// </summary>
        /// <typeparam name="TFrom">Тип источника.</typeparam>
        /// <typeparam name="TTo">Тип объекта для установки полей. Должен совпадать или быть наследником <typeparamref name="TFrom"/>.</typeparam>
        /// <param name="from">Источник.</param>
        /// <param name="to">Объект для установки полей.</param>
        public static void FieldsCopy<TFrom, TTo>([NotNull] TFrom from, [NotNull] TTo to)
            where TFrom : class
            where TTo : class, TFrom
        {
            if (from == null)
                throw new ArgumentNullException(nameof(@from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            var serializer = SerializersManager.GetSerializer(from.GetType());
            var shallowCopySerializer = serializer as IShallowCopySerializer;
            if (shallowCopySerializer == null)
                throw new SerializerException(string.Format("Fields copy for type {0} not supported", from.GetType()));

            shallowCopySerializer.ShallowCopy(from, to);
        }

        [NotNull]
        public static T DeepClone<T>([NotNull] T obj)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            // ReSharper restore CompareNonConstrainedGenericWithNull

            var serializer = SerializersManager.GetSerializer(obj.GetType());
            var clone = serializer.DeepClone(obj, true, new DeserializationContext(EntityManager.EntitiesProvider));
            Debug.Assert(clone != null);
            return (T)clone;
        }
        #endregion

        #region Root Entity Creation
        public static TRootEntityBaseType CreateRootEntity<TRootEntityBaseType>([NotNull] Type type) where TRootEntityBaseType : Entity, IRootEntity
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (!typeof(TRootEntityBaseType).IsAssignableFrom(type))
                throw new ArgumentException(string.Format("Type {0} not derived from {1}", type, typeof(TRootEntityBaseType)), nameof(type));

            var factory = (IRootEntityFactory<TRootEntityBaseType>)SerializersManager.GetSerializer(type);
            return factory.Create();
        }
        #endregion
    }
}