using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using ExifFileRenamer;
using ExifFileRenamer.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExifRenamer.Tests
{
    /// <summary>
    /// Integration tests: real files on disk, results verified through GDI+.
    /// </summary>
    [TestClass]
    public class ExifWriterServiceTests
    {
        private string _testDir;

        [TestInitialize]
        public void Setup()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "ExifRenamerTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_testDir);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try { Directory.Delete(_testDir, true); } catch { }
        }

        private string CreateJpeg(string name)
        {
            var path = Path.Combine(_testDir, name);
            using (var bmp = new Bitmap(40, 30))
                bmp.Save(path, ImageFormat.Jpeg);
            return path;
        }

        private static string ReadAsciiTag(string fileName, int tagId)
        {
            using (var image = Image.FromFile(fileName))
            {
                var item = image.PropertyItems.FirstOrDefault(p => p.Id == tagId);
                return item == null ? null : Encoding.UTF8.GetString(item.Value).TrimEnd('\0');
            }
        }

        [TestMethod]
        public void TrySaveExifInfo_Jpeg_TagsReadableByGdiPlus()
        {
            var file = CreateJpeg("save.jpg");
            var service = new ExifWriterService();

            var ok = service.TrySaveExifInfo(file, new ExifEditValues
            {
                OriginalDateTime = new DateTime(2024, 6, 15, 10, 20, 30),
                MakeName = "TestMake",
                Artist = "Alex"
            }, out string errors);

            Assert.IsTrue(ok, errors);
            Assert.AreEqual("TestMake", ReadAsciiTag(file, 271));
            Assert.AreEqual("Alex", ReadAsciiTag(file, 315));
            Assert.AreEqual("2024:06:15 10:20:30", ReadAsciiTag(file, 36867));
        }

        [TestMethod]
        public void TrySaveExifInfo_ShiftedDate_OverwritesPreviousDate()
        {
            var file = CreateJpeg("shift.jpg");
            var service = new ExifWriterService();
            var initial = new DateTime(2024, 6, 15, 10, 20, 30);
            Assert.IsTrue(service.TrySaveExifInfo(file, new ExifEditValues { OriginalDateTime = initial }, out _));

            // simulates the "Shift Dates" feature: read date, add offset, write back
            var ok = service.TrySaveExifInfo(file,
                new ExifEditValues { OriginalDateTime = initial + new TimeSpan(1, 3, 0, 0) }, out string errors);

            Assert.IsTrue(ok, errors);
            Assert.AreEqual("2024:06:16 13:20:30", ReadAsciiTag(file, 36867));
            Assert.AreEqual("2024:06:16 13:20:30", ReadAsciiTag(file, 306));
        }

        [TestMethod]
        public void TryRemoveExifInfo_Jpeg_RemovesAllMetadata()
        {
            var file = CreateJpeg("strip.jpg");
            var service = new ExifWriterService();
            Assert.IsTrue(service.TrySaveExifInfo(file, new ExifEditValues
            {
                OriginalDateTime = new DateTime(2024, 6, 15, 10, 20, 30),
                MakeName = "TestMake"
            }, out _));

            var ok = service.TryRemoveExifInfo(file, out string errors);

            Assert.IsTrue(ok, errors);
            var bytes = File.ReadAllBytes(file);
            Assert.IsNull(TestData.GetExifTiff(bytes), "APP1 segment must be gone");
            using (var image = Image.FromFile(file))
                Assert.IsFalse(image.PropertyIdList.Any(id => id == 271 || id == 36867));
        }

        [TestMethod]
        public void TryRemoveExifInfo_Jpeg_KeepsImageDecodable()
        {
            var file = CreateJpeg("decodable.jpg");
            var service = new ExifWriterService();
            Assert.IsTrue(service.TrySaveExifInfo(file, new ExifEditValues { MakeName = "TestMake" }, out _));

            Assert.IsTrue(service.TryRemoveExifInfo(file, out string errors), errors);

            using (var image = Image.FromFile(file))
            {
                Assert.AreEqual(40, image.Width);
                Assert.AreEqual(30, image.Height);
            }
        }

        [TestMethod]
        public void TrySaveExifInfo_WithoutTemplate_WritesOnlyGivenFields()
        {
            var file = CreateJpeg("no-template.jpg");
            var service = new ExifWriterService();

            Assert.IsTrue(service.TrySaveExifInfo(file,
                new ExifEditValues { MakeName = "Canon", WriteFullTemplate = false }, out string errors), errors);

            using (var image = Image.FromFile(file))
            {
                Assert.AreEqual("Canon", ReadAsciiTag(file, 271));
                // no template tags written
                Assert.IsFalse(image.PropertyIdList.Contains(274), "Orientation must not be written");
                Assert.IsFalse(image.PropertyIdList.Contains(305), "Software must not be written");
                Assert.IsFalse(image.PropertyIdList.Contains(36864), "ExifVersion must not be written");
            }
        }

        [TestMethod]
        public void TrySaveExifInfo_WithTemplate_WritesFullStandardSet()
        {
            var file = CreateJpeg("template.jpg");
            var service = new ExifWriterService();

            Assert.IsTrue(service.TrySaveExifInfo(file, new ExifEditValues
            {
                OriginalDateTime = new DateTime(2024, 6, 15, 10, 20, 30),
                MakeName = "Canon",
                WriteFullTemplate = true
            }, out string errors), errors);

            using (var image = Image.FromFile(file))
            {
                var ids = image.PropertyIdList;
                // editable value wins over template default
                Assert.AreEqual("Canon", ReadAsciiTag(file, 271));
                Assert.AreEqual("2024:06:15 10:20:30", ReadAsciiTag(file, 36867));
                // template technical tags present with defaults
                foreach (var id in new[] { 270, 272, 274, 282, 283, 296, 305, 315, 33432, 36864, 40961 })
                    Assert.IsTrue(ids.Contains(id), "template tag missing: " + id);

                var orientation = image.GetPropertyItem(274);
                Assert.AreEqual(3, orientation.Type);                 // SHORT
                Assert.AreEqual(1, BitConverter.ToUInt16(orientation.Value, 0));
                var xres = image.GetPropertyItem(282);
                Assert.AreEqual(5, xres.Type);                        // RATIONAL
                Assert.AreEqual(72u, BitConverter.ToUInt32(xres.Value, 0));
                Assert.AreEqual(1u, BitConverter.ToUInt32(xres.Value, 4));
                Assert.AreEqual("EXIF Image Renamer and Editor", ReadAsciiTag(file, 305));
            }
        }

        [TestMethod]
        public void TrySaveExifInfo_MissingFile_ReturnsError()
        {
            var service = new ExifWriterService();

            var ok = service.TrySaveExifInfo(Path.Combine(_testDir, "missing.jpg"),
                new ExifEditValues { MakeName = "X" }, out string errors);

            Assert.IsFalse(ok);
            Assert.IsNotNull(errors);
        }
    }
}
