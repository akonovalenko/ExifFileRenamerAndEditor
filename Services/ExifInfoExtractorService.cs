
using ExifFileRenamer.Model;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;

namespace ExifImageRenamer.Services
{
    /// <summary>
    /// Service that extracts EXIF metadata from image files and populates ExifInfo instances.
    /// </summary>
    /// <remarks>Reads System.Drawing.Image.PropertyItems and decodes common EXIF tags (GPS coordinates,
    /// camera make and model, capture timestamps, exposure, focal length, orientation, resolution and related fields).
    /// For JPEGs, verifies the presence of an EXIF APP1 segment ('Exif\0\0') before loading the image and scans JPEG
    /// markers safely. Exceptions are caught and returned via the errors out parameter.</remarks>
    internal class ExifInfoExtractorService
    {
        /// <summary>
        /// Extracts EXIF metadata from the specified image file and populates an ExifInfo instance.
        /// </summary>
        /// <remarks>Reads System.Drawing.Image.PropertyItems and decodes common EXIF tags (GPS, camera
        /// make/model, timestamps, exposure, focal length, orientation, resolution and related fields). Exceptions are
        /// caught and returned via the errors output.</remarks>
        /// <param name="imageFileName">Path of the image file to read.</param>
        /// <param name="result">On success, populated with extracted EXIF data; otherwise a default ExifInfo.</param>
        /// <param name="errors">Contains an error message if extraction fails; otherwise null.</param>
        /// <returns>True if EXIF metadata was found or extracted; otherwise false.</returns>
        public bool TryExtractExifInfo(string imageFileName, out ExifInfo result, out string errors)
        {
            result = new ExifInfo();
            errors = null;

            try
            {
                string extension = Path.GetExtension(imageFileName);
                bool isJpeg = extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                    || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase);

                if (isJpeg && !HasJpegExifSegment(imageFileName))
                    return false;

                using (var bitmap = Image.FromFile(imageFileName))
                {
                    var properties = bitmap.PropertyItems;
                    if (properties == null)
                        return isJpeg;

                    string gpsLatRef = null;
                    string gpsLonRef = null;

                    foreach (PropertyItem propertyItem in properties)
                    {
                        if (propertyItem.Value == null)
                            continue;

                        int charsUsed;
                        string value = string.Empty;

                        switch (propertyItem.Type)
                        {
                            case 3:
                                if (propertyItem.Value.Length >= 2)
                                    value = BitConverter.ToUInt16(propertyItem.Value, 0).ToString();
                                break;

                            case 4:
                                if (propertyItem.Value.Length >= 4)
                                    value = BitConverter.ToUInt32(propertyItem.Value, 0).ToString();
                                break;

                            case 5:
                                if (propertyItem.Value.Length >= 8)
                                {
                                    uint num = BitConverter.ToUInt32(propertyItem.Value, 0);
                                    uint den = BitConverter.ToUInt32(propertyItem.Value, 4);
                                    value = den != 0
                                        ? ((double)num / den).ToString(CultureInfo.InvariantCulture)
                                        : num.ToString();
                                }
                                break;

                            case 10:
                                if (propertyItem.Value.Length >= 8)
                                {
                                    int num = BitConverter.ToInt32(propertyItem.Value, 0);
                                    int den = BitConverter.ToInt32(propertyItem.Value, 4);
                                    value = den != 0
                                        ? ((double)num / den).ToString("0.00", CultureInfo.InvariantCulture)
                                        : num.ToString();
                                }
                                break;

                            default:
                                Encoding encoding = Encoding.UTF8;
                                Decoder decoder = encoding.GetDecoder();
                                char[] chars = new char[propertyItem.Value.Length];
                                decoder.Convert(propertyItem.Value, 0, propertyItem.Value.Length,
                                    chars, 0, chars.Length, true,
                                    out _, out charsUsed, out _);
                                value = new string(chars, 0, charsUsed);
                                break;
                        }

                        switch (propertyItem.Id)
                        {
                            case 1:
                                gpsLatRef = value.TrimEnd('\0');
                                break;
                            case 2:
                                if (propertyItem.Value.Length >= 24)
                                    result.GpsLatitude = FormatGps(ParseGpsDms(propertyItem.Value), gpsLatRef ?? "N");
                                break;
                            case 3:
                                gpsLonRef = value.TrimEnd('\0');
                                break;
                            case 4:
                                if (propertyItem.Value.Length >= 24)
                                    result.GpsLongitude = FormatGps(ParseGpsDms(propertyItem.Value), gpsLonRef ?? "E");
                                break;
                            case 270:
                                result.Description = value.TrimEnd('\0');
                                break;
                            case 271:
                                result.MakeName = value.TrimEnd('\0');
                                break;
                            case 272:
                                result.ModelName = value.TrimEnd('\0');
                                break;
                            case 274:
                                result.Orientation = value;
                                break;
                            case 296:
                                result.ResolutionUnit = value;
                                break;
                            case 305:
                                result.Software = value.TrimEnd('\0');
                                break;
                            case 306:
                                if (result.OriginalDateTime == default &&
                                    DateTime.TryParseExact(value.TrimEnd('\0'), "yyyy:MM:dd HH:mm:ss",
                                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt3))
                                    result.OriginalDateTime = dt3;
                                break;
                            case 315:
                                result.Artist = value.TrimEnd('\0');
                                break;
                            case 33432:
                                result.Copyright = value.TrimEnd('\0');
                                break;
                            case 33434:
                                if (propertyItem.Value.Length >= 8)
                                {
                                    uint num = BitConverter.ToUInt32(propertyItem.Value, 0);
                                    uint den = BitConverter.ToUInt32(propertyItem.Value, 4);
                                    if (den > 0)
                                        result.ExposureTime = num >= den
                                            ? $"{(double)num / den:0.##} s"
                                            : $"1/{(uint)Math.Round((double)den / num)} s";
                                }
                                break;
                            case 33437:
                                if (propertyItem.Value.Length >= 8)
                                {
                                    uint num = BitConverter.ToUInt32(propertyItem.Value, 0);
                                    uint den = BitConverter.ToUInt32(propertyItem.Value, 4);
                                    if (den > 0)
                                        result.FNumber = $"f/{(double)num / den:0.#}";
                                }
                                break;
                            case 34855:
                                result.ISOSpeed = "ISO " + value;
                                break;
                            case 36864:
                                result.ExifVersion = value.TrimEnd('\0');
                                break;
                            case 36867:
                                if (DateTime.TryParseExact(value.TrimEnd('\0'), "yyyy:MM:dd HH:mm:ss",
                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                                    result.OriginalDateTime = dt;
                                break;
                            case 37385:
                                if (short.TryParse(value, out short flashVal))
                                    result.Flash = (flashVal & 1) != 0 ? "Flash fired" : "No flash";
                                break;
                            case 37386:
                                if (propertyItem.Value.Length >= 8)
                                {
                                    uint num = BitConverter.ToUInt32(propertyItem.Value, 0);
                                    uint den = BitConverter.ToUInt32(propertyItem.Value, 4);
                                    if (den > 0)
                                        result.FocalLength = $"{(double)num / den:0.#} mm";
                                }
                                break;
                            case 40962:
                                result.Width = value;
                                break;
                            case 40963:
                                result.Height = value;
                                break;
                            case 34850:
                                result.ExposureProgram = value switch
                                {
                                    "0" => "Not defined",
                                    "1" => "Manual",
                                    "2" => "Normal program",
                                    "3" => "Aperture priority",
                                    "4" => "Shutter priority",
                                    "5" => "Creative program",
                                    "6" => "Action program",
                                    "7" => "Portrait mode",
                                    "8" => "Landscape mode",
                                    _ => value
                                };
                                break;
                            case 37380:
                                result.ExposureBias = value + " EV";
                                break;
                            case 37383:
                                result.MeteringMode = value switch
                                {
                                    "1" => "Average",
                                    "2" => "Center weighted average",
                                    "3" => "Spot",
                                    "4" => "Multi-spot",
                                    "5" => "Multi-segment",
                                    "6" => "Partial",
                                    "255" => "Other",
                                    _ => value
                                };
                                break;
                            case 40961:
                                result.ColorSpace = value == "1" ? "sRGB"
                                    : value == "65535" ? "Uncalibrated" : value;
                                break;
                            case 41486:
                                result.XResolution = value;
                                break;
                            case 41487:
                                result.YResolution = value;
                                break;
                            case 41986:
                                result.ExposureMode = value switch
                                {
                                    "0" => "Auto exposure",
                                    "1" => "Manual exposure",
                                    "2" => "Auto bracket",
                                    _ => value
                                };
                                break;
                            case 41987:
                                result.WhiteBalance = value == "0" ? "Auto"
                                    : value == "1" ? "Manual" : value;
                                break;
                            case 41989:
                                result.FocalLengthIn35mm = value + " mm";
                                break;
                            case 41990:
                                result.SceneCaptureType = value switch
                                {
                                    "0" => "Standard",
                                    "1" => "Landscape",
                                    "2" => "Portrait",
                                    "3" => "Night scene",
                                    _ => value
                                };
                                break;
                        }
                    }

                    if (isJpeg)
                        return true;

                    bool hasGpsIfd = properties.Any(item => item.Id == 34853);
                    return properties.Any(item => IsExifMetadataTag(item) ||
                        (hasGpsIfd && item.Id >= 1 && item.Id <= 4));
                }
            }
            catch (Exception ex)
            {
                errors = $"Error source: {ex.Source}; Error message: {ex}";
                return false;
            }
        }

        #region Private methods

        /// <summary>
        /// Checks whether the specified JPEG file contains an EXIF APP1 segment (marker 0xE1 with signature
        /// 'Exif\0\0').
        /// </summary>
        /// <remarks>Scans the file sequentially, verifies the SOI marker, iterates JPEG markers (handling
        /// padding and standalone markers), and inspects APP1 payloads for the ASCII signature 'Exif\0\0'. Returns
        /// false for truncated or malformed JPEGs or if the scan reaches compressed image data (SOS or EOI) without
        /// finding the signature.</remarks>
        /// <param name="fileName">Path to the JPEG file to inspect.</param>
        /// <returns>True if an EXIF APP1 segment is present; otherwise, false.</returns>
        private static bool HasJpegExifSegment(string fileName)
        {
            using var stream = File.OpenRead(fileName);
            using var reader = new BinaryReader(stream);

            if (stream.Length < 4 ||
                reader.ReadByte() != 0xFF ||
                reader.ReadByte() != 0xD8)
                return false;

            while (stream.Position < stream.Length)
            {
                // Every marker starts with FF. Repeated FF bytes are padding.
                if (reader.ReadByte() != 0xFF)
                    return false;

                byte marker;
                do
                {
                    if (stream.Position >= stream.Length)
                    {
                        return false;
                    }

                    marker = reader.ReadByte();
                } while (marker == 0xFF);

                // Stop before the compressed image data.
                if (marker == 0xDA || marker == 0xD9)
                {
                    return false;
                }

                // Standalone markers do not have a length field.
                if (marker == 0x01 || (marker >= 0xD0 && marker <= 0xD7))
                {
                    continue;
                }

                if (stream.Position + 2 > stream.Length)
                {
                    return false;
                }

                int length = (reader.ReadByte() << 8) | reader.ReadByte();
                if (length < 2 || stream.Position + length - 2 > stream.Length)
                {
                    return false;
                }

                int payloadLength = length - 2;

                if (marker == 0xE1 && payloadLength >= 6)
                {
                    byte[] signature = reader.ReadBytes(6);
                    if (signature.Length == 6 &&
                        signature[0] == (byte)'E' &&
                        signature[1] == (byte)'x' &&
                        signature[2] == (byte)'i' &&
                        signature[3] == (byte)'f' &&
                        signature[4] == 0 &&
                        signature[5] == 0)
                        return true;

                    stream.Position += payloadLength - 6;
                }
                else
                {
                    stream.Position += payloadLength;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified PropertyItem corresponds to a recognized EXIF metadata tag.
        /// </summary>
        /// <remarks>Compares the item's Id against a predefined list of EXIF tag identifiers used for
        /// image metadata.</remarks>
        /// <param name="item">PropertyItem to test for membership in the set of EXIF tags.</param>
        /// <returns>true if the PropertyItem's Id matches a known EXIF tag identifier; otherwise, false.</returns>
        private static bool IsExifMetadataTag(PropertyItem item)
        {
            switch (item.Id)
            {
                case 11:    // ProcessingSoftware - software used to process the image
                case 270:   // ImageDescription - description of the image
                case 271:   // Make - camera manufacturer
                case 272:   // Model - camera model
                case 274:   // Orientation - image orientation
                case 305:   // Software - software used to create or modify the image
                case 306:   // DateTime - date and time of image modification
                case 315:   // Artist - image author
                case 33432: // Copyright - copyright information
                case 33434: // ExposureTime - exposure time
                case 33437: // FNumber - lens f-number (aperture)
                case 34850: // ExposureProgram - exposure program
                case 34853: // GPSInfo - GPS metadata
                case 34855: // ISOSpeedRatings - ISO sensitivity
                case 36864: // ExifVersion - EXIF version
                case 36867: // DateTimeOriginal - original date and time of capture
                case 36868: // DateTimeDigitized - date and time of digitization
                case 37121: // ComponentsConfiguration - color component configuration
                case 37122: // CompressedBitsPerPixel - compressed bits per pixel
                case 37377: // ShutterSpeedValue - shutter speed value (APEX)
                case 37378: // ApertureValue - aperture value (APEX)
                case 37380: // ExposureBiasValue - exposure compensation
                case 37383: // MeteringMode - exposure metering mode
                case 37385: // Flash - flash status and mode
                case 37386: // FocalLength - focal length of the lens
                case 37500: // MakerNote - manufacturer-specific metadata
                case 37510: // UserComment - user comment
                case 40960: // FlashpixVersion - FlashPix version
                case 40961: // ColorSpace - color space
                case 40965: // InteroperabilityIFDPointer - pointer to interoperability information
                case 41483: // FlashEnergy - flash energy
                case 41486: // FocalPlaneXResolution - focal plane X resolution
                case 41487: // FocalPlaneYResolution - focal plane Y resolution
                case 41488: // FocalPlaneResolutionUnit - focal plane resolution unit
                case 41492: // SubjectLocation - subject location within the image
                case 41493: // ExposureIndex - exposure index
                case 41728: // FileSource - source of the image
                case 41729: // SceneType - type of scene
                case 41986: // ExposureMode - exposure mode
                case 41987: // WhiteBalance - white balance mode
                case 41989: // FocalLengthIn35mmFilm - 35 mm equivalent focal length
                case 41990: // SceneCaptureType - type of scene capture
                case 42016: // CameraLabel - camera label
                case 42032: // CameraOwnerName - camera owner's name
                case 42033: // BodySerialNumber - camera body serial number
                case 42034: // LensSpecification - lens specifications
                case 42035: // LensMake - lens manufacturer
                case 42036: // LensModel - lens model
                case 42037: // LensSerialNumber - lens serial number
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Convert GPS coordinates encoded as degrees/minutes/seconds rationals into decimal degrees.
        /// </summary>
        /// <param name="value">Byte array containing six 32-bit unsigned rationals (numerator/denominator pairs) in order: degrees
        /// numerator, degrees denominator, minutes numerator, minutes denominator, seconds numerator, seconds
        /// denominator.</param>
        /// <returns>Decimal degrees as a double (degrees + minutes/60 + seconds/3600). If a denominator is zero, the
        /// corresponding component is treated as zero.</returns>
        private static double ParseGpsDms(byte[] value)
        {
            uint degNum = BitConverter.ToUInt32(value, 0);
            uint degDen = BitConverter.ToUInt32(value, 4);
            uint minNum = BitConverter.ToUInt32(value, 8);
            uint minDen = BitConverter.ToUInt32(value, 12);
            uint secNum = BitConverter.ToUInt32(value, 16);
            uint secDen = BitConverter.ToUInt32(value, 20);

            double deg = degDen != 0 ? (double)degNum / degDen : 0;
            double min = minDen != 0 ? (double)minNum / minDen : 0;
            double sec = secDen != 0 ? (double)secNum / secDen : 0;

            return deg + min / 60.0 + sec / 3600.0;
        }

        /// <summary>
        /// Format a decimal-degree angle as D°MM'SS.S" followed by a reference suffix.
        /// </summary>
        /// <remarks>Degrees are the integer part of the input; minutes and seconds are derived from the
        /// fractional parts. Seconds are formatted with a single decimal digit.</remarks>
        /// <param name="decimalDeg">Decimal degrees value to format.</param>
        /// <param name="reference">Reference suffix appended to the result (for example, 'N', 'S', 'E', or 'W').</param>
        /// <returns>A string formatted as D°MM'SS.S" <reference>, with minutes zero-padded and seconds shown with one decimal
        /// place.</returns>
        private static string FormatGps(double decimalDeg, string reference)
        {
            var d = (int)decimalDeg;
            var mTotal = (decimalDeg - d) * 60;
            var m = (int)mTotal;
            var s = (mTotal - m) * 60;

            return $"{d}°{m:D2}'{s:00.0}\" {reference}";
        }

        #endregion
    }
}
