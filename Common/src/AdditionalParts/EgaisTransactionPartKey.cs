namespace Resto.Data
{
    public partial class EgaisTransactionPartKey
    {
        protected bool Equals(EgaisTransactionPartKey other)
        {
            return string.Equals(sourceRarId, other.sourceRarId) &&
                   string.Equals(alcCode, other.alcCode) &&
                   string.Equals(aRegId, other.aRegId) &&
                   string.Equals(bRegId, other.bRegId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EgaisTransactionPartKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = sourceRarId.GetHashCode();
                hashCode = (hashCode * 397) ^ alcCode.GetHashCode();
                hashCode = (hashCode * 397) ^ (aRegId != null ? aRegId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (bRegId != null ? bRegId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}