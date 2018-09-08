using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;

namespace clipr.Converters
{
    public class IPAddressConverter : TypeConverter
    {
        private bool TryParse(string value, out IPAddress result)
        {
            if (String.IsNullOrEmpty(value))
            {
                result = null;
                return false;
            }

            if (!IPAddress.TryParse(value,
                out IPAddress addr))
            {
                result = null;
                return false;
            }
            result = addr;
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(IPAddress);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (str == null)
            {
                throw new FormatException("Unable to convert null or non-string value to IPAddress");
            }
            if (TryParse(str, out IPAddress result))
            {
                return result;
            }
            throw new FormatException(String.Format(
                "Unable to convert '{0}' to an IPAddress. Must be in format 'ipaddr' or 'domain'", value));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(IPAddress))
            {
                throw new FormatException("Unable to use IAddressConverter to convert to any type other than IPAddress");
            }
            return ConvertFrom(context, culture, value);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            var str = value as string;
            if (str == null) return false;
            return TryParse(str, out _);
        }
    }
}
