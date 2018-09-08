using System;
using System.Collections.Generic;
using System.Text;

namespace clipr.Converters
{
    /// <summary>
    /// This TypeConverter takes a String and converts it into an IPEndpoint.
    /// This class is similar to the <see cref="IPEndPointConverter"/> except
    /// it also supports resolving domain names to an IP address. Use the format
    /// 'ipaddr:port' or 'domain:port'. This converter
    /// supports both IPv4 and IPv6 addresses, but prefers an IPv4 address during
    /// domain lookup if one is available for the host. An IPv6 address in the
    /// input MUST be enclosed in [brackets] to eliminate ambiguity between
    /// port and IPv6 component.
    /// </summary>
    public class IPHostConverterPreferIPv4 : IPHostConverter
    {
        /// <summary>
        /// This TypeConverter takes a String and converts it into an IPEndpoint.
        /// This class is similar to the <see cref="IPEndPointConverter"/> except
        /// it also supports resolving domain names to an IP address. Use the format
        /// 'ipaddr:port' or 'domain:port'. This converter
        /// supports both IPv4 and IPv6 addresses, but prefers an IPv4 address during
        /// domain lookup if one is available for the host. An IPv6 address in the
        /// input MUST be enclosed in [brackets] to eliminate ambiguity between
        /// port and IPv6 component.
        /// </summary>
        public IPHostConverterPreferIPv4()
            : base(IPPreference.IPv4)
        {
        }

        /// <summary>
        /// Create a new IPHostConverter with a default port. Typically
        /// only parameterless TypeConverter constructors are supported by
        /// the converter machinery, so one must create a derived class that
        /// calls this constructor with a default port.
        /// </summary>
        /// <param name="defaultPort"></param>
        public IPHostConverterPreferIPv4(int defaultPort)
            : base(defaultPort, IPPreference.IPv4)
        {
        }
    }
}
