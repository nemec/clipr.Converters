using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace clipr.Converters.UnitTests
{
    [TestClass]
    public class RegexConverterUnitTest
    {
        [TestMethod]
        public void RegexIsValid_WithValidRegex_ReturnsTrue()
        {
            const string input = @"\d+";
            var converter = new RegexConverter();

            Assert.IsTrue(converter.IsValid(input));
        }

        [TestMethod]
        public void ConvertRegex_WithRegularExpressionInput_ParsesRegex()
        {
            var expected = new Regex(@"\d+").ToString();
            const string input = @"\d+";

            var converter = new RegexConverter();
            var actual = converter.ConvertFromInvariantString(input).ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
