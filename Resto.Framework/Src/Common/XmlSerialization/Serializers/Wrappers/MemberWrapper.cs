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
    internal static class DefaultValueChecker
    {
        private static readonly Dictionary<Type, Delegate> TypeToDefaultValueChecker;

        static DefaultValueChecker()
        {
            TypeToDefaultValueChecker = new Dictionary<Type, Delegate>(7)
                                            {
                                                { typeof(bool), (Func<bool, bool>)(value => value == default) },
                                                { typeof(DateTime), (Func<DateTime, bool>)(value => value == default) },
                                                { typeof(decimal), (Func<decimal, bool>)(value => value == default) },
                                                { typeof(Guid), (Func<Guid, bool>)(value => value == default) },
                                                { typeof(int), (Func<int, bool>)(value => value == default) },
                                                { typeof(TimeSpan), (Func<TimeSpan, bool>)(value => value == default) },
                                                // ReSharper disable CompareOfFloatsByEqualityOperator
                                                { typeof(double), (Func<double, bool>)(value => value == default) }
                                                // ReSharper restore CompareOfFloatsByEqualityOperator
                                            };
        }

        [CanBeNull]
        internal static Func<T, bool> GetIsDefaultValueChecker<T>()
        {
            if (TypeToDefaultValueChecker.TryGetValue(typeof(T), out var checker))
                return (Func<T, bool>)checker;

            return null;
        }
    }

    internal abstract class MemberWrapper<T>
    {
        #region Inner Types
        private abstract class BaseGenericMemberWrapper<TValue> : MemberWrapper<T>
        {
            [NotNull]
            protected readonly Func<T, TValue> GetValue;
            [NotNull]
            protected readonly Action<T, TValue> SetValue;

            protected readonly SerializationMetadata SerializationMetadata;

            [NotNull]
            protected ISerializerWrapper<TValue> Serializer => SerializerWrapper.GetFor<TValue>();

            protected BaseGenericMemberWrapper([NotNull] CustomMemberInfo memberInfo)
                : base(memberInfo.Name)
            {
                GetValue = memberInfo.CreateGetter<T, TValue>();
                SetValue = memberInfo.CreateSetter<T, TValue>();

                var forceTypeAttribute = memberInfo.GetCustomAttribute<ForceTypeAttributeInXmlAttribute>(false);
                var collectionDefaults = memberInfo.GetCustomAttribute<DefaultCollectionItemTypeAttribute>(false);
                var dictionaryDefaults = memberInfo.GetCustomAttribute<DefaultDictionaryEntryTypesAttribute>(false);
                Debug.Assert(collectionDefaults == null || dictionaryDefaults == null); // нет смысла применять оба атрибута
                SerializationMetadata = new SerializationMetadata(collectionDefaults?.DefaultCollectionItemType,
                    dictionaryDefaults?.DefaultKeyType, dictionaryDefaults?.DefaultValueType, forceTypeAttribute != null);
            }

            internal sealed override void ShallowCopy([NotNull] T from, [NotNull] T to)
            {
                Debug.Assert(from != null);
                Debug.Assert(to != null);

                SetValue(to, GetValue(from));
            }

            internal sealed override void DeepCopy([NotNull] T from, [NotNull] T to, [NotNull] IDeserializationContext context)
            {
                Debug.Assert(from != null);
                Debug.Assert(to != null);
                Debug.Assert(context != null);

                SetValue(to, Serializer.DeepClone(GetValue(from), false, context));
            }
        }

        private sealed class MemberWrapperForNotNullSealedTypeMember<TField> : BaseGenericMemberWrapper<TField>
        {
            [CanBeNull]
            private readonly Func<TField, bool> isDefaultValue;

            public MemberWrapperForNotNullSealedTypeMember([NotNull] CustomMemberInfo memberInfo)
                : base(memberInfo)
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                Debug.Assert(default(TField) != null);
                // ReSharper restore CompareNonConstrainedGenericWithNull
                Debug.Assert(memberInfo.MemberType.IsSealed);

                if (memberInfo.IsDefined(typeof(DontSerializeIfDefaultValueAttribute), false))
                    isDefaultValue = DefaultValueChecker.GetIsDefaultValueChecker<TField>();
            }

            internal override void SerializeField([NotNull] T obj, [NotNull] XmlWriter writer, [NotNull] ISerializationContext context)
            {
                Debug.Assert(obj != null);
                Debug.Assert(writer != null);
                Debug.Assert(context != null);

                var value = GetValue(obj);

                if (isDefaultValue != null && isDefaultValue(value))
                    return;

                Serializer.WriteElement(Name, writer, value, false, context, SerializationMetadata);
            }

            internal override void DeserializeField([NotNull] T obj, [NotNull] XmlReader reader, [NotNull] IDeserializationContext context)
            {
                Debug.Assert(obj != null);
                Debug.Assert(reader != null);
                Debug.Assert(context != null);

                var value = Serializer.ReadElement(reader, false, context, SerializationMetadata);
                SetValue(obj, value);
            }

            internal override string GetFieldValueDebugString([NotNull] T obj, int depth)
            {
                Debug.Assert(obj != null);

                return Serializer.ToDebugString(GetValue(obj), depth);
            }
        }

        private sealed class MemberWrapperForCanBeNullMember<TField> : BaseGenericMemberWrapper<TField>
        {
            private readonly bool withHasDefaultValueAttribute;
            private readonly bool withNotNullFieldAttribute;

            [NotNull]
            private readonly Type defaultType;

            public MemberWrapperForCanBeNullMember([NotNull] CustomMemberInfo memberInfo)
                : base(memberInfo)
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                Debug.Assert(default(TField) == null);
                // ReSharper restore CompareNonConstrainedGenericWithNull

                withHasDefaultValueAttribute = memberInfo.IsDefined(typeof(HasDefaultValueAttribute), false);
                withNotNullFieldAttribute = memberInfo.IsDefined(typeof(NotNullFieldAttribute), false);


                var defaultTypeAttribute = memberInfo.GetCustomAttribute<DefaultTypeAttribute>(false);
                if (defaultTypeAttribute == null)
                {
                    defaultType = typeof(TField);
                }
                else
                {
                    if (!typeof(TField).IsAssignableFrom(defaultTypeAttribute.DefaultType))
                        throw new ArgumentException($"Type of {nameof(memberInfo)} ('{typeof(TField)}') is not assignable from default type ('{defaultTypeAttribute.DefaultType}')", nameof(memberInfo));

                    defaultType = defaultTypeAttribute.DefaultType;
                }
            }

            internal override void SerializeField([NotNull] T obj, [NotNull] XmlWriter writer, [NotNull] ISerializationContext context)
            {
                Debug.Assert(obj != null);
                Debug.Assert(writer != null);
                Debug.Assert(context != null);

                var value = GetValue(obj);
                if (value == null)
                    return;

                Serializer.WriteElement(Name, writer, defaultType, value, false, context, SerializationMetadata);
            }

            internal override void DeserializeField([NotNull] T obj, [NotNull] XmlReader reader, [NotNull] IDeserializationContext context)
            {
                Debug.Assert(obj != null);
                Debug.Assert(reader != null);
                Debug.Assert(context != null);

                var value = Serializer.ReadElement(reader, defaultType, false, context, SerializationMetadata);

                // ReSharper disable CompareNonConstrainedGenericWithNull
                if (value == null)
                // ReSharper restore CompareNonConstrainedGenericWithNull
                {
                    if (!withNotNullFieldAttribute)
                    {
                        SetValue(obj, default);
                    }
                    else if (!withHasDefaultValueAttribute)
                    {
                        //Should be exception here, but current DB has some incorrect data
                        XmlSerialization.Serializer.LogWrapper.Log.WarnFormat("Null value in not-null field '{0}' in type '{1}'", Name, typeof(T));
                    }
                }
                else
                {
                    SetValue(obj, value);
                }
            }

            internal override string GetFieldValueDebugString([NotNull] T obj, int depth)
            {
                Debug.Assert(obj != null);

                var value = GetValue(obj);
                // ReSharper disable CompareNonConstrainedGenericWithNull
                if (value == null)
                    // ReSharper restore CompareNonConstrainedGenericWithNull
                    return StringExtensions.NullRepresentation;

                return value is Entity entity ? GuidGenerator.FormatGuid(entity.Id) : Serializer.ToDebugString(value, depth);
            }
        }
        #endregion

        #region Factory
        [NotNull]
        internal static MemberWrapper<T> CreateMemberWrapper([NotNull] CustomMemberInfo memberInfo)
        {
            Debug.Assert(memberInfo != null);

            var memberType = memberInfo.MemberType;
            var sealedType = memberType.IsSealed;
            var notNullType = IsNotNull(memberType);

            Type memberWrapperType;
            if (notNullType)
            {
                // Все типы, для которых недопустимо значение null, являются sealed-типами.
                // Типы System.Enum и System.ValueType в расчёт не берём.
                Debug.Assert(sealedType);
                memberWrapperType = typeof(MemberWrapperForNotNullSealedTypeMember<>);
            }
            else
            {
                memberWrapperType = typeof(MemberWrapperForCanBeNullMember<>);
            }

            return (MemberWrapper<T>)Activator.CreateInstance(memberWrapperType.MakeGenericType(typeof(T), memberType), memberInfo);
        }

        private static bool IsNotNull([NotNull] Type type)
        {
            // Суть проверки — default(T) != null
            if (type == null)
                throw new ArgumentNullException(nameof(type));


            if (!type.IsValueType)
                return false; // ref-type
            if (Nullable.GetUnderlyingType(type) != null)
                return false; // Nullable<T>
            return true; // value-type
        }
        #endregion

        #region Fields

        #endregion

        #region Ctor
        private MemberWrapper([NotNull] string memberName)
        {
            Debug.Assert(memberName != null);

            Name = memberName;
        }
        #endregion

        #region Props
        [NotNull]
        internal string Name { get; }
        #endregion

        #region Methods
        internal abstract void SerializeField([NotNull] T obj, [NotNull] XmlWriter writer, [NotNull] ISerializationContext context);
        internal abstract void DeserializeField([NotNull] T obj, [NotNull] XmlReader reader, [NotNull] IDeserializationContext context);
        internal abstract void ShallowCopy([NotNull] T from, [NotNull] T to);
        internal abstract void DeepCopy([NotNull] T from, [NotNull] T to, [NotNull] IDeserializationContext context);

        [NotNull]
        [Pure]
        internal abstract string GetFieldValueDebugString([NotNull] T obj, int depth);
        #endregion
    }
}