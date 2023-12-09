using System;
using System.Collections.Generic;
using Resto.Framework.Properties;

namespace Resto.Framework.Common
{
    public static class OlapHelper
    {
        private static readonly Dictionary<string, bool?> StringToBoolDictionary = new Dictionary<string, bool?> {
            { "true", true },
            { "false", false },
            { Resources.OlapHelperYes, true }, // OLAP иногда в качестве булевских значений возвращает Да/Нет :(
            { Resources.OlapHelperNo, false },
            { "null", null },
        };

        public static T GetValue<T>(IDictionary<string, object> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key) || dictionary[key] == null)
                return default(T);
            var value = dictionary[key];
            if (typeof(T) == typeof(Guid))
            {
                return (T)(object)new Guid(value.ToString());
            }
            if (typeof(T) == typeof(bool) && value is string)
            {
                return (T)(object)StringToBoolDictionary[(string)value].GetValueOrDefault();
            }
            if (typeof(T) == typeof(bool?) && value is string)
            {
                return (T)(object)StringToBoolDictionary[(string)value];
            }
            if (typeof(T) == typeof(DateTime))
            {
                return (T)(object)Convert.ToDateTime(value); //(T)(object)DateTime.Parse((string)value);
            }
            if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value);
            }
            return (T)value;
        }
    }
}
