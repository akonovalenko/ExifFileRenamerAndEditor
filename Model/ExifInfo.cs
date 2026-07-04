using System;

namespace ExifFileRenamer.Model
{
    /// <summary>
    /// Image file EXIF information
    /// </summary>
    internal class ExifInfo
    {
        public string Description { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string Orientation { get; set; }
        public string XResolution { get; set; }
        public string YResolution { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string ResolutionUnit { get; set; }
        public DateTime OriginalDateTime { get; set; }
        public string Software { get; set; }

        public string Artist { get; set; }
        public string ExifVersion { get; set; }

        public string Copyright { get; set; }
        public string FocalLength { get; set; }

        public string ExposureTime { get; set; }
        public string FNumber { get; set; }
        public string ISOSpeed { get; set; }
        public string Flash { get; set; }
        public string GpsLatitude { get; set; }
        public string GpsLongitude { get; set; }

        public string ExposureProgram { get; set; }
        public string MeteringMode { get; set; }
        public string ExposureBias { get; set; }
        public string WhiteBalance { get; set; }
        public string ExposureMode { get; set; }
        public string FocalLengthIn35mm { get; set; }
        public string SceneCaptureType { get; set; }
        public string ColorSpace { get; set; }

    }
}
