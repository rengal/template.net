using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
    {
        [CanBeNull]
        private readonly T1 first;
        [CanBeNull]
        private readonly T2 second;

        public Pair(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }

        public T1 First
        {
            get { return first; }
        }

        public T2 Second
        {
            get { return second; }
        }

        public bool Equals(Pair<T1, T2> other)
        {
            return Equals(other.first, first) && Equals(other.second, second);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(Pair<T1, T2>))
                return false;
            return Equals((Pair<T1, T2>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                return 
                    ((first != null ? first.GetHashCode() : 0) * 397) ^
                    (second != null ? second.GetHashCode() : 0);
                // ReSharper restore CompareNonConstrainedGenericWithNull
            }
        }

        public static bool operator ==(Pair<T1, T2> left, Pair<T1, T2> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Pair<T1, T2> left, Pair<T1, T2> right)
        {
            return !left.Equals(right);
        }

        public void Deconstruct(out T1 value1, out T2 value2)
        {
            value1 = first;
            value2 = second;
        }
    }
}