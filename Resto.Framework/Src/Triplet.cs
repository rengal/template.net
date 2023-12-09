using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public struct Triplet<T1, T2, T3> : IEquatable<Triplet<T1, T2, T3>>
    {
        [CanBeNull]
        private readonly T1 first;
        [CanBeNull]
        private readonly T2 second;
        [CanBeNull]
        private readonly T3 third;

        public Triplet(T1 first, T2 second, T3 third)
        {
            this.first = first;
            this.second = second;
            this.third = third;
        }

        public T1 First
        {
            get { return first; }
        }

        public T2 Second
        {
            get { return second; }
        }

        public T3 Third
        {
            get { return third; }
        }

        public bool Equals(Triplet<T1, T2, T3> other)
        {
            return Equals(other.first, first) && Equals(other.second, second) && Equals(other.third, third);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(Triplet<T1, T2, T3>))
                return false;
            return Equals((Triplet<T1, T2, T3>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                var result = (first != null ? first.GetHashCode() : 0);
                result = (result * 397) ^ (second != null ? second.GetHashCode() : 0);
                result = (result * 397) ^ (third != null ? third.GetHashCode() : 0);
                // ReSharper restore CompareNonConstrainedGenericWithNull
                return result;
            }
        }

        public static bool operator ==(Triplet<T1, T2, T3> left, Triplet<T1, T2, T3> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Triplet<T1, T2, T3> left, Triplet<T1, T2, T3> right)
        {
            return !left.Equals(right);
        }
    }
}