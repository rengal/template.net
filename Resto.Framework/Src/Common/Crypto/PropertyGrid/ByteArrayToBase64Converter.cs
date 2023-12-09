using System;
using System.ComponentModel;
using System.Globalization;

namespace Resto.Framework.Common.Crypto.PropertyGrid
{
    public sealed class ByteArrayToBase64Converter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
          object value, Type destType)
        {
            if ((value != null) && (value is byte[]))
            {
                return Convert.ToBase64String((byte[]) value);
            }
            else
            {
                return String.Empty;   
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value != null)
            {
                return Convert.FromBase64String((string)value);
            }
            else
            {
                return null;
            }
        }

    }
}