using System;

namespace ExifFileRenamer.Model
{
    /// <summary>
    /// Image file common information
    /// </summary>
    class ImageInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int HorizontalResolution { get; set; }
        public int VerticalResolution { get; set; }
        public DateTime OriginalDateTime { get; set; }
        public string PixelFormat { get; set; }

    }
}
