using System;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс, содержащий методы расширения массивов.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Возвращает массив, представляющий склейку текущего экземпляра с указанным аргументом(ами).
        /// </summary>
        /// <typeparam name="T">Тип элементов массивов.</typeparam>
        /// <param name="self">Текущий экземпляр массива.</param>
        /// <param name="other">Элементы (или массив), которые будут склеены с текущим экземпляром.</param>
        /// <returns>Массив, содержащий склейку текущего массива с переданными аргументами.</returns>
        /// <remarks>
        /// Текущий экземпляр массива остается неизменным.
        /// </remarks>
        public static T[] Concat<T>(this T[] self, params T[] other)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (other == null)
                throw new ArgumentNullException(nameof(other));

            var oldLength = self.Length;
            Array.Resize(ref self, oldLength + other.Length);
            other.CopyTo(self, oldLength);
            return self;
        }
    }
}
