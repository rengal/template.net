using System;
using System.Linq;
using Resto.Framework.Common.Phone;

namespace Resto.Common.src.FormatProviders
{
    /// <summary>
    /// FormatProvider для телефонов для представления их в международном формате.
    /// </summary>
    public class PhoneToInternationalFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type type)
        {
            return this;
        }

        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (!(arg is string))
                return arg.ToString();
            var phones = (string)arg;
            return string.Join("; ", phones.Split(';').Select(phone => PhoneUtils.ConvertToInternationalFormat(phone.Trim())));
        }

        public static PhoneToInternationalFormatProvider Instance = new PhoneToInternationalFormatProvider();
    }
}
