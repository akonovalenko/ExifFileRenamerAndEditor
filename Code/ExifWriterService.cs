using ExifFileRenamer.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ExifFileRenamer
{
    /// <summary>
    /// Writes editable EXIF fields back to an image file.
    /// JPEG: lossless — only the APP1 metadata segment is rewritten, pixel data is untouched.
    /// Other formats: re-saved via System.Drawing property items.
    /// </summary>
    internal class ExifWriterService
    {
        private const short EXIF_TYPE_ASCII = 2;

        private const int TAG_DESCRIPTION = 270;
        private const int TAG_MAKE = 271;
        private const int TAG_MODEL = 272;
        private const int TAG_MODIFY_DATE = 306;
        private const int TAG_SOFTWARE = 305;
        private const int TAG_ARTIST = 315;
        private const int TAG_COPYRIGHT = 33432;
        private const int TAG_DATETIME_ORIGINAL = 36867;
        private const int TAG_DATETIME_DIGITIZED = 36868;

        // Extra tags written only for the "full template" option
        private const int TAG_ORIENTATION = 274;
        private const int TAG_X_RESOLUTION = 282;
        private const int TAG_Y_RESOLUTION = 283;
        private const int TAG_RESOLUTION_UNIT = 296;
        private const int TAG_EXIF_VERSION = 36864;
        private const int TAG_COLOR_SPACE = 40961;

        private const short EXIF_TYPE_SHORT = 3;
        private const short EXIF_TYPE_RATIONAL = 5;
        private const short EXIF_TYPE_UNDEFINED = 7;

        private const string TEMPLATE_SOFTWARE = "EXIF Image Renamer and Editor";
        private static readonly byte[] ExifVersion0232 = { (byte)'0', (byte)'2', (byte)'3', (byte)'2' };

        public bool TrySaveExifInfo(string imageFileName, ExifEditValues values, out string errors)
        {
            errors = default;
            try
            {
                var extension = Path.GetExtension(imageFileName).TrimStart('.').ToUpperInvariant();
                if (extension == "JPG" || extension == "JPEG")
                    SaveJpegLossless(imageFileName, values);
                else
                    SaveWithGdiPlus(imageFileName, values);
                return true;
            }
            catch (Exception ex)
            {
                errors = string.Format("Error source: {0}; Error message: {1};", ex.Source, ex);
                return false;
            }
        }

        /// <summary>
        /// Removes all EXIF metadata from the file. JPEG: lossless (the APP1 segment
        /// is dropped, image data untouched); other formats: re-saved without property items.
        /// </summary>
        public bool TryRemoveExifInfo(string imageFileName, out string errors)
        {
            errors = default;
            try
            {
                var extension = Path.GetExtension(imageFileName).TrimStart('.').ToUpperInvariant();
                if (extension == "JPG" || extension == "JPEG")
                {
                    var sourceBytes = File.ReadAllBytes(imageFileName);
                    var newBytes = JpegExifRewriter.RemoveExif(sourceBytes);
                    if (!ReferenceEquals(newBytes, sourceBytes))
                        ReplaceFile(imageFileName, tempFile => File.WriteAllBytes(tempFile, newBytes));
                }
                else
                {
                    var sourceBytes = File.ReadAllBytes(imageFileName);
                    using (var ms = new MemoryStream(sourceBytes))
                    using (var image = Image.FromStream(ms))
                    {
                        foreach (var propertyId in image.PropertyIdList.ToArray())
                            image.RemovePropertyItem(propertyId);
                        ReplaceFile(imageFileName, tempFile => image.Save(tempFile, image.RawFormat));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errors = string.Format("Error source: {0}; Error message: {1};", ex.Source, ex);
                return false;
            }
        }

        #region JPEG lossless path

        private static void SaveJpegLossless(string fileName, ExifEditValues values)
        {
            var ifd0Changes = new List<ExifTagChange>();
            var exifIfdChanges = new List<ExifTagChange>();

            AddTextChange(ifd0Changes, TAG_DESCRIPTION, values.Description);
            AddTextChange(ifd0Changes, TAG_MAKE, values.MakeName);
            AddTextChange(ifd0Changes, TAG_MODEL, values.ModelName);
            AddTextChange(ifd0Changes, TAG_SOFTWARE, values.Software);
            AddTextChange(ifd0Changes, TAG_ARTIST, values.Artist);
            AddTextChange(ifd0Changes, TAG_COPYRIGHT, values.Copyright);

            if (values.OriginalDateTime.HasValue)
            {
                var exifDate = FormatExifDate(values.OriginalDateTime.Value);
                ifd0Changes.Add(ExifTagChange.Text(TAG_MODIFY_DATE, exifDate));
                exifIfdChanges.Add(ExifTagChange.Text(TAG_DATETIME_ORIGINAL, exifDate));
                exifIfdChanges.Add(ExifTagChange.Text(TAG_DATETIME_DIGITIZED, exifDate));
            }

            if (values.WriteFullTemplate)
                AppendJpegTemplate(ifd0Changes, exifIfdChanges);

            if (ifd0Changes.Count == 0 && exifIfdChanges.Count == 0)
                return;

            var sourceBytes = File.ReadAllBytes(fileName);
            var newBytes = JpegExifRewriter.ApplyExif(sourceBytes, ifd0Changes, exifIfdChanges);
            if (ReferenceEquals(newBytes, sourceBytes))
                return;
            ReplaceFile(fileName, tempFile => File.WriteAllBytes(tempFile, newBytes));
        }

        private static void AddTextChange(List<ExifTagChange> changes, int tag, string value)
        {
            if (value != null)
                changes.Add(ExifTagChange.Text(tag, value));
        }

        /// <summary>
        /// Adds the standard EXIF tags with default values for tags not already set by the
        /// editable fields (which take precedence). Date tags are left to the editable step.
        /// </summary>
        private static void AppendJpegTemplate(List<ExifTagChange> ifd0, List<ExifTagChange> exif)
        {
            void AddIfMissing(List<ExifTagChange> list, ExifTagChange change)
            {
                if (!list.Exists(c => c.Tag == change.Tag))
                    list.Add(change);
            }

            AddIfMissing(ifd0, ExifTagChange.Ascii(TAG_DESCRIPTION, string.Empty));
            AddIfMissing(ifd0, ExifTagChange.Ascii(TAG_MAKE, string.Empty));
            AddIfMissing(ifd0, ExifTagChange.Ascii(TAG_MODEL, string.Empty));
            AddIfMissing(ifd0, ExifTagChange.Short(TAG_ORIENTATION, 1));
            AddIfMissing(ifd0, ExifTagChange.Rational(TAG_X_RESOLUTION, 72, 1));
            AddIfMissing(ifd0, ExifTagChange.Rational(TAG_Y_RESOLUTION, 72, 1));
            AddIfMissing(ifd0, ExifTagChange.Ascii(TAG_SOFTWARE, TEMPLATE_SOFTWARE));
            AddIfMissing(ifd0, ExifTagChange.Short(TAG_RESOLUTION_UNIT, 2)); // inches
            AddIfMissing(ifd0, ExifTagChange.Ascii(TAG_ARTIST, string.Empty));
            AddIfMissing(ifd0, ExifTagChange.Ascii(TAG_COPYRIGHT, string.Empty));

            AddIfMissing(exif, ExifTagChange.Undefined(TAG_EXIF_VERSION, ExifVersion0232));
            AddIfMissing(exif, ExifTagChange.Short(TAG_COLOR_SPACE, 1)); // sRGB
        }

        #endregion

        #region GDI+ path (non-JPEG formats)

        private static void SaveWithGdiPlus(string fileName, ExifEditValues values)
        {
            // Load from memory so the source file stays unlocked and can be replaced.
            var sourceBytes = File.ReadAllBytes(fileName);
            using (var ms = new MemoryStream(sourceBytes))
            using (var image = Image.FromStream(ms))
            {
                ApplyAsciiTag(image, TAG_DESCRIPTION, values.Description);
                ApplyAsciiTag(image, TAG_MAKE, values.MakeName);
                ApplyAsciiTag(image, TAG_MODEL, values.ModelName);
                ApplyAsciiTag(image, TAG_SOFTWARE, values.Software);
                ApplyAsciiTag(image, TAG_ARTIST, values.Artist);
                ApplyAsciiTag(image, TAG_COPYRIGHT, values.Copyright);

                if (values.OriginalDateTime.HasValue)
                {
                    var exifDate = FormatExifDate(values.OriginalDateTime.Value);
                    ApplyAsciiTag(image, TAG_DATETIME_ORIGINAL, exifDate);
                    ApplyAsciiTag(image, TAG_DATETIME_DIGITIZED, exifDate);
                    ApplyAsciiTag(image, TAG_MODIFY_DATE, exifDate);
                }

                if (values.WriteFullTemplate)
                    AppendGdiTemplate(image);

                ReplaceFile(fileName, tempFile => image.Save(tempFile, image.RawFormat));
            }
        }

        private static void ApplyAsciiTag(Image image, int tagId, string value)
        {
            if (value == null)
                return;

            if (value.Length == 0)
            {
                if (image.PropertyIdList.Contains(tagId))
                    image.RemovePropertyItem(tagId);
                return;
            }

            var data = Encoding.UTF8.GetBytes(value + "\0");
            var item = CreatePropertyItem();
            item.Id = tagId;
            item.Type = EXIF_TYPE_ASCII;
            item.Len = data.Length;
            item.Value = data;
            image.SetPropertyItem(item);
        }

        private static void AppendGdiTemplate(Image image)
        {
            void AddIfMissing(int id, short type, byte[] data)
            {
                if (!image.PropertyIdList.Contains(id))
                    ApplyRawTag(image, id, type, data);
            }

            AddIfMissing(TAG_DESCRIPTION, EXIF_TYPE_ASCII, AsciiBytes(string.Empty));
            AddIfMissing(TAG_MAKE, EXIF_TYPE_ASCII, AsciiBytes(string.Empty));
            AddIfMissing(TAG_MODEL, EXIF_TYPE_ASCII, AsciiBytes(string.Empty));
            AddIfMissing(TAG_ORIENTATION, EXIF_TYPE_SHORT, ShortBytes(1));
            AddIfMissing(TAG_X_RESOLUTION, EXIF_TYPE_RATIONAL, RationalBytes(72, 1));
            AddIfMissing(TAG_Y_RESOLUTION, EXIF_TYPE_RATIONAL, RationalBytes(72, 1));
            AddIfMissing(TAG_SOFTWARE, EXIF_TYPE_ASCII, AsciiBytes(TEMPLATE_SOFTWARE));
            AddIfMissing(TAG_RESOLUTION_UNIT, EXIF_TYPE_SHORT, ShortBytes(2));
            AddIfMissing(TAG_ARTIST, EXIF_TYPE_ASCII, AsciiBytes(string.Empty));
            AddIfMissing(TAG_COPYRIGHT, EXIF_TYPE_ASCII, AsciiBytes(string.Empty));
            AddIfMissing(TAG_EXIF_VERSION, EXIF_TYPE_UNDEFINED, ExifVersion0232);
            AddIfMissing(TAG_COLOR_SPACE, EXIF_TYPE_SHORT, ShortBytes(1));
        }

        private static void ApplyRawTag(Image image, int tagId, short type, byte[] data)
        {
            var item = CreatePropertyItem();
            item.Id = tagId;
            item.Type = type;
            item.Len = data.Length;
            item.Value = data;
            image.SetPropertyItem(item);
        }

        private static byte[] AsciiBytes(string value)
        {
            return Encoding.UTF8.GetBytes((value ?? string.Empty) + "\0");
        }

        private static byte[] ShortBytes(ushort value)
        {
            return new[] { (byte)(value & 0xFF), (byte)(value >> 8) };
        }

        private static byte[] RationalBytes(uint numerator, uint denominator)
        {
            return new[]
            {
                (byte)numerator, (byte)(numerator >> 8), (byte)(numerator >> 16), (byte)(numerator >> 24),
                (byte)denominator, (byte)(denominator >> 8), (byte)(denominator >> 16), (byte)(denominator >> 24)
            };
        }

        private static PropertyItem CreatePropertyItem()
        {
            // PropertyItem has no public constructor
            return (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
        }

        #endregion

        private static string FormatExifDate(DateTime value)
        {
            return value.ToString("yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        private static void ReplaceFile(string fileName, Action<string> writeToTemp)
        {
            var tempFile = fileName + ".exiftmp";
            var creationTime = File.GetCreationTime(fileName);
            try
            {
                writeToTemp(tempFile);
                // Atomic swap: the original stays intact if anything above failed.
                File.Replace(tempFile, fileName, null);
                File.SetCreationTime(fileName, creationTime);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }
    }
}
