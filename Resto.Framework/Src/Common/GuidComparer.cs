namespace Resto.Framework.Common
{
    public sealed class GuidComparer : IEqualityComparer<Guid>
    {
        public static readonly IEqualityComparer<Guid> Default = new GuidComparer();

        public bool Equals(Guid x, Guid y)
        {
            return x.ToByteArray().SequenceEqual(y.ToByteArray());
        }

        public int GetHashCode(Guid obj)
        {
            var bytes = obj.ToByteArray();
            return BitConverter.ToInt32(bytes, 0) ^
                   BitConverter.ToInt32(bytes, 4) ^
                   BitConverter.ToInt32(bytes, 8) ^
                   BitConverter.ToInt32(bytes, 12);
        }
    }
}
