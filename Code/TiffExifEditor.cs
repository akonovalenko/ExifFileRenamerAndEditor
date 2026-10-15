using System;
using System.Collections.Generic;
using System.Linq;

namespace ExifFileRenamer
{
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

        /// <summary>
        /// Initializes a new instance of the TiffExifEditor class with the given TIFF block.   
        /// </summary>
        /// <param name="tiff">The TIFF block to edit.</param>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// Applies the requested changes to IFD0 and the Exif sub-IFD, returning the new TIFF block.
        /// </summary>
        /// <param name="ifd0Changes">The changes to apply to IFD0.</param>
        /// <param name="exifIfdChanges">The changes to apply to the Exif sub-IFD.</param>
        /// <returns>The new TIFF block, or null if the result would contain no EXIF data.</returns>
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

        /// <summary>
        /// Patches an existing IFD entry with new type and value data. 
        /// If the new value is larger than 4 bytes, it is stored in external 
        /// storage and the entry's offset is updated accordingly.
        /// </summary>
        /// <param name="entryOff">The offset of the entry to patch.</param>
        /// <param name="change">The change to apply.</param>
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

        /// <summary>
        /// Finds the offset of an IFD entry with the given tag, or returns -1 if not found.    
        /// </summary>
        /// <param name="ifdOff">The offset of the IFD.</param>
        /// <param name="tag">The tag to find.</param>
        /// <returns>The offset of the entry, or -1 if not found.</returns>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// Returns the tag number of an item, which can be either a raw 12-byte IFD entry or an ExifTagChange.
        /// </summary>
        /// <param name="item">The item for which to get the tag number.</param>
        /// <returns>The tag number.</returns>
        private int ItemTag(object item)
        {
            var raw = item as byte[];
            if (raw != null)
                return _le ? raw[0] | (raw[1] << 8) : (raw[0] << 8) | raw[1];
            return ((ExifTagChange)item).Tag;
        }

        /// <summary>
        /// Returns the number of values in the given ExifTagChange, based on its type and data length.
        /// </summary>
        /// <param name="change">The ExifTagChange for which to count values.</param>
        /// <returns>The number of values.</returns>
        private static int ValueCount(ExifTagChange change)
        {
            var size = TypeSize(change.Type);
            return size > 1 ? change.Data.Length / size : change.Data.Length;
        }

        /// <summary>
        /// Returns the size in bytes of a single value of the given TIFF type.
        /// </summary>
        /// <param name="type">The TIFF type.</param>
        /// <returns>The size in bytes.</returns>
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

        /// <summary>
        /// Returns a copy of the raw bytes from the buffer, for example to preserve an untouched IFD entry.
        /// </summary>
        /// <param name="offset">The offset from which to start copying.</param>
        /// <param name="length">The number of bytes to copy.</param>
        /// <returns>The array of copied bytes.</returns>
        private byte[] GetRaw(int offset, int length)
        {
            CheckRange(offset, length);
            var result = new byte[length];
            for (int i = 0; i < length; i++)
                result[i] = _buf[offset + i];
            return result;
        }

        /// <summary>
        /// Appends data to the buffer, aligning the start of the data to an even offset as required by the TIFF specification.
        /// </summary>
        /// <param name="data">The data to append.</param>
        /// <returns>The offset of the appended data.</returns>
        private int AppendAligned(byte[] data)
        {
            if ((_buf.Count & 1) == 1)
                _buf.Add(0);
            int off = _buf.Count;
            _buf.AddRange(data);
            return off;
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer from the buffer at the specified offset, taking into account the endianness of the TIFF data.
        /// </summary>
        /// <param name="offset">The offset from which to read.</param>
        /// <returns>The 16-bit unsigned integer.</returns>
        private int ReadU16(int offset)
        {
            CheckRange(offset, 2);
            return _le
                ? _buf[offset] | (_buf[offset + 1] << 8)
                : (_buf[offset] << 8) | _buf[offset + 1];
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer from the buffer at the specified offset, taking into account the endianness of the TIFF data.
        /// </summary>
        /// <param name="offset">The offset from which to read.</param>
        /// <returns>The 32-bit unsigned integer.</returns>
        private uint ReadU32(int offset)
        {
            CheckRange(offset, 4);
            return _le
                ? (uint)(_buf[offset] | (_buf[offset + 1] << 8) | (_buf[offset + 2] << 16) | (_buf[offset + 3] << 24))
                : (uint)((_buf[offset] << 24) | (_buf[offset + 1] << 16) | (_buf[offset + 2] << 8) | _buf[offset + 3]);
        }

        /// <summary>
        /// Writes a single byte to the buffer at the specified offset.
        /// </summary>
        /// <param name="offset">The offset at which to write.</param>
        /// <param name="value">The byte value to write.</param>
        private void WriteByte(int offset, byte value)
        {
            CheckRange(offset, 1);
            _buf[offset] = value;
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer to the buffer at the specified offset, taking into account the endianness of the TIFF data.
        /// </summary>
        /// <param name="offset">The offset at which to write.</param>
        /// <param name="value">The 16-bit unsigned integer to write.</param>
        private void WriteU16(int offset, ushort value)
        {
            CheckRange(offset, 2);
            if (_le) { _buf[offset] = (byte)value; _buf[offset + 1] = (byte)(value >> 8); }
            else { _buf[offset] = (byte)(value >> 8); _buf[offset + 1] = (byte)value; }
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer to the buffer at the specified offset, taking into account the endianness of the TIFF data.    
        /// </summary>
        /// <param name="offset">The offset at which to write.</param>
        /// <param name="value">The 32-bit unsigned integer to write.</param>
        private void WriteU32(int offset, uint value)
        {
            CheckRange(offset, 4);
            var bytes = U32Bytes(value);
            for (int i = 0; i < 4; i++)
                _buf[offset + i] = bytes[i];
        }

        /// <summary>
        /// Returns the byte array representation of a 32-bit unsigned integer, taking into account the endianness of the TIFF data.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer for which to get byte representation.</param>
        /// <returns>The byte array.</returns>
        private byte[] U32Bytes(uint value)
        {
            return _le
                ? new[] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) }
                : new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
        }

        /// <summary>
        /// Appends a 16-bit unsigned integer to the end of the buffer, taking into account the endianness of the TIFF data.    
        /// </summary>
        /// <param name="value">The 16-bit unsigned integer to append.</param>
        private void AppendU16(ushort value)
        {
            if (_le) { _buf.Add((byte)value); _buf.Add((byte)(value >> 8)); }
            else { _buf.Add((byte)(value >> 8)); _buf.Add((byte)value); }
        }

        /// <summary>
        /// Appends a 32-bit unsigned integer to the end of the buffer, taking into account the endianness of the TIFF data.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to append.</param>
        private void AppendU32(uint value)
        {
            _buf.AddRange(U32Bytes(value));
        }

        /// <summary>
        /// Checks if the specified range (offset and length) is within the bounds of the buffer. Throws an exception if the range is invalid.
        /// </summary>
        /// <param name="offset">The offset at which to check.</param>
        /// <param name="length">The length of the range to check.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private void CheckRange(int offset, int length)
        {
            if (offset < 0 || offset + length > _buf.Count)
                throw new InvalidOperationException("EXIF structure is corrupt (offset out of range).");
        }

        #endregion
    }
}
