namespace Resto.Data
{
    public abstract partial class EventMetadata
    {
        public bool Equals(EventMetadata other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.id, id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is EventMetadata)) return false;
            return Equals((EventMetadata)obj);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}