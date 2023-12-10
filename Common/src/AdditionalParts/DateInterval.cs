using System;
using System.Diagnostics.Contracts;

namespace Resto.Data
{
    partial class DateInterval : IEquatable<DateInterval>
    {
        public override string ToString()
        {
            return $"с {From.GetValueOrDefault():dd.MM.yyyy HH:mm:ss} по {To.GetValueOrDefault():dd.MM.yyyy HH:mm:ss}";
        }

        public bool InInterval(DateTime? dateTime)
        {
            return dateTime.HasValue &&
                   dateTime.Value >= From.GetValueOrDefault() &&
                   dateTime.Value <= To.GetValueOrDefault();
        }

        #region Equality

        public bool Equals(DateInterval other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FieldsMatch(other, this);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(DateInterval)) return false;
            return FieldsMatch((DateInterval)obj, this);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((from.HasValue ? from.Value.GetHashCode() : 0) * 397) ^ (to.HasValue ? to.Value.GetHashCode() : 0);
            }
        }

        public static bool operator ==(DateInterval left, DateInterval right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DateInterval left, DateInterval right)
        {
            return !Equals(left, right);
        }

        private static bool FieldsMatch(DateInterval i1, DateInterval i2)
        {
            return (i1.From == i2.From) && (i1.To == i2.To);
        }

        #endregion

        /// <summary>
        /// Возвращает значение, указывающее, что два закрытых интервала пересекаются.
        /// Интервалы, имеющие общую точку, считаются пересекающимися.
        /// </summary>
        public static bool Intersects(DateInterval i1, DateInterval i2)
        {
            Contract.Requires(i1.From != null);
            Contract.Requires(i1.To != null);
            Contract.Requires(i2.From != null);
            Contract.Requires(i2.To != null);

            Contract.Requires(i1.From <= i1.To);
            Contract.Requires(i2.From <= i2.To);

            var maxFrom = i1.From.GetValueOrDefault() > i2.From.GetValueOrDefault() ? i1.From.GetValueOrDefault() : i2.From.GetValueOrDefault();
            var minTo = i1.To.GetValueOrDefault() < i2.To.GetValueOrDefault() ? i1.To.GetValueOrDefault() : i2.To.GetValueOrDefault();
            return minTo >= maxFrom;
        }

        /// <summary>
        /// Обёртка для свойства <see cref="From"/>, чтобы каждый
        /// раз не делать проверки "if (From.HasValue)".
        /// </summary>
        /// <remarks>
        /// Проверка на то, что свойство <see cref="From"/>
        /// содержит значение, не осуществляется. Использовать
        /// только в случаях, когда есть уверенность, что
        /// null там быть не может.
        /// </remarks>
        public DateTime DateFrom
        {
            get
            {
                Contract.Assert(From != null);
                return From.Value;
            }
        }

        /// <summary>
        /// Обёртка для свойства <see cref="To"/>, чтобы каждый
        /// раз не делать проверки "if (From.HasValue)".
        /// </summary>
        /// <remarks>
        /// Проверка на то, что свойство <see cref="To"/>
        /// содержит значение, не осуществляется. Использовать
        /// только в случаях, когда есть уверенность, что
        /// null там быть не может.
        /// </remarks>
        public DateTime DateTo
        {
            get
            {
                Contract.Assert(To != null);
                return To.Value;
            }
        }
    }
}