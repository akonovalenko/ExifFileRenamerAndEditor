using ExifFileRenamer.Model;
using System;
using System.IO;

namespace ExifImageRenamer.Model
{
    /// <summary>
    /// Snapshot of the dialog values: survives the dialog disposal, so the
    /// background batch worker can safely build per-file EXIF values from it.
    /// </summary>
    internal class AddExifRequest
    {
        public AddExifDateSource DateSource;
        public DateTime FixedDate;
        public string Description;
        public string MakeName;
        public string ModelName;
        public string Artist;
        public string Copyright;
        public bool WriteFullTemplate;

        public ExifEditValues CreateValuesFor(FileInfo file)
        {
            DateTime? date = null;
            switch (DateSource)
            {
                case AddExifDateSource.CreationTime: date = file.CreationTime; break;
                case AddExifDateSource.LastWriteTime: date = file.LastWriteTime; break;
                case AddExifDateSource.Fixed: date = FixedDate; break;
            }
            return new ExifEditValues
            {
                OriginalDateTime = date,
                Description = NullIfEmpty(Description),
                MakeName = NullIfEmpty(MakeName),
                ModelName = NullIfEmpty(ModelName),
                Artist = NullIfEmpty(Artist),
                Copyright = NullIfEmpty(Copyright),
                WriteFullTemplate = WriteFullTemplate
            };
        }

        /// <summary>Empty field in the ADD dialog means "do not write" (null), not "remove tag" ("").</summary>
        private static string NullIfEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
