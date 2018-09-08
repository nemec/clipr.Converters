using System;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;

namespace clipr.Converters.Sample
{
    class Program
    {
        public static void ManualConvert()
        {
            var converter = new IPAddressConverter();
            IPAddress addr = (IPAddress)converter.ConvertFromString("192.168.1.1");
            Console.WriteLine("Manual Address: {0}", addr);
        }

        class Options
        {
            [TypeConverter(typeof(IPAddressConverter))]
            public IPAddress DestinationIP { get; set; }
        }

        public static void ConvertWithConverterFinder()
        {
            var property = typeof(Options).GetProperty("DestinationIP");
            var converter = ConverterFinder.GetConverterForProperty(property);
            IPAddress addr = (IPAddress)converter.ConvertFromString("1fbf:0:a88:85a3::ac1f");
            Console.WriteLine("ConverterFinder Address: {0}", addr);
        }

        class IPEndPointConverterWithDefaultPort80 : IPEndPointConverter
        {
            public IPEndPointConverterWithDefaultPort80()
                : base(80)
            {
            }
        }

        public static void ConvertWithDefaultPort()
        {
            var converter = new IPEndPointConverterWithDefaultPort80();
            var addr = (IPEndPoint)converter.ConvertFromString("192.168.1.1:8000");
            Console.WriteLine("Explcit port: {0}", addr.Port);
            addr = (IPEndPoint)converter.ConvertFromString("192.168.1.1");
            Console.WriteLine("Default port: {0}", addr.Port);
        }

        public static void ConvertWithRegex()
        {
            var converter = new RegexConverter();
            var rx = (Regex)converter.ConvertFromInvariantString("th.s");
            var match = rx.Match("send this home");
            Console.WriteLine("Matched regex case sensitive: {0}", match.Value);
            match = rx.Match("send thus home");
            Console.WriteLine("Matched regex case sensitive: {0}", match.Value);
            match = rx.Match("SEND THIS HOME");
            Console.WriteLine("Matched regex case sensitive: {0}", match.Value);
        }

        class RegexConverterWithCaseInsensitivity : RegexConverter
        {
            public RegexConverterWithCaseInsensitivity()
                : base(RegexOptions.IgnoreCase)
            {
            }
        }

        public static void ConvertWithCaseInsensitiveRegex()
        {
            var converter = new RegexConverterWithCaseInsensitivity();
            var rx = (Regex)converter.ConvertFromInvariantString("th.s");
            var match = rx.Match("send this home");
            Console.WriteLine("Matched regex case insensitive: {0}", match.Value);
            match = rx.Match("send thus home");
            Console.WriteLine("Matched regex case insensitive: {0}", match.Value);
            match = rx.Match("SEND THIS HOME");
            Console.WriteLine("Matched regex case insensitive: {0}", match.Value);
        }

        static void Main(string[] args)
        {
            ManualConvert();
            ConvertWithConverterFinder();
            ConvertWithDefaultPort();
            ConvertWithRegex();
            ConvertWithCaseInsensitiveRegex();
        }
    }
}
