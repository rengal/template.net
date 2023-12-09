using System;
using System.Collections.Generic;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Тип-обёртка для одного из двух значений, левого или правого.
    /// </summary>
    /// <typeparam name="TLeft">Тип левого значения.</typeparam>
    /// <typeparam name="TRight">Тип правого значения.</typeparam>
    public abstract class Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        #region Inner Types
        private sealed class Left : Either<TLeft, TRight>
        {
            #region Fields
            private readonly TLeft value;
            #endregion

            #region Ctor
            public Left(TLeft value)
            {
                this.value = value;
            }
            #endregion

            #region Methods

            public override Either<TLeftResult, TRightResult> Select<TLeftResult, TRightResult>(Func<TLeft, TLeftResult> ofLeft, Func<TRight, TRightResult> ofRight)
            {
                return Either<TLeftResult, TRightResult>.CreateLeft(ofLeft(value));
            }

            public override TResult Case<TResult>([NotNull] Func<TLeft, TResult> ofLeft, [NotNull] Func<TRight, TResult> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                return ofLeft(value);
            }

            public override void Case([NotNull] Action<TLeft> ofLeft, [NotNull] Action<TRight> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                ofLeft(value);
            }

            public override string ToString()
            {
                return value == null ? "Left(null)" : string.Format("Left({0})", value);
            }
            #endregion

            #region Equality
            public override bool Equals([CanBeNull] Either<TLeft, TRight> other)
            {
                var left = other as Left;

                return
                    left != null &&
                    EqualityComparer<TLeft>.Default.Equals(value, left.value);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TLeft>.Default.GetHashCode(value);
            }
            #endregion
        }

        private sealed class Right : Either<TLeft, TRight>
        {
            #region Fields
            private readonly TRight value;
            #endregion

            #region Ctor
            public Right(TRight value)
            {
                this.value = value;
            }
            #endregion

            #region Methods
            public override Either<TLeftResult, TRightResult> Select<TLeftResult, TRightResult>(Func<TLeft, TLeftResult> ofLeft, Func<TRight, TRightResult> ofRight)
            {
                return Either<TLeftResult, TRightResult>.CreateRight(ofRight(value));
            }

            public override TResult Case<TResult>([NotNull] Func<TLeft, TResult> ofLeft, [NotNull] Func<TRight, TResult> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                return ofRight(value);
            }

            public override void Case([NotNull] Action<TLeft> ofLeft, [NotNull] Action<TRight> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                ofRight(value);
            }

            public override string ToString()
            {
                return value == null ? "Right(null)" : string.Format("Right({0})", value);
            }
            #endregion

            #region Equality
            public override bool Equals([CanBeNull] Either<TLeft, TRight> other)
            {
                var right = other as Right;

                return
                    right != null &&
                    EqualityComparer<TRight>.Default.Equals(value, right.value);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TRight>.Default.GetHashCode(value);
            }
            #endregion
        }
        #endregion

        #region Ctor & Factory Methods
        private Either()
        { }

        [NotNull]
        public static Either<TLeft, TRight> CreateLeft(TLeft value)
        {
            return new Left(value);
        }

        [NotNull]
        public static Either<TLeft, TRight> CreateRight(TRight value)
        {
            return new Right(value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// В зависимости от того, какое значение хранится, преобразует значение с помощью соответствующей функции.
        /// </summary>
        /// <param name="ofLeft">Функция для левого значения.</param>
        /// <param name="ofRight">Функция для правого значения.</param>
        /// <returns>Результат, который вернула вызванная функция.</returns>
        [Pure]
        public abstract Either<TLeftResult, TRightResult> Select<TLeftResult, TRightResult>([NotNull] Func<TLeft, TLeftResult> ofLeft, [NotNull] Func<TRight, TRightResult> ofRight);

        /// <summary>
        /// В зависимости от того, какое значение хранится, вызывает соответствующую функцию и возвращает результат.
        /// </summary>
        /// <typeparam name="TResult">Тип возвращаемого значения.</typeparam>
        /// <param name="ofLeft">Функция для левого значения.</param>
        /// <param name="ofRight">Функция для правого значения.</param>
        /// <returns>Результат, который вернула вызванная функция.</returns>
        public abstract TResult Case<TResult>([NotNull] Func<TLeft, TResult> ofLeft, [NotNull] Func<TRight, TResult> ofRight);

        /// <summary>
        /// В зависимости от того, какое значение хранится, вызывает соответствующее действие.
        /// </summary>
        /// <param name="ofLeft">Действие для левого значения.</param>
        /// <param name="ofRight">Действие для правого значения.</param>
        public abstract void Case([NotNull] Action<TLeft> ofLeft, [NotNull] Action<TRight> ofRight);

        #region Equality
        public sealed override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return
                obj is Either<TLeft, TRight> &&
                Equals((Either<TLeft, TRight>)obj);
        }

        public abstract override int GetHashCode();

        public abstract bool Equals(Either<TLeft, TRight> other);

        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right)
        {
            return !Equals(left, right);
        }
        #endregion

        public static implicit operator Either<TLeft, TRight>(TLeft left)
        {
            return CreateLeft(left);
        }

        public static implicit operator Either<TLeft, TRight>(TRight right)
        {
            return CreateRight(right);
        }

        #endregion
    }
}