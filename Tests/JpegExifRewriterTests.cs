using System;
using System.Collections.Generic;
using ExifFileRenamer;
using ExifImageRenamer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExifRenamer.Tests
{
    [TestClass]
    public class JpegExifRewriterTests
    {
        private const int TAG_MAKE = 271;
        private const int TAG_ARTIST = 315;
        private const int TAG_DATETIME_ORIGINAL = 36867;

        private static List<ExifTagChange> Changes(params ExifTagChange[] items)
        {
            return new List<ExifTagChange>(items);
        }

        private static List<ExifTagChange> NoChanges()
        {
            return new List<ExifTagChange>();
        }

        [TestMethod]
        public void ApplyExif_JpegWithoutExif_AddsApp1AndKeepsScanData()
        {
            var jpeg = TestData.BuildJpeg(null);

            var result = JpegExifRewriteService.ApplyExif(jpeg,
                Changes(ExifTagChange.Text(TAG_MAKE, "TestMake")),
                Changes(ExifTagChange.Text(TAG_DATETIME_ORIGINAL, "2024:06:15 10:20:30")));

            CollectionAssert.AreEqual(TestData.GetScanSection(jpeg), TestData.GetScanSection(result),
                "compressed data must be byte-identical");
            var tags = TestData.ReadAsciiTags(TestData.GetExifTiff(result));
            Assert.AreEqual("TestMake", tags[TAG_MAKE]);
            Assert.AreEqual("2024:06:15 10:20:30", tags[TAG_DATETIME_ORIGINAL]);
        }

        [TestMethod]
        public void ApplyExif_JpegWithExif_ReplacesSegmentAndKeepsScanData()
        {
            var originalTiff = TiffExifEditor.Apply(null,
                Changes(ExifTagChange.Text(TAG_MAKE, "OldMake"), ExifTagChange.Text(TAG_ARTIST, "Alex")),
                NoChanges());
            var jpeg = TestData.BuildJpeg(originalTiff);

            var result = JpegExifRewriteService.ApplyExif(jpeg, Changes(ExifTagChange.Text(TAG_MAKE, "NewMake")), NoChanges());

            CollectionAssert.AreEqual(TestData.GetScanSection(jpeg), TestData.GetScanSection(result));
            var tags = TestData.ReadAsciiTags(TestData.GetExifTiff(result));
            Assert.AreEqual("NewMake", tags[TAG_MAKE]);
            Assert.AreEqual("Alex", tags[TAG_ARTIST], "untouched tag must survive");
        }

        [TestMethod]
        public void RemoveExif_RemovesOnlyTheExifSegment()
        {
            var tiff = TiffExifEditor.Apply(null, Changes(ExifTagChange.Text(TAG_MAKE, "TestMake")), NoChanges());
            var jpegWithExif = TestData.BuildJpeg(tiff);
            var jpegWithoutExif = TestData.BuildJpeg(null);

            var result = JpegExifRewriteService.RemoveExif(jpegWithExif);

            CollectionAssert.AreEqual(jpegWithoutExif, result,
                "removing EXIF must produce exactly the same file as one built without EXIF");
        }

        [TestMethod]
        public void RemoveExif_NoExifSegment_ReturnsSameArray()
        {
            var jpeg = TestData.BuildJpeg(null);

            var result = JpegExifRewriteService.RemoveExif(jpeg);

            Assert.AreSame(jpeg, result);
        }

        [TestMethod]
        public void ApplyExif_RemovalOnlyOnCleanJpeg_ReturnsSameArray()
        {
            var jpeg = TestData.BuildJpeg(null);

            var result = JpegExifRewriteService.ApplyExif(jpeg, Changes(ExifTagChange.Text(TAG_MAKE, "")), NoChanges());

            Assert.AreSame(jpeg, result, "nothing to write and nothing to remove — no rewrite expected");
        }

        [TestMethod]
        public void ApplyExif_InvalidJpeg_Throws()
        {
            var notJpeg = new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44 };

            Assert.ThrowsException<InvalidOperationException>(
                () => JpegExifRewriteService.ApplyExif(notJpeg, Changes(ExifTagChange.Text(TAG_MAKE, "X")), NoChanges()));
        }

        [TestMethod]
        public void ApplyExif_SecondEditOfSameFile_KeepsScanDataAndOtherTags()
        {
            var jpeg = TestData.BuildJpeg(null);
            var afterFirst = JpegExifRewriteService.ApplyExif(jpeg,
                Changes(ExifTagChange.Text(TAG_MAKE, "Make1"), ExifTagChange.Text(TAG_ARTIST, "Alex")),
                Changes(ExifTagChange.Text(TAG_DATETIME_ORIGINAL, "2024:06:15 10:20:30")));

            var afterSecond = JpegExifRewriteService.ApplyExif(afterFirst,
                Changes(ExifTagChange.Text(TAG_MAKE, "Make2-longer-than-before")),
                NoChanges());

            CollectionAssert.AreEqual(TestData.GetScanSection(jpeg), TestData.GetScanSection(afterSecond));
            var tags = TestData.ReadAsciiTags(TestData.GetExifTiff(afterSecond));
            Assert.AreEqual("Make2-longer-than-before", tags[TAG_MAKE]);
            Assert.AreEqual("Alex", tags[TAG_ARTIST]);
            Assert.AreEqual("2024:06:15 10:20:30", tags[TAG_DATETIME_ORIGINAL]);
        }

        [TestMethod]
        public void ApplyExif_TruncatedSegmentLength_Throws()
        {
            // SOI followed by APP0 declaring a segment larger than the available buffer.
            var truncated = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x01, 0x02 };

            Assert.ThrowsException<InvalidOperationException>(
                () => JpegExifRewriteService.ApplyExif(truncated,
                    Changes(ExifTagChange.Text(TAG_MAKE, "X")), NoChanges()));
        }

        [TestMethod]
        public void RemoveExif_TruncatedSegmentLength_Throws()
        {
            var truncated = new byte[] { 0xFF, 0xD8, 0xFF, 0xE1, 0x00, 0x10, 0x45, 0x78 };

            Assert.ThrowsException<InvalidOperationException>(() => JpegExifRewriteService.RemoveExif(truncated));
        }

    }
}
