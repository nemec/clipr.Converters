using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace clipr.Converters
{
    public class RegexConverter : TypeConverter
    {
        private readonly RegexOptions? _options;

        public RegexConverter()
        {
        }

        public RegexConverter(RegexOptions options)
        {
            _options = options;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Regex);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (str == null)
            {
                throw new FormatException("Unable to convert null or non-string value to Regex");
            }
            if (_options.HasValue)
            {
                return new Regex(str, _options.Value);
            }
            return new Regex(str);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(Regex))
            {
                throw new FormatException("Unable to use RegexConverter to convert to any type other than Regex");
            }
            return ConvertFrom(context, culture, value);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            var str = value as string;
            if (str == null) return false;
            try
            {
                if (_options.HasValue)
                {
                    new Regex(str, _options.Value);
                }
                else
                {
                    new Regex(str);
                }
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
