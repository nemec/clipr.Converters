using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace clipr.Converters.UnitTests
{
    [TestClass]
    public class DirectoryInfoUnitTest
    {
        [TestMethod]
        public void ConvertDirectory_WithNonexistentDirname_ReturnsDirectoryInfo()
        {
            var expected = new DirectoryInfo("foldernoexist").FullName;

            var input = "foldernoexist";
            var converter = new DirectoryInfoConverter();

            var actual = (DirectoryInfo)converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual.FullName);
        }

        [TestMethod]
        public void ConvertDirectory_WithExistingDirname_ReturnsDirectoryInfoObject()
        {
            var expected = new DirectoryInfo("folder").FullName;

            var input = "folder";
            var converter = new DirectoryInfoConverter();

            var actual = (DirectoryInfo)converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual.FullName);
        }

        [TestMethod]
        public void ConvertDirectory_WithInvalidDirname_ThrowsFormatException()
        {
            var input = "fol||der";
            var converter = new DirectoryInfoConverter();

            Assert.ThrowsException<FormatException>(
                () => converter.ConvertFromInvariantString(input));
        }

        [TestMethod]
        public void ConvertDirectory_WithExistingDirname_DirectoryInfoExistsIsTrue()
        {
            var input = "folder";
            var converter = new DirectoryInfoConverter();

            var actual = (DirectoryInfo)converter.ConvertFromInvariantString(input);

            Assert.IsTrue(actual.Exists);
        }
    }
}
