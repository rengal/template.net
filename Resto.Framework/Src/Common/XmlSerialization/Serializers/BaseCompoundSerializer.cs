using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers.Wrappers;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal abstract class BaseCompoundSerializer<T> : BaseReferenceTypeSerializer<T>, IShallowCopySerializer where T : class
    {
        #region Fields
        [NotNull]
        protected readonly Func<T> CreateInstance;

        [NotNull]
        private readonly MemberWrapper<T>[] memberWrappers;
        [NotNull]
        private readonly IReadOnlyDictionary<string, MemberWrapper<T>> memberWrappersByName;
        #endregion

        #region Ctor
        protected BaseCompoundSerializer()
        {
            var type = typeof(T);

            if (type.IsInterface)
                throw new SerializerException($"Type {type.FullName} is interface. Cannot create interface instance.");
            var ctorInfo = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, Array.Empty<ParameterModifier>());
            if (ctorInfo == null)
                throw new SerializerException($"Type {type.FullName} doesn't contain ctor without parameters");

            CreateInstance = ExpressionHelper.GetParameterlessCtorCall<T>(ctorInfo);

            var ignoredFields = TypesWithIgnoredMembers[type];
            var fields = EnumerateFields(type)
                .Where(field => !ignoredFields.Contains(field)) // не заменять на Except, тут шустрый HashSet<T>.Contains
                .Select(field => new CustomMemberInfo(field));
            var properties = EnumerateProperties(type)
                .Select(property => new CustomMemberInfo(property));
            memberWrappers = fields.Concat(properties)
                .Select(MemberWrapper<T>.CreateMemberWrapper)
                .ToArray();

            memberWrappersByName = memberWrappers.ToDictionary(memberWrapper => memberWrapper.Name);
        }

        private static IEnumerable<FieldInfo> EnumerateFields([NotNull] Type type)
        {
            Debug.Assert(type != null);

            return EnumerableEx
                .Generate(type, t => t != typeof(object), t => t.BaseType, t => t.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                .SelectMany(fields => fields)
                .Where(field => !field.IsDefined(typeof(TransientAttribute), false))
                .Where(field => !field.FieldType.IsSubclassOf(typeof(Delegate)))
                .Where(field => !field.IsDefined(typeof(CompilerGeneratedAttribute), false));
        }

        private static IEnumerable<PropertyInfo> EnumerateProperties([NotNull] Type type)
        {
            Debug.Assert(type != null);

            return EnumerableEx
                .Generate(type, t => t != typeof(object), t => t.BaseType, t => t.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                .SelectMany(properties => properties)
                .Where(property => !property.IsDefined(typeof(TransientAttribute), false))
                .Where(property => !property.PropertyType.IsSubclassOf(typeof(Delegate)))
                .Where(IsValidAutoProperty);
        }

        [Pure]
        private static bool IsValidAutoProperty([NotNull] PropertyInfo property)
        {
            // автосвойства всегда имеют getter, и если его нет — это обычное свойство, пропускаем
            if (!property.CanRead)
                return false;

            // get/set-методы для автосвойства генерирует компилятор, так что если нет соответствующего атрибута — это обычное свойство, пропускаем
            var getter = property.GetGetMethod(true);
            if (!getter.IsDefined(typeof(CompilerGeneratedAttribute)))
                return false;

            // поддержка виртуальных автосвойств пока не требуется и не реализована (а зачем?)
            // считаем, что в классах данных это не нужно, автосвойства должны быть так же просты, как поля, virtual/override ни к чему
            //
            // Однако, одной проверки на виртуальность мало, ибо метод может быть виртуальным не только из-за явного объявления виртуальным,
            // но и ради реализации интерфейса.
            // Чтобы отличить, объявлено ли свойство виртуальным явно (для возможности переопределения в наследниках),
            // или же неявно компилятором (для реализации интерфейса), дополнительно смотрим атрибут final (sealed).
            if (getter.Attributes.HasFlag(MethodAttributes.Virtual) && !getter.Attributes.HasFlag(MethodAttributes.Final))
                throw new SerializerException($"Type {property.DeclaringType} has a virtual autoproperty {property.Name}.");

            // readonly-автосвойства не поддерживаются, т.к. записывать придётся в автосгенерированное поле, а его имя как бы неизвестно
            // строго говоря, имя автосгенерированного поля — это деталь реализации компилятора, которая может поменяться в следующей его версии.
            // похоже, что в метаданных не остаётся никакой связи между свойством и его полем,
            // эта связь есть только у компилятора на этапе компиляции конструктора (предполагается же, что дальше никто не будет менять значение)
            if (!property.CanWrite)
                throw new SerializerException($"Type {property.DeclaringType} has a readonly autoproperty {property.Name}.");

            return true;
        }
        #endregion

        #region Methods
        protected void WriteFields([NotNull] XmlWriter writer, [NotNull] T value, [NotNull] ISerializationContext context)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            // ReSharper disable ForCanBeConvertedToForeach
            for (var i = 0; i < memberWrappers.Length; i++)
            // ReSharper restore ForCanBeConvertedToForeach
            {
                memberWrappers[i].SerializeField(value, writer, context);
            }
        }

        protected void ReadFields([NotNull] XmlReader reader, [NotNull] T value, [NotNull] IDeserializationContext context)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            if (reader.IsEmptyElement)
                return;

            reader.Read();
            while (reader.NodeType != XmlNodeType.None)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        //parsing field value
                        var elementName = reader.Name;
                        if (!memberWrappersByName.TryGetValue(elementName, out var memberWrapper))
                        {
                            reader.Skip();
                            continue;
                        }

                        context.DeserializationStack.PushField(elementName);
                        memberWrapper.DeserializeField(value, reader, context);
                        reader.Read();
                        context.DeserializationStack.PopField();
                        break;

                    case XmlNodeType.EndElement:
                        return;

                    default:
                        reader.Read();
                        break;
                }
            }
            throw new SerializerException("Unexpected end of data. " + typeof(T));
        }

        public sealed override string ToDebugString([NotNull] T value, int depth)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (depth == 0)
            {
                var entity = value as Entity;
                if (entity != null)
                    return GuidGenerator.FormatGuid((entity).Id);

                return value.GetType().Name + "@" + value.GetHashCode();
            }

            var result = memberWrappers
                .Select(fieldWrapper => fieldWrapper.Name + ": " + fieldWrapper.GetFieldValueDebugString(value, depth - 1))
                .Join(", ");

            return "{" + result + "}";
        }

        protected void DeepCopyFields([NotNull] T from, [NotNull] T to, [NotNull] IDeserializationContext context)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            // ReSharper disable ForCanBeConvertedToForeach
            for (var i = 0; i < memberWrappers.Length; i++)
            // ReSharper restore ForCanBeConvertedToForeach
            {
                memberWrappers[i].DeepCopy(from, to, context);
            }
        }

        void IShallowCopySerializer.ShallowCopy([NotNull] object from, [NotNull] object to)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            var type = typeof(T);
            if (from.GetType() != type)
                throw new SerializerException($"Invalid object type: {from.GetType()}. Expected: {type}");
            if (!(to is T))
                throw new SerializerException($"Invalid object type: {to.GetType()}. Expected: {type} or it's inheritor.");

            var source = (T)from;
            var target = (T)to;

            // ReSharper disable ForCanBeConvertedToForeach
            for (var i = 0; i < memberWrappers.Length; i++)
            // ReSharper restore ForCanBeConvertedToForeach
            {
                memberWrappers[i].ShallowCopy(source, target);
            }
        }
        #endregion
    }
}