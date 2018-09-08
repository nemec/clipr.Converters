using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace clipr.Converters
{
    /// <summary>
    /// This TypeConverter takes a String and converts it into an IPEndpoint.
    /// This class is similar to the <see cref="IPEndPointConverter"/> except
    /// it also supports resolving domain names to an IP address. Use the format
    /// 'ipaddr:port' or 'domain:port'. This converter
    /// supports both IPv4 and IPv6 addresses. An IPv6 address MUST be enclosed
    /// in [brackets] to eliminate ambiguity between port and IPv6 component.
    /// </summary>
    public class IPHostConverter : TypeConverter
    {
        private readonly int? _defaultPort;

        private readonly IPPreference _preference;

        public IPHostConverter()
        {
        }

        /// <summary>
        /// Create a new IPHostConverter with a default port. Typically
        /// only parameterless TypeConverter constructors are supported by
        /// the converter machinery, so one must create a derived class that
        /// calls this constructor with a default port.
        /// </summary>
        /// <param name="defaultPort"></param>
        public IPHostConverter(int defaultPort)
        {
            _defaultPort = defaultPort;
        }

        /// <summary>
        /// Create a new IPHostConverter that orders IPv4 addresses for a
        /// domain name over IPv6 if both are available. Typically
        /// only parameterless TypeConverter constructors are supported by
        /// the converter machinery, so one must create a derived class that
        /// calls this constructor.
        /// </summary>
        /// <param name="preferIPv4"></param>
        internal IPHostConverter(IPPreference preference)
        {
            _preference = preference;
        }

        /// <summary>
        /// Create a new IPHostConverter with a default port and
        /// orders IPv4 addresses for a
        /// domain name over IPv6 if both are available. Typically
        /// only parameterless TypeConverter constructors are supported by
        /// the converter machinery, so one must create a derived class that
        /// calls this constructor with a default port and preference.
        /// </summary>
        /// <param name="defaultPort"></param>
        /// <param name="preferIPv4"></param>
        internal IPHostConverter(int defaultPort, IPPreference preference)
        {
            _defaultPort = defaultPort;
            _preference = preference;
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
            if (braceOpenIdx >= 0)
            {
                braceCloseIdx = value.IndexOf(']', braceOpenIdx + 1);
                if(braceCloseIdx < 0)
                {
                    // No closing brace - invalid IP
                    result = null;
                    return false;
                }
            }

            var idx = value.LastIndexOf(':', value.Length - 1, value.Length - (braceCloseIdx + 1));
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
                var hosts = Dns.GetHostAddresses(addrstr);
                switch(_preference)
                {
                    case IPPreference.IPv4:
                        hosts = hosts.OrderBy(i =>
                            i.AddressFamily == AddressFamily.InterNetwork ? 0 : 1).ToArray();
                        break;
                    case IPPreference.IPv6:
                        hosts = hosts.OrderBy(i =>
                            i.AddressFamily == AddressFamily.InterNetworkV6 ? 0 : 1).ToArray();
                        break;
                }

                addr = hosts.FirstOrDefault();
                if (addr == null)
                {
                    result = null;
                    return false;
                }
            }
            int port;
            if(portstr == null)
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
            if (str == null)
            {
                throw new FormatException("Unable to convert null or non-string value to IPEndpoint");
            }
            if (TryParse(str, out IPEndPoint result))
            {
                return result;
            }
            throw new FormatException(String.Format(
                "Unable to convert '{0}' to an IPEndpoint. Must be in format 'ipaddr:port' or 'domain:port'", value));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(IPEndPoint))
            {
                throw new FormatException("Unable to use IPHostConverter to convert to any type other than IPEndPoint");
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
