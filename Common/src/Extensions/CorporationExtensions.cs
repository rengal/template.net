using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;
using Resto.Framework.Data;

namespace Resto.Common.Extensions
{
    public static class CorporationExtentions
    {
        public static bool UseAddressNewFormat([NotNull] this Corporation entity) => entity.AddressShowTypeSettings.UseNewFormat;
        public static bool UseAddressLookup([NotNull] this Corporation entity) => entity.AddressShowTypeSettings.UseNewFormat
            && entity.AddressShowTypeSettings.UseLiveSearch;
        public static bool UseAddressCity([NotNull] this Corporation entity) => entity.AddressShowTypeSettings.UseNewFormat
            && entity.AddressShowTypeSettings.AddressShowType == AddressShowType.CITY;
        public static bool UseAddressInternational([NotNull] this Corporation entity) => entity.AddressShowTypeSettings.UseNewFormat
            && entity.AddressShowTypeSettings.AddressShowType == AddressShowType.INTERNATIONAL;
        public static bool UseAddressIntNoPostCode([NotNull] this Corporation entity) => entity.AddressShowTypeSettings.UseNewFormat
            && entity.AddressShowTypeSettings.AddressShowType == AddressShowType.NOPOSTCODE;
        public static bool UseAddressSystemLegacy([NotNull] this Corporation entity) => !entity.AddressShowTypeSettings.UseNewFormat
            || entity.AddressShowTypeSettings.AddressShowType == AddressShowType.LEGACY;

    }
}