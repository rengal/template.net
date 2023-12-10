using System;

namespace Resto.Data
{
    partial class TaxationSystem
    {
        public static TaxationSystem ParseOrDefault(string value)
        {
            try
            {
                return Parse(value);
            }
            catch (ArgumentException)
            {
                return COMMON;
            }
        }

        public static string GetValueOrDefault(object value)
        {
            var taxationSystem = value as TaxationSystem;
            return taxationSystem != null ? taxationSystem._Value : COMMON._Value;
        }

        public static TaxationSystem GetOrDefault(TaxationSystem taxationSystem)
        {
            return taxationSystem ?? COMMON;
        }

        public static string GetValueOrDefault(string value)
        {
            return ParseOrDefault(value)._Value;
        }
    }
}
