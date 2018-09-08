using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace clipr.Converters.UnitTests
{
    [TestClass]
    public class TimeSpanUnitTest
    {
        [TestMethod]
        public void ConvertTimeSpan_WithStandardTimeSpanInput_CanConvert()
        {
            const string input = "12:15:10";

            var converter = new TimeSpanConverter();

            Assert.IsTrue(converter.IsValid(input));
        }

        [TestMethod]
        public void ConvertTimeSpan_WithTimeSpanNoFormatInput_CanConvert()
        {
            const string input = "12";

            var converter = new TimeSpanConverter();

            Assert.IsTrue(converter.IsValid(input));
        }

        [TestMethod]
        public void ConvertTimeSpan_WithTimeSpanMillisecondsInput_CanConvert()
        {
            const string input = "12ms";

            var converter = new TimeSpanConverter();

            Assert.IsTrue(converter.IsValid(input));
        }

        [TestMethod]
        public void ConvertTimeSpan_WithTimeSpanSecondsInput_CanConvert()
        {
            const string input = "12s";

            var converter = new TimeSpanConverter();

            Assert.IsTrue(converter.IsValid(input));
        }

        [TestMethod]
        public void ConvertTimeSpan_WithTimeSpanMinutesInput_CanConvert()
        {
            const string input = "12m";

            var converter = new TimeSpanConverter();

            Assert.IsTrue(converter.IsValid(input));
        }

        [TestMethod]
        public void ConvertTimeSpan_WithTimeSpanHoursInput_CanConvert()
        {
            const string input = "12h";

            var converter = new TimeSpanConverter();

            Assert.IsTrue(converter.IsValid(input));
        }

        [TestMethod]
        public void ConvertTimeSpan_WithTimeSpanDaysInput_CanConvert()
        {
            const string input = "12d";

            var converter = new TimeSpanConverter();

            Assert.IsTrue(converter.IsValid(input));
        }

        [TestMethod]
        public void ParseTimeSpan_WithStandardTimeSpanString_ParsesTimeSpan()
        {
            var expected = new TimeSpan(12, 15, 10);
            var input = "12:15:10";

            var converter = new TimeSpanConverter();
            var actual = converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTimeSpan_WithTimeSpanNoFormatString_ParsesTimeSpan()
        {
            var expected = new TimeSpan(0, 0, 12);
            var input = "12";

            var converter = new TimeSpanConverter();
            var actual = converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTimeSpan_WithTimeSpanMillisecondsString_ParsesTimeSpan()
        {
            var expected = new TimeSpan(0, 0, 0, 0, 12);
            var input = "12ms";

            var converter = new TimeSpanConverter();
            var actual = converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTimeSpan_WithTimeSpanSecondsString_ParsesTimeSpan()
        {
            var expected = new TimeSpan(0, 0, 12);
            var input = "12s";

            var converter = new TimeSpanConverter();
            var actual = converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTimeSpan_WithTimeSpanMinutesString_ParsesTimeSpan()
        {
            var expected = new TimeSpan(0, 12, 0);
            var input = "12m";

            var converter = new TimeSpanConverter();
            var actual = converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTimeSpan_WithTimeSpanHoursString_ParsesTimeSpan()
        {
            var expected = new TimeSpan(12, 0, 0);
            var input = "12h";

            var converter = new TimeSpanConverter();
            var actual = converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseTimeSpan_WithTimeSpanDaysString_ParsesTimeSpan()
        {
            var expected = new TimeSpan(12, 0, 0, 0);
            var input = "12d";

            var converter = new TimeSpanConverter();
            var actual = converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTimeSpan_WithNegativeTimeSpanSecondsInput_ParsesTimeSpan()
        {
            var expected = new TimeSpan(0, 0, -12);
            const string input = "-12s";

            var converter = new TimeSpanConverter();
            var actual = converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual);
        }
    }
}
