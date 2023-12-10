using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.Extensions
{
    public static class ValueTypeExtensions
    {
        [Obsolete("Выпиливать по мере возможности. Добавлено в места использования некорректных свойств, сгенерированных старым ClassConverter'ом (RMS-35418).")]
        public static T GetValueOrFakeDefault<T>([CanBeNull] this T? value) where T : struct 
        {
            return value.GetValueOrDefault();
        }
    }
}
