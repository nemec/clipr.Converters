using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;

namespace clipr.Converters
{
    /// <summary>
    /// TypeConverter that takes a String and turns it into an IPEndPoint
    /// object. The IPEndPoint class represents both an IP address and a port
    /// number, therefore the supported format is 'ipaddr:port'. This converter
    /// supports both IPv4 and IPv6. An IPv6 address MUST be enclosed
    /// in [brackets] to eliminate ambiguity between port and IPv6 component.
    /// </summary>
    public class IPEndPointConverter : TypeConverter
    {
        private readonly int? _defaultPort;

        public IPEndPointConverter()
        {
        }

        /// <summary>
        /// Create a new IPEndPointConverter with a default port. Typically
        /// only parameterless TypeConverter constructors are supported by
        /// the converter machinery, so one must create a derived class that
        /// calls this constructor with a default port.
        /// </summary>
        /// <param name="defaultPort"></param>
        public IPEndPointConverter(int defaultPort)
        {
            _defaultPort = defaultPort;
        }

        private bool TryParse(string value, out IPEndPoint result)
        {
            if (String.IsNullOrEmpty(value))
            {
                result = null;
                return false;
            }

            // IPv6 addresses contain colons too
            var braceOpenIdx = value.IndexOf('[');
            var braceCloseIdx = 0;
            if(braceOpenIdx >= 0)
            {
                braceCloseIdx = value.IndexOf(']', braceOpenIdx + 1);
                if (braceCloseIdx < 0)
                {
                    // No closing brace - invalid IP
                    result = null;
                    return false;
                }
            }

            var idx = value.LastIndexOf(':', value.Length-1, value.Length - (braceCloseIdx + 1));
            if (idx < 0 && !_defaultPort.HasValue)
            {
                result = null;
                return false;
            }

            string addrstr;
            string portstr = null;

            if (idx > 0)
            {
                addrstr = value.Substring(0, idx);
                portstr = value.Substring(idx + 1);
            }
            else
            {
                addrstr = value;
            }

            if (!IPAddress.TryParse(addrstr,
                out IPAddress addr))
            {
                result = null;
                return false;
            }
            int port;
            if (portstr == null)
            {
                port = _defaultPort.Value;
            }
            else if (!int.TryParse(portstr,
                out port))
            {
                result = null;
                return false;
            }
            result = new IPEndPoint(addr, port);
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(IPEndPoint);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if(str == null)
            {
                throw new FormatException("Unable to convert null or non-string value to IPEndpoint");
            }
            if(TryParse(str, out IPEndPoint result))
            {
                return result;
            }
            throw new FormatException(String.Format(
                "Unable to convert '{0}' to an IPEndpoint. Must be in format 'ipaddr:port'", value));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(destinationType != typeof(IPEndPoint))
            {
                throw new FormatException("Unable to use IPEndPointConverter to convert to any type other than IPEndPoint");
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
