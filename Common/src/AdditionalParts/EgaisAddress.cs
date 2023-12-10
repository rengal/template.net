using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class EgaisAddress
    {
        public static bool IsCorrectCountry(string country)
        {
            return !country.IsNullOrWhiteSpace() && Regex.IsMatch(country, EgaisConstraints.COUNTRY.Pattern);
        }

        public static bool IsCorrectRegionCode(string regionCode, EgaisOrganizationType organizationType)
        {
            return organizationType == EgaisOrganizationType.FO ||
                    (!regionCode.IsNullOrWhiteSpace() && Regex.IsMatch(regionCode, EgaisConstraints.REGION_CODE.Pattern));
        }

        public static bool IsCorrectAddressDescription(string description)
        {
            return !description.IsNullOrWhiteSpace() && Regex.IsMatch(description, EgaisConstraints.ANY_STRING_2000.Pattern);
        }

        public override string ToString()
        {
            if (Description.IsNullOrEmpty())
            {
                return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                    Country,
                    Index,
                    RegionCode,
                    Area,
                    City,
                    Place,
                    Street,
                    House,
                    Building,
                    Liter);
            }
            return Description;
        }
    }
}
