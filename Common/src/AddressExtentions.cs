using System.Collections.Generic;
using Resto.Data;
using Resto.Common.Properties;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Common
{
    public static class AddressExtentions
    {
        public static string AsString(this IAddress address)
        {
            var fields = new List<Pair<string, string>>
            {
                Tuples.Pair(Resources.AddressLine1Format, address.GetOrCalculateLine1()),
                Tuples.Pair(Resources.AddressLine2Format, address.GetOrCalculateLine2()),
            };

            return StringExtensions.FormatPairs(Resources.AddressLine1Line2Format, fields, Resources.AddressPartsDelimiter);
        }

        /// <summary>
        /// Получить/создать поле Line1 из Address. 
        /// </summary>
        /// <param name="address">Адрес.</param>
        /// <param name="forceRecalculate">Принудительно пересоздать поле из Адреса.</param>
        /// <returns>Line1.</returns>
        public static string GetOrCalculateLine1(this IAddress address, bool forceRecalculate = false)
        {
            if (!forceRecalculate && !string.IsNullOrEmpty(address.Line1))
            {
                return address.Line1;
            }

            var street = address.Street;
            if (street == null)
                return string.Empty;

            return MakeLine1FromAddressStrings(street.City.Name, address.Index, street.Name, address.House, address.Building);
        }

        /// <summary>
        /// Получить/создать поле Line2 из Address. 
        /// </summary>
        /// <param name="address">Адрес.</param>
        /// <param name="forceRecalculate">Принудительно пересоздать поле из Адреса.</param>
        /// <returns>Line2.</returns>
        public static string GetOrCalculateLine2(this IAddress address, bool forceRecalculate = false)
        {
            if (!forceRecalculate && !string.IsNullOrEmpty(address.Line2))
            {
                return address.Line2;
            }

            var fields = new List<Pair<string, string>>
            {
                Tuples.Pair(Resources.AddressEntranceFormat, address.Entrance),
                Tuples.Pair(Resources.AddressFloorFormat, address.Floor),
                Tuples.Pair(Resources.AddressFlatFormat, address.Flat),
                Tuples.Pair(Resources.AddressDoorphoneInfoFormat, address.Doorphone),
            };

            return StringExtensions.FormatPairs(Resources.AddressLine2FromStreetFormat, fields, Resources.AddressPartsDelimiter);
        }

        public static bool IsValid([CanBeNull] this IAddress address)
        {
            return address?.Street != null && !address.House.IsNullOrWhiteSpace();
        }
        /// <summary>
        /// Сделать Line1 из отдельных полей.
        /// </summary>
        /// <param name="cityName">Название города.</param>
        /// <param name="streetName">название улицы.</param>
        /// <param name="addressIndex">Индекс.</param>
        /// <param name="addressHouse">Номер дома.</param>
        /// <param name="addressBuilding">КОрпус.</param>
        /// <returns></returns>
        public static string MakeLine1FromAddressStrings(string cityName, string addressIndex, string streetName, string addressHouse, string addressBuilding)
        {
            var useUaeAddressingSystem = false;

            var fields = new List<Pair<string, string>>
            {
                Tuples.Pair(Resources.AddressCityFormat, cityName),
                Tuples.Pair(Resources.AddressIndexFormat, addressIndex),
                useUaeAddressingSystem
                    ? Tuples.Pair(Resources.AddressUaeStreetFormat, streetName)
                    : Tuples.Pair(Resources.AddressStreetFormat, streetName),
                useUaeAddressingSystem
                    ? Tuples.Pair(Resources.UaeAddressDetailsFormat, addressHouse)
                    : Tuples.Pair(Resources.AddressHouseFormat, addressHouse),
                Tuples.Pair(Resources.AddressBuildingFormat, addressBuilding),
            };

            return StringExtensions.FormatPairs(Resources.AddressLine1FromStreetFormat, fields, Resources.AddressPartsDelimiter);
        }
    }
}