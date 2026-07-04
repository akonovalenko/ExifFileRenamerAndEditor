using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExifRenamer.Tests
{
    /// <summary>
    /// Builders and parsers for synthetic JPEG/TIFF structures used by the tests.
    /// </summary>
    internal static class TestData
    {
        public static readonly byte[] ScanData = { 0x11, 0x22, 0x33, 0xFF, 0x00, 0x44, 0x55 };

        /// <summary>Builds a minimal structurally valid JPEG: SOI, APP0, [APP1], DQT, SOS, scan, EOI.</summary>
        public static byte[] BuildJpeg(byte[] exifTiff)
        {
            var ms = new MemoryStream();
            ms.Write(new byte[] { 0xFF, 0xD8 }, 0, 2); // SOI

            // APP0 / JFIF, payload 14 bytes → segment length 16
            var app0 = new byte[] { 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01, 0x01, 0x00, 0x00, 0x60, 0x00, 0x60, 0x00, 0x00 };
            ms.Write(app0, 0, app0.Length);

            if (exifTiff != null)
            {
                int segLen = 2 + 6 + exifTiff.Length;
                ms.WriteByte(0xFF);
                ms.WriteByte(0xE1);
                ms.WriteByte((byte)(segLen >> 8));
                ms.WriteByte((byte)(segLen & 0xFF));
                ms.Write(Encoding.ASCII.GetBytes("Exif\0\0"), 0, 6);
                ms.Write(exifTiff, 0, exifTiff.Length);
            }

            // dummy DQT, payload 3 bytes
            var dqt = new byte[] { 0xFF, 0xDB, 0x00, 0x05, 0xAA, 0xBB, 0xCC };
            ms.Write(dqt, 0, dqt.Length);

            // SOS header, then entropy data, then EOI
            var sos = new byte[] { 0xFF, 0xDA, 0x00, 0x08, 0x01, 0x01, 0x00, 0x00, 0x3F, 0x00 };
            ms.Write(sos, 0, sos.Length);
            ms.Write(ScanData, 0, ScanData.Length);
            ms.Write(new byte[] { 0xFF, 0xD9 }, 0, 2); // EOI

            return ms.ToArray();
        }

        /// <summary>Returns everything from the SOS marker to the end of the file.</summary>
        public static byte[] GetScanSection(byte[] jpeg)
        {
            int pos = 2;
            while (pos + 4 <= jpeg.Length)
            {
                if (jpeg[pos] != 0xFF)
                    throw new InvalidOperationException("Corrupt segment chain at " + pos);
                if (jpeg[pos + 1] == 0xDA)
                {
                    var result = new byte[jpeg.Length - pos];
                    Array.Copy(jpeg, pos, result, 0, result.Length);
                    return result;
                }
                int segLen = (jpeg[pos + 2] << 8) | jpeg[pos + 3];
                pos += 2 + segLen;
            }
            throw new InvalidOperationException("SOS not found");
        }

        /// <summary>Extracts the TIFF payload of the EXIF APP1 segment, or null if absent.</summary>
        public static byte[] GetExifTiff(byte[] jpeg)
        {
            int pos = 2;
            while (pos + 4 <= jpeg.Length)
            {
                byte marker = jpeg[pos + 1];
                if (marker == 0xDA)
                    return null;
                int segLen = (jpeg[pos + 2] << 8) | jpeg[pos + 3];
                if (marker == 0xE1 && segLen > 8
                    && jpeg[pos + 4] == 'E' && jpeg[pos + 5] == 'x' && jpeg[pos + 6] == 'i' && jpeg[pos + 7] == 'f')
                {
                    var tiff = new byte[segLen - 2 - 6];
                    Array.Copy(jpeg, pos + 10, tiff, 0, tiff.Length);
                    return tiff;
                }
                pos += 2 + segLen;
            }
            return null;
        }

        /// <summary>Reads all ASCII tags of IFD0 and the Exif sub-IFD into a dictionary.</summary>
        public static Dictionary<int, string> ReadAsciiTags(byte[] tiff)
        {
            var tags = new Dictionary<int, string>();
            bool le = tiff[0] == 0x49;
            ReadIfd(tiff, (int)U32(tiff, 4, le), le, tags);
            return tags;
        }

        /// <summary>Returns tag ids of one IFD in their physical order (for sort-order asserts).</summary>
        public static List<int> ReadIfd0TagOrder(byte[] tiff)
        {
            bool le = tiff[0] == 0x49;
            int ifdOff = (int)U32(tiff, 4, le);
            int count = U16(tiff, ifdOff, le);
            var order = new List<int>();
            for (int i = 0; i < count; i++)
                order.Add(U16(tiff, ifdOff + 2 + i * 12, le));
            return order;
        }

        private static void ReadIfd(byte[] tiff, int ifdOff, bool le, Dictionary<int, string> tags)
        {
            if (ifdOff == 0)
                return;
            int count = U16(tiff, ifdOff, le);
            for (int i = 0; i < count; i++)
            {
                int entry = ifdOff + 2 + i * 12;
                int tag = U16(tiff, entry, le);
                int type = U16(tiff, entry + 2, le);
                int valueCount = (int)U32(tiff, entry + 4, le);

                if (tag == 0x8769) // Exif sub-IFD pointer
                {
                    ReadIfd(tiff, (int)U32(tiff, entry + 8, le), le, tags);
                    continue;
                }
                if (type != 2)
                    continue;

                var data = new byte[valueCount];
                int source = valueCount <= 4 ? entry + 8 : (int)U32(tiff, entry + 8, le);
                Array.Copy(tiff, source, data, 0, valueCount);
                tags[tag] = Encoding.UTF8.GetString(data).TrimEnd('\0');
            }
        }

        public static int U16(byte[] data, int off, bool le)
        {
            return le ? data[off] | (data[off + 1] << 8) : (data[off] << 8) | data[off + 1];
        }

        public static uint U32(byte[] data, int off, bool le)
        {
            return le
                ? (uint)(data[off] | (data[off + 1] << 8) | (data[off + 2] << 16) | (data[off + 3] << 24))
                : (uint)((data[off] << 24) | (data[off + 1] << 16) | (data[off + 2] << 8) | data[off + 3]);
        }
    }
}
