using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ExifFileRenamer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExifRenamer.Tests
{
    [TestClass]
    public class ExifInfoExtractorServiceTests
    {
        private string _testDir;

        [TestInitialize]
        public void Setup()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "ExifRenamerExtractorTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_testDir);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try { Directory.Delete(_testDir, true); } catch { }
        }

        [TestMethod]
        public void TryExtractExifInfo_ExifWithoutDate_IsRecognizedAsExif()
        {
            var file = Path.Combine(_testDir, "no-date.jpg");
            using (var bitmap = new Bitmap(20, 20))
                bitmap.Save(file, ImageFormat.Jpeg);

            var writer = new ExifWriterService();
            Assert.IsTrue(writer.TrySaveExifInfo(file,
                new ExifFileRenamer.Model.ExifEditValues { MakeName = "TestMake", WriteFullTemplate = false }, out string writeError), writeError);

            var extractor = new ExifInfoExtractorService();
            var recognized = extractor.TryExtractExifInfo(file, out ExifFileRenamer.Model.ExifInfo exif, out string error);

            Assert.IsTrue(recognized, error);
            Assert.IsNotNull(exif);
            Assert.AreEqual("TestMake", exif.MakeName);
            Assert.AreEqual(default(DateTime), exif.OriginalDateTime);
        }

        [TestMethod]
        public void TryExtractExifInfo_JpegWithoutMetadata_IsNotRecognizedAsExif()
        {
            var file = Path.Combine(_testDir, "plain.jpg");
            using (var bitmap = new Bitmap(20, 20))
                bitmap.Save(file, ImageFormat.Jpeg);

            var extractor = new ExifInfoExtractorService();
            var recognized = extractor.TryExtractExifInfo(file, out ExifFileRenamer.Model.ExifInfo exif, out string error);

            Assert.IsFalse(recognized, error);
            Assert.IsNotNull(exif);
            Assert.AreEqual(default(DateTime), exif.OriginalDateTime);
        }

        [TestMethod]
        public void FileScanService_JpegWithoutExif_IsClassifiedAsNonExifImage()
        {
            var file = Path.Combine(_testDir, "plain-scan.jpg");
            using (var bitmap = new Bitmap(20, 20))
                bitmap.Save(file, ImageFormat.Jpeg);

            var service = new FileScanService();
            var result = service.CreateFileInfo(new FileInfo(file));

            Assert.IsFalse(result.IsExifImage, "An image without EXIF tags must not be classified as EXIF.");
            Assert.IsTrue(result.IsBitmapImage, "A valid image without EXIF should still be recognized as an image.");
            Assert.AreEqual("Image non EXIF", result.Status);
        }

        [TestMethod]
        public void TryExtractExifInfo_ExifWithDate_ExtractsOriginalDate()
        {
            var file = Path.Combine(_testDir, "with-date.jpg");
            using (var bitmap = new Bitmap(20, 20))
                bitmap.Save(file, ImageFormat.Jpeg);

            var expected = new DateTime(2024, 6, 15, 10, 20, 30);
            var writer = new ExifWriterService();
            Assert.IsTrue(writer.TrySaveExifInfo(file, new ExifFileRenamer.Model.ExifEditValues
            {
                OriginalDateTime = expected,
                WriteFullTemplate = false
            }, out string writeError), writeError);

            var extractor = new ExifInfoExtractorService();
            Assert.IsTrue(extractor.TryExtractExifInfo(file, out ExifFileRenamer.Model.ExifInfo exif, out string error), error);
            Assert.AreEqual(expected, exif.OriginalDateTime);
        }

    }
}
