using System;
using System.Collections.Generic;
using ExifFileRenamer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExifRenamer.Tests
{
    [TestClass]
    public class TiffExifEditorTests
    {
        private const int TAG_DESCRIPTION = 270;
        private const int TAG_MAKE = 271;
        private const int TAG_MODEL = 272;
        private const int TAG_ARTIST = 315;
        private const int TAG_DATETIME_ORIGINAL = 36867;

        private static List<ExifTagChange> Changes(params ExifTagChange[] items)
        {
            return new List<ExifTagChange>(items);
        }

        private static byte[] CreateBaseTiff()
        {
            return TiffExifEditor.Apply(
                null,
                Changes(ExifTagChange.Text(TAG_MAKE, "TestMake"), ExifTagChange.Text(TAG_MODEL, "TestModel")),
                Changes(ExifTagChange.Text(TAG_DATETIME_ORIGINAL, "2024:06:15 10:20:30")));
        }

        [TestMethod]
        public void Apply_NullTiff_CreatesReadableTiff()
        {
            var tiff = CreateBaseTiff();

            Assert.IsNotNull(tiff);
            var tags = TestData.ReadAsciiTags(tiff);
            Assert.AreEqual("TestMake", tags[TAG_MAKE]);
            Assert.AreEqual("TestModel", tags[TAG_MODEL]);
            Assert.AreEqual("2024:06:15 10:20:30", tags[TAG_DATETIME_ORIGINAL]);
        }

        [TestMethod]
        public void Apply_UpdateWithShorterValue_PatchesWithoutGrowingBlock()
        {
            var tiff = CreateBaseTiff();

            var updated = TiffExifEditor.Apply(tiff, Changes(ExifTagChange.Text(TAG_MAKE, "X")), Changes());

            Assert.AreEqual(tiff.Length, updated.Length, "in-place patch must not grow the block");
            var tags = TestData.ReadAsciiTags(updated);
            Assert.AreEqual("X", tags[TAG_MAKE]);
            Assert.AreEqual("TestModel", tags[TAG_MODEL]);
        }

        [TestMethod]
        public void Apply_UpdateWithLongerValue_AppendsAndKeepsOtherTags()
        {
            var tiff = CreateBaseTiff();

            var longValue = new string('M', 64);
            var updated = TiffExifEditor.Apply(tiff, Changes(ExifTagChange.Text(TAG_MODEL, longValue)), Changes());

            Assert.IsTrue(updated.Length > tiff.Length);
            var tags = TestData.ReadAsciiTags(updated);
            Assert.AreEqual(longValue, tags[TAG_MODEL]);
            Assert.AreEqual("TestMake", tags[TAG_MAKE]);
            Assert.AreEqual("2024:06:15 10:20:30", tags[TAG_DATETIME_ORIGINAL]);
        }

        [TestMethod]
        public void Apply_RemoveTag_RemovesOnlyThatTag()
        {
            var tiff = CreateBaseTiff();

            var updated = TiffExifEditor.Apply(tiff, Changes(ExifTagChange.Text(TAG_MAKE, "")), Changes());

            var tags = TestData.ReadAsciiTags(updated);
            Assert.IsFalse(tags.ContainsKey(TAG_MAKE));
            Assert.AreEqual("TestModel", tags[TAG_MODEL]);
            Assert.AreEqual("2024:06:15 10:20:30", tags[TAG_DATETIME_ORIGINAL]);
        }

        [TestMethod]
        public void Apply_RemoveEverything_ReturnsNull()
        {
            var tiff = TiffExifEditor.Apply(null, Changes(ExifTagChange.Text(TAG_MAKE, "TestMake")), Changes());

            var updated = TiffExifEditor.Apply(tiff, Changes(ExifTagChange.Text(TAG_MAKE, "")), Changes());

            Assert.IsNull(updated);
        }

        [TestMethod]
        public void Apply_AddTag_KeepsExistingAndSortsEntries()
        {
            var tiff = CreateBaseTiff();

            var updated = TiffExifEditor.Apply(tiff, Changes(ExifTagChange.Text(TAG_DESCRIPTION, "New description"), ExifTagChange.Text(TAG_ARTIST, "Alex")), Changes());

            var tags = TestData.ReadAsciiTags(updated);
            Assert.AreEqual("New description", tags[TAG_DESCRIPTION]);
            Assert.AreEqual("Alex", tags[TAG_ARTIST]);
            Assert.AreEqual("TestMake", tags[TAG_MAKE]);
            Assert.AreEqual("2024:06:15 10:20:30", tags[TAG_DATETIME_ORIGINAL]);

            var order = TestData.ReadIfd0TagOrder(updated);
            for (int i = 1; i < order.Count; i++)
                Assert.IsTrue(order[i - 1] < order[i], "IFD entries must stay sorted by tag id");
        }

        [TestMethod]
        public void Apply_BigEndianTiff_UpdatesCorrectly()
        {
            // hand-crafted MM (big-endian) TIFF: one ASCII tag Make = "OldMake\0" (8 bytes) at offset 26
            var tiff = new byte[]
            {
                0x4D, 0x4D, 0x00, 0x2A, 0x00, 0x00, 0x00, 0x08, // header, IFD0 at 8
                0x00, 0x01,                                     // 1 entry
                0x01, 0x0F, 0x00, 0x02, 0x00, 0x00, 0x00, 0x08, // tag 271, ASCII, count 8
                0x00, 0x00, 0x00, 0x1A,                         // value offset 26
                0x00, 0x00, 0x00, 0x00,                         // next IFD
                (byte)'O', (byte)'l', (byte)'d', (byte)'M', (byte)'a', (byte)'k', (byte)'e', 0x00
            };

            var updated = TiffExifEditor.Apply(tiff, Changes(ExifTagChange.Text(TAG_MAKE, "NewMake"), ExifTagChange.Text(TAG_MODEL, "AddedModel")), Changes());

            Assert.AreEqual(0x4D, updated[0], "byte order must be preserved");
            var tags = TestData.ReadAsciiTags(updated);
            Assert.AreEqual("NewMake", tags[TAG_MAKE]);
            Assert.AreEqual("AddedModel", tags[TAG_MODEL]);
        }

        [TestMethod]
        public void Apply_CyrillicValue_RoundtripsAsUtf8()
        {
            var tiff = TiffExifEditor.Apply(null, Changes(ExifTagChange.Text(TAG_DESCRIPTION, "Привет, мир")), Changes());

            var tags = TestData.ReadAsciiTags(tiff);
            Assert.AreEqual("Привет, мир", tags[TAG_DESCRIPTION]);
        }

        [TestMethod]
        public void Apply_CorruptHeader_Throws()
        {
            var corrupt = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };

            Assert.ThrowsException<InvalidOperationException>(
                () => TiffExifEditor.Apply(corrupt, Changes(ExifTagChange.Text(TAG_MAKE, "X")), Changes()));
        }
    }
}
