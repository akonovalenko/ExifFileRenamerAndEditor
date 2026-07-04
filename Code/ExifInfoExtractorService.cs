using ExifFileRenamer.Model;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;

namespace ExifFileRenamer
{
    internal class ExifInfoExtractorService
    {
        public bool TryExtractExifInfo(string imageFileName, out ExifInfo result, out string errors)
        {
            result = new ExifInfo();
            bool isProcessed = false;
            errors = default;

            try
            {
                using (var bitmap = Image.FromFile(imageFileName))
                {
                    if (bitmap.PropertyItems != null)
                    {
                        string gpsLatRef = null;
                        string gpsLonRef = null;

                        foreach (PropertyItem propertyItem in bitmap.PropertyItems)
                        {
                            if (propertyItem.Value == null)
                                continue;
                            int charsUsed;
                            var value = string.Empty;

                            switch (propertyItem.Type)
                            {
                                case 3:
                                    value = System.BitConverter.ToUInt16(propertyItem.Value, 0).ToString();
                                    break;
                                case 4:
                                    value = System.BitConverter.ToUInt32(propertyItem.Value, 0).ToString();
                                    break;
                                case 5:
                                    if (propertyItem.Value.Length >= 8)
                                    {
                                        var num = System.BitConverter.ToUInt32(propertyItem.Value, 0);
                                        var den = System.BitConverter.ToUInt32(propertyItem.Value, 4);
                                        value = den != 0
                                            ? ((double)num / den).ToString(CultureInfo.InvariantCulture)
                                            : num.ToString();
                                    }
                                    break;
                                case 10: // SRATIONAL (signed rational)
                                    if (propertyItem.Value.Length >= 8)
                                    {
                                        var num = System.BitConverter.ToInt32(propertyItem.Value, 0);
                                        var den = System.BitConverter.ToInt32(propertyItem.Value, 4);
                                        value = den != 0
                                            ? ((double)num / den).ToString("0.00", CultureInfo.InvariantCulture)
                                            : num.ToString();
                                    }
                                    break;
                                default:
                                    // UTF-8 is a superset of ASCII; the EXIF writer stores text tags as UTF-8
                                    Encoding textEncoding = Encoding.UTF8;
                                    System.Text.Decoder decoder = textEncoding.GetDecoder();
                                    char[] charArray = new char[propertyItem.Len];
                                    decoder.Convert(propertyItem.Value, 0, propertyItem.Value.Length, charArray, 0, charArray.Length, true, out int bytesUsed, out charsUsed, out bool completed);
                                    for (int i = 0; i < charsUsed; i++)
                                        value += charArray[i];
                                    break;
                            }

                            switch (propertyItem.Id)
                            {
                                case 1: // GPSLatitudeRef
                                    gpsLatRef = value.TrimEnd('\0');
                                    break;

                                case 2: // GPSLatitude (3× RATIONAL)
                                    if (propertyItem.Value.Length >= 24)
                                        result.GpsLatitude = FormatGps(ParseGpsDms(propertyItem.Value), gpsLatRef ?? "N");
                                    break;

                                case 3: // GPSLongitudeRef
                                    gpsLonRef = value.TrimEnd('\0');
                                    break;

                                case 4: // GPSLongitude (3× RATIONAL)
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

                                case 306: // ModifyDate — fallback if DateTimeOriginal absent
                                    if (result.OriginalDateTime == default
                                        && DateTime.TryParseExact(value.TrimEnd('\0'), "yyyy:MM:dd HH:mm:ss",
                                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt3))
                                    {
                                        result.OriginalDateTime = dt3;
                                    }
                                    break;

                                case 315:
                                    result.Artist = value.TrimEnd('\0');
                                    break;

                                case 33432:
                                    result.Copyright = value.TrimEnd('\0');
                                    break;

                                case 33434: // ExposureTime (RATIONAL)
                                    if (propertyItem.Value.Length >= 8)
                                    {
                                        var num = System.BitConverter.ToUInt32(propertyItem.Value, 0);
                                        var den = System.BitConverter.ToUInt32(propertyItem.Value, 4);
                                        if (den > 0)
                                            result.ExposureTime = num >= den
                                                ? $"{(double)num / den:0.##} s"
                                                : $"1/{(uint)Math.Round((double)den / num)} s";
                                    }
                                    break;

                                case 33437: // FNumber (RATIONAL)
                                    if (propertyItem.Value.Length >= 8)
                                    {
                                        var num = System.BitConverter.ToUInt32(propertyItem.Value, 0);
                                        var den = System.BitConverter.ToUInt32(propertyItem.Value, 4);
                                        if (den > 0)
                                            result.FNumber = $"f/{(double)num / den:0.#}";
                                    }
                                    break;

                                case 34855: // ISOSpeedRatings (SHORT)
                                    result.ISOSpeed = "ISO " + value;
                                    break;

                                case 36864:
                                    result.ExifVersion = value.TrimEnd('\0');
                                    break;

                                case 36867: // DateTimeOriginal
                                    if (DateTime.TryParseExact(value.TrimEnd('\0'), "yyyy:MM:dd HH:mm:ss",
                                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                                    {
                                        result.OriginalDateTime = dt;
                                    }
                                    break;

                                case 37385: // Flash (SHORT)
                                    if (short.TryParse(value, out short flashVal))
                                        result.Flash = (flashVal & 0x01) != 0 ? "Flash fired" : "No flash";
                                    break;

                                case 37386: // FocalLength (RATIONAL)
                                    if (propertyItem.Value.Length >= 8)
                                    {
                                        var num = System.BitConverter.ToUInt32(propertyItem.Value, 0);
                                        var den = System.BitConverter.ToUInt32(propertyItem.Value, 4);
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

                                case 34850: // ExposureProgram (SHORT)
                                    switch (value)
                                    {
                                        case "0": result.ExposureProgram = "Not defined"; break;
                                        case "1": result.ExposureProgram = "Manual"; break;
                                        case "2": result.ExposureProgram = "Normal program"; break;
                                        case "3": result.ExposureProgram = "Aperture priority"; break;
                                        case "4": result.ExposureProgram = "Shutter priority"; break;
                                        case "5": result.ExposureProgram = "Creative program"; break;
                                        case "6": result.ExposureProgram = "Action program"; break;
                                        case "7": result.ExposureProgram = "Portrait mode"; break;
                                        case "8": result.ExposureProgram = "Landscape mode"; break;
                                        default: result.ExposureProgram = value; break;
                                    }
                                    break;

                                case 37380: // ExposureBiasValue (SRATIONAL)
                                    result.ExposureBias = value + " EV";
                                    break;

                                case 37383: // MeteringMode (SHORT)
                                    switch (value)
                                    {
                                        case "1": result.MeteringMode = "Average"; break;
                                        case "2": result.MeteringMode = "Center weighted average"; break;
                                        case "3": result.MeteringMode = "Spot"; break;
                                        case "4": result.MeteringMode = "Multi-spot"; break;
                                        case "5": result.MeteringMode = "Multi-segment"; break;
                                        case "6": result.MeteringMode = "Partial"; break;
                                        case "255": result.MeteringMode = "Other"; break;
                                        default: result.MeteringMode = value; break;
                                    }
                                    break;

                                case 40961: // ColorSpace (SHORT)
                                    result.ColorSpace = value == "1" ? "sRGB" : value == "65535" ? "Uncalibrated" : value;
                                    break;

                                case 41486:
                                    result.XResolution = value;
                                    break;

                                case 41487:
                                    result.YResolution = value;
                                    break;

                                case 41986: // ExposureMode (SHORT)
                                    switch (value)
                                    {
                                        case "0": result.ExposureMode = "Auto exposure"; break;
                                        case "1": result.ExposureMode = "Manual exposure"; break;
                                        case "2": result.ExposureMode = "Auto bracket"; break;
                                        default: result.ExposureMode = value; break;
                                    }
                                    break;

                                case 41987: // WhiteBalance (SHORT)
                                    result.WhiteBalance = value == "0" ? "Auto" : value == "1" ? "Manual" : value;
                                    break;

                                case 41989: // FocalLengthIn35mmFilm (SHORT)
                                    result.FocalLengthIn35mm = value + " mm";
                                    break;

                                case 41990: // SceneCaptureType (SHORT)
                                    switch (value)
                                    {
                                        case "0": result.SceneCaptureType = "Standard"; break;
                                        case "1": result.SceneCaptureType = "Landscape"; break;
                                        case "2": result.SceneCaptureType = "Portrait"; break;
                                        case "3": result.SceneCaptureType = "Night scene"; break;
                                        default: result.SceneCaptureType = value; break;
                                    }
                                    break;
                            }

                        }//foreach
                        isProcessed = result.OriginalDateTime != default;
                    }//if
                }//using

            } //try
            catch (Exception ex)
            {
                isProcessed = false;
                errors = string.Format("Error source: {0}; Error message: {1};", ex.Source, ex);
            }
            return isProcessed;
        }

        private static double ParseGpsDms(byte[] value)
        {
            var degNum = System.BitConverter.ToUInt32(value, 0);
            var degDen = System.BitConverter.ToUInt32(value, 4);
            var minNum = System.BitConverter.ToUInt32(value, 8);
            var minDen = System.BitConverter.ToUInt32(value, 12);
            var secNum = System.BitConverter.ToUInt32(value, 16);
            var secDen = System.BitConverter.ToUInt32(value, 20);
            double deg = degDen != 0 ? (double)degNum / degDen : 0;
            double min = minDen != 0 ? (double)minNum / minDen : 0;
            double sec = secDen != 0 ? (double)secNum / secDen : 0;
            return deg + min / 60.0 + sec / 3600.0;
        }

        private static string FormatGps(double decimalDeg, string reference)
        {
            var d = (int)decimalDeg;
            var mTotal = (decimalDeg - d) * 60;
            var m = (int)mTotal;
            var s = (mTotal - m) * 60;
            return $"{d}°{m:D2}'{s:00.0}\" {reference}";
        }

    }

}
