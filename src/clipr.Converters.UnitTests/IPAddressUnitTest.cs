using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace clipr.Converters.UnitTests
{
    [TestClass]
    public class IPAddressUnitTest
    {
        [TestMethod]
        public void CanConvertFromString_WithCorrectValue_IsTrue()
        {
            const string input = "192.168.1.1";
            var converter = new IPAddressConverter();
            Assert.IsTrue(converter.CanConvertFrom(input.GetType()));
        }

        [TestMethod]
        public void CanConvertToIPEndPoint_WithCorrectValue_IsTrue()
        {
            var converter = new IPAddressConverter();
            Assert.IsTrue(converter.CanConvertTo(typeof(IPAddress)));
        }

        [TestMethod]
        public void ConvertFromString_WithCorrectValue_ParsesIPAddress()
        {
            var expected = IPAddress.Parse("192.168.1.1");
            const string input = "192.168.1.1";
            var converter = new IPAddressConverter();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertFromString_WithIPv6Address_ParsesIPAddress()
        {
            var expected = IPAddress.Parse("1fbf:0:a88:85a3::ac1f");
            const string input = "1fbf:0:a88:85a3::ac1f";
            var converter = new IPAddressConverter();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertFromString_WithIPv6AddressAndBrackets_ParsesIPAddress()
        {
            var expected = IPAddress.Parse("[1fbf:0:a88:85a3::ac1f]");
            const string input = "[1fbf:0:a88:85a3::ac1f]";
            var converter = new IPAddressConverter();

            var actual = converter.ConvertFromString(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
