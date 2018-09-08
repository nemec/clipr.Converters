using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace clipr.Converters
{
    /// <summary>
    /// TypeConverter that turns a String into a Timespan. This converter
    /// supports any format valid in TimeSpan.TryParse as well as a set of
    /// integer formats consisting of an integer followed directly (no spaces)
    /// by a format specifier. The specifiers allowed are 'ms' for milliseconds,
    /// 's' for seconds, 'm' for minutes, 'h' for hours, and 'd' for days. If
    /// no format is given, the integer is interpreted as seconds. Example: '10ms'
    /// </summary>
    public class TimeSpanConverter : TypeConverter
    {
        private bool TryParse(string fmt, out TimeSpan result)
        {
            int comp;
            if (int.TryParse(fmt, out comp))
            {
                result = TimeSpan.FromSeconds(comp);
                return true;
            }

            if (TimeSpan.TryParse(fmt, out result))
            {
                return true;
            }
            if (fmt.EndsWith("ms"))
            {
                if (int.TryParse(
                    fmt.Substring(0, fmt.Length - 2), out comp))
                {
                    result = TimeSpan.FromMilliseconds(comp);
                    return true;
                }
            }
            else if (fmt.EndsWith("s"))
            {
                if (int.TryParse(
                    fmt.Substring(0, fmt.Length - 1), out comp))
                {
                    result = TimeSpan.FromSeconds(comp);
                    return true;
                }
            }
            else if (fmt.EndsWith("m"))
            {
                if (int.TryParse(
                    fmt.Substring(0, fmt.Length - 1), out comp))
                {
                    result = TimeSpan.FromMinutes(comp);
                    return true;
                }
            }
            else if (fmt.EndsWith("h"))
            {
                if (int.TryParse(
                    fmt.Substring(0, fmt.Length - 1), out comp))
                {
                    result = TimeSpan.FromHours(comp);
                    return true;
                }
            }
            else if (fmt.EndsWith("d"))
            {
                if (int.TryParse(
                    fmt.Substring(0, fmt.Length - 1), out comp))
                {
                    result = TimeSpan.FromDays(comp);
                    return true;
                }
            }
            return false;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(TimeSpan);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (str == null)
            {
                throw new FormatException("Unable to convert null or non-string value to TimeSpan");
            }
            if (TryParse(str, out TimeSpan result))
            {
                return result;
            }
            throw new FormatException(String.Format(
                "Unable to convert '{0}' to a TimeSpan. Must be in format hh:mm:ss, \\d+[hms], or some variation.", value));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (!CanConvertFrom(destinationType))
            {
                throw new FormatException("Unable to use TimeSpanConverter to convert to any type other than TimeSpan");
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
