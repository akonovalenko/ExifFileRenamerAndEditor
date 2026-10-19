using ExifFileRenamer;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExifImageRenamer.Services
{

    /// <summary>
    /// Rewrites the EXIF (APP1) segment of a JPEG file without re-encoding image data:
    /// only the metadata segment is replaced, compressed pixel data is copied byte for byte.
    /// </summary>
    internal static class JpegExifRewriteService
    {
        private static readonly byte[] ExifSignature = { 0x45, 0x78, 0x69, 0x66, 0x00, 0x00 }; // "Exif\0\0"
        private const int MAX_TIFF_LENGTH = 65535 - 2 - 6; // segment length field minus signature

        /// <summary>
        /// Returns new JPEG bytes with the changes applied, or the original array
        /// unchanged when there is nothing to write.
        /// </summary>
        public static byte[] ApplyExif(byte[] jpeg, List<ExifTagChange> ifd0Changes, List<ExifTagChange> exifIfdChanges)
        {
            if (jpeg == null || jpeg.Length < 4 || jpeg[0] != 0xFF || jpeg[1] != 0xD8)
                throw new InvalidOperationException("Not a valid JPEG file (SOI marker missing).");

            int app1Start = -1, app1TotalLen = 0, insertPos = 2;
            int pos = 2;
            while (pos + 4 <= jpeg.Length)
            {
                if (jpeg[pos] != 0xFF)
                    throw new InvalidOperationException("Corrupt JPEG segment structure.");
                if (jpeg[pos + 1] == 0xFF) { pos++; continue; } // fill byte
                byte marker = jpeg[pos + 1];
                if (marker == 0xDA || marker == 0xD9) // SOS / EOI: metadata section is over
                    break;
                int segLen = (jpeg[pos + 2] << 8) | jpeg[pos + 3];
                if (segLen < 2 || pos + 2 + segLen > jpeg.Length)
                    throw new InvalidOperationException("Corrupt JPEG segment length.");
                if (marker == 0xE1 && app1Start < 0 && HasExifSignature(jpeg, pos + 4, segLen - 2))
                {
                    app1Start = pos;
                    app1TotalLen = 2 + segLen;
                }
                if (marker == 0xE0 && insertPos == pos)
                    insertPos = pos + 2 + segLen; // keep JFIF APP0 first
                pos += 2 + segLen;
            }

            byte[] tiff = null;
            if (app1Start >= 0)
            {
                int tiffLen = app1TotalLen - 4 - ExifSignature.Length;
                tiff = new byte[tiffLen];
                Array.Copy(jpeg, app1Start + 4 + ExifSignature.Length, tiff, 0, tiffLen);
            }

            byte[] newTiff = TiffExifEditor.Apply(tiff, ifd0Changes, exifIfdChanges);
            if (newTiff == null && app1Start < 0)
                return jpeg; // nothing to store, nothing to remove
            if (newTiff != null && newTiff.Length > MAX_TIFF_LENGTH)
                throw new InvalidOperationException("Resulting EXIF block exceeds the JPEG APP1 segment size limit.");

            var result = new MemoryStream(jpeg.Length + 1024);
            int copyFrom;
            if (app1Start >= 0)
            {
                result.Write(jpeg, 0, app1Start);
                copyFrom = app1Start + app1TotalLen;
            }
            else
            {
                result.Write(jpeg, 0, insertPos);
                copyFrom = insertPos;
            }
            WriteApp1(result, newTiff);
            result.Write(jpeg, copyFrom, jpeg.Length - copyFrom);
            return result.ToArray();
        }

        /// <summary>
        /// Returns JPEG bytes with the EXIF (APP1) segment removed, or the original
        /// array unchanged when the file has no EXIF segment. Image data is untouched.
        /// </summary>
        public static byte[] RemoveExif(byte[] jpeg)
        {
            if (jpeg == null || jpeg.Length < 4 || jpeg[0] != 0xFF || jpeg[1] != 0xD8)
                throw new InvalidOperationException("Not a valid JPEG file (SOI marker missing).");

            var result = new MemoryStream(jpeg.Length);
            result.Write(jpeg, 0, 2); // SOI
            int pos = 2;
            bool removed = false;
            while (pos + 4 <= jpeg.Length)
            {
                if (jpeg[pos] != 0xFF)
                    throw new InvalidOperationException("Corrupt JPEG segment structure.");
                if (jpeg[pos + 1] == 0xFF) { result.WriteByte(jpeg[pos]); pos++; continue; } // fill byte
                byte marker = jpeg[pos + 1];
                if (marker == 0xDA || marker == 0xD9) // SOS / EOI: metadata section is over
                    break;
                int segLen = (jpeg[pos + 2] << 8) | jpeg[pos + 3];
                if (segLen < 2 || pos + 2 + segLen > jpeg.Length)
                    throw new InvalidOperationException("Corrupt JPEG segment length.");
                if (marker == 0xE1 && HasExifSignature(jpeg, pos + 4, segLen - 2))
                    removed = true; // skip the whole EXIF segment
                else
                    result.Write(jpeg, pos, 2 + segLen);
                pos += 2 + segLen;
            }

            if (!removed)
                return jpeg;
            result.Write(jpeg, pos, jpeg.Length - pos); // SOS + compressed data, byte for byte
            return result.ToArray();
        }

        /// <summary>
        /// Determines whether the JPEG segment at the specified offset begins with the EXIF signature.
        /// </summary>
        /// <remarks>Compares bytes against the ExifSignature constant. Does not perform null checks or
        /// validate that offset is within jpeg bounds; the caller must ensure parameters are valid.</remarks>
        /// <param name="jpeg">The byte array containing JPEG data.</param>
        /// <param name="offset">The start index within jpeg at which to check for the signature.</param>
        /// <param name="payloadLen">The available length of the segment starting at offset.</param>
        /// <returns>true if payloadLen is at least the EXIF signature length and the bytes at offset match the EXIF signature;
        /// otherwise, false.</returns>
        private static bool HasExifSignature(byte[] jpeg, int offset, int payloadLen)
        {
            if (payloadLen < ExifSignature.Length)
                return false;
            for (int i = 0; i < ExifSignature.Length; i++)
                if (jpeg[offset + i] != ExifSignature[i])
                    return false;
            return true;
        }

        /// <summary>
        /// Writes an EXIF APP1 (0xFFE1) segment containing the Exif signature followed by the provided TIFF data to the
        /// specified stream.
        /// </summary>
        /// <remarks>Writes the APP1 marker (0xFFE1), a two-byte big-endian segment length, the Exif
        /// signature, and the raw TIFF bytes. The segment length is calculated as 2 + ExifSignature.Length +
        /// tiff.Length. Returns immediately if tiff is null.</remarks>
        /// <param name="stream">The MemoryStream to which the APP1 segment is written.</param>
        /// <param name="tiff">TIFF-formatted EXIF payload to embed; if null, no APP1 segment is written.</param>
        private static void WriteApp1(MemoryStream stream, byte[] tiff)
        {
            if (tiff == null)
                return; // EXIF removed entirely
            int segLen = 2 + ExifSignature.Length + tiff.Length;
            stream.WriteByte(0xFF);
            stream.WriteByte(0xE1);
            stream.WriteByte((byte)(segLen >> 8));
            stream.WriteByte((byte)(segLen & 0xFF));
            stream.Write(ExifSignature, 0, ExifSignature.Length);
            stream.Write(tiff, 0, tiff.Length);
        }
    }
}
