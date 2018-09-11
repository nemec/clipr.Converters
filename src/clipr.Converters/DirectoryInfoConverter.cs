using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace clipr.Converters
{
    public class DirectoryInfoConverter : TypeConverter
    {
        private ArgumentException TryParse(string value, out DirectoryInfo result)
        {
            if (String.IsNullOrEmpty(value))
            {
                result = null;
                return new ArgumentException("Path cannot be null or empty");
            }

            try
            {
                Path.GetFullPath(value);
            }
            catch (ArgumentException e)
            {
                result = null;
                return e;
            }
            result = new DirectoryInfo(value);
            return null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(DirectoryInfo);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (str == null)
            {
                throw new FormatException("Unable to convert null or non-string value to DirectoryInfo");
            }
            ArgumentException e;
            if ((e = TryParse(str, out DirectoryInfo result)) == null)
            {
                return result;
            }
            throw new FormatException(String.Format(
                "Unable to convert '{0}' to a DirectoryInfo: {1}", value, e.ToString()));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(DirectoryInfo))
            {
                throw new FormatException("Unable to use DirectoryInfoConverter to convert to any type other than DirectoryInfo");
            }
            return ConvertFrom(context, culture, value);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            var str = value as string;
            if (str == null) return false;
            return TryParse(str, out _) == null;
        }
    }
}
