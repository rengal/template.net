using System;
using System.Collections.Generic;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Базовая реализация метода <see cref="MulticastDelegate.GetHashCode"/> для делегатов возвращает хешкод для типа, а не экземпляра,
    /// поэтому при добавлении нескольких экземпляров делегатов одного и того же типа в словарь хеш-таблица будет работать неэффективно, сваливаясь в O(N).
    /// Данный класс подмешивает к хешкоду от типа делегата ещё и хешкод от экземпляра объекта.
    /// </summary>
    /// <remarks>
    /// Подробнее можно почитать тут: https://stackoverflow.com/questions/6624151/why-do-2-delegate-instances-return-the-same-hashcode
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public sealed class DelegateComparer<T> : IEqualityComparer<T>
        where T : Delegate
    {
        public static readonly IEqualityComparer<T> Instance = new DelegateComparer<T>();

        private DelegateComparer()
        { }

        public bool Equals(T x, T y) => object.Equals(x, y);

        public int GetHashCode(T obj)
        {
            var result = obj.GetHashCode();
            var target = obj.Target;
            if (target != null)
                result ^= target.GetHashCode();

            return result;
        }
    }
}
