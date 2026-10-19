using ExifFileRenamer;
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

namespace ExifImageRenamer.Services
{
    /// <summary>
    /// Writes editable EXIF fields back to an image file.
    /// JPEG: lossless — only the APP1 metadata segment is rewritten, pixel data is untouched.
    /// Other formats: re-saved via System.Drawing property items.
    /// </summary>
    internal class ExifWriterService
    {
        #region Private constants for EXIF tags and types
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

        #endregion

        /// <summary>
        /// Saves the editable EXIF fields to the file. JPEG: lossless (the APP1 segment is rewritten, image data untouched); other formats: re-saved with property items.
        /// </summary>
        /// <param name="imageFileName">The path to the image file.</param>
        /// <param name="values">The EXIF values to save.</param>
        /// <param name="errors">Any errors that occurred during saving.</param>
        /// <returns>True if saving was successful, otherwise false.</returns>
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
                    var newBytes = JpegExifRewriteService.RemoveExif(sourceBytes);
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

        /// <summary>
        /// Saves the editable EXIF fields to a JPEG file using lossless rewriting of the APP1 segment. 
        /// The pixel data is untouched. If any error occurs during saving, the original file remains intact.
        /// </summary>
        /// <param name="fileName">The path to the JPEG file.</param>
        /// <param name="values">The EXIF values to save.</param>
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
            var newBytes = JpegExifRewriteService.ApplyExif(sourceBytes, ifd0Changes, exifIfdChanges);
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

        /// <summary>
        /// Saves the editable EXIF fields to a non-JPEG image file using GDI+. 
        /// The image is loaded into memory, modified, and then saved back to the original file. 
        /// If any error occurs during saving, the original file remains intact.
        /// </summary>
        /// <param name="fileName">The path to the image file.</param>
        /// <param name="values">The EXIF values to save.</param>
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

        /// <summary>
        /// Applies an ASCII EXIF tag to the image. If the value is null, the tag is not modified. 
        /// If the value is an empty string, the tag is removed. Otherwise, the tag is set to the provided value.
        /// </summary>
        /// <param name="image">The image to modify.</param>
        /// <param name="tagId">The ID of the EXIF tag to apply.</param>
        /// <param name="value">The value to set for the tag.</param>
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

        /// <summary>
        /// Adds the standard EXIF tags with default values for tags not already set by the
        /// </summary>
        /// <param name="image">The image to modify.</param>
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

        /// <summary>
        /// Applies a raw EXIF tag to the image with the specified ID, type, and data.
        /// </summary>
        /// <param name="image">The image to modify.</param>
        /// <param name="tagId">The ID of the EXIF tag to apply.</param>
        /// <param name="type">The type of the EXIF tag.</param>
        /// <param name="data">The data for the EXIF tag.</param>
        private static void ApplyRawTag(Image image, int tagId, short type, byte[] data)
        {
            var item = CreatePropertyItem();
            item.Id = tagId;
            item.Type = type;
            item.Len = data.Length;
            item.Value = data;
            image.SetPropertyItem(item);
        }

        /// <summary>
        /// Converts a string to a null-terminated ASCII byte array for EXIF storage. 
        /// If the input string is null, it returns an empty string with a null terminator.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The null-terminated ASCII byte array.</returns>
        private static byte[] AsciiBytes(string value)
        {
            return Encoding.UTF8.GetBytes((value ?? string.Empty) + "\0");
        }

        /// <summary>
        /// Converts a ushort value to a byte array in little-endian format for EXIF storage.
        /// </summary>
        /// <param name="value">The ushort value to convert.</param>
        /// <returns>The byte array in little-endian format.</returns>
        private static byte[] ShortBytes(ushort value)
        {
            return new[] { (byte)(value & 0xFF), (byte)(value >> 8) };
        }

        /// <summary>
        /// Converts a numerator and denominator to a byte array representing a rational number 
        /// in little-endian format for EXIF storage.
        /// </summary>
        /// <param name="numerator">The numerator of the rational number.</param>
        /// <param name="denominator">The denominator of the rational number.</param>
        /// <returns>The byte array representing the rational number.</returns>
        private static byte[] RationalBytes(uint numerator, uint denominator)
        {
            return new[]
            {
                (byte)numerator, (byte)(numerator >> 8), (byte)(numerator >> 16), (byte)(numerator >> 24),
                (byte)denominator, (byte)(denominator >> 8), (byte)(denominator >> 16), (byte)(denominator >> 24)
            };
        }

        /// <summary>
        /// Creates an uninitialized PropertyItem instance using FormatterServices.
        /// </summary>
        /// <returns>The uninitialized PropertyItem instance.</returns>
        private static PropertyItem CreatePropertyItem()
        {
            // PropertyItem has no public constructor
            return (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
        }

        #endregion

        /// <summary>
        /// Formats a DateTime value into the EXIF date string format "yyyy:MM:dd HH:mm:ss".
        /// </summary>
        /// <param name="value">The DateTime value to format.</param>
        /// <returns>The formatted EXIF date string.</returns>
        private static string FormatExifDate(DateTime value)
        {
            return value.ToString("yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Replaces the original file with a temporary file atomically. The original file remains intact if any error occurs during the writing process.
        /// </summary>
        /// <param name="fileName">The path to the original file.</param>
        /// <param name="writeToTemp">A delegate that writes the temporary file.</param>
        private static void ReplaceFile(string fileName, Action<string> writeToTemp)
        {
            var directory = Path.GetDirectoryName(fileName);
            var fileNameOnly = Path.GetFileName(fileName);
            var tempFile = Path.Combine(directory ?? string.Empty,
                "." + fileNameOnly + "." + Guid.NewGuid().ToString("N") + ".exiftmp");
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
