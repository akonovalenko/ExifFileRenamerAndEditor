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
                new Model.ExifEditValues { MakeName = "TestMake", WriteFullTemplate = false }, out string writeError), writeError);

            var extractor = new ExifInfoExtractorService();
            var recognized = extractor.TryExtractExifInfo(file, out Model.ExifInfo exif, out string error);

            Assert.IsTrue(recognized, error);
            Assert.IsNotNull(exif);
            Assert.AreEqual("TestMake", exif.MakeName);
            Assert.AreEqual(default(DateTime), exif.OriginalDateTime);
        }
    }
}
