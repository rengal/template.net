using System;
using Resto.Common;
using Resto.Framework.Common;

// ReSharper disable once CheckNamespace
namespace Resto.Data
{
    public partial class Address : IAddress, ICloneable
    {
        public override string ToString()
        {
            return this.AsString();
        }

        public object Clone()
        {
            return new Address(isMainAddress, Line1, Line2, House, Building, Flat, Entrance,
                Floor, Doorphone, AdditionalInfo, Street, Region, externalCartographyId, Index);
        }

        public bool IsAlike(Street street, string house, string building, string flat)
        {
            return Equals(Street, street) &&
                   string.Equals(House, house.TrimSafe()) &&
                   string.Equals(Building, building.TrimSafe()) &&
                   string.Equals(Flat, flat.TrimSafe());
        }
        public override bool Equals(object obj)
        {
            return obj is Address address && Equals(Street, address.Street) &&
                   Equals(Region, address.Region) &&
                   string.Equals(House, address.House) &&
                   string.Equals(Building, address.Building) &&
                   string.Equals(Flat, address.Flat) &&
                   string.Equals(Entrance, address.Entrance) &&
                   string.Equals(Floor, address.Floor) &&
                   string.Equals(Doorphone, address.Doorphone) &&
                   string.Equals(AdditionalInfo, address.AdditionalInfo) &&
                   string.Equals(Index, address.Index) &&
                   string.Equals(Line1, address.Line1) &&
                   string.Equals(Line2, address.Line2);
        }

        public override int GetHashCode()
        {
            var hashCode = street.GetHashCode() ^ house.GetHashCode();
            if (!string.IsNullOrEmpty(flat))
            {
                hashCode ^= flat.GetHashCode();
            }
            if (!string.IsNullOrEmpty(building))
            {
                hashCode ^= building.GetHashCode();
            }
            if (!string.IsNullOrEmpty(entrance))
            {
                hashCode ^= entrance.GetHashCode();
            }
            if (!string.IsNullOrEmpty(floor))
            {
                hashCode ^= floor.GetHashCode();
            }
            if (!string.IsNullOrEmpty(doorphone))
            {
                hashCode ^= doorphone.GetHashCode();
            }
            if (region != null)
            {
                hashCode ^= region.GetHashCode();
            }
            if (!string.IsNullOrEmpty(additionalInfo))
            {
                hashCode ^= additionalInfo.GetHashCode();
            }
            if (!string.IsNullOrEmpty(index))
            {
                hashCode ^= index.GetHashCode();
            }
            if (!string.IsNullOrEmpty(line1))
            {
                hashCode ^= line1.GetHashCode();
            }
            if (!string.IsNullOrEmpty(line2))
            {
                hashCode ^= line2.GetHashCode();
            }
            return hashCode;
        }
    }
}
