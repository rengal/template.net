using System;

namespace Resto.Data
{
    /// <summary>
    /// Обертка для класса <see cref="TimeSpan"/>. 
    /// <remarks>Отличается форматированием вида HH:mm при преобразовании к строке.</remarks>
    /// </summary>
    public struct TimeSpanWithoutSeconds : IComparable<TimeSpanWithoutSeconds>, IComparable, IEquatable<TimeSpanWithoutSeconds>
    {
        public TimeSpan TimeSpan { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="timeSpan"></param>
        public TimeSpanWithoutSeconds(TimeSpan timeSpan) : this()
        {
            TimeSpan = timeSpan;
        }

        public static explicit operator TimeSpan(TimeSpanWithoutSeconds timeSpanWithoutSeconds)
        {
            return timeSpanWithoutSeconds.TimeSpan;
        }

        public static implicit operator TimeSpanWithoutSeconds(TimeSpan timeSpan)
        {
            return new TimeSpanWithoutSeconds(timeSpan);
        }

        

        public override string ToString()
        {
            return string.Format("{0:00}:{1:00}", (int)TimeSpan.TotalHours, TimeSpan.Minutes);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TimeSpanWithoutSeconds other)
        {
            return TimeSpan.Equals((TimeSpan) other);
        }

        public int CompareTo(TimeSpanWithoutSeconds other)
        {
            return TimeSpan.CompareTo((TimeSpan) other);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is TimeSpanWithoutSeconds || obj is TimeSpan))
                throw new ArgumentNullException("Parameter must be TimeSpan or TimeSpanWithoutSeconds.");
            return CompareTo((TimeSpanWithoutSeconds)obj);
        }
    }
}