using System;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DefaultTypeAttribute : Attribute
    {
        public DefaultTypeAttribute([NotNull] Type defaultType)
        {
            if (defaultType == null)
                throw new ArgumentNullException(nameof(defaultType));

            DefaultType = defaultType;
        }

        [NotNull]
        public Type DefaultType { get; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DefaultCollectionItemTypeAttribute : Attribute
    {
        public DefaultCollectionItemTypeAttribute([NotNull] Type defaultItemType)
        {
            if (defaultItemType == null)
                throw new ArgumentNullException(nameof(defaultItemType));

            DefaultCollectionItemType = defaultItemType;
        }

        [NotNull]
        public Type DefaultCollectionItemType { get; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DefaultDictionaryEntryTypesAttribute : Attribute
    {
        [CanBeNull]
        public Type DefaultKeyType { get; set; }

        [CanBeNull]
        public Type DefaultValueType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DontSerializeIfDefaultValueAttribute : Attribute
    { }

    /// <summary>
    /// Писать в xml атрибут cls="КонкретныйТип" даже если с точки зрения C# все generic-и выводятся до конкретных типов.
    /// В java в рантайме доступно меньше информации, поэтому добавялем избыточность вручную.
    /// TODO RMS-48020 убрать, если получится заставить сериализатор (возможно, BaseReferenceTypeSerializer) правильно угадывать, что нужно java
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ForceTypeAttributeInXmlAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class TransientAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EnumClassAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class)]
    [BaseTypeRequired(typeof(Entity))]
    public sealed class RootEntityAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class HasDefaultValueAttribute : Attribute
    { }

    /// <summary>
    /// Атрибут, подсказывающий xml-сериализатору (<see cref="Common.XmlSerialization.Serializer"/>),
    /// что для поля nullable-типа (reference type, <see cref="Nullable{T}"/>) не допускается значение <c>null</c>.
    /// </summary>
    /// <remarks>Runtime-аналог <see cref="NotNullAttribute"/>.</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class NotNullFieldAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum)]
    public sealed class DataClassAttribute : Attribute
    {
        public DataClassAttribute([NotNull] string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; }
    }
}