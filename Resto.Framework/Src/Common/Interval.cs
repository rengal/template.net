using System;
using System.Collections.Generic;

namespace Resto.Framework.Common
{
    public struct Interval<T> : IEquatable<Interval<T>>
    {
        public readonly T Start;
        public readonly T End;

        public Interval(T start, T end)
        {
            if (Compare(start, end) > 0)
                throw new ArgumentException("start cannot be greater than end");
            
            Start = start;
            End = end;
        }

        private static int Compare(T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y);
        }

        private static bool IsValidNotZeroInterval(T start, T end)
        {
            return Compare(start, end) < 0;
        }

        private static bool isPoint(T start, T end)
        {
            return Compare(start, end) == 0;
        }

        public List<Interval<T>> Subtract(Interval<T> interval)
        {
            //Proper included or equals
            if (IsIncluded(interval))
                return new List<Interval<T>>();

            if (interval.IsPoint || !Intersects(interval))
                return new List<Interval<T>> { this };

            var list = new List<Interval<T>>(2);

            if (IsValidNotZeroInterval(Start, interval.Start))
                list.Add(new Interval<T>(Start, interval.Start));

            if (IsValidNotZeroInterval(interval.End, End))
                list.Add(new Interval<T>(interval.End, End));

            return list;
        }

        public bool Intersects(Interval<T> interval)
        {
            return Intersects(interval.Start, interval.End);
        }

        public bool Intersects(T start, T end)
        {
            return Compare(Start, end) <= 0 && Compare(End, start) >= 0;
        }

        public bool IsIncluded(Interval<T> interval)
        {
            return IsIncluded(interval.Start, interval.End);
        }

        public bool IsIncluded(T start, T end)
        {
            return Compare(Start, start) >= 0 && Compare(End, end) <= 0;
        }

        public bool Includes(Interval<T> interval)
        {
            return Includes(interval.Start, interval.End);
        }

        public bool Includes(T start, T end)
        {
            return Compare(Start, start) <= 0 && Compare(End, end) >= 0;
        }

        public bool Includes(T point)
        {
            return Includes(point, point);
        }
     
        public bool IsPoint
        {
            get { return isPoint(Start, End); }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Interval<T>)) return false;
            return Equals((Interval<T>) obj);
        }

        public bool Equals(Interval<T> other)
        {
            return Equals(other.Start, Start) && Equals(other.End, End);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Start.GetHashCode()*397) ^ End.GetHashCode();
            }
        }
    }
}
