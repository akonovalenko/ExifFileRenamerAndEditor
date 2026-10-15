using System.Text;

namespace ExifFileRenamer
{
    /// <summary>
    /// A single EXIF tag modification. Null data removes the tag from its IFD.
    /// </summary>
    public class ExifTagChange
    {
        public int Tag;
        public short Type;      // 2 = ASCII, 4 = LONG (used for IFD pointers)
        public byte[] Data;     // raw value bytes (text incl. trailing NUL); null = remove tag

        /// <summary>
        /// Create an ExifTagChange for a text EXIF tag encoded as UTF-8 with a trailing null terminator.
        /// </summary>
        /// <remarks>Encoding uses UTF-8 and appends a single 0x00 byte when value is non-empty.</remarks>
        /// <param name="tag">Exif tag identifier.</param>
        /// <param name="value">Text to encode; when empty, Data is set to null.</param>
        /// <returns>An ExifTagChange with Tag set to tag, Type set to 2, and Data containing UTF-8 bytes of value plus a
        /// terminating NUL, or null when value is empty.</returns>
        public static ExifTagChange Text(int tag, string value)
        {
            return value.Length == 0
                ? new ExifTagChange { Tag = tag, Type = 2, Data = null }
                : new ExifTagChange { Tag = tag, Type = 2, Data = Encoding.UTF8.GetBytes(value + "\0") };
        }

        /// <summary>ASCII tag that is always written, even for an empty string (stored as a lone NUL).</summary>
        public static ExifTagChange Ascii(int tag, string value)
        {
            return new ExifTagChange { Tag = tag, Type = 2, Data = Encoding.UTF8.GetBytes((value ?? string.Empty) + "\0") };
        }

        // SHORT/RATIONAL data below is little-endian. Safe because these are only used when
        // building a fresh EXIF block (Add EXIF targets files without existing metadata),
        // and TiffExifEditor creates fresh blocks in little-endian ("II") order.
        public static ExifTagChange Short(int tag, ushort value)
        {
            return new ExifTagChange { Tag = tag, Type = 3, Data = new[] { (byte)(value & 0xFF), (byte)(value >> 8) } };
        }

        /// <summary>
        /// Creates an ExifTagChange for a rational EXIF tag.
        /// </summary>
        /// <param name="tag">Exif tag identifier.</param>
        /// <param name="numerator">The numerator of the rational value.</param>
        /// <param name="denominator">The denominator of the rational value.</param>
        /// <returns>An ExifTagChange with Tag set to tag, Type set to 5, and Data containing the little-endian bytes of the rational value.</returns>
        public static ExifTagChange Rational(int tag, uint numerator, uint denominator)
        {
            return new ExifTagChange
            {
                Tag = tag,
                Type = 5,
                Data = new[]
                {
                    (byte)numerator, (byte)(numerator >> 8), (byte)(numerator >> 16), (byte)(numerator >> 24),
                    (byte)denominator, (byte)(denominator >> 8), (byte)(denominator >> 16), (byte)(denominator >> 24)
                }
            };
        }

        /// <summary>
        /// Creates an ExifTagChange for an undefined EXIF tag.
        /// </summary>
        /// <param name="tag">Exif tag identifier.</param>
        /// <param name="data">The raw bytes for the tag value.</param>
        /// <returns>An ExifTagChange with Tag set to tag, Type set to 7, and Data containing the raw bytes.</returns>
        public static ExifTagChange Undefined(int tag, byte[] data)
        {
            return new ExifTagChange { Tag = tag, Type = 7, Data = data };
        }
    }
}
