using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    public partial class LegalAddress
    {
        protected bool Equals(LegalAddress other)
        {
            return string.Equals(zipCode, other.zipCode) && string.Equals(country, other.country) &&
                   string.Equals(region, other.region) && string.Equals(district, other.district) &&
                   string.Equals(city, other.city) && string.Equals(community, other.community) &&
                   string.Equals(street, other.street) && string.Equals(house, other.house) &&
                   string.Equals(building, other.building) && string.Equals(office, other.office);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LegalAddress) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (zipCode != null ? zipCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (country != null ? country.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (region != null ? region.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (district != null ? district.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (city != null ? city.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (community != null ? community.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (street != null ? street.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (house != null ? house.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (building != null ? building.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (office != null ? office.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(LegalAddress left, LegalAddress right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LegalAddress left, LegalAddress right)
        {
            return !Equals(left, right);
        }
    }
}