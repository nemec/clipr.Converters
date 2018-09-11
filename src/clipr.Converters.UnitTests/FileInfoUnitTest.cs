using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace clipr.Converters.UnitTests
{
    [TestClass]
    public class FileInfoUnitTest
    {
        [TestMethod]
        public void ConvertFile_WithNonexistentFilename_ReturnsFileInfo()
        {
            var expected = new FileInfo("folder\\samplenoexist.txt").FullName;

            var input = "folder\\samplenoexist.txt";
            var converter = new FileInfoConverter();

            var actual = (FileInfo)converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual.FullName);
        }

        [TestMethod]
        public void ConvertFile_WithExistingFilename_ReturnsFileInfoObject()
        {
            var expected = new FileInfo("folder\\sample.txt").FullName;

            var input = "folder\\sample.txt";
            var converter = new FileInfoConverter();

            var actual = (FileInfo)converter.ConvertFromInvariantString(input);

            Assert.AreEqual(expected, actual.FullName);
        }

        [TestMethod]
        public void ConvertFile_WithExistingFilename_FileInfoExistsIsTrue()
        {
            var input = "folder\\sample.txt";
            var converter = new FileInfoConverter();

            var actual = (FileInfo)converter.ConvertFromInvariantString(input);

            Assert.IsTrue(actual.Exists);
        }

        [TestMethod]
        public void ConvertFile_WithInvalidFilename_ThrowsFormatException()
        {
            var input = "folder\\fi||le.txt";
            var converter = new FileInfoConverter();

            Assert.ThrowsException<FormatException>(
                () => converter.ConvertFromInvariantString(input));
        }
    }
}
