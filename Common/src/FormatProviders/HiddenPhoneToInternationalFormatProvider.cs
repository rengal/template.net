using System;
using System.Linq;
using System.Text.RegularExpressions;
using Resto.Framework.Common.Phone;

namespace Resto.Common.src.FormatProviders
{
    /// <summary>
    /// FormatProvider для маскирования номеров телефонов 
    /// </summary>
    public class HiddenPhoneToInternationalFormatProvider : IFormatProvider, ICustomFormatter
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
            return string.Join("; ", phones.Split(';').Select(phone =>PhoneUtils.HidePhoneString(phone.Trim())));
        }

        public static HiddenPhoneToInternationalFormatProvider Instance = new HiddenPhoneToInternationalFormatProvider();
    }
}
