using System;
using System.Collections.Generic;
using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class EnumSerializer<T> : BasePrimitiveValueTypeSerializer<T> where T : struct
    {
        private const string OldSeparator = "|";
        private readonly Dictionary<T, string> valueToString;
        private readonly Dictionary<string, T> stringToValue;

        public EnumSerializer()
        {
            var enumType = typeof(T);

            if (!enumType.IsEnum)
                throw new InvalidOperationException($"Enum serializer support only enum generic types. Actual type is '{enumType}'.");

            var values = (T[])Enum.GetValues(enumType);
            valueToString = new Dictionary<T, string>(values.Length);
            stringToValue = new Dictionary<string, T>(values.Length);

            // ReSharper disable ForCanBeConvertedToForeach
            for (var i = 0; i < values.Length; i++)
            // ReSharper restore ForCanBeConvertedToForeach
            {
                var value = values[i];
                var str = Enum.Format(enumType, value, "f");
                if (!valueToString.ContainsKey(value))
                    valueToString.Add(value, str);
                stringToValue.Add(str, value);
            }
        }

        [NotNull]
        [Pure]
        protected override string ToStringCore(T value, bool _)
        {
            return valueToString.TryGetValue(value, out var result)
                ? result
                : Enum.Format(typeof(T), value, "f");
        }

        [Pure]
        protected override T DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            //Для совместимости со старыми данными кеш-сервера, которые использовали «|» в качестве разделителя
            text = text.Replace(OldSeparator, ",");

            return stringToValue.TryGetValue(text, out var result)
                ? result
                : (T)Enum.Parse(typeof(T), text);
        }
    }
}