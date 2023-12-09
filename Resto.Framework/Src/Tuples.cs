namespace Resto.Framework.Common
{
    public static class Tuples
    {
        public static Pair<T1, T2> Pair<T1, T2>(T1 first, T2 second)
        {
            return new Pair<T1, T2>(first, second);
        }

        public static Triplet<T1, T2, T3> Triplet<T1, T2, T3>(T1 first, T2 second, T3 third)
        {
            return new Triplet<T1, T2, T3>(first, second, third);
        }
    }
}