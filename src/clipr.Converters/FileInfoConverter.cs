using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace clipr.Converters
{
    public class FileInfoConverter : TypeConverter
    {
        private ArgumentException TryParse(string value, out FileInfo result)
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
            catch(ArgumentException e)
            {
                result = null;
                return e;
            }
            result = new FileInfo(value);
            return null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(FileInfo);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (str == null)
            {
                throw new FormatException("Unable to convert null or non-string value to FileInfo");
            }
            ArgumentException e;
            if ((e = TryParse(str, out FileInfo result)) == null)
            {
                return result;
            }
            throw new FormatException(String.Format(
                "Unable to convert '{0}' to a FileInfo: {1}", value, e.ToString()));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(FileInfo))
            {
                throw new FormatException("Unable to use FileInfoConverter to convert to any type other than FileInfo");
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
