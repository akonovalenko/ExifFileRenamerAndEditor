using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExifFileRenamer
{
    /// <summary>
    /// A single EXIF tag modification. Null data removes the tag from its IFD.
    /// </summary>
    internal class ExifTagChange
    {
        public int Tag;
        public short Type;      // 2 = ASCII, 4 = LONG (used for IFD pointers)
        public byte[] Data;     // raw value bytes (text incl. trailing NUL); null = remove tag

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

        public static ExifTagChange Undefined(int tag, byte[] data)
        {
            return new ExifTagChange { Tag = tag, Type = 7, Data = data };
        }
    }

    /// <summary>
    /// Rewrites the EXIF (APP1) segment of a JPEG file without re-encoding image data:
    /// only the metadata segment is replaced, compressed pixel data is copied byte for byte.
    /// </summary>
    internal static class JpegExifRewriter
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

        private static bool HasExifSignature(byte[] jpeg, int offset, int payloadLen)
        {
            if (payloadLen < ExifSignature.Length)
                return false;
            for (int i = 0; i < ExifSignature.Length; i++)
                if (jpeg[offset + i] != ExifSignature[i])
                    return false;
            return true;
        }

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

    /// <summary>
    /// Edits ASCII tags inside an EXIF TIFF block (IFD0 and the Exif sub-IFD).
    /// Existing value data is never moved: entries are patched in place when possible,
    /// otherwise the IFD table itself is relocated to the end of the block. This keeps
    /// absolute offsets used by untouched entries (maker notes, GPS, thumbnail) valid.
    /// </summary>
    internal class TiffExifEditor
    {
        private const int TAG_EXIF_IFD_POINTER = 0x8769;

        private readonly List<byte> _buf;
        private readonly bool _le;

        /// <summary>
        /// Applies changes to an existing TIFF block, or builds a minimal one when tiff is null.
        /// Returns null when the result would contain no EXIF data at all.
        /// </summary>
        public static byte[] Apply(byte[] tiff, List<ExifTagChange> ifd0Changes, List<ExifTagChange> exifIfdChanges)
        {
            return new TiffExifEditor(tiff).ApplyCore(ifd0Changes, exifIfdChanges);
        }

        private TiffExifEditor(byte[] tiff)
        {
            if (tiff == null)
            {
                _le = true;
                _buf = new List<byte> { 0x49, 0x49, 0x2A, 0x00, 0x00, 0x00, 0x00, 0x00 };
            }
            else
            {
                if (tiff.Length < 8)
                    throw new InvalidOperationException("EXIF TIFF header is truncated.");
                if (tiff[0] == 0x49 && tiff[1] == 0x49) _le = true;
                else if (tiff[0] == 0x4D && tiff[1] == 0x4D) _le = false;
                else throw new InvalidOperationException("Unknown EXIF byte order marker.");
                _buf = new List<byte>(tiff);
            }
        }

        private byte[] ApplyCore(List<ExifTagChange> ifd0Changes, List<ExifTagChange> exifIfdChanges)
        {
            int ifd0Off = (int)ReadU32(4);

            int oldExifIfdOff = 0;
            if (ifd0Off != 0)
            {
                int ptrEntry = FindEntry(ifd0Off, TAG_EXIF_IFD_POINTER);
                if (ptrEntry >= 0)
                    oldExifIfdOff = (int)ReadU32(ptrEntry + 8);
            }

            int newExifIfdOff = EditIfd(oldExifIfdOff, exifIfdChanges);

            var allIfd0Changes = new List<ExifTagChange>(ifd0Changes);
            if (newExifIfdOff != oldExifIfdOff)
            {
                allIfd0Changes.Add(new ExifTagChange
                {
                    Tag = TAG_EXIF_IFD_POINTER,
                    Type = 4,
                    Data = newExifIfdOff == 0 ? null : U32Bytes((uint)newExifIfdOff)
                });
            }

            int newIfd0Off = EditIfd(ifd0Off, allIfd0Changes);
            if (newIfd0Off == 0)
                return null;
            WriteU32(4, (uint)newIfd0Off);
            return _buf.ToArray();
        }

        /// <summary>
        /// Applies changes to one IFD. Returns the (possibly new) IFD offset,
        /// or 0 when the IFD does not exist / has no entries left.
        /// </summary>
        private int EditIfd(int ifdOff, List<ExifTagChange> changes)
        {
            if (ifdOff == 0)
            {
                var adds = changes.Where(c => c.Data != null).Cast<object>().ToList();
                return adds.Count == 0 ? 0 : WriteNewIfd(adds, 0);
            }

            int count = ReadU16(ifdOff);
            bool needRebuild = false;
            foreach (var change in changes)
            {
                bool exists = FindEntry(ifdOff, change.Tag) >= 0;
                if (exists != (change.Data != null))
                    needRebuild = true;
            }

            if (!needRebuild)
            {
                foreach (var change in changes.Where(c => c.Data != null))
                    PatchEntry(FindEntry(ifdOff, change.Tag), change);
                return ifdOff;
            }

            // Rebuild the IFD table at the end of the block. Untouched entries are
            // copied verbatim, so offsets they reference stay valid.
            var items = new List<object>();
            var replacedTags = new HashSet<int>();
            for (int i = 0; i < count; i++)
            {
                int entryOff = ifdOff + 2 + i * 12;
                int tag = ReadU16(entryOff);
                var change = changes.FirstOrDefault(c => c.Tag == tag);
                if (change == null)
                    items.Add(GetRaw(entryOff, 12));
                else if (change.Data != null && replacedTags.Add(tag))
                    items.Add(change);
                // change.Data == null → drop the entry
            }
            foreach (var change in changes)
            {
                if (change.Data != null && FindEntry(ifdOff, change.Tag) < 0 && replacedTags.Add(change.Tag))
                    items.Add(change);
            }

            if (items.Count == 0)
                return 0;
            int nextIfd = (int)ReadU32(ifdOff + 2 + count * 12);
            return WriteNewIfd(items, nextIfd);
        }

        /// <summary>
        /// Appends a new IFD table (entries sorted by tag, as the spec requires).
        /// Items are either raw 12-byte entries or ExifTagChange values to serialize.
        /// </summary>
        private int WriteNewIfd(List<object> items, int nextIfdOffset)
        {
            items.Sort((a, b) => ItemTag(a).CompareTo(ItemTag(b)));

            var externalOffsets = new Dictionary<ExifTagChange, int>();
            foreach (var item in items)
            {
                var change = item as ExifTagChange;
                if (change != null && change.Data.Length > 4)
                    externalOffsets[change] = AppendAligned(change.Data);
            }

            if ((_buf.Count & 1) == 1)
                _buf.Add(0);
            int ifdOff = _buf.Count;

            AppendU16((ushort)items.Count);
            foreach (var item in items)
            {
                var raw = item as byte[];
                if (raw != null)
                {
                    _buf.AddRange(raw);
                    continue;
                }
                var change = (ExifTagChange)item;
                AppendU16((ushort)change.Tag);
                AppendU16((ushort)change.Type);
                AppendU32((uint)ValueCount(change));
                if (change.Data.Length > 4)
                {
                    AppendU32((uint)externalOffsets[change]);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                        _buf.Add(i < change.Data.Length ? change.Data[i] : (byte)0);
                }
            }
            AppendU32((uint)nextIfdOffset);
            return ifdOff;
        }

        private void PatchEntry(int entryOff, ExifTagChange change)
        {
            int oldType = ReadU16(entryOff + 2);
            long oldByteLen = ReadU32(entryOff + 4) * (long)TypeSize(oldType);

            WriteU16(entryOff + 2, (ushort)change.Type);
            WriteU32(entryOff + 4, (uint)ValueCount(change));

            if (change.Data.Length <= 4)
            {
                for (int i = 0; i < 4; i++)
                    WriteByte(entryOff + 8 + i, i < change.Data.Length ? change.Data[i] : (byte)0);
            }
            else if (oldByteLen > 4 && oldByteLen >= change.Data.Length)
            {
                // new value fits into the old external storage — reuse it
                int off = (int)ReadU32(entryOff + 8);
                for (int i = 0; i < change.Data.Length; i++)
                    WriteByte(off + i, change.Data[i]);
            }
            else
            {
                WriteU32(entryOff + 8, (uint)AppendAligned(change.Data));
            }
        }

        private int FindEntry(int ifdOff, int tag)
        {
            if (ifdOff <= 0 || ifdOff + 2 > _buf.Count)
                throw new InvalidOperationException("EXIF IFD offset is out of range.");
            int count = ReadU16(ifdOff);
            for (int i = 0; i < count; i++)
            {
                int entryOff = ifdOff + 2 + i * 12;
                if (ReadU16(entryOff) == tag)
                    return entryOff;
            }
            return -1;
        }

        private int ItemTag(object item)
        {
            var raw = item as byte[];
            if (raw != null)
                return _le ? raw[0] | (raw[1] << 8) : (raw[0] << 8) | raw[1];
            return ((ExifTagChange)item).Tag;
        }

        private static int ValueCount(ExifTagChange change)
        {
            var size = TypeSize(change.Type);
            return size > 1 ? change.Data.Length / size : change.Data.Length;
        }

        private static int TypeSize(int type)
        {
            switch (type)
            {
                case 3: case 8: return 2;            // SHORT, SSHORT
                case 4: case 9: case 11: return 4;   // LONG, SLONG, FLOAT
                case 5: case 10: case 12: return 8;  // RATIONAL, SRATIONAL, DOUBLE
                default: return 1;                   // BYTE, ASCII, SBYTE, UNDEFINED
            }
        }

        #region Buffer helpers

        private byte[] GetRaw(int offset, int length)
        {
            CheckRange(offset, length);
            var result = new byte[length];
            for (int i = 0; i < length; i++)
                result[i] = _buf[offset + i];
            return result;
        }

        private int AppendAligned(byte[] data)
        {
            if ((_buf.Count & 1) == 1)
                _buf.Add(0);
            int off = _buf.Count;
            _buf.AddRange(data);
            return off;
        }

        private int ReadU16(int offset)
        {
            CheckRange(offset, 2);
            return _le
                ? _buf[offset] | (_buf[offset + 1] << 8)
                : (_buf[offset] << 8) | _buf[offset + 1];
        }

        private uint ReadU32(int offset)
        {
            CheckRange(offset, 4);
            return _le
                ? (uint)(_buf[offset] | (_buf[offset + 1] << 8) | (_buf[offset + 2] << 16) | (_buf[offset + 3] << 24))
                : (uint)((_buf[offset] << 24) | (_buf[offset + 1] << 16) | (_buf[offset + 2] << 8) | _buf[offset + 3]);
        }

        private void WriteByte(int offset, byte value)
        {
            CheckRange(offset, 1);
            _buf[offset] = value;
        }

        private void WriteU16(int offset, ushort value)
        {
            CheckRange(offset, 2);
            if (_le) { _buf[offset] = (byte)value; _buf[offset + 1] = (byte)(value >> 8); }
            else { _buf[offset] = (byte)(value >> 8); _buf[offset + 1] = (byte)value; }
        }

        private void WriteU32(int offset, uint value)
        {
            CheckRange(offset, 4);
            var bytes = U32Bytes(value);
            for (int i = 0; i < 4; i++)
                _buf[offset + i] = bytes[i];
        }

        private byte[] U32Bytes(uint value)
        {
            return _le
                ? new[] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) }
                : new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
        }

        private void AppendU16(ushort value)
        {
            if (_le) { _buf.Add((byte)value); _buf.Add((byte)(value >> 8)); }
            else { _buf.Add((byte)(value >> 8)); _buf.Add((byte)value); }
        }

        private void AppendU32(uint value)
        {
            _buf.AddRange(U32Bytes(value));
        }

        private void CheckRange(int offset, int length)
        {
            if (offset < 0 || offset + length > _buf.Count)
                throw new InvalidOperationException("EXIF structure is corrupt (offset out of range).");
        }

        #endregion
    }
}
