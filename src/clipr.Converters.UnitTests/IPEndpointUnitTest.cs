using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace clipr.Converters.UnitTests
{
    [TestClass]
    public class IPEndpointUnitTest
    {
        [TestMethod]
        public void CanConvertFromString_WithCorrectValue_IsTrue()
        {
            const string input = "192.168.1.1";
            var converter = new IPEndPointConverter();
            Assert.IsTrue(converter.CanConvertFrom(input.GetType()));
        }

        [TestMethod]
        public void CanConvertToIPEndPoint_WithCorrectValue_IsTrue()
        {
            var converter = new IPEndPointConverter();
            Assert.IsTrue(converter.CanConvertTo(typeof(IPEndPoint)));
        }

        [TestMethod]
        public void ConvertFromString_WithCorrectValue_ParsesIPEndPoint()
        {
            var expected = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 8000);
            const string input = "192.168.1.1:8000";
            var converter = new IPEndPointConverter();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertFromString_WithIPv6Address_ParsesIPEndPoint()
        {
            var expected = new IPEndPoint(IPAddress.Parse("[1fbf:0:a88:85a3::ac1f]"), 8000);
            const string input = "[1fbf:0:a88:85a3::ac1f]:8000";
            var converter = new IPEndPointConverter();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertFromString_WithIPv6AddressAndNoBrackets_ParsesIPEndPoint()
        {
            var expected = new IPEndPoint(IPAddress.Parse("[1fbf:0:a88:85a3::ac1f]"), 8000);
            const string input = "[1fbf:0:a88:85a3::ac1f]:8000";
            var converter = new IPEndPointConverter();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }

        private class ConverterWithDefaultPort : IPEndPointConverter
        {
            public ConverterWithDefaultPort()
                : base(8080)
            {
            }
        }

        [TestMethod]
        public void ConvertFromString_WithDefaultPortAndExplicitPortSpecified_ParsesIPEndPointWithExplicitPort()
        {
            var expected = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 8000);
            const string input = "192.168.1.1:8000";
            var converter = new ConverterWithDefaultPort();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertFromString_WithDefaultPortAndPortMissing_ParsesIPEndPointWithDefaultPort()
        {
            var expected = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 8080);
            const string input = "192.168.1.1";
            var converter = new ConverterWithDefaultPort();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertFromString_WithIPv6AndDefaultPortAndExplicitPortSpecified_ParsesIPEndPointWithExplicitPort()
        {
            var expected = new IPEndPoint(IPAddress.Parse("[1fbf:0:a88:85a3::ac1f]"), 8000);
            const string input = "[1fbf:0:a88:85a3::ac1f]:8000";
            var converter = new ConverterWithDefaultPort();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertFromString_WithIPv6AndDefaultPortAndPortMissing_ParsesIPEndPointWithDefaultPort()
        {
            var expected = new IPEndPoint(IPAddress.Parse("[1fbf:0:a88:85a3::ac1f]"), 8080);
            const string input = "[1fbf:0:a88:85a3::ac1f]";
            var converter = new ConverterWithDefaultPort();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
