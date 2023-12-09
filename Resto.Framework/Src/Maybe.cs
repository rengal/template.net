using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Тип-обёртка для значений. Допускает отсутствие значения (<see cref="Empty"/>).
    /// В отличие от <see cref="Nullable{T}"/> можно обернуть значение reference-типа.
    /// </summary>
    /// <typeparam name="T">Тип обёрнутого значения</typeparam>
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        #region Fields
        private readonly bool hasValue;
        private readonly T value;
        #endregion

        #region Ctor
        public Maybe(T value)
        {
            hasValue = true;
            this.value = value;
        }
        #endregion

        #region Props
        /// <summary>
        /// Отсутствующее значение для типа <typeparamref name="T" />.
        /// </summary>
        public static Maybe<T> Empty
        {
            get { return new Maybe<T>(); }
        }

        /// <summary>
        /// Возвращает флаг наличия значения.
        /// </summary>
        public bool HasValue
        {
            get { return hasValue; }
        }

        /// <summary>
        /// Возвращает флаг отсутствия значения.
        /// </summary>
        public bool IsEmpty
        {
            get { return !hasValue; }
        }

        /// <summary>
        /// Обёрнутое значение.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Значение отсутствует.
        /// </exception>
        public T Value
        {
            get
            {
                if (!hasValue)
                    throw new InvalidOperationException();

                return value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Возвращает this, если есть значение или <paramref name="other"/>, если значение отсутствует.
        /// </summary>
        public Maybe<T> Or(Maybe<T> other)
        {
            return hasValue ? this : other;
        }

        /// <summary>
        /// Возвращает <see cref="Value"/>, если есть значение, иначе возвращает значение по умолчанию для <typeparamref name="T"/>.
        /// </summary>
        public T GetValueOrDefault()
        {
            return value;
        }

        /// <summary>
        /// Возвращает <see cref="Value"/>, если есть значение, иначе возвращает <paramref name="defaultValue"/>.
        /// </summary>
        public T GetValueOrDefault(T defaultValue)
        {
            return hasValue ? value : defaultValue;
        }

        /// <summary>
        /// Возвращает <see cref="Value"/>, если есть значение, иначе возвращает результат вызова <paramref name="defaultValueFactory"/>.
        /// </summary>
        public T GetValueOrDefault([NotNull] Func<T> defaultValueFactory)
        {
            return hasValue ? value : defaultValueFactory();
        }

        public Maybe<TResult> Select<TResult>([NotNull] Func<T, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return hasValue ? selector(value) : Maybe<TResult>.Empty;
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public override string ToString()
        {
            if (!hasValue)
                return "Maybe.Empty";

            if (value == null)
                return "Maybe(null)";

            return $"Maybe({value})";
        }

        #region Equality
        public bool Equals(Maybe<T> other)
        {
            return hasValue ? other.hasValue && EqualityComparer<T>.Default.Equals(value, other.value) : !other.hasValue;
        }

        public override bool Equals(object obj)
        {
            return obj is Maybe<T> && Equals((Maybe<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (hasValue.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(value);
            }
        }

        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !left.Equals(right);
        }
        #endregion

        #endregion
    }
}