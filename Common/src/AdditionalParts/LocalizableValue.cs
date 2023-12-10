using System;
using Resto.Common.Localization;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public sealed partial class LocalizableValue : IComparable<LocalizableValue>, IComparable
    {
        public LocalizableValue(string customValue)
            : this(null, null, customValue)
        {
        }

        public LocalizableValue(LocalizableValue other)
            : this(other.DefaultResourceId, other.CurrentResourceId, other.customValue)
        {
        }

        // Порядок определения действующего значения (RMS-32408, RMS-39624):
        // * ресурс с ключом currentResourceId, если ключ задан и соответствующий ресурс имеется,
        // * иначе строка customValue, если она задана,
        // * иначе ресурс с ключом defaultResourceId, если ключ задан и соответствующий ресурс имеется,
        // * иначе непосредственно ключ currentResourceId, если он задан (понятнее, чем пустая строка),
        // * иначе непосредственно ключ defaultResourceId, если он задан (понятнее, чем пустая строка),
        // * иначе пустая строка
        public EffectiveValue GetEffectiveValue()
        {
            if (currentResourceId != null)
            {
                var value = RestoServerResourcesExtensions.TryGetLocalResource(currentResourceId);
                if (value != null)
                    return new EffectiveValue(value, currentResourceId);
            }

            if (customValue != null)
                return customValue;

            if (defaultResourceId != null)
            {
                var value = RestoServerResourcesExtensions.TryGetLocalResource(defaultResourceId);
                if (value != null)
                    return new EffectiveValue(value, defaultResourceId);
            }

            return currentResourceId ?? defaultResourceId ?? string.Empty;
        }

        public string Local => GetEffectiveValue();

        public int CompareTo(LocalizableValue other)
        {
            if (other == null)
            {
                return 1;
            }
            // При переключении языка бекофиса происходит его перезапуск, так что можно сравнивать по локализованному значению.
            return string.Compare(Local, other.Local, StringComparison.CurrentCulture);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as LocalizableValue);
        }

        public override string ToString()
        {
            return Local;
        }

        public struct EffectiveValue
        {
            [CanBeNull]
            public readonly string ResourceId;
            [NotNull]
            public readonly string Value;

            public EffectiveValue([NotNull] string value, string resourceId = null)
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                ResourceId = resourceId;
                Value = value;
            }

            [NotNull]
            public static implicit operator string(EffectiveValue e)
            {
                return e.Value;
            }

            public static implicit operator EffectiveValue([NotNull] string s)
            {
                if (s == null)
                    throw new ArgumentNullException(nameof(s));

                return new EffectiveValue(s);
            }
        }
    }
}