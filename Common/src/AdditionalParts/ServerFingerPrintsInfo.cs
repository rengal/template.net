namespace Resto.Data
{
    public partial class ServerFingerPrintsInfo
    {
        protected bool Equals(ServerFingerPrintsInfo other)
        {
            return string.Equals(crmId, other.crmId) && dbId.Equals(other.dbId) && string.Equals(hwId, other.hwId) && string.Equals(installationPath, other.installationPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ServerFingerPrintsInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (CrmId != null ? CrmId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DbId.GetHashCode();
                hashCode = (hashCode * 397) ^ (HwId != null ? HwId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InstallationPath != null ? InstallationPath.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}